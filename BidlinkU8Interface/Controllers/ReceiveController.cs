using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BidlinkU8Interface.Models.Receive;
using BidlinkU8Interface.Models;
using BidlinkU8Interface.Helper;
using BidlinkU8Interface.Entities;
using KlbU8Vouch.GLVouch;

namespace BidlinkU8Interface.Controllers
{
    public class ReceiveController : ApiController
    {
        [Route("receive/add")]
        [HttpPost]
        public KlbU8Vouch.GLVouch.Result addReceive([FromBody]InMain inMain)
        {
            Log.AddInfo("ReceiveController.addReceive()", "Begin");
            KlbU8Vouch.GLVouch.Result result = new KlbU8Vouch.GLVouch.Result();
            Log.AddDebug("ReceiveController.addReceive()", "inData:" + inMain.ToJson());
            string AccID = "";//账套
            string User = "";//用户名
            string Password = "";//密码
            string Server = "";//服务器
            AccID = HOTConfig.GetConfig().GetAccID(inMain.ds_sequence);
            User = HOTConfig.GetConfig().GetUser(inMain.ds_sequence);
            Password = HOTConfig.GetConfig().GetPassword(inMain.ds_sequence);
            Server = HOTConfig.GetConfig().GetServer(inMain.ds_sequence);
            result = ReceiveEntity.Add_Pay(result, inMain, AccID, User, Password, Server);

            Log.AddInfo("ReceiveController.addReceive()", "outData:" + result.ToJson());
            Log.AddInfo("ReceiveController.addReceive()", "End");
            return result;
        }    
    }
}
