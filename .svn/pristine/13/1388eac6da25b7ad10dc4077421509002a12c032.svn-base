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
        /// 获取分项统计
        /// </summary>
        /// <param name="co_id">支路ID号</param>
        /// <param name="time">时间</param>
        /// <param name="dataType">类型 日=day月=month年year</param>
        /// <returns></returns>
        public DataTable GetEnergyHisdata(int co_id, DateTime start, DateTime end,string moduleName, string funType)
        {
            DateTime dtFirst = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            DateTime dtLast = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("TagTime", typeof(System.DateTime));
            dtRst.Columns.Add("Module_id", typeof(System.Int32));
            dtRst.Columns.Add("Fun_id", typeof(System.Int32));
            dtRst.Columns.Add("ModuleAddr", typeof(System.String));
            dtRst.Columns.Add("ModuleName", typeof(System.String));
            dtRst.Columns.Add("MeterNo", typeof(System.String));
            dtRst.Columns.Add("LastVal", typeof(System.Decimal));
            dtRst.Columns.Add("FunName", typeof(System.String));
            dtRst.Columns.Add("CoName", typeof(System.String));
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["TagTime"], dtRst.Columns["Module_id"], dtRst.Columns["Fun_id"] };
            DataTable dtSource = dal.GetEnergyHisdata(co_id, moduleName, funType);
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            StringBuilder splitMdQuery = new StringBuilder();
            StringBuilder splitTyQuery = new StringBuilder();
            foreach (DataRow dr in dtSource.Rows)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitMdQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["Module_id"]), ",")))
                {
                    if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
                        splitMdQuery.Append(",");
                    splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitTyQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["FunType"]), ",")))
                {
                    if (!string.IsNullOrEmpty(splitTyQuery.ToString()))
                        splitTyQuery.Append(",");
                    splitTyQuery.Append(CommFunc.ConvertDBNullToString(dr["FunType"]));
                }
            }
            DateTime ucStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long ucUnit = 10000000;
            #region 获取历史数据
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), dtFirst, dtLast, "hour", splitTyQuery.ToString());
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                //
                DateTime tagtime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                //int interval = CommFunc.ConvertDBNullToInt32(dr["Interval"]);
                string dataCfg = CommFunc.ConvertDBNullToString(dr["DataCfg"]);
                dataCfg = string.IsNullOrEmpty(dataCfg) ? "0:0" : dataCfg;
                foreach (string s in dataCfg.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] val = s.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (val.Length != 2) continue;
                    long ticks = CommFunc.ConvertDBNullToLong(val[0]);
                    decimal value = CommFunc.ConvertDBNullToDecimal(val[1]);
                    if (CommFunc.ConvertDBNullToString(curDr["FunType"]).Equals(V0Fun.E.ToString()) || CommFunc.ConvertDBNullToString(curDr["FunType"]).Equals(V0Fun.EAnt.ToString()) || CommFunc.ConvertDBNullToInt32(curDr["IsChrg"]) == 1)
                        value = value + firstVal;
                    long ts = ticks + ((tagtime - ucStart).Ticks / 10000);

                    TimeSpan toNow = new TimeSpan(ticks * ucUnit + ((tagtime - ucStart).Ticks));
                    DateTime tickTime = ucStart.Add(toNow); //tagtime.AddTicks(ticks);
                    DataRow addDr = dtRst.Rows.Find(new object[] { tickTime, dr["Module_id"] ,dr["Fun_id"]});
                    if (addDr == null)
                    {
                        addDr = dtRst.NewRow();
                        addDr["TagTime"] = tickTime;
                        addDr["Module_id"] = dr["Module_id"];
                        addDr["Fun_id"] = dr["Fun_id"];
                        addDr["ModuleAddr"] = curDr["ModuleAddr"];
                        addDr["ModuleName"] = curDr["ModuleName"];
                        addDr["FunName"] = curDr["FunName"];
                        addDr["CoName"] = curDr["CoName"];
                        addDr["MeterNo"] = curDr["MeterNo"];
                        dtRst.Rows.Add(addDr);
                    }
                    addDr["LastVal"] = value.ToString("f" + scale);
                }
            }
            #endregion
            return dtRst;
        }

        public DataTable GetAlarmList(int co_id, DateTime start, DateTime end, string moduleName)
        {
            DateTime dtFirst = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            DateTime dtLast = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);
            return dal.GetAlarmList(co_id, dtFirst, dtLast, moduleName);
        }
        
    }
}
