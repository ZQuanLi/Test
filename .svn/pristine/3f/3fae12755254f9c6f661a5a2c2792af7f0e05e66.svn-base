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
    [RoutePrefix("api/ExpApp/Estate")]
    public class EstateController : ApiController
    {
        private YDS6000.WebApi.Areas.ExpApp.Opertion.Estate.EstateHelper infoHelper = new YDS6000.WebApi.Areas.ExpApp.Opertion.Estate.EstateHelper();

        /// <summary>
        /// 获取用户编号信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAlarmInfo")]
        public APIRst GetAlarmInfo()
        {
            return infoHelper.GetAlarmInfo();
        }


        /// <summary>
        /// 获取用户编号信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetUserInfo")]
        public APIRst GetUserInfo()
        {
            return infoHelper.GetUserInfo();
        }

        /// <summary>
        /// 首页账单
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetBill")]
        public APIRst GetBill()
        {
            return infoHelper.GetBill();
        }

        /// <summary>
        /// 账单明细
        /// </summary>
        /// <param name="start">开始日期</param>
        /// <param name="end">介绍日期</param>
        /// <param name="type">=0 ，全部记录=1扣费记录 =2 充值记录</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetBillDetail")]
        public APIRst GetBillDetail(DateTime start,DateTime end,int type=0)
        {
            return infoHelper.GetBillDetail(start, end, type);
        }

        /// <summary>
        /// 支付购电
        /// </summary>
        /// <param name="co_id">房间ID号</param>
        /// <param name="payAmt">充值金额</param>        
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("ForPay")]
        public APIRst ForPay(int co_id, decimal payAmt)
        {
            return infoHelper.ForPay(co_id, payAmt);
        }
    }
}
