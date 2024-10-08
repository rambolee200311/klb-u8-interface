using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BidlinkU8Interface.Helper;
using BidlinkU8Interface.Models.TradeID;
using BidlinkU8Interface.App_Start;

namespace BidlinkU8Interface.Controllers
{
    public class TradeController : ApiController
    {
        //定义变量
        string LocalDBPath = HOTConfig.GetConfig().GetHOTConnetionString(
               HOTConfig.GetConfig().GetSQLDataSource(),
               HOTConfig.GetConfig().GetSQLInitialCatalog(),
               HOTConfig.GetConfig().GetSQLUserID(),
               HOTConfig.GetConfig().GetSQLPassword());


        SQLiteHelper site = new SQLiteHelper();

        public TradeController()
        {
            site.buildConnetionString(LocalDBPath);//读取本地数据库数据
        }
        [Route("system/trade")]
        [HttpGet]
        [APIFilter]
        public TradeResult getTradeID(string from_account, string app_key,string token)
        {
            Log.AddInfo("TradeController.getTradeID()", "Begin");
            TradeResult tradeResult = new TradeResult();
            tradeResult.errcode = "0";
            tradeResult.errmsg = "成功";
            string datetime = DateTime.Now.ToString();
            try
            {
                JsonDocument InData = new JsonDocument();

                InData.add(new JsonElement("from_account", from_account));
                InData.add(new JsonElement("token", token));
                InData.add(new JsonElement("app_key", app_key));
                InData.add(new JsonElement("datetime", datetime));                
                JObject JInData = (JObject)JsonConvert.DeserializeObject(InData.innerText);
                Log.AddInfo("TradeController.getTradeID()", "inData:" + InData.innerText);

                Trade trade = new Trade();
                trade.id = TradeHelper.getTradeMD5(JInData);
                tradeResult.trade = trade;
            }
            catch (Exception ex)
            {
                tradeResult.errcode = "0";
                tradeResult.errmsg = ex.Message;
            }
            Log.AddInfo("TradeController.getTradeID()", "outData:" + tradeResult.ToJson());
            Log.AddInfo("TradeController.getTradeID()", "End");
            return tradeResult;
        }
    }
}
