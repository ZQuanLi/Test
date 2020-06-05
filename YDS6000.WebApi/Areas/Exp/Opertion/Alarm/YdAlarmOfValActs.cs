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
        public APIRst GetYdAlarmOfChrgType()
        {
            APIRst rst = new APIRst();            
            try
            {
                int show = bll.GetYdAlarmOfChrgType();
                rst.data = new { show = show };
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取告警剩余电量错误(GetYdAlarmOfChrgType)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        public APIRst GetYdAlarmOfValList(string strcName,string coName)
        {
            APIRst rst = new APIRst();            
            try
            {
                DataTable dtSource = bll.GetYdAlarmOfValList(strcName, coName);
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               SyVal = CommFunc.ConvertDBNullToDecimal(s1["SyVal"]).ToString("f2"),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取告警剩余电量错误(GetYdAlarmOfValList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 汇出Excel
        /// </summary>
        /// <returns></returns>

        public APIRst GetYdAlarmOfValOnExport(string data,int Show)
        {
            APIRst rst = new APIRst();
            try
            {
                string path = GetYdAlarmOfValOnExport_Path(data, Show);
                rst.data = path;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("GetYdMontionOnExport:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private string GetYdAlarmOfValOnExport_Path(string jsonDataTable, int Show)
        {
            string fn = "/XTemp/电量使用告警.xls";
            string filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"/XTemp");
            if (System.IO.Directory.Exists(filePath) == false)
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            string filename = System.Web.Hosting.HostingEnvironment.MapPath(fn);
            if (System.IO.File.Exists(filename))/*先删除已存在的文件，再汇出Excel*/
                System.IO.File.Delete(filename);
            DataTable dtSource = new DataTable();
            if (jsonDataTable.Length > 0) dtSource = JsonHelper.ToDataTable(jsonDataTable);
            Excel.ExcelCellStyle columnCellStyle0 = new Excel.ExcelCellStyle();
            columnCellStyle0 = new Excel.ExcelCellStyle()
            {
                DataFormart = "0.00",
                HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT
            };
            Excel.ExcelCellStyle columnCellStyle1 = new Excel.ExcelCellStyle();
            columnCellStyle1 = new Excel.ExcelCellStyle()
            {
                DataFormart = "yyyy-MM-dd HH:mm:ss",
            };
            Excel.ExcelColumnCollection columns = new Excel.ExcelColumnCollection();
            columns.Add(new Excel.ExcelColumn("序号", "RowId", 10) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("建筑", "CoStrcName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("房间", "CoName", 15) { IsSetWith = true });
            if (Show > 0)
                columns.Add(new Excel.ExcelColumn("电表", "ModuleAddr", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("当前剩余电量", "SyVal", 15) { IsSetWith = true });


            Excel.ExcelOparete excel = new Excel.ExcelOparete("电量使用告警");
            excel.SetObjectValue("电量使用告警", 0, 0, 3);
            excel.SetColumnName(columns, 1, 0);
            excel.SetColumnValue(columns, dtSource.Select(), 2, 0);
            excel.SaveExcelByFullFileName(filename);
            return fn;
        }
        /// <summary>
        /// 获取首页欠费户数
        /// </summary>
        /// <returns></returns>
        public APIRst GetHomeUserAlarm()
        {
            APIRst rst = new APIRst();
            try
            {
                int dtSource = bll.GetHomeUserAlarm();

                object obj = new { Number = dtSource };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页用户数错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        #region 隐患管理
        /// <summary>
        /// 采集异常告警—获取采集异常告警列表
        /// </summary>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="status">操作状态(可选)</param>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <returns></returns>
        public APIRst GetYdAlarmList(string startTime, string endTime, int status = 0, string strcName = "", string coName = "")
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdAlarmList(strcName, coName, status, CommFunc.ConvertDBNullToDateTime(startTime), CommFunc.ConvertDBNullToDateTime(endTime));
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Log_id = CommFunc.ConvertDBNullToInt32(s1["Log_id"]),
                               Status = CommFunc.ConvertDBNullToInt32(s1["Status"]),
                               Create_dt = CommFunc.ConvertDBNullToDateTime(s1["Create_dt"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               Content = CommFunc.ConvertDBNullToString(s1["Content"]),
                               Appr_by = CommFunc.ConvertDBNullToInt32(s1["Appr_by"]),
                               CollectValue = CommFunc.ConvertDBNullToString(s1["CollectValue"]),
                               CollectName = CommFunc.ConvertDBNullToString(s1["CollectName"]),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取告警内容错误(GetYdAlarmList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 忽略此隐患
        /// </summary>
        /// <param name="log_id">隐患编号</param>
        /// <returns></returns>
        public APIRst IgnoreAlarm(long log_id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.IgnoreAlarm(log_id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("错误(IgnoreAlarm)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdUserList(int uid)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdUserList(uid);
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Uid = CommFunc.ConvertDBNullToInt32(s1["Uid"]),
                               UName = CommFunc.ConvertDBNullToString(s1["UName"]),
                               TelNo = CommFunc.ConvertDBNullToString(s1["TelNo"]),
                               RoleName = CommFunc.ConvertDBNullToString(s1["RoleName"]),
                           };
                object obj =  res1.ToList() ;
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取用户列表错误(GetYdUserList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 派工单
        /// </summary>
        /// <param name="log_id">隐患编号</param>
        /// <param name="uid">指派人员ID</param>
        /// <returns></returns>
        public APIRst JobOrder(long log_id, int uid)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.JobOrder(log_id, uid);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("错误(JobOrder)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 工单列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetSolutionList(string startTime, string endTime, int status = 0, string strcName = "", string coName = "")
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetSolutionList(strcName, coName, status, CommFunc.ConvertDBNullToDateTime(startTime), CommFunc.ConvertDBNullToDateTime(endTime));
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Log_id = CommFunc.ConvertDBNullToInt32(s1["Log_id"]),
                               Status = CommFunc.ConvertDBNullToInt32(s1["Status"]),
                               CreateTime = CommFunc.ConvertDBNullToDateTime(s1["Create_dt"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               SolutionName = CommFunc.ConvertDBNullToString(s1["SolutionName"]),
                               AlarmTime = CommFunc.ConvertDBNullToDateTime(s1["AlarmTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               Content = CommFunc.ConvertDBNullToString(s1["Content"]),
                               SolutionTime = CommFunc.ConvertDBNullToDateTime(s1["SolutionTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),                              
                               CollectValue = CommFunc.ConvertDBNullToString(s1["CollectValue"]),
                               SolutionImage = CommFunc.ConvertDBNullToString(s1["SolutionImage"]),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取告警内容错误(GetSolutionList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 工单状态修改
        /// </summary>
        /// <param name="log_id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public APIRst SolutionStatus(long log_id, int status)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SolutionStatus(log_id,status);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取告警内容错误(SolutionStatus)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIRst GetIdxList(DateTime startTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetIdxList(startTime);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CellName = CommFunc.ConvertDBNullToString(s1["CellName"]),
                               ProjectName = CommFunc.ConvertDBNullToString(s1["ProjectName"]),
                               UnitName = CommFunc.ConvertDBNullToString(s1["UnitName"]),
                               AreaName = CommFunc.ConvertDBNullToString(s1["AreaName"]),
                               Idx = CommFunc.ConvertDBNullToInt32(s1["Idx"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("(GetIdxList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        public APIRst UpdateIdx(int co_id, DateTime startTime, int idx)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.UpdateIdx(co_id, startTime, idx);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("(UpdateIdx)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        #endregion
    }
}
