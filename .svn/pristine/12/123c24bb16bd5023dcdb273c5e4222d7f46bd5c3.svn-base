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
        public DataTable GetRptRoomSightData(int areaId, int roomSightId = 0)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select GetCoChildList(@Ledger,@Co_id)as strList");
            string strlist = CommFunc.ConvertDBNullToString(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = areaId }));
            strSql.Clear();
            strSql.Append("select a.Meter_id,a.MeterAddr,a.MeterName,a.Multiply,a.Co_id,b.Module_id,b.ModuleAddr,b.ModuleName,c.Fun_id,c.Scale,");
            strSql.Append("d.Parent_id,d.CustAddr,d.CoNo,d.CoName,d.CoStrcName,d.Cic_id as RoomSightId,c1.CicName as RoomSightName");
            strSql.Append(" from vp_mdinfo as a inner join v1_gateway_esp_module as b on a.Ledger = b.Ledger and a.Meter_id = b.Meter_id");
            strSql.Append(" inner join v0_fun as c on a.Ledger = c.Ledger and a.Mm_id = c.Mm_id and c.FunType = 'E'");
            strSql.Append(" inner join vp_coinfo as d on a.Ledger = d.Ledger and a.Co_id = d.Co_id");
            strSql.Append(" left join v0_cic as c1 on d.Ledger=c1.Ledger and d.Cic_id=c1.Cic_id");
            strSql.Append(" where a.Ledger=@Ledger and FIND_IN_SET(a.Co_id, @Strlist)");
            if (roomSightId != 0)
                strSql.Append(" and d.Cic_id=@Cic_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Strlist = strlist, Cic_id = roomSightId });
        }
    }
}
