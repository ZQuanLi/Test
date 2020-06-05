using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Exp.Syscont
{
    public partial class ExpTimingDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public ExpTimingDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取定时电表策略
        /// </summary>
        /// <param name="Descr">筛选条件：费率描述</param>
        /// <param name="rate_id">费率ID号</param>
        /// <returns></returns>
        public DataTable GetYdTiming(int Si_id, string Descr)
        {
            if (string.IsNullOrEmpty(Descr) || Descr == "{Descr}" || Descr == "null")
                Descr = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Ledger,a.Si_id,a.Descr,a.SiSSR,a.Md,a.Wk,a.Ts,a.Disabled,a.Create_by,a.Create_dt,b.UName as Update_by,a.Update_dt");
            strSql.Append(" from v1_si_ssr as a INNER JOIN sys_user as b on a.Ledger=b.Ledger and a.Update_by=b.Uid");
            strSql.Append(" where a.Ledger=@Ledger and a.Descr like @Descr");
            if (Si_id != 0)
                strSql.Append(" and a.Si_id=@Si_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Si_id = Si_id, Descr = "%" + Descr + "%" });
        }

        /// <summary>
        /// 获取新增页面内的时段设置的表格
        /// </summary>
        /// <param name="nAct"></param>
        /// <param name="Psi_id"></param>
        /// <returns></returns>
        public DataTable GetYdm_si_ssr(int Si_id, string Descr)
        {
            if (string.IsNullOrEmpty(Descr) || Descr == "{Descr}" || Descr == "null")
                Descr = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Ledger,a.Si_id,a.Descr,a.SiSSR,a.Md,a.Wk,a.Ts,a.Disabled,a.Create_by,a.Create_dt,b.UName as Update_by,a.Update_dt");
            strSql.Append(" from v1_si_ssr as a INNER JOIN sys_user as b on a.Ledger=b.Ledger and a.Update_by=b.Uid");
            strSql.Append(" where a.Ledger=@Ledger and a.Descr like @Descr");
            if (Si_id != 0)
                strSql.Append(" and a.Si_id=@Si_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Si_id = Si_id, Descr = "%" + Descr + "%" });
        }

        /// <summary>
        /// 设置保存费率信息
        /// </summary>
        /// <param name="T1st">尖单价-对应的开始时间</param>
        /// <param name="T2nd">峰单价-对应的开始时间</param>
        /// <param name="T3rd">平单价-对应的开始时间</param>
        /// <param name="T4th">谷单价-对应的开始时间</param>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="Descr">费率描述</param>
        /// <param name="Pri1st">尖单价</param>
        /// <param name="Pri2nd">峰单价</param>
        /// <param name="Pri3rd">平单价</param>
        /// <param name="Pri4th">谷单价</param>
        /// <returns></returns>
        public int EditRow(v1_si_ssrVModel si_ssr)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = null;
            if (si_ssr.si_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Si_id) as Si_id from v1_si_ssr where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                si_ssr.si_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Si_id"]) + 1 : 1;
                //si_ssr.si_id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Si_id"]) + 1;
                strSql.Clear();
                strSql.Append("insert into v1_si_ssr(");
                strSql.Append("Ledger,Si_id,Descr,SiSSR,Md,Wk,Ts,Disabled,Create_by,Create_dt,Update_by,Update_dt)");
                strSql.Append(" values (");
                strSql.Append("@Ledger,@Si_id,@Descr,@SiSSR,@Md,@Wk,@Ts,@Disabled,@uid,now(),@uid,now())");
                return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Si_id = si_ssr.si_id, Descr = si_ssr.descr, SiSSR = si_ssr.siSSR, Md = si_ssr.md, Wk = si_ssr.wk, Ts = si_ssr.ts, Disabled = si_ssr.disabled, uid = this.SysUid });
            }
            else
            {
                strSql.Clear();
                strSql.Append("update v1_si_ssr set ");
                strSql.Append("Descr=@Descr,");
                strSql.Append("SiSSR=@SiSSR,");
                strSql.Append("Md=@Md,");
                strSql.Append("Wk=@Wk,");
                strSql.Append("Ts=@Ts,");
                strSql.Append("Disabled=@Disabled,");
                strSql.Append("Update_by=@uid,");
                strSql.Append("Update_dt=now()");
                strSql.Append(" where Ledger=@Ledger and Si_id=@Si_id ");
                return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Si_id = si_ssr.si_id, Descr = si_ssr.descr, SiSSR = si_ssr.siSSR, Md = si_ssr.md, Wk = si_ssr.wk, Ts = si_ssr.ts, Disabled = si_ssr.disabled, uid = this.SysUid });
            }

        }

        /// <summary>
        /// 删除费率信息
        /// </summary>
        /// <param name="si_ssr"></param>
        /// <returns></returns>
        public int DelRow(v1_si_ssrVModel si_ssr)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("delete from v1_si_ssr where Ledger=@Ledger and Si_id=@Si_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Si_id = si_ssr.si_id });
        }


    }
}
