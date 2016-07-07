using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OctopusLibrary.Results
{
    public class CookieRemoveResult : FileResult
    {
        private StringBuilder builder;
        public CookieRemoveResult()
            : base("text/html")
        {
            this.builder = new StringBuilder();
        }

        public void RemoveCookie(string key, string ReLoadURL = "")
        {
            this.builder.AppendLine("<!DOCTYPE html>");
            this.builder.AppendLine("<html>");
            this.builder.AppendLine("<head>");
            this.builder.AppendLine("<meta charset=\"utf-8\" />");
            this.builder.AppendLine("</head>");
            this.builder.AppendLine("<body>");
            this.builder.AppendLine("<script type=\"text/javascript\">");
            this.builder.AppendLine("var expireDate = new Date();");
            this.builder.AppendLine("expireDate.setDate(expireDate.getDate() - 1);");
            this.builder.AppendFormat("document.cookie = \"{0} = \" + \"; expires=\" + expireDate.toGMTString() + \"; path=/\";", key);
            if (!String.IsNullOrEmpty(ReLoadURL))
            {
                this.builder.Append(Environment.NewLine);
                this.builder.AppendFormat("location.href='{0}';", ReLoadURL);
            }
            this.builder.AppendLine("</script>");
            this.builder.AppendLine("</body>");
            this.builder.AppendLine("</html>");
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            Stream outputStream = response.OutputStream;
            byte[] byteArray = Encoding.UTF8.GetBytes(this.builder.ToString());
            response.OutputStream.Write(byteArray, 0, byteArray.GetLength(0));
        }
    }
}
