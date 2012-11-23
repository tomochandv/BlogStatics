using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TomochanClass
{
    /// <summary>
    /// Paging을 위한 class
    /// 총 게시물수, 보여질 행의 수를 넘겨주면 string으로 페이저 돌려줌. 스타일은 naver의 NULI 스타일을 적용.
    /// </summary>
    public class TomochanPaging
    {
        /// <summary>
        /// 총갯수
        /// </summary>
        public static int total { get; set; }
        /// <summary>
        /// 보여질 행의 수
        /// </summary>
        public static int row { get; set; }
        /// <summary>
        /// Paging의 Style : CSS의 class 는 paginate 입니다.
        /// </summary>
        public static string pagingStyle = @"<style type='text/css'>
        .paginate{padding:15px 0;text-align:center; margin:0;}
        .paginate a,.paginate strong{display:inline-block;position:relative;_width /**/:17px;margin-right:1px;padding:3px 3px 5px;border:1px solid #fff;color:#000;font-family:Verdana;font-size:13px;font-weight:bold;line-height:normal;text-decoration:none}
        .paginate strong{border:1px solid #e9e9e9;color:#f23219 !important}
        .paginate .pre{margin-right:9px;padding:7px 6px 5px 16px;background:url(http://static.naver.com/common/paginate/bu_pg3_l_off.gif) no-repeat 6px 9px !important}
        .paginate .next{margin-left:9px;padding:7px 16px 5px 6px;background:url(http://static.naver.com/common/paginate/bu_pg3_r_off.gif) no-repeat 71px 9px !important}
        .paginate a.pre{background:url(http://static.naver.com/common/paginate/bu_pg3_l_on.gif) no-repeat 6px 9px !important}
        .paginate a.next{background:url(http://static.naver.com/common/paginate/bu_pg3_r_on.gif) no-repeat 74px 9px !important}
        .paginate .pre,.paginate .next{display:inline-block;position:relative;top:1px;_width /**/:84px;border:1px solid #e9e9e9;color:#ccc;font-family:'굴림',Gulim;font-size:12px;line-height:normal}
        .paginate a.pre,.paginate a.next{color:#565656}
        .paginate a:hover{border:1px solid #e9e9e9;background-color:#f7f7f7 !important}
        </style>";

        /// <summary>
        /// 페이저 호출
        /// </summary>
        /// <returns></returns>
        #region public static string Pager()
        public static string Pager()
        {
            HttpRequest Request = HttpContext.Current.Request;
            StringBuilder text = new StringBuilder();
            string requestPage = TomochanUtill.RequestParameterCheck("page");
            int page = requestPage == "" ? 1 : int.Parse(requestPage);


            #region 블럭 계산
            int temp = page % row;
            int temp1 = page / row;

            if (temp == 0) { temp1 = (page / row) - 1; }
            int startBlock = temp1 * 10 + 1;
            int EndBlock = startBlock + 9;
            if (startBlock * row > total)
            {
                EndBlock = startBlock;
            }
            #endregion

            #region style include
            text.Append(pagingStyle);
            #endregion

            #region div 시작  include
            text.Append("<div class=\"paginate\">");
            #endregion

            #region 링크 만듬
            //이전 block 존재 하면 
            if (startBlock > 1)
            {
                text.Append(string.Format("<a href=\"{0}\" class=\"pre\">이전페이지</a>", MakeQuryString(page - 1)));
            }
            else
            {
                text.Append(string.Format("<span class=\"pre\">이전페이지</span>"));
            }

            for (int i = startBlock; i <= EndBlock; i++)
            {
                if (page == i)
                {
                    text.Append(string.Format("<strong>{0}</strong>", i));
                }
                else
                {
                    if (total >= ((i * row) - 10))
                    {
                        text.Append(string.Format("<a href=\"{0}\">{1}</a>", MakeQuryString(i), i));
                    }
                }
            }

            //다음 block 존재 하면
            if ((EndBlock * row) < total)
            {
                text.Append(string.Format("<a href=\"{0}\" class=\"next\">다음페이지</a>", MakeQuryString(page + 1)));
            }
            else
            {
                text.Append(string.Format("<span class=\"next\">다음페이지</span>"));
            }
            #endregion

            #region div 닫음 include
            text.Append("</div>");
            #endregion

            return text.ToString();
        }
        #endregion

        #region 페이징을 위한 url 만들기
        private static string MakeQuryString(int page)
        {
            HttpRequest Request = HttpContext.Current.Request;
            string returnStr = string.Format("{0}?page={1}", Request.Url.AbsolutePath, page);
            string currentquery = Request.Url.Query.Replace("?", "");
            string[] tempq = currentquery.Split('&');
            for (int i = 0; i < tempq.Length; i++)
            {
                if (!tempq[i].Contains("page"))
                {
                    if (i != 0)
                    {
                        returnStr += string.Format("&{0}", tempq[i]);
                    }
                    else
                    {
                        returnStr += string.Format("{0}", tempq[i]);
                    }
                }
            }

            return returnStr;
        }
        #endregion
    }
}
