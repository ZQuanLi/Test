using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Platform.Controllers
{
    /// <summary>
    /// 平台用户管理
    /// </summary>
    [RoutePrefix("api/Platform/User")]
    public class YdUserController : ApiController
    {
        private UserHelper infoHelper = new UserHelper();

        #region 角色
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetRoleList")]
        public APIRst GetRoleList()
        {
            return infoHelper.GetRoleList();
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("AddRole")]
        public APIRst AddRole(sys_role role)
        {
            role.Role_id = 0;
            return infoHelper.SetRole(role);
        }
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetRole")]
        public APIRst SetRole(sys_role role)
        {
            return infoHelper.SetRole(role);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色ID号</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("DelRole")]
        public APIRst DelRole(int id)
        {
            return infoHelper.DelRole(id);
        }

        /// <summary>
        /// 获取角色权限列表
        /// </summary>
        /// <param name="id">角色ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMenuOnOperateList")]
        public APIRst GetMenuOnOperateList(int id)
        {
            return infoHelper.GetMenuOnOperateList(id);
        }

        /// <summary>
        /// 保存角色权限列表
        /// </summary>
        /// <param name="id">角色ID号</param>
        /// <param name="ids">选中的权限ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SaveMenuOnOperateList")]
        public APIRst SaveMenuOnOperateList(int id,string ids)
        {
            return infoHelper.SaveMenuOnOperateList(id, ids);
        }
        #endregion

        #region 用户
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetUserList")]
        public APIRst GetUserList()
        {
            return infoHelper.GetUserList();
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("AddUser")]
        public APIRst AddUser(sys_user user)
        {
            user.Uid = 0;
            return infoHelper.SetUser(user);
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetUser")]
        public APIRst SetUser(sys_user user)
        {
            return infoHelper.SetUser(user);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID号</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("DelUser")]
        public APIRst DelUser(int id)
        {
            return infoHelper.DelUser(id);
        }
        #endregion

    }
}
