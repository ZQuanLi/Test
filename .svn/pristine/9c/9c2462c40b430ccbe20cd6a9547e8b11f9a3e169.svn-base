using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Platform.User
{
    partial class UserDAL
    {
        public DataTable GetRoleList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT a.Role_id,a.Descr,a.Remark");
            strSql.Append(" FROM sys_role as a");
            strSql.Append(" WHERE a.Ledger=@ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 设置角色信息
        /// </summary>
        /// <param name="sys_role">角色类型</param>
        /// <returns></returns>
        public int SetRole(sys_role role)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(*)as cnt from sys_role where Ledger=@Ledger and Role_id!=@Role_id and Descr=@Descr");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Role_id = role.Role_id, Descr = role.Descr });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("角色:" + role.Descr + "已存在");
            if (role.Role_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Role_id)as Role_id from sys_role where Ledger=@Ledger");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger });
                role.Role_id = CommFunc.ConvertDBNullToInt32(obj) + 1;
                //////////
                strSql.Clear();
                strSql.Append("insert sys_role(Ledger,Role_id,Descr,Update_by,Update_dt,Remark)values(@Ledger,@Role_id,@Descr,@Update_by,now(),@Remark)");
                return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Role_id = role.Role_id, Descr = role.Descr, Update_by = this.SysUid, Remark = role.Remark });
            }
            else
            {
                strSql.Clear();
                strSql.Append("update sys_role set Descr=@Descr,Remark=@Remark,Update_by=@Update_by,Update_dt=now() where Ledger=@Ledger and Role_id=@Role_id");
                return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Role_id = role.Role_id, Descr = role.Descr, Update_by = this.SysUid, Remark = role.Remark });
            }
        }

        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="role_id">角色ID号</param>
        /// <returns></returns>
        public int DelRole(int role_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(*)as cnt from sys_role where  Ledger=@Ledger and Role_id=@Role_id and UserType=1");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Role_id = role_id });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("系统角色不能删除!");
            strSql.Clear();
            strSql.Append("delete from sys_role where Ledger=@Ledger and Role_id=@Role_id;");
            strSql.Append("delete from sys_user_prog where Ledger=@Ledger and Role_id=@Role_id;");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Role_id = role_id });
        }

        public DataTable GetMenuOnOperateList(int role_id,string project)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.menu_no,a.descr,ifnull(a.parent_no,'')as parent_no,a.ordno,a.prog_id,b._read as power");
            strSql.Append(" from sys_menu as a left join sys_user_prog as b on b.Ledger=@Ledger and a.prog_id=b.prog_id and b.Role_id=@Role_id");
            strSql.Append(" where a.project=@project");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Role_id = role_id, project= project });
        }

        public int SaveMenuOnOperateList(int role_id, string ids)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("delete from sys_user_prog where Ledger=@Ledger and Role_id=@Role_id;");
            foreach(string prog_id in ids.Split(new char[] { ','},StringSplitOptions.RemoveEmptyEntries))
            {
                strSql.Append("insert into sys_user_prog(Ledger,Role_id,prog_id,_read,_write,_delete,_app)values");
                strSql.Append("(@Ledger,@Role_id,'" + prog_id + "',1,1,1,1);");
            }
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Role_id = role_id });
        }
    }
}
