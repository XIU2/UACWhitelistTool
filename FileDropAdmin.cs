using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FileDropAdmin_cs
{
    // 解决管理员权限下无法拖放文件的问题
    public sealed class FileDropAdmin : IMessageFilter, IDisposable
    {

        #region native members

        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint message, ChangeFilterAction action, in ChangeFilterStruct pChangeFilterStruct);

        [DllImport("shell32.dll", SetLastError = false, CallingConvention = CallingConvention.Winapi)]
        private static extern void DragAcceptFiles(IntPtr hWnd, bool fAccept);

        [DllImport("shell32.dll", SetLastError = false, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
        private static extern uint DragQueryFile(IntPtr hWnd, uint iFile, StringBuilder lpszFile, int cch);

        [DllImport("shell32.dll", SetLastError = false, CallingConvention = CallingConvention.Winapi)]
        private static extern void DragFinish(IntPtr hDrop);

        [StructLayout(LayoutKind.Sequential)]
        private struct ChangeFilterStruct
        {
            public uint CbSize;
            public ChangeFilterStatu ExtStatus;
        }

        private enum ChangeFilterAction : uint
        {
            MSGFLT_RESET,
            MSGFLT_ALLOW,
            MSGFLT_DISALLOW
        }

        private enum ChangeFilterStatu : uint
        {
            MSGFLTINFO_NONE,
            MSGFLTINFO_ALREADYALLOWED_FORWND,
            MSGFLTINFO_ALREADYDISALLOWED_FORWND,
            MSGFLTINFO_ALLOWED_HIGHER
        }

        private const uint WM_COPYGLOBALDATA = 0x0049;
        private const uint WM_COPYDATA = 0x004A;
        private const uint WM_DROPFILES = 0x0233;

        #endregion


        private const uint GetIndexCount = 0xFFFFFFFF;

        private Control _ContainerControl;

        private readonly bool _DisposeControl;

        public Control ContainerControl { get; }

        public FileDropAdmin(Control containerControl) : this(containerControl, false) { }

        public FileDropAdmin(Control containerControl, bool releaseControl)
        {
            _ContainerControl = containerControl ?? throw new ArgumentNullException("control", "control is null.");

            if (containerControl.IsDisposed)
            {
                throw new ObjectDisposedException("control");
            }

            _DisposeControl = releaseControl;

            var status = new ChangeFilterStruct() { CbSize = 8 };

            if (!ChangeWindowMessageFilterEx(containerControl.Handle, WM_DROPFILES, ChangeFilterAction.MSGFLT_ALLOW, in status)) throw new Win32Exception(Marshal.GetLastWin32Error());
            if (!ChangeWindowMessageFilterEx(containerControl.Handle, WM_COPYGLOBALDATA, ChangeFilterAction.MSGFLT_ALLOW, in status)) throw new Win32Exception(Marshal.GetLastWin32Error());
            if (!ChangeWindowMessageFilterEx(containerControl.Handle, WM_COPYDATA, ChangeFilterAction.MSGFLT_ALLOW, in status)) throw new Win32Exception(Marshal.GetLastWin32Error());
            DragAcceptFiles(containerControl.Handle, true); // 允许控件接收拖放消息

            Application.AddMessageFilter(this);
        }

        public bool PreFilterMessage(ref Message m)
        {
            /*if (m.Msg != 512 && m.Msg != 160)
            {
                Debug.Print(m.Msg.ToString());
            }*/
            if (_ContainerControl == null || _ContainerControl.IsDisposed)
            {
                return false;
            }
            if (_ContainerControl.AllowDrop)
            {
                return _ContainerControl.AllowDrop = false;
            }
            if (m.Msg == WM_DROPFILES)
            {
                var handle = m.WParam;

                var fileCount = DragQueryFile(handle, GetIndexCount, null, 0); // 获取拖放进来的文件数量
                //Debug.Print(fileCount.ToString());
                var fileNames = new string[fileCount]; // 创建数组
                var sb = new StringBuilder(262); // 创建缓冲区
                var charLength = sb.Capacity;
                for (uint i = 0; i < fileCount; i++) // 循环添加到数组中
                {
                    if (DragQueryFile(handle, i, sb, charLength) > 0) // 判断是否为空
                    {
                        fileNames[i] = sb.ToString(); // 获取拖放进来的文件名
                        //Debug.Print(fileNames[i]);
                    }
                }
                DragFinish(handle); // 拖放结束，释放缓冲区
                _ContainerControl.AllowDrop = true; // 开启控件的允许拖放选项
                _ContainerControl.DoDragDrop(fileNames, DragDropEffects.All); // 执行拖放操作
                _ContainerControl.AllowDrop = false; // 关闭控件的允许拖放选项
                return true;
            }
            return false;
        }

        public void Dispose() // 释放
        {
            if (_ContainerControl == null)
            {
                if (_DisposeControl && !_ContainerControl.IsDisposed) _ContainerControl.Dispose();
                Application.RemoveMessageFilter(this);
                _ContainerControl = null;
            }
        }
    }
}
