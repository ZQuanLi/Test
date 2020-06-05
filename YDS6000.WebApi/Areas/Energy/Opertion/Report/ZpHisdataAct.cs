using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Energy.Opertion.Report
{
    partial class ReportHelper
    {
        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="co_id">支路ID号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public APIRst GetEnergyHisdata(int co_id, DateTime start, DateTime end,string moduleName,string funType = "")
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetEnergyHisdata(co_id, start, end, moduleName, funType);
                var res1 = from s1 in dtSource.AsEnumerable()
                           orderby  s1["MeterNo"],s1["Module_id"], s1["TagTime"]
                           select new
                           {
                               TagTime = CommFunc.ConvertDBNullToDateTime(s1["TagTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               BuildName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               LastVal = CommFunc.ConvertDBNullToDecimal(s1["LastVal"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取历史数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="co_id">支路ID号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public APIRst GetAlarmList(int co_id, DateTime start, DateTime end, string moduleName)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetAlarmList(co_id, start, end, moduleName);
                var res1 = from s1 in dtSource.AsEnumerable()
                           orderby s1["Module_id"], s1["CollectTime"]
                           select new
                           {
                               TagTime = CommFunc.ConvertDBNullToDateTime(s1["CollectTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               BuildName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               //FunName = "",
                               LastVal = 1,
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取历史数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        
    }
}