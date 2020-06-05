using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.PDU.Controllers
{
    /// <summary>
    /// 设备管理
    /// </summary>
    [RoutePrefix("api/Pdu/Mgr")]
    public class PduMgrController : ApiController
    {
        private YDS6000.WebApi.Areas.PDU.Opertion.Mgr.MgrHelper infoHelper = new YDS6000.WebApi.Areas.PDU.Opertion.Mgr.MgrHelper();

        /// <summary>
        /// PDU运行状态
        /// </summary>     
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMgrStatus")]
        public APIRst GetMgrStatus()
        {
            return infoHelper.GetMgrStatus();
        }

        /// <summary>
        /// PDU在线控制
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMgrCtrl")]
        public APIRst GetMgrCtrl()
        {
            return infoHelper.GetMgrCtrl("");
        }

        /// <summary>
        /// 拉闸--关闭
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [SecurityCtrl("拉闸")]
        [Route("Closed")]
        public APIRst Closed(Tags tags)
        {
            return new YDS6000.WebApi.Areas.IFSMgr.Opertion.Monitor.MonitorHelper().SendVal(tags.list, "1");
        }

        /// <summary>
        /// 合闸--开启
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [SecurityCtrl("合闸")]
        [Route("Open")]
        public APIRst Open(Tags tags)
        {
            return new YDS6000.WebApi.Areas.IFSMgr.Opertion.Monitor.MonitorHelper().SendVal(tags.list, "0");
        }



        /// <summary>
        /// PDU在线限制设置
        /// </summary>
        /// <param name="moduleName">插座名称</param>       
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMgrLimit")]
        public APIRst GetMgrLimit(string moduleName = "")
        {
            return infoHelper.GetMgrLimit(moduleName);
        }


        /// <summary>
        /// 设置采集点值
        /// </summary>
        /// <param name="tag">采集点</param>
        /// <param name="dataValue">设置值</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [SecurityCtrl("设置采集点值")]
        [Route("SetMgrLimit")]
        public APIRst SetMgrLimit(string tag,string dataValue)
        {
            List<string> tags = new List<string>();
            tags.Add(tag);
            return new YDS6000.WebApi.Areas.IFSMgr.Opertion.Monitor.MonitorHelper().SendVal(tags, dataValue);
        }

        /// <summary>
        /// 获取控制状态信息是否成功
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [SecurityCtrl("控制状态信息是否成功")]
        [Route("GetSuccess")]
        [Obsolete("放弃使用",true)]
        public APIRst GetSuccess(Tags tags)
        {
            return new YDS6000.WebApi.Areas.IFSMgr.Opertion.Monitor.MonitorHelper().GetSuccess(tags.list);
        }
    }
}
