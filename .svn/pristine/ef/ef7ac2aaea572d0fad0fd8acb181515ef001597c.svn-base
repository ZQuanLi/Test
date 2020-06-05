using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YDS6000.Models.Tables;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.ExpAdminApp.Controllers
{
    /// <summary>
    /// 首页_管理端App
    /// </summary>
    [RoutePrefix("api/ExpAdminApp/Home")]
    public class ExpAdminHomeController : ApiController
    {
        private YDS6000.WebApi.Areas.ExpAdminApp.Opertion.Home.ExpAdminHomeHelper infoHelper = new YDS6000.WebApi.Areas.ExpAdminApp.Opertion.Home.ExpAdminHomeHelper();

        /// <summary>
        /// 登录用户_平台
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [SecurityCtrl(false, false)]
        [Route("Login")]
        public APIRst Login(HomeLogin obj)
        {
            return infoHelper.Login(CommFunc.ConvertDBNullToInt32(obj.ledger), CommFunc.ConvertDBNullToString(obj.name), CommFunc.ConvertDBNullToString(obj.pwd));
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetUser")]
        public APIRst GetUser()
        {
            return infoHelper.GetUser();
        }

        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="Name">用户名称</param>
        /// <param name="Pwd">登录密码</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetUser")]
        public APIRst SetUser(string Name, string Pwd)
        {
            return infoHelper.SetUser( Name, Pwd);
        }

        /// <summary>
        /// 获取运行历史报表
        /// </summary>
        /// <param name="StrcName">建筑名称</param>
        /// <param name="CoName">房间名称</param>
        /// <param name="DateType">报表类型</param>
        /// <param name="StartTime">日期开始</param>
        /// <param name="EndTime">结束开始</param>
        /// <param name="Co_id">默认0</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdMontionOnList")]
        public APIRst GetYdMontionOnList(string StrcName, string CoName, string DateType, DateTime StartTime, DateTime EndTime, int Co_id = 0)
        {
            return infoHelper.GetYdMontionOnList(StrcName, CoName, DateType, StartTime, EndTime, Co_id);
        }

        /// <summary>
        /// 获取首页当月缴费和笔数
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetHomePayMonth")]
        public APIRst GetHomePayMonth()
        {
            return infoHelper.GetHomePay(2); // 2 = 查询当月
        }

        /// <summary>
        /// 获取首页建筑数和房间(总用户数量取数)
        /// </summary>
        /// <param name="Attrib">// 0=空,100=建筑,9000=房间</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetHomeBuilding")]
        public APIRst GetHomeBuilding(int Attrib)
        {
            return infoHelper.GetHomeBuilding(Attrib);
        }

        /// <summary>
        /// 获取首页设备数(仪表数)
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetHomeModule")]
        public APIRst GetHomeModule()
        {
            return infoHelper.GetHomeModule();
        }


    }
}