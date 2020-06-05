using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.Energy.Controllers
{
    /// <summary>
    /// 能源首页Api
    /// </summary>
    [RoutePrefix("api/Energy/Home")]
    public partial class EngHomeController : ApiController
    {
        private HomeHelper infoHelper = new HomeHelper();

        /// <summary>
        /// 获取系统的信息
        /// </summary>     
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetSysInfo")]
        public APIRst GetSysInfo()
        {
            return infoHelper.GetSysInfo();
        }

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetProjectInfo")]
        public APIRst GetProjectInfo()
        {
            return infoHelper.GetProjectInfo();
        }

        /// <summary>
        /// 获取弹窗告警功能
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAlarmInfo")]
        public APIRst GetAlarmInfo()
        {
            return infoHelper.GetAlarmInfo();
        }

        /// <summary>
        /// 获取能源分类信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetEnergyList")]
        public APIRst GetEnergyList()
        {
            return infoHelper.GetEnergyList();
        }
        /// <summary>
        /// 获取当日用能趋势
        /// 第一个是当天的小时用能
        /// 第二个是昨日的小时用能
        /// 第三个当天的用能总和
        /// 最后是同期比
        /// </summary>
        /// <param name="id">分类ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetDayForChart")]
        public APIRst GetDayForChart(int id)
        {
            return infoHelper.GetDayForChart(id);
        }

        /// <summary>
        /// 获取当日用能趋势
        /// 第一个是当天的小时用能
        /// 第二个是昨日的小时用能
        /// 第三个当天的用能总和
        /// 最后是同期比
        /// </summary>
        /// <param name="id">配电房ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetDayForBuild")]
        public APIRst GetDayForBuild(int id)
        {
            return infoHelper.GetDayForBuild(id);
        }

        /// <summary>
        /// 获取建筑当日用能前10名
        /// </summary>
        /// <param name="id">分类ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetDayForTopLine")]
        public APIRst GetDayForTopLine(int id)
        {
            return infoHelper.GetDayForTopLine(id);
        }

        /// <summary>
        /// 获取分类用能数据
        /// </summary>     
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetClassification")]
        public APIRst GetClassification()
        {
            return infoHelper.GetClassification();
        }

        /// <summary>
        /// 获取分项用能数据
        /// </summary>     
        /// <param name="id">分类ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetDayForEnergy")]
        public APIRst GetDayForEnergy(int id)
        {
            return infoHelper.GetDayForEnergy(id);
        }
        /// <summary>
        /// 获取支路树形数据
        /// </summary>     
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetTree")]
        public APIRst GetTree()
        {
            return infoHelper.GetTree();
        }

    }
}
