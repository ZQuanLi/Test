using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;


namespace YDS6000.DAL.PDU.Alarm
{
    public partial class AlarmDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;

        public AlarmDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        public DataTable GetAlarmData(DateTime start, DateTime end, string moduleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,b.ModuleName,a.Co_id,a.Fun_id,b.FunType,b.FunName,");
            strSql.Append("a.AType,a.Content,a.CollectTime,a.CollectValue,c.CoName,d.DataValue as IaMax,e.DataValue as IaMin");
            strSql.Append(" from v2_alarm_log as a inner join vp_funinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and a.Fun_id=b.Fun_id");
            strSql.Append(" left join vp_coinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" left join vp_funinfo as d on a.Ledger=d.Ledger and a.Module_id=d.Module_id and d.FunType=CONCAT(b.FunType,'Max') "); //'IaMax'
            strSql.Append(" left join vp_funinfo as e on a.Ledger=e.Ledger and a.Module_id=e.Module_id and e.FunType=CONCAT(b.FunType,'Min')");//'IaMin'
            strSql.Append(" where a.Ledger=@Ledger and a.CDate>=@Start and a.CDate<=@End and a.AType=@AType");
            if (!string.IsNullOrEmpty(moduleName))
                strSql.Append(" and b.ModuleName like @ModuleName");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Start = start,End = end, AType = AlarmType.Err_OFlow.ToString(), ModuleName = moduleName + "%" });
        }

    }
}
