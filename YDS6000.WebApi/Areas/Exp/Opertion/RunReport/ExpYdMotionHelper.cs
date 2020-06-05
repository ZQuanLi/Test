using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using YDS6000.Models.Tables;

namespace YDS6000.WebApi.Areas.Exp.Opertion.RunReport
{
    public partial class ExpYdMotionHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.RunReport.ExpYdMotionBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpYdMotionHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.RunReport.ExpYdMotionBLL(WebConfig.SysProject, user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 运行历史报表
        /// </summary>
        /// <param name="StrcName">建筑名称</param>
        /// <param name="CoName">房间名称</param>
        /// <param name="DateType">报表类型</param>
        /// <param name="StartTime">日期开始</param>
        /// <returns></returns>
        public APIRst GetYdMontionOnList(string StrcName, string CoName, string DateType, DateTime StartTime)
        {
            APIRst rst = new APIRst();
            try
            {
                string time_str = CommFunc.ConvertDBNullToString(StartTime) + (DateType.Equals("month") ? "-01" : DateType.Equals("year") ? "-01-01" : "");
                DateTime start = CommFunc.ConvertDBNullToDateTime(time_str);
                DateTime end = start;
                string msg = "";
                if (DateType.Equals("hour"))
                {
                    start = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
                    end = new DateTime(start.Year, start.Month, start.Day, 23, 0, 0);
                }
                if (DateType.Equals("day"))
                {
                    //start = new DateTime(start.Year, start.Month, start.Day);
                    start = new DateTime(start.Year, start.Month, 1);
                    end = start.AddMonths(1).AddDays(-1);
                }
                if (DateType.Equals("month"))
                {
                    start = new DateTime(start.Year, 1, 1);
                    end = start.AddYears(1).AddDays(-1);
                }
                if (DateType.Equals("year"))
                {
                    start = new DateTime(start.Year, 1, 1);
                    end = start.AddYears(1).AddDays(-1);
                }
                int total = 0;

                object rows = GetYdMontionOnList(StrcName, CoName, DateType, start, end, out total);
                object obj = new { total = total, rows = rows };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("运行历史报表(GetYdMontionOnList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        private object GetYdMontionOnList(string CoStrcName, string CoName, string DateType, DateTime Start, DateTime End, out int total)
        {
            DataTable dtSource = bll.GetYdMonitorOnList(CoStrcName, CoName, DateType, Start, End);
            total = dtSource.Rows.Count;
            var res1 = from s1 in dtSource.AsEnumerable()
                       select new
                       {
                           RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                           TagTime = CommFunc.ConvertDBNullToDateTime(s1["TagTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                           ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                           ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                           CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                           CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           TagTimeS = CommFunc.ConvertDBNullToString(s1["TagTimeS"]),
                           Cnt = CommFunc.ConvertDBNullToInt32(s1["Cnt"]),
                           h00 = s1["h00"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h00"]).ToString(),
                           h01 = s1["h01"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h01"]).ToString(),
                           h02 = s1["h02"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h02"]).ToString(),
                           h03 = s1["h03"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h03"]).ToString(),
                           h04 = s1["h04"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h04"]).ToString(),
                           h05 = s1["h05"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h05"]).ToString(),
                           h06 = s1["h06"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h06"]).ToString(),
                           h07 = s1["h07"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h07"]).ToString(),
                           h08 = s1["h08"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h08"]).ToString(),
                           h09 = s1["h09"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h09"]).ToString(),
                           h10 = s1["h10"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h10"]).ToString(),
                           h11 = s1["h11"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h11"]).ToString(),
                           h12 = s1["h12"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h12"]).ToString(),
                           h13 = s1["h13"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h13"]).ToString(),
                           h14 = s1["h14"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h14"]).ToString(),
                           h15 = s1["h15"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h15"]).ToString(),
                           h16 = s1["h16"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h16"]).ToString(),
                           h17 = s1["h17"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h17"]).ToString(),
                           h18 = s1["h18"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h18"]).ToString(),
                           h19 = s1["h19"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h19"]).ToString(),
                           h20 = s1["h20"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h20"]).ToString(),
                           h21 = s1["h21"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h21"]).ToString(),
                           h22 = s1["h22"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h22"]).ToString(),
                           h23 = s1["h23"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h23"]).ToString(),
                           h24 = s1["h24"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h24"]).ToString(),
                           h25 = s1["h25"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h25"]).ToString(),
                           h26 = s1["h26"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h26"]).ToString(),
                           h27 = s1["h27"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h27"]).ToString(),
                           h28 = s1["h28"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h28"]).ToString(),
                           h29 = s1["h29"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h29"]).ToString(),
                           h30 = s1["h30"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["h30"]).ToString()
                       };
            return res1.ToList();
        }



        /// <summary>
        /// 汇出Excel
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public APIRst GetYdMontionOnExport(DataModels data)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = JsonHelper.ToDataTable(data.Data);
                string path = GetYdMontionOnExport(dtSource);
                rst.data = path;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("GetYdMontionOnExport(汇出Excel)错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private string GetYdMontionOnExport(DataTable dtSource)
        {
            string fn = "/XTemp/运行历史报表.xls";
            string filePath = System.Web.HttpContext.Current.Server.MapPath(@"/XTemp");
            if (System.IO.Directory.Exists(filePath) == false)
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            string filename = System.Web.HttpContext.Current.Server.MapPath(fn);
            if (System.IO.File.Exists(filename))/*先删除已存在的文件，再汇出Excel*/
                System.IO.File.Delete(filename);
            if (dtSource == null || dtSource.Rows.Count == 0)
                throw new Exception("没有数据");
            int cnt = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Cnt"]);
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
            columns.Add(new Excel.ExcelColumn("序号", "RowId", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("建筑", "CoStrcName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("房间", "CoName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("电表地址", "ModuleAddr", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("日期", "TagTimeS", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn(cnt == 12 ? "01月" : cnt == 24 ? "00:00" : "01号", "h00", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn(cnt == 12 ? "02月" : cnt == 24 ? "01:00" : "02号", "h01", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn(cnt == 12 ? "03月" : cnt == 24 ? "02:00" : "03号", "h02", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn(cnt == 12 ? "04月" : cnt == 24 ? "03:00" : "04号", "h03", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn(cnt == 12 ? "05月" : cnt == 24 ? "04:00" : "05号", "h04", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn(cnt == 12 ? "06月" : cnt == 24 ? "05:00" : "06号", "h05", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn(cnt == 12 ? "07月" : cnt == 24 ? "06:00" : "07号", "h06", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn(cnt == 12 ? "08月" : cnt == 24 ? "07:00" : "08号", "h07", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn(cnt == 12 ? "09月" : cnt == 24 ? "08:00" : "09号", "h08", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn(cnt == 12 ? "10月" : cnt == 24 ? "09:00" : "10号", "h09", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn(cnt == 12 ? "11月" : cnt == 24 ? "10:00" : "11号", "h10", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn(cnt == 12 ? "12月" : cnt == 24 ? "11:00" : "12号", "h11", 15) { IsSetWith = true });
            if (cnt > 13)
            {
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "12:00" : "13号", "h12", 15) { IsSetWith = true });
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "13:00" : "14号", "h13", 15) { IsSetWith = true });
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "14:00" : "15号", "h14", 15) { IsSetWith = true });
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "15:00" : "16号", "h15", 15) { IsSetWith = true });
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "16:00" : "17号", "h16", 15) { IsSetWith = true });
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "17:00" : "18号", "h17", 15) { IsSetWith = true });
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "18:00" : "19号", "h18", 15) { IsSetWith = true });
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "19:00" : "20号", "h19", 15) { IsSetWith = true });
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "20:00" : "21号", "h20", 15) { IsSetWith = true });
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "21:00" : "22号", "h21", 15) { IsSetWith = true });
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "22:00" : "23号", "h22", 15) { IsSetWith = true });
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "23:00" : "24号", "h23", 15) { IsSetWith = true });
            }
            if (cnt > 24)
            {
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "00:00" : "25号", "h24", 15) { IsSetWith = true });
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "00:00" : "26号", "h25", 15) { IsSetWith = true });
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "00:00" : "27号", "h26", 15) { IsSetWith = true });
                columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "00:00" : "28号", "h27", 15) { IsSetWith = true });
                if (cnt >= 29)
                    columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "00:00" : "29号", "h28", 15) { IsSetWith = true });
                if (cnt >= 30)
                    columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "00:00" : "30号", "h29", 15) { IsSetWith = true });
                if (cnt >= 31)
                    columns.Add(new Excel.ExcelColumn(cnt == 12 ? "00月" : cnt == 24 ? "00:00" : "31号", "h30", 15) { IsSetWith = true });
            }
            //columns.Add(new Excel.ExcelColumn("E", "E", 15) { IsSetWith = true });
            Excel.ExcelOparete excel = new Excel.ExcelOparete("运行历史报表");
            excel.SetObjectValue("运行历史报表", 0, 0, 3);
            excel.SetColumnName(columns, 1, 0);
            excel.SetColumnValue(columns, dtSource.Select(), 2, 0);
            excel.SaveExcelByFullFileName(filename);
            return fn;
        }

        #region new2.1
        /// <summary>
        /// 区域下拉列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetAearCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetVp_coinfo(CoAttribV2_1.Area);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               text = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("区域下拉列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 单位下拉列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetUnitCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetVp_coinfo(CoAttribV2_1.Unit);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               text = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("单位下拉列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 项目下拉列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetProjectCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetVp_coinfo(CoAttribV2_1.Project);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               text = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("项目下拉列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 用电单元
        /// </summary>
        /// <returns></returns>
        public APIRst GetCellCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetVp_coinfo(CoAttribV2_1.Cell);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               text = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("用电单元:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 回路下拉列表
        /// </summary>
        /// <param name="co_id">用电单元ID号</param>
        /// <returns></returns>
        public APIRst GetModuleCombox(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetModuleCombox(co_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               text = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("回路下拉列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 采集项下拉列表
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <returns></returns>
        public APIRst GetFunCombox(int module_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetFunCombox(module_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToString(s1["FunType"]),
                               text = CommFunc.ConvertDBNullToString(s1["FunName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("采集项下拉列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 查询类型下拉列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetDataTypeCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                List<object> dd = new List<object>();
                dd.Add(new { id = "hour", text = "逐时" });
                dd.Add(new { id = "day", text = "逐日" });
                dd.Add(new { id = "month", text = "逐月" });
                rst.data = dd;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("查询类型下拉列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 历史数据
        /// </summary>
        ///// <param name="co_id">用电单元ID号</param>
        /// <param name="module_id">回路ID号</param>
        /// <param name="funType">采集项目</param>
        /// <param name="dataType">查询类型</param>
        /// <param name="startTime">开始日期</param>
        /// <returns></returns>
        public APIRst GetHisData(int module_id, string funType, string dataType, DateTime startTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DateTime start = startTime, end = startTime;
                if (dataType.Equals("hour"))
                {
                    start = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
                    end = new DateTime(start.Year, start.Month, start.Day, 23, 0, 0);
                }
                if (dataType.Equals("day"))
                {
                    start = new DateTime(start.Year, start.Month, 1);
                    end = start.AddMonths(1).AddDays(-1);
                }
                if (dataType.Equals("month"))
                {
                    start = new DateTime(start.Year, 1, 1);
                    end = start.AddYears(1).AddDays(-1);
                }
                DataTable dtSource = bll.GetHisData(module_id, funType, dataType, start, end);
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               TagTime = CommFunc.ConvertDBNullToDateTime(s1["TagTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               Fun_id = CommFunc.ConvertDBNullToInt32(s1["Fun_id"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               TagTimeS = CommFunc.ConvertDBNullToString(s1["TagTimeS"]),
                               Cnt = CommFunc.ConvertDBNullToInt32(s1["Cnt"]),
                               Value = s1["Value"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["Value"]).ToString(),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("查询类型下拉列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 数据对比
        /// </summary>
        /// <param name="areaId">区域ID号</param>
        /// <param name="unitId">单元ID号</param>
        /// <param name="projectId">项目ID号</param>
        /// <returns></returns>
        public APIRst GetCompare(int areaId, int unitId, int projectId)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetCompare(areaId, unitId, projectId);
                int curTime = CommFunc.ConvertDBNullToInt32(dtSource.Compute("sum(CurTims)","true"));
                int preTime = CommFunc.ConvertDBNullToInt32(dtSource.Compute("sum(PreTims)", "true"));
                int per = preTime == 0 ? 0 : (int)((curTime - preTime) / (decimal)preTime) * 100;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               TagTime = CommFunc.ConvertDBNullToDateTime(s1["TagTime"]).ToString("yyyy年MM月"),
                               CurTims = CommFunc.ConvertDBNullToInt32(s1["CurTims"]),
                               PreTims = CommFunc.ConvertDBNullToInt32(s1["PreTims"]),
                               Per = CommFunc.ConvertDBNullToInt32(s1["Per"]),
                           };
                rst.data = new { list = res1.ToList(), per = per };
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("数据对比:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        #endregion
    }
}