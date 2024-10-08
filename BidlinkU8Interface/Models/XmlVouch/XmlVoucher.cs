using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
namespace BidlinkU8Interface.Models.XMLVouch
{
    [XmlRootAttribute("voucher", IsNullable = false)] 
    public class XmlVoucher
    {
        //[XmlAttribute("id")]
        //public int VoucherID { get; set; }

        [XmlElementAttribute("voucher_head", IsNullable = false)] 
        public XmlVoucherHead voucher_head { get; set; }

        [XmlArrayAttribute("voucher_body")]
        public List<entry> voucher_body { get; set; }
    }
}