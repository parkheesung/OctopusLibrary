using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OctopusLibrary.Results
{
    public class ExcelResult : FileResult
    {
        public string Title { get; set; }
        private StringBuilder builder { get; set; }
        public ExcelResult() : base("application/ms-excel")
        {
            this.Title = String.Empty;
            this.builder = new StringBuilder();
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            Stream outputStream = response.OutputStream;
            byte[] byteArray = Encoding.UTF8.GetBytes(builder.ToString());
            response.OutputStream.Write(byteArray, 0, byteArray.GetLength(0));

            string FileName = "ExcelOutput_" + DateTime.Now.Date.ToString().Substring(0, 10).Replace("-", "") + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            response.AddHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
        }

        public void Create<T>(List<T> Data)
        {
            builder.Clear();
            builder.AppendLine("<html>");
            builder.AppendLine("<head>");
            builder.AppendLine("<meta charset=\"utf-8\" />");
            builder.AppendLine("<meta http-equiv=Content-Type content=''application/ms-excel; charset=utf-8''>");
            builder.AppendLine(String.Format("<title>{0}</title>", Title));
            builder.AppendLine("</head>");
            builder.AppendLine("<body>");
            builder.AppendLine("<div>");
            builder.AppendLine(String.Format("<h3>{0}</h3>", Title));
            builder.AppendLine("<table border='1'>");
            builder.AppendLine("<tr>");

            Type tClass = typeof(T);
            PropertyInfo[] pClass = tClass.GetProperties();
            for (int i = 0; i < pClass.Length; i++)
            {
                if (!(pClass[i].Name == "EntityState" || pClass[i].Name == "EntityKey" || pClass[i].Name.Trim().ToLower() == "idx"))
                {
                    builder.AppendLine(String.Format("<td bgcolor='#EFEFEF' align='center'><b>{0}</b></td>", pClass[i].Name));
                }
            }

            builder.AppendLine("</tr>");
            Type type;
            PropertyInfo[] pc;
            foreach (T obj in Data)
            {
                try
                {
                    builder.AppendLine("<tr>");

                    type = obj.GetType();
                    pc = type.GetProperties();
                    foreach (PropertyInfo getpc in pc)
                    {
                        if (!(getpc.Name == "EntityState" || getpc.Name == "EntityKey" || getpc.Name.Trim().ToLower() == "idx"))
                        {
                            builder.AppendLine(String.Format("<td align='center'>=\"{0}\"</td>", getpc.GetValue(obj, null)));
                        }
                    }
                    builder.AppendLine("</tr>");
                }
                catch
                {

                }
            }
            builder.AppendLine("</table>");
            builder.AppendLine("</div>");
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");
        }
    }
}
