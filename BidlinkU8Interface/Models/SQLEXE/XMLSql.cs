using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
namespace BidlinkU8Interface.Models.SQLEXE
{
    public class XMLSql
    {
        [XmlAttribute("value")]
        public string valuesql { get; set; }
        [XmlText]
        public string sql { get; set; }
    }
}