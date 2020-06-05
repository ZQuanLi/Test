using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Platform.User
{
    partial class UserBLL
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public DataTable GetUserList(int uid = 0)
        {
            return dal.GetUserList(uid);
        }

        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="user">用户信息</param>
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
        public int DelUser(int uid)
        {
            return dal.DelUser(uid);
        }
    }
}
