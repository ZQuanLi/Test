using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.SystemMgr.Controllers
{
    /// <summary>
    /// 用户操作Api类
    /// </summary>
    [AuthorizeAttribute]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private UserHelper userHelper = new UserHelper();
        /// <summary>
        /// 获取用户登陆用户信息
        /// </summary>
        /// <param name="usign">登陆用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Login")]
        public APIResult Login(string usign,string pwd)
        {
            return userHelper.Login(usign, pwd);
        }

        #region 角色
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="id">id号 =0全部</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRoleList")]
        public APIResult GetRoleList(int id)
        {
            return userHelper.GetRoleList(id);
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetRole")]
        public APIResult SetRole(sys_role role)
        {
            return userHelper.SetRole(role);
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddRole")]
        public APIResult AddRole(sys_role role)
        {
            role.Role_id = 0;
            return userHelper.SetRole(role);
        }


        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色ID号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DelRole")]
        public APIResult DelRole(int id)
        {
            return userHelper.DelRole(id);
        }
        #endregion

        #region 平台用户
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="id">id号 =0全部</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserList")]
        public APIResult GetUserList(int id)
        {
            return userHelper.GetUserList(id);
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetUser")]
        public APIResult SetUser(sys_user user)
        {
            return userHelper.SetUser(user);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddUser")]
        public APIResult AddUser(sys_user user)
        {
            user.Uid = 0;
            return userHelper.SetUser(user);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DelUser")]
        public APIResult DelUser(int id)
        {
            return userHelper.DelUser(id);
        }
        #endregion
    }
}
