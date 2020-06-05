using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Exp.Dqhz
{
    public partial class DqhzDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        public DqhzDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }
        /// <summary>
        /// 获取电气或者列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetDqhzList()
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Module_id,a.Meter_id,a.ModuleName,a.MeterName,b.Fun_id,b.FunType,b.FunName,b.Scale,b.LpszDbVarName");
            strSql.Append(" from vp_mdinfo as a inner join vp_funinfo as b on a.Ledger = b.Ledger and a.Module_id = b.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and a.IsDefine = 200");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");    
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, AreaPowerStr });
        }

        /// <summary>
        /// 参数下拉框
        /// </summary>
        /// <returns></returns>
        public DataTable GetFunTypes()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select b.Fun_id,b.FunName,b.FunType");
            strSql.Append(" from v0_module as a inner join v0_fun as b on a.Ledger = b.Ledger and a.Mm_id = b.Mm_id");
            strSql.Append(" where a.Ledger = @Ledger and a.IsDefine = 200");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }
        /// <summary>
        /// 参数下拉框
        /// </summary>
        /// <returns></returns>
        public DataTable GetFunTypes(int module_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.Co_id,a.Fun_id,a.FunName,a.FunType,a.Scale");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" where a.Ledger = @Ledger and a.Module_id=@Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id= module_id });
        }
        public DataTable GetFunData(int module_id,string funtypes)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Module_id,a.Fun_id,a.FunType,a.Scale");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" where a.Ledger = @Ledger and a.Module_id = @Module_id and FIND_IN_SET(a.FunType,@FunTypes)");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, FunTypes = funtypes });
        }
    }
}
