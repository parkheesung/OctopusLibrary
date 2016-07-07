using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OctopusLibrary.Utility
{
    public class MailHandler : IDisposable
    {
        protected string HostAddress { get; set; }
        protected int HostPort { get; set; }
        protected bool UseDefaultCredentials { get; set; }
        protected SmtpClient Client { get; set; }
        protected MailMessage MailMessage { get; set; }

        protected NetworkCredential networtCredential { get; set; }
        protected Encoding BodyEncoding { get; set; }
        protected Encoding SubjectEncoding { get; set; }
        protected List<MailAddress> ToMailAddress { get; set; }

        public bool EnableSsl { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public MailAddress FromMail { get; set; }

        public MailHandler()
        {
            this.HostAddress = String.Empty;
            this.HostPort = -1;
            this.UseDefaultCredentials = true;

            this.EnableSsl = true;
            this.IsBodyHtml = true;

            this.BodyEncoding = Encoding.UTF8;
            this.SubjectEncoding = Encoding.UTF8;
            this.ToMailAddress = new List<MailAddress>();
        }

        /// <summary>
        /// 실제 발송자 정보를 설정합니다.
        /// </summary>
        /// <param name="usermail">메일주소</param>
        /// <param name="password">비밀번호</param>
        public void Initialize(string usermail, string password)
        {
            this.networtCredential = new NetworkCredential(usermail, password);
        }

        /// <summary>
        /// 발송된 메일에 표시될 발송자 정보를 설정합니다.
        /// </summary>
        /// <param name="usermail">메일주소</param>
        /// <param name="Title">메일 제목</param>
        public void FromMailSet(string usermail, string Title)
        {
            this.FromMail = new MailAddress(usermail, Title);
        }

        /// <summary>
        /// 수신자 메일목록에 메일을 추가합니다.
        /// </summary>
        /// <param name="mailAddress">메일주소</param>
        public virtual void AddMail(string mailAddress)
        {
            this.ToMailAddress.Add(new MailAddress(mailAddress));
        }

        /// <summary>
        /// 이메일 내용 작성
        /// </summary>
        /// <param name="skinPath">기본 HTML이 위치한 경로</param>
        /// <param name="replaceNameValue">치환할 문자열 목록</param>
        /// <returns></returns>
        public static string CraeteMessage(string skinPath, NameValueCollection replaceNameValue)
        {
            return CraeteMessage(skinPath, replaceNameValue, Encoding.Default);
        }

        /// <summary>
        /// 이메일 내용 작성
        /// </summary>
        /// <param name="skinPath">기본 HTML이 위치한 경로</param>
        /// <param name="replaceNameValue">치환할 문자열 목록</param>
        /// <param name="readEncoding">Encoding 설정</param>
        /// <returns></returns>
        public static string CraeteMessage(string skinPath, NameValueCollection replaceNameValue, Encoding readEncoding)
        {
            StringBuilder Result = new StringBuilder();

            if (File.Exists(skinPath))
            {
                Result.Append(File.ReadAllText(skinPath, readEncoding));
                foreach (string name in replaceNameValue.AllKeys)
                {
                    Result.Replace(name, replaceNameValue[name]);
                }
            }
            return Result.ToString();
        }

        /// <summary>
        /// 메일발송
        /// </summary>
        public void Send()
        {
            this.Send(this.FromMail, this.ToMailAddress, this.Subject, this.Message);
        }

        /// <summary>
        /// 메일발송
        /// </summary>
        /// <param name="toMailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="sendMessage"></param>
        public void Send(MailAddress toMailAddress, string subject, string sendMessage)
        {
            this.Send(this.FromMail, toMailAddress, subject, sendMessage);
        }

        /// <summary>
        /// 메일발송
        /// </summary>
        /// <param name="fromMailAddress"></param>
        /// <param name="toMailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="sendMessage"></param>
        public void Send(MailAddress fromMailAddress, MailAddress toMailAddress, string subject, string sendMessage)
        {
            List<MailAddress> ToList = new List<MailAddress>();
            ToList.Add(toMailAddress);
            this.Send(fromMailAddress, ToList, subject, sendMessage);
        }

        /// <summary>
        /// 메일발송
        /// </summary>
        /// <param name="toMailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="sendMessage"></param>
        public void Send(List<MailAddress> toMailAddress, string subject, string sendMessage)
        {
            this.Send(this.FromMail, toMailAddress, subject, sendMessage);
        }

        /// <summary>
        /// 메일발송
        /// </summary>
        /// <param name="fromMailAddress"></param>
        /// <param name="toMailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="sendMessage"></param>
        public void Send(MailAddress fromMailAddress, List<MailAddress> toMailAddress, string subject, string sendMessage)
        {
            if (this.Client == null)
            {
                this.Client = new SmtpClient();
            }
            this.Client.Host = this.HostAddress;
            this.Client.Port = this.HostPort;
            this.Client.UseDefaultCredentials = this.UseDefaultCredentials;
            this.Client.EnableSsl = this.EnableSsl;
            this.Client.Credentials = this.networtCredential;
            this.MailMessage = new MailMessage();
            this.MailMessage.From = fromMailAddress;
            foreach (MailAddress toAddress in toMailAddress)
            {
                this.MailMessage.To.Add(toAddress);
            }
            this.MailMessage.Body = sendMessage;
            this.MailMessage.IsBodyHtml = this.IsBodyHtml;
            this.MailMessage.BodyEncoding = this.BodyEncoding;
            this.MailMessage.Subject = subject;
            this.MailMessage.SubjectEncoding = this.SubjectEncoding;

            this.Client.Send(this.MailMessage);
            this.Client.Dispose();
        }

        public void Dispose()
        {
            try
            {
                if (this.Client != null)
                {
                    this.Client.Dispose();
                    this.Client = null;
                }
                if (this.MailMessage != null)
                {
                    this.MailMessage.Dispose();
                    this.MailMessage = null;
                }
            }
            catch { }
        }
    }
}
