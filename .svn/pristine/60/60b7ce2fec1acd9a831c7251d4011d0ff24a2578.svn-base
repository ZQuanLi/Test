using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using YDS6000.Models.Tables;


namespace YDS6000.WebApi.Areas.Exp.Opertion.RunReport
{
    public partial class ExpYdCollectHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.RunReport.ExpYdCollectBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpYdCollectHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.RunReport.ExpYdCollectBLL(WebConfig.SysProject, user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 获取采集历史报表
        /// </summary>
        /// <param name="StrcName">建筑名称</param>
        /// <param name="CoName">房间名称</param>
        /// <param name="FunType">电参量:0=全部,10=商户,20=商铺,30=宿舍,40=厂房</param>
        /// <param name="IsDefine">属性:E=电量,Ia=A相电流,Ib=B相电流,Ic=C相电流,Ua=A相电压,Ub=B相电压,Uc=C相电压,Psum=总有功功率,Pa=A相有功功率,Pb=B相有功功率,Pc=C相有功功率,Qsum=总无功功率,Qa=A相无功功率,Qb=B相无功功率,Qc=C相无功功率,Pfav=总功率因数,PFa=A相功率因数,PFb=B相功率因数,PFc=C相功率因数</param>
        /// <param name="StartTime">日期开始</param>
        /// <param name="EndTime">日期结束</param>
        /// <returns></returns>
        public APIRst GetYdCollectData(string StrcName, string CoName, string FunType, int IsDefine, DateTime StartTime, DateTime EndTime)
        {
            APIRst rst = new APIRst();
            try
            {
                if (string.IsNullOrEmpty(FunType)) FunType = "E";
                DataTable dtSource = bll.GetYdCollectData(StrcName, CoName, FunType, IsDefine, StartTime, EndTime);
                var res1 = from s1 in dtSource.AsEnumerable()
                           orderby s1["Module_id"], s1["ModuleAddr"]
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               TagTime = CommFunc.ConvertDBNullToDateTime(s1["TagTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               LastVal = CommFunc.ConvertDBNullToDecimal(s1["LastVal"]),
                               FunType = CommFunc.ConvertDBNullToString(s1["FunType"]),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取采集历史报表(GetYdCollectData):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 汇出Excel
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public APIRst GetYdCollectOnExport(DataModels data)
        {
            APIRst rst = new APIRst();
            try
            {
                //DataTable dtSource = JsonHelper.ToDataTable(data.Data);
                string path = GetYdCollectOnExport(data.Data);
                rst.data = path;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("GetYdCollectOnExport(汇出Excel)错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private string GetYdCollectOnExport(string jsonDataTable)
        {
            string fn = "/XTemp/采集历史报表.xls";
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
            string funType = dtSource.Rows.Count == 0 ? "" : funType = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["FunType"]);
            string content = "";
            System.Reflection.FieldInfo info = typeof(V0Fun).GetField(funType);
            if (info != null)
            {
                var obj = info.GetCustomAttributes(typeof(Describe), false);
                if (obj != null)
                {
                    foreach (Describe md in obj)
                        content = md.describe;
                }
            }

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
            columns.Add(new Excel.ExcelColumn("序号", "RowId", 5) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("建筑", "CoStrcName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("房间", "CoName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("电表地址", "ModuleAddr", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("采集时间", "TagTime", 20) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("读数", "LastVal", 15) { IsSetWith = true });
            Excel.ExcelOparete excel = new Excel.ExcelOparete("采集历史报表");
            excel.SetObjectValue("采集历史报表" + (content == "" ? "" : ":" + content), 0, 0, 3);
            excel.SetColumnName(columns, 1, 0);
            excel.SetColumnValue(columns, dtSource.Select(), 2, 0);
            excel.SaveExcelByFullFileName(filename);
            return fn;
        }
    }
}