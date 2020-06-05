using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.Energy.Controllers
{
    /// <summary>
    /// 能源实时监控Api
    /// </summary>
    [RoutePrefix("api/Energy/Monitor")]
    public class EngMonitorController : ApiController
    {
        private MonitorHelper infoHelper = new MonitorHelper();

        /// <summary>
        /// 获取回路的实时列表数据
        /// </summary>
        /// <param name="id">配电房ID号</param>
        /// <param name="moduleName">回路名称</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetRealList")]
        public APIRst GetRealList(int id,string moduleName="")
        {
            return infoHelper.GetRealList(id, moduleName);
        }

        /// <summary>
        /// 获取采集点实时数据
        /// </summary>
        /// <param name="tag">采集点号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetRealTime")]
        public APIRst GetRealTime(string tag)
        {
            return infoHelper.GetRealTime(tag);
        }
        /// <summary>
        /// 获取回路的实时曲线数据
        /// </summary>
        /// <param name="id">回路ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetRealChart")]
        public APIRst GetRealChart(int id)      
        {
            return infoHelper.GetRealChart(id);
        }

        /// <summary>
        /// 更换电表
        /// </summary>
        /// <param name="id">设备信息ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetModuleOfMapCollect")]
        public APIRst GetModuleOfMapCollect(int id)
        {
            return infoHelper.GetModuleOfMapCollect(id);
        }
        
        /// <summary>
        /// 保存更换电表
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="fun_id">采集ID号</param>
        /// <param name="lastVal">最后值</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("UpdateLastVal")]
        public APIRst UpdateLastVal(int module_id,int fun_id,decimal lastVal)
        {
            return infoHelper.UpdateLastVal(module_id, fun_id, lastVal);
        }

    }
}
