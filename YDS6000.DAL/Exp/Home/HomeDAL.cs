using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Exp.Home
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
        /// 获取设备数量
        /// </summary>
        /// <returns></returns>
        public DataTable GetEquCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.MeterName,a.IsDefine");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.GwDisabled=0 and a.EpDisabled=0 and a.MeterDisabled=0 and a.MdDisabled=0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        public DataTable GetEquAlarm()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.MeterName,a.IsDefine,b.Log_id");
            strSql.Append(" from vp_mdinfo as a inner join v2_alarm_log as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and a.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and b.CDate BETWEEN @CDate and @CDate");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger , CDate = DateTime.Now.ToString("yyyy-MM-dd",System.Globalization.DateTimeFormatInfo.InvariantInfo)});
        }

        /// <summary>
        /// 获取最新一条告警
        /// </summary>
        /// <returns></returns>
        public DataTable GetAlarmFisrt()
        {
            //1.U 越高限，地点：8101房间10N表，当前值：230V
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Log_id,a.Content,a.CollectValue,b.MeterName from v2_alarm_log as a left join vp_mdinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            strSql.Append(" where a.Ledger=@Ledger ");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            strSql.Append(" order by a.Log_id desc limit 1");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, AreaPowerStr = AreaPowerStr });
            //    object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger });
            //return CommFunc.ConvertDBNullToString(obj);
        }
        /// <summary>
        /// 项目导航
        /// </summary>
        /// <returns></returns>
        public DataTable GetProjectList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Co_id,a.CoName,a.CustAddr,b.Area,b.BuildType,a.Gps");
            strSql.Append(" from vp_coinfo as a");
            strSql.Append(" left join v1_custinfobuild as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Attrib=@Attrib and a.Disabled=0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib= (int)CoAttribV2_1.Project });
        }
        public DataTable GetAlarmCompare()
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Log_id,a.CDate,a.Module_id,a.Content");
            strSql.Append(" from v2_alarm_log as a ");
            strSql.Append(" inner join vp_funinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and a.Fun_id=b.Fun_id");
            strSql.Append(" where a.CDate>=@CDate and a.Ledger=@Ledger");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, CDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1), AreaPowerStr = AreaPowerStr });
        }
    }
}
