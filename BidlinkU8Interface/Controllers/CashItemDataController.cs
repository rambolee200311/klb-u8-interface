using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BidlinkU8Interface.Helper;
using KlbU8Vouch;
using KlbU8Vouch.GLVouch;
using KlbU8Vouch.CashItemDataSource;

namespace BidlinkU8Interface.Controllers
{
    public class CashItemDataController : ApiController
    {
        [Route("cashitemdata/add")]
        [HttpPost]
        public KlbU8Vouch.GLVouch.Result addCashItemData([FromBody]InMain inMain)
        {
            Log.AddInfo("CashItemDataController.addCashItemData()", "Begin");
            KlbU8Vouch.GLVouch.Result result = new KlbU8Vouch.GLVouch.Result();
            Log.AddDebug("CashItemDataController.addCashItemData()", "inData:" + inMain.ToJson());
            string AccID = "";//账套
            string User = "";//用户名
            string Password = "";//密码
            string Server = "";//服务器
            AccID = HOTConfig.GetConfig().GetAccID(inMain.ds_sequence);
            User = HOTConfig.GetConfig().GetUser(inMain.ds_sequence);
            Password = HOTConfig.GetConfig().GetPassword(inMain.ds_sequence);
            Server = HOTConfig.GetConfig().GetServer(inMain.ds_sequence);
            result = CashItemDataEntity.addDBVouch(result, inMain, AccID, User, Password, Server);

            Log.AddInfo("CashItemDataController.addCashItemData()", "outData:" + result.ToJson());
            Log.AddInfo("CashItemDataController.addCashItemData()", "End");
            return result;
        }
        [Route("cashitemdata/sample")]
        [HttpGet]
        public InMain addCashItemData()
        {
            InMain inMain = new InMain();
            inMain.ds_sequence = 1;
            inMain.iyear = 2023;
            inMain.cashitemdatas = new List<CashItemData>();
                CashItemData cashitemdata1 = new CashItemData();
                cashitemdata1.citemcode = "04";
                cashitemdata1.cdatasource = "660211";
                CashItemData cashitemdata2 = new CashItemData();
                cashitemdata2.citemcode = "04";
                cashitemdata2.cdatasource = "660211";
            inMain.cashitemdatas.Add(cashitemdata1);
            inMain.cashitemdatas.Add(cashitemdata2);
            return inMain;
        }
    }
}
