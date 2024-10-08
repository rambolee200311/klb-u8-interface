using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BidlinkU8Interface.Models.TradeID
{
    public class TradeResult
    {
        public String errcode { get; set; }
        public String errmsg { get; set; }
        public Trade trade { get; set; }
    }
}