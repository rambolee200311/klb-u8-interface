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
        [Route("purchaseorder/add")]
        [HttpPost]
        public KlbU8Vouch.GLVouch.Result addPurchaseOrder([FromBody]InMain inMain)
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
        [Route("purchaseorder/getsample")]
        [HttpGet]
        public InMain getSample()
        {
            InMain inMain = new InMain();
            inMain.ds_sequence=3;
            inMain.oa_mid= "106114";
            inMain.vencode= "01001";
            inMain.depcode= "01";
            inMain.personcode= "01001";
            inMain.cpocode= "KLB20221055-120-GZ001";
            inMain.currency= "人民币";
            inMain.pucode= "11";
            inMain.memo= "测试测试测试";
            inMain.ddate= "2024-10-11";
            inMain.cmaker= "宋媛媛";
            inMain.itemclass= "97";
            inMain.itemcode= "A001";
            inMain.details = new List<InDetail>();
            InDetail inDetail = new InDetail();
            inDetail.oa_did = "3";
            inDetail.invcode= "I72030134";
            inDetail.qty= 4;
            inDetail.txtrate= 13;
            inDetail.unitprice= 1769.91m;
            inDetail.taxprice = 2000;
            inMain.details.Add(inDetail);
            return inMain;
        }

    }
}
