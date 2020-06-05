using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Platform.Controllers
{
    /// <summary>
    /// 平台首页Api
    /// </summary>
    [RoutePrefix("api/Platform/Home")]
    public class HomeController : ApiController
    {
        private HomeHelper infoHelper = new HomeHelper();


        /// <summary>
        /// 登录用户
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [SecurityCtrl(false,false)]
        [Route("Login")]
        public APIRst Login(HomeLogin obj)
        {
            return infoHelper.Login(CommFunc.ConvertDBNullToInt32(obj.ledger), CommFunc.ConvertDBNullToString(obj.name), CommFunc.ConvertDBNullToString(obj.pwd));
        }


        /// <summary>
        /// 获取子系统列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetSubSystem")]
        public APIRst GetSubSystem()
        {
            return infoHelper.GetSubSystem();
        }

        /// <summary>
        /// 获取菜单项
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMenuList")]
        public APIRst GetMenuList()
        {
            return infoHelper.GetMenuList();
        }
    }
}
