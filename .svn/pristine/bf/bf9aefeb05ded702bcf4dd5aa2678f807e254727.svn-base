using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Platform.Home
{
    public class HomeDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public HomeDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSys_user(string uSign)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT a.Ledger,a.Uid,a.USign,GetDECODE(a.UPasswd,@EncryptPwd) UPasswd ,a.UName,a.Role_id,a.Disabled,a.LoginDate,a.LoginInt,m.Project");
            strSql.Append(" FROM sys_user as a inner join syscont as m on a.Ledger=m.Ledger LEFT JOIN sys_role as b on a.Ledger=b.Ledger and a.Role_id=b.Role_id");
            strSql.Append(" WHERE a.Ledger=@Ledger and a.USign=@USign ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger , USign = uSign, EncryptPwd = WHoleDAL.EncryptPwd});
        }

        public int UpdateLogin(string uSign, int num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update sys_user set LoginDate=NOW(),LoginInt=case when @Num>0 then LoginInt+1 else 0 end  WHERE Ledger=@Ledger and USign=@USign");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, USign= uSign, Num = num });
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="role_id"></param>
        /// <param name="prog_id"></param>
        /// <returns></returns>
        public bool GetPower(int role_id, string prog_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(a.Ledger) as cnt");
            strSql.Append(" from sys_user_prog as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Role_id=@Role_id and a.Prog_id=@Prog_id");
            strSql.Append(" and a._read + a._write + a._delete + a._app > 0 ");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Role_id = role_id, Prog_id = prog_id });
            return CommFunc.ConvertDBNullToInt32(obj) > 0 ? true : false;
        }

        /// <summary>
        /// 获取子系统信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSubSystem()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select ProId,Project,ProName,Parent_id,Path");
            strSql.Append(" from sys_pro");
            strSql.Append(" where Disabled=0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="project">项目名称</param>
        /// <returns></returns>
        public DataTable GetMenuList(string project)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select Role_id from sys_user where Ledger=@Ledger and Uid=@Uid");
            int role_id = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Uid = this.SysUid }));
            //
            strSql.Clear();
            strSql.Append("select UserType from sys_role where Ledger=@Ledger and Role_id=@Role_id");
            int userType = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Role_id = role_id }));
            //
            strSql.Clear();
            strSql.Append("select a.menu_no,a.descr,trim(ifnull(a.parent_no,''))as parent_no,a.prog_id,a.path,a.ordno,a.attrib,a.icon,");
            strSql.Append("ifnull(b._delete, 0) + ifnull(b._write, 0) + ifnull(b._app, 0) + ifnull(b._read,0) as pass,");
            strSql.Append("CONCAT('d:',ifnull(b._delete,0),',w:',ifnull(b._write,0),',r:',case when ifnull(b._delete,0)+ifnull(b._write,0)+ifnull(b._app,0)>0 then 1 else ifnull(b._read,0) end) as power");
            strSql.Append(" from sys_menu as a");
            //if (userType == 1)
            //    strSql.Append(" left join sys_user_prog as b on b.Ledger=@Ledger and b.Role_id=@Role_id and a.prog_id=b.prog_id");
            //else
            strSql.Append(" left join sys_user_prog as b on b.Ledger=@Ledger and b.Role_id=@Role_id and a.prog_id=b.prog_id");
            strSql.Append(" where a.ledger=@Ledger and ifnull(a.disabled,0)=0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, project = project, Role_id = role_id, UserType = userType });
        }
    }
}
