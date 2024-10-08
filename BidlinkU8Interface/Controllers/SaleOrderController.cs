using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BidlinkU8Interface.Helper;
using KlbU8Vouch;
using KlbU8Vouch.GLVouch;
using KlbU8Vouch.GLVouch;
using KlbU8Vouch.SaleOrder;

namespace BidlinkU8Interface.Controllers
{
    public class SaleOrderController : ApiController
    {
        [Route("saleorder/add")]
        [HttpPost]
        public KlbU8Vouch.GLVouch.Result addDispatchList([FromBody]InMain inMain)
        {
            Log.AddInfo("SaleOrderController.addDispatchList()", "Begin");
            KlbU8Vouch.GLVouch.Result result = new KlbU8Vouch.GLVouch.Result();
            Log.AddDebug("SaleOrderController.addDispatchList()", "inData:" + inMain.ToJson());
            string AccID = "";//账套
            string User = "";//用户名
            string Password = "";//密码
            string Server = "";//服务器
            AccID = HOTConfig.GetConfig().GetAccID(inMain.ds_sequence);
            User = HOTConfig.GetConfig().GetUser(inMain.ds_sequence);
            Password = HOTConfig.GetConfig().GetPassword(inMain.ds_sequence);
            Server = HOTConfig.GetConfig().GetServer(inMain.ds_sequence);
            result = SaleOrderEntity.addDBVouch(result, inMain, AccID, User, Password, Server);

            Log.AddInfo("SaleOrderController.addDispatchList()", "outData:" + result.ToJson());
            Log.AddInfo("SaleOrderController.addDispatchList()", "End");
            return result;
        }
    }
}
