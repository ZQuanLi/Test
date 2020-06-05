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
    /// 系统配置-定时断送电策略
    /// </summary>
    [RoutePrefix("api/Exp/Syscont")]
    public class ExpTimingController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpTimingHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpTimingHelper();

        /// <summary>
        /// 获取定时断送电策略
        /// </summary>
        /// <param name="psi_id"></param>
        /// <param name="Descr">筛选条件：描述</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdTiming")]
        public APIRst GetYdTiming(int psi_id, string Descr)
        {
            return infoHelper.GetYdTiming(psi_id, Descr);
        }

        /// <summary>
        /// 获取新增页面内的时段设置的表格
        /// </summary>
        /// <param name="nAct">1=增加,2=修改,3=删除</param>
        /// <param name="Psi_id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetsiSSR")]
        public APIRst GetsiSSR(int nAct, int Psi_id)
        {
            return infoHelper.GetsiSSR(nAct, Psi_id);
        }
        /// <summary>
        /// 获取电表号列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetListM_Num")]
        public APIRst GetListM_Num()
        {
            return infoHelper.GetListM_Num();
        }

        /// <summary>
        /// 获取时间列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetListHourNum")]
        public APIRst GetListHourNum()
        {
            return infoHelper.GetListHourNum();
        }

        /// <summary>
        /// 获取分钟列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetListSecondNum")]
        public APIRst GetListSecondNum()
        {
            return infoHelper.GetListSecondNum();
        }

        /// <summary>
        /// 设置定时电表策略
        /// </summary>
        /// <param name="nAct">1=增加,2=修改,3=删除</param>
        /// <param name="si_id">策略Id,修改和删除需要,新增时传0即可</param>
        /// <param name="pRemark">描述</param>
        /// <param name="pIsClosed">是否启用:1=启用,0=不启用</param>
        /// <param name="pcT">日时区策略,用逗号拼接成字符串</param>
        /// <param name="pcM">日时区策略-定时段表号,用逗号拼接成字符串</param>
        /// <param name="pcW">星期策略,用逗号拼接成字符串</param>
        /// <param name="pcD">特殊日期策略,用逗号拼接成字符串</param>
        /// <param name="pcB">特殊日期策略-定时段表号,用逗号拼接成字符串</param>
        /// <param name="Timing"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetTiming")]
        public APIRst SetTiming(int nAct,int si_id, string pRemark, int pIsClosed, string pcT, string pcM, string pcW, string pcD, string pcB, DataModels Timing)
        {
            return infoHelper.SetTiming(nAct, si_id, pRemark, pIsClosed, pcT, pcM, pcW, pcD, pcB, Timing);
        }

    }
}