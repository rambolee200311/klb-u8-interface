using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BidlinkU8Interface.Models.GLVouch
{
    public class CashFlow
    {
        public String cexch_name { get; set; }//币种
        public String RowGuid { get; set; }//行标识
        public String iYPeriod { get; set; }//年期间
        public String iyear { get; set; }//年
        public String csign { get; set; }//凭证类别字
        public decimal nc_s { get; set; }//数量贷方
        public String mc_f { get; set; }//外币贷方
        public decimal nd_s { get; set; }//数量借方
        public decimal md_f { get; set; }//外币借方
        public String ccode { get; set; }//科目编码
        public decimal mc { get; set; }//贷方金额
        public decimal md { get; set; }//借方金额
        public String cCashItem { get; set; }//现金项目
        public String cash_item { get; set; }//现金项目
        public decimal natural_credit_currency { get; set; }//本币贷方发生额*与本币借方发生额不能同时为空
        public decimal natural_debit_currency { get; set; }//本币借方发生额*与本币贷方发生额不能同时为空
        public String cdept_id { get; set; }//部门
        public String cperson_id { get; set; }//人员
        public String ccus_id { get; set; }//客户
        public String csup_id { get; set; }//供应商
        public String citem_class { get; set; }//项目大类
        public String citem_id { get; set; }//项目档案
        public String cDefine1 { get; set; }//自定义字段1
        public String cDefine2 { get; set; }//自定义字段2
        public String cDefine3 { get; set; }//自定义字段3
        public String cDefine4 { get; set; }//自定义字段4
        public String cDefine5 { get; set; }//自定义字段5
        public String cDefine6 { get; set; }//自定义字段6
        public String cDefine7 { get; set; }//自定义字段7
        public String cDefine8 { get; set; }//自定义字段8
        public String cDefine9 { get; set; }//自定义字段9
        public String cDefine10 { get; set; }//自定义字段10
        public String cDefine11 { get; set; }//自定义字段11
        public String cDefine12 { get; set; }//自定义字段12
        public String cDefine13 { get; set; }//自定义字段13
        public String cDefine14 { get; set; }//自定义字段14
        public String cDefine15 { get; set; }//自定义字段15
        public String cDefine16 { get; set; }//自定义字段16

    }
}