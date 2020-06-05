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
        /// 采集异常告警列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdAlarmOfUnusualList(string strcName,string coName,string startTime,string endTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdAlarmOfUnusualList(strcName, coName, CommFunc.ConvertDBNullToDateTime(startTime), CommFunc.ConvertDBNullToDateTime(endTime));
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               Log_id = CommFunc.ConvertDBNullToInt32(s1["Log_id"]),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               Fun_id = CommFunc.ConvertDBNullToInt32(s1["Fun_id"]),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),                             
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               ModuleAddrS = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               AType = CommFunc.ConvertDBNullToString(s1["AType"]),
                               Content = CommFunc.ConvertDBNullToString(s1["Content"]),
                               ErrCode = CommFunc.ConvertDBNullToString(s1["ErrCode"]),
                               CollectTime = CommFunc.ConvertDBNullToDateTime(s1["CollectTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               FirstVal = CommFunc.ConvertDBNullToDecimal(s1["FirstVal"]),
                               LastVal = CommFunc.ConvertDBNullToDecimal(s1["LastVal"]),
                               LastValOld = CommFunc.ConvertDBNullToDecimal(s1["LastVal"]),
                               LastTime = CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取告警内容错误(GetYdAlarmOfUnusualList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 保存修改错误数据
        /// </summary>
        /// <returns></returns>
        public APIRst SaveVal(int RowId,int Co_id,int Log_id,int Module_id,int Fun_id,string ModuleAddr,decimal LastValOld,decimal LastVal,decimal FirstVal)
        {
            decimal charge = LastValOld - FirstVal;            
            FirstVal = LastVal-charge;
            APIRst rst = new APIRst();
            try
            {
                int total = 0;
                bll.SetV2_infoInVal(Log_id,Co_id,Module_id, ModuleAddr,Fun_id, FirstVal, LastVal);

                DataTable dtSource = bll.GetTabVal(Co_id, ModuleAddr);
                total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = RowId,
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               Log_id = CommFunc.ConvertDBNullToInt32(s1["Log_id"]),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),                              
                               AType = CommFunc.ConvertDBNullToString(s1["AType"]),
                               Content = CommFunc.ConvertDBNullToString(s1["Content"]),
                               ErrCode = CommFunc.ConvertDBNullToString(s1["ErrCode"]),
                               CollectTime = CommFunc.ConvertDBNullToDateTime(s1["CollectTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               FirstVal = CommFunc.ConvertDBNullToDecimal(s1["FirstVal"]),
                               LastVal = CommFunc.ConvertDBNullToDecimal(s1["LastVal"]),
                               LastValOld = CommFunc.ConvertDBNullToDecimal(s1["LastVal"]),
                               LastTime = CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).ToString("yyyy-MM-dd HH:mm:ss"),

                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("保存修改错误数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 保存修改错误时间
        /// </summary>
        /// <returns></returns>
        public APIRst SaveTime(int RowId,int Co_id,int Log_id,int Module_id,int Fun_id,string ModuleAddr,string LastTime)
        {
            APIRst rst = new APIRst();
            try
            {
                int total = 0;
                bll.SetV2_infoInTime(Log_id, Co_id, Module_id, ModuleAddr, Fun_id, CommFunc.ConvertDBNullToDateTime(LastTime));

                DataTable dtSource = bll.GetTabVal(Co_id, ModuleAddr);
                total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = RowId,
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               Log_id = CommFunc.ConvertDBNullToInt32(s1["Log_id"]),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               AType = CommFunc.ConvertDBNullToString(s1["AType"]),
                               Content = CommFunc.ConvertDBNullToString(s1["Content"]),
                               ErrCode = CommFunc.ConvertDBNullToString(s1["ErrCode"]),
                               CollectTime = CommFunc.ConvertDBNullToDateTime(s1["CollectTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               FirstVal = CommFunc.ConvertDBNullToDecimal(s1["FirstVal"]),
                               LastVal = CommFunc.ConvertDBNullToDecimal(s1["LastVal"]),
                               LastValOld = CommFunc.ConvertDBNullToDecimal(s1["LastVal"]),
                               LastTime = CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("保存修改错误时间错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

    }
}
