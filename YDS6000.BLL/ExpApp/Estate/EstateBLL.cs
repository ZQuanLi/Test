using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.ExpApp.Estate
{
    public class EstateBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private YDS6000.DAL.ExpApp.Estate.EstateDAL dal = null;

        public EstateBLL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            dal = new YDS6000.DAL.ExpApp.Estate.EstateDAL(ledger, uid);
        }

        public string GetAlarmInfo()
        {
            return dal.GetAlarmInfo();
        }
        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserInfo()
        {
            return dal.GetUserInfo();
        }
        /// <summary>
        /// 账单
        /// </summary>
        /// <returns></returns>
        public DataTable GetBill()
        {
            DataTable dtSource = dal.GetMdInfo();
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            StringBuilder splitMdQuery = new StringBuilder();
            StringBuilder splitTyQuery = new StringBuilder();
            foreach (DataRow dr in dtSource.Rows)
            {
                if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
                    splitMdQuery.Append(",");
                splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitTyQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["FunType"]), ",")))
                {
                    if (!string.IsNullOrEmpty(splitTyQuery.ToString()))
                        splitTyQuery.Append(",");
                    splitTyQuery.Append(CommFunc.ConvertDBNullToString(dr["FunType"]));
                }
            }

            DataTable dtRst = dal.GetBill();
            dtRst.Columns.Add("EleUseVal",typeof(System.Decimal));
            dtRst.Columns.Add("ElePrice", typeof(System.Decimal));
            dtRst.Columns.Add("EleUseAmt", typeof(System.Decimal));

            foreach (DataRow drRst in dtRst.Rows)
            {
                DateTime firstTime = CommFunc.ConvertDBNullToDateTime(drRst["FirstTime"]);
                DateTime lastTime = CommFunc.ConvertDBNullToDateTime(drRst["LastTime"]);
                DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), firstTime, lastTime, "day", splitTyQuery.ToString());
                foreach (DataRow dr in dtUse.Rows)
                {
                    DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                    if (curDr == null) continue;

                    int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                    decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                    decimal price = CommFunc.ConvertDBNullToDecimal(curDr["Price"]);
                    scale = scale == 0 ? 2 : scale;
                    DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                    decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                    decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                    decimal useVal = lastVal - firstVal;
                    useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);
                    decimal useAmt = Math.Round(useVal * price, 2, MidpointRounding.AwayFromZero);

                    drRst["EleUseVal"] = CommFunc.ConvertDBNullToDecimal(drRst["EleUseVal"]) + useVal;
                    drRst["ElePrice"] = price;
                    drRst["EleUseAmt"] = CommFunc.ConvertDBNullToDecimal(drRst["EleUseAmt"]) + useAmt;
                }
            }
            return dtRst;
        }

        public DataTable GetBillDetail_01(DateTime start, DateTime end)
        {
            DataTable dtSource = dal.GetMdInfo();
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            StringBuilder splitMdQuery = new StringBuilder();
            StringBuilder splitTyQuery = new StringBuilder();
            foreach (DataRow dr in dtSource.Rows)
            {
                if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
                    splitMdQuery.Append(",");
                splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitTyQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["FunType"]), ",")))
                {
                    if (!string.IsNullOrEmpty(splitTyQuery.ToString()))
                        splitTyQuery.Append(",");
                    splitTyQuery.Append(CommFunc.ConvertDBNullToString(dr["FunType"]));
                }
            }

            DataTable dtRst = dal.GetBillDetail_01(start, end);
            dtRst.Columns.Add("EleUseVal", typeof(System.Decimal));
            dtRst.Columns.Add("ElePrice", typeof(System.Decimal));
            dtRst.Columns.Add("EleUseAmt", typeof(System.Decimal));

            foreach (DataRow drRst in dtRst.Rows)
            {
                DateTime firstTime = CommFunc.ConvertDBNullToDateTime(drRst["FirstTime"]);
                DateTime lastTime = CommFunc.ConvertDBNullToDateTime(drRst["LastTime"]);
                DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), firstTime, lastTime, "day", splitTyQuery.ToString());
                foreach (DataRow dr in dtUse.Rows)
                {
                    DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                    if (curDr == null) continue;

                    int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                    decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                    decimal price = CommFunc.ConvertDBNullToDecimal(curDr["Price"]);
                    scale = scale == 0 ? 2 : scale;
                    DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                    decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                    decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                    decimal useVal = lastVal - firstVal;
                    useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);
                    decimal useAmt = Math.Round(useVal * price, 2, MidpointRounding.AwayFromZero);

                    drRst["EleUseVal"] = CommFunc.ConvertDBNullToDecimal(drRst["EleUseVal"]) + useVal;
                    drRst["ElePrice"] = price;
                    drRst["EleUseAmt"] = CommFunc.ConvertDBNullToDecimal(drRst["EleUseAmt"]) + useAmt;
                }
            }
            return dtRst;
        }

        public DataTable GetBillDetail_02(DateTime start, DateTime end)
        {
            return dal.GetBillDetail_02(start, end);
        }


        /// <summary>
        /// 增加记录log
        /// </summary>
        /// <param name="pay"></param>
        /// <returns></returns>
        public bool AddPayLog(v4_pay_logVModel pay,decimal rdAmt)
        {
            return dal.AddPayLog(pay, rdAmt);
        }
    }
}
