using OctopusLibrary.HtmlHelpers;
using System;
using System.Text;
using System.Web.Mvc;

namespace OctopusLibrary.HtmlHelpers
{
    /// <summary>
    /// 실제 페이징 구성을 위한 메소드
    /// </summary>
    public static class PagingExtention
    {
        /// <summary>
        /// 페이징 태그 생성
        /// </summary>
        /// <param name="html">MVC에서 페이징을 손쉽게 호출하기 위해 HTML 클래스를 확장합니다.</param>
        /// <param name="helpers">PagingHelpers 모델, 페이징에 필요한 항목들</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString Paging(this HtmlHelper html, PagingHelpers helpers)
        {
            StringBuilder result = new StringBuilder();

            if (helpers.TotalCount > 0)
            {

                int st = 1;  //시작페이지 구분

                //(전체 게시물 / 페이징 단위)로 마지막 페이징 페이지를 확인
                helpers.LastPage = Convert.ToInt32(helpers.TotalCount / helpers.PageSize);
                //만약 만들어야할 페이징 목록 마지막 게시물 갯수가 1개라도 더 있다면, 페이지 1개를 추가로 생성
                if ((helpers.TotalCount % helpers.PageSize) > 0) helpers.LastPage = helpers.LastPage + 1;
                //마지막 페이지가 0보다 크고(실제 컨텐츠가 있고) 현재 페이지가 마지막 페이지보다 크다면, 현재 페이지는 마지막 페이지로 대체
                if (helpers.CurPage > helpers.LastPage && helpers.LastPage > 0) helpers.CurPage = helpers.LastPage;

                //만약 현재페이지가 페이징 단위보다 크다면
                if (helpers.CurPage > helpers.PagePerCols)
                {
                    //현재 페이지를 페이징 단위로 나누어 나머지 값을 측량
                    if ((helpers.CurPage % helpers.PagePerCols) > 0)
                    {
                        //나머지가 0보다 크다면, 페이징 단위가 넘어간 것으로 간주
                        st = ((helpers.CurPage / helpers.PagePerCols) * helpers.PagePerCols) + 1;
                    }
                    else
                    {
                        //나머지가 0이면, 페이징의 마지막 페이지를 보고 있는 것으로 간주
                        st = ((helpers.CurPage / helpers.PagePerCols) * helpers.PagePerCols) - helpers.PagePerCols + 1;
                    }
                }

                int ed = st + helpers.PageSize - 1;  //종료페이지 구분
                if (ed > helpers.LastPage) ed = helpers.LastPage;  //만약 종료페이징 페이지가 최종 페이지보다 크다면 최종 페이지 값으로 교체

                result.Append("<ul>");

                if (helpers.CurPage > helpers.PageSize)
                {
                    result.AppendFormat("<li><a href=\"{0}\"><img src=\"{1}\" alt=\"처음으로\" /></a></li>", CreateHref(helpers, st - helpers.PageSize), helpers.firstMoveImage);
                }
                else
                {
                    result.AppendFormat("<li><a href=\"{0}\"><img src=\"{1}\" alt=\"처음으로\" /></a></li>", CreateHref(helpers, 1), helpers.firstMoveImage);
                }

                if (helpers.CurPage > helpers.PageSize)
                {
                    result.AppendFormat("<li><a href=\"{0}\"><img src=\"{1}\" alt=\"이전으로\" /></a></li>", CreateHref(helpers, st - helpers.PageSize), helpers.previousMoveImage);
                }
                else
                {
                    result.AppendFormat("<li><a href=\"{0}\"><img src=\"{1}\" alt=\"이전으로\" /></a></li>", CreateHref(helpers, 1), helpers.previousMoveImage);
                }

                for (int i = st; i <= ed; i++)
                {
                    if (i == helpers.CurPage)
                    {
                        result.AppendFormat("<li class=\"active\"><a href=\"{0}\">{1}</a></li>", CreateHref(helpers, i), i);
                    }
                    else
                    {
                        result.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", CreateHref(helpers, i), i);
                    }
                }

                result.AppendFormat("<li><a href=\"{0}\"><img src=\"{1}\" alt=\"다음으로\" /></a></li>", CreateHref(helpers, st + helpers.PageSize), helpers.nextMoveImage);
                result.AppendFormat("<li><a href=\"{0}\"><img src=\"{1}\" alt=\"끝으로\" /></a></li>", CreateHref(helpers, helpers.LastPage), helpers.lastMoveImage);
                result.Append("</ul>");
            }
            else
            {
                result.Append("<ul>");
                result.AppendFormat("<li><a href=\"{0}\"><img src=\"{1}\" alt=\"처음으로\" /></a></li>", CreateHref(helpers, 1), helpers.firstMoveImage);
                result.AppendFormat("<li><a href=\"{0}\"><img src=\"{1}\" alt=\"이전으로\" /></a></li>", CreateHref(helpers, 1), helpers.previousMoveImage);
                result.AppendFormat("<li class=\"active\"><a href=\"{0}\">{1}</a></li>", CreateHref(helpers, 1), 1);
                result.AppendFormat("<li><a href=\"{0}\"><img src=\"{1}\" alt=\"다음으로\" /></a></li>", CreateHref(helpers, 1), helpers.nextMoveImage);
                result.AppendFormat("<li><a href=\"{0}\"><img src=\"{1}\" alt=\"끝으로\" /></a></li>", CreateHref(helpers, 1), helpers.lastMoveImage);
                result.Append("</ul>");
            }

            return MvcHtmlString.Create(result.ToString());
        }

        /// <summary>
        /// 상기 Paging 메소드에서 URL 링크 구성을 위한 메소드
        /// </summary>
        /// <param name="helpers">페이징 정보를 가지고 있는 PagingHelpers 모델</param>
        /// <param name="curpage">생성할 URL의 페이징 번호</param>
        /// <returns>string</returns>
        private static string CreateHref(PagingHelpers helpers, int curpage = 1)
        {
            StringBuilder builder = new StringBuilder();
            if (helpers != null && !String.IsNullOrEmpty(helpers.Href))
            {

                if (curpage < 1)
                {
                    curpage = 1;
                }

                if (helpers.Href.Substring(0, 1) != "/")
                {
                    builder.Append("/");
                }
                builder.Append(helpers.Href);
                if (helpers.Href.IndexOf("?") > -1)
                {
                    builder.AppendFormat("&{0}={1}", helpers.PageParameterName, curpage);
                }
                else
                {
                    builder.AppendFormat("?{0}={1}", helpers.PageParameterName, curpage);
                }

                if (!String.IsNullOrEmpty(helpers.UrlParameter))
                {
                    builder.AppendFormat("&{0}", helpers.UrlParameter);
                }
            }

            return builder.ToString();
        }
    }
}
