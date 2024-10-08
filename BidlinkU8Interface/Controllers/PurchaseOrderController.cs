using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BidlinkU8Interface.Helper;
using KlbU8Vouch;
using KlbU8Vouch.GLVouch;
using KlbU8Vouch.PurchaseOrder;

namespace BidlinkU8Interface.Controllers
{
    public class PurchaseOrderController : ApiController
    {
        [Route("purchase/add")]
        [HttpPost]
        public KlbU8Vouch.GLVouch.Result addDispatchList([FromBody]InMain inMain)
        {
            Log.AddInfo("PurchaseOrderController.addDispatchList()", "Begin");
            KlbU8Vouch.GLVouch.Result result = new KlbU8Vouch.GLVouch.Result();
            Log.AddDebug("PurchaseOrderController.addDispatchList()", "inData:" + inMain.ToJson());
            string AccID = "";//账套
            string User = "";//用户名
            string Password = "";//密码
            string Server = "";//服务器
            AccID = HOTConfig.GetConfig().GetAccID(inMain.ds_sequence);
            User = HOTConfig.GetConfig().GetUser(inMain.ds_sequence);
            Password = HOTConfig.GetConfig().GetPassword(inMain.ds_sequence);
            Server = HOTConfig.GetConfig().GetServer(inMain.ds_sequence);
            result = PurchaseOrderEntity.addDBVouch(result, inMain, AccID, User, Password, Server);

            Log.AddInfo("PurchaseOrderController.addDispatchList()", "outData:" + result.ToJson());
            Log.AddInfo("PurchaseOrderController.addDispatchList()", "End");
            return result;
        }
    }
}
