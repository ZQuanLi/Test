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
        public object GetEnergyForDayX261()
        {
            DateTime fm = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime to = DateTime.Now.AddHours(-1);
            List<decimal> dd = new List<decimal>();
            int nn = to.Hour;
            while (nn-- >= 0)
                dd.Add(0);
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("ModuleAddr", typeof(System.String));
            dtRst.Columns.Add("ModuleName", typeof(System.String));
            dtRst.Columns.Add("ParentName", typeof(System.String));
            dtRst.Columns.Add("UseVal", typeof(System.Decimal));
            dtRst.Columns.Add("UseObj", typeof(System.Object));
            dtRst.Rows.Add("R6B2.X261.ⅠE", "X261 OP30/40", "焊接X261 OP30/40",0, JsonHelper.Deserialize<List<decimal>>(JsonHelper.Serialize(dd)));
            dtRst.Rows.Add("R6B1.1#.LJ", "1# X261 OP30/40", "焊接X261 OP30/40", 0, JsonHelper.Deserialize<List<decimal>>(JsonHelper.Serialize(dd)));
            dtRst.Rows.Add("R6B1.2#.LJ", "2# X261 OP30/40", "焊接X261 OP30/40", 0, JsonHelper.Deserialize<List<decimal>>(JsonHelper.Serialize(dd)));

            dtRst.Rows.Add("R6B2.X261.ⅡE", "X261 OP45/50", "焊接X261 OP45/50", 0, JsonHelper.Deserialize<List<decimal>>(JsonHelper.Serialize(dd)));
            dtRst.Rows.Add("R6B1.3#.LJ", "3# X261 OP45/50", "焊接X261 OP45/50", 0, JsonHelper.Deserialize<List<decimal>>(JsonHelper.Serialize(dd)));
            dtRst.Rows.Add("R6B1.4#.LJ", "4# X261 OP45/50", "焊接X261 OP45/50", 0, JsonHelper.Deserialize<List<decimal>>(JsonHelper.Serialize(dd)));

            dtRst.Rows.Add("R6B2.X261.ⅢE", "X261 OP60/70", "焊接X261 OP60/70", 0, JsonHelper.Deserialize<List<decimal>>(JsonHelper.Serialize(dd)));
            dtRst.Rows.Add("R6B1.5#.LJ", "5# X261 OP60/70", "焊接X261 OP60/70", 0, JsonHelper.Deserialize<List<decimal>>(JsonHelper.Serialize(dd)));
            dtRst.Rows.Add("R6B1.6#.LJ", "6# X261 OP60/70", "焊接X261 OP60/70", 0, JsonHelper.Deserialize<List<decimal>>(JsonHelper.Serialize(dd)));

            dtRst.Rows.Add("R6B2.X261.ⅣE", "X261 OP80/85", "焊接X261 OP80/85", 0, JsonHelper.Deserialize<List<decimal>>(JsonHelper.Serialize(dd)));
            dtRst.Rows.Add("R6B1.7#.LJ", "7# X261 OP80/85", "焊接X261 OP80/85", 0, JsonHelper.Deserialize<List<decimal>>(JsonHelper.Serialize(dd)));
            dtRst.Rows.Add("R6B1.8#.LJ", "8# X261 OP80/85", "焊接X261 OP80/85", 0, JsonHelper.Deserialize<List<decimal>>(JsonHelper.Serialize(dd)));
            StringBuilder strAddrs = new StringBuilder();
            foreach (DataRow dr in dtRst.Rows)
            {
                if (!string.IsNullOrEmpty(strAddrs.ToString()))
                    strAddrs.Append(",");
                strAddrs.Append(CommFunc.ConvertDBNullToString(dr["ModuleAddr"]));
            }
            //
            DataTable dtSource = dal.GetEnergyForDayX261(strAddrs.ToString());
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["ModuleAddr"] };
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
            }
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), fm, to, "hour", splitTyQuery.ToString());
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                scale = scale == 0 ? 2 : scale;
                DataRow addDr = dtRst.Rows.Find(curDr["ModuleAddr"]);
                if (addDr == null) continue;

                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;
                useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);
                addDr["UseVal"] = CommFunc.ConvertDBNullToDecimal(addDr["UseVal"]) + useVal;
                List<decimal> rr = addDr["UseObj"] as List<decimal>;
                if (rr == null) continue;
                rr[tagTime.Hour] = rr[tagTime.Hour] + useVal;
                addDr["UseObj"] = rr;
            }
            var res1 = from s1 in dtRst.AsEnumerable()
                       group s1 by new { ParentName = CommFunc.ConvertDBNullToString(s1["ParentName"]) } into g1
                       select new {
                           ParentName = g1.Key.ParentName,
                           UseVal = g1.Sum(p=>CommFunc.ConvertDBNullToDecimal(p["UseVal"]))
                       };
            List<object> result = new List<object>();
            foreach (var s1 in res1)
            {
                var res2 = from s2 in dtRst.AsEnumerable()
                           where CommFunc.ConvertDBNullToString(s2["ParentName"]).Equals(s1.ParentName)
                           select new
                           {
                               name = CommFunc.ConvertDBNullToString(s2["ModuleName"]),
                               data = s2["UseObj"] as List<decimal>,
                           };
                result.Add(new { name = s1.ParentName, data = res2.ToList() });
            }
            return result;
        }

    }
}
