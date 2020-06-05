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
        /// <summary>
        /// 获取参数设置列表
        /// </summary>
        /// <param name="coName"></param>
        /// <param name="moduleAddr"></param>
        /// <returns></returns>
        public DataTable GetYdParamsOfList(string CoStrcName, string CoName)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);

            object params01 = new
            {
                Ledger = this.Ledger,
                CoStrcName = "%" + CoStrcName + "%",
                CoName = "%" + CoName + "%",
                AreaPowerStr = AreaPowerStr
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Module_id,a.ModuleName,a.ModuleAddr,a.Fun_id,a.FunType,a.FunName,b.DataValue,b.`Status`,a.Co_id,c1.CoStrcName,c1.CoName");
            strSql.Append(" from vp_funinfo as a ");
            strSql.Append(" inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" inner join vp_mdinfo as m1 on a.Ledger=m1.Ledger and a.Module_id=m1.Module_id");          
            strSql.Append(" left join v1_map as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and a.Fun_id=b.Fun_id");
            strSql.Append(" left join vp_coinfo as c1 on a.Ledger=c1.Ledger and a.Co_id=c1.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Action=1");
            strSql.Append(" and m1.ModuleAddr=case when s1.Zb=0 then m1.MeterAddr else m1.ModuleAddr end");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            strSql.Append(" and ifnull(c1.CoStrcName,'') like @CoStrcName");
            strSql.Append(" and ifnull(c1.CoName,'') like @CoName ");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        public DataTable GetYdParamsOfFunType()
        {
            object params01 = new
            {
                Ledger = this.Ledger
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DISTINCT FunType,FunName");
            strSql.Append(" from v0_fun where Ledger=@Ledger and Action=1 and Disabled=0");
            return SQLHelper.Query(strSql.ToString(), params01);
        }
    }
}
