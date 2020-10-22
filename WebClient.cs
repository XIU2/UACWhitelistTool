using System;
using System.IO;
using System.Net;

namespace WebClient_cs
{
    public class GetHTTP
    {
        /// <summary>
        /// 获取HTTP
        /// </summary>
        /// <param name="URL">URL</param>
        /// <param name="Type">提交类型，默认：true(GET)，false为POST</param>
        /// <param name="PostDate">POST 数据</param>
        /// <param name="TimeOut">超时时间，默认：30000 毫秒，单位：毫秒</param>
        /// <param name="ReadWriteTimeout">GetResponse 和 GetRequestStream 方法的超时时间，默认：300000 毫秒，单位：毫秒</param>
        /// <param name="CookieContainer">Cookie，默认：null</param>
        /// <param name="UserAgent">UserAgent，默认：Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36</param>
        /// <param name="Referer">Referer，默认：null</param>
        /// <param name="Encoding">编码，默认：utf-8</param>
        /// <param name="AllowAutoRedirect">是否重定向，默认：true</param>
        /// <returns>成功返回网页内容，失败返回 null</returns>
        public static string Get_HTTP(string URL, bool Type = true, string PostDate = null, int TimeOut = 30000, int ReadWriteTimeout = 300000, CookieContainer CookieContainer = null, string UserAgent = null, string Referer = null, System.Text.Encoding Encoding = null, bool AllowAutoRedirect = true)
        {
            NewWebClient myWebClient = new NewWebClient();
            if (TimeOut > 100)
            {
                myWebClient.TimeOut = TimeOut;
            }
            if (ReadWriteTimeout > 100)
            {
                myWebClient.ReadWriteTimeout = ReadWriteTimeout;
            }
            myWebClient.CookieContainer = CookieContainer;
            if (UserAgent != null)
            {
                myWebClient.UserAgent = UserAgent;
            }
            myWebClient.Referer = Referer;
            myWebClient.AllowAutoRedirect = AllowAutoRedirect;
            if (Encoding == null)
            {
                myWebClient.Encoding = System.Text.Encoding.UTF8;
            }
            if (Type == false)
            {
                if (PostDate != null)
                {
                    return myWebClient.UploadString(URL, PostDate);
                }
                return null;
            }
            else
            {
                Stream myStream = myWebClient.OpenRead(URL);
                StreamReader sr = new StreamReader(myStream);
                string strHTML = sr.ReadToEnd();
                myStream.Close();
                return strHTML;
            }
        }
    }
    /// <summary>
    /// WebClient的扩展
    /// </summary>
    public class NewWebClient : WebClient
    {
        /// <summary>是否重定向 默认为true</summary>
        public bool AllowAutoRedirect { get; set; }

        /// <summary>
        ///获取或设置 Referer HTTP 标头的值。默认值为 null。
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// 获取或设置 Content-type HTTP 标头的值。 默认: application/x-www-form-urlencoded
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 获取或设置 User-agent HTTP 标头的值。默认: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 获取或设置 GetResponse 和 GetRequestStream 方法的超时值（以毫秒为单位）。 默认: 30,000 毫秒（30 秒）。
        /// </summary>
        public int TimeOut { get; set; }

        /// <summary>
        /// 获取或设置写入或读取流时的超时（以毫秒为单位）。默认: 300,000 毫秒（5 分钟）
        /// </summary>
        public int ReadWriteTimeout { get; set; }

        /// <summary>获取或设置请求相关联的Cookie</summary>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>创建一个新的 WebClient 实例。</summary>
        public NewWebClient()
        {
            this.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36";
            this.ContentType = "application/x-www-form-urlencoded";
            this.CookieContainer = new CookieContainer();
            this.AllowAutoRedirect = true;
            this.TimeOut = 30 * 1000;
            this.ReadWriteTimeout = 300 * 1000;
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            if (this.Referer != null) request.Referer = this.Referer;
            if (this.ContentType != null) request.ContentType = this.ContentType;
            if (this.UserAgent != null) request.UserAgent = this.UserAgent;
            request.AllowAutoRedirect = this.AllowAutoRedirect;
            request.CookieContainer = this.CookieContainer; //这句很关键，有了他可以保存返回的Cookie
            request.Timeout = this.TimeOut;
            request.ReadWriteTimeout = this.ReadWriteTimeout;
            return request;
        }
    }
}
