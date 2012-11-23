using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Web;

namespace TomochanClass
{
    /// <summary>
    /// 에러로그를 저장 합니다.
    /// 경로는 web.config의 appsettings에 errorLogPath 값을 읽습니다.
    /// </summary>
    public class TomochanError
    {
        private static string rootPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["errorLogPath"]);
        private static string path = string.Format("{0}\\{1}.txt", rootPath, DateTime.Now.ToShortDateString().Replace("-", ""));
        /// <summary>
        /// Error 파일을 만들고 씁니다.
        /// </summary>
        /// <param name="ex">Exception</param>
        #region public static void WriteErrorLog(Exception ex)
        public static void WriteErrorLog(Exception ex)
        {
            try
            {
                DirectoryInfo drtPath = new DirectoryInfo(rootPath);

                if (!drtPath.Exists)
                    drtPath.Create();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("====================================================================================================");
                sb.AppendLine(string.Format("Date : {0}", DateTime.Now.ToString()));
                sb.AppendLine(string.Format("Message : {0}", ex.Message));
                sb.AppendLine(string.Format("Source : {0}", ex.Source));
                sb.AppendLine(string.Format("StackTrace : {0}", ex.StackTrace));
                sb.AppendLine("====================================================================================================");
                //오류로그 작성
                StreamWriter stream = new StreamWriter(path, true, System.Text.Encoding.Default);
                stream.WriteLine(sb.ToString());
                stream.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        } 
        #endregion

        /// <summary>
        /// 지정못하는 디렉토리에 스트링 로그를 남깁니다. 
        /// </summary>
        /// <param name="str">로그내용</param>
        #region public static void WriteErrorLog(string str)
        public static void WriteErrorLog(string str)
        {
            try
            {
                DirectoryInfo drtPath = new DirectoryInfo(rootPath);

                if (!drtPath.Exists)
                    drtPath.Create();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("====================================================================================================");
                sb.AppendLine(string.Format("Date : {0}", DateTime.Now.ToString()));
                sb.AppendLine(string.Format("Message : {0}", str));
                sb.AppendLine("====================================================================================================");
                //오류로그 작성
                StreamWriter stream = new StreamWriter(path, true, System.Text.Encoding.Default);
                stream.WriteLine(sb.ToString());
                stream.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        } 
        #endregion

        /// <summary>
        /// 지정한 디렉토리에 스트링 로그를 남깁니다. 
        /// </summary>
        /// <param name="path">디렉토리</param>
        /// <param name="str">로그내용</param>
        #region public static void WriteErrorLog(string path, string str)
        public static void WriteErrorLog(string path, string str)
        {
            try
            {
                DirectoryInfo drtPath = new DirectoryInfo(path);

                if (!drtPath.Exists)
                    drtPath.Create();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("====================================================================================================");
                sb.AppendLine(string.Format("Date : {0}", DateTime.Now.ToString()));
                sb.AppendLine(string.Format("Message : {0}", str));
                sb.AppendLine("====================================================================================================");
                //오류로그 작성
                StreamWriter stream = new StreamWriter(string.Format("{0}{1}.txt", path, DateTime.Now.ToShortDateString())
                    , true, System.Text.Encoding.Default);
                stream.WriteLine(sb.ToString());
                stream.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        } 
        #endregion
    }
}
