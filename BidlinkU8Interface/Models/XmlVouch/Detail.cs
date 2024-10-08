using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
namespace BidlinkU8Interface.Models.XmlVouch
{
    
    public class Detail
    {
        [XmlElementAttribute("cash_flow_statement", IsNullable = true)] 
        public Cash_flow_statement cash_flow_statement { get; set; }
        [XmlElementAttribute("Code_remark_statement", IsNullable = true)] 
        public Code_remark_statement code_remark_statement { get; set; }
    }
}