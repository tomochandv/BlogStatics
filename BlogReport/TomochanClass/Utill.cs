using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.Net;
using System.Data;
using System.Collections;

namespace TomochanClass
{
    /// <summary>
    /// 웹에서 쓰이는 유틸 클래스
    /// </summary>
    public class TomochanUtill
    {
        /// <summary>
        /// 메시지만 출력
        /// </summary>
        /// <param name="str">메세지</param>
        #region public static void JavascriptMessage(string str)
        public static void JavascriptMessage(string str)
        {
            string returnText = string.Empty;
            returnText += string.Format("<script type='text/javascript'>");
            returnText += string.Format("alert('{0}');", str);
            returnText += string.Format("</script>");
            HttpContext.Current.Response.Write(returnText);
        } 
        #endregion

        /// <summary>
        /// 메세지 출력후 이동
        /// </summary>
        /// <param name="str">메세지</param>
        /// <param name="url">url</param>
        #region public static void JavascriptMessageAndMove(string str, string url)
        public static void JavascriptMessageAndMove(string str, string url)
        {
            string returnText = string.Empty;
            returnText += string.Format("<script type='text/javascript'>");
            returnText += string.Format("alert('{0}');", str);
            returnText += string.Format("location.href='{0}';", url);
            returnText += string.Format("</script>");
            HttpContext.Current.Response.Write(returnText);
        } 
        #endregion

        /// <summary>
        /// 메세지 출력 후 닫기
        /// </summary>
        /// <param name="str">메세지</param>
        #region public static void JavascriptMessageAndClose(string str)
        public static void JavascriptMessageAndClose(string str)
        {
            string returnText = string.Empty;
            returnText += string.Format("<script type='text/javascript'>");
            returnText += string.Format("alert('{0}');", str);
            returnText += string.Format("window.close();");
            returnText += string.Format("</script>");
            HttpContext.Current.Response.Write(returnText);
        } 
        #endregion

        /// <summary>
        /// 해당 문자열 자르기
        /// </summary>
        /// <param name="str">문자열</param>
        /// <param name="max">문자갯수</param>
        /// <returns>자른후 문자열</returns>
        #region public static string GetSplitString(string str, int max)
        public static string GetSplitString(string str, int max)
        {
            string split_string1 = "";
            string s = str.ToString(); //null이면 ""리턴
            if (s == "")
                return split_string1;
            string str1 = str.Trim().ToString();
            int count = 0;
            char[] chrarr = str1.ToCharArray();

            if (str1.Length != 0)
            {
                //while(count <= max || i < chrarr.Length )
                for (int i = 0; i < chrarr.Length; i++)
                {
                    int temp = Convert.ToInt32(chrarr[i]);
                    if (temp < 0 || temp >= 128)
                        count = count + 2; // 한글일 경우 2바이트
                    else
                        count = count + 1;

                    if (count <= max)
                        split_string1 = split_string1 + str1.Substring(i, 1);
                    else
                    {
                        split_string1 = split_string1 + "..";
                        break;
                    }
                }
            }
            return split_string1;
        } 
        #endregion

        /// <summary>
        /// 문자길이 알아내기
        /// </summary>
        /// <param name="a_str">문자열</param>
        /// <returns></returns>
        #region public static int GetStringLength(string a_str)
        public static int GetStringLength(string a_str)
        {
            UnicodeEncoding enc = new UnicodeEncoding();
            byte[] lbt_bytes = enc.GetBytes(a_str);

            int len = 0;
            foreach (byte bt in lbt_bytes)
            {
                if (bt != 0) len++;
            }
            return len;
        } 
        #endregion

        /// <summary>
        /// 날짜형식인지 알아내기
        /// </summary>
        /// <param name="str">날짜</param>
        /// <returns>true, false</returns>
        #region public static bool CheckDateType(string str)
        public static bool CheckDateType(string str)
        {
            try
            {
                DateTime.Parse(str);
            }
            catch
            {
                return false;
            }

            return true;
        } 
        #endregion

