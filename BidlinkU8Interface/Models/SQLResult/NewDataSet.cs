using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
namespace BidlinkU8Interface.Models.SQLResult
{
    public class NewDataSet
    {
        [XmlElementAttribute("vouchid", IsNullable = true)]
        public Vouchid vouchid { get; set; }
    }
}