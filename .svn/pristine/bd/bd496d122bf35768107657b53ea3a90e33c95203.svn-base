using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DBUtility;

namespace YDS6000.DAL.Exp.Alarm
{
    partial class AlarmDAL
    {
        public DataTable GetYdAlarmOfCmdList(string CoStrcName, string CoName, DateTime Start, DateTime End)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);

            object params01 = new {
                Ledger=this.Ledger,
                AreaPowerStr=AreaPowerStr,
                CoStrcName="%"+CoStrcName+"%",
                CoName="%"+CoName+"%",
                Start=Start.ToString("yyyy-MM-dd"),
                End=End.ToString("yyyy-MM-dd"),
                MdItems= WHoleDAL.MdItems,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Log_id,a.Co_id,a.Module_id,a.ModuleAddr,b.ModuleName,a.Fun_id,a.FunType,a.DataValue,");
            strSql.Append("case when ifnull(a.ErrCode,0)=0 then a.Create_dt else a.EndTime end as Create_dt,a.ErrCode,a.ErrTxt,b.HandledBY,c.CoStrcName,c.CoName,s2.UName as Create_by");
            strSql.Append(" from v2_command as a inner join vp_mdinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            strSql.Append(" left join vp_coinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" left join sys_user as s2 on a.Ledger=s2.Ledger and a.Create_by=s2.Uid");
            strSql.Append(" where a.Ledger=@Ledger and a.CDate>=@Start and a.CDate<=@End and a.ErrCode<=0");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            strSql.Append(" and ifnull(c.CoStrcName,'') like @CoStrcName ");
            strSql.Append(" and ifnull(c.CoName,'') like @CoName ");
            if (!string.IsNullOrEmpty(WHoleDAL.MdItems))
                strSql.Append(" and FIND_IN_SET(b.IsDefine,@MdItems)");
            strSql.Append(" order by a.Log_id desc");
            return SQLHelper.Query(strSql.ToString(), params01);
        }
    }
}