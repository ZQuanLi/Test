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
        public DataTable GetYdAlarmOfChargeList(string CoStrcName, string CoName, string IsPay, DateTime Start, DateTime End)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);

            object params01 = new
            {
                Ledger = this.Ledger,
                AreaPowerStr = AreaPowerStr,
                CoStrcName = "%" + CoStrcName + "%",
                CoName = "%" + CoName + "%",
                IsPay = IsPay + "%",
                Start = Start.ToString("yyyy-MM-dd"),
                End = End.ToString("yyyy-MM-dd"),
                MdItems = WHoleDAL.MdItems,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Log_id,a.CDate,a.Co_id,a.Module_id,a.ModuleAddr,c.ModuleName,b.CoStrcName,b.CoName,b.CoFullName,");
            strSql.Append("a.PayAmt,a.PayVal,a.PayType,a.IsWrong,a.IsPay,a.Trade_no,case when a.IsPay=1 then s1.CrmName else s2.UName end as Create_by,");
            strSql.Append("a.Create_dt,a.ErrCode,a.EndTime,a.ErrTxt");
            strSql.Append(" from v4_pay_log as a");
            if (string.IsNullOrEmpty(WHoleDAL.MdItems))
                strSql.Append(" left join vp_mdinfo as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id and a.ModuleAddr=c.ModuleAddr");
            else
                strSql.Append(" inner join vp_mdinfo as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id and a.ModuleAddr=c.ModuleAddr and FIND_IN_SET(c.IsDefine,@MdItems)");
            strSql.Append(" left join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v3_user as s1 on a.Ledger=s1.Ledger and a.Create_by=s1.Crm_id");
            strSql.Append(" left join sys_user as s2 on a.Ledger=s2.Ledger and a.Create_by=s2.Uid");
            strSql.Append(" where a.Ledger=@Ledger and a.CDate>=@Start and a.CDate<=@End and a.ErrCode!=1");
            strSql.Append(" and ifnull(b.CoStrcName,'') like @CoStrcName ");
            strSql.Append(" and ifnull(b.CoName,'') like @CoName ");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            if (!string.IsNullOrEmpty(IsPay))
                strSql.Append(" and ifnull(a.IsPay,0) like @IsPay");/*现金支付*/
            strSql.Append(" order by a.Log_id desc");
            return SQLHelper.Query(strSql.ToString(), params01);
        }
    }
}
