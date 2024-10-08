using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
namespace BidlinkU8Interface.Helper
{
    public class XmlSerializeHelper
    {
        /// <summary>
        /// 将实体对象转换成XML
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="obj">实体对象</param>
        public static string XmlSerialize<T>(T obj)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                //去除xml声明
                settings.OmitXmlDeclaration = false;
                settings.Encoding = Encoding.UTF8;
                
                System.IO.MemoryStream mem = new MemoryStream();
                using (XmlWriter writer = XmlWriter.Create(mem,settings))
                {                    
                    Type t = obj.GetType();
                    XmlSerializer formatter  = new XmlSerializer(t);
                    //去除默认命名空间xmlns:xsd和xmlns:xsi
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    formatter.Serialize(writer, obj, ns);
                    return Encoding.UTF8.GetString(mem.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("将实体对象转换成XML异常", ex);
            }
        }

        /// <summary>
        /// 将XML转换成实体对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="strXML">XML</param>
        public static T DESerializer<T>(string strXML) where T : class
        {
            try
            {
                using (StringReader sr = new StringReader(strXML))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(sr) as T;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("将XML转换成实体对象异常", ex);
            }
        }
    }
}