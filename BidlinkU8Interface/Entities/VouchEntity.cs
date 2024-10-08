using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using BidlinkU8Interface.Models;
using BidlinkU8Interface.Models.GLVouch;
using BidlinkU8Interface.Models.XMLVouch;
using BidlinkU8Interface.Helper;
using BidlinkU8Interface.App_Start;
using System.Data;
using System.Data.SqlClient;

namespace BidlinkU8Interface.Entities
{
    public class VouchEntity
    {
       //cvoucher 凭证
        public static Result addDBVouch(Result re, GLVoucherReq vouchreq, string datetime, String from_account, String to_account, String app_key, String token, String tradeid, int ds_sequence, SQLiteHelper site)
        {
            
            Result result = re;
            /*
            string AccID = "";//账套
            string User = "";//用户名
            string Password = "";//密码
            string Server = "";//服务器
            string Year = "";//年
            string Month = "";//月
            string Date = "";//日
            string Vendor = "";//供应商
            string Cust = "";//客户
            string Dept = "";//部门
            string Person = "";//业务员
            string ItemClass = "";
            string ItemCode = "";
            string Type = "";
            string strSql = "";
            string cdefine1 = "";
            string cdefine2 = "";
            string cdefine3 = "";
            decimal natural_currency = 0;
            decimal primary_amount = 0;
            string coutno_id = "";
            int attachment_number = 0;
            int Inid = 0;
            ADODB.Connection conn = new ADODB.Connection();
            Log.AddInfo("VouchEntity.addDBVouch()", "begin");
            object objOut = null;
            string strResult = "";
            try
            {
                AccID = HOTConfig.GetConfig().GetAccID(ds_sequence);
                User = HOTConfig.GetConfig().GetUser(ds_sequence);
                Password = HOTConfig.GetConfig().GetPassword(ds_sequence);
                Server = HOTConfig.GetConfig().GetServer(ds_sequence);
                
                Date = Convert.ToDateTime(vouchreq.voucher.date).ToShortDateString(); //DateTime.Now.ToShortDateString();    2019-11-21修改凭证日期
                if (vouchreq.voucher.accounting_period != null)
                {
                    Month = vouchreq.voucher.accounting_period.ToString();
                }
                else
                {
                    Month = Convert.ToDateTime(vouchreq.voucher.date).Month.ToString();
                }
                if (vouchreq.voucher.fiscal_year != null)
                {
                    Year = vouchreq.voucher.fiscal_year.ToString();
                }
                else
                {
                    Year = Convert.ToDateTime(vouchreq.voucher.date).Year.ToString();
                }
                if (vouchreq.voucher.attachment_number != null)
                {
                    attachment_number = vouchreq.voucher.attachment_number;
                }
                string strGuid = Guid.NewGuid().ToString("N");
                if (vouchreq.voucher.reserve2 != null)
                {
                    coutno_id = vouchreq.voucher.reserve2;
                }
                else
                {
                    coutno_id = strGuid;
                }
                U8Login.clsLogin m_login = new U8Login.clsLogin();
                Log.AddInfo("VouchEntity.addDBVouch()", "begin login");
                if (m_login.Login("AA", AccID, Year, User, Password, Date, Server, null))
                { 
                    strResult = "login success!";
                    Log.AddInfo("VouchEntity.addDBVouch()", strResult);
                }
                else
                {
                    strResult = "login failed! User=" + User + ";Server=" + Server + ";AccID=" + AccID;
                    Log.AddError("VouchEntity.addDBVouch()", strResult);
                    result.errcode = "1111";
                    result.errmsg = strResult;
                    result.id = "";
                    return result;
                }
                //2 创建组件
                CVoucher.CVInterface obj = new CVoucher.CVInterface();
                conn.Open(m_login.UfDbName);

                //3 创建临时表
                //string strSql = "";
                obj.strTempTable = "tempdb..cus_gl_accvouch_" + strGuid;
                strSql = "CREATE TABLE " + obj.strTempTable + " "
                        + "(csign nvarchar(28),ino_id smallint, "
                        + "inid smallint, cbill nvarchar(80), doutbilldate DATETIME, ccashier nvarchar(80), "
                        + "idoc smallint default 0, ctext1 nvarchar(50), ctext2 nvarchar(50), cexch_name nvarchar(28), "
                        + "cdigest nvarchar(1000) null, ccode nvarchar(40), md money default 0, mc money default 0, "
                        + "md_f money default 0, mc_f money default 0, nfrat float default 0, nd_s float default 0, nc_s float default 0, csettle nvarchar(23), "
                        + "cn_id nvarchar(30), dt_date DATETIME, cdept_id nvarchar(12), cperson_id nvarchar(80), ccus_id nvarchar(80), csup_id nvarchar (20), "
                        + "citem_id nvarchar(80), citem_class nvarchar(22), cname nvarchar(40), ccode_equal nvarchar(50), "
                        + "bvouchedit bit default 0, bvouchaddordele bit default 0, bvouchmoneyhold bit default 0, bvalueedit bit default 0, bcodeedit bit default 0, ccodecontrol nvarchar(50), bPCSedit bit default 0, bDeptedit bit default 0, bItemedit bit default 0, bCusSupInput bit default 0, "
                        + "coutaccset nvarchar(23), ioutyear smallint, coutsysname nvarchar(50) NOT NULL, coutsysver nvarchar(50), ioutperiod tinyint NOT NULL, coutsign nvarchar(80) NOT NULL, coutno_id nvarchar(100) NOT NULL, doutdate DATETIME, coutbillsign nvarchar(80), coutid nvarchar(50), iflag tinyint"
                        + ",iBG_ControlResult smallint null,daudit_date DateTime NULL,cblueoutno_id nvarchar(50) NULL,bWH_BgFlag bit,cDefine1 nvarchar(40),"
                        + "cDefine2 nvarchar(40),cDefine3 nvarchar(40),cDefine4 DateTime,cDefine5 int,cDefine6 DateTime,cDefine7 Float,cDefine8 nvarchar(4),cDefine9 nvarchar(8),"
                        + "cDefine10 nvarchar(60),cDefine11 nvarchar(120),cDefine12 nvarchar(120),cDefine13 nvarchar(120),cDefine14 nvarchar(120),cDefine15 int,cDefine16 float)";
                conn.Execute(strSql, out objOut);
                //4凭证分录
                #region//4.1 借方
                    foreach (Entry glentry in vouchreq.voucher.debit.entry)
                    {
                        Vendor = "";//供应商
                        Cust = "";//客户
                        Dept = "";//部门
                        Person = "";//业务员
                        ItemClass = "";
                        ItemCode = "";
                        cdefine1 = "";
                        cdefine2 = "";
                        cdefine3 = "";
                        natural_currency = 0;
                        primary_amount = 0;
                        if (glentry.primary_debit_amount != null)
                        {
                            primary_amount = glentry.primary_debit_amount;
                        }
                        if (glentry.natural_debit_currency != null)
                        {
                            natural_currency = glentry.natural_debit_currency;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.cust_id))
                        {
                            Cust = glentry.auxiliary.cust_id;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.supplier_id))
                        {
                            Vendor = glentry.auxiliary.supplier_id;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.dept_id))
                        {
                            Dept = glentry.auxiliary.dept_id;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.personnel_id))
                        {
                            Person = glentry.auxiliary.personnel_id;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.item_class))
                        {
                            ItemClass = glentry.auxiliary.item_class;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.item_id))
                        {
                            ItemCode = glentry.auxiliary.item_id;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.self_define1))
                        {
                            cdefine1 = glentry.auxiliary.self_define1;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.self_define2))
                        {
                            cdefine2 = glentry.auxiliary.self_define2;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.self_define3))
                        {
                            cdefine3 = glentry.auxiliary.self_define3;
                        }
                        strSql = "insert into " + obj.strTempTable
                                   + "(ioutperiod,coutsign ,cSign,"
                                   + "cdigest,coutno_id,coutsysname,cbill,inid,"
                                   + "ccode,cexch_name ,doutbilldate,dt_date,bvouchedit,bvalueedit,bcodeedit,"
                                   + "md_f,md,ccus_id,idoc,"
                                   + "csup_id,cdept_id,citem_class,citem_id,cperson_id,"
                                   +"cdefine1,cdefine2,cdefine3)  " +
                                   " values(" + Month + ",N'" + vouchreq.voucher.voucher_type + "',N'" + vouchreq.voucher.voucher_type + "', '"
                                   + glentry.@abstract + "',N'" + coutno_id + "',N'',N'" + vouchreq.voucher.enter + "'," + glentry.entry_id + ",'"
                                   + glentry.account_code + "',N'人民币','" + Date + "','" + Date + "',1,1,1,"
                                   + primary_amount.ToString() + "," + natural_currency.ToString() + ",'" + Cust + "'," + attachment_number.ToString() + ",'"
                                   + Vendor + "','" + Dept + "','" + ItemClass + "','" + ItemCode + "','" + Person + "'";
                        if (string.IsNullOrEmpty(cdefine1))
                        {
                            strSql += ",null";
                        }
                        else
                        {
                            strSql += ",'" + cdefine1 + "'";
                        }
                        if (string.IsNullOrEmpty(cdefine2))
                        {
                            strSql += ",null";
                        }
                        else
                        {
                            strSql += ",'" + cdefine2 + "'";
                        }
                        if (string.IsNullOrEmpty(cdefine3))
                        {
                            strSql += ",null";
                        }
                        else
                        {
                            strSql += ",'" + cdefine3 + "'";
                        }
                        strSql += ")";
                        Log.AddInfo("VouchEntity.addDBVouch()", strSql);
                        conn.Execute(strSql, out objOut);
                    }
                #endregion
                
                #region//4.2 贷方
                    foreach (Entry glentry in vouchreq.voucher.credit.entry)
                    {
                        Vendor = "";//供应商
                        Cust = "";//客户
                        Dept = "";//部门
                        Person = "";//业务员
                        ItemClass = "";
                        ItemCode = "";
                        cdefine1 = "";
                        cdefine2 = "";
                        cdefine3 = "";
                        natural_currency = 0;
                        primary_amount = 0;
                        if (glentry.primary_debit_amount != null)
                        {
                            primary_amount = glentry.primary_debit_amount;
                        }
                        if (glentry.natural_debit_currency != null)
                        {
                            natural_currency = glentry.natural_debit_currency;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.cust_id))
                        {
                            Cust = glentry.auxiliary.cust_id;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.supplier_id))
                        {
                            Vendor = glentry.auxiliary.supplier_id;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.dept_id))
                        {
                            Dept = glentry.auxiliary.dept_id;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.personnel_id))
                        {
                            Person = glentry.auxiliary.personnel_id;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.item_class))
                        {
                            ItemClass = glentry.auxiliary.item_class;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.item_id))
                        {
                            ItemCode = glentry.auxiliary.item_id;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.self_define1))
                        {
                            cdefine1 = glentry.auxiliary.self_define1;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.self_define2))
                        {
                            cdefine2 = glentry.auxiliary.self_define2;
                        }
                        if (!string.IsNullOrEmpty(glentry.auxiliary.self_define3))
                        {
                            cdefine3 = glentry.auxiliary.self_define3;
                        }
                        strSql = "insert into " + obj.strTempTable
                                   + "(ioutperiod,coutsign ,cSign,"
                                   + "cdigest,coutno_id,coutsysname,cbill,inid,"
                                   + "ccode,cexch_name ,doutbilldate,dt_date,bvouchedit,bvalueedit,bcodeedit,"
                                   + "mc_f,mc,ccus_id,idoc,"
                                   + "csup_id,cdept_id,citem_class,citem_id,cperson_id,"
                                   +"cdefine1,cdefine2,cdefine3)  " +
                                   " values(" + Month + ",N'" + vouchreq.voucher.voucher_type + "',N'" + vouchreq.voucher.voucher_type + "', '"
                                   + glentry.@abstract + "',N'" + coutno_id + "',N'',N'" + vouchreq.voucher.enter + "'," + glentry.entry_id + ",'"
                                   + glentry.account_code + "',N'人民币','" + Date + "','" + Date + "',1,1,1,"
                                   + primary_amount.ToString() + "," + natural_currency.ToString() + ",'" + Cust + "'," + attachment_number.ToString() + ",'"
                                   + Vendor + "','" + Dept + "','" + ItemClass + "','" + ItemCode + "','" + Person + "'";
                        if (string.IsNullOrEmpty(cdefine1))
                        {
                            strSql += ",null";
                        }
                        else
                        {
                            strSql+=",'"+cdefine1+"'";
                        }
                        if (string.IsNullOrEmpty(cdefine2))
                        {
                            strSql += ",null";
                        }
                        else
                        {
                            strSql += ",'" + cdefine2 + "'";
                        }
                        if (string.IsNullOrEmpty(cdefine3))
                        {
                            strSql += ",null";
                        }
                        else
                        {
                            strSql += ",'" + cdefine3 + "'";
                        }
                        strSql += ")";
                        Log.AddInfo("VouchEntity.addDBVouch()", strSql);
                        conn.Execute(strSql, out objOut);
                    }
                    #endregion


                //6 调用保存         
                obj.set_Connection(conn);
                obj.LoginByUserToken(m_login.userToken);
                if (obj.SaveVoucher())
                {
                    strResult = "";
                    strResult = DBhelper.getDataFromSql(m_login.UfDbName, "select convert(nvarchar(4),iyear)+'年'+convert(nvarchar(4),iperiod)+'月'+csign+'-'+convert(nvarchar(4),ino_id) glcode from gl_accvouch where coutno_id='" + coutno_id + "' and isnull(iflag,0)=0");

                    result.errcode = "0";
                    result.id = strResult;
                    result.errmsg = "";
                    Log.AddInfo("VouchEntity.addDBVouch()",strResult);
                    datetime = DateTime.Now.ToString();
                    writeTradeLog("voucheradd", from_account, app_key, tradeid, datetime, "end", vouchreq.ToJson(), result.errcode, result.id, site);
                    return re;
                }
                else
                {
                    strResult = obj.strErrMessage.ToString();
                    result.errcode = "7777";
                    result.id = "";
                    result.errmsg = strResult;
                    writeTradeLog("voucheradd", from_account, app_key, tradeid, datetime, "end", vouchreq.ToJson(), result.errcode, result.errmsg, site);
                }
            }
            catch (Exception ex)
            {
                Log.AddError("VouchEntity.addDBVouch()", ex.Message);
                result.errcode = "9999";
                result.errmsg = ex.Message;
                result.id = "";
            }
            */
            return result;
        }


