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
    public partial class ExpRateDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public ExpRateDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取费率信息
        /// </summary>
        /// <param name="Descr">筛选条件：费率描述</param>
        /// <param name="rate_id">费率ID号</param>
        /// <returns></returns>
        public DataTable GetYdRateList(int Rate_id, string Descr)
        {
            if (string.IsNullOrEmpty(Descr) || Descr == "{Descr}" || Descr == "null")
                Descr = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select Rate_id,Descr,Pri1st,Pri2nd,Pri3rd,Pri4th,Disabled,DataCfg");
            strSql.Append(" from v1_rate where Ledger=@Ledger and Descr like @Descr and Attrib=0");
            if (Rate_id != 0)
                strSql.Append(" and Rate_id=@Rate_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Rate_id = Rate_id, Descr = "%" + Descr + "%" });
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
        public int SaveYdRate(v1_rateVModel rv)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = null;
            if (rv.Rate_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Rate_id) as id from v1_rate where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                rv.Rate_id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["id"]) + 1;
            }
            strSql.Clear();
            strSql.Append("insert into v1_rate(Ledger,Rate_id,Descr,Pri1st,Pri2nd,Pri3rd,Pri4th,Disabled,DataCfg,Create_by,Create_dt,Update_by,Update_dt)values");
            strSql.Append("(@Ledger,@Rate_id,@Descr,@Pri1st,@Pri2nd,@Pri3rd,@Pri4th,0,@DataCfg,@SysUid,now(),@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE Descr=@Descr,Pri1st=@Pri1st,Pri2nd=@Pri2nd,Pri3rd=@Pri3rd,Pri4th=@Pri4th,DataCfg=@DataCfg,Update_by=@SysUid,Update_dt=now()");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Rate_id = rv.Rate_id, Descr = rv.Descr, Pri1st = rv.Pri1st, Pri2nd = rv.Pri2nd, Pri3rd = rv.Pri3rd, Pri4th = rv.Pri4th, DataCfg = rv.DataCfg, SysUid = this.SysUid });
        }

        /// <summary>
        /// 删除费率信息
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <returns></returns>
        public int GetDelYdRate(int Rate_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("delete from v1_rate where Ledger=@Ledger and Rate_id=@Rate_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Rate_id = Rate_id });
        }


    }
}
