using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using YDS6000.Models.Tables;

namespace YDS6000.WebApi.Areas.Exp.Opertion.PayLog
{
    public partial class ExpYdPayLogHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.PayLog.ExpYdPayLogBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpYdPayLogHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.PayLog.ExpYdPayLogBLL(WebConfig.SysProject, user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 获取充值明细列表
        /// </summary>
        /// <param name="StrcName">建筑名称</param>
        /// <param name="CoName">房间名称</param>
        /// <param name="d_ways">缴费类型:0=全部,1=充值,2=退费,3=赠送电,4=退赠送电</param>
        /// <param name="Create_by">操作员</param>
        /// <param name="StartTime">缴费日期开始</param>
        /// <param name="EndTime">缴费日期结束</param>
        /// <returns></returns>
        public APIRst GetYdPayLogOnList(string StrcName, string CoName, int d_ways, string Create_by, DateTime StartTime, DateTime EndTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdPayLogOnList(StrcName, CoName, d_ways, Create_by, StartTime, EndTime);
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
                               IsPay = CommFunc.ConvertDBNullToInt32(s1["IsPay"]),
                               IsPayS = CommFunc.ConvertDBNullToString(s1["IsPayS"]),
                               Trade_no = CommFunc.ConvertDBNullToString(s1["Trade_no"]),
                               PayAmt = CommFunc.ConvertDBNullToDecimal(s1["PayAmt"]).ToString("f2"),
                               PayVal = CommFunc.ConvertDBNullToDecimal(s1["PayVal"]).ToString("f2"),
                               ErrCode = CommFunc.ConvertDBNullToInt32(s1["ErrCode"]),
                               ErrTxt = (CommFunc.ConvertDBNullToInt32(s1["IsPay"]) == 1 && CommFunc.ConvertDBNullToInt32(s1["ErrCode"]) == -2) ? "用户取消" : CommFunc.ConvertDBNullToString(s1["ErrTxt"]),
                               Create_by = CommFunc.ConvertDBNullToString(s1["Create_by"]),
                               Create_dt = CommFunc.ConvertDBNullToDateTime(s1["Create_dt"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               PayType = CommFunc.ConvertDBNullToInt32(s1["PayType"]),
                               PayTypeS = CommFunc.ConvertDBNullToString(s1["PayTypeS"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取充值用电信息错误(GetYdPayLogOnList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取首页缴费
        /// </summary>
        /// <param name="DayOrMonth">1=查询当日,2=查询当月</param>
        /// <returns></returns>
        public APIRst GetHomePay(int DayOrMonth)
        {
            APIRst rst = new APIRst();
            try
            {
                var StartTime = "";
                var EndTime = "";
                if (DayOrMonth == 1) //查询当日
                {
                    StartTime = DateTime.Now.ToString("yyyy-MM-dd");
                    EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (DayOrMonth == 2) //查询当月
                {
                    DateTime dt = DateTime.Now; //当前时间
                    StartTime = dt.AddDays(1 - dt.Day).ToString("yyyy-MM-dd"); //本月月初
                    EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                DataTable dtSource = bll.GetHomePay( StartTime, EndTime);
                decimal PayAmt = 0;
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    PayAmt = PayAmt + Convert.ToDecimal(dtSource.Rows[i]["PayAmt"].ToString());
                }

                object obj = new { Amount = dtSource.Rows.Count, PayAmt = PayAmt };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页缴费(GetHomePay):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 汇出Excel
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public APIRst GetYdPayLogOnExport(DataModels data)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = JsonHelper.ToDataTable(data.Data);
                string path = GetYdPayLogOnExport(dtSource);
                //object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = path;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("GetYdPayLogOnExport(汇出Excel)错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private string GetYdPayLogOnExport(DataTable dtSource)
        {
            string fn = "/XTemp/费用报表.xls";
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
            columns.Add(new Excel.ExcelColumn("建筑", "CoStrcName", 15) { IsSetWith = true }); //FiledName.FiledBuildName
            columns.Add(new Excel.ExcelColumn("房间", "CoName", 15) { IsSetWith = true });    ///FiledName.FiledBoomName
            columns.Add(new Excel.ExcelColumn("电表", "ModuleAddr", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("缴费类型", "PayTypeS", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("缴费方式", "IsPayS", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("金额", "PayAmt", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("电量", "PayVal", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("操作人", "Create_by", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("缴费时间", "Create_dt", 15) { IsSetWith = true });
            Excel.ExcelOparete excel = new Excel.ExcelOparete("费用报表");
            excel.SetObjectValue("费用报表", 0, 0, 3);
            excel.SetColumnName(columns, 1, 0);
            excel.SetColumnValue(columns, dtSource.Select(), 2, 0);
            excel.SaveExcelByFullFileName(filename);
            return fn;
        }


    }
}