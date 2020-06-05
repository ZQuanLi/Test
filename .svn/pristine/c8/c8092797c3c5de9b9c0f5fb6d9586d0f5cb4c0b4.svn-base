using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 收费管理Api
    /// </summary>
    [RoutePrefix("api/Exp/Home")]
    public class ExpHomeController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.Home.HomeHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.Home.HomeHelper();
        /// <summary>
        /// 首页设备数，正常设备 ，故障数
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetEquInfo")]
        public APIRst GetEquInfo()
        {
            return infoHelper.GetEquInfo();
        }

        /// <summary>
        /// 首页告警数
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetEquAlarm")]
        public APIRst GetEquAlarm()
        {
            return infoHelper.GetEquAlarm();
        }


        /// <summary>
        /// 首页获取首页最新告警
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAlarmFisrt")]
        public APIRst GetAlarmFisrt()
        {
            return infoHelper.GetAlarmFisrt();
        }

        /// <summary>
        /// 项目导航
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetProjectList")]
        public APIRst GetProjectList()
        {
            return infoHelper.GetProjectList();
        }
        /// <summary>
        /// 隐患信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAlarmCompare")]
        public APIRst GetAlarmCompare()
        {
            return infoHelper.GetAlarmCompare();
        }
    }
}
