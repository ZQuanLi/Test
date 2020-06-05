using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;


namespace YDS6000.DAL.Energy.Monitor
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
        /// 获取最新采集列表数据
        /// </summary>
        /// <param name="meter_id"></param>
        /// <returns></returns>
        public DataTable GetModuleOfMapCollect(int meter_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Co_id,a.Module_id,a.ModuleAddr,a.Fun_id,a.LastVal,a.LastTime,md.MeterName,md.ModuleName,b.FunType,b.FunName,c.TagName");
            strSql.Append(" from v2_info as a inner join vp_funinfo as b on a.Ledger=b.Ledger and a.Co_id = b.Co_id and a.Module_id = b.Module_id and a.ModuleAddr = b.ModuleAddr and a.Fun_id = b.Fun_id");
            strSql.Append(" inner join vp_mdinfo as md on a.Ledger=md.Ledger and a.Module_id=md.Module_id");
            strSql.Append(" left join v1_map as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id and a.Fun_id=c.Fun_id");
            strSql.Append(" left join vp_coinfo as d on a.Ledger=d.Ledger and a.Co_id=d.Co_id");
            strSql.Append(" where a.Ledger = @Ledger and b.Meter_id=@Meter_id and b.IsChrg = 1");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = meter_id });
        }

        /// <summary>
        /// 保存更换电表
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="fun_id">采集ID号</param>
        /// <param name="lastVal">最后值</param>
        /// <returns></returns>
        public int UpdateLastVal(int module_id, int fun_id, decimal lastVal)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v2_info as a inner join vp_funinfo as b on a.Ledger=b.Ledger and a.Co_id = b.Co_id and a.Module_id = b.Module_id and a.ModuleAddr = b.ModuleAddr and a.Fun_id = b.Fun_id");
            strSql.Append(" set a.LastVal=TRUNCATE(@LastVal, 2),a.YT_id=a.YT_id+1");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and a.Fun_id=@Fun_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, Fun_id = fun_id, LastVal = lastVal });
        }
    }
}
