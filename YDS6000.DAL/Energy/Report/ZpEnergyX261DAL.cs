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
        public DataTable GetEnergyForDayX261(string addrs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.Fun_id,a.FunType,a.ModuleAddr,a.ModuleName,a.Unit,a.Scale,a.Multiply");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" where a.Ledger=@ledger and a.IsChrg=1 and FIND_IN_SET(a.ModuleAddr,@StrList)");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, StrList = addrs });
        }
    }
}
