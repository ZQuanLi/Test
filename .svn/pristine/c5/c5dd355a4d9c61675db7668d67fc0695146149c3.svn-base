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
        public DataTable GetRptSwitchList(int areaId, DateTime start, DateTime end, string @switch)
        {
            #region 表结构
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("RoomId", typeof(System.Int32));// 站点ID号
            dtRst.Columns.Add("Switch", typeof(System.String));// 供电类型
            dtRst.Columns.Add("RoomNo", typeof(System.String));// 站点编号
            dtRst.Columns.Add("Start", typeof(System.DateTime));//当前开始时间
            dtRst.Columns.Add("End", typeof(System.DateTime));//当前结束时间            
            dtRst.Columns.Add("RoomName", typeof(System.String));// 机站点名称
            dtRst.Columns.Add("StrcName", typeof(System.String));// 所属区域
            dtRst.Columns.Add("Address", typeof(System.String));// 站点地址
            dtRst.Columns.Add("SwitchName", typeof(System.String));// 供电类型名称
            dtRst.Columns.Add("UseVal", typeof(System.Decimal));//使用电值
            dtRst.Columns.Add("LastTime", typeof(System.String));// 最后录入时间
            #endregion
            #region 填充基础数据
            StringBuilder splitMdQuery = new StringBuilder();
            DataTable dtSource = dal.GetRptSwitchData(areaId, @switch);
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["RoomId"], dtRst.Columns["Switch"] };
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            foreach (DataRow dr in dtSource.Rows)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitMdQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["Module_id"]), ",")))
                {
                    if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
                        splitMdQuery.Append(",");
                    splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                }
                DataRow addDr = dtRst.Rows.Find(new object[] { dr["Co_id"], dr["Switch"] });
                if (addDr == null)
                {
                    addDr = dtRst.NewRow();
                    addDr["RoomId"] = dr["Co_id"];
                    addDr["Switch"] = dr["Switch"];
                    addDr["RoomNo"] = dr["CoNo"];
                    addDr["Start"] = start;
                    addDr["End"] = end;
                    addDr["RoomName"] = dr["CoName"];
                    addDr["StrcName"] = dr["CoStrcName"];
                    addDr["Address"] = dr["CustAddr"];
                    addDr["SwitchName"] = CommFunc.GetEnumDisplay(typeof(Switch), CommFunc.ConvertDBNullToString(dr["Switch"]));
                    dtRst.Rows.Add(addDr);
                }
            }
            #endregion
            #region 填充采集数据
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), start, end, "day");
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                DateTime tagtime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = (lastVal - firstVal) * multiply;
                scale = scale == 0 ? 2 : scale;
                useVal = Math.Round(useVal, scale, MidpointRounding.AwayFromZero);
                DataRow addDr = dtRst.Rows.Find(new object[] { curDr["Co_id"], curDr["Switch"] });
                if (addDr == null) continue;
                addDr["UseVal"] = CommFunc.ConvertDBNullToDecimal(addDr["UseVal"]) + useVal;
                if (CommFunc.ConvertDBNullToDateTime(addDr["LastTime"]) < CommFunc.ConvertDBNullToDateTime(dr["LastTime"]))
                    addDr["LastTime"] = dr["LastTime"];
            }
            #endregion
            return dtRst;
        }
    }
}
