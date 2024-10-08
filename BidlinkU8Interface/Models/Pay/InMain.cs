using System;
using System.Collections.Generic;
using System.Text;

namespace BidlinkU8Interface.Models.Pay
{
    public class InMain
    {
        public int ds_sequence { get; set; }
        public string oa_mid { get; set; }//Oa应收单编号
        public string sscode { get; set; }//结算方式
        public string ddate { get; set; }//单据日期
        public string depcode { get; set; }//部门名称
        public string personcode { get; set; }//人员姓名
        public string vencode { get; set; }//客户名称
        public string itemclass { get; set; }//项目名称
        public string itemcode { get; set; }//产品名称
        public string ccode { get; set; }//会计科目
        public string memo { get; set; }//备注
        public string cmaker { get; set; }//制单人
        /// <summary>
        /// 明细
        /// </summary>
        public List<InDetail> details { get; set; }
    }
}
