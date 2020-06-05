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
        public APIResult GetYdModuleUseValList(string selKey, DateTime time, string dataType)
        {
            APIResult rst = new APIResult();
            try
            {
                string[] kk = selKey.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                int id = kk.Count() != 2 ? 0 : CommFunc.ConvertDBNullToInt32(kk[1]);
                int co_id = 0, meter_id = 0;
                if (selKey.Contains("M-"))
                    meter_id = id;
                else
                    co_id = id;
                DataTable dtSource = bll.GetYdModuleUseValList(co_id, meter_id, time, dataType);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               Cnt = CommFunc.ConvertDBNullToInt32(s1["Cnt"]),
                               V01 = CommFunc.ConvertDBNullToDecimal(s1["V01"]),
                               V02 = CommFunc.ConvertDBNullToDecimal(s1["V02"]),
                               V03 = CommFunc.ConvertDBNullToDecimal(s1["V03"]),
                               V04 = CommFunc.ConvertDBNullToDecimal(s1["V04"]),
                               V05 = CommFunc.ConvertDBNullToDecimal(s1["V05"]),
                               V06 = CommFunc.ConvertDBNullToDecimal(s1["V06"]),
                               V07 = CommFunc.ConvertDBNullToDecimal(s1["V07"]),
                               V08 = CommFunc.ConvertDBNullToDecimal(s1["V08"]),
                               V09 = CommFunc.ConvertDBNullToDecimal(s1["V09"]),
                               V10 = CommFunc.ConvertDBNullToDecimal(s1["V10"]),
                               V11 = CommFunc.ConvertDBNullToDecimal(s1["V11"]),
                               V12 = CommFunc.ConvertDBNullToDecimal(s1["V12"]),
                               V13 = CommFunc.ConvertDBNullToDecimal(s1["V13"]),
                               V14 = CommFunc.ConvertDBNullToDecimal(s1["V14"]),
                               V15 = CommFunc.ConvertDBNullToDecimal(s1["V15"]),
                               V16 = CommFunc.ConvertDBNullToDecimal(s1["V16"]),
                               V17 = CommFunc.ConvertDBNullToDecimal(s1["V17"]),
                               V18 = CommFunc.ConvertDBNullToDecimal(s1["V18"]),
                               V19 = CommFunc.ConvertDBNullToDecimal(s1["V19"]),
                               V20 = CommFunc.ConvertDBNullToDecimal(s1["V20"]),
                               V21 = CommFunc.ConvertDBNullToDecimal(s1["V21"]),
                               V22 = CommFunc.ConvertDBNullToDecimal(s1["V22"]),
                               V23 = CommFunc.ConvertDBNullToDecimal(s1["V23"]),
                               V24 = CommFunc.ConvertDBNullToDecimal(s1["V24"]),
                               V25 = CommFunc.ConvertDBNullToDecimal(s1["V25"]),
                               V26 = CommFunc.ConvertDBNullToDecimal(s1["V26"]),
                               V27 = CommFunc.ConvertDBNullToDecimal(s1["V27"]),
                               V28 = CommFunc.ConvertDBNullToDecimal(s1["V28"]),
                               V29 = CommFunc.ConvertDBNullToDecimal(s1["V29"]),
                               V30 = CommFunc.ConvertDBNullToDecimal(s1["V30"]),
                               V31 = CommFunc.ConvertDBNullToDecimal(s1["V31"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               RoomName = CommFunc.ConvertDBNullToString(s1["RoomName"]),
                               StrcName = CommFunc.ConvertDBNullToString(s1["StrcName"]),
                           };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { total = dtSource.Rows.Count, rows = res1.ToList() };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取按时间粒度列表错误(GetYdModuleList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 回来能耗统计分析(区间粒度)
        /// </summary>
        /// <param name="selKey">选择的对象</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public APIResult GetYdModuleCollectValList(string selKey, DateTime start, DateTime end)
        {
            APIResult rst = new APIResult();
            try
            {
                string[] kk = selKey.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                int id = kk.Count() != 2 ? 0 : CommFunc.ConvertDBNullToInt32(kk[1]);
                int co_id = 0, meter_id = 0;
                if (selKey.Contains("M-"))
                    meter_id = id;
                else
                    co_id = id;
                DataTable dtSource = bll.GetYdModuleCollectValList(co_id, meter_id, start, end);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               Start = CommFunc.ConvertDBNullToDateTime(s1["Start"]).ToString("yyyy-MM-dd"),
                               End = CommFunc.ConvertDBNullToDateTime(s1["End"]).ToString("yyyy-MM-dd"),
                               FirstVal = s1["FirstVal"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["FirstVal"]).ToString("f2"),
                               LastVal = s1["LastVal"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["LastVal"]).ToString("f2"),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               RoomName = CommFunc.ConvertDBNullToString(s1["RoomName"]),
                               StrcName = CommFunc.ConvertDBNullToString(s1["StrcName"]),
                           };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { total = dtSource.Rows.Count, rows = res1.ToList() };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取按区间粒度列表错误(GetYdModuleCollectValList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}