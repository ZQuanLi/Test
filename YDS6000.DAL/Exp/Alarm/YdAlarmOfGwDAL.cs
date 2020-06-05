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
        public DataTable GetYdAlarmOfGwList(string CoStrcName, string CoName, string AType, DateTime Start, DateTime End)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);

            object params01 = new
            {
                Ledger = this.Ledger,
                AType = AType,
                AreaPowerStr = AreaPowerStr,
                CoStrcName = "%" + CoStrcName + "%",
                CoName = "%" + CoName + "%",
                Start = Start.ToString("yyyy-MM-dd"),
                End = End.ToString("yyyy-MM-dd"),
                MdItems = WHoleDAL.MdItems,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.AType,a.Co_id,a.Module_id,a.ModuleAddr,b.ModuleName,a.Fun_id,cast(CONCAT(a.CDate,' ',a.CTime) as char(30))as Create_dt,a.CollectTime,a.Content,a.ErrCode,a.ErrTxt,b.HandledBY,f1.FunType,f1.FunName,c.CoStrcName,c.CoName");
            strSql.Append(" from v2_alarm_log as a inner join vp_mdinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            strSql.Append(" left join v0_fun as f1 on a.Ledger=f1.Ledger and a.Fun_id=f1.Fun_id");
            strSql.Append(" left join vp_coinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.CDate>=@Start and a.CDate<=@End and a.AType=@AType");
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
