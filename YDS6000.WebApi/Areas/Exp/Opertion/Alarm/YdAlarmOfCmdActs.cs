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
        public APIRst GetYdAlarmOfCmdList(string strcName,string coName,string startTime,string endTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdAlarmOfCmdList(strcName, coName, CommFunc.ConvertDBNullToDateTime(startTime), CommFunc.ConvertDBNullToDateTime(endTime));
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               Log_id = CommFunc.ConvertDBNullToLong(s1["Log_id"]),
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               FunType = CommFunc.ConvertDBNullToString(s1["FunType"]),
                               FunTypeS = CommFunc.ConvertDBNullToString(s1["FunTypeS"]),
                               ErrTxt = CommFunc.ConvertDBNullToString(s1["ErrTxt"]),
                               DataValue = CommFunc.ConvertDBNullToString(s1["DataValue"]),
                               Create_by = CommFunc.ConvertDBNullToString(s1["Create_by"]),
                               Create_dt = CommFunc.ConvertDBNullToDateTime(s1["Create_dt"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取远程控制内容错误(GetYdAlarmOfCmdList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIRst GetYdAlarmOfCmd_Ctrl(int log_id)
        {
            APIRst rst = new APIRst();
            string msg = "";
            if (log_id ==0)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "远程控制ID为空";
                return rst;
            }
            YdToGw gw = new YdToGw(user.Ledger, user.Uid);
            try
            {
                bool upDb = false;
                rst.rst = gw.YdToGwAgain(log_id, out upDb, out msg);
                rst.data = upDb;
                rst.err.msg = msg;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("重发命令错误 (GetYdAlarmOfCmd_Ctrl)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}
