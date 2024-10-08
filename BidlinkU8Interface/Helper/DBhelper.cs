using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADODB;
namespace BidlinkU8Interface.Helper
{
    public class DBhelper
    {
        public static string getDataFromSql(string ConnStr, string Sql)
        {
            string DataResult = "";
            ADODB.Connection conn = new Connection();
            conn.ConnectionString = ConnStr;
            ADODB.Recordset rst = new Recordset();
            try
            {
                conn.Open();
                rst.Open(Sql, conn, CursorTypeEnum.adOpenDynamic, LockTypeEnum.adLockOptimistic);
                if (rst != null)
                {
                    if (!rst.EOF)
                    {
                        //rst.MoveNext();
                        DataResult = rst.Fields[0].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                DataResult = ex.Message;
                Log.AddError("DBhelper.getDataFromSql()", ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return DataResult;
        }

        public static string setDataFromSql(string ConnStr, string Sql)
        {
            string DataResult = "";
            object objOut = null;
            ADODB.Connection conn = new Connection();
            try
            {
                conn.ConnectionString = ConnStr;
                ADODB.Command cmd = new Command();
                conn.Open();
                cmd.ActiveConnection = conn;
                cmd.CommandType = CommandTypeEnum.adCmdText;
                cmd.CommandText = Sql;
                DataResult = cmd.Execute(out objOut).ToString();
            }
            catch (Exception ex)
            {
                DataResult = ex.Message;
                Log.AddError("DBhelper.setDataFromSql()", ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return DataResult;
        }

        public static decimal Round(decimal d1)//四舍五入
        {
            return Math.Round(d1, 2, MidpointRounding.AwayFromZero);
        }
    }
}