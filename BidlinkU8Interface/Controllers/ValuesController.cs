using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using BidlinkU8Interface.Helper;
using BidlinkU8Interface.Models.SQLEXE;
using BidlinkU8Interface.Models.SQLResult;

namespace BidlinkU8Interface.Controllers
{
    //[RoutePrefix("api/v1")]
    public class ValuesController : ApiController
    {
        [Route("abc")]
        [HttpGet]
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("bcd")]
        [HttpGet]
        public string Get(int ds_sequence)
        {
            return HOTConfig.GetConfig().GetEaicode(ds_sequence);
        }

        [Route("cde")]
        [HttpGet]
        // GET api/values/5
        public string Get(int iyear,int iperiod,string csign)
        {
            //BidlinkU8Interface.Models.SQLEXE.XmlUfInterFace ufinterface = new BidlinkU8Interface.Models.SQLEXE.XmlUfInterFace();
            //ufinterface.sender = "996";
            //ufinterface.receiver = "996";
            //ufinterface.roottag = "SQLEXE";
            //ufinterface.proc = "vouchid";
            //ufinterface.codeexchanged = "N";
            //XMLSql xmlsql = new XMLSql();
            //string cyear = iyear.ToString();
            //string cperiod = iperiod.ToString("D2");
            //xmlsql.valuesql = "select isnull(max(ino_id),0)+1 ino_id from ufdata_996_2021..gl_accvouch where iyear="+cyear+" and iperiod='"+cperiod+"' and csign='"+csign+"'";
            //ufinterface.sql = xmlsql;
            //String xmltext = XmlSerializeHelper.XmlSerialize<BidlinkU8Interface.Models.SQLEXE.XmlUfInterFace>(ufinterface);
            //xmltext = "<" + xmltext.Substring(xmltext.IndexOf('<') + 1);
            //xmltext = xmltext.Trim();
            //XmlDocument xmldoc = new XmlDocument();
            //xmldoc.LoadXml(xmltext);
            //xmldoc = PUTtoEAI.putXmltoEai(xmldoc);

            //BidlinkU8Interface.Models.SQLResult.XmlUfInterFace sqlresult = XmlSerializeHelper.DESerializer<BidlinkU8Interface.Models.SQLResult.XmlUfInterFace>(xmldoc.OuterXml);
            //string strResult = sqlresult.newdataset.vouchid.ino_id.value;
            //return strResult;
            return "";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}