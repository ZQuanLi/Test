using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.DataProcess
{
    public class PayEstDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public PayEstDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        public DataTable GetRateInfo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Rate_id,b.CsId,b.PStart,b.PEnd,b.Price");
            strSql.Append(" from v1_rate as a inner join v1_rate_cs as b on a.Ledger=b.Ledger and a.Rate_id=b.Rate_id");
            strSql.Append(" where a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        public DataTable GetPayEstInfo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Co_id,a.CoName,a.Attrib,a.IsDefine,a.CustName,b.BuildType,b.Area,b.Bank,c.Cic_id,c.Rate_id,");
            strSql.Append("d.Rule,d.Unit,d.UnitBase,c1.CicName as ChrgName,c1.IsCtl");
            strSql.Append(" from vp_coinfo as a left join v1_custinfobuild as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" inner join v1_custinfobuild_fee as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" left join v1_rate as d on c.Ledger=d.Ledger and c.Rate_id=d.Rate_id");
            strSql.Append(" left join v0_cic as c1 on c.Ledger=c1.Ledger and c.Cic_id=c1.Cic_id");
            strSql.Append(" where a.Ledger=@Ledger and ifnull(c.Cic_id,0)>0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }
        public int UpdatePayEst(DataTable dtSource)
        {
            DateTime fm = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime to = fm.AddMonths(1).AddDays(-1);
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update v4_pay_est set Disabled=1 where Ledger=@Ledger and Month=UNIX_TIMESTAMP(@Month)");
            int cnt = SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Month = fm });
            foreach (DataRow dr in dtSource.Rows)
            {
                cnt = cnt + UpdatePayEst(dr, fm, fm, to);
            }
            strSql.Clear();
            strSql.Append("delete from v4_pay_est where Ledger=@Ledger and Month=UNIX_TIMESTAMP(@Month) and Disabled=1 and PayAmt=0"); /*ttt*/
            cnt = cnt + SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Month = fm });
            return cnt;
        }

        private int UpdatePayEst(DataRow dr, DateTime month, DateTime payStartTime, DateTime payEndTime)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Month = month,
                Co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]),
                Cic_id = CommFunc.ConvertDBNullToInt32(dr["Cic_id"]),
                UseVal = CommFunc.ConvertDBNullToDecimal(dr["UseVal"]),
                UseAmt = CommFunc.ConvertDBNullToDecimal(dr["UseAmt"]),
                PayStartTime = payStartTime,
                PayEndTime = payEndTime,
                SysUid = this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into v4_pay_est(Ledger,Month,Co_id,Cic_id,UseVal,UseAmt,PayAmt,Status,PayStartTime,PayEndTime,Disabled,Create_by,Create_dt,Update_by,Update_dt)values");
            strSql.Append("(@Ledger,UNIX_TIMESTAMP(@Month),@Co_id,@Cic_id,@UseVal,@UseAmt,0,0,@PayStartTime,@PayEndTime,0,@SysUid,now(),@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE UseVal=@UseVal,UseAmt=@UseAmt,PayStartTime=@PayStartTime,PayEndTime=@PayEndTime,Disabled=0,Update_by=@SysUid,Update_dt=now()");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }


        public DataTable GetV4_pay_charg()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Log_id,a.Co_id,a.FirstVal,a.LastVal,a.FirstTime,a.LastTime,a.Price,a.ChargVal");
            strSql.Append(" from v4_pay_charg as a where a.Ledger=@Ledger and a.Status=0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        public int UpV4_pay_charg(long log_id, int co_id, decimal useAmt)
        {
            StringBuilder strSql = new StringBuilder();
            using (var conn1 = SQLHelper.GetDBConnection())
            {//更新数据库
                try
                {
                    conn1.Open();
                    using (var trans = conn1.BeginTransaction())
                    {
                        DataTable dtSource = this.GetCoEleAmt(conn1, trans, co_id);
                        decimal eleAmt = CommFunc.ConvertDBNullToDecimal(dtSource.Compute("sum(RdAmt)", "true"));
                        eleAmt = Math.Abs(eleAmt);
                        decimal RdCoAmt = this.GetCoRdAmt(conn1, trans, co_id);                       
                        if (RdCoAmt < eleAmt + useAmt) 
                            return  0;
                        foreach (DataRow dr in dtSource.Rows)
                        {
                            int module_id = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);
                            string moduleAddr = CommFunc.ConvertDBNullToString(dr["ModuleAddr"]);
                            int fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]);
                            decimal rdVal = Math.Abs(CommFunc.ConvertDBNullToDecimal(dr["RdVal"]));
                            decimal rdAmt = Math.Abs(CommFunc.ConvertDBNullToDecimal(dr["RdAmt"]));
                            this.UpV2Info(conn1, trans, co_id, module_id, moduleAddr, fun_id, rdVal, rdAmt);
                        }
                        this.UpV4_pay_charg(conn1, trans, log_id, co_id, useAmt);
                        trans.Commit();
                    }
                    conn1.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    conn1.Close();
                }
            }
            return 1;
        }

        private decimal GetCoRdAmt(IDbConnection conn1, IDbTransaction trans, int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select RdAmt from v1_custinfo where Ledger=@Ledger and Co_id=@Co_id");
            object obj = conn1.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id }, trans);
            return CommFunc.ConvertDBNullToDecimal(obj);
        }
        private DataTable GetCoEleAmt(IDbConnection conn1, IDbTransaction trans, int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Co_id,a.Module_id,a.ModuleAddr,a.Fun_id,a.RdVal,ROUND(a.RdVal*s1.Price,2)as RdAmt");
            strSql.Append(" from vp_v2info as a inner join vp_mdinfo as b on a.Ledger = b.Ledger and a.Co_id = b.Co_id and a.Module_id = b.Module_id and a.ModuleAddr = b.ModuleAddr");
            strSql.Append(" inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id a.FunType = 'E' and a.RdVal<0");
            DataTable dtRst = new DataTable();
            IDataReader reader = conn1.ExecuteReader(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id }, trans);
            dtRst.Load(reader);
            return dtRst;
        }
        private int UpV2Info(IDbConnection conn1, IDbTransaction trans, int co_id, int module_id, string moduleAddr, int fun_id, decimal chargVal,decimal chargAmt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v2_info set ChrgVal=ChrgVal+@ChrgVal,RdVal=RdVal+@ChrgVal where Ledger=@Ledger and Co_id=@Co_id and Module_id=@Module_id and ModuleAddr=@ModuleAddr and Fun_id=@Fun_id;");
            strSql.Append("update v1_custinfo set RdAmt=RdAmt-@ChargAmt where  Ledger=@Ledger and Co_id=@Co_id;");
            return conn1.Execute(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id, Module_id = module_id, ModuleAddr = moduleAddr, Fun_id = fun_id, ChrgVal = chargVal, ChargAmt= chargAmt }, trans);
        }

        private int UpV4_pay_charg(IDbConnection conn1, IDbTransaction trans,long log_id, int co_id, decimal useAmt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v4_pay_charg set Status=1,PayTime=now() where Log_id=@Log_id;");
            strSql.Append("update v1_custinfo set RdAmt=RdAmt-@ChargAmt where Ledger=@Ledger and Co_id=@Co_id;");
            return conn1.Execute(strSql.ToString(), new { Ledger = this.Ledger, Log_id= log_id, Co_id = co_id, ChargAmt = useAmt }, trans);
        }
        //public decimal GetCoRdAmt(int co_id)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select RdAmt from v1_custinfo where Ledger=@Ledger and Co_id=@Co_id");
        //    object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
        //    return CommFunc.ConvertDBNullToDecimal(obj);
        //}
    }
}
