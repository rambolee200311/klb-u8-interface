using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
namespace BidlinkU8Interface.Models.XMLVouch
{
    public class entry
    {
        public int entry_id { get; set; }//分录号
        public String account_code { get; set; }//科目编码
        public String @abstract { get; set; }//摘要
        public String currency { get; set; }//币种，默认人民币
        public String unit_price { get; set; }//单价,在科目有数量核算时，填写此项
        public String exchange_rate1 { get; set; }//汇率1，主辅币核算时使用，原币*汇率1=辅币，NC用户使用
        public String exchange_rate2 { get; set; }//汇率2，折本汇率，本币*汇率2=主币，U8用户使用
        public Decimal debit_quantity { get; set; }//借方数量,在科目有数量核算时，填写此项
        public Decimal credit_quantity { get; set; }//贷方数量,在科目有数量核算时，填写此项
        public Decimal primary_debit_amount { get; set; }//原币借方发生额
        public String secondary_debit_amount { get; set; }//辅币借方发生额
        public Decimal natural_debit_currency { get; set; }//本币借方发生额*与本币贷方发生额不能同时为空
        public Decimal primary_credit_amount { get; set; }//原币贷方发生额
        public String secondary_credit_amount { get; set; }//辅币贷方发生额
        public Decimal natural_credit_currency { get; set; }//本币贷方发生额*与本币借方发生额不能同时为空
        
        public String settlement{ get; set; }
        public String document_id{ get; set; }
        public String document_date{ get; set; }
        public String bill_type{ get; set; }
        public String bill_id{ get; set; }
        public String bill_date { get; set; }
        public List<item> auxiliary_accounting { get; set; }
        [XmlElementAttribute("detail", IsNullable = true)]
        public BidlinkU8Interface.Models.XmlVouch.Detail detail { get; set; }

    }
}