        /// <summary>
        /// 폴더명으로 안에 이미지 파일 체크하여 중복안일어나게 하기
        /// </summary>
        /// <param name="Path">경로</param>
        /// /// <param name="FileName">원래파일명</param>
        /// <returns>파일명</returns>
        #region public string MakeImageFileName(string Path, string FileName)
        public static string MakeImageFileName(string Path, string FileName)
        {
            string[] arrfilename = FileName.Split('.');

            string filename = string.Empty;
            DirectoryInfo dir = new DirectoryInfo(Path);
            if (dir.Exists)
            {
                filename = (dir.GetFiles().Length).ToString();
            }
            return filename + "." + arrfilename[arrfilename.Length-1];
        } 
        #endregion

        /// <summary>
        /// 파일 저장한다. 파일명은 자동생성 하여 중복을 피한다.
        /// </summary>
        /// <param name="Path">경로</param>
        /// <param name="file">파일객체</param>
        /// <returns>저장되어진 파일명( 실패시에는 빈값을 보낸다.) </returns>
        public static string FileSave(string Path, HttpPostedFile file)
        {
            try
            {
                string fileName = MakeImageFileName(Path, file.FileName);
                file.SaveAs(Path + fileName);
                return fileName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 썸네일 이미지만들기(가로길이 주면 비율로 자동으로 썸네일 만듬)
        /// </summary>
        /// <param name="path">경로</param>
        /// <param name="filename">파일명</param>
        /// <param name="thumbName">저장될 이름(확장자포함되어야함)</param>
        /// <param name="width">가로길이</param>
        /// <returns>성공여부</returns>
        #region public bool MakeThumbnail(string path, string filename,string thumbName, int width)
        public static bool MakeThumbnail(string path, string filename, string thumbName, int width)
        {
            bool returnBool = true;
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(path + filename);
                int height = (width * image.Height) / image.Width; //종횡비 유지한 웹에 출력될 높이

                System.Drawing.Image thumbnail = new Bitmap(width, height);
                System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(thumbnail);
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;

                graphic.DrawImage(image, 0, 0, width, height);

                System.Drawing.Imaging.ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
                EncoderParameters encoderParameters = new EncoderParameters(1);

                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

                thumbnail.Save(path + thumbName, info[1], encoderParameters);
                image.Dispose();
            }
            catch (Exception ex)
            {
                TomochanError.WriteErrorLog(ex);
                returnBool = false;
            }
            return returnBool;
        }
        /// <summary>
        /// 썸네일 이미지만들기(가로길이, 세로길이 만큼 나머진 자름)
        /// </summary>
        /// <param name="path">경로</param>
        /// <param name="filename">파일명</param>
        /// <param name="thumbName">저장될 이름(확장자포함되어야함)</param>
        /// <param name="width">가로길이</param>
        /// <param name="height">세로길이</param>
        /// <returns>성공여부</returns>
        public static bool MakeThumbnail(string path, string filename, string thumbName, int width, int height)
        {
            bool returnBool = true;
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(path + filename);
                System.Drawing.Imaging.ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

                System.Drawing.Image thumbnail = Crop(image, width, height, 0, 0);
                thumbnail.Save(path + thumbName, info[1], encoderParameters);

                image.Dispose();
            }
            catch (Exception ex)
            {
                TomochanError.WriteErrorLog(ex);
                returnBool = false;
            }
            return returnBool;
        }
        #endregion

        /// <summary>
        /// 특정 위치의 이미지를 자른다.
        /// </summary>
        /// <param name="imgPhoto">이미지 사진</param>
        /// <param name="Width">넓이</param>
        /// <param name="Height">높이</param>
        /// <param name="adjustX">X축 시작점</param>
        /// <param name="adjustY">Y축 시작점</param>
        /// <returns></returns>
        #region private static Image Crop(Image imgPhoto, int Width, int Height, int adjustX, int adjustY)
        private static Image Crop(Image imgPhoto, int Width, int Height, int adjustX, int adjustY)
        {
            //비트맵 종이한장 만들기
            Bitmap bmPhoto = new Bitmap(Width - adjustX, Height - adjustY, PixelFormat.Format24bppRgb);
            //그래픽 이미지 설정
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            int OriginalWidth = imgPhoto.Width;
            int OriginalHeight = imgPhoto.Height;
            //싹뚝 자르기
            grPhoto.DrawImage(imgPhoto,
                   new Rectangle(0, 0, Width, Height),
                   new Rectangle(adjustX, adjustY, Width, Height),
                   GraphicsUnit.Pixel);

            grPhoto.Dispose();
            bmPhoto.MakeTransparent(Color.Black);
            return bmPhoto;
        }
        #endregion

