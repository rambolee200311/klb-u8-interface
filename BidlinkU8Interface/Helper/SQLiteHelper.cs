using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace BidlinkU8Interface.Helper
{
    public class SQLiteHelper
    {
        public String ConnetionString = null;


        //public SQLiteHelper(ILogger<SQLiteHelper> Log)
        //{
        //    _logger = Log;
        //}

        public void buildConnetionString(string connetStr)
        {

            ConnetionString = connetStr;
        }

        public bool IsConnect()
        {

            using (SqlConnection conn = new SqlConnection(ConnetionString))
            {
                try
                {
                    conn.Open();
                    //_logger.LogDebug("连接成功！");
                    return true;
                }
                catch (SqlException ex)
                {
                    // _logger.LogError("连接失败！", ex);
                    //_logger.AddTrack("连接失败！", ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 连接数据库，并选择数据，显示在dataset
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="ConnetStr">连接字符串</param>
        /// <returns></returns>
        public DataSet SelectData(string sqlStr)
        {
            //_logger.LogDebug("SelectData Begin");
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnetionString))
                {
                    SqlDataAdapter sda = new SqlDataAdapter(sqlStr, conn);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    //_logger.LogDebug("SelectData End");
                    return ds;
                }
            }
            catch (Exception SqlEx)
            {
                //_logger.LogError("执行Sql异常！", SqlEx);
                //_logger.AddError("HOTApi.Lib.SqlHelper.SelectData", "执行Sql异常：" + SqlEx.Message);
                throw SqlEx;
            }
        }

        /// <summary>
        /// 连接数据库，并选择数据，显示在dataset
        /// </summary>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="ConnetStr">连接字符串</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public DataSet SelectData(string sqlStr, params SqlParameter[] parameters)
        {
            //_logger.LogDebug("selectData Begin");
            using (SqlConnection conn = new SqlConnection(ConnetionString))
            {
                conn.Open();
                DataSet ds = new DataSet();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = sqlStr;
                        cmd.Parameters.AddRange(parameters);
                        //_logger.LogDebug("执行sql："+cmd.CommandText.ToString());
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);//将适配器内容填充到dataset
                        //_logger.LogDebug("selectData End");
                        return ds;
                    }
                    catch (SqlException SqlEx)
                    {
                        //_logger.AddError("执行sql出错：", sqlex.Message);
                        //_logger.AddTrack("HOTApi.Lib.selectData", "End");
                        //_logger.LogError("执行Sql出错！", SqlEx);
                        throw SqlEx;
                    }
                }
            }
        }

        /// <summary>
        /// 从数据库中修改数据（含增、删、查、改），可重载使用参数化
        /// </summary>
        /// <returns>受影响的行数</returns>
        public int ChangeData(string sqlStr)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnetionString))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sqlStr;
                    int i = cmd.ExecuteNonQuery();
                    return i;
                }
            }
            catch (SqlException SqlEx)
            {
                //_logger.LogError("执行Sql出错！", SqlEx);
                throw SqlEx;
            }
        }

        /// <summary>
        /// 重载函数：
        /// 修改数据库数据，采用参数化执行，示例如下
        /// SqlParameter[] par = {new SqlParameter("@n", name),//关联
        ///                        new SqlParameter("@pwd", pwd)
        ///                        };
        ///string sql = "select count(*) from dbo.student  where studentno =  "+ name +" and loginpwd = '"+pwd +"'";
        ///SqlCommand cmd = new SqlCommand(sql, con);
        ///cmd.Parameters.AddRange(par);//添加参数化数组到cmd中
        /// </summary>
        /// <param name="sqlStr">需要执行的sql语句，含参</param>
        /// <param name="ConnetStr">连接字符串</param>
        /// <param name="parameters">参数化数组</param>
        /// <returns></returns>
        public int ChangeData(string sqlStr, params SqlParameter[] parameters)
        {
            //_logger.LogDebug("changeData Begin");
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnetionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlStr, conn);//conn.CreateCommand();
                    cmd.Parameters.AddRange(parameters);// 添加参数化数组到cmd中
                    //_logger.LogDebug("执行sql：" + cmd.CommandText);
                    int i = cmd.ExecuteNonQuery();
                    //_logger.LogDebug("changeData End");
                    return i;
                }
            }
            catch (SqlException SqlEx)
            {
                //_logger.AddError("HOTApi.Lib.SqlServerHelper.changeData", "执行Sql修改异常：" + SqlEx.Message);
                //_logger.LogError("执行Sql出错！", SqlEx);
                throw SqlEx;
            }
        }
    }
}