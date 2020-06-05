using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using YDS6000.Models.Tables;

namespace YDS6000.WebApi.Areas.Exp.Opertion.RunReport
{
    public partial class ExpYdSsrHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.RunReport.ExpYdSsrBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpYdSsrHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.RunReport.ExpYdSsrBLL(WebConfig.SysProject, user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 获取拉合闸报表
        /// </summary>
        /// <param name="StrcName">建筑名称</param>
        /// <param name="CoName">房间名称</param>
        /// <param name="StartTime">日期开始</param>
        /// <param name="EndTime">日期结束</param>
        /// <returns></returns>
        public APIRst GetYdSsrOfList(string StrcName, string CoName, DateTime StartTime, DateTime EndTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdSsrOfList(StrcName, CoName, StartTime, EndTime);
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               FunType = CommFunc.ConvertDBNullToString(s1["FunType"]),
                               FunTypeS = CommFunc.ConvertDBNullToString(s1["FunTypeS"]),
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
                FileLog.WriteLog("获取拉合闸报表(GetYdSsrOfList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 汇出Excel
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public APIRst GetYdSsrOfExport(DataModels data)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = JsonHelper.ToDataTable(data.Data);
                string path = GetYdSsrOfExport(dtSource);
                rst.data = path;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("GetYdSsrOfExport(汇出Excel)错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private string GetYdSsrOfExport(DataTable dtSource)
        {
            string fn = "/XTemp/拉合闸报表.xls";
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
            //columns.Add(new Excel.ExcelColumn("建筑", "CoStrcName", 15) { IsSetWith = true });
            //columns.Add(new Excel.ExcelColumn("房间", "CoName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("建筑", "CoStrcName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("房间", "CoName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("电表", "ModuleAddr", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("拉合闸状态", "FunTypeS", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("操作人", "Create_by", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("操作时间", "Create_dt", 15) { IsSetWith = true, DefaultExcelCellStyle = columnCellStyle1 });

            //columns.Add(new Excel.ExcelColumn("E", "E", 15) { IsSetWith = true });
            Excel.ExcelOparete excel = new Excel.ExcelOparete("拉合闸报表");
            excel.SetObjectValue("拉合闸报表", 0, 0, 3);
            excel.SetColumnName(columns, 1, 0);
            excel.SetColumnValue(columns, dtSource.Select(), 2, 0);
            excel.SaveExcelByFullFileName(filename);
            return fn;
        }
    }
}