using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace OctopusLibrary
{
    public class TagHelpers
    {
        public static string RemoveTags(string html)
        {
            return Regex.Replace(html, @"[<][a-z|A-Z|/](.|)*?[>]", "");
        }
    }
}
