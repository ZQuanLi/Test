using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.PDU.Mgr
{
    public partial class MgrDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;

        public MgrDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }
        
        public DataTable GetMgrStatus()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Parent_id,b.Fun_id,b.FunType,b.FunName,b.LpszDbVarName,b.Unit,c.CoName");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" left join vp_funinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and b.FunType=@FunType");
            strSql.Append(" left join v1_cust as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Parent_id=0 and a.IsDefine=0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, FunType = V0Fun.Ssr.ToString() });
        }
        /// <summary>
        /// Pdu信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetMgrCtrl(string moduleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Parent_id,b.Fun_id,b.FunType,b.FunName,b.LpszDbVarName,b.Unit,mp.Status,mp.DataValue,mp.Update_dt");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" left join vp_funinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and b.FunType=@FunType");
            strSql.Append(" left join v1_map as mp on b.Ledger=mp.Ledger and b.Module_id=mp.Module_id and b.Fun_id=mp.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and (a.IsDefine=0 or a.IsDefine=50)");
            strSql.Append(" and a.ModuleName like @ModuleName");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, FunType = V0Fun.Ssr.ToString(), ModuleName = moduleName + "%" });
        }
    }
}
