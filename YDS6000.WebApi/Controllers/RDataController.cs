using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YDS6000.Models;

namespace YDS6000.WebApi.Controllers
{
    /// <summary>
    /// 获取实时数据接口
    /// </summary>
    [AuthorizeAttribute]
    [RoutePrefix("api/RData")]
    public class RDataController : ApiController
    {
        /// <summary>
        /// 获取采集值
        /// </summary>
        /// <param name="var">变量集合</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCollectValue")]        
        public APIResult GetCollectValue(List<ApiVar> var)
        {
            APIResult rst = new APIResult();
            bool cc = CacheMgr.SendCollectVal(var);
            rst.Code = cc == true ? 0 : -1;
            rst.Msg = cc == true ? "成功" : "失败";
            rst.Data = "";
            return rst;
        }
    }
}
