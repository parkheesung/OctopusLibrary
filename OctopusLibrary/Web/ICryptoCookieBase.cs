using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctopusLibrary.Web
{
    public interface ICryptoCookieBase
    {
        bool ExistsCookie(string key);
        string GetCookie(string hashKey, string key, string defaultValue);
        string GetCookie(string key, string defaultValue);
        void RemoveClear();
        void RemoveCookie(string key);
        void SetCookie(string key, string value);
        void SetCookie(string key, string value, DateTime expireDate);
        void SetCookie(string hashKey, string key, string value);
        void SetCookie(string hashKey, string key, string value, DateTime expireDate);
    }
}
