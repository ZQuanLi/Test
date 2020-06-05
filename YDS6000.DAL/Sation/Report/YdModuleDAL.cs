using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Report
{
    public partial class ReportDAL
    {
        public DataTable GetYdModuleListParams(int co_id,int meter_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select GetCoChildList(@Ledger,case when @Meter_id=0 then @Co_id else -999 end)as strList");
            string strlist = CommFunc.ConvertDBNullToString(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id, Meter_id = meter_id }));
            strSql.Clear();
            strSql.Append("select a.Meter_id,a.MeterAddr,a.MeterName,a.Multiply,a.Co_id,b.Module_id,b.ModuleAddr,b.ModuleName,c.Fun_id,c.Scale,d.CoStrcName,d.CoName");
            strSql.Append(" from vp_mdinfo as a inner join v1_gateway_esp_module as b on a.Ledger = b.Ledger and a.Meter_id = b.Meter_id");
            strSql.Append(" inner join v0_fun as c on a.Ledger = c.Ledger and a.Mm_id = c.Mm_id and c.FunType = 'E'");
            strSql.Append(" left join vp_coinfo as d on a.Ledger = d.Ledger and a.Co_id = d.Co_id");
            strSql.Append(" where a.Ledger=@Ledger");
            if (meter_id != 0)
                strSql.Append(" and a.Meter_id=@Meter_id");
            if (co_id != 0)
                strSql.Append(" and FIND_IN_SET(a.Meter_id, @Strlist)");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = meter_id, Strlist = strlist });
        }
    }
}
