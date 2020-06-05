using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.Energy.Controllers
{
    /// <summary>
    /// 能源统计分析Api
    /// </summary>
    [RoutePrefix("api/Energy/Report")]
    public partial class EngReportController : ApiController
    {
        YDS6000.WebApi.Areas.Energy.Opertion.Report.ReportHelper infoHelper = new YDS6000.WebApi.Areas.Energy.Opertion.Report.ReportHelper();

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="id">建筑ID号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="moduleName">回路名称</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetEnergyHisdata")]
        public APIRst GetEnergyHisdata(int id, DateTime start, DateTime end, string moduleName = "")
        {
            return infoHelper.GetEnergyHisdata(id, start, end, moduleName);
        }

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="id">建筑ID号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="moduleName">回路名称</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAlarmList")]
        public APIRst GetAlarmList(int id, DateTime start, DateTime end, string moduleName = "")
        {
            return infoHelper.GetAlarmList(id, start, end, moduleName);
        }

        /// <summary>
        /// 获取用能报表
        /// </summary>
        /// <param name="id">建筑ID号</param>
        /// <param name="time">时间</param>
        /// <param name="dataType">类型 日=day月=month年year</param>
        /// <param name="moduleName">回路名称</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetEnergyUseVal")]
        public APIRst GetEnergyUseVal(int id, DateTime time, string dataType, string moduleName = "")
        {
            return infoHelper.GetEnergyUseVal(id, time, dataType, moduleName);
        }

        /// <summary>
        /// 获取分项统计
        /// </summary>
        /// <param name="id">支路ID号</param>
        /// <param name="time">时间</param>
        /// <param name="dataType">类型 日=day月=month年year</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetEnergyItem")]
        public APIRst GetEnergyItem(int id,DateTime time,string dataType)
        {
            return infoHelper.GetEnergyItem(id, time, dataType);
        }

        /// <summary>
        /// 获取X261焊接单元数据(本特勒项目特殊定制)
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetEnergyForDayX261")]
        public APIRst GetEnergyForDayX261()
        {
            return infoHelper.GetEnergyForDayX261();
        }


    }
}
