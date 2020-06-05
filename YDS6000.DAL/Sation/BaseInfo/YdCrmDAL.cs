using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.BaseInfo
{
    partial class BaseInfoDAL
    {
        public DataTable GetCrmList(int crm_id,string crmName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Crm_id,a.CrmNo,a.CrmName,a.MPhone,a.Email,a.Remark");
            strSql.Append(" from v3_user as a ");
            strSql.Append(" where a.Ledger=@Ledger and a.CrmName like @CrmName");
            if (crm_id != 0)
                strSql.Append(" and a.Crm_id=@Crm_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = crm_id, CrmName = crmName + "%" });
        }

        public int SetCrm(CrmVModel crm)
        {
            StringBuilder strSql = new StringBuilder();
            if (crm.Crm_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Crm_id)as cnt from v3_user where Ledger=@Ledger");
                crm.Crm_id = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger })) + 1;
            }
            object params01 = new
            {
                Ledger = this.Ledger,
                Crm_id = crm.Crm_id,
                CrmNo = crm.CrmNo,
                CrmName = crm.CrmName,
                Email = crm.Email,
                MPhone = crm.MPhone,
                Remark = crm.Remark,
                SysUid = this.SysUid
            };
            strSql.Clear();
            strSql.Append("insert into v3_user(");
            strSql.Append("Ledger,Crm_id,CrmNo,CrmName,Email,MPhone,Remark,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Crm_id,@CrmNo,@CrmName,@Email,@MPhone,@Remark,@SysUid,now(),@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE ");
            strSql.Append("CrmNo=@CrmNo,");
            strSql.Append("CrmName=@CrmName,");
            strSql.Append("Email=@Email,");
            strSql.Append("MPhone=@MPhone,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("Update_by=@SysUid,");
            strSql.Append("Update_dt=now();");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }

        public int DelCrm(int crm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete c,b,a from v3_user as a left join v3_userinfo as b on a.Ledger = b.Ledger and a.Crm_id = b.Crm_id");
            strSql.Append(" left join v3_openid as c on a.Ledger = c.Ledger and a.Crm_id = c.Crm_id ");
            strSql.Append(" where a.Ledger=@Ledger and a.Crm_id=@Crm_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = crm_id } );
        }

        public DataTable GetCrmOfRoomList(int crm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Crm_id,b.Co_id,c.CoName,c.CoFullName");
            strSql.Append(" from v3_user as a inner join v3_userinfo as b on a.Ledger = b.Ledger and a.Crm_id = b.Crm_id");
            strSql.Append(" left join vp_coinfo as c on  b.Ledger = c.Ledger and b.Co_id = c.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Crm_id=@Crm_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = crm_id });
        }
        /// <summary>
        /// 获取没有业主信息的站点信息
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public DataTable GetRoomNotExisCrmList(string roomName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Co_id,a.CoName,a.CoFullName");
            strSql.Append(" from vp_coinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.CoName like @CoName");
            strSql.Append(" and not exists(select * from v3_userinfo where Ledger=a.Ledger and Co_id=a.Co_id)");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, CoName = roomName + "%" });
        }

        public int SetCrmOfRoom(int crm_id, string roomStrlist)
        {
            StringBuilder strSql = new StringBuilder();
            int cc = 0;
            foreach (string ss in roomStrlist.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (CommFunc.ConvertDBNullToInt32(ss) != 0)
                {
                    strSql.Clear();
                    strSql.Append("insert into v3_userinfo(Ledger,Crm_id,Co_id,IsHold,Create_by,Create_dt)values(@Ledger,@Crm_id,@Co_id,0,@SysUid,now())");
                    strSql.Append(" ON DUPLICATE KEY UPDATE IsHold=0");
                    cc = cc + SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = crm_id, Co_id = CommFunc.ConvertDBNullToInt32(ss), SysUid = this.SysUid });
                }
            }
            return cc;
        }

        public int DelCrmOfRoom(int crm_id, string roomStrlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete b from v3_user as a inner join v3_userinfo as b on a.Ledger = b.Ledger and a.Crm_id = b.Crm_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Crm_id=@Crm_id and FIND_IN_SET(b.Co_id,@RoomStrlist)");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = crm_id, RoomStrlist = roomStrlist });
        }
    }
}
