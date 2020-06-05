using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Energy.Opertion.Report
{
    partial class ReportHelper
    {
        /// <summary>
        /// 获取用能报表
        /// </summary>
        /// <param name="co_id">支路ID号</param>
        /// <param name="time">时间</param>
        /// <param name="dataType">类型 日=day月=month年year</param>
        /// <returns></returns>
        public APIRst GetEnergyUseVal(int co_id, DateTime time, string dataType,string moduleName)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetEnergyUseVal(co_id, time, dataType, moduleName);
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