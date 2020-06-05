using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;
using YDS6000.DAL.Report;

namespace YDS6000.BLL.Report
{
    /// <summary>
    /// 报表定制类
    /// </summary>
    partial class ReportBLL
    {
        public DataTable GetYdModuleUseValList(int co_id, int meter_id,DateTime time,string dataType)
        {
            DateTime start = time, end = time;
            int cnt = dataType.Equals("hour") ? 24 : dataType.Equals("day") ? cnt = time.AddMonths(1).AddDays(-1).Day : 12;
            if (dataType.Equals("hour"))
            {
                start = new DateTime(time.Year, time.Month, time.Day);
                end = new DateTime(time.Year, time.Month, time.Day, 23, 59, 59);
                cnt = 24;
            }
            else if (dataType.Equals("day"))
            {
                start = new DateTime(time.Year, time.Month, 1);
                end = start.AddMonths(1).AddDays(-1);
                cnt = end.Day;
            }
            else
            {
                start = new DateTime(time.Year, 1, 1);
                end = start.AddYears(1).AddDays(-1);
                cnt = 12;
            }
            StringBuilder splitMdQuery = new StringBuilder();
            DataTable dtRst = this.GetYdModuleUseValTabSchema();
            DataTable dtSource = dal.GetYdModuleListParams(co_id, meter_id);
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["Module_id"], dtRst.Columns["ModuleAddr"] };
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            foreach (DataRow dr in dtSource.Rows)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitMdQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["Module_id"]), ",")))
                {
                    if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
                        splitMdQuery.Append(",");
                    splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                }
            }
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), start, end, dataType);
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                scale = scale == 0 ? 2 : scale;
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                DateTime tagtime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = (lastVal - firstVal) * multiply;
                useVal = Math.Round(useVal, scale, MidpointRounding.AwayFromZero);
                DataRow addDr = dtRst.Rows.Find(new object[] { dr["Module_id"], dr["ModuleAddr"] });
                if (addDr == null)
                {
                    addDr = dtRst.NewRow();
                    addDr["Module_id"] = dr["Module_id"];
                    addDr["ModuleAddr"] = dr["ModuleAddr"];
                    addDr["Cnt"] = cnt;
                    addDr["ModuleName"] = curDr["ModuleName"];
                    addDr["RoomName"] = curDr["CoName"];
                    addDr["StrcName"] = curDr["CoStrcName"];
                    dtRst.Rows.Add(addDr);
                }
                string clsName = "V" + (dataType.Equals("hour") ? tagtime.Hour + 1 : dataType.Equals("day") ? tagtime.Day : tagtime.Month).ToString().PadLeft(2, '0');
                addDr[clsName] = CommFunc.ConvertDBNullToDecimal(addDr[clsName]) + useVal;
            }
            return dtRst;

        }

        /// <summary>
        /// 回来能耗统计分析(区间粒度)
        /// </summary>
        /// <param name="selKey">选择的对象</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public DataTable GetYdModuleCollectValList(int co_id, int meter_id, DateTime start, DateTime end)
        {
            StringBuilder splitMdQuery = new StringBuilder();
            DataTable dtRst = this.GetYdModuleCollectValTabSchema();
            DataTable dtSource = dal.GetYdModuleListParams(co_id, meter_id);
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["Module_id"], dtRst.Columns["ModuleAddr"] };
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            //
            foreach (DataRow dr in dtSource.Rows)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitMdQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["Module_id"]), ",")))
                {
                    if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
                        splitMdQuery.Append(",");
                    splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                }
                DataRow addDr = dtRst.Rows.Find(new object[] { dr["Module_id"], dr["ModuleAddr"] });
                if (addDr == null)
                {
                    addDr = dtRst.NewRow();
                    addDr["Module_id"] = dr["Module_id"];
                    addDr["ModuleAddr"] = dr["ModuleAddr"];
                    addDr["Start"] = start;
                    addDr["End"] = end;
                    addDr["ModuleName"] = dr["ModuleName"];
                    addDr["RoomName"] = dr["CoName"];
                    addDr["StrcName"] = dr["CoStrcName"];
                    dtRst.Rows.Add(addDr);
                }
            }
            this.GetYdModuleCollectValData("FirstVal", splitMdQuery.ToString(), start, end, ref dtRst, ref dtSource);
            this.GetYdModuleCollectValData("LastVal", splitMdQuery.ToString(), start, end, ref dtRst, ref dtSource);
            return dtRst;
        }

        private void GetYdModuleCollectValData(string periods,string splitMdQuery, DateTime start,DateTime end, ref DataTable dtRst, ref DataTable dtSource)
        {
            DataTable dtUse = null;
            if (periods.ToLower().Equals("FirstVal".ToLower()))
                 dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery, start, start, "day");
            else
                dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery, end, end, "day");

            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                scale = scale == 0 ? 2 : scale;
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                DateTime tagtime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]) * multiply;
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]) * multiply;
                firstVal = Math.Round(firstVal, scale, MidpointRounding.AwayFromZero);
                lastVal = Math.Round(lastVal, scale, MidpointRounding.AwayFromZero);
                //
                DataRow addDr = dtRst.Rows.Find(new object[] { dr["Module_id"], dr["ModuleAddr"] });
                if (addDr == null)
                {
                    addDr = dtRst.NewRow();
                    addDr["Module_id"] = dr["Module_id"];
                    addDr["ModuleAddr"] = dr["ModuleAddr"];
                    addDr["Start"] = start;
                    addDr["End"] = end;
                    addDr["ModuleName"] = curDr["ModuleName"];
                    addDr["RoomName"] = curDr["CoName"];
                    addDr["StrcName"] = curDr["CoStrcName"];
                    dtRst.Rows.Add(addDr);
                }
                if (periods.ToLower().Equals("FirstVal".ToLower()))
                    addDr["FirstVal"] = firstVal;
                else
                    addDr["LastVal"] = lastVal;
            }
        }

        private DataTable GetYdModuleUseValTabSchema()
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("Module_id", typeof(System.Int32));// 回路ID号
            dtRst.Columns.Add("ModuleAddr", typeof(System.String));// 回路地址
            dtRst.Columns.Add("Cnt", typeof(System.Int32));//当前值列数
            dtRst.Columns.Add("V01", typeof(System.Decimal));//值01
            dtRst.Columns.Add("V02", typeof(System.Decimal));//值02
            dtRst.Columns.Add("V03", typeof(System.Decimal));//值03
            dtRst.Columns.Add("V04", typeof(System.Decimal));//值04
            dtRst.Columns.Add("V05", typeof(System.Decimal));//值05
            dtRst.Columns.Add("V06", typeof(System.Decimal));//值06
            dtRst.Columns.Add("V07", typeof(System.Decimal));//值07
            dtRst.Columns.Add("V08", typeof(System.Decimal));//值08
            dtRst.Columns.Add("V09", typeof(System.Decimal));//值09
            dtRst.Columns.Add("V10", typeof(System.Decimal));//值10
            dtRst.Columns.Add("V11", typeof(System.Decimal));//值11
            dtRst.Columns.Add("V12", typeof(System.Decimal));//值12
            dtRst.Columns.Add("V13", typeof(System.Decimal));//值13
            dtRst.Columns.Add("V14", typeof(System.Decimal));//值14
            dtRst.Columns.Add("V15", typeof(System.Decimal));//值15
            dtRst.Columns.Add("V16", typeof(System.Decimal));//值16
            dtRst.Columns.Add("V17", typeof(System.Decimal));//值17
            dtRst.Columns.Add("V18", typeof(System.Decimal));//值18
            dtRst.Columns.Add("V19", typeof(System.Decimal));//值19
            dtRst.Columns.Add("V20", typeof(System.Decimal));//值20
            dtRst.Columns.Add("V21", typeof(System.Decimal));//值21
            dtRst.Columns.Add("V22", typeof(System.Decimal));//值22
            dtRst.Columns.Add("V23", typeof(System.Decimal));//值23
            dtRst.Columns.Add("V24", typeof(System.Decimal));//值24
            dtRst.Columns.Add("V25", typeof(System.Decimal));//值25
            dtRst.Columns.Add("V26", typeof(System.Decimal));//值26
            dtRst.Columns.Add("V27", typeof(System.Decimal));//值27
            dtRst.Columns.Add("V28", typeof(System.Decimal));//值28
            dtRst.Columns.Add("V29", typeof(System.Decimal));//值29
            dtRst.Columns.Add("V30", typeof(System.Decimal));//值30
            dtRst.Columns.Add("V31", typeof(System.Decimal));//值31
            dtRst.Columns.Add("ModuleName", typeof(System.String));// 回路名称
            dtRst.Columns.Add("RoomName", typeof(System.String));// 站点名称
            dtRst.Columns.Add("StrcName", typeof(System.String));// 位置
            return dtRst;
        }

        private DataTable GetYdModuleCollectValTabSchema()
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("Module_id", typeof(System.Int32));// 回路ID号
            dtRst.Columns.Add("ModuleAddr", typeof(System.String));// 回路地址
            dtRst.Columns.Add("Start", typeof(System.DateTime));//当前开始时间
            dtRst.Columns.Add("End", typeof(System.DateTime));//当前结束时间
            dtRst.Columns.Add("FirstVal", typeof(System.Decimal));//当前开始值
            dtRst.Columns.Add("LastVal", typeof(System.Decimal));//当前结束值
            dtRst.Columns.Add("ModuleName", typeof(System.String));// 回路名称
            dtRst.Columns.Add("RoomName", typeof(System.String));// 站点名称
            dtRst.Columns.Add("StrcName", typeof(System.String));// 位置
            return dtRst;
        }
    }
}
