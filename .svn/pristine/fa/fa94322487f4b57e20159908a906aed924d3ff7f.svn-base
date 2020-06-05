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
    public partial class ExpYdCollectDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        private string Project = "";
        public ExpYdCollectDAL(string Project, int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            this.Project = Project;
        }
        public DataTable GetYdCollectData(string coStrcName, string coName, string funType, int isDefine)
        {
            if (string.IsNullOrEmpty(coStrcName) || coStrcName == "{StrcName}" || coStrcName == "null")
                coStrcName = string.Empty;
            if (string.IsNullOrEmpty(coName) || coName == "{CoName}" || coName == "null")
                coName = string.Empty;

            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Multiply,a.Co_id,b.Fun_id,b.Scale,b.FunType,c.CoName,c.CoFullName,c.CoStrcName");
            strSql.Append(" from vp_mdinfo a inner join vp_funinfo b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            strSql.Append(" left join vp_coinfo c on  a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and b.FunType=@FunType and not FIND_IN_SET(a.ModuleType,@SplitException)");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            if (isDefine != 0)
                strSql.Append(" and c.IsDefine=@IsDefine");
            if (!string.IsNullOrEmpty(WHoleDAL.MdItems))
                strSql.Append(" and FIND_IN_SET(a.IsDefine,@MdItems)");
            strSql.Append(" and ifnull(c.CoName,'') like @CoName and ifnull(c.CoStrcName,'') like @CoStrcName");

            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, AreaPowerStr = AreaPowerStr, FunType = funType, IsDefine = isDefine, CoStrcName = "%" + coStrcName + "%", CoName = "%" + coName + "%", SplitException = WHoleDAL.splitException, MdItems = WHoleDAL.MdItems });
        }
    }
}
