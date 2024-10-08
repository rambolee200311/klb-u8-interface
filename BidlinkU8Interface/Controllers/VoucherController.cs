using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using BidlinkU8Interface.Models;
//using BidlinkU8Interface.Models.GLVouch;
using BidlinkU8Interface.Models.XMLVouch;
using BidlinkU8Interface.Helper;
using BidlinkU8Interface.App_Start;
using System.Data;
using System.Data.SqlClient;
//using BidlinkU8Interface.Entities;
using KlbU8Vouch.GLVouch;

namespace BidlinkU8Interface.Controllers
{
    public class VoucherController : ApiController
    {
        //定义变量
        string LocalDBPath = HOTConfig.GetConfig().GetHOTConnetionString(
               HOTConfig.GetConfig().GetSQLDataSource(),
               HOTConfig.GetConfig().GetSQLInitialCatalog(),
               HOTConfig.GetConfig().GetSQLUserID(),
               HOTConfig.GetConfig().GetSQLPassword());


        SQLiteHelper site = new SQLiteHelper();
        public VoucherController()
        {
            site.buildConnetionString(LocalDBPath);//读取本地数据库数据
        }

        [Route("voucher/add")]
        [HttpPost]
        [APIFilter]
        public KlbU8Vouch.GLVouch.Result AddVoucher([FromBody]GLVoucherReq vouchreq, String from_account, String to_account, String app_key, String token, String tradeid, int ds_sequence)
        {
            Log.AddInfo("VoucherController.AddVoucher()", "Begin");
            KlbU8Vouch.GLVouch.Result result = new KlbU8Vouch.GLVouch.Result();
            Log.AddDebug("VoucherController.AddVoucher()","inData:"+ vouchreq.ToJson());
            string datetime = DateTime.Now.ToString();
            
            result.tradeid = tradeid;

            //检查凭证tradeid是否重复
            if (isReuplicative(from_account, app_key, tradeid) != 0)
            {
                result.errcode = "1001";
                result.errmsg = "导入过凭证，请勿重复";
                result.id = "";
                return result;
            }
            writeTradeLog("voucheradd", from_account, app_key, tradeid, datetime, "begin", vouchreq.ToJson(), "", "");

            //xml EAI 凭证
            //result = VouchEntity.addXmlVouch(result,vouchreq,datetime, from_account,  to_account,  app_key,  token,  tradeid,  ds_sequence,site);
            //cvoucher 凭证

            string AccID = "";//账套
            string User = "";//用户名
            string Password = "";//密码
            string Server = "";//服务器
            AccID = HOTConfig.GetConfig().GetAccID(ds_sequence);
            User = HOTConfig.GetConfig().GetUser(ds_sequence);
            Password = HOTConfig.GetConfig().GetPassword(ds_sequence);
            Server = HOTConfig.GetConfig().GetServer(ds_sequence);
            result = KlbU8Vouch.VouchEntity.addDBVouch(result, vouchreq, datetime, from_account, to_account, app_key, token, tradeid, AccID, User, Password, Server);
            if (result.errcode == "0")
            {
                writeTradeLog("voucheradd", from_account, app_key, tradeid, datetime, "end", vouchreq.ToJson(), result.errcode, result.id);
            }
            else
            {
                writeTradeLog("voucheradd", from_account, app_key, tradeid, datetime, "end", vouchreq.ToJson(), result.errcode, result.errmsg);
            }


            Log.AddDebug("VoucherController.AddVoucher()", "outData:" + result.ToJson());
            Log.AddInfo("VoucherController.AddVoucher()", "End");
            return result;
        }

        //写入tradeid日志
        private void writeTradeLog(string controller,string from_account, string app_key, string tradeid, string CreateTime, string ctype, string bodyreq, string errcode, string errmsg)
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
        //检查是否重复凭证
        private int isReuplicative(string from_account,string app_key,string tradeid)
        {
            int iResult = 1;
            string sqlstr = "select tradetraceid from oauth.dbo.T_TradeTrace where controller='voucheradd' ";//and ctype='end' and errcode='0' ";
            sqlstr+="and from_account=@from_account and app_key=@app_key and tradeid=@tradeid";
            SqlParameter[] par = {                       
                new SqlParameter("@from_account", from_account),
                new SqlParameter("@app_key",app_key),
                new SqlParameter("@tradeid",tradeid)                                              
                };
            DataSet qry = site.SelectData(sqlstr, par);
            if (qry.Tables.Count == 1 && qry.Tables[0].Rows.Count > 0)
            {                
                iResult=1;
            }
            else
            {
                iResult = 0;
            }
            return iResult;
        }
    }
}
