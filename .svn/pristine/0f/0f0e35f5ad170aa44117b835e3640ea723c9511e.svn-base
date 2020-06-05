using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.PDU.Controllers
{
    /// <summary>
    /// 实时数据
    /// </summary>
    [RoutePrefix("api/Pdu/Monitor")]
    public class PduMonitorController : ApiController
    {
        private YDS6000.WebApi.Areas.PDU.Opertion.Monitor.MonitorHelper infoHelper = new YDS6000.WebApi.Areas.PDU.Opertion.Monitor.MonitorHelper();

        /// <summary>
        /// 获取监测数据信息
        /// </summary>     
        /// <param name="id">Pdu ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMonitorInfo")]
        public APIRst GetMonitorInfo(int id)
        {
            return infoHelper.GetMonitorInfo(id);
        }
        /// <summary>
        /// 获取监测数据列表
        /// </summary>     
        /// <param name="id">Pdu ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMonitorList")]
        public APIRst GetMonitorList(int id)
        {
            return infoHelper.GetMonitorList(id);
        }
        /// <summary>
        /// 获取第一层数据,获取机房信息
        /// </summary>     
        /// <param name="id">层级1的id号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetLayer01")]
        public APIRst GetLayer01(int id)
        {
            return infoHelper.GetLayer01(id);
        }

        /// <summary>
        /// 获取第二层数据,获取机房下的K1，K2信息
        /// </summary>     
        /// <param name="id">层级2的id号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetLayer02")]
        public APIRst GetLayer02(int id)
        {
            return infoHelper.GetLayer02(id);
        }

        /// <summary>
        /// 获取第三层数据，K房下的机柜信息
        /// </summary>     
        /// <param name="id">层级3的id号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetLayer03")]
        public APIRst GetLayer03(int id)
        {
            return infoHelper.GetLayer03(id);
        }

        /// <summary>
        /// 获取第四层数据； 机柜下的Pdu
        /// </summary>     
        /// <param name="id">层级3的id号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetLayer04")]
        public APIRst GetLayer04(int id)
        {
            return infoHelper.GetLayer04(id);
        }
        #region 2019.02.25
        /// <summary>
        /// 获取监测数据信息
        /// </summary>     
        /// <param name="id">Pdu ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMonitorInfo200")]
        public APIRst GetMonitorInfo200(int id)
        {
            return infoHelper.GetMonitorInfo200(id);
        }
        /// <summary>
        /// 获取监测数据列表
        /// </summary>     
        /// <param name="id">Pdu ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMonitorList200")]
        public APIRst GetMonitorList200(int id)
        {
            return infoHelper.GetMonitorList200(id);
        }
        /// <summary>
        /// 获取传感器数据列表
        /// </summary>     
        /// <param name="id">Pdu ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMonitorSensor200")]
        public APIRst GetMonitorSensor200(int id)
        {
            return infoHelper.GetMonitorSensor200(id);
        }
        #endregion
    }
}
