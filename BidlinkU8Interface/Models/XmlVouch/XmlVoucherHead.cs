using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BidlinkU8Interface.Models.XMLVouch
{
    public class XmlVoucherHead
    {
        public String company { get; set; }
        public String voucher_type { get; set; }//凭证类别
        public int fiscal_year { get; set; }//凭证所属的会计年度，不填写取当前年
        public int accounting_period { get; set; }//所属的会计期间，不填写取当前月份
        public String voucher_id { get; set; }//凭证号
        public String date { get; set; }//制单日期
        public String auditdate { get; set; }
        public String enter { get; set; }//制单人名称
        public String cashier { get; set; }//出纳名称
        public String signature{ get; set; }
        public String checker { get; set; }
        public int attachment_number { get; set; }//附单据数
        public String voucher_making_system { get; set; }//外部系统类型，如果需要传入外部凭证业务号，此属性必须传入固定值 CDM
        public String posting_date{ get; set; }
        public String posting_person{ get; set; }
        public String memo1{ get; set; }
        public String memo2{ get; set; }
        public String revokeflag{ get; set; }
        public String reserve1 { get; set; }
        public String reserve2 { get; set; }//外部凭证业务号，如果传入此属性，voucher_making_system 必须传入固定值 CDM。外部凭证U8里无法删除、修改，可以通过作废接口作废voucher/cancel即可，作废接口id参数就是对应此reserve2的值
        
    }
}