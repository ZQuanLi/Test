using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.PDU.Report
{
    public partial class ReportDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;

        public ReportDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// Pdu状态信息
        /// </summary>
        /// <param name="co_id">pdu ID号</param>
        /// <returns></returns>
        public DataTable GetEnergy(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Fun_id,a.FunType,a.FunName,a.LpszDbVarName,a.Unit,a.Scale,a.Multiply,a.IsDefine,b.FrMd");
            strSql.Append(" from vp_funinfo as a inner join syscont as b on a.Ledger=b.Ledger");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id and a.Parent_id=0 and (a.IsChrg=1 or a.FunType='E')");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
        }

        public DataTable GetHisdata(string moduleName, string funName)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Clear();
            //strSql.Append("select GetCoChildList(@Ledger,@Co_id)as ids");
            //object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
            //string strList = CommFunc.ConvertDBNullToString(obj);

            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Co_id,a.Fun_id,a.FunType,a.FunName,a.Unit,a.Scale,a.Multiply,a.IsChrg,");
            strSql.Append("a.LpszDbVarName,a.Unit,a.IsDefine,b.FrMd,c.CoName");
            strSql.Append(" from vp_funinfo as a inner join syscont as b on a.Ledger=b.Ledger");
            strSql.Append(" left join vp_coinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" where a.Ledger=@Ledger");
            if (!string.IsNullOrEmpty(moduleName))
                strSql.Append(" and a.ModuleName like @ModuleName");
            if (!string.IsNullOrEmpty(funName))
                strSql.Append(" and a.FunName like @FunName");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, ModuleName = moduleName + "%", FunName = funName + "%" });
        }

        public DataTable GetEnergyUseVal(string moduleName)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Clear();
            //strSql.Append("select GetCoChildList(@Ledger,@Co_id)as ids");
            //object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
            //string strList = CommFunc.ConvertDBNullToString(obj);

            strSql.Clear();
            strSql.Append("select a.Module_id,a.Fun_id,a.ModuleAddr,a.ModuleName,a.Co_id,a.Unit,a.Scale,a.Multiply,a.FunType,a.FunName,a.IsChrg,");
            strSql.Append("a.EnergyItemCode,d.CoName,a.IsDefine");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" left join vp_coinfo as d on a.Ledger=d.Ledger and a.Co_id=d.Co_id");
            strSql.Append(" where a.Ledger=@ledger and (a.IsChrg=1 or a.FunType='E')");
            if (!string.IsNullOrEmpty(moduleName))
            {
                strSql.Append(" and a.ModuleName like @ModuleName");
            }
            strSql.Append(" order by a.MeterNo,a.Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, ModuleName = moduleName + "%" });
        }
    }
}
