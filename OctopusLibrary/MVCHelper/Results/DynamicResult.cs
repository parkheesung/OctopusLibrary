using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OctopusLibrary.Results
{
    public class DynamicResult : FileResult
    {
        protected string TITLE;
        protected StringBuilder BODY;
        protected StringBuilder HEAD;
        protected StringBuilder FOOTER;
        protected StringBuilder SCRIPT;
        public bool LayoutWrite { get; set; }
        public bool IsMobile { get; set; }

        public DynamicResult()
            : base("text/html")
        {
            this.BODY = new StringBuilder();
            this.HEAD = new StringBuilder();
            this.FOOTER = new StringBuilder();
            this.SCRIPT = new StringBuilder();
            this.LayoutWrite = true;
            this.IsMobile = false;
        }

        public void SetTitle(string title = "")
        {
            this.TITLE = title;
        }

        public void SetBody(string html = "")
        {
            BODY.AppendLine(html);
        }

        public void SetBodyFormat(string html, params object[] param)
        {
            BODY.AppendFormat(html, param);
            BODY.AppendLine("");
        }

        public void SetHeader(string html = "")
        {
            BODY.AppendLine(html);
        }

        public void SetHeaderFormat(string html, params object[] param)
        {
            HEAD.AppendFormat(html, param);
            HEAD.AppendLine("");
        }

        public void SetFooter(string html = "")
        {
            FOOTER.AppendLine(html);
        }

        public void SetFooterFormat(string html, params object[] param)
        {
            FOOTER.AppendFormat(html, param);
            FOOTER.AppendLine("");
        }

        public void SetScript(string script = "")
        {
            SCRIPT.AppendLine(script);
        }

        public void SetScriptFormat(string script, params object[] param)
        {
            SCRIPT.AppendFormat(script, param);
            SCRIPT.AppendLine("");
        }

        protected virtual string SetHTML()
        {
            StringBuilder builder = new StringBuilder();
            if (LayoutWrite)
            {
                builder.AppendLine("<!DOCTYPE html>");
                builder.AppendLine("<html>");
                builder.AppendLine("<head>");
                builder.AppendLine("<meta charset=\"utf-8\" />");
                builder.AppendLine("<meta name=\"viewport\" content=\"width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0\">");
                builder.AppendLine("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />");
                builder.AppendLine("<meta http-equiv=\"Content-Language\" content=\"ko\" />");
                builder.AppendLine("<meta http-equiv=\"imagetoolbar\" content=\"no\" />");
                builder.AppendLine("<meta http-equiv=\"Cache-Control\" content=\"no-cache\" />");
                builder.AppendLine("<meta http-equiv=\"Pragma\" content=\"no-cache\" />");
                builder.AppendLine("<meta name=\"robots\" content=\"noindex,nofollow\" />");
                builder.AppendFormat("<title>{0}</title>{1}", TITLE, Environment.NewLine);
                builder.AppendLine(HEAD.ToString());
                builder.AppendLine("</head>");
                builder.AppendLine("<body>");
                builder.AppendLine(BODY.ToString());
                builder.AppendLine(FOOTER.ToString());
                if (!String.IsNullOrEmpty(SCRIPT.ToString()))
                {
                    builder.AppendLine("<script type=\"text/javascript\">");
                    builder.AppendLine(SCRIPT.ToString());
                    builder.AppendLine("</script>");
                }
                builder.AppendLine("</body>");
                builder.AppendLine("</html>");

            }
            else
            {
                builder.AppendLine(BODY.ToString());
            }
            return builder.ToString();
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            Stream outputStream = response.OutputStream;
            byte[] byteArray = Encoding.UTF8.GetBytes(SetHTML());
            response.OutputStream.Write(byteArray, 0, byteArray.GetLength(0));
        }
    }
}
