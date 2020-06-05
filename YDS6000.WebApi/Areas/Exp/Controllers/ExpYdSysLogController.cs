using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YDS6000.Models.Tables;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 运行报表-系统操作日志
    /// </summary>
    [RoutePrefix("api/Exp/RunReport")]
    public class ExpYdSysLogController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.RunReport.ExpYdSysLogHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.RunReport.ExpYdSysLogHelper();

        /// <summary>
        /// 获取系统操作日志
        /// </summary>
        /// <param name="UName">操作人</param>
        /// <param name="StartTime">日期开始</param>
        /// <param name="EndTime">日期结束</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdSysLogOfCmd")]
        public APIRst GetYdSysLogOfCmd(string UName, DateTime StartTime, DateTime EndTime)
        {
            return infoHelper.GetYdSysLogOfCmd(UName, StartTime, EndTime);
        }

        /// <summary>
        /// 获取系统控制日志
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdSysLogCtrl")]
        public APIRst GetYdSysLogCtrl(string UName, DateTime StartTime, DateTime EndTime)
        {
            return infoHelper.GetYdSysLogCtrl(UName, StartTime, EndTime);
        }
    }
}