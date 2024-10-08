using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
namespace BidlinkU8Interface.Models.SQLResult
{
    public class Vouchid
    {
        [XmlElementAttribute("ino_id", IsNullable = true)]
        public Ino_id ino_id { get; set; }
    }
}