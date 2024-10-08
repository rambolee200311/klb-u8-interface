using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
namespace BidlinkU8Interface.Models.XmlVouch
{
    public class Cash_flow_statement
    {
        [XmlText]
        public string value { get; set; }
    }
}