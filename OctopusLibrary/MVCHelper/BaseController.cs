using System.Configuration;
using System.Web.Mvc;

namespace OctopusLibrary
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            filterContext.HttpContext.Response.AddHeader("p3p", "CP=\"NOI DEVa TAIa OUR BUS UNI\"");
        }

        /// <summary>
        /// 브라우저 캐시 모드를 설정합니다.
        /// </summary>
        /// <param name="filterContext">컨텍스트</param>
        /// <param name="IsCacheUse">캐시 사용 유무</param>
        protected virtual void SetApplicationCache(ActionExecutedContext filterContext, bool IsCacheUse)
        {
            if (IsCacheUse)
            {
                filterContext.HttpContext.Response.Expires = 60 * 24;
                filterContext.HttpContext.Response.Cache.SetAllowResponseInBrowserHistory(true);
                filterContext.HttpContext.Response.CacheControl = "Private"; //no-cache, private, public
            }
            else
            {
                filterContext.HttpContext.Response.Expires = -1;
                filterContext.HttpContext.Response.Cache.SetNoServerCaching();
                filterContext.HttpContext.Response.Cache.SetAllowResponseInBrowserHistory(false);
                filterContext.HttpContext.Response.CacheControl = "no-cache"; //no-cache, private, public
                filterContext.HttpContext.Response.Cache.SetNoStore();
            }
        }

        /// <summary>
        /// 브라우저 캐시 모드를 설정합니다.
        /// </summary>
        /// <param name="filterContext">컨텍스트</param>
        /// <param name="DaySize">캐시가 유지되는 기간</param>
        protected virtual void SetApplicationCache(ActionExecutedContext filterContext, int DaySize)
        {
            filterContext.HttpContext.Response.Expires = DaySize;
            filterContext.HttpContext.Response.Cache.SetAllowResponseInBrowserHistory(true);
            filterContext.HttpContext.Response.CacheControl = "Private"; //no-cache, private, public
        }

        protected string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
