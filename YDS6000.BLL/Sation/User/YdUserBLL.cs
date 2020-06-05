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
    partial class UserBLL
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="uid">用户ID号</param>
        /// <returns></returns>
        public DataTable GetUserList(int uid)
        {
            return dal.GetUserList(uid);
        }

        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="sys_role">用户信息</param>
        /// <returns></returns>
        public int SetUser(sys_user user)
        {
            return dal.SetUser(user);
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="Uid">用户ID号</param>
        /// <returns></returns>
        public int DelUser(int Uid)
        {
            return dal.DelUser(Uid);
        }
    }
}
