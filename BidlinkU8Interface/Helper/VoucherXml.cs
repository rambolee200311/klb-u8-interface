using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BidlinkU8Interface.Models;
using BidlinkU8Interface.Models.GLVouch;
using BidlinkU8Interface.Models.XMLVouch;
using BidlinkU8Interface.Helper;
using BidlinkU8Interface.App_Start;
using System.Xml;
using System.Reflection;
namespace BidlinkU8Interface.Helper
{
    public class VoucherXml
    {
        public static XmlUfInterFace getVoucherXml(GLVoucherReq vouchreq, int ds_sequence)
        {
            //<ufinterface roottag="voucher" billtype="gl" receiver="996" sender="u8" proc="add" codeexchanged="N">
            XmlUfInterFace ufinterface = new XmlUfInterFace();

            ufinterface.roottag = "voucher";
            ufinterface.billtype = "gl";
            String Eaicode = HOTConfig.GetConfig().GetEaicode(ds_sequence);
            ufinterface.sender = Eaicode;
            ufinterface.receiver = Eaicode;
            ufinterface.proc = "add";
            ufinterface.codeexchanged = "N";
            XmlVoucher voucher = new XmlVoucher();
            //voucher.VoucherID = 787878;

            XmlVoucherHead head = new XmlVoucherHead();
            head.company = "";
            string vouchid = "";
            if (string.IsNullOrEmpty(vouchreq.voucher.voucher_id))
            {
                //vouchid = VoucherHelper.GetInoid(Convert.ToInt32(vouchreq.voucher.fiscal_year),
                //    vouchreq.voucher.accounting_period,
                //    vouchreq.voucher.voucher_type,
                //    ds_sequence);
                vouchid = VoucherHelper.GetInoidDB(Convert.ToInt32(vouchreq.voucher.fiscal_year),
                    vouchreq.voucher.accounting_period,
                    vouchreq.voucher.voucher_type,
                    ds_sequence);
            }
            else
            {
                vouchid = vouchreq.voucher.voucher_id;
            }

            head.voucher_id = vouchid;
            head.voucher_type = vouchreq.voucher.voucher_type;
            head.fiscal_year = vouchreq.voucher.fiscal_year;
            head.accounting_period = vouchreq.voucher.accounting_period;
            head.date = vouchreq.voucher.date;
            head.auditdate = "";
            head.enter = vouchreq.voucher.enter;
            head.cashier = "";
            head.signature = "";
            head.checker = "";
            head.posting_date = "";
            head.posting_person = "";
            head.memo1 = "";
            head.memo2 = "";
            head.revokeflag = "";
            head.attachment_number = vouchreq.voucher.attachment_number;
            head.voucher_making_system = "";


            head.reserve1 = "";
            head.reserve2 = "";
            List<entry> entrys = new List<entry>();
            //借方
            foreach (Entry entryJson in vouchreq.voucher.debit.entry)
            {
                entry entryXml = new entry();
                entryXml.entry_id = entryJson.entry_id;
                entryXml.account_code = entryJson.account_code;
                entryXml.@abstract = entryJson.@abstract;
                entryXml.currency = entryJson.currency;
                if ((entryJson.unit_price == null) || (entryJson.unit_price == 0))
                {
                    entryXml.unit_price = "";
                }
                else
                {
                    entryXml.unit_price = entryJson.unit_price.ToString();
                }
                if ((entryJson.exchange_rate1 == null) || (entryJson.exchange_rate1 == 0))
                {
                    entryXml.exchange_rate1 = "";
                }
                else
                {
                    entryXml.exchange_rate1 = entryJson.exchange_rate1.ToString();
                }
                if ((entryJson.exchange_rate2 == null) || (entryJson.exchange_rate2 == 0))
                {
                    entryXml.exchange_rate2 = "0";
                }
                else
                {
                    entryXml.exchange_rate2 = entryJson.exchange_rate2.ToString();
                };
                entryXml.debit_quantity = entryJson.debit_quantity;
                entryXml.primary_debit_amount = entryJson.primary_debit_amount;
                entryXml.secondary_debit_amount = "";
                entryXml.natural_debit_currency = entryJson.natural_debit_currency;
                entryXml.credit_quantity = entryJson.credit_quantity;
                entryXml.primary_credit_amount = entryJson.primary_credit_amount;
                entryXml.secondary_credit_amount = "";
                entryXml.natural_credit_currency = entryJson.natural_credit_currency;
                entryXml.currency = "";
                entryXml.settlement = "";
                entryXml.bill_date = "";
                entryXml.bill_id = "";
                entryXml.bill_type = "";
                entryXml.document_id = "";
                entryXml.document_date = "";
                //if (entryJson.auxiliary != null)
                //{
                entryXml.auxiliary_accounting = getXMLAuxiliary(entryJson.auxiliary);
                //}
                BidlinkU8Interface.Models.XmlVouch.Detail detail = new BidlinkU8Interface.Models.XmlVouch.Detail();
                BidlinkU8Interface.Models.XmlVouch.Cash_flow_statement cashflow = new Models.XmlVouch.Cash_flow_statement();
                cashflow.value = "";
                BidlinkU8Interface.Models.XmlVouch.Code_remark_statement coderemark = new Models.XmlVouch.Code_remark_statement();
                coderemark.value = "";
                detail.cash_flow_statement = cashflow;
                detail.code_remark_statement = coderemark;
                entryXml.detail = detail;

                entrys.Add(entryXml);
            }
            //贷方
            foreach (Entry entryJson in vouchreq.voucher.credit.entry)
            {
                entry entryXml = new entry();
                entryXml.entry_id = entryJson.entry_id;
                entryXml.account_code = entryJson.account_code;
                entryXml.@abstract = entryJson.@abstract;
                entryXml.currency = entryJson.currency;
                if ((entryJson.unit_price == null) || (entryJson.unit_price == 0))
                {
                    entryXml.unit_price = "";
                }
                else
                {
                    entryXml.unit_price = entryJson.unit_price.ToString();
                }
                if ((entryJson.exchange_rate1 == null) || (entryJson.exchange_rate1 == 0))
                {
                    entryXml.exchange_rate1 = "";
                }
                else
                {
                    entryXml.exchange_rate1 = entryJson.exchange_rate1.ToString();
                }
                if ((entryJson.exchange_rate2 == null) || (entryJson.exchange_rate2 == 0))
                {
                    entryXml.exchange_rate2 = "0";
                }
                else
                {
                    entryXml.exchange_rate2 = entryJson.exchange_rate2.ToString();
                };
                entryXml.debit_quantity = entryJson.debit_quantity;
                entryXml.primary_debit_amount = entryJson.primary_debit_amount;
                entryXml.secondary_debit_amount = "";
                entryXml.natural_debit_currency = entryJson.natural_debit_currency;
                entryXml.credit_quantity = entryJson.credit_quantity;
                entryXml.primary_credit_amount = entryJson.primary_credit_amount;
                entryXml.secondary_credit_amount = "";
                entryXml.natural_credit_currency = entryJson.natural_credit_currency;
                entryXml.currency = "";
                entryXml.settlement = "";
                entryXml.bill_date = "";
                entryXml.bill_id = "";
                entryXml.bill_type = "";
                entryXml.document_id = "";
                entryXml.document_date = "";
                //if (entryJson.auxiliary != null)
                //{
                entryXml.auxiliary_accounting = getXMLAuxiliary(entryJson.auxiliary);
                //}
                BidlinkU8Interface.Models.XmlVouch.Detail detail = new BidlinkU8Interface.Models.XmlVouch.Detail();
                BidlinkU8Interface.Models.XmlVouch.Cash_flow_statement cashflow = new Models.XmlVouch.Cash_flow_statement();
                cashflow.value = "";
                BidlinkU8Interface.Models.XmlVouch.Code_remark_statement coderemark = new Models.XmlVouch.Code_remark_statement();
                coderemark.value = "";
                detail.cash_flow_statement = cashflow;
                detail.code_remark_statement = coderemark;
                entryXml.detail = detail;
                entrys.Add(entryXml);
            }

            voucher.voucher_body = entrys;
            voucher.voucher_head = head;
            ufinterface.voucher = voucher;


            return ufinterface;
        }

        private static List<item> getXMLAuxiliary(Auxiliary auxi)
        {
            List<item> xmlauxis = new List<item>();

            foreach (PropertyInfo pi in auxi.GetType().GetProperties())
            {
                object outObj = auxi.GetType().GetProperty(pi.Name).GetValue(auxi, null);
                item xmlauxi = new item();
                xmlauxi.itemname = pi.Name;
                xmlauxi.value = "";
                if (outObj != null)
                {
                    //xmlauxi.value = "";
                    xmlauxi.value = outObj.ToString();

                }
                xmlauxis.Add(xmlauxi);
            }


            return xmlauxis;
        }
    }
}