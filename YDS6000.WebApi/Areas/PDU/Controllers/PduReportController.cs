using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.PDU.Controllers
{
    /// <summary>
    /// 查询统计
    /// </summary>
    [RoutePrefix("api/Pdu/Report")]
    public class PduReportController : ApiController
    {
        private YDS6000.WebApi.Areas.PDU.Opertion.Report.ReportHelper infoHelper = new YDS6000.WebApi.Areas.PDU.Opertion.Report.ReportHelper();
        /// <summary>
        /// 能耗统计
        /// </summary>     
        /// <param name="id">Pdu ID号</param>
        /// <param name="dataType">类型 day 日month 月year 年</param>
        /// <param name="dataTime">时间</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetEnergy")]
        public APIRst GetEnergy(int id, string dataType, DateTime dataTime)
        {
            return infoHelper.GetEnergy(id, dataType, dataTime);
        }

        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="moduleName">设备名称</param>
        /// <param name="funName">采集点名称</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetHisdata")]
        public APIRst GetHisdata( DateTime start, DateTime end, string moduleName = "",string funName = "")
        {
            return infoHelper.GetHisdata(start, end, moduleName, funName);
        }

        /// <summary>
        /// 获取用能报表
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="dataType">类型 日=day月=month年year</param>
        /// <param name="moduleName">插座名称</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetEnergyUseVal")]
        public APIRst GetEnergyUseVal(DateTime time, string dataType, string moduleName = "")
        {
            return infoHelper.GetEnergyUseVal(time, dataType, moduleName);
        }
    }
}
