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
using KlbU8Vouch.Invoice;

namespace BidlinkU8Interface.Controllers
{
    public class InvoiceController : ApiController
    {
        [Route("invoice/add")]
        [HttpPost]
        public KlbU8Vouch.GLVouch.Result addInvoice([FromBody]InMain inMain)
        {
            Log.AddInfo("InvoiceController.addInvoice()", "Begin");
            KlbU8Vouch.GLVouch.Result result = new KlbU8Vouch.GLVouch.Result();
            Log.AddDebug("InvoiceController.addInvoice()", "inData:" + inMain.ToJson());
            string AccID = "";//账套
            string User = "";//用户名
            string Password = "";//密码
            string Server = "";//服务器
            AccID = HOTConfig.GetConfig().GetAccID(inMain.ds_sequence);
            User = HOTConfig.GetConfig().GetUser(inMain.ds_sequence);
            Password = HOTConfig.GetConfig().GetPassword(inMain.ds_sequence);
            Server = HOTConfig.GetConfig().GetServer(inMain.ds_sequence);
            result = InvoiceEntity.addDBVouch(result, inMain, AccID, User, Password, Server);

            Log.AddInfo("InvoiceController.addInvoice()", "outData:" + result.ToJson());
            Log.AddInfo("InvoiceController.addInvoice()", "End");
            return result;
        }
    }
}
