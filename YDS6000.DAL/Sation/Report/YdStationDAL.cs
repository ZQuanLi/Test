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
        public DataTable GetRptStationData(int areaId,int stationTypeId = 0)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select GetCoChildList(@Ledger,@Co_id)as strList");
            string strlist = CommFunc.ConvertDBNullToString(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = areaId }));
            strSql.Clear();
            strSql.Append("select a.Meter_id,a.MeterAddr,a.MeterName,a.Multiply,a.Co_id,b.Module_id,b.ModuleAddr,b.ModuleName,c.Fun_id,c.Scale,");
            strSql.Append("d.Parent_id,c1.CustAddr,c1.CoNo,c1.CoName,c1.CoStrcName,c1.Cic_id as StationType,c2.CicName as StationTypeName");
            strSql.Append(" from vp_mdinfo as a inner join v1_gateway_esp_module as b on a.Ledger = b.Ledger and a.Meter_id = b.Meter_id");
            strSql.Append(" inner join v0_fun as c on a.Ledger = c.Ledger and a.Mm_id = c.Mm_id and c.FunType = 'E'");
            strSql.Append(" inner join vp_coinfo as d on a.Ledger = d.Ledger and a.Co_id = d.Co_id");
            strSql.Append(" inner join vp_coinfo as c1 on c1.Ledger=d.Ledger and c1.Co_id=d.Parent_id");
            strSql.Append(" left join v0_cic as c2 on c2.Ledger=c1.Ledger and c2.Cic_id=c1.Cic_id");
            strSql.Append(" where a.Ledger=@Ledger and FIND_IN_SET(a.Co_id, @Strlist)");
            if (stationTypeId !=0)
                strSql.Append(" and c1.Cic_id=@Cic_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Strlist = strlist, Cic_id = stationTypeId });
        }

        /// <summary>
        /// 获取站点类别下的所以站点信息
        /// </summary>
        /// <param name="stationTypeId"></param>
        /// <returns></returns>
        //public DataTable GetRptStationList(int stationTypeId)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Clear();
        //    strSql.Append("select a.Co_id,a.CustAddr,a.CoNo,a.CoName,a.CoStrcName");
        //    strSql.Append(" from vp_coinfo as a");
        //    strSql.Append(" where a.Ledger=@Ledger and a.Attrib=@Attrib and a.Cic_id=@Cic_id");
        //    return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Cic_id = stationTypeId , Attrib = CoAttrib.Station });
        //}
    }
}
