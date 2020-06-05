using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    partial class AlarmHelper
    {
        /// <summary>
        /// 恶性负载告警列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdAlarmOfLoadList(string strcName,string coName,string startTime,string endTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdAlarmOfLoadList(strcName, coName, CommFunc.ConvertDBNullToDateTime(startTime), CommFunc.ConvertDBNullToDateTime(endTime));
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               CDate = CommFunc.ConvertDBNullToDateTime(s1["CDate"]).ToString("yyyy-MM-dd"),
                               CoFullName = CommFunc.ConvertDBNullToString(s1["CoFullName"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               Content = CommFunc.ConvertDBNullToString(s1["Content"]),
                               CTime = CommFunc.ConvertDBNullToDateTime(s1["CTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               CollectTime = CommFunc.ConvertDBNullToDateTime(s1["CollectTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取告警内容错误(GetYdAlarmOfLoadList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

    }
}
