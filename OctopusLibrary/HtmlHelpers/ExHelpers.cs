using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace OctopusLibrary.HtmlHelpers
{
    public static class ExHelpers
    {

        /// <summary>
        /// 문자열이 숫자인 경우 3자리마다 콤마를 표시합니다.
        /// </summary>
        /// <param name="str">숫자로된 문자열</param>
        /// <returns></returns>
        public static string Format(this string str)
        {
            string result = "0";
            Regex pattern = new Regex("<[^>]+>");
            Regex pattern2 = new Regex("(?<=[0-9])(?=([0-9][0-9][0-9])+(?![0-9]))");

            result = (String.IsNullOrEmpty(str)) ? "" : pattern.Replace(str, "");
            result = (String.IsNullOrEmpty(result)) ? "0" : pattern2.Replace(result, ",");

            return result;
        }

        /// <summary>
        /// 해당 숫자에 3자리마다 콤마를 표시합니다.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string Format(this int num)
        {
            string str = Convert.ToString(num);
            string result = "0";
            Regex pattern = new Regex("<[^>]+>");
            Regex pattern2 = new Regex("(?<=[0-9])(?=([0-9][0-9][0-9])+(?![0-9]))");

            result = (String.IsNullOrEmpty(str)) ? "" : pattern.Replace(str, "");
            result = (String.IsNullOrEmpty(result)) ? "0" : pattern2.Replace(result, ",");

            return result;
        }

        /// <summary>
        /// 해당 숫자에 3자리마다 콤마를 표시합니다.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string Format(this double num)
        {
            string str = Convert.ToString(num);
            string result = "0";
            Regex pattern = new Regex("<[^>]+>");
            Regex pattern2 = new Regex("(?<=[0-9])(?=([0-9][0-9][0-9])+(?![0-9]))");

            result = (String.IsNullOrEmpty(str)) ? "" : pattern.Replace(str, "");
            result = (String.IsNullOrEmpty(result)) ? "0" : pattern2.Replace(result, ",");

            return result;
        }

        /// <summary>
        /// 두값을 비교하여 참인 경우에만 returnHTML을 반환합니다.
        /// </summary>
        /// <param name="originalValue">원본</param>
        /// <param name="compareValue">비교대상</param>
        /// <param name="returnHTML">참일 경우 반환할 문자열</param>
        /// <returns></returns>
        public static MvcHtmlString ValueCompare(this string originalValue, string compareValue, string returnHTML)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(returnHTML) && !String.IsNullOrEmpty(originalValue))
            {
                if (originalValue.ToLower().Equals(compareValue.ToLower()))
                {
                    result = returnHTML;
                }
            }

            return MvcHtmlString.Create(result);
        }

        /// <summary>
        /// 두값을 비교하여 참인 경우에만 returnHTML을 반환합니다.
        /// </summary>
        /// <param name="originalValue">원본</param>
        /// <param name="compareValue">비교대상</param>
        /// <param name="returnHTML">참일 경우 반환할 문자열</param>
        /// <returns></returns>
        public static MvcHtmlString ValueCompare(this int originalValue, int compareValue, string returnHTML)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(returnHTML))
            {
                if (originalValue == compareValue)
                {
                    result = returnHTML;
                }
            }

            return MvcHtmlString.Create(result);
        }

        /// <summary>
        /// 두값을 비교하여 참인 경우에만 returnHTML을 반환합니다.
        /// </summary>
        /// <param name="originalValue">원본</param>
        /// <param name="compareValue">비교대상</param>
        /// <param name="returnHTML">참일 경우 반환할 문자열</param>
        /// <returns></returns>
        public static MvcHtmlString ValueCompare(this bool originalValue, bool compareValue, string returnHTML)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(returnHTML))
            {
                if (originalValue == compareValue)
                {
                    result = returnHTML;
                }
            }

            return MvcHtmlString.Create(result);
        }

        /// <summary>
        /// 두값을 비교하여 참인 경우에만 returnHTML을 반환합니다.
        /// </summary>
        /// <param name="originalValue">원본</param>
        /// <param name="compareValue">비교대상</param>
        /// <param name="returnHTML">참일 경우 반환할 문자열</param>
        /// <returns></returns>
        public static MvcHtmlString ValueCompare(this string originalValue, int compareValue, string returnHTML)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(returnHTML) && !String.IsNullOrEmpty(originalValue))
            {
                if (originalValue.Equals(Convert.ToString(compareValue)))
                {
                    result = returnHTML;
                }
            }

            return MvcHtmlString.Create(result);
        }

        /// <summary>
        /// 두값을 비교하여 참인 경우에만 returnHTML을 반환합니다.
        /// </summary>
        /// <param name="originalValue">원본</param>
        /// <param name="compareValue">비교대상</param>
        /// <param name="returnHTML">참일 경우 반환할 문자열</param>
        /// <returns></returns>
        public static MvcHtmlString ValueCompare(this int originalValue, string compareValue, string returnHTML)
        {
            string result = String.Empty;

            try
            {
                if (!String.IsNullOrEmpty(returnHTML))
                {
                    Regex pattern = new Regex("[0-9]{1,50}");
                    Match matchResult = pattern.Match(compareValue);
                    if (matchResult.Success)
                    {
                        int tmp = Convert.ToInt32(compareValue);
                        if (originalValue == tmp)
                        {
                            result = returnHTML;
                        }
                    }
                }
            }
            catch
            {
                result = String.Empty;
            }

            return MvcHtmlString.Create(result);
        }

        public static MvcHtmlString ValueCompare(this int originalValue, int Max, int Min, string returnHTML)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(returnHTML))
            {
                if (originalValue <= Max && originalValue >= Min)
                {
                    result = returnHTML;
                }
            }

            return MvcHtmlString.Create(result);
        }

        public static MvcHtmlString ValueCompare(this int originalValue, bool CheckWhere, string returnHTML)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(returnHTML))
            {
                if (CheckWhere)
                {
                    result = returnHTML;
                }
            }

            return MvcHtmlString.Create(result);
        }

        public static MvcHtmlString ValueCompare(this int originalValue, int[] compareValue, string returnHTML)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(returnHTML))
            {
                if (compareValue != null && compareValue.Length > 0)
                {
                    for (int i = 0; i < compareValue.Length; i++)
                    {
                        if (compareValue[i] == originalValue)
                        {
                            result = returnHTML;
                            break;
                        }
                    }
                }
            }

            return MvcHtmlString.Create(result);
        }

        public static MvcHtmlString ValueCompare(this string originalValue, string[] compareValue, string returnHTML)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(returnHTML))
            {
                if (compareValue != null && compareValue.Length > 0)
                {
                    for (int i = 0; i < compareValue.Length; i++)
                    {
                        if (compareValue[i] == originalValue)
                        {
                            result = returnHTML;
                            break;
                        }
                    }
                }
            }

            return MvcHtmlString.Create(result);
        }

        public static MvcHtmlString ValueContain(this string originalValue, string compareValue, string returnHTML)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(returnHTML))
            {
                if ((originalValue != null && originalValue.Length > 0) && (compareValue != null && compareValue.Length > 0))
                {
                    if (originalValue.IndexOf(compareValue) > -1)
                    {
                        result = returnHTML;
                    }
                }
            }

            return MvcHtmlString.Create(result);
        }

        public static MvcHtmlString ValueSplitContain(this string arrStr, char indexer, string compareValue, string returnHTML)
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(returnHTML))
            {
                if ((arrStr != null && arrStr.Length > 0) && (compareValue != null && compareValue.Length > 0))
                {
                    string[] arrStrSplit = arrStr.Split(indexer);
                    if (arrStrSplit.Length > 0)
                    {
                        for (int i = 0; i < arrStrSplit.Length; i++)
                        {
                            if (arrStrSplit[i].Trim().ToUpper().Equals(compareValue.Trim().ToUpper()))
                            {
                                result = returnHTML;
                            }
                        }
                    }
                }
            }

            return MvcHtmlString.Create(result);
        }


    }
}
