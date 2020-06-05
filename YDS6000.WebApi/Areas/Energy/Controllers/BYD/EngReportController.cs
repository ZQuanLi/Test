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
    //[RoutePrefix("api/Energy/Report")]
    public partial class EngReportController : ApiController
    {
        //YDS6000.WebApi.Areas.Energy.Opertion.Report.ReportHelper infoHelper = new YDS6000.WebApi.Areas.Energy.Opertion.Report.ReportHelper();
        
        /// <summary>
        /// 获取用能报表(比亚迪)
        /// </summary>
        /// <param name="id">建筑ID号</param>
        /// <param name="time">时间</param>
        /// <param name="dataType">类型 日=day月=month年year</param>
        /// <param name="moduleName">回路名称</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetEnergyUseValForByd")]
        public APIRst GetEnergyUseValForByd(int id, DateTime time, string dataType, string moduleName = "",DateTime? endTime=null)
        {
            return infoHelper.GetEnergyUseValForByd(id, time,endTime, dataType, moduleName);
        }

        /// <summary>
        /// 获取分项统计(比亚迪)
        /// </summary>
        /// <param name="id">支路ID号</param>
        /// <param name="time">时间</param>
        /// <param name="dataType">类型 日=day月=month年year</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetEnergyItemForByd")]
        public APIRst GetEnergyItemForByd(int id,DateTime time,string dataType,DateTime? endTime=null)
        {
            return infoHelper.GetEnergyItemForByd(id, time,endTime, dataType);
        }
        


    }
}
