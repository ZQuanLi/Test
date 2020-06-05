using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;
using YDS6000.DAL;

namespace YDS6000.DAL.Exp.RunReport
{
    public partial class ExpYdSsrDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        private string Project = "";
        public ExpYdSsrDAL(string Project, int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            this.Project = Project;
        }

        public DataTable GetYdSsrOfCmd(string CoStrcName, string CoName, DateTime Start, DateTime End)
        {
            if (string.IsNullOrEmpty(CoStrcName) || CoStrcName == "{StrcName}" || CoStrcName == "null")
                CoStrcName = string.Empty;
            if (string.IsNullOrEmpty(CoName) || CoName == "{CoName}" || CoName == "null")
                CoName = string.Empty;

            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Log_id,a.Co_id,a.Module_id,a.ModuleAddr,b.ModuleName,a.FunType,a.Create_dt,c.CoStrcName,c.CoName,s2.UName as Create_by,a.DataValue");
            strSql.Append(" from v2_command as a inner join vp_mdinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            strSql.Append(" inner join vp_coinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" left join sys_user as s2 on a.Ledger=s2.Ledger and a.Create_by=s2.Uid");
            strSql.Append(" where a.Ledger=@Ledger and a.CDate>=@Start and a.CDate<=@End and a.ErrCode=1");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            strSql.Append(" and a.FunType in('Ssr0','Ssr1','Ssr') and LENGTH(a.DataValue)>0 ");
            strSql.Append(" and ifnull(c.CoStrcName,'') like @CoStrcName ");
            strSql.Append(" and ifnull(c.CoName,'') like @CoName ");
            if (!string.IsNullOrEmpty(WHoleDAL.MdItems))
                strSql.Append(" and FIND_IN_SET(b.IsDefine,@MdItems)");
            strSql.Append(" order by a.Log_id desc");

            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, AreaPowerStr = AreaPowerStr, CoStrcName = "%" + CoStrcName + "%", CoName = "%" + CoName + "%", Start = Start.ToString("yyyy-MM-dd"), End = End.ToString("yyyy-MM-dd"), MdItems = WHoleDAL.MdItems });
        }
    }
}
