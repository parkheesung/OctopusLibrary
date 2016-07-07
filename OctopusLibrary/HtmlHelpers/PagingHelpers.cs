using System;
using System.Text;
using System.Web.Mvc;

namespace OctopusLibrary.HtmlHelpers
{
    /// <summary>
    /// 페이징 구성에 필요한 정보
    /// </summary>
    public class PagingHelpers
    {
        public int PagePerCols { get; set; }  //페이징 목록에 표시 갯수
        public int CurPage { get; set; }  //현재페이지
        public int PageSize { get; set; }  //한화면에 나타나는 게시물의 숫자
        public string firstMoveImage { get; set; }  //처음으로
        public string previousMoveImage { get; set; }  //이전목록
        public string nextMoveImage { get; set; }  //다음목록
        public string lastMoveImage { get; set; }  //마지막으로
        public int TotalCount { get; set; }  //총 게시물의 숫자
        public string PageParameterName { get; set; }  //URL상 페이지번호를 나타내는 변수명(초기값 : CurPage)
        public string UrlParameter { get; set; } //페이징 파라미터외에 항시 붙어다녀야할 파라미터값(GET)
        public string Href { get; set; } //파라미터를 포함하지 않은 기본 URL
        public int LastPage { get; set; }  //맨 페이지징 페이지

        public PagingHelpers()
        {
            this.CurPage = 1;
            this.firstMoveImage = String.Empty;
            this.previousMoveImage = String.Empty;
            this.nextMoveImage = String.Empty;
            this.lastMoveImage = String.Empty;
            this.PagePerCols = 10;  //하단 페이징 갯수는 10개씩 그려줍니다.
            this.PageSize = 10;  //본문 게시물수는 10개씩 그려줍니다.
            this.PageParameterName = "CurPage";
            this.Href = String.Empty;
            this.TotalCount = 0;
        }

        public void PageSet(string href, int totalCnt = 0, int curPage = 1)
        {
            this.Href = href;
            this.TotalCount = totalCnt;
            this.CurPage = curPage;
        }
    }
}
