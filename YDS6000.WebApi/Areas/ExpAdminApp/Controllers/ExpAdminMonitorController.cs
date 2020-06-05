using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YDS6000.Models.Tables;

namespace YDS6000.WebApi.Areas.ExpAdminApp.Controllers
{
    /// <summary>
    /// 监测_管理端App
    /// </summary>
    [RoutePrefix("api/ExpAdminApp/Monitor")]
    public class ExpAdminMonitorController : ApiController
    {
        private YDS6000.WebApi.Areas.ExpAdminApp.Opertion.Monitor.ExpAdminMonitorHelper infoHelper = new YDS6000.WebApi.Areas.ExpAdminApp.Opertion.Monitor.ExpAdminMonitorHelper();

        /// <summary>
        /// 抄表数据监视—获取抄表数据监视列表
        /// </summary>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <param name="ssr">拉合闸状态(可选，默认为空)</param>
        /// <param name="co_id">建筑节点</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdMonitorOnList")]
        public APIRst GetYdMonitorOnList(string strcName = "", string coName = "", string ssr = "", int co_id = 0)
        {
            return infoHelper.GetYdMonitorOnList(strcName, coName, ssr, co_id);
        }

        /// <summary>
        /// 获取城市查询参数
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetCity")]
        public APIRst GetCity()
        {
            return infoHelper.GetCity(0);
        }

        /// <summary>
        /// 获取区域查询参数
        /// </summary>
        /// <param name="Co_id">id</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetRegion")]
        public APIRst GetRegion(int Co_id)
        {
            return infoHelper.GetRegion(Co_id);
        }

        /// <summary>
        /// 获取单位查询参数
        /// </summary>
        /// <param name="Co_id">id</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetUnit")]
        public APIRst GetUnit(int Co_id)
        {
            return infoHelper.GetUnit(Co_id);
        }

        /// <summary>
        /// 获取建筑查询参数
        /// </summary>
        /// <param name="Co_id">id</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetBuilding")]
        public APIRst GetBuilding(int Co_id)
        {
            return infoHelper.GetBuilding(Co_id);
        }

        /// <summary>
        /// 获取房间查询参数
        /// </summary>
        /// <param name="Co_id">id</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetRoom")]
        public APIRst GetRoom(int Co_id)
        {
            return infoHelper.GetRoom(Co_id);
        }

        /// <summary>
        /// 抄表数据监视—批量远程控制(合闸/拉闸/保电/非保电)
        /// </summary>
        /// <param name="v0Fun">功能类型(Ssr:闸，IsPaul:电)</param>
        /// <param name="dataValue">设置值(0:合闸/非保电，1:拉闸/保电)</param>
        /// <param name="pStr">电表id号，多个id号以逗号隔开</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("YdBatchControlOfSsr")]
        public APIRst YdBatchControlOfSsr(string v0Fun, string dataValue, string pStr)
        {
            return infoHelper.YdBatchControlOfSsr(v0Fun, dataValue, pStr);
        }



    }
}