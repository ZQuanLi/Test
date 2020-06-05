using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 系统配置-费率管理
    /// </summary>
    [RoutePrefix("api/Exp/Syscont")]
    public class ExpRateController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpRateHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpRateHelper();

        /// <summary>
        /// 获取费率信息
        /// </summary>
        /// <param name="Descr">筛选条件：费率描述</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdRateList")]
        public APIRst GetYdRateList(string Descr)
        {
            return infoHelper.GetYdRateList(Descr);
        }

        /// <summary>
        /// 设置保存费率信息
        /// </summary>
        /// <param name="T1st">尖单价-对应的开始时间</param>
        /// <param name="T2nd">峰单价-对应的开始时间</param>
        /// <param name="T3rd">平单价-对应的开始时间</param>
        /// <param name="T4th">谷单价-对应的开始时间</param>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="Descr">费率描述</param>
        /// <param name="Pri1st">尖单价</param>
        /// <param name="Pri2nd">峰单价</param>
        /// <param name="Pri3rd">平单价</param>
        /// <param name="Pri4th">谷单价</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetSaveYdRate")]
        public APIRst SetSaveYdRate(string T1st, string T2nd, string T3rd, string T4th, int Rate_id, string Descr, decimal Pri1st, decimal Pri2nd, decimal Pri3rd, decimal Pri4th)
        {
            return infoHelper.SetSaveYdRate(T1st, T2nd, T3rd, T4th, Rate_id, Descr, Pri1st, Pri2nd, Pri3rd, Pri4th);
        }

        /// <summary>
        /// 删除费率信息
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetDelYdRate")]
        public APIRst GetDelYdRate(int Rate_id)
        {
            return infoHelper.GetDelYdRate(Rate_id);
        }
    }
}