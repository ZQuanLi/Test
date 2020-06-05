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

        public DataTable GetYdAlarmOfLoadList(string CoStrcName, string CoName, DateTime Start, DateTime End)
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
            strSql.Append("SELECT a.Log_id,a.CDate,b.CoFullName,b.CoStrcName,b.CoName,a.ModuleAddr,c.ModuleName,a.AType,a.Content,a.CTime,a.CollectTime");
            strSql.Append(" FROM v2_alarm_log as a");
            strSql.Append(" left JOIN vp_coinfo as b on a.Ledger=b.Ledger and  a.Co_id=b.Co_id ");
            strSql.Append(" left join vp_mdinfo as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and a.CDate>=@Start and a.CDate<=@End and a.AType = 'EventWpf' and a.ErrCode=1 and Content!=''");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            if (!string.IsNullOrEmpty(CoStrcName))
                strSql.Append(" and ifnull(b.CoStrcName,'') like @CoStrcName ");
            if (!string.IsNullOrEmpty(CoName))
                strSql.Append(" and ifnull(b.CoName,'') like @CoName ");
            strSql.Append(" ORDER BY a.Log_id DESC");
            return SQLHelper.Query(strSql.ToString(), params01);
        }



    }
}
