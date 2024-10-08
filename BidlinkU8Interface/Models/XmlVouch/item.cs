using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Reflection;
namespace BidlinkU8Interface.Models.XMLVouch
{
    [XmlRootAttribute("item", IsNullable = false)]
    public class item
    {
        [XmlAttribute("name")]
        public string itemname { get; set; }
        [XmlText]
        public string value { get; set; }
    }
}