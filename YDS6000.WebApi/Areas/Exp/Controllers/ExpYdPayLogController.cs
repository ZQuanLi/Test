using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YDS6000.Models;
using YDS6000.Models.Tables;


namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 费用报表-充值明细
    /// </summary>
    [RoutePrefix("api/Exp/PayLog")]
    public class ExpYdPayLogController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.PayLog.ExpYdPayLogHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.PayLog.ExpYdPayLogHelper();

        /// <summary>
        /// 获取充值明细列表
        /// </summary>
        /// <param name="StrcName">建筑名称</param>
        /// <param name="CoName">房间名称</param>
        /// <param name="d_ways">缴费类型:0=全部,1=充值,2=退费,3=赠送电,4=退赠送电</param>
        /// <param name="Create_by">操作员</param>
        /// <param name="StartTime">缴费日期开始</param>
        /// <param name="EndTime">缴费日期结束</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdPayLogOnList")]
        public APIRst GetYdPayLogOnList(string StrcName, string CoName, int d_ways, string Create_by, DateTime StartTime, DateTime EndTime)
        {
            return infoHelper.GetYdPayLogOnList(StrcName, CoName, d_ways, Create_by, StartTime, EndTime);
        }

        /// <summary>
        /// 获取首页当日缴费
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetHomePayDay")]
        public APIRst GetHomePayDay()
        {
            return infoHelper.GetHomePay(1); // 1 = 查询当日
        }

        /// <summary>
        /// 获取首页当月缴费
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetHomePayMonth")]
        public APIRst GetHomePayMonth()
        {
            return infoHelper.GetHomePay(2); // 2 = 查询当月
        }

        /// <summary>
        /// 汇出Excel
        /// </summary>
        /// <param name="Data">对象：传输Json格式的数据,转格式var data = JSON.stringify(this.list);</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("GetYdPayLogOnExport")]
        public APIRst GetYdPayLogOnExport(DataModels Data)
        {
            return infoHelper.GetYdPayLogOnExport(Data);
        }

    }
}