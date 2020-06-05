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
        /// 机房类型统计分析
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="stationTypeId"></param>
        /// <returns></returns>
        public APIResult GetRptStationList(int areaId, DateTime start, DateTime end ,int stationTypeId)
        {
            APIResult rst = new APIResult();
            try
            {               
                DataTable dtSource = bll.GetRptStationList(areaId, start, end, stationTypeId);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               StationId = CommFunc.ConvertDBNullToInt32(s1["StationId"]),
                               StationNo = CommFunc.ConvertDBNullToString(s1["StationNo"]),
                               StationName = CommFunc.ConvertDBNullToString(s1["StationName"]),
                               Address = CommFunc.ConvertDBNullToString(s1["Address"]),
                               StationTypeName = CommFunc.ConvertDBNullToString(s1["StationTypeName"]),
                               StrcName = CommFunc.ConvertDBNullToString(s1["StrcName"]),
                               UseVal = CommFunc.ConvertDBNullToDecimal(s1["UseVal"]).ToString("f2"),
                               LastTime = CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                var res2 = from s1 in dtSource.AsEnumerable()
                           group s1 by new { StationTypeId = CommFunc.ConvertDBNullToInt32(s1["StationTypeId"]) } into g1
                           select new
                           {
                               StationTypeId = g1.Key.StationTypeId,
                               StationTypeName = CommFunc.ConvertDBNullToString(g1.First()["StationTypeName"]),
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
                FileLog.WriteLog("机房类型统计分析(GetRptStationList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 机房类型获取机房信息
        /// </summary>
        /// <param name="stationTypeId"></param>
        /// <returns></returns>
        //public APIResult GetRptStationList(int stationTypeId)
        //{
        //    APIResult rst = new APIResult();
        //    try
        //    {
        //        DataTable dtSource = bll.GetRptStationList(stationTypeId);
        //        var res1 = from s1 in dtSource.AsEnumerable()
        //                   select new
        //                   {
        //                       RowId = dtSource.Rows.IndexOf(s1) + 1,
        //                       StationId = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
        //                       StationNo = CommFunc.ConvertDBNullToString(s1["CoNo"]),
        //                       StationName = CommFunc.ConvertDBNullToString(s1["CoName"]),
        //                       Address = CommFunc.ConvertDBNullToString(s1["CustAddr"]),
        //                       StrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
        //                   };
        //        rst.Code = 0;
        //        rst.Msg = "";
        //        rst.Data = new { total = dtSource.Rows.Count, rows = res1.ToList() };
        //    }
        //    catch (Exception ex)
        //    {
        //        rst.Code = -1;
        //        rst.Msg = ex.Message;
        //        FileLog.WriteLog("机房类型获取机房信息(GetRptStationList):" + ex.Message + ex.StackTrace);
        //    }
        //    return rst;
        //}
    }
}