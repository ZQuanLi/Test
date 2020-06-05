using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 系统配置-物业收费标准
    /// </summary>
    [RoutePrefix("api/Exp/Syscont")]
    public class ExpRateNewController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpRateNewHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpRateNewHelper();

        /// <summary>
        /// 获取物业收费信息
        /// </summary>
        /// <param name="Descr">筛选条件：描述</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdRateNewList")]
        public APIRst GetYdRateNewList(string Descr)
        {
            return infoHelper.GetYdRateNewList(Descr);
        }

        /// <summary>
        /// 设置保存物业收费
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="Descr">描述</param>
        /// <param name="Rule">计算规则: 0=正常,1=时间范围,2=数量范围</param>
        /// <param name="Unit">单位: Area=平方米,Bank=户数</param>
        /// <param name="UnitBase">单位基数</param>
        /// <param name="Disabled">是否弃用:0=否,1=是</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetSaveYdRateNew")]
        public APIRst SetSaveYdRateNew(int Rate_id, string Descr, int Rule, string Unit, decimal UnitBase, int Disabled)
        {
            return infoHelper.SetSaveYdRateNew(Rate_id, Descr, Rule, Unit, UnitBase, Disabled);
        }

        /// <summary>
        /// 删除物业收费
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetDelYdRateNew")]
        public APIRst GetDelYdRateNew(int Rate_id)
        {
            return infoHelper.GetDelYdRateNew(Rate_id);
        }

        /// <summary>
        /// 获取物业收费-单价详情
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdRateNewCs")]
        public APIRst GetYdRateNewCs(int Rate_id)
        {
            return infoHelper.GetYdRateNewCs(Rate_id);
        }

        /// <summary>
        /// 设置物业收费-单价详情
        /// </summary>
        /// <param name="Rule">计算规则: 0=正常,1=时间范围,2=数量范围</param>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="CsId">单价ID号</param>
        /// <param name="Price">单价</param>
        /// <param name="dtPStart">开始区间1</param>
        /// <param name="dtPEnd">结束区间1</param>
        /// <param name="nPStart">开始区间2</param>
        /// <param name="nPEnd">结束区间2</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetSaveYdRateNewCs")]
        public APIRst SetSaveYdRateNewCs(int Rule, int Rate_id, int CsId, decimal Price, DateTime dtPStart, DateTime dtPEnd, decimal nPStart, decimal nPEnd)
        {
            return infoHelper.SetSaveYdRateNewCs(Rule, Rate_id, CsId, Price, dtPStart, dtPEnd, nPStart, nPEnd);
        }

        /// <summary>
        /// 删除物业收费-单价详情
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="csId">单价ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetDelYdRateCs")]
        public APIRst GetDelYdRateCs(int Rate_id, int csId)
        {
            return infoHelper.GetDelYdRateCs(Rate_id, csId);
        }

    }
}