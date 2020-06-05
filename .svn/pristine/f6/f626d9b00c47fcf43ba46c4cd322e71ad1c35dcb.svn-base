using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.ExpApp.Controllers
{
    /// <summary>
    /// 手机用电APP
    /// </summary>
    [RoutePrefix("api/ExpApp/SysMgr")]
    public class ExpAppSysMgrController : ApiController
    {

        private YDS6000.WebApi.Areas.ExpApp.Opertion.SysMgr.SysMgrHelper infoHelper = new YDS6000.WebApi.Areas.ExpApp.Opertion.SysMgr.SysMgrHelper();

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param oldPwd="旧密码"></param>
        /// <param newPwd="新密码"></param>
        /// <param confirmPwd="确认密码"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SetPasspwd")]
        public APIRst SetPasspwd(string oldPwd,string newPwd,string confirmPwd)
        {
            return infoHelper.SetPasspwd(oldPwd, newPwd, confirmPwd);
        }
        /// <summary>
        /// 账号管理
        /// </summary>
        /// <param name=""></param>
        /// <param co_id="房间id"></param>
        /// <param pwd="密码"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("MgrUpdate")]
        public APIRst MgrUpdate(string name, int co_id, string pwd)
        {
            return infoHelper.MgrUpdate(name, co_id, pwd);
        }
        /// <summary>
        /// 账号管理（修改密码）
        /// </summary>
        /// <param roomName="房间名称"></param>
        /// <param newPwd="新密码"></param>
        /// <param confirmPwd="确认密码"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetRoomList")]
        public APIRst GetRoomList(string roomName)
        {
            return infoHelper.GetRoomList(roomName);
        }

        /// <summary>
        /// 快速注册信息
        /// </summary>
        /// <param name="name">登陆名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [SecurityCtrl(false, false)]
        [Route("Register")]
        public APIRst Register(string name,string pwd)
        {
            return infoHelper.Register(name, pwd);
        }
    }
}
