using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BidlinkU8Interface.Helper;
using BidlinkU8Interface.Models.Receive;
using BidlinkU8Interface.Models;
using KlbU8Vouch.GLVouch;
namespace BidlinkU8Interface.Entities
{
    public class ReceiveEntity
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
            string personname = "";
            string itemcode = "";
            string vencode = "";
            decimal amount = 0m;
            decimal sumamount = 0m;
            decimal tax = 0m;
            decimal money = 0m;
            decimal taxrate = 0m;
            string vouchID = "";
            string cLink = "";
            bool bTran = false;
            string cashitemcode = "";
            MSXML2.IXMLDOMDocument2 domHead = new MSXML2.DOMDocument30();
            MSXML2.IXMLDOMDocument2 domBody = new MSXML2.DOMDocument30();
            string Date = pay.ddate;
            string Year = Convert.ToDateTime(pay.ddate).Year.ToString();
            U8Login.clsLogin u8login = new U8Login.clsLogin();
            if (u8login.Login("AP", AccID, Year, User, Password, Date, Server, null))
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
            //检查应付单是否重复
            strResult = DBhelper.getDataFromSql(u8login.UfDbName, "select cdefine11 from ap_closebill where cdefine11='" + pay.oa_mid + "' and cVouchType='48'");
            if (strResult != "")
            {
                strResult = pay.oa_mid + "已生成过收款单";
                result.errcode = "333";
                result.errmsg = strResult;
                return result;
            }
            try
            {
                conn.Open(u8login.UfDbName);
                domHead.load(@"C:\app\XmlModel\ARCloseHeadADD.xml");
                domBody.load(@"C:\app\XmlModel\ARCloseBodyADD.xml");



                #region//body
                MSXML2.IXMLDOMNode xnnode = domBody.selectSingleNode("xml").selectSingleNode("rs:data").selectSingleNode("z:row");
                sumamount = 0m;
                rowno = Convert.ToInt32(DBhelper.getDataFromSql(u8login.UfDbName, "select  isnull(max(id),0)+1 from ap_closebills"));
                foreach (InDetail body in pay.details)
                {
                    amount = body.amount;
                    
                    money = amount - tax;
                    if (money != 0)
                    { taxrate = (tax / money) * 100; }
                    sumamount += amount;
                    MSXML2.IXMLDOMNode xnnodeclone = xnnode.cloneNode(true);
                    xnnodeclone.attributes.getNamedItem("iID").text = DBhelper.getDataFromSql(u8login.UfDbName, "select  isnull(max(iid),0)+1 from ap_closebill");
                    xnnodeclone.attributes.getNamedItem("ID").text = rowno.ToString();
                    rowno++;
                    if (string.IsNullOrEmpty(body.cuscode)) { xnnodeclone.attributes.getNamedItem("cCusVen").text = pay.cuscode; } else { xnnodeclone.attributes.getNamedItem("cCusVen").text = body.cuscode; }                  
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
                    xnnodeclone.attributes.getNamedItem("cDefine22").text = pay.oa_mid;
                    xnnodeclone.attributes.getNamedItem("cDefine23").text = cashitemcode;
                    domBody.selectSingleNode("xml").selectSingleNode("rs:data").appendChild(xnnodeclone);

                }
                if (domBody.selectSingleNode("xml").selectSingleNode("rs:data").childNodes.length > 1)
                {
                    domBody.selectSingleNode("xml").selectSingleNode("rs:data").removeChild(xnnode);
                }
                #endregion

                #region//head
                MSXML2.IXMLDOMNode xnnodehead = domHead.selectSingleNode("xml").selectSingleNode("rs:data").selectSingleNode("z:row");
                xnnodehead.attributes.getNamedItem("cSSCode").text = pay.sscode;
                xnnodehead.attributes.getNamedItem("cDwCode").text = pay.cuscode;
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
                xnnodehead.attributes.getNamedItem("iPeriod").text =Convert.ToDateTime(pay.ddate).Month.ToString();
                xnnodehead.attributes.getNamedItem("iID").text = DBhelper.getDataFromSql(u8login.UfDbName, "select  isnull(max(iid),0)+1 from ap_closebill");
                

                #endregion
                /*
                NetCWAPI.U8NetCWAPIClass uncw = new NetCWAPI.U8NetCWAPIClass();
                uncw.NetCWToVBAdd(u8login.userToken, conn, "SaveVouch", "收款单", domHead.xml, domBody.xml, ref bTran, ref strResult);
                */
                UFAPBO.clsVouchFacade myVouch = new UFAPBO.clsVouchFacade();
                object oSource = Type.Missing;
                myVouch.Init("收款单", u8login, conn, "AR");
                bTran = true;
                string fileName = DateTime.Now.ToString("yyMMddHHmmss");
                domHead.save(@"C:\app\XmlTemp\ARCloseHead_" + fileName + ".xml");
                domBody.save(@"C:\app\XmlTemp\ARCloseBody_" + fileName + ".xml");
                bTran = myVouch.SaveVouch(domHead, domBody, ref strResult, bTran);
                //if (string.IsNullOrEmpty(strResult))

                if (bTran)
                {
                    result.errcode = "0";
                    result.errmsg = "";
                    vouchID = DBhelper.getDataFromSql(u8login.UfDbName, "select cvouchid from ap_closebill where cdefine11='" + pay.oa_mid + "' and cVouchType='48'");
                    cLink = DBhelper.getDataFromSql(u8login.UfDbName, "select iID from ap_closebill where cdefine11='" + pay.oa_mid + "' and cVouchType='48'");
                    DBhelper.setDataFromSql(u8login.UfDbName, "update ap_closebill set iperiod=" + Convert.ToDateTime(pay.ddate).Month.ToString() + " where iID=" + cLink + " and cVouchType='48'");
                    result.errmsg = vouchID;
                    #region//verify vouch
                    //bTran = Verify(u8login, conn, vouchID, cLink);

                    if (bTran)
                    {
                        //bTran = MakeVouch(u8login, conn, vouchID, cLink, pay.head.ddate, pay, personname);
                    }
                    #endregion
                    return result;
                }
                else
                {
                    result.errcode = "888";
                    result.errmsg = strResult;
                    //re.u8code = DBhelper.getDataFromSql(u8login.UfDbName, "select cvouchid from ap_closebill where cdefine1='" + pay.oa_mid + "'");
                    return result;
                }

            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog(typeof(ReceiveEntity), ex);
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