using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Alarm
{
    partial class AlarmBLL
    {
        public DataTable GetYdAlarmOfChargeList(string CoStrcName, string CoName, string IsPay, DateTime Start, DateTime End)
        {
            DataTable dtSource = dal.GetYdAlarmOfChargeList(CoStrcName, CoName, IsPay, Start, End);
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
                    dr["PayTypeS"] = "退费";
                if (payType == (int)PayType.present)
                    dr["PayTypeS"] = "赠电";
                if (payType == (int)PayType.reback)
                    dr["PayTypeS"] = "退赠电";
                if (ispay == 0)
                    dr["IsPayS"] = "现金支付";
                if ((ispay >= 10 && ispay < 20) || ispay == 1)
                    dr["IsPayS"] = "微信支付";
            }
            return dtSource;
        }        
    }
}
