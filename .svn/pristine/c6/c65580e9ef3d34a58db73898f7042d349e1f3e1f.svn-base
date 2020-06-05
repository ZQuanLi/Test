using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.PDU.Monitor
{
    public partial class MonitorDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;

        public MonitorDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// Pdu状态信息
        /// </summary>
        /// <param name="co_id">pdu ID号</param>
        /// <returns></returns>
        public DataTable GetMonitorInfo(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Fun_id,a.FunType,a.FunName,a.LpszDbVarName,a.Unit,a.IsDefine,b.FrMd");
            strSql.Append(" from vp_funinfo as a inner join syscont as b on a.Ledger=b.Ledger");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id and a.Parent_id=0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
        }

        public decimal GetMonitorUseVal(int co_id, int module_id,string moduleAddr,int fun_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.LastVal-a.FirstVal");
            strSql.Append(" from v2_info as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id and a.Module_id=@Module_id and a.ModuleAddr=@ModuleAddr and a.Fun_id=@Fun_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id, Module_id = module_id, ModuleAddr = moduleAddr, Fun_id = fun_id });
            return CommFunc.ConvertDBNullToDecimal(obj);
        }

        /// <summary>
        /// Pdu状态信息
        /// </summary>
        /// <param name="co_id">pdu ID号</param>
        /// <returns></returns>
        public DataTable GetMonitorList(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Fun_id,a.FunType,a.FunName,a.LpszDbVarName,a.Unit,a.IsDefine,b.FrMd,mp1.DataValue,mp1.Status,mp1.Update_dt");
            strSql.Append(" from vp_funinfo as a inner join syscont as b on a.Ledger=b.Ledger");
            strSql.Append(" left join v1_map as mp1 on a.Ledger = mp1.Ledger and a.Module_id = mp1.Module_id and a.Fun_id = mp1.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id and a.Parent_id>0 and FIND_IN_SET(a.FunType,'E,U,I,P,Ssr,IMax,IMin')");// FIND_IN_SET(a.FunType,'E,U,I,P,Ua,Ia,Pa')
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
        }

        /// <summary>
        /// 获取层级1的数据
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public DataTable GetLayer01(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Co_id,a.CoName,a.CustAddr,a.Disabled,COUNT(b.Co_id) as Number");
            strSql.Append(" from vp_coinfo as a left join v1_cust as b on a.Ledger = b.Ledger and a.Co_id = b.Parent_id");
            strSql.Append(" where a.Ledger = @Ledger and a.Parent_id = @Parent_id");
            strSql.Append(" group by a.Co_id ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Parent_id = co_id });
        }

        /// <summary>
        /// 获取层级2的数据
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public DataTable GetLayer02(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Co_id,a.CoName,a.CustAddr,a.Disabled,COUNT(b.Co_id) as Number");
            strSql.Append(" from vp_coinfo as a left join v1_cust as b on a.Ledger = b.Ledger and a.Co_id = b.Parent_id");
            strSql.Append(" where a.Ledger = @Ledger and a.Parent_id = @Parent_id");
            strSql.Append(" group by a.Co_id ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Parent_id = co_id });
        }

        public DataTable GetLastInfo(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select GetCoChildList(@Ledger,@Co_id)");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
            string strList = CommFunc.ConvertDBNullToString(obj);
            strSql.Clear();
            strSql.Append("select a.Co_id,b.ModuleType,a.Module_id,a.ModuleAddr,a.LastVal,a.MemcachKey");
            strSql.Append(" from vp_v2info as a inner join vp_mdinfo as b on a.Ledger = b.Ledger and a.Module_id = b.Module_id and a.Co_id = b.Co_id and a.Mm_id = b.Mm_id");
            strSql.Append(" where a.Ledger = @Ledger and(b.ModuleType = 'SD' or b.ModuleType = 'WSD')");
            strSql.Append(" and FIND_IN_SET(a.Co_id,@Strlist)");
            strSql.Append(" group by a.Co_id, b.ModuleType");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Parent_id = co_id, Strlist = strList });
        }

        public DataTable GetLayer03(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            string strList = "";
            if (co_id != 0)
            {
                strSql.Clear();
                strSql.Append("select GetCoChildList(@Ledger,@Co_id)");
                object obj1 = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
                strList = CommFunc.ConvertDBNullToString(obj1);
            }
            strSql.Clear();
            strSql.Append("select a.Co_id,a.CoName,a.Disabled,count(c.Module_id)as Number");
            strSql.Append(" from vp_coinfo as a");
            strSql.Append(" inner join vp_mdinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join vp_mdinfo as c on b.Ledger=c.Ledger and b.Module_id=c.Parent_id");
            strSql.Append(" where a.Ledger=@Ledger and b.Parent_id=0");
            if (co_id != 0)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@Strlist)");
            strSql.Append(" group by a.Co_id");
            //strSql.Clear();
            //strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Disabled,count(b.Module_id)as Number");
            //strSql.Append(" from vp_funinfo as a");
            //strSql.Append(" left join vp_mdinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Parent_id");
            //strSql.Append(" where a.Ledger=@Ledger and a.Parent_id=0 and a.FunType='E'");
            //if (co_id != 0)
            //    strSql.Append(" and FIND_IN_SET(a.Co_id,@Strlist)");
            //strSql.Append(" group by a.Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Strlist = strList });
        }

    }
}
