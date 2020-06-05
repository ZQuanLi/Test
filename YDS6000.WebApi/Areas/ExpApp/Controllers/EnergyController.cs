using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.ExpApp.Controllers
{
    /// <summary>
    /// 手机用电APP
    /// </summary>
    [RoutePrefix("api/ExpApp/Energy")]
    public class ExpAppEnergyController : ApiController
    {
        private YDS6000.WebApi.Areas.ExpApp.Opertion.Energy.EnergyHelper infoHelper = new YDS6000.WebApi.Areas.ExpApp.Opertion.Energy.EnergyHelper();

        /// <summary>
        /// 本月用电信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMonthUseVal")]
        public APIRst GetMonthUseVal()
        {
            return infoHelper.GetMonthUseVal();
        }

        /// <summary>
        /// 支付购电信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetPayUseVal")]
        public APIRst GetPayUseVal()
        {
            return infoHelper.GetPayUseVal();
        }

        /// <summary>
        /// 电量电费
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetElectricity")]
        public APIRst GetElectricity()
        {
            return infoHelper.GetElectricity();
        }

        /// <summary>
        /// 购电记录
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetPayLog")]
        public APIRst GetPayLog(DateTime startTime,DateTime endTime)
        {
            return infoHelper.GetPayLog(startTime,endTime);
        }

        /// <summary>
        /// 月份用能分析
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMonthAnalysis")]
        public APIRst GetMthAnalysis(DateTime time)
        {
            return infoHelper.GetMonthAnalysis(time);
        }

        /// <summary>
        /// 支付购电
        /// </summary>
        /// <param name="module_id">电表ID号</param>
        /// <param name="payAmt">充值金额</param>        
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("ForPay")]
        public APIRst ForPay(int module_id, decimal payAmt)
        {
            return infoHelper.ForPay(module_id, payAmt);
        }
    }
}
