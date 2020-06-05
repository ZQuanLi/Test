using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using YDS6000.BLL.Report;

namespace YDS6000.WebApi.Areas.SystemMgr.Controllers
{
    partial class ReportHelper
    {
        /// <summary>
        /// 站点直供/转供统计分析
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="switch"></param>
        /// <returns></returns>
        public APIResult GetRptSwitchList(int areaId, DateTime start, DateTime end, string @switch)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetRptSwitchList(areaId, start, end, @switch);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               RoomId = CommFunc.ConvertDBNullToInt32(s1["RoomId"]),
                               Switch = CommFunc.ConvertDBNullToString(s1["Switch"]),
                               RoomNo = CommFunc.ConvertDBNullToString(s1["RoomNo"]),
                               RoomName = CommFunc.ConvertDBNullToString(s1["RoomName"]),
                               Address = CommFunc.ConvertDBNullToString(s1["Address"]),
                               SwitchName = CommFunc.ConvertDBNullToString(s1["SwitchName"]),
                               StrcName = CommFunc.ConvertDBNullToString(s1["StrcName"]),
                               UseVal = CommFunc.ConvertDBNullToDecimal(s1["UseVal"]).ToString("f2"),
                               LastTime = CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                var res2 = from s1 in dtSource.AsEnumerable()
                           group s1 by new { Switch = CommFunc.ConvertDBNullToString(s1["Switch"]) } into g1
                           select new
                           {
                               Switch = g1.Key.Switch,
                               SwitchName = CommFunc.ConvertDBNullToString(g1.First()["SwitchName"]),
                               UseVal = g1.Sum(p => CommFunc.ConvertDBNullToDecimal(p["UseVal"])),
                               Cnt = g1.Count()
                           };

                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { total = dtSource.Rows.Count, rows = res1.ToList(), charts = res2.ToList() };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("站点直供/转供统计分析(GetRptSwitchList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}