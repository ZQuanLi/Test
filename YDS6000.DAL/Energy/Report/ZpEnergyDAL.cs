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
        public DataTable GetEnergyMdList(int co_id, string moduleName)
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
            strSql.Append(" where a.Ledger=@ledger and a.IsChrg=1 and a.EnergyItemCode!='' and FIND_IN_SET(a.Co_id,@StrList)");
            if (!string.IsNullOrEmpty(moduleName))
            {
                strSql.Append(" and a.ModuleName like @ModuleName");
            }
            strSql.Append(" order by a.MeterNo,a.Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, StrList = strList, ModuleName = moduleName + "%" });
        }

        public DataTable GetEnergyHisdata(int co_id, string moduleName,string funType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select GetCoChildList(@Ledger,@Co_id)as ids");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
            string strList = CommFunc.ConvertDBNullToString(obj);

            strSql.Clear();
            strSql.Append("select a.Module_id,a.Fun_id,a.ModuleAddr,a.ModuleName,a.MeterNo,a.Co_id,a.Unit,a.Scale,a.Multiply,a.FunType,a.FunName,a.IsChrg,");
            strSql.Append("a.EnergyItemCode,b.EnergyItemName,b.ParentItemCode,c.EnergyItemName as ParentItemName,d.CoName");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" left join v0_energyitemdict as b on a.EnergyItemCode=b.EnergyItemCode");
            strSql.Append(" left join v0_energyitemdict as c on b.ParentItemCode=c.EnergyItemCode");
            strSql.Append(" left join vp_coinfo as d on a.Ledger=d.Ledger and a.Co_id=d.Co_id");
            strSql.Append(" where a.Ledger=@ledger and FIND_IN_SET(a.Co_id,@StrList)");
            if (string.IsNullOrEmpty(funType))
                strSql.Append(" and a.IsChrg=1");
            else
                strSql.Append(" and a.FunType=@FunType");
            if (!string.IsNullOrEmpty(moduleName))
            {
                strSql.Append(" and a.ModuleName like @ModuleName");
            }
            strSql.Append(" order by a.MeterNo,a.Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, StrList = strList, ModuleName = moduleName + "%", FunType = funType });
        }

        public DataTable GetAlarmList(int co_id, DateTime start, DateTime end, string moduleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select GetCoChildList(@Ledger,@Co_id)as ids");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
            string strList = CommFunc.ConvertDBNullToString(obj);

            strSql.Clear();
            strSql.Append("select a.Log_id,a.CDate,a.Co_id,a.Module_id,a.ModuleAddr,a.Content,a.CollectTime,a.CollectValue,b.CoName,c.ModuleName");
            strSql.Append(" from v2_alarm_log as a inner join vp_coinfo as b on a.Ledger = b.Ledger and a.Co_id = b.Co_id");
            strSql.Append(" inner join vp_mdinfo as c on a.Ledger = c.Ledger and a.Module_id = c.Module_id and a.ModuleAddr = c.ModuleAddr");
            strSql.Append(" where a.CDate>=@Start and a.CDate<=@End and a.Ledger=@Ledger and FIND_IN_SET(a.Co_id,@StrList) and a.AType=@AType");
            strSql.Append(" and c.ModuleName like @ModuleName");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Start = start, End = end, StrList = strList, AType = AlarmType.Err_Tx.ToString(), ModuleName = moduleName + "%" });
        }
    }
}
