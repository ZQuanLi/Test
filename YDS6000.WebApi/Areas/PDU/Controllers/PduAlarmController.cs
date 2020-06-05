using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.PDU.Controllers
{
    /// <summary>
    /// 告警信息
    /// </summary>
    [RoutePrefix("api/Pdu/Alarm")]
    public class PduAlarmController : ApiController
    {
        private YDS6000.WebApi.Areas.PDU.Opertion.Alarm.AlarmHelper infoHelper = new YDS6000.WebApi.Areas.PDU.Opertion.Alarm.AlarmHelper();

        /// <summary>
        /// 获取告警信息
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="moduleName">设备名称</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAlarmData")]
        public APIRst GetAlarmData(DateTime start, DateTime end, string moduleName = "")
        {
            return infoHelper.GetAlarmData(start, end, moduleName);
        }
    }
}
