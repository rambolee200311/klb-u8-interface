using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BidlinkU8Interface.Models.Receive
{
    public class InDetail
    {
        public string oa_did { get; set; }//明细行号
        public decimal amount { get; set; }//金额（含税金额）
        public string depcode { get; set; }//部门名称
        public string personcode { get; set; }//人员姓名
        public string cuscode { get; set; }//客户名称
        public string itemclass { get; set; }//项目名称
        public string itemcode { get; set; }//产品名称
        public string ccode { get; set; }//会计科目
        public string memo { get; set; }//备注
    }
}