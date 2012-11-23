using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Collections.Generic;

namespace TomochanClass
{
    /// <summary>
    /// MS-SQL 핸들링을 위한.
    /// DB Handler
    /// </summary>
    public class TomochanData
    {
        private static string ConnectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;

        /// <summary>
        /// SQL 실행후 영향받은 행의 수를 반환
        /// </summary>
        /// <param name="param">파라미터</param>
        /// <param name="spName">프로시저 네임</param>
        /// <returns>영향받은 행수</returns>
        #region public static int ExecuteNonQuery(object[] param, string spName)
        public static int ExecuteNonQuery(object[] param, string spName)
        {
            DbConnectionLog(param, spName);
            int row = 0;
            try
            {
                row = SqlHelper.ExecuteNonQuery(ConnectionString, spName, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }
        #endregion

        /// <summary>
        /// SQL 실행후 영향받은 행의 수를 반환
        /// </summary>
        /// <param name="strQuery">쿼리</param>
        /// <returns>영향받은 행수</returns>
        #region public static int ExecuteNonQuery(string strQuery)
        public static int ExecuteNonQuery(string strQuery)
        {
            DbConnectionLog(strQuery);
            int row = 0;
            try
            {
                row = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }
        #endregion


        /// <summary>
        /// SQL 트랜젝션으로 실행후 영향받은 행의 수를 반환(같은 sp)
        /// </summary>
        /// <param name="list">파라미터 값들 List 집합</param>
        /// <param name="spName">프로시저 네임</param>
        /// <returns>영향받은 행수</returns>
        #region public static int ExecuteNonQuery(List<object[]> list, string spName)
        public static int ExecuteNonQuery(List<object[]> list, string spName)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            int row = 0;
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                for (int i = 0; i < list.Count; i++)
                {

                    object[] param = list[i];
                    for (int j = 0; j < list[i].Length; j++)
                    {
                        param[j] = list[i][j];
                    }

                    row += SqlHelper.ExecuteNonQuery(tran, spName, param);
                }
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            con.Close();
            return row;
        }
        #endregion

        /// <summary>
        /// SQL 트랜젝션으로 실행후 영향받은 행의 수를 반환(여러sp)
        /// </summary>
        /// <param name="list">파라미터 값들 List 집합</param>
        /// <param name="spNameList">프로시저 명 List 집합</param>
        /// <returns>영향받은 행수</returns>
        #region public static int ExecuteNonQuery(List<object[]> list, List<string> spNameList)
        public static int ExecuteNonQuery(List<object[]> list, List<string> spNameList)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            int row = 0;
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                for (int i = 0; i < list.Count; i++)
                {

                    object[] param = list[i];
                    for (int j = 0; j < list[i].Length; j++)
                    {
                        param[j] = list[i][j];
                    }

                    row += SqlHelper.ExecuteNonQuery(tran, spNameList[i], param);
                }
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            con.Close();
            return row;
        }
        #endregion

        /// <summary>
        /// DataSet으로 반환 (프로시저)
        /// Return To DataSet (Procedure)
        /// </summary>
        /// <param name="param">파라미터</param>
        /// <param name="spName">프로시저 네임</param>
        /// <returns>DataSet</returns>
        #region public static DataSet GetDataSet(object[] param, string spName)
        public static DataSet GetDataSet(object[] param, string spName)
        {
            DbConnectionLog(param, spName);
            DataSet ds = null;
            try
            {
                ds = SqlHelper.ExecuteDataset(ConnectionString, spName, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        } 
        #endregion

        /// <summary>
        /// DataSet으로 반환 (인라인쿼리)
        /// Return To DataSet (Inline Query)
        /// </summary>
        /// <param name="strQuery">쿼리</param>
        /// <returns>DataSet</returns>
        #region public static DataSet GetDataSet(string strQuery)
        public static DataSet GetDataSet(string strQuery)
        {
            DbConnectionLog(strQuery);
            DataSet ds = null;
            try
            {
                ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        #endregion

        /// <summary>
        /// DataTable로 반환(프로시저)
        /// Return DataTable(Procedure)
        /// </summary>
        /// <param name="param">파라미터</param>
        /// <param name="spName">프로시저 네임</param>
        /// <returns>DataTable</returns>
        #region public static DataTable GetDataTable(object[] param, string spName)
        public static DataTable GetDataTable(object[] param, string spName)
        {
            DbConnectionLog(param, spName);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, spName, param);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        } 
        #endregion

        /// <summary>
        /// DataTable로 반환(인라인 쿼리)
        /// Return DataTable(Inline Query)
        /// </summary>
        /// <param name="strQuery">Query</param>
        /// <returns>DataTable</returns>
        #region public static DataTable GetDataTable(string strQuery)
        public static DataTable GetDataTable(string strQuery)
        {
            DbConnectionLog(strQuery);
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strQuery);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt; 
        }
        #endregion

        /// <summary>
        /// 첫번째 행 반환
        /// </summary>
        /// <param name="param">파라미터</param>
        /// <param name="spName">프로시저 네임</param>
        /// <returns>DataRow</returns>
        #region public static DataRow GetFirstRow(object[] param, string spName)
        public static DataRow GetFirstRow(object[] param, string spName)
        {
            DbConnectionLog(param, spName);
            DataRow row = null;
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConnectionString,spName, param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    row = ds.Tables[0].Rows[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        } 
        #endregion

        /// <summary>
        /// 첫번째 행 반환
        /// </summary>
        /// <param name="strQuery">프로시저 네임</param>
        /// <returns>DataRow</returns>
        #region public static DataRow GetFirstRow( string strQuery)
        public static DataRow GetFirstRow(string strQuery)
        {
            DbConnectionLog(strQuery);
            DataRow row = null;
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, strQuery);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    row = ds.Tables[0].Rows[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }
        #endregion

        /// <summary>
        /// 첫행 해당 컬럼 반환
        /// </summary>
        /// <param name="param">파라미터</param>
        /// <param name="spName">프로시저 네임</param>
        /// <param name="columNm">칼럼명</param>
        /// <returns>string</returns>
        #region public static string GetOneColumn(object[] param, string spName, string columNm)
        public static string GetOneColumn(object[] param, string spName, string columNm)
        {
            DbConnectionLog(param, spName);
            string returnText = string.Empty;
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConnectionString,  spName, param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    returnText = ds.Tables[0].Rows[0][columNm].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnText;
        } 
        #endregion

        /// <summary>
        /// 첫행 해당 컬럼 반환
        /// </summary>
        /// <param name="strQuey">쿼리</param>
        /// <param name="columNm">칼럼명</param>
        /// <returns>string</returns>
        #region public static string GetOneColumn(string strQuey, string columNm)
        public static string GetOneColumn(string strQuey, string columNm)
        {
            DbConnectionLog(strQuey);
            string returnText = string.Empty;
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text,strQuey);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    returnText = ds.Tables[0].Rows[0][columNm].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnText;
        }
        #endregion

        /// <summary>
        /// 첫행 첫번째 컬럼 반환
        /// </summary>
        /// <param name="param">파라미터</param>
        /// <param name="spName">프로시저 네임</param>
        /// <returns>string</returns>
        #region public static string GetOneColumn(object[] param, string spName)
        public static string GetOneColumn(object[] param, string spName)
        {
            DbConnectionLog(param, spName);
            string returnText = string.Empty;
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, spName, param);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    returnText = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnText;
        }
        #endregion

        /// <summary>
        /// 첫행 첫번째 컬럼 반환
        /// </summary>
        /// <param name="strQuey">쿼리</param>
        /// <returns>string</returns>
        #region public static string GetOneColumn(string strQuey)
        public static string GetOneColumn(string strQuey)
        {
            DbConnectionLog(strQuey);
            string returnText = string.Empty;
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text ,strQuey);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    returnText = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnText;
        }
        #endregion

        /// <summary>
        /// Db Connection Log
        /// </summary>
        private static void DbConnectionLog(object[] param, string spName)
        {
            string logYn = ConfigurationManager.AppSettings["dblog"];
            if (logYn == "Y")
            {
                string sql = string.Format("EXEC {0}", spName);
                for (int i = 0; i < param.Length; i++)
                {
                    if (i == 0)
                    {
                        sql += string.Format(" '{0}'", param[i]);
                    }
                    else
                    {
                        sql += string.Format(" ,'{0}'", param[i]);
                    }
                }
                TomochanError.WriteErrorLog("C:\\tomochanDbLog\\", sql);
            }
        }

        private static void DbConnectionLog(string qry)
        {
            string logYn = ConfigurationManager.AppSettings["dblog"];
            TomochanError.WriteErrorLog("C:\\tomochanDbLog\\", qry);
        }
    }
}
