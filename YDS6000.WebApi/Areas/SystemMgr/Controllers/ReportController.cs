using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YDS6000.Models;
using YDS6000.BLL.Report;

namespace YDS6000.WebApi.Areas.SystemMgr.Controllers
{
    /// <summary>
    /// 报表定制
    /// </summary>
    [AuthorizeAttribute]
    [RoutePrefix("api/Rpt")]
    public partial class ReportController : ApiController
    {
        private ReportHelper helper = new ReportHelper();

        #region 回路报表
        /// <summary>
        /// 对象选择列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSelectObject")]
        public APIResult GetSelectObject()
        {
            return helper.GetSelectObject();
        }

        /// <summary>
        /// 回路能耗统计分析(时间粒度)
        /// </summary>
        /// <param name="selKey">选择的对象</param>
        /// <param name="time">时间</param>
        /// <param name="dataType">查询类型 hour 逐时 day 日 month 月</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetYdModuleUseValList")]
        public APIResult GetYdModuleUseValList(string selKey,DateTime time,string dataType)
        {
            return helper.GetYdModuleUseValList(selKey, time, dataType);
        }
        /// <summary>
        /// 回路能耗统计分析(区间粒度)
        /// </summary>
        /// <param name="selKey">选择的对象</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetYdModuleCollectValList")]
        public APIResult GetYdModuleCollectValList(string selKey, DateTime start,DateTime end)
        {
            return helper.GetYdModuleCollectValList(selKey, start, end);
        }
        #endregion

        #region 机房类型统计分析
        /// <summary>
        /// 机房类型统计分析
        /// </summary>
        /// <param name="areaId">区域ID号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="stationTypeId">机房类型ID号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRptStationList")]
        public APIResult GetRptStationList(int areaId, DateTime start, DateTime end, int stationTypeId = 0)
        {
            return helper.GetRptStationList(areaId, start, end, stationTypeId);
        }
        #endregion

        #region 机房直流/交流统计分析
        /// <summary>
        /// 机房直流/交流统计分析
        /// </summary>
        /// <param name="areaId">区域ID号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="psWay">直流/交流</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRptPSWayList")]
        public APIResult GetRptPSWayList(int areaId, DateTime start, DateTime end, string psWay = "")
        {
            return helper.GetRptPSWayList(areaId, start, end, psWay);
        }
        #endregion

        #region 站点直供/转供统计分析
        /// <summary>
        /// 站点直供/转供统计分析
        /// </summary>
        /// <param name="areaId">区域ID号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="switch">直供/转供</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRptSwitchList")]
        public APIResult GetRptSwitchList(int areaId, DateTime start, DateTime end, string @switch = "")
        {
            return helper.GetRptSwitchList(areaId, start, end, @switch);
        }
        #endregion

        #region 站点场景类型统计分析
        /// <summary>
        /// 站点场景类型统计分析
        /// </summary>
        /// <param name="areaId">区域ID号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="roomSightId">场景ID号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRptRoomSightList")]
        public APIResult GetRptRoomSightList(int areaId, DateTime start, DateTime end, int roomSightId = 0)
        {
            return helper.GetRptRoomSightList(areaId, start, end, roomSightId);
        }
        #endregion
    }
}
