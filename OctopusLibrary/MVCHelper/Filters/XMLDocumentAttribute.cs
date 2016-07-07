using System.Web.Mvc;

namespace OctopusLibrary.Filters
{
    public class XMLDocumentAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            filterContext.HttpContext.Response.ContentType = "Text/XML";
            filterContext.HttpContext.Response.Charset = "UTF-8";
        }
    }
}
