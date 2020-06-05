using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.PayLog
{
    public partial class ExpYdPayLogBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";
        private readonly YDS6000.DAL.Exp.PayLog.ExpYdPayLogDAL dal = null;
        public ExpYdPayLogBLL(string _project, int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            this.Project = _project;
            dal = new YDS6000.DAL.Exp.PayLog.ExpYdPayLogDAL(_project, _ledger, _uid);
        }

        public DataTable GetYdPayLogOnList(string StrcName, string CoName, int d_ways, string Create_by, DateTime StartTime, DateTime EndTime)
        {
            DataTable dtSource = dal.GetYdPayLogOnList(StrcName, CoName, d_ways, Create_by, StartTime, EndTime);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            dtSource.Columns.Add("PayTypeS", typeof(System.String));
            dtSource.Columns.Add("IsPayS", typeof(System.String));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["RowId"] = ++RowId;
                int payType = CommFunc.ConvertDBNullToInt32(dr["PayType"]);
                int ispay = CommFunc.ConvertDBNullToInt32(dr["IsPay"]);
                if (payType == (int)PayType.pay)
                    dr["PayTypeS"] = "充值";
                if (payType == (int)PayType.refund)
                {
                    dr["PayAmt"] = -CommFunc.ConvertDBNullToDecimal(dr["PayAmt"]);
                    dr["PayVal"] = -CommFunc.ConvertDBNullToDecimal(dr["PayVal"]);
                    dr["PayTypeS"] = "退费";
                }
                if (payType == (int)PayType.present)
                    dr["PayTypeS"] = "赠电";
                if (payType == (int)PayType.reback)
                {
                    dr["PayAmt"] = -CommFunc.ConvertDBNullToDecimal(dr["PayAmt"]);
                    dr["PayVal"] = -CommFunc.ConvertDBNullToDecimal(dr["PayVal"]);
                    dr["PayTypeS"] = "退赠电";
                }
                if (ispay == 0)
                    dr["IsPayS"] = "现金支付";
                if ((ispay >= 10 && ispay < 20) || ispay == 1)
                    dr["IsPayS"] = "微信支付";
            }
            return dtSource;
        }

        public DataTable GetHomePay( string StartTime, string EndTime)
        {
            DataTable dtSource = dal.GetHomePay( StartTime, EndTime);            
            return dtSource;
        }
    }

}
