using System;
using System.Collections.Generic;
using System.Text;
using BidlinkU8Interface.Helper;
using BidlinkU8Interface.Models.Pay;
using BidlinkU8Interface.Models;
using KlbU8Vouch.GLVouch;

namespace BidlinkU8Interface.Entities
{
    public class PayEntity
    {
        public static KlbU8Vouch.GLVouch.Result Add_Pay(KlbU8Vouch.GLVouch.Result re, InMain pay, string AccID,
          string User,
          string Password,
          string Server)
        {
            KlbU8Vouch.GLVouch.Result result = re;
            int rowno = 1;
            ADODB.Connection conn = new ADODB.Connection();
            string strResult = "";
            string strSql = "";
            string depcode = "";
            string personcode = "";
            string itemcode = "";
            string vencode = "";
            decimal amount = 0m;
            decimal sumamount = 0m;
            decimal tax = 0m;
            decimal money = 0m;
            decimal taxrate = 0m;
            string vouchID = "";
            string cLink = "";
            string cashitemcode = "";
            bool bTran = false;
            U8Login.clsLogin m_ologin = new U8Login.clsLogin();
            
            //LogHelper.WriteLog(typeof(PayEntity), "msxml2 start");
            MSXML2.IXMLDOMDocument2 domHead = new MSXML2.DOMDocument30();
            MSXML2.IXMLDOMDocument2 domBody = new MSXML2.DOMDocument30();
            //LogHelper.WriteLog(typeof(PayEntity), "u8login start");
            string Date = pay.ddate;
            string Year = Convert.ToDateTime(pay.ddate).Year.ToString();
            if (m_ologin.Login("AR", AccID, Year, User, Password, Date, Server, null))
            {
                strResult = "login success!";
            }
            else
            {
                strResult = "login failed! User=" + User + ";Server=" + Server + ";AccID=" + AccID;
                result.errcode = "1111";
                result.errmsg = strResult;
                //result.id = "";
                return result;
            }
            //LogHelper.WriteLog(typeof(PayEntity), "checkrepeat start");
            //检查应付单是否重复
            strResult = DBhelper.getDataFromSql(m_ologin.UfDbName, "select cdefine11 from ap_closebill where cdefine11='" + pay.oa_mid + "' and cVouchType='49'");
            if (strResult != "")
            {
                strResult = pay.oa_mid + "已生成过付款单";
                result.errcode = "333";
                result.errmsg = strResult;
                return result;
            }            
            try
            {
                conn.Open(m_ologin.UfDbName);
                domHead.load(@"C:\app\XmlModel\APCloseHeadADD.xml");
                domBody.load(@"C:\app\XmlModel\APCloseBodyADD.xml");

                //LogHelper.WriteLog(typeof(PayEntity), "xml body start");

                #region//body
                MSXML2.IXMLDOMNode xnnode = domBody.selectSingleNode("xml").selectSingleNode("rs:data").selectSingleNode("z:row");
                sumamount = 0m;
                rowno = Convert.ToInt32(DBhelper.getDataFromSql(m_ologin.UfDbName, "select  isnull(max(id),0)+1 from ap_closebills"));
                foreach (InDetail body in pay.details)
                {
                    amount = body.amount;
                   
                    money = amount - tax;
                    if (money != 0)
                    { taxrate = (tax / money) * 100; }
                    sumamount += amount;
                    MSXML2.IXMLDOMNode xnnodeclone = xnnode.cloneNode(true);
                    xnnodeclone.attributes.getNamedItem("iID").text = DBhelper.getDataFromSql(m_ologin.UfDbName, "select  isnull(max(iid),0)+1 from ap_closebill");
                    xnnodeclone.attributes.getNamedItem("ID").text = rowno.ToString();
                    rowno++;
                    if (string.IsNullOrEmpty(body.vencode)) { xnnodeclone.attributes.getNamedItem("cCusVen").text = pay.vencode; } else { xnnodeclone.attributes.getNamedItem("cCusVen").text = body.vencode; }                   
                    xnnodeclone.attributes.getNamedItem("iAmt").text = amount.ToString();
                    xnnodeclone.attributes.getNamedItem("iAmt_f").text = amount.ToString();
                    xnnodeclone.attributes.getNamedItem("iRAmt").text = money.ToString();
                    xnnodeclone.attributes.getNamedItem("iRAmt_f").text = money.ToString();
                    xnnodeclone.attributes.getNamedItem("cKm").text = body.ccode;
                    if (string.IsNullOrEmpty(body.depcode)) { xnnodeclone.attributes.getNamedItem("cDepCode").text = pay.depcode; } else { xnnodeclone.attributes.getNamedItem("cDepCode").text = body.depcode; }
                    if (string.IsNullOrEmpty(body.personcode)) { xnnodeclone.attributes.getNamedItem("cPersonCode").text = pay.personcode; } else { xnnodeclone.attributes.getNamedItem("cPersonCode").text = body.personcode; }
                    if (string.IsNullOrEmpty(body.itemclass)) { xnnodeclone.attributes.getNamedItem("cXmClass").text = pay.itemclass; } else { xnnodeclone.attributes.getNamedItem("cXmClass").text = body.itemclass; }
                    if (string.IsNullOrEmpty(body.itemclass)) { xnnodeclone.attributes.getNamedItem("cXm").text = pay.itemcode; } else { xnnodeclone.attributes.getNamedItem("cXm").text = body.itemcode; }
                    xnnodeclone.attributes.getNamedItem("cDigest").text = body.memo;
                    xnnodeclone.attributes.getNamedItem("cMemo").text = body.memo;
                    xnnodeclone.attributes.getNamedItem("cDefine22").text = body.oa_did;
                    //现金流量
                    //xnnodeclone.attributes.getNamedItem("cDefine28").text = cashitemcode;
                    domBody.selectSingleNode("xml").selectSingleNode("rs:data").appendChild(xnnodeclone);

                }
                if (domBody.selectSingleNode("xml").selectSingleNode("rs:data").childNodes.length > 1)
                {
                    domBody.selectSingleNode("xml").selectSingleNode("rs:data").removeChild(xnnode);
                }
                #endregion
                //LogHelper.WriteLog(typeof(PayEntity), "xml head start");
                #region//head
                MSXML2.IXMLDOMNode xnnodehead = domHead.selectSingleNode("xml").selectSingleNode("rs:data").selectSingleNode("z:row");

                xnnodehead.attributes.getNamedItem("cSSCode").text = pay.sscode;
                xnnodehead.attributes.getNamedItem("cDwCode").text = pay.vencode;
                xnnodehead.attributes.getNamedItem("cDeptCode").text = pay.depcode;
                xnnodehead.attributes.getNamedItem("cPerson").text = pay.personcode;                
                xnnodehead.attributes.getNamedItem("cItemCode").text = pay.itemcode;
                xnnodehead.attributes.getNamedItem("cItem_Class").text = pay.itemclass;
                xnnodehead.attributes.getNamedItem("iAmount").text = sumamount.ToString();
                xnnodehead.attributes.getNamedItem("iAmount_f").text = sumamount.ToString();
                xnnodehead.attributes.getNamedItem("iRAmount").text = sumamount.ToString();
                xnnodehead.attributes.getNamedItem("iRAmount_f").text = sumamount.ToString();
                xnnodehead.attributes.getNamedItem("cDefine11").text = pay.oa_mid;
                xnnodehead.attributes.getNamedItem("cCode").text = pay.ccode;
                xnnodehead.attributes.getNamedItem("cDigest").text = pay.memo;
                xnnodehead.attributes.getNamedItem("dVouchDate").text = pay.ddate;
                xnnodehead.attributes.getNamedItem("iPeriod").text = Convert.ToDateTime(pay.ddate).Month.ToString();
                xnnodehead.attributes.getNamedItem("iID").text = DBhelper.getDataFromSql(m_ologin.UfDbName, "select  isnull(max(iid),0)+1 from ap_closebill");

                #endregion

                //LogHelper.WriteLog(typeof(PayEntity), "netcwapi start");
                /*
                NetCWAPI.U8NetCWAPIClass uncw = new NetCWAPI.U8NetCWAPIClass();
                uncw.NetCWToVBAdd(m_ologin.userToken, conn, "SaveVouch", "付款单", domHead.xml, domBody.xml, ref bTran, ref strResult);
                */
                UFAPBO.clsVouchFacade myVouch = new UFAPBO.clsVouchFacade();
                object oSource = Type.Missing;
                myVouch.Init("付款单", m_ologin, conn, "AP");
                bTran = true;
                string fileName = DateTime.Now.ToString("yyMMddHHmmss");
                domHead.save(@"C:\app\XmlTemp\APCloseHead_"+fileName+".xml");
                domBody.save(@"C:\app\XmlTemp\APCloseBody_" + fileName + ".xml");
                bTran = myVouch.SaveVouch(domHead, domBody, ref strResult, bTran);
                //LogHelper.WriteLog(typeof(PayEntity), "netcwapi result");
                //if (string.IsNullOrEmpty(strResult))
                if (bTran)
                {
                    result.errcode = "0";
                    result.errmsg = "";
                    vouchID = DBhelper.getDataFromSql(m_ologin.UfDbName, "select cvouchid from ap_closebill where cdefine11='" + pay.oa_mid + "' and cVouchType='49'");
                    cLink = DBhelper.getDataFromSql(m_ologin.UfDbName, "select iID from ap_closebill where cdefine11='" + pay.oa_mid + "' and cVouchType='49'");
                    DBhelper.setDataFromSql(m_ologin.UfDbName, "update ap_closebill set iperiod=" + Convert.ToDateTime(pay.ddate).Month.ToString() + " where iID=" + cLink + " and cVouchType='49'");
                    result.errmsg = vouchID;
                    #region//verify vouch
                    //bTran = Verify(u8login, conn, vouchID, cLink);

                    if (bTran)
                    {
                        //bTran = MakeVouch(u8login, conn, vouchID, cLink, pay.head.ddate, pay);
                    }
                    #endregion
                    return result;
                }
                else
                {
                    result.errcode = "888";
                    result.errmsg = strResult;
                    //re.u8code = DBhelper.getDataFromSql(m_ologin.UfDbName, "select cvouchid from ap_closebill where cdefine1='" + pay.head.oacode + "'");
                    return result;
                }

            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog(typeof(PayEntity), ex);
                result.errcode = "999";
                result.errmsg = ex.Message;
                return result;
            }
            finally
            {
                if (conn.State == 1)
                {
                    conn.Close();
                }
            }
        }
    }
}
