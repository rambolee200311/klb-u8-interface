using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
namespace BidlinkU8Interface.Models.XMLVouchResult
{
    public class Item
    {
        //<item accounting_period="5" voucher_type="收" voucher_id="3" entry_id="" succeed="0" dsc="凭证已成功保存" u8accounting_period="5" u8voucher_id="3">
        //<item key="p16102" succeed="101" dsc="客户编码已经存在，不可重复！"></item
        [XmlAttribute("accounting_period")]
        public string accounting_period { get; set; }
        [XmlAttribute("voucher_type")]
        public string voucher_type { get; set; }
        [XmlAttribute("voucher_id")]
        public string voucher_id { get; set; }
        [XmlAttribute("entry_id")]
        public string entry_id { get; set; }
        [XmlAttribute("succeed")]
        public string succeed { get; set; }
        [XmlAttribute("dsc")]
        public string dsc { get; set; }
        [XmlAttribute("u8accounting_period")]
        public string u8accounting_period { get; set; }
        [XmlAttribute("u8voucher_id")]
        public string u8voucher_id { get; set; }
        [XmlAttribute("key")]
        public string key { get; set; }
    }
}