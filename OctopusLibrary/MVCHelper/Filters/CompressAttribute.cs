using System;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;
namespace OctopusLibrary.Filters
{
    public class CompressAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Global.asax.cs
        /// GlobalFilters.Filters.Add(new CompressAttribute());
        /// </summary>
        private const string _acceptEncodingHeader = "Accept-Encoding";
        private const string _contentEncodingHeader = "Content-Encoding";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase request = filterContext.HttpContext.Request;
            string acceptEncoding = request.Headers[_acceptEncodingHeader];
            if (String.IsNullOrEmpty(acceptEncoding)) return;
            acceptEncoding = acceptEncoding.ToUpperInvariant();
            HttpResponseBase response = filterContext.HttpContext.Response;
            if (acceptEncoding.Contains("GZIP"))
            {
                response.AppendHeader(_contentEncodingHeader, "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE"))
            {
                response.AppendHeader(_contentEncodingHeader, "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }
    }
}
