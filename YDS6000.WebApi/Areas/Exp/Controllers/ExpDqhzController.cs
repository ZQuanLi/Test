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
    [RoutePrefix("api/Exp/Dqhz")]
    public class ExpDqhzController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.Dqhz.DqhzHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.Dqhz.DqhzHelper();
        /// <summary>
        /// 电气火灾列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetDqhzList")]
        public APIRst GetDqhzList()
        {
            return infoHelper.GetDqhzList();
        }
        /// <summary>
        /// 参数下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetFunTypes")]
        public APIRst GetFunTypes()
        {
            return infoHelper.GetFunTypes();
        }

        /// <summary>
        /// 获取参数详情
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="date">日期</param>
        /// <param name="dateType">类型 日=day 月=month 年=year</param>
        /// <param name="funType">参数类型</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetFunData")]
        public APIRst GetFunData(int module_id,DateTime date,string dateType, string funType)
        {
            return infoHelper.GetFunData(module_id, date, dateType, funType);
        }
    }
}
