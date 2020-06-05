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
        /// 获取X261焊接单元数据(本特勒项目特殊定制)
        /// </summary>
        /// <returns></returns>
        public APIRst GetEnergyForDayX261()
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetEnergyForDayX261();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取X261焊接单元数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}