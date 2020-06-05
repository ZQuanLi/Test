using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.PDU.Opertion.Report
{
    public partial class ReportHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.PDU.Report.ReportBLL bll = null;
        public ReportHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.PDU.Report.ReportBLL(user.Ledger, user.Uid);
        }
        /// <summary>
        /// 能耗统计
        /// </summary>     
        /// <param name="co_id">Pdu ID号</param>
        /// <param name="dataType">类型 day 日month 月year 年</param>
        /// <param name="dataTime">时间</param>
        /// <returns></returns>
        public APIRst GetEnergy(int co_id, string dataType, DateTime dataTime)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetEnergy(co_id, dataType, dataTime);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取能耗统计错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="moduleName">设备名称</param>
        /// <returns></returns>
        public APIRst GetHisdata(DateTime start, DateTime end, string moduleName = "", string funName ="")
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetHisdata(start, end, moduleName, funName);
                var res1 = from s1 in dtSource.AsEnumerable()
                           orderby s1["Module_id"],s1["Fun_id"], s1["TagTime"]
                           select new
                           {
                               TagTime = CommFunc.ConvertDBNullToDateTime(s1["TagTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               PduName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               LastVal = CommFunc.ConvertDBNullToDecimal(s1["LastVal"]).ToString("f2"),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取历史数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取用能报表
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="dataType">类型 日=day月=month年year</param>
        /// <param name="moduleName">回路名称</param>
        /// <returns></returns>
        public APIRst GetEnergyUseVal(DateTime time, string dataType, string moduleName = "")
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetEnergyUseVal(time, dataType, moduleName);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取用能报表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}