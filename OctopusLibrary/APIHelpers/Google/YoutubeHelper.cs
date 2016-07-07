using Google.Apis.YouTube.v3.Data;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace OctopusLibrary.APIHelpers.Google
{
    public class YoutubeHelper
    {
        public static string Thumbnail(string youtubeID)
        {
            return String.Format("//img.youtube.com/vi/{0}/0.jpg", youtubeID);
        }

        public static string Thumbnail(Uri url)
        {
            string youtubeID = String.Empty;
            string tmp = url.ToString();
            if (tmp.IndexOf("?") > -1)
            {
                tmp = tmp.Split('?')[1];
            }
            string key = String.Empty;
            string value = String.Empty;

            if (tmp.IndexOf("&") > -1)
            {
                string[] arr = tmp.Split('&');
                foreach(string t in arr)
                {
                    key = t.Split('=')[0];
                    value = t.Split('=')[1];
                    if (key.Equals("v"))
                    {
                        youtubeID = value;
                        break;
                    }
                }
            }
            else
            {
                key = tmp.Split('=')[0];
                value = tmp.Split('=')[1];
                if (key.Equals("v"))
                {
                    youtubeID = value;
                }

            }

            return Thumbnail(youtubeID);
        }
    }

    public static class ExYoutubeHelper
    {
        public static string GetThumbnail(this SearchResult _youtube)
        {
            string result = String.Empty;

            if (_youtube.Snippet.Thumbnails.Standard != null && _youtube.Snippet.Thumbnails.Standard.Url != null)
            {
                result = _youtube.Snippet.Thumbnails.Standard.Url;
            }
            else if (_youtube.Snippet.Thumbnails.Medium != null && _youtube.Snippet.Thumbnails.Medium.Url != null)
            {
                result = _youtube.Snippet.Thumbnails.Medium.Url;
            }
            else if (_youtube.Snippet.Thumbnails.High != null && _youtube.Snippet.Thumbnails.High.Url != null)
            {
                result = _youtube.Snippet.Thumbnails.High.Url;
            }

            return result;
        }

        public static string GetTitle(this SearchResult _youtube)
        {
            StringBuilder result = new StringBuilder();
            result.Append(HttpContext.Current.Server.UrlDecode(_youtube.Snippet.Title));
            Regex reg = new Regex("[\\!\\@\\#\\$\\%\\^\\&\\*\\(\\)\\;\\\\\\\\/\\|\\<\\>\\\"\\'\\[\\]\\{\\}\\-\\+\\=\\:\\.]+");
            return reg.Replace(result.ToString(), "").Trim();
        }
    }
}
