using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;
using YDS6000.DAL.User;

namespace YDS6000.BLL.User
{
    public partial class UserBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;

        private readonly UserDAL dal = null;
        public UserBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new UserDAL(_ledger, _uid);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="USign"></param>
        /// <returns></returns>
        public DataTable GetSysUser(string usign)
        {
            return dal.GetSysUser(usign);
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="role_id">角色ID号</param>
        /// <returns></returns>
        public DataTable GetRoleList(int role_id)
        {
            return dal.GetRoleList(role_id);
        }

        /// <summary>
        /// 设置角色信息
        /// </summary>
        /// <param name="sys_role">角色类型</param>
        /// <returns></returns>
        public int SetRole(sys_role role)
        {
            return dal.SetRole(role);
        }

        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="role_id">角色ID号</param>
        /// <returns></returns>
        public int DelRole(int role_id)
        {
            return dal.DelRole(role_id);
        }
    }
}
