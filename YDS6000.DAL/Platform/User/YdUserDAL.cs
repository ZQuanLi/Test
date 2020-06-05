﻿using System;
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
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public DataTable GetUserList(int uid = 0)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Uid,a.USign,a.Role_id,a.UName,a.Disabled,GetDECODE(a.UPasswd,@EncryptPwd) UPasswd,b.Descr,c.UName as CreateName,a.Create_dt as CreateTime,a.TelNo");
            strSql.Append(" from sys_user a left join sys_role b on a.Ledger=b.Ledger and a.Role_id=b.Role_id");
            strSql.Append(" left join sys_user as c on a.Ledger=c.Ledger and a.Create_by=c.Uid");
            strSql.Append(" where a.Ledger=@Ledger");
            if (uid != 0)
                strSql.Append(" and a.Uid=@Uid");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Uid = uid, EncryptPwd = WHoleDAL.EncryptPwd });
        }

        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="sys_user">用户信息</param>
        /// <returns></returns>
        public int SetUser(sys_user user)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(*)as cnt from sys_user where Ledger=@Ledger and Uid!=@Uid and USign=@USign");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Uid = user.Uid, USign = user.USign });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("账号:" + user.USign + "已存在");
            if (user.Uid == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Uid)as Uid from sys_user where Ledger=@Ledger");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger });
                user.Uid = CommFunc.ConvertDBNullToInt32(obj) + 1;
                //////////
                strSql.Clear();
                strSql.Append("insert into sys_user( Ledger,Uid,USign,UPasswd,UName,Role_id,Disabled,Create_by,Create_dt,Update_by,Update_dt)");
                strSql.Append("values(@Ledger,@Uid,@USign,GetENCODE(@UPasswd,@EncryptPwd),@UName,@Role_id,@Disabled,@SysUid,now(),@SysUid,now());");
                strSql.Append("insert into sys_userinfo(Ledger,Uid,TelNo,Create_by,Create_dt,Update_by,Update_dt)values(@Ledger,@Uid,@TelNo,@SysUid,now(),@SysUid,now())");
                return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Uid = user.Uid, USign = user.USign, UPasswd = user.UPasswd, EncryptPwd = WHoleDAL.EncryptPwd, UName = user.UName, Role_id = user.Role_id, Disabled = user.Disabled, TelNo = user.TelNo, SysUid = this.SysUid });
            }
            else
            {
                strSql.Clear();
                strSql.Append("update sys_user set USign=@USign,UPasswd=GetENCODE(@UPasswd,@EncryptPwd),UName=@UName,Role_id=@Role_id,Disabled=@Disabled,Update_by=@SysUid,Update_dt=now() where Ledger=@Ledger and Uid=@Uid");
                strSql.Append("update sys_userinfo set TelNo=@TelNoUpdate_by=@SysUid,Update_dt=now() where Ledger=@Ledger and Uid=@Uid");
                return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Uid = user.Uid, USign = user.USign, UPasswd = user.UPasswd, EncryptPwd = WHoleDAL.EncryptPwd, UName = user.UName, Role_id = user.Role_id, Disabled = user.Disabled, TelNo = user.TelNo, SysUid = this.SysUid });
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="Uid">用户ID号</param>
        /// <returns></returns>
        public int DelUser(int uid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(*)as cnt from sys_user where Ledger=@Ledger and Uid=@Uid and UserType=1");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Uid = uid });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("系统用户不能删除!");
            strSql.Clear();
            strSql.Append("delete from sys_user where Ledger=@Ledger and Uid=@Uid;");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Uid = uid });
        }
    }
}