        /// <summary>
        /// 파일삭제
        /// </summary>
        /// <param name="path">경로</param>
        /// <param name="filename">파일명</param>
        /// <returns>성공여부</returns>
        #region public bool FileDelete(string path, string filename)
        public static bool FileDelete(string path, string filename)
        {
            bool returnBool = true;
            try
            {
                FileInfo file = new FileInfo(path + filename);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                TomochanError.WriteErrorLog(ex);
                returnBool = false;
            }
            return returnBool;
        } 
        #endregion

        /// <summary>
        /// 해당 디렉토리의 파일 및 디렉토리 삭제
        /// </summary>
        /// <param name="path">경로</param>
        /// <returns>성공여부</returns>
        #region public bool DirectoryFileDelete(string path)
        public static bool DirectoryFileDelete(string path)
        {
            bool returnBool = true;
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                if (dir.Exists)
                {
                    FileInfo[] files = dir.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        if (file.Exists)
                            file.Delete();
                    }
                    dir.Delete(true);
                }
            }
            catch (Exception ex)
            {
                TomochanError.WriteErrorLog(ex);
                returnBool = false;
            }
            return returnBool;
        } 
        #endregion

        /// <summary>
        /// Request값들 받기[인젝션에 대비해 ' , --  무자열 대체 하여 받음. get,post 값 상관없이 다 받음.
        /// </summary>
        /// <param name="ParameterName">파라미터명</param>
        /// <returns>value</returns>
        #region public static string RequestParameterCheck(string ParameterName)
        public static string RequestParameterCheck(string ParameterName)
        {
            HttpRequest Request = HttpContext.Current.Request;
            string returnValue = string.Empty;
            if (Request.Form[ParameterName] != null)
            {
                if (!Request.Form[ParameterName].Equals("'") && !Request.Form[ParameterName].Equals("--"))
                {
                    returnValue = HttpUtility.UrlDecode(Request.Form[ParameterName]);
                }
            }
            if (Request.QueryString[ParameterName] != null)
            {
                if (!Request.QueryString[ParameterName].Equals("'") && !Request.QueryString[ParameterName].Equals("--"))
                {
                    returnValue = HttpUtility.UrlDecode(Request.QueryString[ParameterName]);
                }
            }
            return returnValue;
        }
        #endregion

        private static byte[] iv = { 11, 29, 51, 112, 210, 78, 98, 186 };
        private static byte[] key = { 57, 129, 125, 118, 233, 60, 13, 94, 153, 156, 188, 9, 109, 20, 138, 7, 31, 221, 223, 91, 241, 82, 254, 189 };

        /// <summary>
        /// 암호화
        /// </summary>
        /// <param name="str">암호시킬 문자열</param>
        /// <returns></returns>
        #region static public string Encryption(string str)
        public static string Encryption(string str)
        {
            string encryptStr = string.Empty;
            byte[] bytIn = null;
            byte[] bytOut = null;
            MemoryStream ms = null;
            TripleDESCryptoServiceProvider tcs = null;
            ICryptoTransform ct = null;
            CryptoStream cs = null;
            try
            {
                bytIn = System.Text.Encoding.UTF8.GetBytes(str);
                ms = new MemoryStream();
                tcs = new TripleDESCryptoServiceProvider();
                ct = tcs.CreateEncryptor(key, iv);
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                bytOut = ms.ToArray();
                encryptStr = System.Convert.ToBase64String(bytOut, 0, bytOut.Length);
            }
            catch (Exception ex)
            {
                TomochanError.WriteErrorLog(ex);
            }
            finally
            {
                if (cs != null) { cs.Clear(); cs = null; }
                if (ct != null) { ct.Dispose(); ct = null; }
                if (tcs != null) { tcs.Clear(); tcs = null; }
                if (ms != null) { ms = null; }
            }
            return encryptStr;
        }
        #endregion

        /// <summary>
        /// 암호화된 문자열을 복구화시킴
        /// </summary>
        /// <param name="str">암호화된 문자열</param>
        /// <returns></returns>
        #region public static string Decryption(string str)
        public static string Decryption(string str)
        {
            string decryptStr = string.Empty;
            byte[] bytIn = null;
            MemoryStream ms = null;
            TripleDESCryptoServiceProvider tcs = null;
            CryptoStream cs = null;
            ICryptoTransform ct = null;
            StreamReader sr = null;
            try
            {
                bytIn = System.Convert.FromBase64String(str);
                ms = new MemoryStream(bytIn, 0, bytIn.Length);
                tcs = new TripleDESCryptoServiceProvider();
                ct = tcs.CreateDecryptor(key, iv);
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Read);
                sr = new StreamReader(cs);
                decryptStr = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                TomochanError.WriteErrorLog(ex);
            }
            finally
            {
                if (sr != null) { sr.Close(); sr = null; }
                if (cs != null) { cs.Clear(); cs = null; }
                if (ct != null) { ct.Dispose(); ct = null; }
                if (tcs != null) { tcs.Clear(); tcs = null; }
                if (ms != null) { ms.Close(); ms = null; }
            }
            return decryptStr;
        }
        #endregion

        /// <summary>
        /// 해당 파일을 다운로드 합니다.
        /// </summary>
        /// <param name="path">/경로/파일명</param>
        public static void Download(string path, string downname)
        {
            string fileName = path + downname;
            string name = downname;
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                int BUFFER_SIZE = 1024 * 8;
                long fileSize = fs.Length;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                HttpContext.Current.Response.ContentType = "application/unknown";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "inline; filename=" + HttpUtility.UrlEncode(name, Encoding.UTF8));
                HttpContext.Current.Response.AddHeader("Content-Length", fileSize.ToString());
                long len = 0;
                long off = 0;
                byte[] bytes = new byte[BUFFER_SIZE];
                while (off < fileSize)
                {
                    len = ((off + BUFFER_SIZE) > fileSize) ? (fileSize - off) : BUFFER_SIZE;
                    fs.Read(bytes, 0, (int)len);
                    off += len;
                    HttpContext.Current.Response.BinaryWrite(bytes);
                    HttpContext.Current.Response.Flush();
                }
            }
            catch (HttpException exce)
            {
                TomochanError.WriteErrorLog(exce.Message);
                fs.Close();
            }
            finally
            {
                fs.Close();
            }
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// Excel 저장
        /// </summary>
        /// <param name="fileName">저장할 파일명 확장자는 필요없음.</param>
        /// <param name="dt">데이터 테이블</param>
        /// <param name="headerList">헤더명(단, 데이터 테이블의 칼럼 갯수와 동일하여야함. 순번도 동일하여야함)</param>
        #region public static void CreateExcelDownload(string fileName, DataTable dt, ArrayList headerList)
        public static void CreateExcelDownload(string fileName, DataTable dt, ArrayList headerList)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //header 만들기
                for (int i = 0; i < headerList.Count; i++)
                {
                    if (i != 0)
                    {
                        sb.Append(string.Format(",{0}", headerList[i]));
                    }
                    else
                    {
                        sb.Append(string.Format("{0}", headerList[i]));
                    }
                }
                sb.Append(string.Format("{0}", "\r\n"));
                // //header 만들기

                int coulmnCount = dt.Columns.Count;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < coulmnCount; j++)
                    {
                        if (i != 0)
                        {
                            sb.Append(string.Format(",{0}", dt.Rows[i][j]));
                        }
                        else
                        {
                            sb.Append(string.Format("{0}", dt.Rows[i][j]));
                        }
                    }
                    sb.Append(string.Format("{0}", "\r\n"));
                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.Charset = "euc-kr";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("euc-kr");
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("inline; filename={0}.csv", fileName));
                HttpContext.Current.Response.Write(sb.ToString());
                HttpContext.Current.Response.Flush();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion
    }
}
