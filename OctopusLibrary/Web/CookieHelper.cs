using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OctopusLibrary.Web
{
    public class CookieHelper
    {
        /// <summary>
        /// 해당 쿠키가 있는지 여부를 확인합니다.
        /// </summary>
        /// <param name="key">쿠키 이름</param>
        /// <returns></returns>
        public static bool ExistsCookie(string key)
        {
            bool result = false;

            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(key);
                if (cookie == null)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public static string GetCookie(string key, string defaultValue = "")
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(key);
            if (cookie == null)
            {
                return defaultValue;
            }
            else
            {
                return HttpContext.Current.Server.UrlDecode(cookie.Value);
            }
        }

        /// <summary>
        /// 쿠키를 모두 삭제합니다.
        /// 삭제를 보장하지 않으므로 주의하여 사용하세요.
        /// </summary>
        public static void RemoveClear()
        {
            string[] keys = HttpContext.Current.Request.Cookies.AllKeys;

            try
            {
                HttpCookie cookie = null;
                foreach (string key in keys)
                {
                    cookie = HttpContext.Current.Request.Cookies.Get(key);
                    cookie.Value = "";
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(cookie);
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

        /// <summary>
        /// 특정 쿠키를 삭제합니다.
        /// 삭제를 보장하지 않으므로 주의하여 사용하세요.
        /// </summary>
        /// <param name="key">삭제할 쿠키 이름</param>
        public static void RemoveCookie(string key)
        {
            HttpCookie cookie = new HttpCookie(key);
            try
            {
                if (cookie != null)
                {
                    cookie.Value = String.Empty;
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(cookie);
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

        public static void SetCookie(string key, string value)
        {
            //SetCookie(key, value, DateTime.Now.AddDays(1));
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(key);
            if (cookie == null) cookie = new HttpCookie(key);
            cookie.Value = HttpContext.Current.Server.UrlEncode(value);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void SetCookie(string key, string value, DateTime expireDate)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(key);
            if (cookie == null) cookie = new HttpCookie(key);
            cookie.Value = HttpContext.Current.Server.UrlEncode(value);
            cookie.Expires = expireDate;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
