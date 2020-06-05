using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.PDU.Home
{
    public partial class HomeDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;

        public HomeDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取PDU列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetPduList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select distinct a.Co_id,b.CoName");
            strSql.Append(" from vp_mdinfo as a inner join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// Pdu状态信息
        /// </summary>
        /// <param name="co_id">pdu ID号</param>
        /// <returns></returns>
        public DataTable GetPduStatus(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Fun_id,a.LpszDbVarName,a.FunType,a.FunName,a.Unit,a.IsDefine,b.FrMd");
            strSql.Append(" from vp_funinfo as a inner join syscont as b on a.Ledger=b.Ledger");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id and a.Parent_id=0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
        }

        /// <summary>
        /// Pdu状态信息
        /// </summary>
        /// <param name="co_id">pdu ID号</param>
        /// <returns></returns>
        public DataTable GetPduFunTypes(int co_id,string funTypes)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Fun_id,a.FunType,a.LpszDbVarName,a.FunName,a.Scale,a.Multiply,a.Unit,a.IsDefine,b.FrMd,a.IsChrg");
            strSql.Append(" from vp_funinfo as a inner join syscont as b on a.Ledger=b.Ledger");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id and a.Parent_id=0 and a.IsDefine=@IsDefine and FIND_IN_SET(a.FunType,@FunTypes)");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id, IsDefine = MmDefine.MM, FunTypes = funTypes });
        }

        /// <summary>
        /// 获取告警数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetPduAlarm(int co_id)
        {
            DateTime fm = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime to = fm.AddMonths(1).AddDays(-1);

            StringBuilder strSql = new StringBuilder();
            string strList = "";
            if (co_id != 0)
            {
                strSql.Clear();
                strSql.Append("select GetCoChildList(@Ledger,@Co_id)");
                object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
                strList = CommFunc.ConvertDBNullToString(obj);
            }

            strSql.Clear();
            strSql.Append("select a.CDate,a.Co_id,a.Module_id,a.ModuleAddr,a.Fun_id,a.Content,a.CollectTime,a.CollectValue,a.Total,b.ModuleName,b.FunName,c.CoName");
            strSql.Append(" from v2_alarm_log as a inner join vp_funinfo as b on a.Ledger = b.Ledger and a.Module_id = b.Module_id and a.Fun_id = b.Fun_id");
            strSql.Append(" left join v1_cust as c on a.Ledger = c.Ledger and a.Co_id = c.Co_id");
            strSql.Append(" where a.CDate>=@Fm and a.CDate<=@To and a.Ledger=@Ledger and a.AType=@AType");
            if (co_id != 0)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@Strlist)");
            strSql.Append(" order by a.Log_id desc");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Fm = fm, To = to, AType = AlarmType.Err_OFlow.ToString(), Strlist = strList });
        }
    }
}
