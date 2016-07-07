using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OctopusLibrary.Results
{
    public class StreamResult : FileResult
    {
        private StringBuilder builder = new StringBuilder();

        public StreamResult()
            : base("text/event-stream")
        {
            this.builder = new StringBuilder();
        }

        public void SetBody(string html = "")
        {
            this.builder.Append(html);
            this.builder.Append(Environment.NewLine);
        }

        public void SetBodyFormat(string html, params object[] param)
        {
            this.builder.AppendFormat(html, param);
            this.builder.Append(Environment.NewLine);
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            Stream outputStream = response.OutputStream;
            byte[] byteArray = Encoding.UTF8.GetBytes(builder.ToString());
            response.OutputStream.Write(byteArray, 0, byteArray.GetLength(0));
        }
    }
}
