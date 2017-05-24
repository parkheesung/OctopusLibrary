using OctopusLibrary.Crypto;
using System;
using System.Web;

namespace OctopusLibrary.Web
{
    public class CryptoCookie
    {
        public CryptoCookie()
        {
        }

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
                HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
                if (cookie == null)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal("==[ UVCFramework.Web.Crypto ]==");
                Logger.Fatal("CryptoCookie -> ExistsCookie Fail (Parapmeter [ key : {0} ])", key);
                if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                {
                    Logger.Fatal("Exception Detail : {0}", ex.InnerException.Message);
                }
                result = false;
            }

            return result;
        }

        /// <summary>
        /// AES로 암호화된 쿠키 값을 가져옵니다.
        /// </summary>
        /// <param name="hashKey">AES 암호화키</param>
        /// <param name="key">쿠키 이름</param>
        /// <param name="defaultValue">해당 쿠키값이 없을 경우 대체할 기본값</param>
        /// <returns></returns>
        public static string GetCookie(string hashKey, string key, string defaultValue = "")
        {
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
                if (cookie == null)
                {
                    return defaultValue;
                }
                else
                {
                    return HttpContext.Current.Server.UrlDecode(AES256.Decrypt(cookie.Value, hashKey));
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal("==[ UVCFramework.Web.Crypto ]==");
                Logger.Fatal("CryptoCookie -> GetCookie Fail (Parapmeter [ hashKey : {0}, key : {1}, defaultValue : {2} ])", hashKey, key, defaultValue);
                if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                {
                    Logger.Fatal("Exception Detail : {0}", ex.InnerException.Message);
                }
                return defaultValue;
            }
        }

        /// <summary>
        /// Base64로 암호화된 쿠키 값을 가져옵니다.
        /// </summary>
        /// <param name="key">쿠키 이름</param>
        /// <param name="defaultValue">해당 쿠키값이 없을 경우 대체할 기본값</param>
        /// <returns></returns>
        public static string GetCookie(string key, string defaultValue = "")
        {
            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
                if (cookie == null)
                {
                    return defaultValue;
                }
                else
                {
                    return HttpContext.Current.Server.UrlDecode(AES256.Decrypt(cookie.Value, Salt.PrivateKey));
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal("==[ UVCFramework.Web.Crypto ]==");
                Logger.Fatal("CryptoCookie -> GetCookie Fail (Parapmeter [ key : {0}, defaultValue : {1} ])", key, defaultValue);
                if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                {
                    Logger.Fatal("Exception Detail : {0}", ex.InnerException.Message);
                }
                return defaultValue;
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
                    cookie = HttpContext.Current.Request.Cookies[key];
                    cookie.Value = null;
                    cookie.Expires = DateTime.Now.AddDays(-1d);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                    HttpContext.Current.Response.Cookies.Remove(key);
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal("==[ UVCFramework.Web.Crypto ]==");
                Logger.Fatal("CryptoCookie -> RemoveClear Fail");
                if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                {
                    Logger.Fatal("Exception Detail : {0}", ex.InnerException.Message);
                }
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
            try
            {
                if (HttpContext.Current.Request.Cookies[key] != null)
                {
                    HttpCookie cookie = new HttpCookie(key);
                    cookie.Expires = DateTime.Now.AddDays(-1d);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }

            }
            catch (Exception ex)
            {
                Logger.Fatal("==[ UVCFramework.Web.Crypto ]==");
                Logger.Fatal("CryptoCookie -> RemoveCookie Fail (Parapmeter [ key : {0} ])", key);
                if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                {
                    Logger.Fatal("Exception Detail : {0}", ex.InnerException.Message);
                }
            }
            finally
            {
                HttpContext.Current.Response.Cookies.Remove(key);
            }
        }

        /// <summary>
        /// 쿠키 값을 Base64로 암호화 하여 저장합니다.
        /// </summary>
        /// <param name="key">쿠키 이름</param>
        /// <param name="value">저장할 값</param>
        public static void SetCookie(string key, string value)
        {
            //SetCookie(key, value, DateTime.Now.AddDays(1));
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null) cookie = new HttpCookie(key);
            cookie.Value = AES256.Encrypt(HttpContext.Current.Server.UrlEncode(value), Salt.PrivateKey);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 쿠키 값을 Base64로 암호화 하여 저장합니다.
        /// </summary>
        /// <param name="key">쿠키 이름</param>
        /// <param name="value">저장할 값</param>
        /// <param name="expireDate">쿠키 만료일</param>
        public static void SetCookie(string key, string value, DateTime expireDate)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null) cookie = new HttpCookie(key);
            cookie.Value = AES256.Encrypt(HttpContext.Current.Server.UrlEncode(value), Salt.PrivateKey);
            cookie.Expires = expireDate;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 쿠키 값을 RSA AES 256으로 암호화하여 저장합니다.
        /// </summary>
        /// <param name="key">쿠키 이름</param>
        /// <param name="value">저장할 값</param>
        public static void SetCookie(string hashKey, string key, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null) cookie = new HttpCookie(key);
            cookie.Value = AES256.Encrypt(HttpContext.Current.Server.UrlEncode(value), hashKey);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 쿠키 값을 RSA AES 256으로 암호화하여 저장합니다.
        /// </summary>
        /// <param name="key">쿠키 이름</param>
        /// <param name="value">저장할 값</param>
        /// <param name="expireDate">쿠키 만료일</param>
        public static void SetCookie(string hashKey, string key, string value, DateTime expireDate)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null) cookie = new HttpCookie(key);
            cookie.Value = AES256.Encrypt(HttpContext.Current.Server.UrlEncode(value), hashKey);
            cookie.Expires = expireDate;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }


}
