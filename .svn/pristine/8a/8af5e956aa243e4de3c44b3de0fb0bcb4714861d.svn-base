using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Syscont
{
    public partial class ExpUserBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Exp.Syscont.ExpUserDAL dal = null;
        public ExpUserBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Exp.Syscont.ExpUserDAL(_ledger, _uid);
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetUser(int uid)
        {
            DataTable dtSource = dal.GetUser(uid);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
                dr["RowId"] = ++RowId;
            return dtSource;
        }

        /// <summary>
        /// 新增修改用户
        /// </summary>
        /// <param name="sys_user"></param>
        public void EditUser(sys_user sys_user, int nAct)
        {
            if (nAct == 1)
                dal.AddUser(sys_user);
            else if (nAct == 2)
                dal.EditUser(sys_user);
            else
                throw new Exception("操作类型错误");
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sys_user"></param>
        public void DelUser(int Uid)
        {
            dal.DelUser(Uid);
        }


    }
}
