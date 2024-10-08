using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using BidlinkU8Interface.Helper;
using BidlinkU8Interface.Models.SQLEXE;
using BidlinkU8Interface.Models.SQLResult;
using System.Data;

namespace BidlinkU8Interface.Helper
{
    public class VoucherHelper
    {
       
        
        //ds_sequence to Eaicode
        public static string GetEaicode(int ds_sequence)
        {
            return HOTConfig.GetConfig().GetEaicode(ds_sequence);
        }
        public static string GetDatabase(int ds_sequence)
        {
            return HOTConfig.GetConfig().GetDatabase(ds_sequence);
        }
        //get max(ino_id)+1
        public static string GetInoid(int iyear, int iperiod, string csign, int ds_sequence)
        {
            string strResult = "";
            //try
            //{
            //    BidlinkU8Interface.Models.SQLEXE.XmlUfInterFace ufinterface = new BidlinkU8Interface.Models.SQLEXE.XmlUfInterFace();
            //    String Eaicode = HOTConfig.GetConfig().GetEaicode(ds_sequence);
            //    ufinterface.sender = Eaicode;
            //    ufinterface.receiver = Eaicode;
            //    ufinterface.roottag = "SQLEXE";
            //    ufinterface.proc = "vouchid";
            //    ufinterface.codeexchanged = "N";
            //    XMLSql xmlsql = new XMLSql();
            //    string cyear = iyear.ToString();
            //    string cperiod = iperiod.ToString("D2");
            //    xmlsql.valuesql = "select isnull(max(ino_id),0)+1 ino_id from gl_accvouch where iyear=" + cyear + " and iperiod=" + iperiod.ToString() + " and csign='" + csign + "'";
            //    ufinterface.sql = xmlsql;
            //    String xmltext = XmlSerializeHelper.XmlSerialize<BidlinkU8Interface.Models.SQLEXE.XmlUfInterFace>(ufinterface);
            //    xmltext = "<" + xmltext.Substring(xmltext.IndexOf('<') + 1);
            //    xmltext = xmltext.Trim();
            //    XmlDocument xmldoc = new XmlDocument();
            //    xmldoc.LoadXml(xmltext);
            //    xmldoc = PUTtoEAI.putXmltoEai(xmldoc);

            //    BidlinkU8Interface.Models.SQLResult.XmlUfInterFace sqlresult = XmlSerializeHelper.DESerializer<BidlinkU8Interface.Models.SQLResult.XmlUfInterFace>(xmldoc.OuterXml);
            //    strResult = sqlresult.newdataset.vouchid.ino_id.value;
            //}
            //catch(Exception ex)
            //{
            //    Log.AddError("VoucherHelper.GetInoid",ex.Message);
            //    strResult = ex.Message;
            //}
            return strResult;
        }
        public static string GetInoidDB(int iyear, int iperiod, string csign, int ds_sequence)
        {
            
            string strResult = "";
            try
            {
                String database = HOTConfig.GetConfig().GetDatabase(ds_sequence);
                string cyear = iyear.ToString();
                string cperiod = iperiod.ToString("D2");
                String sqlstr = "select isnull(max(ino_id),0)+1 ino_id from " + database + "..gl_accvouch where iyear=" + cyear + " and iperiod=" +iperiod.ToString() + " and csign='" + csign + "'";
                //定义变量
                string LocalDBPath = HOTConfig.GetConfig().GetHOTConnetionString(
                       HOTConfig.GetConfig().GetSQLDataSource(),
                       HOTConfig.GetConfig().GetSQLInitialCatalog(),
                       HOTConfig.GetConfig().GetSQLUserID(),
                       HOTConfig.GetConfig().GetSQLPassword());


                SQLiteHelper site = new SQLiteHelper();
                site.buildConnetionString(LocalDBPath);//读取本地数据库数据
                DataSet qry = site.SelectData(sqlstr);
                if (qry.Tables.Count == 1 && qry.Tables[0].Rows.Count == 0)
                {
                    strResult = "";
                }
                else
                {
                    strResult = qry.Tables[0].Rows[0]["ino_id"].ToString();
                    
                }
            }
            catch (Exception ex)
            {
                Log.AddError("VoucherHelper.GetInoid", ex.Message);
                strResult = ex.Message;
            }
            return strResult;
        }
    }
}