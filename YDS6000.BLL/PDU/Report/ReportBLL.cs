using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.PDU.Report
{
    public partial class ReportBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.PDU.Report.ReportDAL dal = null;
        public ReportBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.PDU.Report.ReportDAL(_ledger, _uid);
        }

        public object GetEnergy(int co_id, string dataType, DateTime dataTime)
        {
            DataTable dtSource = dal.GetEnergy(co_id);
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            StringBuilder splitMdQuery = new StringBuilder();
            StringBuilder splitTyQuery = new StringBuilder();
          
            foreach (DataRow dr in dtSource.Rows)
            {
                if (string.IsNullOrEmpty(splitMdQuery.ToString()))
                    splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitTyQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["FunType"]), ",")))
                {
                    if (!string.IsNullOrEmpty(splitTyQuery.ToString()))
                        splitTyQuery.Append(",");
                    splitTyQuery.Append(CommFunc.ConvertDBNullToString(dr["FunType"]));                   
                }
            }
            string ddType = dataType;
            DateTime start = dataTime, end = dataTime;
          
            int count = 0, cc = 0;
            if (dataType.ToLower().Equals("day"))
            {
                ddType = "hour";
                start = new DateTime(dataTime.Year, dataTime.Month, dataTime.Day);
                if (start == new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                    end = new DateTime(dataTime.Year, dataTime.Month, dataTime.Day, DateTime.Now.Hour, 59, 59).AddHours(-1);
                else
                    end = new DateTime(dataTime.Year, dataTime.Month, dataTime.Day, 23, 59, 59);
                cc = end.Hour + 1;
                count = 24;
            }
            else if (dataType.ToLower().Equals("month"))
            {
                ddType = "day";
                start = new DateTime(dataTime.Year, dataTime.Month, 1);
                if (start == new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
                    end = new DateTime(dataTime.Year, dataTime.Month, DateTime.Now.Day);
                else
                    end = start.AddMonths(1).AddDays(-1);
                cc = end.Day;
                count = start.AddMonths(1).AddDays(-1).Day;
            }
            else if (dataType.ToLower().Equals("year"))
            {
                ddType = "month";
                start = new DateTime(dataTime.Year, 1, 1);
                if (start == new DateTime(DateTime.Now.Year, 1, 1))
                    end = new DateTime(dataTime.Year, DateTime.Now.Month, 1);
                else
                    end = start.AddMonths(1).AddDays(-1);
                cc = end.Month;
                count = 12;
            }
            //增加
            Dictionary<string, List<decimal>> dic = new Dictionary<string, List<decimal>>();
            foreach (var ss in splitTyQuery.ToString().Split(new char[] { ','},StringSplitOptions.RemoveEmptyEntries))
            {                
                int cin = cc;
                List<decimal> dd = new List<decimal>();
                while (--cin >= 0)
                    dd.Add(0);              
                dic.Add(ss, dd);
            }
            //
            decimal allUse = 0;
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), start, end, ddType, splitTyQuery.ToString());
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                scale = scale == 0 ? 2 : scale;
                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;
                useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);
                List<decimal> dd = null;
                if (dic.TryGetValue(CommFunc.ConvertDBNullToString(curDr["FunType"]), out dd) == false)
                    continue;
                //
                if (ddType.Equals("hour"))
                    dd[tagTime.Hour] = CommFunc.ConvertDBNullToDecimal(dd[tagTime.Hour]) + useVal;
                else if (ddType.Equals("day"))
                    dd[tagTime.Day - 1] = CommFunc.ConvertDBNullToDecimal(dd[tagTime.Day - 1]) + useVal;
                else if (ddType.Equals("month"))
                    dd[tagTime.Month - 1] = CommFunc.ConvertDBNullToDecimal(dd[tagTime.Month - 1]) + useVal;
                allUse = allUse + useVal;
            }
            //object ss;
            List<object> rr = new List<object>();
            foreach (var pk in dic)
            {
                rr.Add(new { name = pk.Key, list = pk.Value });
            }
            return new { data = rr, count = count };
        }

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetHisdata(DateTime start, DateTime end, string moduleName,string funName)
        {
            DateTime dtFirst = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
            DateTime dtLast = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("TagTime", typeof(System.DateTime));
            dtRst.Columns.Add("Module_id", typeof(System.Int32));
            dtRst.Columns.Add("Fun_id", typeof(System.Int32));
            dtRst.Columns.Add("ModuleAddr", typeof(System.String));
            dtRst.Columns.Add("ModuleName", typeof(System.String));
            //dtRst.Columns.Add("MeterNo", typeof(System.String));
            dtRst.Columns.Add("LastVal", typeof(System.Decimal));
            dtRst.Columns.Add("FunName", typeof(System.String));
            dtRst.Columns.Add("CoName", typeof(System.String));
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["TagTime"], dtRst.Columns["Module_id"], dtRst.Columns["Fun_id"] };
            DataTable dtSource = dal.GetHisdata(moduleName, funName);
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
                    if (tickTime < dtFirst || tickTime > dtLast) continue;
                    DataRow addDr = dtRst.Rows.Find(new object[] { tickTime, dr["Module_id"], dr["Fun_id"] });
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
                        //addDr["MeterNo"] = curDr["MeterNo"];
                        dtRst.Rows.Add(addDr);
                    }
                    addDr["LastVal"] = value.ToString("f" + scale);
                }
            }
            #endregion
            return dtRst;
        }
    }
}
