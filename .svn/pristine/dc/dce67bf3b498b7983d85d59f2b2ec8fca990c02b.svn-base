using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.PDU.Controllers
{
    /// <summary>
    /// 首页Api
    /// </summary>
    [RoutePrefix("api/Pdu/Home")]
    public class PduHomeController : ApiController
    {
        private YDS6000.WebApi.Areas.PDU.Opertion.Home.HomeHelper infoHelper = new YDS6000.WebApi.Areas.PDU.Opertion.Home.HomeHelper();

        /// <summary>
        /// 获取PDU列表
        /// </summary>     
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetPduList")]
        public APIRst GetPduList()
        {
            return infoHelper.GetPduList();
        }
        /// <summary>
        /// 获取PDU运行状态、环境监测信息
        /// </summary>
        /// <param name="id">Pdu ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetPduStatus")]
        public APIRst GetPduStatus(int id)
        {
            return infoHelper.GetPduStatus(id);
        }


        /// <summary>
        /// 获取PDU采集点曲线信息
        /// </summary>
        /// <param name="id">Pdu ID号</param>
        /// <param name="funTypes">采集点 Ua,Ub,Uc电压;Ia,Ib,Ic电流;Pa,Pb,Pc功率;PFa,PFb,PFc功率因素</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetPduFunTypes")]
        public APIRst GetPduFunTypes(int id, string funTypes)
        {
            return infoHelper.GetPduFunTypes(id, funTypes);
        }

        /// <summary>
        /// 获取PDU电能信息
        /// </summary>
        /// <param name="id">Pdu ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetPduEnergy")]
        [Obsolete("放弃使用",true)]
        public APIRst GetPduEnergy(int id)
        {
            return infoHelper.GetPduEnergy(id);
        }

        /// <summary>
        /// 获取全部告警数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetPduAlarm")]
        public APIRst GetPduAlarm(int id = 0)
        {
            return infoHelper.GetPduAlarm(id);
        }

        #region 修改HOME 页面API

        /// <summary>
        /// 获取PDU列表
        /// </summary>     
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetPduMap")]
        public APIRst GetPduMap()
        {
            return infoHelper.GetPduMap();
        }

        /// <summary>
        /// 获取PDU列表
        /// </summary>     
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetPduTree")]
        public APIRst GetPduTree()
        {
            return infoHelper.GetPduTree();
        }
        #region

        /// <summary>
        /// 获取PDU列表
        /// </summary>     
        /// <param name="id">PDU的ID号</param>
        /// <returns></returns>       
        [HttpGet, HttpOptions]
        [Route("GetPduTree200")]
        public APIRst GetPduTree200(int id=0)
        {
            return infoHelper.GetPduTree200(id);
        }
        #endregion
        /// <summary>
        /// 获取PDU列表
        /// </summary>     
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetPduGeneral")]
        public APIRst GetPduGeneral(int id = 0)
        {
            return infoHelper.GetPduGeneral(id);
        }

        /// <summary>
        /// 获取PDU能耗分布
        /// </summary>     
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetPduEnergyPie")]
        public APIRst GetPduEnergyPie(int id = 0)
        {
            return infoHelper.GetPduEnergyPie(id);
        }
        #endregion
    }
}
