using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;
using YDS6000.DAL;

namespace YDS6000.DAL.Exp.PayLog
{
    public partial class ExpYdPayLogDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        private string Project = "";
        public ExpYdPayLogDAL(string Project, int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            this.Project = Project;
        }

        public DataTable GetYdPayLogOnList(string StrcName, string CoName, int d_ways, string Create_by, DateTime StartTime, DateTime EndTime)
        {
            if (string.IsNullOrEmpty(StrcName) || StrcName == "{StrcName}" || StrcName == "null")
                StrcName = string.Empty;
            if (string.IsNullOrEmpty(CoName) || CoName == "{CoName}" || CoName == "null")
                CoName = string.Empty;
            if (string.IsNullOrEmpty(Create_by) || Create_by == "{Create_by}" || Create_by == "null")
                Create_by = string.Empty;
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Log_id,a.CDate,a.Co_id,a.Module_id,a.ModuleAddr,c.ModuleName,b.CoStrcName,b.CoName,b.CoFullName,");
            strSql.Append("a.PayAmt,a.PayVal,a.PayType,a.IsWrong,a.IsPay,a.Trade_no,s1.UName as Create_by,");
            strSql.Append("a.Create_dt,a.ErrCode,a.EndTime,a.ErrTxt");
            strSql.Append(" from v4_pay_log as a");
            if (string.IsNullOrEmpty(WHoleDAL.MdItems))
                strSql.Append(" left join vp_mdinfo as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id and a.ModuleAddr=c.ModuleAddr");
            else
                strSql.Append(" inner join vp_mdinfo as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id and a.ModuleAddr=c.ModuleAddr and FIND_IN_SET(c.IsDefine,@MdItems)");
            strSql.Append(" left join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join vp_userinfo as s1 on a.Ledger=s1.Ledger and a.Create_by=s1.Uid");
            //strSql.Append(" left join sys_user as s2 on a.Ledger=s2.Ledger and a.Create_by=s2.Uid");
            strSql.Append(" where a.Ledger=@Ledger and a.CDate>=@Start and a.CDate<=@End and a.ErrCode=1");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            strSql.Append(" and ifnull(b.CoStrcName,'') like @CoStrcName ");
            strSql.Append(" and ifnull(b.CoName,'') like @CoName ");
            if (!string.IsNullOrEmpty(Create_by))
                strSql.Append(" and ifnull(s1.UName,'') like @Create_by");
            if (d_ways == 1)
                strSql.Append(" and a.PayType=1");/*充值*/
            else if (d_ways == 2)
                strSql.Append(" and a.PayType=2");/*退费*/
            else if (d_ways == 3)
                strSql.Append(" and a.PayType=3");/*赠送电*/
            else if (d_ways == 4)
                strSql.Append(" and a.PayType=4");/*退赠送电*/
            else if (d_ways != 0)
                strSql.Append(" and 1=2");/*没有记录*/
            strSql.Append(" order by a.Log_id desc");

            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, AreaPowerStr = AreaPowerStr, CoStrcName = "%" + StrcName + "%", CoName = "%" + CoName + "%", Create_by = "%" + Create_by + "%", Start = StartTime.ToString("yyyy-MM-dd"), End = EndTime.ToString("yyyy-MM-dd"), MdItems = WHoleDAL.MdItems });
        }

        public DataTable GetHomePay(string StartTime, string EndTime)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Log_id,a.CDate,a.Co_id,a.Module_id,a.ModuleAddr,c.ModuleName,b.CoStrcName,b.CoName,b.CoFullName,");
            strSql.Append("a.PayAmt,a.PayVal,a.PayType,a.IsWrong,a.IsPay,a.Trade_no,s1.UName as Create_by,");
            strSql.Append("a.Create_dt,a.ErrCode,a.EndTime,a.ErrTxt");
            strSql.Append(" from v4_pay_log as a");
            if (string.IsNullOrEmpty(WHoleDAL.MdItems))
            {
                strSql.Append(" left join vp_mdinfo as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id and a.ModuleAddr=c.ModuleAddr");
            }
            else
            {
                strSql.Append(" inner join vp_mdinfo as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id and a.ModuleAddr=c.ModuleAddr and FIND_IN_SET(c.IsDefine,@MdItems)");
            }
            strSql.Append(" left join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join vp_userinfo as s1 on a.Ledger=s1.Ledger and a.Create_by=s1.Uid");
            //strSql.Append(" left join sys_user as s2 on a.Ledger=s2.Ledger and a.Create_by=s2.Uid");
            strSql.Append(" where a.Ledger=@Ledger and a.CDate>=@Start and a.CDate<=@End and a.ErrCode=1");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            strSql.Append(" and a.PayType=1");/*充值*/
            strSql.Append(" order by a.Log_id desc");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, AreaPowerStr = AreaPowerStr, Start = StartTime, End = EndTime, MdItems = WHoleDAL.MdItems });
        }

    }
}
