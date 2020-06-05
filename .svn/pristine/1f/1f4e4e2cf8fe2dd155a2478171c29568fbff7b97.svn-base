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
        /// 站点场景类型统计分析
        /// </summary>
        /// <param name="areaId">区域ID号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="roomSightId">场景ID号</param>
        /// <returns></returns>
        public APIResult GetRptRoomSightList(int areaId, DateTime start, DateTime end, int roomSightId)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetRptRoomSightList(areaId, start, end, roomSightId);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               RoomId = CommFunc.ConvertDBNullToInt32(s1["RoomId"]),
                               RoomSightId = CommFunc.ConvertDBNullToInt32(s1["RoomSightId"]),
                               RoomNo = CommFunc.ConvertDBNullToString(s1["RoomNo"]),
                               RoomName = CommFunc.ConvertDBNullToString(s1["RoomName"]),
                               Address = CommFunc.ConvertDBNullToString(s1["Address"]),
                               RoomSightName = CommFunc.ConvertDBNullToString(s1["RoomSightName"]),
                               StrcName = CommFunc.ConvertDBNullToString(s1["StrcName"]),
                               UseVal = CommFunc.ConvertDBNullToDecimal(s1["UseVal"]).ToString("f2"),
                               LastTime = CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                var res2 = from s1 in dtSource.AsEnumerable()
                           group s1 by new { RoomSightId = CommFunc.ConvertDBNullToInt32(s1["RoomSightId"]) } into g1
                           select new
                           {
                               RoomSightId = g1.Key.RoomSightId,
                               RoomSightName = CommFunc.ConvertDBNullToString(g1.First()["RoomSightName"]),
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
                FileLog.WriteLog("站点场景类型统计分析(GetRptRoomSightList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}