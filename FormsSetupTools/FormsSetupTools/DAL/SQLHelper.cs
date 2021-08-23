using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public static class SQLHelper
    {
        private static SqlConnection DefaultDBCon;

        static SQLHelper()
        {
            DefaultDBCon = new SqlConnection();
            DefaultDBCon.ConnectionString = ConfigurationManager.ConnectionStrings["sqs"].ToString();
        }

        #region Exposed Methods

        public static SqlParameter[] AddParameter(ref SqlParameter[] arrParam, SqlParameter newParam)
        {
            try
            {
                Array.Resize(ref arrParam, arrParam.Length + 1);
                arrParam[arrParam.Length - 1] = newParam;
            }
            catch (Exception ex)
            {
            }
            return arrParam;
        }

        public static DataSet GetDatasetByQuery(string query, SqlParameter[] param)
        {
            DataSet dsResult = null;

            try
            {
                dsResult = GetResultSet(query, CommandType.Text, param, DefaultDBCon);
            }
            catch (Exception ex)
            {
            }
            return dsResult;
        }

        public static DataSet GetDatasetByQuery(string query, SqlParameter[] param, SqlConnection DBCon)
        {
            DataSet dsResult = null;

            try
            {
                dsResult = GetResultSet(query, CommandType.Text, param, DBCon);
            }
            catch (Exception ex)
            {
            }
            return dsResult;
        }

        public static DataSet GetDatasetBySP(string query, SqlParameter[] param)
        {
            DataSet dsResult = null;

            try
            {
                dsResult = GetResultSet(query, CommandType.StoredProcedure, param, DefaultDBCon);
            }
            catch (Exception ex)
            {
            }
            return dsResult;
        }

        public static DataSet GetDatasetBySP(string query, SqlParameter[] param, SqlConnection DBCon)
        {
            DataSet dsResult = null;

            try
            {
                dsResult = GetResultSet(query, CommandType.StoredProcedure, param, DBCon);
            }
            catch (Exception ex)
            {
            }
            return dsResult;
        }

        public static int ExecuteNonQuery(string query, SqlParameter[] param)
        {
            int noOfRowsEffected = -1;
            try
            {
                noOfRowsEffected = ExecuteNonQuery(query, CommandType.StoredProcedure, param, DefaultDBCon);
            }
            catch (Exception ex)
            {
            }
            return noOfRowsEffected;
        }

        public static int ExecuteNonQuery(string query, SqlParameter[] param, SqlConnection DBCon)
        {
            int noOfRowsEffected = -1;
            try
            {
                noOfRowsEffected = ExecuteNonQuery(query, CommandType.StoredProcedure, param, DBCon);
            }
            catch (Exception ex)
            {
            }
            return noOfRowsEffected;
        }

        #endregion

        #region Private Methods

        private static bool Connect(SqlConnection DBCon)
        {
            try
            {
                if (DBCon.State == ConnectionState.Closed)
                {
                    DBCon.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool Disconnect(SqlConnection DBCon)
        {
            try
            {
                if (DBCon.State == ConnectionState.Open)
                {
                    DBCon.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static DataSet GetResultSet(string queryOrSP, CommandType cmdType, SqlParameter[] param, SqlConnection DBCon)
        {
            DataSet dsResult = new DataSet();
            using (SqlCommand SQLCmd = new SqlCommand())
            {

                SQLCmd.Connection = DBCon;
                SQLCmd.CommandText = queryOrSP;
                SQLCmd.CommandType = cmdType;
                SQLCmd.Parameters.AddRange(param);
                Connect(DBCon);

                SqlDataAdapter SQLDA = new SqlDataAdapter(SQLCmd);
                SQLDA.Fill(dsResult);
                Disconnect(DBCon);
            }

            return dsResult;
        }

        private static int ExecuteNonQuery(string queryOrSP, CommandType cmdType, SqlParameter[] param, SqlConnection DBCon)
        {
            int noOfRowsEffected = -1;
            using (SqlCommand SQLCmd = new SqlCommand())
            {

                SQLCmd.Connection = DBCon;
                SQLCmd.CommandText = queryOrSP;
                SQLCmd.CommandType = cmdType;
                SQLCmd.Parameters.AddRange(param);
                Connect(DBCon);

                noOfRowsEffected = SQLCmd.ExecuteNonQuery();
                Disconnect(DBCon);
            }

            return noOfRowsEffected;
        }

        #endregion
    }
}
