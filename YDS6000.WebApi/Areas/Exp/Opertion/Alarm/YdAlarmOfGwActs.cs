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
        public APIRst GetYdAlarmOfGwList(string strcName,string coName,string aType,string startTime,string endTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdAlarmOfGwList(strcName, coName, aType, CommFunc.ConvertDBNullToDateTime(startTime), CommFunc.ConvertDBNullToDateTime(endTime));
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId =  CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               HandledBY = CommFunc.ConvertDBNullToString(s1["HandledBY"]),
                               Content = CommFunc.ConvertDBNullToString(s1["Content"]),
                               //ContentS = CommFunc.ConvertDBNullToString(s1["ContentS"]),
                               ErrTxt = CommFunc.ConvertDBNullToString(s1["ErrTxt"]),
                               Create_dt = CommFunc.ConvertDBNullToDateTime(s1["CollectTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取告警内容错误(GetYdAlarmOfGwList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

    }
}
