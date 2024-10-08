using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BidlinkU8Interface.Models
{
    public class Token
    {
        public String appKey { get; set; }
        public int expiresIn { get; set; }
        public string id { get; set; }
    }
}