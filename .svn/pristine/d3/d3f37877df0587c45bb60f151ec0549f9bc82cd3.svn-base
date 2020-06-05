using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;
using YDS6000.Models;

namespace YDS6000.BLL.Energy.Report
{
    partial class ReportBLL
    {
        /// <summary>
        /// 获取分项统计(比亚迪)
        /// </summary>
        /// <param name="co_id">支路ID号</param>
        /// <param name="time">时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="dataType">类型 日=day月=month年year</param>
        /// <returns></returns>
        public object GetEnergyItemForByd(int co_id, DateTime time,DateTime? endTime, string dataType)
        {
            DataTable dtRst = this.GetEnergyItemTimeTabSchema();
            int cnt = 24;
            DateTime fm = time, to = time;
            if (dataType.ToLower().Equals("day"))
            {
                fm = new DateTime(time.Year, time.Month, time.Day,time.Hour,time.Minute,time.Second);
                to = new DateTime(endTime.Value.Year, endTime.Value.Month, endTime.Value.Day, endTime.Value.Hour, endTime.Value.Minute, endTime.Value.Second);
                cnt = (int)(to - fm).TotalHours + 1;
                dataType = "hour";
                dtRst = this.GetEnergyItemTimeTabSchemaForByd(fm, to);
            }
            else if (dataType.ToLower().Equals("month"))
            {
                fm = new DateTime(time.Year, time.Month, 1);
                to = fm.AddMonths(1).AddDays(-1);
                cnt = to.Day;
                dataType = "day";
            }
            else if (dataType.ToLower().Equals("year"))
            {
                fm = new DateTime(time.Year, 1, 1);
                to = new DateTime(time.Year, 12, 1);
                cnt = 12;
                dataType = "month";
            }
            else
            {
                throw new Exception("查询类型错误");
            }
            DataTable dtSource = dal.GetEnergyMdList(co_id, "");
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["EnergyItemCode"] };
            //
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

                if (dr["EnergyItemCode"].ToString().Equals("01C00") || dr["EnergyItemCode"].ToString().Equals("01D00"))
                {
                    dr["EnergyItemCode"] = "01A00";
                    dr["EnergyItemName"] = "照明插座用电";
                }

