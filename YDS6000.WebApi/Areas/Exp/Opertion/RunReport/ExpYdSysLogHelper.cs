using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using YDS6000.Models.Tables;
namespace YDS6000.WebApi.Areas.Exp.Opertion.RunReport
{
    public partial class ExpYdSysLogHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.RunReport.ExpYdSysLogBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpYdSysLogHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.RunReport.ExpYdSysLogBLL(user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 获取拉合闸报表
        /// </summary>
        /// <param name="UName">操作人</param>
        /// <param name="StartTime">日期开始</param>
        /// <param name="EndTime">日期结束</param>
        /// <returns></returns>
        public APIRst GetYdSysLogOfCmd(string UName, DateTime StartTime, DateTime EndTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdSysLogOfList(UName, StartTime, EndTime);
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               UName = CommFunc.ConvertDBNullToString(s1["UName"]),
                               Content = CommFunc.ConvertDBNullToString(s1["Content"]),
                               Result = "访问成功",
                               Ctime = CommFunc.ConvertDBNullToDateTime(s1["Ctime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取系统操作日志内容错误(GetYdSysLogOfCmd):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取系统控制日志
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdSysLogCtrl(string UName, DateTime StartTime, DateTime EndTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdSysLogCtrl(UName, StartTime, EndTime);
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               UName = CommFunc.ConvertDBNullToString(s1["UName"]),
                               Content = CommFunc.ConvertDBNullToString(s1["Descr"]),
                               Result = CommFunc.ConvertDBNullToInt32(s1["ErrCode"]) == 0 ? "已下发" : (CommFunc.ConvertDBNullToInt32(s1["ErrCode"]) == 1 ? "执行成功" : CommFunc.ConvertDBNullToString(s1["ErrTxt"])),
                               Ctime = CommFunc.ConvertDBNullToDateTime(s1["Create_dt"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取系统操作日志内容错误(GetYdSysLogCtrl):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}