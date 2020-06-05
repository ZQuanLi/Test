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
    public partial class ExpUserDAL
    {
        private static string EncryptPwd = "DiLidili";
        private int Ledger = 0;
        private int SysUid = 1;
        public ExpUserDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetUser(int Uid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Role_id,a.Create_by,a.Create_dt,a.Ledger,a.Uid,a.UName,cast( GetDECODE(a.UPasswd,'" + EncryptPwd + "') as char) UPasswd,");
            strSql.Append("a.USign,IFNULL(c1.UName,'') Create_byName ,b.Descr,c.TelNo");
            strSql.Append(" from sys_user a left JOIN sys_role  b on a.Ledger=b.Ledger and a.Role_id=b.Role_id");
            strSql.Append(" left join sys_userinfo as c on a.Ledger=c.Ledger and a.Uid=c.Uid");
            strSql.Append(" left join sys_user as c1 on a.Ledger=c1.Ledger and a.Create_by=c1.Uid");
            strSql.Append(" where a.Ledger=@Ledger  and a.Disabled=0 ");
            //strSql.Append("select a.Ledger,a.Uid,a.USign,a.UPasswd,a.UName,a.Role_id,a.Disabled,a.Create_by,a.Create_dt,a.Update_by,a.Update_dt,b.Descr");
            //strSql.Append(" from sys_user as a");
            //strSql.Append(" left join sys_role as b on a.Role_id=b.Role_id");
            //strSql.Append(" where a.Ledger=@ledger");
            if (Uid != 0)
                strSql.Append("and a.Uid=@Uid; ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Uid = Uid });
        }
        
        /// <summary>
        /// 硬删除用户
        /// </summary>
        /// <param name="Uid"></param>
        public void DelUser(int Uid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT UserType FROM sys_user WHERE Ledger=@Ledger and Uid=@Uid and UserType=0;");
            DataTable obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Uid = Uid });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["UserType"]) > 0)
            {
                throw new Exception("此用户为厂家用户，不可删除");
            }
            strSql.Clear();
            strSql.Append(" delete  from sys_user  where Ledger=@Ledger and Uid=@Uid");
            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Uid = Uid });
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="Uid"></param>
        public void AddUser(sys_user sys_user)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select UName from sys_user where Ledger=@Ledger and USign=@USign;");
            DataTable obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, USign = sys_user.USign });
            if (obj.Rows.Count > 0)
            {
                throw new Exception("登录名称:" + sys_user.USign + "已经存在");
            }
            strSql.Clear();
            strSql.Append(" select max(Uid) from sys_user where Ledger=@Ledger;");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
            sys_user.Uid = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["max(Uid)"]) + 1 : 1;
            //sys_user.Uid = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["max(Uid)"]) + 1;
            //
            strSql.Clear();
            strSql.Append("INSERT INTO sys_user ( Ledger ,Uid ,USign ,UPasswd ,UName ,Role_id ,Disabled ,Create_by ,Create_dt ,Update_by ,Update_dt ) ");
            strSql.Append("VALUES (@Ledger,@Uid,@USign,GetENCODE(@UPasswd,'" + EncryptPwd + "'),@UName,@Role_id,@Disabled,@Create_by,@Create_dt,@Update_by,@Update_dt) ON DUPLICATE KEY UPDATE  USign =@USign,UName = @UName,Role_id=@Role_id; ");
            //
            strSql.Append("insert into sys_userinfo(Ledger,Uid,TelNo,Create_by,Create_dt,Update_by,Update_dt)values(@Ledger,@Uid,@TelNo,@Update_by,now(),@Update_by,now()) ON DUPLICATE KEY UPDATE TelNo=@TelNo,Update_by=@Update_by,Update_dt=NOW();");

            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Uid = sys_user.Uid, USign = sys_user.USign, UPasswd = sys_user.UPasswd, UName = sys_user.UName, Role_id = sys_user.Role_id, Disabled = sys_user.Disabled, TelNo = sys_user.TelNo ,Create_by = this.SysUid, Create_dt = DateTime.Now, Update_by = this.SysUid, Update_dt = DateTime.Now });
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="Uid"></param>
        public void EditUser(sys_user sys_user)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update sys_user set USign=@USign ,UPasswd=GetENCODE(@UPasswd,'" + EncryptPwd + "'),UName=@UName  ,Role_id=@Role_id ,Disabled=0,Create_by=@Create_by,");
            strSql.Append("Create_dt=NOW(),Update_by=@Update_by,Update_dt=NOW() where Ledger=@Ledger and Uid=@Uid;");
            strSql.Append("insert into sys_userinfo(Ledger,Uid,TelNo,Create_by,Create_dt,Update_by,Update_dt)values(@Ledger,@Uid,@TelNo,@Update_by,now(),@Update_by,now()) ON DUPLICATE KEY UPDATE TelNo=@TelNo,Update_by=@Update_by,Update_dt=NOW();");
            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, USign = sys_user.USign, UPasswd = sys_user.UPasswd, UName = sys_user.UName, Role_id = sys_user.Role_id, TelNo = sys_user.TelNo, Create_by = sys_user.Uid, Update_by = this.SysUid, Uid = sys_user.Uid });
        }

    }
}
