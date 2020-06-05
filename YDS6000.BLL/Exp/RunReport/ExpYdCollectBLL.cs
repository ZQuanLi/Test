using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.RunReport
{
    public partial class ExpYdCollectBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";
        private readonly YDS6000.DAL.Exp.RunReport.ExpYdCollectDAL dal = null;
        public ExpYdCollectBLL(string _project, int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            this.Project = _project;
            dal = new YDS6000.DAL.Exp.RunReport.ExpYdCollectDAL(_project, _ledger, _uid);
        }


        public DataTable GetYdCollectData(string coStrcName, string coName, string funType, int isDefine, DateTime dtFirst, DateTime dtLast)
        {
            dtFirst = new DateTime(dtFirst.Year, dtFirst.Month, dtFirst.Day, 0, 0, 0);
            dtLast = new DateTime(dtLast.Year, dtLast.Month, dtLast.Day, 23, 59, 59);
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("TagTime", typeof(System.DateTime));
            dtRst.Columns.Add("Module_id", typeof(System.Int32));
            dtRst.Columns.Add("ModuleAddr", typeof(System.String));
            dtRst.Columns.Add("ModuleName", typeof(System.String));
            dtRst.Columns.Add("Co_id", typeof(System.Int32));
            dtRst.Columns.Add("LastVal", typeof(System.Decimal));
            dtRst.Columns.Add("Multiply", typeof(System.Decimal));/*倍率*/
            dtRst.Columns.Add("Fun_id", typeof(System.Int32));
            dtRst.Columns.Add("FunType", typeof(System.String));
            dtRst.Columns.Add("Scale", typeof(System.Int32));
            dtRst.Columns.Add("CoStrcName", typeof(System.String));
            dtRst.Columns.Add("CoName", typeof(System.String));
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["TagTime"], dtRst.Columns["Module_id"] };
            DataTable dtSource = dal.GetYdCollectData(coStrcName, coName, funType, isDefine);
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            StringBuilder splitMdQuery = new StringBuilder();
            foreach (DataRow dr in dtSource.Rows)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitMdQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["Module_id"]), ",")))
                {
                    if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
                        splitMdQuery.Append(",");
                    splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                }
            }
            DateTime ucStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long ucUnit = 10000000;
            #region 获取历史数据
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), dtFirst, dtLast, "hour", funType);
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
                int interval = CommFunc.ConvertDBNullToInt32(dr["Interval"]);
                string dataCfg = CommFunc.ConvertDBNullToString(dr["DataCfg"]);
                dataCfg = string.IsNullOrEmpty(dataCfg) ? "0:0" : dataCfg;
                foreach (string s in dataCfg.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] val = s.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (val.Length != 2) continue;
                    long ticks = CommFunc.ConvertDBNullToLong(val[0]);
                    decimal value = CommFunc.ConvertDBNullToDecimal(val[1]);
                    if (CommFunc.ConvertDBNullToString(curDr["FunType"]).Equals(V0Fun.E.ToString()) || CommFunc.ConvertDBNullToString(curDr["FunType"]).Equals(V0Fun.EAnt.ToString()))
                    {
                        //value = value + lastVal;
                        value = value + firstVal;
                    }
                    long ts = ticks + ((tagtime - ucStart).Ticks / 10000);

                    TimeSpan toNow = new TimeSpan(ticks * ucUnit + ((tagtime - ucStart).Ticks));
                    DateTime tickTime = ucStart.Add(toNow); //tagtime.AddTicks(ticks);
                    //int min = (tickTime.Minute / 15) * 15;
                    //DateTime nowTime = new DateTime(tickTime.Year, tickTime.Month, tickTime.Day, tickTime.Hour, min, 0);
                    DataRow addDr = dtRst.Rows.Find(new object[] { tickTime, dr["Module_id"] });
                    if (addDr == null)
                    {
                        addDr = dtRst.NewRow();
                        addDr["TagTime"] = tickTime;
                        addDr["Module_id"] = dr["Module_id"];
                        addDr["ModuleAddr"] = curDr["ModuleAddr"];
                        addDr["ModuleName"] = curDr["ModuleName"];
                        addDr["Co_id"] = curDr["Co_id"];
                        addDr["Multiply"] = curDr["Multiply"];
                        addDr["Fun_id"] = curDr["Fun_id"];
                        addDr["FunType"] = curDr["FunType"];
                        addDr["Scale"] = curDr["Scale"];
                        addDr["CoStrcName"] = curDr["CoStrcName"];
                        addDr["CoName"] = curDr["CoName"];
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
