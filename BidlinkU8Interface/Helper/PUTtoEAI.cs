using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace BidlinkU8Interface.Helper
{
    public class PUTtoEAI
    {
        public static XmlDocument putXmltoEai(XmlDocument xmlDoc)
        {
            string responseXml;
            string strUrl = HOTConfig.GetConfig().GetEAIURL();
            XmlDocument xmlResult = new XmlDocument();
            //创建EAI服务代理接口对象
            //U8Distribute.iDistributeClass eaiBroker = new U8Distribute.iDistributeClass();
            MSXML2.XMLHTTPClass xmlHttp = new MSXML2.XMLHTTPClass();
            try
            {
                //调用EAI服务代理的数据交换方法Process，传入Request交换消息， 并获取EAI返回的Response消息。
                //responseXml = eaiBroker.Process(xmlDoc.InnerXml.ToString());
                //释放EAI服务代理接口对象
                //System.Runtime.InteropServices.Marshal.FinalReleaseComObject(eaiBroker);
                xmlHttp.open("POST", strUrl, false, null, null);
                xmlHttp.send(xmlDoc.OuterXml);
                responseXml = xmlHttp.responseText;
                xmlResult.LoadXml(responseXml);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(xmlHttp);
                //ResponseToDB rtb = new ResponseToDB();
                //rtb.doResponseToDB(responseXml);
                // xmlResult.Save("C:\\LstStockOut\\dispathlist_response_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml");
            }
            catch(Exception e)
            {
                //
                Log.AddError("PUTtoEAI.putXmltoEai", e.Source+"-"+e.Message);
            }

            return xmlResult;
        }
    }
}