using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YDS6000.Models.Tables;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 运行报表-能耗历史报表
    /// </summary>
    [RoutePrefix("api/Exp/RunReport")]
    public class ExpYdRepHisController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.RunReport.ExpYdRepHisHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.RunReport.ExpYdRepHisHelper();

        /// <summary>
        /// 获取能耗历史报表
        /// </summary>
        /// <param name="StrcName">建筑名称</param>
        /// <param name="CoName">房间名称</param>
        /// <param name="StartTime">日期开始</param>
        /// <param name="EndTime">日期结束</param>
        /// <param name="IsMultiply">是否按倍率计算：1=是,</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdRepHisData")]
        public APIRst GetYdRepHisData(string StrcName, string CoName, DateTime StartTime, DateTime EndTime, int IsMultiply)
        {
            return infoHelper.GetYdRepHisData(StrcName, CoName, StartTime, EndTime, IsMultiply);
        }

        /// <summary>
        /// 汇出Excel
        /// </summary>
        /// <param name="Data">对象：传输Json格式的数据,转格式var data = JSON.stringify(this.list);</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("GetYdRepHisOnExport")]
        public APIRst GetYdRepHisOnExport(DataModels Data)
        {
            return infoHelper.GetYdRepHisOnExport(Data);
        }
    }
}