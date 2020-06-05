using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YDS6000.Models;
using DBUtility;

namespace YDS6000.DAL.Exp.Monitor
{
    partial class MonitorDAL
    {
        public DataTable GetYdSmsContent(int co_id,int module_id, string moduleAddr)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Co_id = co_id,
                Module_id = module_id,
                ModuleAddr = moduleAddr,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Ledger,a.Module_id,a.ModuleAddr,a.Co_id,a.Multiply,b.LastVal as CurVal,round(b.RdVal * a.Multiply,b.Scale)as RdVal,round(b.LastVal * a.Multiply,b.Scale)as LastVal,");
            strSql.Append("b.LastTime,c1.CoName,c1.CoStrcName,''as Ssr,b.MemcachKey,a.HandledBY,u1.Crm_id,u2.CrmName,");
            strSql.Append("u2.MPhone,u2.Email,round(IF(d.price>0,d.price,e.price),3) Price ");
            strSql.Append(" from vp_mdinfo as a left join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and a.Mm_id=b.Mm_id and b.FunType='E'");
            strSql.Append(" left join vp_coinfo as c1 on a.Ledger=c1.Ledger and a.Co_id=c1.Co_id left join v3_userinfo as u1 on a.Ledger=u1.Ledger and a.Co_id=u1.Co_id ");
            strSql.Append(" left join v3_user as u2 on u1.Ledger=u2.Ledger and u1.Crm_id=u2.Crm_id");
            strSql.Append(" INNER JOIN v1_gateway_esp_module_info as d on a.Ledger=d.Ledger and a.Module_id=d.Module_id ");
            strSql.Append(" INNER JOIN syscont e on a.Ledger=e.Ledger ");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id and a.Module_id=@Module_id and a.ModuleAddr=@ModuleAddr");

            //strSql.Append("select a.Ledger,a.Module_id,a.ModuleAddr,a.Co_id,a.Multiply,");
            //strSql.Append("b.LastVal as CurVal,round(b.RdVal * a.Multiply,b.Scale)as RdVal,");
            //strSql.Append("round(b.LastVal * a.Multiply,b.Scale)as LastVal,");
            //strSql.Append("b.LastTime,c1.CoName,c1.CoStrcName,''as Ssr,s1.FrMd,b.MemcachKey,a.HandledBY,");
            //strSql.Append("u1.Crm_id,u2.CrmName,u2.MPhone,u2.Email,s1.Price");
            //strSql.Append(" from vp_mdinfo as a");
            //strSql.Append(" left join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and b.FunType='E'");
            //strSql.Append(" left join vp_coinfo as c1 on a.Ledger=c1.Ledger and a.Co_id=c1.Co_id");
            //strSql.Append(" left join v3_userinfo as u1 on a.Ledger=u1.Ledger and a.Co_id=u1.Co_id");
            //strSql.Append(" left join v3_user as u2 on u1.Ledger=u2.Ledger and u1.Crm_id=u2.Crm_id");
            //strSql.Append(" left join syscont as s1 on a.Ledger=s1.Ledger");
            //strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id and a.Module_id=@Module_id and a.ModuleAddr=@ModuleAddr");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        public DataTable GetMailInfo()
        {
            object params01 = new {
                Ledger=this.Ledger,                
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT CfKey,CfValue FROM sys_config WHERE Ledger=@Ledger and CfType='Email'");
            return SQLHelper.Query(strSql.ToString(), params01);
        }
    }
}
