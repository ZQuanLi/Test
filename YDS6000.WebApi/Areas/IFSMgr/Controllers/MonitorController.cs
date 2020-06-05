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
    [RoutePrefix("Api/Devices")]
    public class MonitorController : ApiController
    {
        private YDS6000.WebApi.Areas.IFSMgr.Opertion.Monitor.MonitorHelper infoHelper = new YDS6000.WebApi.Areas.IFSMgr.Opertion.Monitor.MonitorHelper();

        /// <summary>
        /// 获取实时数据
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpGet,HttpOptions]
        [SecurityCtrl(false, false)]
        [Route("GetRealVal")]
        public APIRst GetRealVal(string tag)
        {
            return infoHelper.GetRealVal(tag);
        }
        
        /// <summary>
        /// 批量获取实时数据
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [SecurityCtrl(false, false)]
        [Route("GetRealVal")]
        public APIRst GetBatchRealVal(Tags tags)
        {
            return infoHelper.GetBatchRealVal(tags.list);
        }

        /// <summary>
        /// 测试POST跨域
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [SecurityCtrl(false, false)]
        [Route("GetTest")]
        public APIRst GetTest(Tags obj)
        {
            //List<string> tag = new List<string>();
            CacheUser user = WebConfig.GetSession();
            string key = (user == null || user.Uid == 0) ? "" : user.CacheKey;

            if (string.IsNullOrEmpty(key))
                key = WebConfig.MemcachKey;
            APIRst rst = new APIRst() { rst = true, data = new { key = key, tag = JsonHelper.Serialize(obj.list) } };
            return rst;
        }

        /// <summary>
        /// 获取采集数据
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [SecurityCtrl(false, false)]
        [Route("ResultNotify")]
        public APIRst ResultNotify()
        {
            return infoHelper.ResultNotify();
        }
    }
}
