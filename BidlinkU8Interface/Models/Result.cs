using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BidlinkU8Interface.Models
{
    public class Result
    {
        public String errcode { get; set; }
        public String errmsg { get; set; }
        public Token token { get; set; }
        public String id { get; set; }
        public String tradeid { get; set; }
    }
}