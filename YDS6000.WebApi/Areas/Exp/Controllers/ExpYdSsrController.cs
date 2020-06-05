using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YDS6000.Models.Tables;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 运行报表-拉合闸报表
    /// </summary>
    [RoutePrefix("api/Exp/RunReport")]
    public class ExpYdSsrController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.RunReport.ExpYdSsrHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.RunReport.ExpYdSsrHelper();

        /// <summary>
        /// 获取拉合闸报表
        /// </summary>
        /// <param name="StrcName">建筑名称</param>
        /// <param name="CoName">房间名称</param>
        /// <param name="StartTime">日期开始</param>
        /// <param name="EndTime">日期结束</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdSsrOfList")]
        public APIRst GetYdSsrOfList(string StrcName, string CoName, DateTime StartTime, DateTime EndTime)
        {
            return infoHelper.GetYdSsrOfList(StrcName, CoName, StartTime, EndTime);
        }

        /// <summary>
        /// 汇出Excel
        /// </summary>
        /// <param name="Data">对象：传输Json格式的数据,转格式var data = JSON.stringify(this.list);</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("GetYdSsrOfExport")]
        public APIRst GetYdSsrOfExport(DataModels Data)
        {
            return infoHelper.GetYdSsrOfExport(Data);
        }
    }
}