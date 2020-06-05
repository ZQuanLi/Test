using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.SystemMgr.Controllers
{
    /// <summary>
    /// 系统菜单信息
    /// </summary>
    [AuthorizeAttribute]
    [RoutePrefix("api/Menu")]
    public class MenuController : ApiController
    {
        private MenuInfoHelper infoHelper = new MenuInfoHelper();

        /// <summary>
        /// 获取菜单树形列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMenuList")]
        public APIResult GetMenuList()
        {
            return infoHelper.GetMenuList();
        }
    }

}
