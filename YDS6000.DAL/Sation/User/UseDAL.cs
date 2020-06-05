using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.User
{
    public partial class UserDAL
    {
        private static string EncryptPwd = "DiLidili";
        private int Ledger = 0;
        private int SysUid = 1;
        public UserDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="USign"></param>
        /// <returns></returns>
        public DataTable GetSysUser(string usign)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Uid,a.USign,cast(DECODE(a.UPasswd,@EncryptPwd) as char) UPasswd,a.UName,a.Role_id,a.Disabled");
            strSql.Append(" from sys_user a left join sys_role b on a.Ledger = b.Ledger and a.Role_id = b.Role_id");
            strSql.Append(" where a.Ledger = @Ledger and a.USign=@USign");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, USign = usign, EncryptPwd = EncryptPwd });
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="role_id">角色ID号</param>
        /// <returns></returns>
        public DataTable GetRoleList(int role_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Role_id,Descr,UserType,Update_by,Update_dt,Remark from sys_role ");
            strSql.Append(" where Ledger=@Ledger");
            if (role_id != 0)
                strSql.Append(" and Role_id=@Role_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Role_id = role_id });
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
            if (CommFunc.ConvertDBNullToInt32(obj)>0)
                throw new Exception("系统角色不能删除!");
            strSql.Clear();
            strSql.Append("delete from sys_role where Ledger=@Ledger and Role_id=@Role_id;");
            strSql.Append("delete from sys_user_prog where Ledger=@Ledger and Role_Id=@Role_id;");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Role_id = role_id });
        }
    }
}
