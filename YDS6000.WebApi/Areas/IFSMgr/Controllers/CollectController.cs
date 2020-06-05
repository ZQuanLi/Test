using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.IFSMgr.Controllers
{
    /// <summary>
    /// 第三方接口API
    /// </summary>
    [RoutePrefix("Api/Monitor")]
    public class CollectController : ApiController
    {
        private YDS6000.WebApi.Areas.IFSMgr.Opertion.Collect.CollectHelper infoHelper = new YDS6000.WebApi.Areas.IFSMgr.Opertion.Collect.CollectHelper();
        /// <summary>
        /// 获取采集数据
        /// </summary>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [SecurityCtrl(false, false)]
        [Route("CollectData")]
        public APIRst CollectData(UploadData dataValue)
        {
            //FileLog.WriteLog(JsonHelper.Serialize(dataValue));
            return infoHelper.CollectData(dataValue.data);
        }

        public class UploadData
        {
            public int code { get; set; }
            public string data { get; set; }
        }


    }
}
