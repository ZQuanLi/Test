using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using YDS6000.Models.Tables;

namespace YDS6000.WebApi.Areas.Exp.Opertion.RunReport
{
    public partial class ExpYdRepHisHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.RunReport.ExpYdRepHisBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpYdRepHisHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.RunReport.ExpYdRepHisBLL(WebConfig.SysProject, user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 运行历史报表
        /// </summary>
        /// <param name="StrcName">建筑名称</param>
        /// <param name="CoName">房间名称</param>
        /// <param name="StartTime">日期开始</param>
        /// <param name="EndTime">日期结束</param>
        /// <param name="IsMultiply">是否按倍率计算：1=是,</param>
        /// <returns></returns>
        public APIRst GetYdRepHisData(string StrcName, string CoName, DateTime StartTime, DateTime EndTime, int IsMultiply)
        {
            APIRst rst = new APIRst();
            try
            {
                var list = bll.GetYdRepHisList(StartTime, EndTime, StrcName, CoName, IsMultiply, new string[] { "E" });
                object obj = new { total = list.Count, rows = list.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("运行历史报表(GetYdRepHisData):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 汇出Excel
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public APIRst GetYdRepHisOnExport(DataModels data)
        {
            APIRst rst = new APIRst();
            try
            {
                string path = GetYdRepHisOnExport(data.Data);
                rst.data = path;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("GetYdRepHisOnExport(汇出Excel)错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private string GetYdRepHisOnExport(string jsonDataTable)
        {
            string fn = "/XTemp/能耗历史报表.xls";
            string filePath = System.Web.HttpContext.Current.Server.MapPath(@"/XTemp");
            if (System.IO.Directory.Exists(filePath) == false)
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            string filename = System.Web.HttpContext.Current.Server.MapPath(fn);
            if (System.IO.File.Exists(filename))/*先删除已存在的文件，再汇出Excel*/
                System.IO.File.Delete(filename);
            DataTable dtSource = new DataTable();
            if (jsonDataTable.Length > 10) dtSource = JsonHelper.ToDataTable(jsonDataTable);
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
            columns.Add(new Excel.ExcelColumn("序号", "ID", 15) { IsSetWith = true });
            //columns.Add(new Excel.ExcelColumn("建筑", "CoStrcName", 15) { IsSetWith = true });
            //columns.Add(new Excel.ExcelColumn("房间", "CoName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("建筑", "CoStrcName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("房间", "CoName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("电表地址", "ModuleAddr", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("日期范围", "Date", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("倍率", "Multiply", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("上期读数", "FirstVal", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("本期读数", "LastVal", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("用电量", "UseVal", 15) { IsSetWith = true });
            //columns.Add(new Excel.ExcelColumn("E", "E", 15) { IsSetWith = true });
            Excel.ExcelOparete excel = new Excel.ExcelOparete("能耗历史报表");
            excel.SetObjectValue("能耗历史报表", 0, 0, 3);
            excel.SetColumnName(columns, 1, 0);
            excel.SetColumnValue(columns, dtSource.Select(), 2, 0);
            excel.SaveExcelByFullFileName(filename);
            return fn;
        }
    }
}