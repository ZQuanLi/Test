using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Dapper;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Exp.Charge
{
    partial class ChargeDAL
    {
        /// <summary>
        /// 获取设备预付费信息
        /// </summary>
        /// <param name="CoStrcName"></param>
        /// <param name="CoName"></param>
        /// <returns></returns>
        public DataTable GetYdPrePayInMdOnList(int Module_id, string CoStrcName, string CoName,int co_id)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            //
            StringBuilder strSql = new StringBuilder();
            string ids = "";
            if ("{strcName}".Equals(CoStrcName))
                CoStrcName = "";
            if ("{coName}".Equals(CoName))
                CoName = "";
            if (co_id != 0)
            {
                strSql.Clear();
                strSql.Append("select GetCoChildList(@Ledger,@Co_id)as ids");
                object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
                ids = CommFunc.ConvertDBNullToString(obj);
            }
            //
            object params01 = new {
                Ledger=this.Ledger,
                Module_id=Module_id,
                CoStrcName="%"+CoStrcName+"%",
                CoName="%"+CoName+"%",
                MdItems= WHoleDAL.MdItems,
                AreaPowerStr = AreaPowerStr,
                Ids = ids,
            };
            strSql.Clear();
            //strSql.Append("select a.Module_id,a.ModuleAddr,a.Co_id,a.Multiply,b.LastVal as CurVal,");
            //strSql.Append("round(b.ChrgVal * a.Multiply,2)as ChrgVal,round((b.LastVal-b.FirstVal),2) * a.Multiply as UseVal,");
            //strSql.Append("round(b.RdVal * a.Multiply,2) as RdVal,c1.CoStrcName,c1.CoName,s1.Price,a.IsCharg");
            //strSql.Append(" from vp_mdinfo as a");
            //strSql.Append(" left join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and a.Mm_id=b.Mm_id and b.FunType='E'");
            //strSql.Append(" left join vp_coinfo as c1 on a.Ledger=c1.Ledger and a.Co_id=c1.Co_id");
            //strSql.Append(" left join syscont as s1 on a.Ledger=s1.Ledger");
            //strSql.Append(" where a.Ledger=@Ledger and a.ModuleType!='YDH20D'");
            strSql.Append("select a.Module_id,a.MeterName,vc.Fun_id,a.ModuleAddr,a.Co_id,a.Multiply,b.LastVal as CurVal,round(b.ChargVal * a.Multiply,2)as ChrgVal,");
            strSql.Append("round((b.LastVal-b.FirstVal),2) * a.Multiply as UseVal,round(b.RdVal * a.Multiply,2) as RdVal,");
            strSql.Append("c1.CoStrcName,c1.CoName,round(IF(ifnull(d.price,0)>0,d.price,e.price),3) Price,a.IsCharg,");
            strSql.Append("round(b.ZsVal * a.Multiply,2) as ZsVal,round(b.SyZsVal * a.Multiply,2)as SyZsVal");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" inner join syscont e on a.Ledger=e.Ledger");
            strSql.Append(" inner join vp_funinfo as vc on a.Ledger=vc.Ledger and a.Module_id=vc.Module_id and vc.FunType='E'");
            strSql.Append(" left join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and a.Mm_id=b.Mm_id and vc.Fun_id=b.Fun_id and b.Hist_id=0");
            strSql.Append(" left join vp_coinfo as c1 on a.Ledger=c1.Ledger and a.Co_id=c1.Co_id");
            strSql.Append(" left join v1_gateway_esp_module_info as d on a.Ledger=d.Ledger and a.Module_id=d.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and a.ModuleType!='YDH20D'");
            strSql.Append(" and a.ModuleAddr=case when e.Zb=0 then a.MeterAddr else a.ModuleAddr end");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            if (Module_id != 0)
                strSql.Append(" and a.Module_id=@Module_id");
            strSql.Append(" and ifnull(c1.CoStrcName,'') like @CoStrcName ");
            strSql.Append(" and ifnull(c1.CoName,'') like @CoName ");
            if (!string.IsNullOrEmpty(WHoleDAL.MdItems))
                strSql.Append(" and FIND_IN_SET(a.IsDefine,@MdItems)");
            if (!string.IsNullOrEmpty(ids))
                strSql.Append(" and FIND_IN_SET(a.Co_id,@Ids)");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        /// <summary>
        /// 获取明细信息
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Module_id"></param>
        /// <param name="ModuleAddr"></param>
        /// <returns></returns>
        public DataTable GetYdPrePayInMdOnV2Info(int Co_id, int Module_id, string ModuleAddr)
        {
            object params01 = new 
            {
               Ledger= this.Ledger,
               Co_id= Co_id,      
               Module_id= Module_id,      
               ModuleAddr= ModuleAddr,      
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select round(b.LastVal * a.Multiply, 2)as LastVal,b.LastTime,");
            strSql.Append("b.LastVal as CurVal,b.RdVal * a.Multiply as RdVal,b.DebtTime,round(b.ChargVal * a.Multiply,2)as ChrgVal,");
            strSql.Append("b.ZsVal  * a.Multiply as ZsVal,b.SyZsVal * a.Multiply as SyZsVal");
            strSql.Append(" from vp_mdinfo as a inner join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and b.FunType='E' and b.Hist_id=0");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and a.ModuleAddr=@ModuleAddr and b.Co_id=@Co_id");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        /// <summary>
        /// 更新付款信息
        /// </summary>
        /// <param name="pay"></param>
        public long YdPrePayInMdOnChrg(v4_pay_logVModel pay)
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
                            this.YdPrePayInMdOnRefund(conn1, trans, pay.Co_id, pay.PayVal);
                        }
                        cmd_log_id = this.YdPrePayInMdOnChangeV2Info(conn1, trans, pay.Co_id, pay.Module_id, pay.ModuleAddr, pay.PayVal, pay.PayType);/*更新设备用电信息*/
                        this.YdPrePayInMdOnChangePayLog(conn1, trans, pay);
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
        //

        private void YdPrePayInMdOnRefund(IDbConnection conn1, IDbTransaction trans, int Co_id, decimal PayVal)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Co_id = Co_id,
                PayVal = PayVal,
                SysUid = this.SysUid,
                Month = 99999999999,
                MthPayVal = (decimal)0
            };
            //
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            //
            decimal allVal = PayVal;
            while (allVal > 0)
            {
                int mm = 0;
                decimal mthPayVal = 0;
                strSql.Clear();
                strSql.Append("select Month,MthPayVal from v4_billmonth where Ledger=@Ledger and Co_id=@Co_id and Month<UNIX_TIMESTAMP(@Month) ORDER BY `Month` DESC LIMIT 1");                
                dt = SQLHelper.Query(strSql.ToString(), params01);
                foreach (DataRow dr in dt.Rows)
                {
                    mm = CommFunc.ConvertDBNullToInt32(dr["Month"]);
                    mthPayVal = CommFunc.ConvertDBNullToDecimal(dr["MthPayVal"]);
                }
                if (mm == 0) break; 

                decimal backVal = allVal > mthPayVal ? mthPayVal : allVal;
                params01 = new
                {
                    Ledger = this.Ledger,
                    Co_id = Co_id,
                    PayVal = PayVal,
                    SysUid = this.SysUid,
                    Month = mm,
                    MthPayVal = backVal
                };
                //
                strSql.Clear();
                strSql.Append("update v4_billmonth set MthPayVal=MthPayVal-@MthPayVal where Ledger=@Ledger and Month=@Month and Co_id=@Co_id");
                conn1.Execute(strSql.ToString(), params01, trans);
                allVal = allVal - backVal;
            }
            strSql.Clear();
            strSql.Append("update v4_bill set PayVal=case when PayVal-@PayVal<0 then 0 else PayVal-@PayVal end where Ledger=@Ledger and Co_id=@Co_id");
            conn1.Execute(strSql.ToString(), params01, trans);
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
               Ledger= this.Ledger,
               Month= new DateTime(DateTime.Now.Year,DateTime.Now.Month,1),
               Co_id= Co_id,  
               PayVal= PayVal,
               SysUid= this.SysUid,
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
                Ledger= this.Ledger,
                Co_id= Co_id,  
                PayVal= PayVal,
                SysUid= this.SysUid,
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
        private long YdPrePayInMdOnChangeV2Info(IDbConnection conn1, IDbTransaction trans, int Co_id, int Module_id, string ModuleAddr, decimal PayVal,int payType)
        {
            object params01 = new 
            {
               Ledger= this.Ledger,
               Co_id= Co_id,      
               Module_id= Module_id,      
               ModuleAddr= ModuleAddr,      
               FunType= (payType==1 ? V0Fun.Pay.ToString() : V0Fun.Refund.ToString()),
               SysUid= this.SysUid,
               Fun_id= 0,
               PayVal= (decimal)0,
               DataValue= (decimal)0,
               DebtTime= DBNull.Value,
            };
            StringBuilder strSql = new StringBuilder();
            DataTable dt = new DataTable();
            decimal chrgVal = 0, rdVal = 0, multiply=0;
            DateTime? debtTime = null;
            int fun_id = 0, isCharg = 0;
            strSql.Clear();
            strSql.Append("select vf.Fun_id,b.ChargVal,b.RdVal,b.DebtTime,a.Multiply,a.IsCharg");
            //strSql.Append(" from vp_mdinfo as a left join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and b.FunType='E'");
            strSql.Append(" from vp_mdinfo as a inner join v0_fun as vf on a.Ledger=vf.Ledger and a.Mm_id=vf.Mm_id and vf.FunType='E'");
            strSql.Append(" left join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and vf.Fun_id=b.Fun_id and b.Hist_id=0");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id");
            dt = SQLHelper.Query(strSql.ToString(), params01);
            foreach (DataRow dr in dt.Rows)
            {
                fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]);
                chrgVal = CommFunc.ConvertDBNullToDecimal(dr["ChargVal"]);
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
               Ledger= this.Ledger,
               Co_id= Co_id,      
               Module_id= Module_id,      
               ModuleAddr= ModuleAddr,      
               FunType= (payType==1 ? V0Fun.Pay.ToString() : V0Fun.Refund.ToString()),
               SysUid= this.SysUid,
               Fun_id= fun_id,
               PayVal= payVal,
               DataValue= dataValue,
               DebtTime= debtTime,
            };
            //
            strSql.Clear();
            strSql.Append("INSERT INTO v2_info (Ledger, Co_id, Module_id, ModuleAddr, Fun_id, Hist_id,YT_id, BeginVal, FirstVal, LastVal, MinVal, MaxVal, FirstTime, LastTime, MinTime ,MaxTime,");
            strSql.Append("LastVal1st,LastVal2nd,LastVal3rd,LastVal4th,ChargVal,RdVal,DebtTime,Create_by, Create_dt, Update_by, Update_dt)");
            strSql.Append("VALUES (@Ledger, @Co_id, @Module_id, @ModuleAddr, @Fun_id, 0,0, 0, 0, 0, 0, 0, '1900-01-01', '1900-01-01', '1900-01-01', '1900-01-01', ");
            strSql.Append("0, 0, 0, 0, case when @PayVal <=0 then 0 else @PayVal end,  @PayVal ,null, @SysUid, NOW(), @SysUid, NOW())");
            strSql.Append("ON DUPLICATE KEY UPDATE ChargVal=case when ChargVal+@PayVal<=0 then 0 else ChargVal+@PayVal end,RdVal= RdVal+@PayVal,DebtTime=@DebtTime");
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
               Log_id= pay.Log_id,
               ErrCode= pay.ErrCode,      
               ErrTxt= pay.ErrTxt,      
               PayTxt= pay.PayTxt,
               SysUid= this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v4_pay_log set ErrCode=@ErrCode,ErrTxt=@ErrTxt,EndTime=now(),PayTxt=@PayTxt where Log_id=@Log_id");
            return conn1.Execute(strSql.ToString(), params01, trans);
        }

        public DataTable SendSms(long log_id)
        {
            object params01 = new 
            {
               Ledger= this.Ledger,
               Log_id=log_id,
               SysUid= this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct a.PayAmt,d.MPhone,b.ModuleName");
            strSql.Append(" from v4_pay_log as a");
            strSql.Append(" inner join vp_mdinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            strSql.Append(" inner join v3_userinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" inner join v3_user as d on c.Ledger=d.Ledger and c.Crm_id=d.Crm_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Log_id=@Log_id and a.PayType=1");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        /*这款用于河北工业大学的SB设备，特殊定制化*/
        public DataTable GetGwPayData(int module_id)
        {
            object params01 = new 
            {
               Ledger= this.Ledger,
               Module_id=module_id,
               SysUid= this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Esp_id,a.ComPort,a.Baud,a.DataBit,a.Parity,a.StopBit,");
            strSql.Append("a.Ip,a.TcpPort,a.TimeOut,a.HandledBY,a.ModuleType,a.ModulePwd,a.ModuleUid,a.TransferType,");
            strSql.Append("a.Module_id,a.ModuleAddr,a.Fun_id,a.FunName,a.FunType,a.Protocol,a.Co_id,a.Action,a.LpszDbVarName,");
            strSql.Append("b.RdVal,b.SyZsVal");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" inner join vp_v2info b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and a.Co_id=b.Co_id and a.Fun_id=b.Fun_id and b.FunType='E'");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and a.ModuleType='YD-ZB20-4PL2-SB'");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        /// <summary>
        /// 赠送电(临时使用，又是一个演示的坑)
        /// </summary>
        /// <param name="pay"></param>
        /// <returns></returns>
        public long YdPrePayInMdOnTmp(v4_pay_logVModel pay)
        {
            object params01 = new 
            {
               Ledger= this.Ledger,
               Co_id= pay.Co_id,
               Module_id= pay.Module_id,
               ModuleAddr= pay.ModuleAddr,
               Fun_id= pay.Fun_id,
               PayVal= pay.PayVal,
               SysUid= this.SysUid,
            };
            long cmd_log_id = 0;
            StringBuilder strSql = new StringBuilder();
            using (var conn1 = SQLHelper.GetDBConnection())
            {//更新数据库
                try
                {
                    conn1.Open();
                    using (var trans = conn1.BeginTransaction())
                    {
                        strSql.Clear();
                        if (pay.PayType == 3)
                        {/*赠电*/
                            strSql.Append("update v2_info set BaseVal=BaseVal+@PayVal,RaVal=RaVal+@PayVal where Ledger=@Ledger and Co_id=@Co_id and Module_id=@Module_id and ModuleAddr=@ModuleAddr and Fun_id=@Fun_id");
                        }
                        else if (pay.PayType == 4)
                        {
                            strSql.Append("update v2_info set BaseVal=case when BaseVal-@PayVal < 0 then 0 else BaseVal-@PayVal end,");
                            strSql.Append("RaVal = case when RaVal-@PayVal<0 then 0 else RaVal-@PayVal end where Ledger=@Ledger and Co_id=@Co_id and Module_id=@Module_id and ModuleAddr=@ModuleAddr and Fun_id=@Fun_id");
                        }
                        conn1.Execute(strSql.ToString(), params01, trans);

                        this.YdPrePayInMdOnChangePayLog(conn1, trans, pay);
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
        /// 清零（）
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <returns></returns>
        public int YdPrepPayInMd_Reset(int module_id, int fun_id)
        {
            object params01 = new 
            {
               Ledger= this.Ledger,
               Module_id= module_id,
               Fun_id= fun_id,
               SysUid= this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select max(a.Hist_id)as Hist_id from v2_info as a where a.Ledger=@Ledger and a.Module_id=@Module_id and a.Fun_id=@Fun_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            int hist_id = CommFunc.ConvertDBNullToInt32(obj) + 1;
            //
            object params02 = new
            {
                Ledger = this.Ledger,
                Module_id = module_id,
                Fun_id = fun_id,
                Hist_id = hist_id,
                SysUid = this.SysUid,
            };
            //
            strSql.Clear();
            strSql.Append("START TRANSACTION;");
            strSql.Append("insert into v2_info(`Ledger`, `Co_id`, `Module_id`, `ModuleAddr`, `Fun_id`, `Hist_id`, `YT_id`,");
            strSql.Append("`BeginVal`, `FirstVal`, `LastVal`, `MinVal`, `MaxVal`, `FirstTime`, `LastTime`, `MinTime`, `MaxTime`, `DataCfg`,");
            strSql.Append("`FirstVal1st`,`FirstVal2nd`, `FirstVal3rd`, `FirstVal4th`, `LastVal1st`, `LastVal2nd`, `LastVal3rd`, `LastVal4th`, `ChargVal`, `RdVal`, `DebtTime`,");
            strSql.Append("`Create_by`, `Create_dt`, `Update_by`, `Update_dt`, `BaseVal`, `RaVal`, `InVal`, `Remark`)");
            strSql.Append(" select a.Ledger,a.Co_id,a.Module_id,a.ModuleAddr,a.Fun_id,@Hist_id,a.YT_id,");
            strSql.Append(" a.BeginVal,a.FirstVal,a.LastVal,a.MinVal,a.MaxVal,a.FirstTime,a.LastTime,a.MinTime,a.MaxTime,a.DataCfg,");
            strSql.Append(" a.FirstVal1st,a.FirstVal2nd,a.FirstVal3rd,a.FirstVal4th,a.LastVal1st,a.LastVal2nd,a.LastVal3rd,a.LastVal4th,a.ChargVal,a.RdVal,DebtTime,");
            strSql.Append(" @SysUid, now(), @SysUid, now(), a.BaseVal,a.RaVal,a.InVal,'清零'");
            strSql.Append(" from v2_info as a inner join vp_funinfo as b on a.Ledger=b.Ledger and a.Co_id = b.Co_id and a.Module_id = b.Module_id and a.ModuleAddr = b.ModuleAddr and a.Fun_id = b.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and a.Fun_id=@Fun_id and a.Hist_id=0;");
            //
            strSql.Append("update v2_info as a inner join vp_funinfo as b on a.Ledger=b.Ledger and a.Co_id = b.Co_id and a.Module_id = b.Module_id and a.ModuleAddr = b.ModuleAddr and a.Fun_id = b.Fun_id");
            strSql.Append(" set a.ChargVal=0,a.RdVal=0,a.BaseVal=0,a.RaVal=0,FirstVal=LastVal,YT_id=YT_id+1");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and a.Fun_id=@Fun_id and a.Hist_id=0;");
            strSql.Append("COMMIT;");
            return SQLHelper.Execute(strSql.ToString(), params02);
        }

        /// <summary>
        /// 打印缴费明细
        /// </summary>
        /// <param name="Log_id"></param>
        /// <returns></returns>
        public DataTable GetPayList(long Log_id)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Log_id = Log_id
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Log_id,a.CDate,a.Co_id,a.Module_id,a.ModuleAddr,b.CoStrcName,b.CoName,b.CoFullName,c.Multiply,s3.Price,");
            strSql.Append("a.PayAmt,a.PayVal,a.PayType,a.IsWrong,a.IsPay,a.Trade_no,case when a.IsPay=0 then s2.UName else s1.CrmName end as Create_by,");
            strSql.Append("a.Create_dt,a.ErrCode,a.EndTime,a.ErrTxt");
            strSql.Append(" FROM v4_pay_log as a");
            strSql.Append(" left join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" LEFT JOIN vp_mdinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" left join v3_user as s1 on a.Ledger=s1.Ledger and a.Create_by=s1.Crm_id");
            strSql.Append(" left join sys_user as s2 on a.Ledger=s2.Ledger and a.Create_by=s2.Uid");
            strSql.Append(" LEFT JOIN syscont as s3 on a.Ledger=s3.Ledger");
            strSql.Append(" WHERE a.Ledger=@Ledger and a.Log_id=@Log_id and a.IsWrong=0");
            return SQLHelper.Query(strSql.ToString(), params01);
        }
    }
}
