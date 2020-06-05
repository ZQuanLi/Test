using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Energy.Home
{
    public partial class HomeDAL
    {
        //private int Ledger = 0;
        //private int SysUid = 0;

        //public HomeDAL(int ledger, int uid)
        //{
        //    this.Ledger = ledger;
        //    this.SysUid = uid;
        //}
        public DataTable GetEnergyUseVal(int co_id,int? Attrib=null,string EnergyItemCode=null,string Module_ids=null)
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
            if (Attrib!=null)
            {
                strSql.Append(" and exists (select 1 from v1_gateway_esp_meter where Ledger=@ledger and Meter_id=a.Meter_id and Attrib=@Attrib)");
            }
            if (EnergyItemCode!=null)
            {
                strSql.Append(" and a.EnergyItemCode=@EnergyItemCode");
            }
            if (Module_ids!=null)
            {
                strSql.Append(" and find_in_set(a.Module_id,@Module_ids)");
            }
            strSql.Append(" order by a.MeterNo,a.Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, StrList = strList, Attrib = Attrib, EnergyItemCode = EnergyItemCode, Module_ids = Module_ids });
        }

        /// <summar y>
        /// 获取监测数据信息
        /// </summary>
        /// <param name="co_id">co_id</param>
        /// <returns></returns>
        public DataTable GetMonitorList(int co_id,int? Attrib=null,string EnergyItemCode=null,string Module_ids=null)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select GetCoChildList(@Ledger,@Co_id)as ids");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
            string strList = CommFunc.ConvertDBNullToString(obj);

            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,c.MeterName as ModuleName,a.Fun_id,a.FunType,a.FunName,a.LpszDbVarName,a.Unit,a.IsDefine,b.FrMd");
            strSql.Append(" from vp_funinfo as a inner join syscont as b on a.Ledger=b.Ledger");
            strSql.Append(" inner join v1_gateway_esp_meter as c on a.Ledger=c.Ledger and a.Meter_id=c.Meter_id");
            strSql.Append(" where a.Ledger=@Ledger and FIND_IN_SET(a.Co_id,@StrList) and FIND_IN_SET(a.FunType,'E,P')");// FIND_IN_SET(a.FunType,'E,U,I,P,Ua,Ia,Pa')
            if (Attrib != null)
            {
                strSql.Append(" and c.Attrib=@Attrib");
            }
            if (EnergyItemCode != null)
            {
                strSql.Append(" and a.EnergyItemCode=@EnergyItemCode");
            }
            if (Module_ids != null)
            {
                strSql.Append(" and a.Module_id in(@Module_ids)");
            }
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, StrList = strList, Attrib = Attrib, EnergyItemCode = EnergyItemCode, Module_ids = Module_ids });
        }

        public DataTable GetChildCos(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT Co_id,CoName FROM v1_cust");
            strSql.Append(" WHERE Ledger=@Ledger AND Parent_id=@Parent_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Parent_id = co_id });
        }
    }
}
