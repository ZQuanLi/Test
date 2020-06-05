using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.ExpApp.Estate
{
    public class EstateDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        public EstateDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }
        public string GetAlarmInfo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select ErrTxt from v2_alarm_log as a order by a.Log_id desc limit 1");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), null);
            return CommFunc.ConvertDBNullToString(obj);
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserInfo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT a.CrmName,a.CrmNo,a.Crm_id,a.Contract ,");
            strSql.Append("s1.Project,b.Co_id,c.CoFullName,d.ModuleName,d.MeterNo,c1.RdAmt");
            strSql.Append(" FROM v3_user as a");
            strSql.Append(" inner JOIN v3_userinfo as b on a.Ledger=b.Ledger and a.Crm_id=b.Crm_id");
            strSql.Append(" inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" left join vp_coinfo as c on b.Ledger=c.Ledger and b.Co_id=c.Co_id");
            strSql.Append(" left join v1_custinfo as c1 on b.Ledger=c1.Ledger and b.Co_id=c1.Co_id");
            strSql.Append(" left join vp_mdinfo as d on b.Ledger=d.Ledger and b.Co_id=d.Co_id");
            strSql.Append(" WHERE a.Ledger=@Ledger and a.Crm_id=@Crm_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = this.SysUid });
        }

        /// <summary>
        /// 账单
        /// </summary>
        /// <returns></returns>
        public DataTable GetBill()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Log_id,a.CDate,a.Co_id,a.LastVal - a.FirstVal as WaterUseVal,a.FirstTime,a.LastTime,a.Price,");
            strSql.Append("(a.LastVal-a.FirstVal)*a.Price as LastAmt,a.ChargVal,b.CoName,a.Status,a.PayTime");
            strSql.Append(" from v4_pay_charg as a inner join vp_coinfo as b on a.Ledger = b.Ledger and a.Co_id = b.Co_id");
            strSql.Append(" inner join vp_v3userinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and c.Crm_id=@SysUid");
            strSql.Append(" order by a.Log_id desc limit 2");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, SysUid = this.SysUid });
        }
        public DataTable GetMdInfo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT d.Module_id,d.Fun_id,d.FunType,d.Multiply,d.Scale,case when ifnull(vm.Price,0)=0 then s1.Price else vm.Price end as Price");
            strSql.Append(" from v3_user as a");
            strSql.Append(" inner JOIN v3_userinfo as b on a.Ledger=b.Ledger and a.Crm_id=b.Crm_id");
            strSql.Append(" inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" inner join vp_coinfo as c on b.Ledger=c.Ledger and b.Co_id=c.Co_id");
            strSql.Append(" inner join vp_funinfo as d on b.Ledger=d.Ledger and b.Co_id=d.Co_id and d.FunType='E'");
            strSql.Append(" left join v1_gateway_esp_module_info as vm on d.Ledger=vm.Ledger and d.Module_id=vm.Module_id");
            strSql.Append(" WHERE a.Ledger=@Ledger and a.Crm_id=@SysUid");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, SysUid = this.SysUid });
        }

        /// <summary>
        /// 账单
        /// </summary>
        /// <returns></returns>
        public DataTable GetBillDetail_01(DateTime start,DateTime end)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Log_id,a.CDate,a.Co_id,a.LastVal - a.FirstVal as WaterUseVal,a.FirstTime,a.LastTime,a.Price,");
            strSql.Append("(a.LastVal-a.FirstVal)*a.Price as LastAmt,a.ChargVal,b.CoName,s.Status,a.PayTime,a.SyAmt");
            strSql.Append(" from v4_pay_charg as a inner join vp_coinfo as b on a.Ledger = b.Ledger and a.Co_id = b.Co_id");
            strSql.Append(" inner join vp_v3userinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and c.Crm_id=@SysUid");
            strSql.Append(" and a.CDate>=@Start and a.CDate<=@End");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, SysUid = this.SysUid, Start = start.ToString("yyyy-MM-dd"), End = end.ToString("yyyy-MM-dd") });
        }

        /// <summary>
        /// 账单
        /// </summary>
        /// <returns></returns>
        public DataTable GetBillDetail_02(DateTime start, DateTime end)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Log_id,a.CDate,a.Co_id,PayAmt,a.Price,b.CoName,a.Create_dt as PayTime,a.SyAmt");
            strSql.Append(" from v4_pay_log as a inner join vp_coinfo as b on a.Ledger = b.Ledger and a.Co_id = b.Co_id");
            strSql.Append(" inner join vp_v3userinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and c.Crm_id=@SysUid and a.ErrCode=1");
            strSql.Append(" and a.CDate>=@Start and a.CDate<=@End");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, SysUid = this.SysUid, Start = start.ToString("yyyy-MM-dd"), End = end.ToString("yyyy-MM-dd") });
        }

        /// <summary>
        /// 增加记录log
        /// </summary>
        /// <param name="pay"></param>
        /// <returns></returns>
        public bool AddPayLog(v4_pay_logVModel pay, decimal rdAmt)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Co_id = pay.Co_id,
                Module_id = pay.Module_id,
                ModuleAddr = pay.ModuleAddr,
                Fun_id = pay.Fun_id,
                PayVal = pay.PayVal,
                PayAmt = pay.PayAmt,
                PayType = pay.PayType,
                IsWrong = pay.IsWrong,
                IsPay = pay.IsPay,
                Trade_no = pay.Trade_no,
                PayStartTime = pay.PayStartTime,
                PayEndTime = pay.PayEndTime,
                IsCharg = pay.IsCharg,
                Price = pay.Price,
                ErrCode = pay.ErrCode,
                ErrTxt = "",
                SysUid = this.SysUid,
                SyAmt = rdAmt
            };
            StringBuilder strSql = new StringBuilder();
            //
            strSql.Clear();
            strSql.Append("insert into v4_pay_log(");
            strSql.Append("Ledger,CDate,Co_id,Module_id,ModuleAddr,Fun_id,PayVal,PayAmt,PayType,IsWrong,Create_by,Create_dt,IsPay,Trade_no,PayStartTime,PayEndTime,IsCharg,Price,ErrCode,ErrTxt,SyAmt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,now(),@Co_id,@Module_id,@ModuleAddr,@Fun_id,@PayVal,@PayAmt,@PayType,@IsWrong,@SysUid,now(),@IsPay,@Trade_no,@PayStartTime,@PayEndTime,@IsCharg,@Price,@ErrCode,@ErrTxt,@SyAmt);");
            strSql.Append("select last_insert_id();");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            pay.Log_id = CommFunc.ConvertDBNullToLong(obj);
            if (pay.Log_id > 0) return true;
            else return false;
        }
    }
}
