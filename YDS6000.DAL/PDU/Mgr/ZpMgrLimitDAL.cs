using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.PDU.Mgr
{
    public partial class MgrDAL
    {
        public DataTable GetMgrLimit11()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Ledger,a.Module_id,a.ModuleName,b.LpszDbVarName as TagUp,mp1.DataValue as DataUp,mp1.Status as StatusUp,mp1.Update_dt as TimeUp,");
            strSql.Append("c.LpszDbVarName as TagDown,mp2.DataValue as DatatDown,mp2.Status as StatusDown,mp2.Update_dt as TimeDown");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" inner join vp_funinfo as b on a.Ledger = b.Ledger and a.Module_id = b.Module_id and b.FunType = 'IaMax'");
            strSql.Append(" inner join vp_funinfo as c on a.Ledger = c.Ledger and a.Module_id = c.Module_id and c.FunType = 'IaMin'");
            strSql.Append(" left join v1_map as mp1 on b.Ledger = mp1.Ledger and b.Module_id = mp1.Module_id and b.Fun_id = mp1.Fun_id");
            strSql.Append(" left join v1_map as mp2 on c.Ledger = mp2.Ledger and c.Module_id = mp2.Module_id and c.Fun_id = mp2.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, FunType = V0Fun.Ssr.ToString() });
        }

        public DataTable GetMgrLimit(string moduleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Ledger,a.Module_id,a.ModuleName,b.Fun_id,b.FunName,b.LpszDbVarName as Tag,mp1.DataValue,mp1.Status,mp1.Update_dt");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" inner join vp_funinfo as b on a.Ledger = b.Ledger and a.Module_id = b.Module_id");
            strSql.Append(" left join v1_map as mp1 on b.Ledger = mp1.Ledger and b.Module_id = mp1.Module_id and b.Fun_id = mp1.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and FIND_IN_SET(b.FunType,'IaMax,IaMin,IbMax,IbMin,IcMax,IcMin,IMax,IMin')");
            strSql.Append(" and a.ModuleName like @ModuleName");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, ModuleName = moduleName + "%", FunType = V0Fun.Ssr.ToString() });
        }
    }
}