         //写入tradeid日志
        private static void writeTradeLog(string controller,string from_account, string app_key, string tradeid, string CreateTime, string ctype, string bodyreq, string errcode, string errmsg,SQLiteHelper site)
        {
            //Log.AddInfo("TokenController.writeTokenLog()", "Begin");
            string sqlstr = "INSERT INTO [dbo].[T_TradeTrace]([controller],[from_account],[app_key],[tradeid],[CreateTime],[ctype],[bodyreq],[errcode] ,[errmsg]) values ";
            sqlstr += "(@controller,@from_account,@app_key,@tradeid,@CreateTime,@ctype,@bodyreq,@errcode ,@errmsg)";
            SqlParameter[] par = {
                new SqlParameter("@controller", controller),
                new SqlParameter("@from_account", from_account),
                new SqlParameter("@app_key",app_key),
                new SqlParameter("@tradeid",tradeid),
                new SqlParameter("@CreateTime",CreateTime),
                new SqlParameter("@ctype",ctype),
                new SqlParameter("@bodyreq",bodyreq),
                new SqlParameter("@errcode",errcode),
                new SqlParameter("@errmsg",errmsg)                                 
                };
            int i = site.ChangeData(sqlstr, par);
            //Log.AddInfo("TokenController.writeTokenLog()", "End");
        }
    }
}