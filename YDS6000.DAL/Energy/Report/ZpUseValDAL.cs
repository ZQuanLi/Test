using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Energy.Report
{

    partial class ReportDAL
    {
        public DataTable GetEnergyUseVal(int co_id, string moduleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select GetCoChildList(@Ledger,@Co_id)as ids");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
            string strList = CommFunc.ConvertDBNullToString(obj);

            strSql.Clear();
            strSql.Append("select a.Module_id,a.Fun_id,a.ModuleAddr,a.ModuleName,a.Co_id,a.Unit,a.Scale,a.Multiply,a.FunType,a.FunName,a.IsChrg,");
            strSql.Append("a.EnergyItemCode,b.EnergyItemName,b.ParentItemCode,c.EnergyItemName as ParentItemName,d.CoName");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" left join v0_energyitemdict as b on a.EnergyItemCode=b.EnergyItemCode");
            strSql.Append(" left join v0_energyitemdict as c on b.ParentItemCode=c.EnergyItemCode");
            strSql.Append(" left join vp_coinfo as d on a.Ledger=d.Ledger and a.Co_id=d.Co_id");
            strSql.Append(" where a.Ledger=@ledger and a.IsChrg=1 and FIND_IN_SET(a.Co_id,@StrList)");
            if (!string.IsNullOrEmpty(moduleName))
            {
                strSql.Append(" and a.ModuleName like @ModuleName");
            }
            strSql.Append(" order by a.MeterNo,a.Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, StrList = strList, ModuleName = moduleName + "%" });
        }
    }
}
