using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
namespace BidlinkU8Interface.Models.XMLVouch
{
    [XmlRootAttribute("ufinterface", IsNullable = false)] 
    public class XmlUfInterFace
    {
        [XmlAttribute("roottag")]
        public string roottag { get; set; }
        [XmlAttribute("billtype")]
        public string billtype { get; set; }
        [XmlAttribute("receiver")]
        public string receiver { get; set; }
        [XmlAttribute("sender")]
        public string sender { get; set; }
        [XmlAttribute("proc")]
        public string proc { get; set; }
        [XmlAttribute("codeexchanged")]
        public string codeexchanged { get; set; }
        [XmlElementAttribute("voucher", IsNullable = false)] 
        public XmlVoucher voucher { get; set; }
         
    }
}