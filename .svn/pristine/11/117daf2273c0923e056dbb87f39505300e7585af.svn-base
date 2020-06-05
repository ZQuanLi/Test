using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YDS6000.Models;


namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 系统配置-用户管理
    /// </summary>
    [RoutePrefix("api/Exp/User")]
    public class ExpUserController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.User.ExpUserHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.User.ExpUserHelper();
        private YDS6000.WebApi.Areas.Exp.Opertion.User.ExpRoleHelper roleHelper = new YDS6000.WebApi.Areas.Exp.Opertion.User.ExpRoleHelper();

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
        /// <param name="Uid">用户ID</param>
        /// <param name="sAct">操作类型：增加=1，修改=2，删除=3</param>
        /// <param name="Rid">用户权限</param>
        /// <param name="Name">用户名称</param>
        /// <param name="SName">登录名</param>
        /// <param name="Pwd">登录密码</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetUser")]
        public APIRst SetUser(int Uid, int sAct, int Rid, string Name, string SName, string Pwd, string TelNo)
        {
            return infoHelper.SetUser(Uid, sAct, Rid, Name, SName, Pwd, TelNo);
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="id">权限ID</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetRole")]
        public APIRst GetRole()
        {
            return roleHelper.GetRole();
        }

        /// <summary>
        /// 设置角色信息
        /// </summary>
        /// <param name="Role_id">权限ID</param>
        /// <param name="sAct">操作类型：增加=1，修改=2，删除=3</param>
        /// <param name="role_idS">角色名称</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetRole")]
        public APIRst SetRole(int Role_id, int sAct, string role_idS)
        {
            return roleHelper.SetRole(Role_id, sAct, role_idS);
        }

        /// <summary>
        /// 获取基本权限信息
        /// </summary>
        /// <param name="id">权限ID</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetPowerById")]
        public APIRst GetPowerById(int id)
        {
            return roleHelper.GetPowerById(id);
        }

        /// <summary>
        /// 设置基本权限
        /// </summary>
        /// <param name="pRole_id">权限ID</param>
        /// <param name="ids">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetPower")]
        public APIRst SetPower(int pRole_id, string ids)
        {
            return roleHelper.SetPower(pRole_id, ids);
        }

        /// <summary>
        /// 获取区域权限信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAreaById")]
        public APIRst GetAreaById(int id)
        {
            return roleHelper.GetAreaById(id);
        }

        /// <summary>
        /// 设置区域权限信息
        /// </summary>
        /// <param name="pRole_id">权限ID</param>
        /// <param name="ids">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetPowerArea")]
        public APIRst SetPowerArea(int pRole_id, string ids)
        {
            return roleHelper.SetPowerArea(pRole_id, ids);
        }


    }
}