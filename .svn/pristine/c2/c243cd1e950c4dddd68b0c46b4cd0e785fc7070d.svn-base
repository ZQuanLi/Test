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
    public partial class ExpYdRepHisDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        private string Project = "";
        public ExpYdRepHisDAL(string Project, int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            this.Project = Project;
        }
        public DataTable GetModuleListByOrg(string CoStrcName, string CoName, params string[] funTypeParams)
        {
            if (string.IsNullOrEmpty(CoStrcName) || CoStrcName == "{StrcName}" || CoStrcName == "null")
                CoStrcName = string.Empty;
            if (string.IsNullOrEmpty(CoName) || CoName == "{CoName}" || CoName == "null")
                CoName = string.Empty;

            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select c.Co_id,c.CoName,c.CoFullName,c.CoStrcName,a.ModuleAddr,a.ModuleName,a.Module_id,a.Multiply,b.Fun_id,b.FunType");
            strSql.Append(" from vp_mdinfo a  INNER JOIN  v0_fun b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id and a.MdDisabled=0 and b.Disabled=0");
            strSql.Append(" INNER JOIN vp_coinfo c on  a.Ledger=c.Ledger and a.Co_id=c.Co_id and c.Disabled=0");
            strSql.Append(" where a.Ledger=@Ledger and not FIND_IN_SET(a.ModuleType,@SplitException)");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");

            if (funTypeParams != null && funTypeParams.Length > 0)
            {
                strSql.Append(" and ( ");
                for (int i = 0; i < funTypeParams.Length; i++)
                {
                    if (i > 0) strSql.Append(" or ");
                    strSql.Append(" b.FunType='" + funTypeParams[i] + "'");
                }
                strSql.Append(" ) ");
            }
            if (!string.IsNullOrEmpty(WHoleDAL.MdItems))
                strSql.Append(" and FIND_IN_SET(a.IsDefine,@MdItems)");
            if (!string.IsNullOrEmpty(CoStrcName)) strSql.Append(" and ifnull(c.CoStrcName,'') like @CoStrcName ");
            if (!string.IsNullOrEmpty(CoName)) strSql.Append(" and ifnull(c.CoName,'') like @CoName ");
            strSql.Append("  GROUP BY c.Co_id,c.CoName,c.CoFullName,c.CoStrcName,a.ModuleAddr,a.Module_id,b.FunType ");
            strSql.Append(" order by a.Module_id,b.Fun_id");

            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, AreaPowerStr = AreaPowerStr, CoStrcName = "%" + CoStrcName + "%", CoName = "%" + CoName + "%", SplitException = WHoleDAL.splitException, MdItems = WHoleDAL.MdItems });
        }
    }
}
