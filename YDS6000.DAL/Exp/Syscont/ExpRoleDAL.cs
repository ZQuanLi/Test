using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Exp.Syscont
{
    public partial class ExpRoleDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public ExpRoleDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetRole(int Role_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT Role_id,Descr,UserType FROM sys_role ");
            strSql.Append(" where Ledger=@Ledger");
            if (Role_id != 0)
                strSql.Append(" and Role_id=@Role_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Role_id = Role_id });
        }

        /// <summary>
        /// 硬删除角色信息
        /// </summary>
        /// <param name="Uid"></param>
        public void DelRole(int Role_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT UserType FROM sys_role WHERE Ledger=@Ledger and Role_id=@Role_id and UserType=0;");
            DataTable obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Role_id = Role_id });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["UserType"]) > 0)
            {
                throw new Exception("此用户为厂家角色，不可删除");
            }
            strSql.Clear();
            strSql.Append(" delete from sys_role  where Ledger=@Ledger");
            if (Role_id != 0)
                strSql.Append(" and Role_id=@Role_id");
            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Role_id = Role_id });
        }

        /// <summary>
        /// 新增角色信息
        /// </summary>
        /// <param name="Uid"></param>
        public void AddRole(sys_role sys_role)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append(" select Role_id from sys_role where Ledger=@Ledger and Descr=@Descr;");
            DataTable obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Descr = sys_role.Descr });
            if (obj.Rows.Count > 0)
            {
                throw new Exception("角色名称:" + sys_role.Descr + "已经存在。");
            }
            strSql.Clear();
            strSql.Append(" select max(Role_id) from sys_role where Ledger=@Ledger;");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
            sys_role.Role_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["max(Role_id)"]) + 1 : 1;
            //sys_role.Role_id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["max(Role_id)"]) + 1;
            //
            strSql.Clear();
            strSql.Append(" INSERT INTO sys_role ( Ledger ,Role_id ,Descr ,Update_by ,Update_dt ) ");
            strSql.Append(" VALUES (@Ledger,@Role_id,@Descr,@Update_by ,@Update_dt) ");
            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Role_id = sys_role.Role_id, Descr = sys_role.Descr, Update_by = this.SysUid, Update_dt = DateTime.Now });
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="Uid"></param>
        public void EditRole(sys_role sys_role)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append(" select Role_id from sys_role where Ledger=@Ledger and Descr=@Descr;");
            DataTable obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Descr = sys_role.Descr });
            if (obj.Rows.Count > 0)
            {
                throw new Exception("角色名称:" + sys_role.Descr + "已经存在。");
            }
            strSql.Clear();
            strSql.Append(" update sys_role set Descr=@Descr ,");
            strSql.Append( "Update_by=@Update_by,Update_dt=NOW() where Ledger=@Ledger and Role_id=@Role_id");
            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Descr = sys_role.Descr, Update_by = this.SysUid, Role_id = sys_role.Role_id });
        }

        /// <summary>
        /// 获取基本权限
        /// </summary>
        /// <returns></returns>
        public DataTable GetPowerById(int Role_Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.menu_no,a.descr,a.parent_no,a.prog_id,case when b._read + b._write + b._delete + b._app > 0 then 1 else 0 end as power");
            strSql.Append(" from sys_menu a left join sys_user_prog as b on a.Ledger=b.ledger and b.Role_Id=@Role_Id and a.prog_id=b.prog_id ");
            strSql.Append(" where a.Ledger=@Ledger and ifnull(a.disabled,0)=0;");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Role_Id = Role_Id });
        }

        /// <summary>
        /// 设置基本权限
        /// </summary>
        /// <param name="pRole_id">权限ID</param>
        /// <param name="ids">选中的ID号（多个用逗号拼接）</param>
        public int SetPower(int Role_id, string ids)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            //strSql.Append("delete a from sys_user_prog as a ,sys_menu as b where a.Ledger=@Ledger and b.project=@Project and a.prog_id=b.prog_id and a.Role_Id=@role_id;");
            //foreach (string prog_id in ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            //{
            //    strSql.Append("update sys_menu set disabled=0 where ledger=@Ledger and menu_no='" + prog_id + "' ;");
            //    strSql.Append("insert into sys_user_prog(Ledger,Role_Id,prog_id,_read,_write,_delete,_app)values");
            //    strSql.Append("(@Ledger,@role_id,'" + prog_id + "',1,1,1,1);");
            //}
            foreach (string prog_id in ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {//先清空一遍,根据选择的权限ID
                strSql.Append("delete from sys_user_prog where Ledger=@Ledger and Role_id=@Role_id and prog_id=@prog_id;");                
                SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Role_id = Role_id, prog_id = prog_id });
            }
            strSql.Clear();
            foreach (string prog_id in ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {//再进行添加权限操作
                strSql.Append("insert into sys_user_prog(Ledger,Role_id,prog_id,_read,_write,_delete,_app)values");
                strSql.Append("(@Ledger,@Role_id,'" + prog_id + "',1,1,1,1);");
            }
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Role_id = Role_id });
        }
        /// <summary>
        /// 获取基本权限
        /// </summary>
        /// <returns></returns>
        public DataTable GetAreaById2()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT Ledger,Co_id,CoNo,CoName,Parent_id,Create_by,Create_dt,Update_by,Update_dt");
            strSql.Append(" FROM v1_cust ");
            strSql.Append(" where Ledger=@Ledger;");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 获取基本权限
        /// </summary>
        /// <returns></returns>
        public DataTable GetAreaById(int Role_Id, string Project)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            //strSql.Append("SELECT Ledger,Co_id,CoNo,CoName,Parent_id,Create_by,Create_dt,Update_by,Update_dt");
            //strSql.Append(" FROM v1_cust ");
            //strSql.Append(" where Ledger=@Ledger;");
            strSql.Append("SELECT a._area  FROM sys_user_prog as a inner join syscont as b on a.Ledger=b.Ledger and a.prog_id=b.Project where a.Ledger=@Ledger and Role_Id=@Role_Id");// and prog_id=@Project;");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Role_Id = Role_Id, Project = Project });
        }

        /// <summary>
        /// 设置区域权限
        /// </summary>
        /// <param name="pRole_id">权限ID</param>
        /// <param name="ids">选中的ID号（多个用逗号拼接）</param>
        public int SetPowerArea(int Role_id, string ids, string Project)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            //strSql.Append("delete from sys_user_prog where Ledger=@Ledger and Role_Id=@role_id and prog_id=@Project;");
            //foreach (string prog_id in ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            //{
            //    strSql.Append("insert into sys_user_prog(Ledger,Role_id,prog_id,_read,_write,_delete,_app)values");
            //    strSql.Append("(@Ledger,@Role_id,'" + prog_id + "',1,1,1,1);");
            //}
            //strSql.Append(" insert into sys_user_prog(Ledger,Role_Id,prog_id,_read,_write,_delete,_app,_area)values(@Ledger,@Role_id,@Project,1,1,1,1,@area);");
            //因为where条件,清除的是对应的数据
            strSql.Append("delete a from sys_user_prog as a inner join syscont as b on a.Ledger=b.Ledger and a.prog_id=b.Project where a.Ledger=@Ledger and a.Role_Id=@role_id;");
            strSql.Append(" insert into sys_user_prog(Ledger,Role_Id,prog_id,_read,_write,_delete,_app,_area)select @Ledger,@Role_id,Project,1,1,1,1,@area from syscont where Ledger=@Ledger");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Role_id = Role_id, Project= Project, area = ids });
        }


    }
}
