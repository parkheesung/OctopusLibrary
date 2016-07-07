using OctopusLibrary.Crypto;
using System;
using System.Web;


namespace OctopusLibrary.Web
{
    public class CryptoCookieBase : ICryptoCookieBase
    {
        private HttpCookie cookie;
        private string domain;

        public CryptoCookieBase()
        {

        }

        public CryptoCookieBase(string _domain)
        {
            this.domain = _domain;
        }

        public bool ExistsCookie(string key)
        {
            cookie = HttpContext.Current.Request.Cookies.Get(key);
            if (cookie == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string GetCookie(string key, string defaultValue)
        {
            cookie = HttpContext.Current.Request.Cookies.Get(key);
            if (cookie == null)
            {
                return defaultValue;
            }
            else
            {
                return HttpContext.Current.Server.UrlDecode(AES256.Decrypt(cookie.Value, Salt.PrivateKey));
            }
        }

        public string GetCookie(string hashKey, string key, string defaultValue)
        {
            cookie = HttpContext.Current.Request.Cookies.Get(key);
            if (cookie == null)
            {
                return defaultValue;
            }
            else
            {
                return HttpContext.Current.Server.UrlDecode(AES256.Decrypt(cookie.Value, hashKey));
            }
        }

        public void RemoveClear()
        {
            string[] keys = HttpContext.Current.Request.Cookies.AllKeys;
            try
            {
                foreach (string key in keys)
                {
                    cookie = HttpContext.Current.Request.Cookies.Get(key);
                    cookie.Value = null;
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Remove(key);
                }
            }
            catch
            {
            }
            finally
            {
                HttpContext.Current.Response.Cookies.Clear();
            }
        }

        public void RemoveCookie(string key)
        {
            cookie = HttpContext.Current.Request.Cookies.Get(key);
            try
            {
                if (cookie != null)
                {
                    cookie.Value = null;
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Set(cookie);
                }
            }
            catch
            {
            }
            finally
            {
                HttpContext.Current.Response.Cookies.Remove(key);
            }
        }

        public void SetCookie(string key, string value)
        {
            cookie = HttpContext.Current.Request.Cookies.Get(key);
            if (cookie == null)
            {
                cookie = new HttpCookie(key);
                if (!String.IsNullOrEmpty(this.domain))
                {
                    cookie.Domain = this.domain;
                }
            }
            cookie.Value = AES256.Encrypt(HttpContext.Current.Server.UrlEncode(value), Salt.PrivateKey);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void SetCookie(string hashKey, string key, string value)
        {
            cookie = HttpContext.Current.Request.Cookies.Get(key);
            if (cookie == null)
            {
                cookie = new HttpCookie(key);
                if (!String.IsNullOrEmpty(this.domain))
                {
                    cookie.Domain = this.domain;
                }
            }
            cookie.Value = AES256.Encrypt(HttpContext.Current.Server.UrlEncode(value), hashKey);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void SetCookie(string key, string value, DateTime expireDate)
        {
            cookie = HttpContext.Current.Request.Cookies.Get(key);
            if (cookie == null)
            {
                cookie = new HttpCookie(key);
                if (!String.IsNullOrEmpty(this.domain))
                {
                    cookie.Domain = this.domain;
                }
            }
            cookie.Value = AES256.Encrypt(HttpContext.Current.Server.UrlEncode(value), Salt.PrivateKey);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void SetCookie(string hashKey, string key, string value, DateTime expireDate)
        {
            cookie = HttpContext.Current.Request.Cookies.Get(key);
            if (cookie == null)
            {
                cookie = new HttpCookie(key);
                if (!String.IsNullOrEmpty(this.domain))
                {
                    cookie.Domain = this.domain;
                }
            }
            cookie.Value = AES256.Encrypt(HttpContext.Current.Server.UrlEncode(value), hashKey);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
