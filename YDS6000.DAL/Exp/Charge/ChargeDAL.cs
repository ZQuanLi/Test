using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YDS6000.Models;
using DBUtility;

namespace YDS6000.DAL.Exp.Charge
{
    public partial class ChargeDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";

        //private string SplitModuleTypeOnDqhz = "YdSep2";/*电气火灾类型*/
        public ChargeDAL(string project, int ledger, int uid)
        {
            this.Project = project;
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 增加记录log
        /// </summary>
        /// <param name="pay"></param>
        /// <returns></returns>
        public bool AddV4_pay_log(v4_pay_logVModel pay)
        {
            object params01 = new{
					Ledger= this.Ledger,
					CDate= DateTime.Now,
					Co_id= pay.Co_id,
					Module_id= pay.Module_id,
					ModuleAddr= pay.ModuleAddr,
                    Fun_id= pay.Fun_id,
                    PayVal= pay.PayVal,
					PayAmt= pay.PayAmt,
					PayType= pay.PayType,
					IsWrong= pay.IsWrong,
					IsPay= pay.IsPay,
					Trade_no= pay.Trade_no,
					PayStartTime= pay.PayStartTime,
					PayEndTime= pay.PayEndTime,
					IsCharg=pay.IsCharg,
					Price=pay.Price,
                    ErrCode=pay.ErrCode,
                    ErrTxt=pay.ErrTxt,
                    SysUid=this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            //
            strSql.Clear();
            strSql.Append("insert into v4_pay_log(");
            strSql.Append("Ledger,CDate,Co_id,Module_id,ModuleAddr,Fun_id,PayVal,PayAmt,PayType,IsWrong,Create_by,Create_dt,IsPay,Trade_no,PayStartTime,PayEndTime,IsCharg,Price,ErrCode,ErrTxt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@CDate,@Co_id,@Module_id,@ModuleAddr,@Fun_id,@PayVal,@PayAmt,@PayType,@IsWrong,@SysUid,now(),@IsPay,@Trade_no,@PayStartTime,@PayEndTime,@IsCharg,@Price,@ErrCode,@ErrTxt);");
            strSql.Append("select last_insert_id();");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            pay.Log_id = CommFunc.ConvertDBNullToLong(obj);
            if (pay.Log_id > 0) return true;
            else return false;
        }

        /// <summary>
        /// 获取付费历史记录
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        public v4_pay_logVModel GetV4_pay_log(long log_id)
        {
            object params01 = new 
            {
                Ledger=this.Ledger,
                Log_id=log_id,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Log_id,CDate,Co_id,Module_id,ModuleAddr,Fun_id,PayVal,PayAmt,PayType,IsWrong,Create_by,Create_dt,IsPay,Trade_no,PayStartTime,PayEndTime,IsCharg");
            strSql.Append(" from v4_pay_log where Log_id=@Log_id");
            DataTable dt = SQLHelper.Query(strSql.ToString(), params01);
            v4_pay_logVModel payView = new v4_pay_logVModel();
            if (dt.Rows.Count == 1)
                payView = ModelHandler<v4_pay_logVModel>.FillModel(dt.Rows[0]);
            return payView;
        }

        public int UpV4_pay_log(v4_pay_logVModel pay)
        {
            object params01 = new
            {
                Log_id = pay.Log_id,
                Ledger = this.Ledger,
                Co_id = pay.Co_id,
                ErrCode = pay.ErrCode,
                ErrTxt = pay.ErrTxt,
                PayTxt = pay.PayTxt,
                SysUid = this.SysUid,
                PayAmt = pay.PayAmt,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v4_pay_log set ErrCode=@ErrCode,ErrTxt=@ErrTxt,EndTime=now(),PayTxt=@PayTxt where Log_id=@Log_id;");
            strSql.Append("update v1_custinfo set Total=Total+@PayAmt,RdAmt=RdAmt+@PayAmt where Ledger=@Ledger and Co_id=@Co_id;");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }

        /// <summary>
        /// 获取充值比率
        /// </summary>
        /// <returns></returns>
        public Decimal GetRatio()
        {
            object params01 = new 
            {
               Ledger= this.Ledger,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Price from syscont");
            strSql.Append(" where Ledger=@Ledger");
            //strSql.Append("select ratio from syscont where ledger=@ledger");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            return CommFunc.ConvertDBNullToDecimal(obj);
        }

        /// <summary>
        /// 获取仪表下的房间等信息
        /// </summary>
        /// <param name="Module_id"></param>
        /// <returns></returns>
        public DataTable GetModuleInfo(int Module_id)
        {
            object params01 = new {
                Ledger=this.Ledger,
                Module_id=Module_id,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT a.Module_id,a.ModuleAddr,b.Co_id");
            strSql.Append(" FROM v2_info as a");
            strSql.Append(" INNER JOIN vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        //判断用户是否具有选择打印小票权限
        public bool GetPower(int Ledger, int Role_id,string Prog_id)
        {
            object params01 = new 
            {
                Ledger=Ledger,
                Role_id=Role_id,
                Prog_id=Prog_id
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(a.Ledger) as cnt");
            strSql.Append(" from sys_user_prog as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Role_id=@Role_id and a.Prog_id=@Prog_id");
            strSql.Append(" and a._read + a._write + a._delete + a._app > 0 ");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            return CommFunc.ConvertDBNullToInt32(obj) > 0 ? true : false;
        }

        public long GetYdPayListResult(string out_trade_no)
        {
            object params01 = new 
            {
                Ledger=Ledger,
                Trade_no=out_trade_no,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Log_id from v4_pay_log where Ledger=@Ledger and Trade_no=@Trade_no and ifnull(Trade_no,'')<>'' and ErrCode<>1");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            return CommFunc.ConvertDBNullToLong(obj);
        }
    }
}
