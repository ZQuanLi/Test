using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 系统监视Api
    /// </summary>
    [RoutePrefix("api/Exp/PayCharg")]
    public class ExpPayChargController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.PayCharg.PayChargHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.PayCharg.PayChargHelper();

        /// <summary>
        /// 获取物业列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetChargList")]
        public APIRst GetChargList()
        {
            return infoHelper.GetChargList();
        }

        /// <summary>
        /// 新增物业列表
        /// </summary>
        /// <param name="co_id">房间ID号</param>
        /// <param name="firstVal">上期计数</param>
        /// <param name="lastVal">本期计数</param>
        /// <param name="firstTime">上期时间</param>
        /// <param name="lastTime">本期时间</param>
        /// <param name="price">水费单价</param>
        /// <param name="chargAmt">物业费</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("AddCharg")]
        public APIRst AddCharg(int co_id, decimal firstVal, decimal lastVal, DateTime firstTime, DateTime lastTime, decimal price, decimal chargAmt)
        {
            return infoHelper.AddCharg(co_id, firstVal, lastVal, firstTime, lastTime, price, chargAmt);
        }
        /// <summary>
        /// 修改物业列表
        /// </summary>
        /// <param name="log_id">ID号</param>
        /// <param name="firstVal">上期计数</param>
        /// <param name="lastVal">本期计数</param>
        /// <param name="firstTime">上期时间</param>
        /// <param name="lastTime">本期时间</param>
        /// <param name="price">水费单价</param>
        /// <param name="chargAmt">物业费</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("UpdateCharg")]
        public APIRst UpdateCharg(long log_id,decimal firstVal, decimal lastVal, DateTime firstTime, DateTime lastTime, decimal price, decimal chargAmt)
        {
            return infoHelper.UpdateCharg(log_id, firstVal, lastVal, firstTime, lastTime, price, chargAmt);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="log_id">ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("DeleteCharg")]
        public APIRst DeleteCharg(long log_id)
        {
            return infoHelper.DeleteCharg(log_id);
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("ToLeadCharg")]
        public APIRst ToLeadCharg()
        {
            return infoHelper.ToLeadCharg();
        }
    }

}
