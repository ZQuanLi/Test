using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.ExpApp.Energy
{
    public class EnergyBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private YDS6000.DAL.ExpApp.Energy.EnergyDAL dal = null;

        public EnergyBLL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            dal = new YDS6000.DAL.ExpApp.Energy.EnergyDAL(ledger, uid);
        }

        /// <summary>
        /// 本月用电信息
        /// </summary>
        /// <param name="module_id"></param>
        /// <returns></returns>
        public DataTable GetMonthUseVal(int module_id)
        {
            DataTable dtSource = dal.GetMonthUseVal(module_id);
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
            #region 获取存储数据
            DateTime fm = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime to = fm.AddMonths(1).AddDays(-1);
            decimal allUseVal = 0, allUseAmt = 0;
            //
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), fm, to, "month", splitTyQuery.ToString());
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;

                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                decimal price =  CommFunc.ConvertDBNullToDecimal(curDr["Price"]);
                scale = scale == 0 ? 2 : scale;
                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;
                useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);
                decimal useAmt = Math.Round(useVal * price, 2, MidpointRounding.AwayFromZero);
                allUseVal = allUseVal + useVal;
                allUseAmt = allUseAmt + useAmt;
            }
            #endregion 
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("UseVal", typeof(System.Decimal));
            dtRst.Columns.Add("UseAmt", typeof(System.Decimal));
            dtRst.Rows.Add(new object[] { allUseVal, allUseAmt });
            return dtRst;
        }


        /// <summary>
        /// 支付购电
        /// </summary>
        /// <param name="module_id"></param>
        /// <returns></returns>
        public DataTable GetPayUseVal(int module_id)
        {
            return dal.GetPayUseVal(module_id);
        }

        /// <summary>
        /// 电量电费
        /// </summary>
        /// <param name="module_id"></param>
        /// <returns></returns>
        public DataTable GetElectricity(int module_id)
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("UseVal", typeof(System.Decimal));/*当月电力*/
            dtRst.Columns.Add("UseAmt", typeof(System.Decimal));/*当月电力*/
            dtRst.Columns.Add("UpUseVal", typeof(System.Decimal));/*上一月电量*/
            dtRst.Columns.Add("UpUseAmt", typeof(System.Decimal));/*上一月电费*/
            dtRst.Columns.Add("LastVal", typeof(System.Decimal));/*当前表码值*/
            //
            dtRst.Rows.Add(new object[] { 0, 0, 0, 0, 0 });
            DataRow rstDr = dtRst.Rows[0];
            //
            DataTable dtSource = dal.GetPayUseVal(module_id);
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
                rstDr["LastVal"] = dr["LastVal"];
            }
            #region 本月获取存储数据
            DateTime fm = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime to = fm.AddMonths(1).AddDays(-1);
            //
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), fm, to, "month", splitTyQuery.ToString());
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                if (CommFunc.ConvertDBNullToInt32(curDr["Co_id"]) != CommFunc.ConvertDBNullToInt32(dr["Co_id"]))  /*必须相同房间才计算*/
                    continue;
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
                rstDr["UseVal"] = CommFunc.ConvertDBNullToDecimal(rstDr["UseVal"]) + useVal;
                rstDr["UseAmt"] = CommFunc.ConvertDBNullToDecimal(rstDr["UseAmt"]) + useAmt;
            }
            #endregion
            #region 上月月获取存储数据
            fm = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            to = fm.AddDays(-1);
            fm = new DateTime(to.Year, to.Month, 1);
            //
            dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), fm, to, "month", splitTyQuery.ToString());
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                if (CommFunc.ConvertDBNullToInt32(curDr["Co_id"]) != CommFunc.ConvertDBNullToInt32(dr["Co_id"]))  /*必须相同房间才计算*/
                    continue;
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
                rstDr["UpUseVal"] = CommFunc.ConvertDBNullToDecimal(rstDr["UpUseVal"]) + useVal;
                rstDr["UpUseAmt"] = CommFunc.ConvertDBNullToDecimal(rstDr["UpUseAmt"]) + useAmt;
            }
            #endregion
            return dtRst;
        }
        /// <summary>
        /// 购电记录
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataTable GetPayLog(int module_id, DateTime startTime, DateTime endTime)
        {
            return dal.GetPayLog(module_id, startTime, endTime);
        }

        public DataTable GetMonthAnalysis(int module_id, DateTime time)
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("Month", typeof(System.Int32));/*月份*/
            dtRst.Columns.Add("UseVal", typeof(System.Decimal));/*当月电力*/
            dtRst.Columns.Add("UseAmt", typeof(System.Decimal));/*当月电力*/
            dtRst.Columns.Add("Step", typeof(System.Int32));
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["Month"] };
            //
            DataTable dtSource = dal.GetPayUseVal(module_id);
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
            #region 本月获取存储数据
            DateTime fm = new DateTime(time.Year, 1, 1);
            DateTime to = fm.AddYears(1).AddDays(-1);
            //
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), fm, to, "month", splitTyQuery.ToString());
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                if (CommFunc.ConvertDBNullToInt32(curDr["Co_id"]) != CommFunc.ConvertDBNullToInt32(dr["Co_id"]))  /*必须相同房间才计算*/
                    continue;

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
                ///////////////////////////////
                DataRow addDr = dtRst.Rows.Find(tagTime.Month);
                if (addDr == null)
                {
                    addDr = dtRst.NewRow();
                    addDr["Month"] = tagTime.Month;
                    addDr["UseVal"] = 0;
                    addDr["UseAmt"] = 0;
                    addDr["Step"] = 1;
                    dtRst.Rows.Add(addDr);
                }
                addDr["UseVal"] = CommFunc.ConvertDBNullToDecimal(addDr["UseVal"]) + useVal;
                addDr["UseAmt"] = CommFunc.ConvertDBNullToDecimal(addDr["UseAmt"]) + useAmt;
            }
            int i = 0;
            while (++i <= 12)
            {
                DataRow addDr = dtRst.Rows.Find(i);
                if (addDr == null)
                {
                    addDr = dtRst.NewRow();
                    addDr["Month"] = i;
                    addDr["UseVal"] = 0;
                    addDr["UseAmt"] = 0;
                    addDr["Step"] = 0;/*不存在月份*/
                    dtRst.Rows.Add(addDr);
                }
            }
            #endregion            
            return dtRst;
        }


        /// <summary>
        /// 增加记录log
        /// </summary>
        /// <param name="pay"></param>
        /// <returns></returns>
        public bool AddPayLog(v4_pay_logVModel pay)
        {
            return dal.AddPayLog(pay);
        }
    }
}
