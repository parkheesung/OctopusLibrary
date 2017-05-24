using System;

namespace OctopusLibrary.Models
{
    public class ReturnValues<T> where T : new()
    {
        public long Code { get; set; }
        public bool Check { get; set; }
        public string Message { get; set; }
        public string Value { get; set; }

        public T Data { get; set; }

        public ReturnValues()
        {
            this.Code = 0;
            this.Check = false;
            this.Message = String.Empty;
            this.Value = String.Empty;
            this.Data = new T();
        }

        public void Success(long resultCode)
        {
            this.Success(resultCode, "", "", new T());
        }

        public void Success(long resultCode, string value)
        {
            this.Success(resultCode, value, "", new T());
        }

        public void Success(long resultCode, T data)
        {
            this.Success(resultCode, "", "", data);
        }

        public void Success(long resultCode, string value, T data)
        {
            this.Success(resultCode, value, "", data);
        }

        public void Success(long resultCode, string value, string message, T data)
        {
            this.Code = resultCode;
            this.Check = (this.Code > 0);
            this.Value = value;
            this.Message = message;
            this.Data = data;
        }

        public void Error(string message)
        {
            this.Code = -1;
            this.Check = false;
            this.Message = message;
        }

        public void Error(Exception ex)
        {
            this.Code = -1;
            this.Check = false;
            this.Message = ex.Message;
        }
    }
}
