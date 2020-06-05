using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.ExpApp.SysMgr
{
    public class SysMgrDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        //private string Project = "";
        public SysMgrDAL(int ledger, int uid)
        {
            //this.Project = project;
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="crm_id">登陆ID号</param>
        /// <returns></returns>
        public DataTable GetV3_User(int crm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT a.CrmName,a.CrmNo,a.Crm_id,cast(GetDECODE(a.Passwd,@EncryptPwd) as char) Passwd ,a.Contract , s1.Project"); //b.Co_id,
            strSql.Append(" FROM v3_user as a");
            strSql.Append(" inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" WHERE a.Ledger=@Ledger and a.Crm_id=@Crm_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = crm_id, EncryptPwd = WHoleDAL.EncryptPwd });
        }

        /// <summary>
        /// 获取客户登陆号码是否存在
        /// </summary>
        /// <param name="crmNo">证件号（登陆号码）</param>
        /// <returns></returns>
        public bool GetExistsCrmNo(string crmNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT count(a.Ledger)as cnt");
            strSql.Append(" FROM v3_user as a");
            strSql.Append(" WHERE a.Ledger=@Ledger and a.CrmNo=@CrmNo");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, CrmNo = crmNo });
            return CommFunc.ConvertDBNullToInt32(obj) > 0 ? true : false;
        }
        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="crm_id"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool SetPasspwd(int crm_id, string pwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v3_user set Passwd=GetENCODE(@Passwd,@EncryptPwd) where Ledger=@Ledger and Crm_id=@Crm_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = crm_id, Passwd = pwd, EncryptPwd = WHoleDAL.EncryptPwd }) > 0 ? true : false;
        }
        /// <summary>
        /// 获取房间列表
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public DataTable GetRoomList(string roomName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT a.Co_id,a.CoFullName,a.CoName");
            strSql.Append(" from vp_coinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.CoFullName like @CoFullName");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, CoFullName = roomName + "%" });
        }

        /// <summary>
        /// 账号管理
        /// </summary>
        /// <param name="crmName">名称</param>
        /// <param name="co_id">房间ID</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public int MgrUpdate(string crmName, int co_id, string pwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v3_user set CrmName=@CrmName,Passwd=GetENCODE(@Passwd,@EncryptPwd),Update_by=@SysUid,Update_dt=now() where Ledger=@Ledger and Crm_id=@Crm_id;");
            strSql.Append("delete from v3_userinfo where Ledger=@Ledger and Crm_id=@Crm_id;");
            strSql.Append("insert into v3_userinfo(Ledger,Crm_id,Co_id,IsHold,Create_by,Create_dt)values(@Ledger,@Crm_id,@Co_id,0,@SysUid,now())");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = this.SysUid, Co_id = co_id, CrmName = crmName, Passwd = pwd, EncryptPwd = WHoleDAL.EncryptPwd, SysUid = 1 });
        }

        public int Register(string crmNo, string pwd)
        {
            StringBuilder strSql = new StringBuilder();
            int crm_id = 0;
            strSql.Clear();
            strSql.Append("select Crm_id as cnt from v3_user where Ledger=@Ledger and CrmNo=@CrmNo");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, CrmNo = crmNo });
            crm_id = CommFunc.ConvertDBNullToInt32(obj);
            if (crm_id > 0)
                throw new Exception("用户名已存在");
            if (crm_id == 0)
            {
                obj = null;
                strSql.Clear();
                strSql.Append("select max(Crm_id) as id from v3_user where Ledger=@Ledger");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger });
                crm_id = CommFunc.ConvertDBNullToInt32(obj) + 1;
            }
            strSql.Clear();
            strSql.Append(" INSERT INTO v3_user(Ledger ,Crm_id ,CrmNo ,CrmName ,Passwd ,Create_by ,Create_dt ,Update_by , Update_dt )");
            strSql.Append(" VALUES (@Ledger,@Crm_id,@CrmNo,'',GetENCODE(@Passwd,@EncryptPwd),@SysUid,now(),@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE ");
            strSql.Append("CrmNo=@CrmNo,");
            strSql.Append("Update_by=@SysUid,");
            strSql.Append("Update_dt=now();");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = crm_id, CrmNo = crmNo, Passwd = pwd, EncryptPwd = WHoleDAL.EncryptPwd, SysUid = 1 });
        }
    }
}
