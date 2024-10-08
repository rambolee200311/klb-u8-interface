using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
namespace BidlinkU8Interface.Models.XMLVouchResult
{
    [XmlRootAttribute("ufinterface", IsNullable = false)] 
    public class XmlUfInterFace
    {
        [XmlAttribute("roottag")]
        public string roottag { get; set; }
        
        [XmlAttribute("receiver")]
        public string receiver { get; set; }
        [XmlAttribute("sender")]
        public string sender { get; set; }
        [XmlAttribute("proc")]
        public string proc { get; set; }
        [XmlAttribute("codeexchanged")]
        public string codeexchanged { get; set; }

        [XmlAttribute("docid")]
        public string docid { get; set; }
         
        [XmlAttribute("maxdataitems")]
        public string maxdataitems { get; set; }
        [XmlAttribute("bignoreextenduserdefines")]
        public string bignoreextenduserdefines { get; set; }

        //[XmlAttribute("equest-roottag")]
        //public string @equest-roottag { get; set; }
        [XmlElementAttribute("item",IsNullable=false)]
        public Item item { get; set; }
    }
}