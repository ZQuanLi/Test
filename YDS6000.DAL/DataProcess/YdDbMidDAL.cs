using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;
using DBUtility;
using Dapper;
using Dapper.Contrib.Extensions;

namespace YDS6000.DAL.DataProcess
{
    public class YdDbMidDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public YdDbMidDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        public DataTable GetDk_buy()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT count(*)as cnt FROM information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'dk_buy' LIMIT 1");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), null);
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
            {
                strSql.Clear();
                strSql.Append("select a.recno,a.FJID,a.tranamt,a.endatatime,a.custsn,a.TBID");
                strSql.Append(" from dk_buy as a");
                strSql.Append(" where a.endatatime>=@Endatatime "); //and CAST(a.TBID AS SIGNED)=0
                //strSql.Append(" and not exists(select m.Ledger from v4_pay_log as m where m.Ledger=@Ledger and m.CDate>=@Endatatime and m.Co_id=a.FJID and m.ErrCode=1 and m.Month=UNIX_TIMESTAMP(a.endatatime))");
                strSql.Append(" and not exists(select m.Ledger from v4_pay_log as m where m.Ledger=@Ledger and m.CDate>=@Endatatime and m.Co_id=a.FJID and m.ErrCode=1 and m.Recno=a.recno)");
                return SQLHelper.Query(strSql.ToString(), new {  Ledger = this.Ledger, Endatatime = DateTime.Now.AddDays(-2) });
                //return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Endatatime = new DateTime(2019, 5, 27) });

            }
            return null;
        }

        public DataTable GetPayInfo(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Module_id,a.Fun_id,a.ModuleAddr,md.ModuleName,md.Multiply,a.Co_id,ff.ChrgVal,ff.ZsVal,ff.RdVal,ff.SyZsVal,ff.LastTime,d.CoFullName,d.CoStrcName,d.CoName,");
            strSql.Append("ff.ChrgVal-(ff.LastVal-ff.FirstVal)as KtVal,ff.LastVal-ff.FirstVal as UseVal,");
            strSql.Append("round(case when ifnull(c.Price,0)=0 then b.Price else c.Price end,4) as Price,");
            strSql.Append("b.IsClosed,case when ifnull(c.AlarmVal,0)=0 then b.OdValue else c.AlarmVal end as OdValue,ey.EnergyItemName");
            strSql.Append(" from vp_funinfo as a inner join syscont as b on a.Ledger = b.Ledger and a.FunType='E'");
            strSql.Append(" inner join vp_mdinfo as md on a.Ledger=md.Ledger and a.Module_id=md.Module_id");
            strSql.Append(" inner join vp_coinfo as d on a.Ledger = d.Ledger and a.Co_id = d.Co_id");
            strSql.Append(" left join vp_v2info as ff on a.Ledger=ff.Ledger and a.Co_id=ff.Co_id and a.Module_id=ff.Module_id and a.ModuleAddr=ff.ModuleAddr and a.Fun_id=ff.Fun_id");
            strSql.Append(" left join v1_gateway_esp_module_info as c on a.Ledger = c.Ledger and a.Module_id = c.Module_id");
            strSql.Append(" left join v0_energyitemdict as ey on md.EnergyItemCode = ey.EnergyItemCode");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
        }
        /// <summary>
        /// 增加记录log
        /// </summary>
        /// <param name="pay"></param>
        /// <returns></returns>
        public bool YdDbMidAddPayLog(v4_pay_logVModel pay, DateTime endatatime, long recno)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                CDate = DateTime.Now,
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
                ErrTxt = pay.ErrTxt,
                SysUid = this.SysUid,
                Month = endatatime,
                Recno = recno,
            };
            StringBuilder strSql = new StringBuilder();
            //
            strSql.Clear();
            strSql.Append("insert into v4_pay_log(");
            strSql.Append("Ledger,CDate,Co_id,Module_id,ModuleAddr,Fun_id,PayVal,PayAmt,PayType,IsWrong,Create_by,Create_dt,IsPay,Trade_no,PayStartTime,PayEndTime,IsCharg,Price,ErrCode,ErrTxt,Month,Recno)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@CDate,@Co_id,@Module_id,@ModuleAddr,@Fun_id,@PayVal,@PayAmt,@PayType,@IsWrong,@SysUid,now(),@IsPay,@Trade_no,@PayStartTime,@PayEndTime,@IsCharg,@Price,@ErrCode,@ErrTxt,UNIX_TIMESTAMP(@Month),@Recno);");
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Log_id,CDate,Co_id,Module_id,ModuleAddr,Fun_id,PayVal,PayAmt,PayType,IsWrong,Create_by,Create_dt,IsPay,Trade_no,PayTxt,PayStartTime,PayEndTime,IsCharg");
            strSql.Append(" from v4_pay_log where Log_id=@Log_id");
            DataTable dtSource = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Log_id = log_id });
            v4_pay_logVModel payView = new v4_pay_logVModel();
            if (dtSource.Rows.Count == 1)
                payView = ModelHandler<v4_pay_logVModel>.FillModel(dtSource.Rows[0]);
            return payView;
        }

        /// <summary>
        /// 更新付款信息
        /// </summary>
        /// <param name="pay"></param>
        public long YdPrePayInMdOnChrg(v4_pay_logVModel pay,long recno)
        {
            long cmd_log_id = 0;
            using (var conn1 = SQLHelper.GetDBConnection())
            {//更新数据库
                try
                {
                    conn1.Open();
                    using (var trans = conn1.BeginTransaction())
                    {
                        if (pay.PayType == 1)
                        {
                            this.YdPrePayInMdOnChangeMthBill(conn1, trans, pay.Co_id, pay.PayVal);/*更新组织逐月用电*/
                            this.YdPrePayInMdOnChangeBill(conn1, trans, pay.Co_id, pay.PayVal);/*更新组织用电*/
                        }
                        else if (pay.PayType == 2)
                        {
                            throw new Exception("类型错误");
                        }
                        cmd_log_id = this.YdPrePayInMdOnChangeV2Info(conn1, trans, pay.Co_id, pay.Module_id, pay.ModuleAddr, pay.PayVal, pay.PayType);/*更新设备用电信息*/
                        this.YdPrePayInMdOnChangePayLog(conn1, trans, pay);
                        this.UpdateDk_buy(conn1, trans, recno, pay.Co_id, pay.PayVal);
                        trans.Commit();
                    }
                    conn1.Close();
                }
                catch (Exception ex)
                {
                    pay.ErrCode = -1;
                    pay.ErrTxt = ex.Message;
                    this.UpV4_pay_log(pay);
                    throw ex;
                }
            }
            return cmd_log_id;
        }

        /// <summary>
        /// 更新使用用能单位逐月付款信息表
        /// </summary>
        /// <param name="conn1"></param>
        /// <param name="trans"></param>
        /// <param name="Co_id"></param>
        /// <param name="PayVal"></param>
        private void YdPrePayInMdOnChangeMthBill(IDbConnection conn1, IDbTransaction trans, int Co_id, decimal PayVal)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Month = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                Co_id = Co_id,
                PayVal = PayVal,
                SysUid = this.SysUid,
            };
            //
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into v4_billmonth(Ledger,Month,Co_id,MthPayVal,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,UNIX_TIMESTAMP(@Month),@Co_id,@PayVal,@SysUid,now(),@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE MthPayVal=MthPayVal+@PayVal,Update_by=@SysUid,Update_dt=now()");

            conn1.Execute(strSql.ToString(), params01, trans);
        }

        /// <summary>
        /// 更新用用能单位付款信息表
        /// </summary>
        /// <param name="conn1"></param>
        /// <param name="trans"></param>
        /// <param name="Co_id"></param>
        /// <param name="PayVal"></param>
        private void YdPrePayInMdOnChangeBill(IDbConnection conn1, IDbTransaction trans, int Co_id, decimal PayVal)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Co_id = Co_id,
                PayVal = PayVal,
                SysUid = this.SysUid,
            };
            //
            StringBuilder strSql = new StringBuilder();

            decimal payVal = 0, useVal = 0;
            DateTime? debtTime = null;
            strSql.Clear();
            strSql.Append("select PayVal,UseVal,DebtTime from v4_bill where Ledger=@Ledger and Co_id=@Co_id");

            DataTable dt = SQLHelper.Query(strSql.ToString(), params01);
            foreach (DataRow dr in dt.Rows)
            {
                payVal = CommFunc.ConvertDBNullToDecimal(dr["PayVal"]);
                useVal = CommFunc.ConvertDBNullToDecimal(dr["UseVal"]);
                if (CommFunc.ConvertDBNullToDateTime(dr["DebtTime"]).Year != 1900)
                    debtTime = CommFunc.ConvertDBNullToDateTime(dr["DebtTime"]);
            }
            //付款金额加总，并判断是否欠费
            payVal = payVal + PayVal;
            if (payVal < useVal)
                debtTime = debtTime == null ? DateTime.Now : debtTime;
            else
                debtTime = null;
            //
            params01 = new
            {
                Ledger = this.Ledger,
                Co_id = Co_id,
                PayVal = PayVal,
                SysUid = this.SysUid,
                DebtTime = debtTime
            };
            //
            strSql.Clear();
            strSql.Append("insert into v4_bill(Ledger,Co_id,PayVal,DebtTime,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,@Co_id,@PayVal,@DebtTime,@SysUid,now(),@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE PayVal=PayVal+@PayVal,DebtTime=@DebtTime,Update_by=@SysUid,Update_dt=now()");
            conn1.Execute(strSql.ToString(), params01, trans);
        }

        /// <summary>
        /// 更新 设备采集状况表
        /// </summary>
        /// <param name="conn1"></param>
        /// <param name="trans"></param>
        /// <param name="Co_id"></param>
        /// <param name="Module_id"></param>
        /// <param name="ModuleAddr"></param>
        /// <param name="PayVal"></param>
        private long YdPrePayInMdOnChangeV2Info(IDbConnection conn1, IDbTransaction trans, int Co_id, int Module_id, string ModuleAddr, decimal PayVal, int payType)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Co_id = Co_id,
                Module_id = Module_id,
                ModuleAddr = ModuleAddr,
                FunType = (payType == 1 ? V0Fun.Pay.ToString() : V0Fun.Refund.ToString()),
                SysUid = this.SysUid,
                Fun_id = 0,
                PayVal = (decimal)0,
                DataValue = (decimal)0,
                DebtTime = DBNull.Value,
            };
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            decimal chrgVal = 0, rdVal = 0, multiply = 0;
            DateTime? debtTime = null;
            int fun_id = 0, isCharg = 0;
            strSql.Clear();
            strSql.Append("select vf.Fun_id,b.ChrgVal,b.RdVal,b.DebtTime,a.Multiply,a.IsCharg");
            //strSql.Append(" from vp_mdinfo as a left join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and b.FunType='E'");
            strSql.Append(" from vp_mdinfo as a inner join v0_fun as vf on a.Ledger=vf.Ledger and a.Mm_id=vf.Mm_id and vf.FunType='E'");
            strSql.Append(" left join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and vf.Fun_id=b.Fun_id");

            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id");
            dt = SQLHelper.Query(strSql.ToString(), params01);
            foreach (DataRow dr in dt.Rows)
            {
                fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]);
                chrgVal = CommFunc.ConvertDBNullToDecimal(dr["ChrgVal"]);
                rdVal = CommFunc.ConvertDBNullToDecimal(dr["RdVal"]);
                multiply = CommFunc.ConvertDBNullToDecimal(dr["Multiply"]);
                isCharg = CommFunc.ConvertDBNullToInt32(dr["IsCharg"]);
                if (CommFunc.ConvertDBNullToDateTime(dr["DebtTime"]).Year != 1900)
                    debtTime = CommFunc.ConvertDBNullToDateTime(dr["DebtTime"]);
            }
            //     
            if (multiply == 0)
                throw new Exception("倍率不能为零");

            decimal dataValue = PayVal / multiply;
            decimal payVal = payType == 1 ? dataValue : 0 - dataValue;
            //
            if (rdVal + payVal < 0)
                debtTime = debtTime == null ? DateTime.Now : debtTime;/*这里可能需要修改，有逻辑错误*/
            else
                debtTime = null;
            //
            params01 = new
            {
                Ledger = this.Ledger,
                Co_id = Co_id,
                Module_id = Module_id,
                ModuleAddr = ModuleAddr,
                FunType = (payType == 1 ? V0Fun.Pay.ToString() : V0Fun.Refund.ToString()),
                SysUid = this.SysUid,
                Fun_id = fun_id,
                PayVal = payVal,
                DataValue = dataValue,
                DebtTime = debtTime,
            };
            //
            strSql.Clear();
            strSql.Append("INSERT INTO v2_info (Ledger, Co_id, Module_id, ModuleAddr, Fun_id, YT_id, BeginVal, FirstVal, LastVal, MinVal, MaxVal, FirstTime, LastTime, MinTime ,MaxTime,");
            strSql.Append("Val1st,Val2nd,Val3rd,val4th,E4Time,ChrgVal,RdVal,MRdVal,DebtTime,Create_by, Create_dt, Update_by, Update_dt)");
            strSql.Append("VALUES (@Ledger, @Co_id, @Module_id, @ModuleAddr, @Fun_id, 0, 0, 0, 0, 0, 0, '1900-01-01', '1900-01-01', '1900-01-01', '1900-01-01', ");
            strSql.Append("0, 0, 0, 0, null , case when @PayVal <=0 then 0 else @PayVal end,  @PayVal  , @PayVal ,null, @SysUid, NOW(), @SysUid, NOW())");
            strSql.Append("ON DUPLICATE KEY UPDATE ChrgVal=case when ChrgVal+@PayVal<=0 then 0 else ChrgVal+@PayVal end,RdVal= RdVal+@PayVal,DebtTime=@DebtTime");
            conn1.Execute(strSql.ToString(), params01, trans);
            //
            long cmd_log_id = 0;
            if (isCharg == 1)
            {/*充值命令*/
                strSql.Clear();
                strSql.Append("insert into v2_command(");
                strSql.Append("CDate,Ledger,Co_id,Module_id,ModuleAddr,Fun_id,FunType,DataValue,Create_by,Create_dt,ErrCode)");
                strSql.Append("select now(),a.Ledger,@Co_id,a.Module_id,@ModuleAddr,ifnull(b.Fun_id,0),@FunType,@DataValue,@SysUid,now(),0");
                strSql.Append(" from vp_mdinfo as a left join v0_fun as b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id and b.FunType=@FunType");
                strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id;");
                strSql.Append("select last_insert_id();");
                object obj = conn1.ExecuteScalar(strSql.ToString(), params01, trans);
                cmd_log_id = CommFunc.ConvertDBNullToLong(obj);
            }
            return cmd_log_id;
        }

        private int YdPrePayInMdOnChangePayLog(IDbConnection conn1, IDbTransaction trans, v4_pay_logVModel pay)
        {
            object params01 = new
            {
                Log_id = pay.Log_id,
                ErrCode = pay.ErrCode,
                ErrTxt = pay.ErrTxt,
                PayTxt = pay.PayTxt,
                SysUid = this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v4_pay_log set ErrCode=@ErrCode,ErrTxt=@ErrTxt,EndTime=now(),PayTxt=@PayTxt where Log_id=@Log_id");
            return conn1.Execute(strSql.ToString(), params01, trans);
        }

        private int UpdateDk_buy(IDbConnection conn1, IDbTransaction trans, long recno, int co_id, decimal payVal)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                recno = recno,
                FJID = co_id,
                AllAmp = payVal,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dk_buy set TBID='1' where recno=@recno;");
            strSql.Append("INSERT INTO dk_use (elecdate,XQID,FJID,usedAmp,allAmp)");
            strSql.Append(" select DATE_FORMAT(now(), '%Y-%m-%d'),XQID,FJID,0,@AllAmp from dk_room where FJID=@FJID");
            strSql.Append(" ON DUPLICATE KEY UPDATE allAmp=allAmp+@AllAmp;");
            strSql.Append(" update dk_room as a");
            strSql.Append(" inner join(select a.Co_id,sum(a.RdVal) as RdVal from vp_v2info as a");
            strSql.Append(" inner join vp_mdinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and a.Co_id=b.Co_id and a.Mm_id=b.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@FJID and a.FunType = 'E' group by a.Co_id) as b on a.FJID = b.Co_id");
            strSql.Append(" set a.UsedAmp = b.RdVal");
            strSql.Append(" where a.FJID = @FJID");
            return conn1.Execute(strSql.ToString(), params01, trans);
        }
        public int UpV4_pay_log(v4_pay_logVModel pay)
        {
            object params01 = new
            {
                Log_id = pay.Log_id,
                ErrCode = pay.ErrCode,
                ErrTxt = pay.ErrTxt,
                PayTxt = pay.PayTxt,
                SysUid = this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v4_pay_log set ErrCode=@ErrCode,ErrTxt=@ErrTxt,EndTime=now(),PayTxt=@PayTxt where Log_id=@Log_id");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        public CommandVModel GetYdToGwOfSendCmd(long log_id)
        {
            object params01 = new
            {
                Log_id = log_id,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from vp_cmdinfo where Log_id=@Log_id");
            DataTable dt = SQLHelper.Query(strSql.ToString(), params01);
            CommandVModel cmd = null;
            if (dt.Rows.Count > 0)
                cmd = ModelHandler<CommandVModel>.FillModel(dt.Rows[0]);
            return cmd;
        }

        /// <summary>
        /// 获取设备后付费信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdPostPayInMdOnList(int Module_id)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Module_id = Module_id,
                MdItems = WHoleDAL.MdItems,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.Co_id,a.Multiply,b.LastVal as CurVal,round(b.ChrgVal * a.Multiply,2)as ChrgVal,");
            strSql.Append("round((b.LastVal-b.FirstVal),2) * a.Multiply as UseVal,round(b.RdVal * a.Multiply,2) as RdVal,");
            strSql.Append("c1.CoStrcName,c1.CoName,round(IF(d.price>0,d.price,e.price),3) Price,a.IsCharg,");
            strSql.Append("round(b.ZsVal * a.Multiply,2) as ZsVal,round(b.SyZsVal * a.Multiply,2)as SyZsVal,");
            strSql.Append("CAST(cfg.Rule as unsigned) as IsClosed,CAST(cfg.CfValue as DECIMAL(18, 4))as OdValue");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" inner JOIN syscont e on a.Ledger=e.Ledger");
            strSql.Append(" left join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and a.Mm_id=b.Mm_id and b.FunType='E'");
            strSql.Append(" left join vp_coinfo as c1 on a.Ledger=c1.Ledger and a.Co_id=c1.Co_id");
            strSql.Append(" left JOIN v1_gateway_esp_module_info as d on a.Ledger=d.Ledger and a.Module_id=d.Module_id");
            strSql.Append(" left join sys_config as cfg on a.Ledger=cfg.Ledger and cfg.CfKey='AlarmValOd'");
            strSql.Append(" where a.Ledger=@Ledger and a.ModuleType!='YDH20D'");
            strSql.Append(" and a.Module_id=@Module_id");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        public int GetYdToFun_id(int module_id, string funType)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Module_id = module_id,
                FunType = funType.ToString(),
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select b.Fun_id");
            strSql.Append(" from vp_mdinfo as a inner join v0_fun as b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and b.FunType=@FunType");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            return CommFunc.ConvertDBNullToInt32(obj);
        }

        /// <summary>
        /// 下发命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public long YdToGwOfAddCmd(int module_id, int fun_id, string funType, string dataValue, int isRn)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Module_id = module_id,
                Fun_id = fun_id,
                FunType = funType,
                DataValue = dataValue,
                IsRn = isRn,
                SysUid = this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into v2_command(");
            strSql.Append("CDate,Ledger,Co_id,Module_id,ModuleAddr,Fun_id,FunType,DataValue,Create_by,Create_dt,ErrCode,IsRn)");
            strSql.Append("select now(),a.Ledger,a.Co_id,a.Module_id,a.ModuleAddr,@Fun_id,@FunType,@DataValue,@SysUid,now(),0,@IsRn");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id;");
            strSql.Append("select last_insert_id();");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            return CommFunc.ConvertDBNullToLong(obj);
        }
    }
}
