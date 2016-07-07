using System;

namespace OctopusLibrary.Models
{
    public class ReturnData
    {
        public bool Check { get; set; }
        public long Code { get; set; }
        public string Message { get; set; }
        public string Value { get; set; }

        public void Success(long code, string msg = "", string value = "")
        {
            this.Check = true;
            this.Code = code;
            this.Message = msg;
            this.Value = value;
        }

        public void Error(string msg)
        {
            this.Check = false;
            this.Code = -1;
            this.Message = msg;
        }

        public void Error(Exception ex)
        {
            this.Check = false;
            this.Code = -1;

            if (ex != null)
            {
                if (ex.InnerException != null)
                {
                    if (!String.IsNullOrEmpty(ex.InnerException.Message))
                    {
                        this.Message = ex.InnerException.Message;
                    }
                    else
                    {
                        this.Message = ex.Message;
                    }
                }
                else
                {
                    this.Message = ex.Message;
                }
            }
        }

        public void Fail(string msg)
        {
            this.Check = false;
            this.Code = -1;
            this.Message = msg;
        }

        public void Error(string msg, params object[] data)
        {
            this.Error(String.Format(msg, data));
        }

        public void Fail(string msg, params object[] data)
        {
            this.Fail(String.Format(msg, data));
        }
    }
}
