using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace BidlinkU8Interface.Helper
{
    public class TradeHelper
    {
        private static string secret = HOTConfig.GetConfig().GetTokenSecret();
        public static string getTradeMD5(JObject payload)
        {
            String strTrade = "";
            strTrade = payload.ToJson()+secret;
            strTrade = EncryptHelper.MD5EncryptFor32(strTrade);
            return strTrade;
        }
    }
}