using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.ExpApp.Energy
{
    public class EnergyDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        //private string Project = "";
        public EnergyDAL(int ledger, int uid)
        {
            //this.Project = project;
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 本月用电信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DataTable GetMonthUseVal(int module_id)
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
            strSql.Append(" WHERE a.Ledger=@Ledger and d.Module_id=@module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id });
        }

        /// <summary>
        /// 支付购电
        /// </summary>
        /// <param name="crm_id"></param>
        /// <returns></returns>
        public DataTable GetPayUseVal(int module_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT d.Module_id,d.ModuleAddr,d.Co_id,d.Fun_id,d.FunType,d.Multiply,d.Scale,case when ifnull(vm.Price,0)=0 then s1.Price else vm.Price end as Price,");
            strSql.Append("v2.LastVal,v2.RdVal,c.CoFullName");
            strSql.Append(" from v3_user as a");
            strSql.Append(" inner join v3_userinfo as b on a.Ledger=b.Ledger and a.Crm_id=b.Crm_id");
            strSql.Append(" inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" inner join vp_coinfo as c on b.Ledger=c.Ledger and b.Co_id=c.Co_id");
            strSql.Append(" inner join vp_funinfo as d on b.Ledger=d.Ledger and b.Co_id=d.Co_id and d.FunType='E'");
            strSql.Append(" left join v1_gateway_esp_module_info as vm on d.Ledger=vm.Ledger and d.Module_id=vm.Module_id");
            strSql.Append(" left join vp_v2info as v2 on d.Ledger=v2.Ledger and d.Co_id=v2.Co_id and d.Module_id=v2.Module_id and d.ModuleAddr=v2.ModuleAddr and d.Fun_id=v2.Fun_id");
            strSql.Append(" WHERE a.Ledger=@Ledger and d.Module_id=@Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id });
        }

        /// <summary>
        /// 购电记录
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataTable GetPayLog(int module_id, DateTime startTime, DateTime endTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Log_id,a.Create_dt,a.PayVal,a.PayAmt,a.IsPay");
            strSql.Append(" from v4_pay_log as a");
            strSql.Append(" where a.Ledger=@Ledger and a.CDate>=@StartTime and a.CDate<=@EndTime and a.Module_id=@Module_id and a.ErrCode = 1 and a.PayType = 1");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, StartTime = startTime, endTime = endTime });
        }

        /// <summary>
        /// 增加记录log
        /// </summary>
        /// <param name="pay"></param>
        /// <returns></returns>
        public bool AddPayLog(v4_pay_logVModel pay)
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
            };
            StringBuilder strSql = new StringBuilder();
            //
            strSql.Clear();
            strSql.Append("insert into v4_pay_log(");
            strSql.Append("Ledger,CDate,Co_id,Module_id,ModuleAddr,Fun_id,PayVal,PayAmt,PayType,IsWrong,Create_by,Create_dt,IsPay,Trade_no,PayStartTime,PayEndTime,IsCharg,Price,ErrCode,ErrTxt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,now(),@Co_id,@Module_id,@ModuleAddr,@Fun_id,@PayVal,@PayAmt,@PayType,@IsWrong,@SysUid,now(),@IsPay,@Trade_no,@PayStartTime,@PayEndTime,@IsCharg,@Price,@ErrCode,@ErrTxt);");
            strSql.Append("select last_insert_id();");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            pay.Log_id = CommFunc.ConvertDBNullToLong(obj);
            if (pay.Log_id > 0) return true;
            else return false;
        }
    }
}
