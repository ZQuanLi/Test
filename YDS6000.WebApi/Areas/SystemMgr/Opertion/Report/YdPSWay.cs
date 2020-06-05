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
        /// 直流/交流统计分析
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="psWay"></param>
        /// <returns></returns>
        public APIResult GetRptPSWayList(int areaId, DateTime start, DateTime end, string psWay)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetRptPSWayList(areaId, start, end, psWay);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               StationId = CommFunc.ConvertDBNullToInt32(s1["StationId"]),
                               PSWay = CommFunc.ConvertDBNullToString(s1["PSWay"]),
                               StationNo = CommFunc.ConvertDBNullToString(s1["StationNo"]),
                               StationName = CommFunc.ConvertDBNullToString(s1["StationName"]),
                               Address = CommFunc.ConvertDBNullToString(s1["Address"]),
                               PSWayName = CommFunc.ConvertDBNullToString(s1["PSWayName"]),
                               StrcName = CommFunc.ConvertDBNullToString(s1["StrcName"]),
                               UseVal = CommFunc.ConvertDBNullToDecimal(s1["UseVal"]).ToString("f2"),
                               LastTime = CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                var res2 = from s1 in dtSource.AsEnumerable()
                           group s1 by new { PSWay = CommFunc.ConvertDBNullToString(s1["PSWay"]) } into g1
                           select new
                           {
                               PSWay = g1.Key.PSWay,
                               PSWayName = CommFunc.ConvertDBNullToString(g1.First()["PSWayName"]), 
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
                FileLog.WriteLog("直流/交流统计分析(GetRptPSWayList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}