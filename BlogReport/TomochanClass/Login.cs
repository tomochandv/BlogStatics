using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace TomochanClass
{
    /// <summary>
    /// 로그인을 위한 클래스.
    /// 1. 암호화 하여 쿠키에 저장
    /// 2. 쿠키생성
    /// 3. cookie 쓰기
    /// 4. cookie 읽기
    /// 5. cookie 버림.
    /// 6. web.config 설정 하여야함.
    /// </summary>
    public class TomochanLogin
    {   
        /// <summary>
        /// Cookie 생성 및 사이트 인증 함.
        /// Cookie 값은 암호화됨.
        /// cookie 안의 값들에 ":" , ";" 값은 넣으면 안됨!.
        /// </summary>
        /// <param name="dic">dictionary 객체. [키, 값] 으로 구성</param>
        /// <returns>성공여부</returns>
        #region public static bool CreateLoginCookie(Dictionary<string, string> dic)
        public static bool CreateLoginCookie(Dictionary<string, string> dic)
        {
            bool returnValue = true;
            try
            {
                string strCookie = string.Empty;
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    strCookie += string.Format("{0}:{1};", kvp.Key, kvp.Value);
                }
                FormsAuthentication.SetAuthCookie(TomochanUtill.Encryption(strCookie), true);
            }
            catch (Exception ex)
            {
                TomochanError.WriteErrorLog(ex);
                returnValue = false;
            }
            return returnValue;
        } 
        #endregion

        /// <summary>
        /// 쿠키의 값을 얻어옴
        /// </summary>
        /// <param name="key">키값</param>
        /// <returns>value</returns>
        #region public static string GetCookieValue(string key)
        public static string GetCookieValue(string key)
        {
            System.Web.UI.Page page = new System.Web.UI.Page();
            string value = string.Empty;
            try
            {
                string encIdentity = TomochanUtill.Decryption(page.User.Identity.Name);
                string[] temp1 = encIdentity.Split(';');
                for (int i = 0; i < temp1.Length; i++)
                {
                    string[] arrTemp = temp1[i].Split(':');
                    if (arrTemp[0] == key)
                    {
                        value = arrTemp[1];
                    }
                }
            }
            catch (Exception ex)
            {
                TomochanError.WriteErrorLog(ex);
            }
            return value;
        } 
        #endregion

        /// <summary>
        /// 로그아웃
        /// </summary>
        /// <returns>성공여부</returns>
        #region public static bool GoodByeSite()
        public static bool GoodByeSite()
        {
            bool returnValue = true;
            try
            {
                FormsAuthentication.SignOut();
            }
            catch (Exception ex)
            {
                TomochanError.WriteErrorLog(ex);
                returnValue = false;
            }
            return returnValue;
        } 
        #endregion

        /// <summary>
        /// 로그인 유무
        /// </summary>
        /// <returns></returns>
        #region public static bool LoginYn()
        public static bool LoginYn()
        {
            System.Web.UI.Page page = new System.Web.UI.Page();
            bool returnValue = true;
            try
            {
                if (page.User.Identity.IsAuthenticated)
                {
                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                }
            }
            catch (Exception ex)
            {
                TomochanError.WriteErrorLog(ex);
                returnValue = false;
            }
            return returnValue;
        } 
        #endregion
    }
}