                DataRow addDr = dtRst.Rows.Find(dr["EnergyItemCode"]);
                if (addDr == null)
                {
                    addDr = dtRst.NewRow();
                    addDr["EnergyItemCode"] = dr["EnergyItemCode"];
                    addDr["Cnt"] = cnt;
                    addDr["EnergyItemName"] = dr["EnergyItemName"];
                    addDr["ParentItemCode"] = dr["ParentItemCode"];
                    addDr["ParentItemName"] = dr["ParentItemName"];
                    dtRst.Rows.Add(addDr);
                }
            }
            #region 获取存储数据
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), fm, to, dataType, splitTyQuery.ToString());
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;

                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                scale = scale == 0 ? 2 : scale;

                DataRow addDr = dtRst.Rows.Find(curDr["EnergyItemCode"]);
                if (addDr == null) continue;
                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;

                useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

                if (dataType.Equals("hour"))
                    addDr[tagTime.ToString("yyyyMMdd-HH")] = CommFunc.ConvertDBNullToDecimal(addDr[tagTime.ToString("yyyyMMdd-HH")]) + useVal;
                    //addDr["V" + (tagTime.Hour + 1).ToString().PadLeft(2, '0')] = CommFunc.ConvertDBNullToDecimal(addDr["V" + (tagTime.Hour + 1).ToString().PadLeft(2, '0')]) + useVal;
                else if (dataType.Equals("day"))
                    addDr["V" + tagTime.Day.ToString().PadLeft(2, '0')] = CommFunc.ConvertDBNullToDecimal(addDr["V" + tagTime.Day.ToString().PadLeft(2, '0')]) + useVal;
                else if (dataType.Equals("month"))
                    addDr["V" + tagTime.Month.ToString().PadLeft(2, '0')] = CommFunc.ConvertDBNullToDecimal(addDr["V" + tagTime.Month.ToString().PadLeft(2, '0')]) + useVal;
                addDr["UseVal"] = CommFunc.ConvertDBNullToDecimal(addDr["UseVal"]) + useVal;
            }
            #endregion
            #region 获取结果集数据
            //var res1 = from s1 in dtRst.AsEnumerable()
            //           select new
            //           {
            //               EnergyItemName = CommFunc.ConvertDBNullToString(s1["EnergyItemName"]),
            //               Cnt = CommFunc.ConvertDBNullToInt32(s1["Cnt"]),
            //               V01 = CommFunc.ConvertDBNullToDecimal(s1["V01"]),
            //               V02 = CommFunc.ConvertDBNullToDecimal(s1["V02"]),
            //               V03 = CommFunc.ConvertDBNullToDecimal(s1["V03"]),
            //               V04 = CommFunc.ConvertDBNullToDecimal(s1["V04"]),
            //               V05 = CommFunc.ConvertDBNullToDecimal(s1["V05"]),
            //               V06 = CommFunc.ConvertDBNullToDecimal(s1["V06"]),
            //               V07 = CommFunc.ConvertDBNullToDecimal(s1["V07"]),
            //               V08 = CommFunc.ConvertDBNullToDecimal(s1["V08"]),
            //               V09 = CommFunc.ConvertDBNullToDecimal(s1["V09"]),
            //               V10 = CommFunc.ConvertDBNullToDecimal(s1["V10"]),
            //               V11 = CommFunc.ConvertDBNullToDecimal(s1["V11"]),
            //               V12 = CommFunc.ConvertDBNullToDecimal(s1["V12"]),
            //               V13 = CommFunc.ConvertDBNullToDecimal(s1["V13"]),
            //               V14 = CommFunc.ConvertDBNullToDecimal(s1["V14"]),
            //               V15 = CommFunc.ConvertDBNullToDecimal(s1["V15"]),
            //               V16 = CommFunc.ConvertDBNullToDecimal(s1["V16"]),
            //               V17 = CommFunc.ConvertDBNullToDecimal(s1["V17"]),
            //               V18 = CommFunc.ConvertDBNullToDecimal(s1["V18"]),
            //               V19 = CommFunc.ConvertDBNullToDecimal(s1["V19"]),
            //               V20 = CommFunc.ConvertDBNullToDecimal(s1["V20"]),
            //               V21 = CommFunc.ConvertDBNullToDecimal(s1["V21"]),
            //               V22 = CommFunc.ConvertDBNullToDecimal(s1["V22"]),
            //               V23 = CommFunc.ConvertDBNullToDecimal(s1["V23"]),
            //               V24 = CommFunc.ConvertDBNullToDecimal(s1["V24"]),
            //               V25 = CommFunc.ConvertDBNullToDecimal(s1["V25"]),
            //               V26 = CommFunc.ConvertDBNullToDecimal(s1["V26"]),
            //               V27 = CommFunc.ConvertDBNullToDecimal(s1["V27"]),
            //               V28 = CommFunc.ConvertDBNullToDecimal(s1["V28"]),
            //               V29 = CommFunc.ConvertDBNullToDecimal(s1["V29"]),
            //               V30 = CommFunc.ConvertDBNullToDecimal(s1["V30"]),
            //               V31 = CommFunc.ConvertDBNullToDecimal(s1["V31"]),
            //           };
            List<object> dd = new List<object>();
            //Dictionary<string, object> dd = new Dictionary<string, object>();
            var res2 = from s1 in dtRst.AsEnumerable()
                       where !string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(s1["ParentItemCode"]))
                       group s1 by new { ParentItemCode = CommFunc.ConvertDBNullToString(s1["ParentItemCode"]) } into g1
                       select new
                       {
                           ParentItemCode = g1.Key.ParentItemCode,
                           ParentItemName = CommFunc.ConvertDBNullToString(g1.First()["ParentItemName"])
                       };
            foreach (var p1 in res2)
            {
                var res3 = from s1 in dtRst.AsEnumerable()
                           where CommFunc.ConvertDBNullToString(s1["ParentItemCode"]).Equals(p1.ParentItemCode)
                           select new
                           {
                               EnergyItemName = CommFunc.ConvertDBNullToString(s1["EnergyItemName"]),
                               UseVal = CommFunc.ConvertDBNullToDecimal(s1["UseVal"]),
                           };
                dd.Add(new { Key = p1.ParentItemName, Value = res3.ToList() });
                //dd.Add(p1.ParentItemName, res3.ToList());
            }
            object rst = new { rows = dtRst, pie = dd.ToList() };
            return rst;
            #endregion
        }
        private DataTable GetEnergyItemTimeTabSchemaForByd(DateTime fm,DateTime to)
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("EnergyItemCode", typeof(System.String));// 分项ID号
            dtRst.Columns.Add("Cnt", typeof(System.Int32));//当前值列数
            while(fm<=to)
            {
                dtRst.Columns.Add(fm.ToString("yyyyMMdd-HH"), typeof(System.Decimal));
                fm = fm.AddHours(1);
            }
            dtRst.Columns.Add("UseVal", typeof(System.Decimal));//汇总值
            dtRst.Columns.Add("EnergyItemName", typeof(System.String));// 分项名称
            dtRst.Columns.Add("ParentItemCode", typeof(System.String));// 父分项ID号
            dtRst.Columns.Add("ParentItemName", typeof(System.String));// 父分项名称
            return dtRst;
        }
    }
}
