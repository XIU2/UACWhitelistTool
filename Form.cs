using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Text;
using IWshRuntimeLibrary;
using UAC白名单小工具.Properties;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Drawing;

namespace UAC白名单小工具
{
    public partial class Form : System.Windows.Forms.Form
    {
        readonly string[] args;

        public FileDropAdmin_cs.FileDropAdmin FileDroper;
        public Form(string[] args)
        {
            InitializeComponent();
            this.args = args;
        }
        string J_VerInfo;// 软件版本号
        // 程序创建前
        private void Form1_Load(object sender, EventArgs e)
        {
            if (args.Length > 0)
            {
                //Debug.Print(args[0]);
                Handling_File_Drop(args[0]);
            }
            FileDroper = new FileDropAdmin_cs.FileDropAdmin(this);
            FileVersionInfo VerInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            J_VerInfo = VerInfo.FileVersion;
            J_VerInfo = J_VerInfo.Replace(".0","");
            this.Text = "UAC白名单小工具 v" + J_VerInfo;
            NotKey();
        }
        // 有文件拖放进来了
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }
        // 处理拖放进来的文件
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            Handling_File_Drop(((string[])e.Data.GetData(typeof(string[])))[0]);
        }
        // 处理拖放进来的文件
        private void Handling_File_Drop(string Drag_File_PATH)
        {
            Debug.Print(Drag_File_PATH);
            if (Path.GetExtension(Drag_File_PATH).ToLower() == ".exe" || Path.GetExtension(Drag_File_PATH).ToLower()  == ".bat")
            {
                if (System.IO.File.Exists(Drag_File_PATH))
                {
                    TextBox_程序位置.Text = Drag_File_PATH;
                    TextBox_程序名称.Text = Path.GetFileNameWithoutExtension(TextBox_程序位置.Text);
                    TextBox_启动参数.Text = "";
                    TextBox_起始位置.Text = "";
                    TextBox_启动参数.SendToBack();
                    TextBox_起始位置.SendToBack();
                    TextBox_程序位置.BringToFront();
                    TextBox_程序名称.BringToFront();
                }
                else
                {
                    MessageBox.Show("文件不存在！请检查！" + Environment.NewLine + Drag_File_PATH, "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (Path.GetExtension(Drag_File_PATH).ToLower() == ".lnk")
            {
                if (System.IO.File.Exists(Drag_File_PATH))
                {
                    WshShell shell = new WshShell();
                    IWshShortcut Shortcut = (IWshShortcut)shell.CreateShortcut(Drag_File_PATH);
                    if (System.IO.File.Exists(Shortcut.TargetPath))
                    {
                        TextBox_程序位置.Text = Shortcut.TargetPath;
                        TextBox_程序名称.Text = Path.GetFileNameWithoutExtension(TextBox_程序位置.Text);
                        TextBox_启动参数.Text = Shortcut.Arguments;
                        TextBox_起始位置.Text = Shortcut.WorkingDirectory;
                        TextBox_程序位置.BringToFront();
                        TextBox_程序名称.BringToFront();
                        if(TextBox_启动参数.Text != "")
                        {
                            TextBox_启动参数.BringToFront();
                        }
                        else
                        {
                            TextBox_启动参数.SendToBack();
                        }
                        if (TextBox_起始位置.Text != "")
                        {
                            TextBox_起始位置.BringToFront();
                        }
                        else
                        {
                            TextBox_起始位置.SendToBack();
                        }
                    }
                    else
                    {
                        MessageBox.Show("B文件不存在！请检查！" + Environment.NewLine + Shortcut.TargetPath, "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("A文件不存在！请检查！" + Environment.NewLine + Drag_File_PATH, "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("只支持拖入 .exe .bat .lnk 格式的文件！", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // 监视输入框
        private void TextBox_程序名称_TextChanged(object sender, EventArgs e)
        {
            TextBox_程序名称.Text = Regex.Replace(TextBox_程序名称.Text, @"[^\u4e00-\u9fa5_a-zA-Z0-9\.]", "");
            //Debug.Print(TextBox_程序名称.Text);
            if (TextBox_程序名称.Text != "")
            {
                if (TextBox_程序位置.Text != "")
                {
                    Button_添加.Enabled = true;
                }
            }
            else
            {
                Button_添加.Enabled = false;
            }
        }
        // 监视输入框
        private void TextBox_程序位置_TextChanged(object sender, EventArgs e)
        {
            if (TextBox_程序位置.Text != "")
            {
                if (TextBox_程序名称.Text != "")
                {
                    Button_添加.Enabled = true;
                }
            }
            else
            {
                Button_添加.Enabled = false;
            }
        }
        // 用对话框选择文件
        private void Button_浏览_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(openFileDialog1.FileName).ToLower() == ".exe" || Path.GetExtension(openFileDialog1.FileName).ToLower()  == ".bat")
                {
                    if (System.IO.File.Exists(openFileDialog1.FileName))
                    {
                        TextBox_程序位置.Text = openFileDialog1.FileName;
                        TextBox_程序名称.Text = Path.GetFileNameWithoutExtension(TextBox_程序位置.Text);
                        TextBox_启动参数.Text = "";
                        TextBox_起始位置.Text = "";
                        TextBox_启动参数.SendToBack();
                        TextBox_起始位置.SendToBack();
                        TextBox_程序位置.BringToFront();
                        TextBox_程序名称.BringToFront();
                    }
                    else
                    {
                        MessageBox.Show("文件不存在！请检查！" + Environment.NewLine + openFileDialog1.FileName, "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (Path.GetExtension(openFileDialog1.FileName).ToLower() == ".lnk")
                {
                    if (System.IO.File.Exists(openFileDialog1.FileName))
                    {
                        WshShell shell = new WshShell();
                        IWshShortcut Shortcut = (IWshShortcut)shell.CreateShortcut(openFileDialog1.FileName);
                        if (System.IO.File.Exists(Shortcut.TargetPath))
                        {
                            TextBox_程序位置.Text = Shortcut.TargetPath;
                            TextBox_程序名称.Text = Path.GetFileNameWithoutExtension(TextBox_程序位置.Text);
                            TextBox_启动参数.Text = Shortcut.Arguments;
                            TextBox_起始位置.Text = Shortcut.WorkingDirectory;
                            TextBox_程序位置.BringToFront();
                            TextBox_程序名称.BringToFront();
                            if (TextBox_启动参数.Text != "")
                            {
                                TextBox_启动参数.BringToFront();
                            }
                            else
                            {
                                TextBox_启动参数.SendToBack();
                            }
                            if (TextBox_起始位置.Text != "")
                            {
                                TextBox_起始位置.BringToFront();
                            }
                            else
                            {
                                TextBox_起始位置.SendToBack();
                            }
                        }
                        else
                        {
                            MessageBox.Show("文件不存在！请检查！" + Environment.NewLine + Shortcut.TargetPath, "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("文件不存在！请检查！" + Environment.NewLine + openFileDialog1.FileName, "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("只支持拖入 .exe .bat .lnk 格式的文件！", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // 添加、写入
        private void Button_添加_Click(object sender, EventArgs e)
        {
            // 先判断一下程序名称前缀是否有 noUAC.
            // if (TextBox_程序名称.Text.Length >= 6)
            // {
            //     if (TextBox_程序名称.Text.Substring(0, 6) != "noUAC.")
            //     {
            //         TextBox_程序名称.Text = "noUAC." + TextBox_程序名称.Text;
            //     }
            // }
            // else
            // {
            //     TextBox_程序名称.Text = "noUAC." + TextBox_程序名称.Text;
            // }
            // 文件夹名称
            string FolderName = "noUAC\\";
            string TempFileName = Path.GetDirectoryName(Application.ExecutablePath) + @"\" + TextBox_程序名称.Text + ".xml";
            string XML_Text = Resources.XML_前 + Environment.NewLine + Resources.XML_程序位置_前 + TextBox_程序位置.Text + Resources.XML_程序位置_后;
            if (TextBox_启动参数.Text != "")
            {
                XML_Text = XML_Text + Environment.NewLine + Resources.XML_启动参数_前 + TextBox_启动参数.Text + Resources.XML_启动参数_后;
            }
            if (TextBox_起始位置.Text != "")
            {
                XML_Text = XML_Text + Environment.NewLine + Resources.XML_起始位置_前 + TextBox_起始位置.Text + Resources.XML_起始位置_后;
            }
            XML_Text = XML_Text + Environment.NewLine + Resources.XML_后;
            System.IO.File.WriteAllText(TempFileName, XML_Text, Encoding.Unicode);
            ProcessStartInfo Schtasks = new ProcessStartInfo
            {
                FileName = "schtasks.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                // Arguments = "/create " + "/tn " + '"' + TextBox_程序名称.Text + '"' + " /xml " + '"' + @TempFileName + '"'
                Arguments = "/create " + "/tn " + '"' + FolderName + TextBox_程序名称.Text + '"' + " /xml " + '"' + @TempFileName + '"'
            };
            //Debug.Print("/create " + "/tn " + '"' + TextBox_程序名称.Text + '"' + " /xml " + '"' + @TempFileName + '"');
            //Schtasks.Verb = "runas";
            Process.Start(Schtasks);
            Create_Shortcut(FolderName);
            System.Threading.Thread.Sleep(200);
            System.IO.File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + @"\" + TextBox_程序名称.Text + ".xml");
            MessageBox.Show("UAC白名单添加成功！" + Environment.NewLine + Environment.NewLine + "快捷方式位于桌面：" + Environment.NewLine + System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + @"\" + TextBox_程序名称.Text + ".lnk" + Environment.NewLine + "注意：只有通过该快捷方式运行才不会提示 UAC，快捷方式可复制、移动、重命名。", "信息：",MessageBoxButtons.OK);
        }
        // 创建快捷方式
        public void Create_Shortcut(string FolderName)
        {
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + @"\" + TextBox_程序名称.Text + ".lnk");
            //Debug.Print(Path.GetDirectoryName(Application.ExecutablePath) + @"\" + TextBox_程序名称.Text + ".lnk");
            shortcut.TargetPath = "schtasks.exe";
            shortcut.Arguments = "/run " + "/tn " + '"' + FolderName + TextBox_程序名称.Text + '"';
            shortcut.IconLocation = TextBox_程序位置.Text + ", 0";
            shortcut.WindowStyle = 7;
            shortcut.Save();
        }

        private void Button_打开_Click(object sender, EventArgs e)
        {
            Process.Start("taskschd.msc", "/s");
        }

        private void CheckBox_添加到右键菜单_CheckedChanged(object sender, EventArgs e)
        {
            //Debug.Print(checkBox_添加到右键菜单.Checked.ToString());
            if (checkBox_添加到右键菜单.Checked == true)
            {
                AddKey();
            }
            else
            {
                DelKey();
            }
            
        }
        private void AddKey()
        {
            if (Registry.GetValue(@"HKEY_CLASSES_ROOT\exefile\shell\添加到 UAC 白名单\command\", "", null) == null)
            {
                RegistryKey Key1 = Registry.ClassesRoot.CreateSubKey(@"exefile\shell\添加到 UAC 白名单");
                RegistryKey Key2 = Registry.ClassesRoot.CreateSubKey(@"exefile\shell\添加到 UAC 白名单\command");
                Key1.SetValue("Icon", '"' + Application.ExecutablePath + '"');
                Key2.SetValue("", '"'+ Application.ExecutablePath + '"' + " " + '"' + "%1" + '"');
                
            }
        }
        private void DelKey()
        {
            if (Registry.GetValue(@"HKEY_CLASSES_ROOT\exefile\shell\添加到 UAC 白名单\command\", "", null) != null)
            {
                Registry.ClassesRoot.DeleteSubKeyTree(@"exefile\shell\添加到 UAC 白名单");
            }
        }
        private void NotKey()
        {
            //RegistryKey Key = Registry.ClassesRoot;
            if (Registry.GetValue(@"HKEY_CLASSES_ROOT\exefile\shell\添加到 UAC 白名单\command\", "", null) == null)
            {
                checkBox_添加到右键菜单.Checked = false;
            }
            else
            {
                checkBox_添加到右键菜单.Checked = true;
            }
            //Debug.Print(Reg.GetValue("").ToString());

        }
        // 切换焦点为输入框
        private void Label_程序位置_MouseClick(object sender, MouseEventArgs e)
        {
            TextBox_程序位置.Focus();
        }

        private void Label_程序名称_MouseClick(object sender, MouseEventArgs e)
        {
            TextBox_程序名称.Focus();
        }

        private void Label_启动参数_MouseClick(object sender, MouseEventArgs e)
        {
            TextBox_启动参数.Focus();
        }

        private void Label_起始位置_MouseClick(object sender, MouseEventArgs e)
        {
            TextBox_起始位置.Focus();
        }
        // 置顶输入框并修改背景颜色
        private void TextBox_程序位置_Enter(object sender, EventArgs e)
        {
            TextBox_程序位置.BringToFront();
            TextBox_程序位置.BackColor = Color.Gainsboro;
            Label_程序位置.BackColor = Color.Gainsboro;
        }

        private void TextBox_程序位置_Leave(object sender, EventArgs e)
        {
            if (TextBox_程序位置.Text == "")
                TextBox_程序位置.SendToBack();
            TextBox_程序位置.BackColor = Color.WhiteSmoke;
            Label_程序位置.BackColor = Color.WhiteSmoke;
        }

        private void TextBox_程序名称_Enter(object sender, EventArgs e)
        {
            TextBox_程序名称.BringToFront();
            TextBox_程序名称.BackColor = Color.Gainsboro;
            Label_程序名称.BackColor = Color.Gainsboro;
        }

        private void TextBox_程序名称_Leave(object sender, EventArgs e)
        {
            if (TextBox_程序名称.Text == "")
                TextBox_程序名称.SendToBack();
            TextBox_程序名称.BackColor = Color.WhiteSmoke;
            Label_程序名称.BackColor = Color.WhiteSmoke;
        }

        private void TextBox_启动参数_Enter(object sender, EventArgs e)
        {
            TextBox_启动参数.BringToFront();
            TextBox_启动参数.BackColor = Color.Gainsboro;
            Label_启动参数.BackColor = Color.Gainsboro;
        }

        private void TextBox_启动参数_Leave(object sender, EventArgs e)
        {
            if (TextBox_启动参数.Text == "")
                TextBox_启动参数.SendToBack();
            TextBox_启动参数.BackColor = Color.WhiteSmoke;
            Label_启动参数.BackColor = Color.WhiteSmoke;
        }

        private void TextBox_起始位置_Enter(object sender, EventArgs e)
        {
            TextBox_起始位置.BringToFront();
            TextBox_起始位置.BackColor = Color.Gainsboro;
            Label_起始位置.BackColor = Color.Gainsboro;
        }

        private void TextBox_起始位置_Leave(object sender, EventArgs e)
        {
            if (TextBox_起始位置.Text == "")
                TextBox_起始位置.SendToBack();
            TextBox_起始位置.BackColor = Color.WhiteSmoke;
            Label_起始位置.BackColor = Color.WhiteSmoke;
        }

        private void Button_添加_EnabledChanged(object sender, EventArgs e)
        {
            if (Button_添加.Enabled == true)
            {
                Button_添加.BackColor = Color.MediumSeaGreen;
            }
            else
            {
                Button_添加.BackColor = SystemColors.ButtonShadow;
            }
        }
    }
}
