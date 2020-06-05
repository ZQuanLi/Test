using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Exp.Charge
{
    partial class ChargeDAL
    {
        /// <summary>
        /// 获取设备后付费信息
        /// </summary>
        /// <param name="CoStrcName"></param>
        /// <param name="CoName"></param>
        /// <returns></returns>
        public DataTable GetYdPostPayInMdOnList(int Module_id,string CoStrcName, string CoName)
        {
            object params01 = new {
                Ledger=this.Ledger,
                Module_id=Module_id,
                CoStrcName="%"+CoStrcName+"%",
                CoName="%"+CoName+"%",
                MdItems= WHoleDAL.MdItems,
            };
            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select a.Module_id,a.ModuleAddr,a.Co_id,a.Multiply,b.LastVal as CurVal,");
            //strSql.Append("round(b.ChrgVal * a.Multiply,2)as ChrgVal,round((b.LastVal-b.FirstVal),2) * a.Multiply as UseVal,");
            //strSql.Append("round(b.RdVal * a.Multiply,2) as RdVal,c1.CoStrcName,c1.CoName,s1.Price,a.IsCharg");
            //strSql.Append(" from vp_mdinfo as a");
            //strSql.Append(" left join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and a.Mm_id=b.Mm_id and b.FunType='E'");
            //strSql.Append(" left join vp_coinfo as c1 on a.Ledger=c1.Ledger and a.Co_id=c1.Co_id");
            //strSql.Append(" left join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append("select a.Module_id,a.ModuleAddr,a.Co_id,a.Multiply,b.LastVal as CurVal,round(b.ChargVal * a.Multiply,2)as ChrgVal,");
            strSql.Append("round((b.LastVal-b.FirstVal),2) * a.Multiply as UseVal,round(b.RdVal * a.Multiply,2) as RdVal,");
            strSql.Append("c1.CoStrcName,c1.CoName,round(IF(d.price>0,d.price,e.price),3) Price,a.IsCharg,");
            strSql.Append("round(b.ZsVal * a.Multiply,2) as ZsVal,round(b.SyZsVal * a.Multiply,2)as SyZsVal,");
            strSql.Append("CAST(cfg.Rule as unsigned) as IsClosed,CAST(cfg.CfValue as DECIMAL(18, 4))as OdValue");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" inner JOIN syscont e on a.Ledger=e.Ledger");
            strSql.Append(" left join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and a.Mm_id=b.Mm_id and b.FunType='E' and b.Hist_id=0");
            strSql.Append(" left join vp_coinfo as c1 on a.Ledger=c1.Ledger and a.Co_id=c1.Co_id");
            strSql.Append(" left JOIN v1_gateway_esp_module_info as d on a.Ledger=d.Ledger and a.Module_id=d.Module_id");
            strSql.Append(" left join sys_config as cfg on a.Ledger=cfg.Ledger and cfg.CfKey='AlarmValOd'");
            strSql.Append(" where a.Ledger=@Ledger and a.ModuleType!='YDH20D'");
            if (Module_id != 0)
                strSql.Append(" and a.Module_id=@Module_id");
            strSql.Append(" and ifnull(c1.CoStrcName,'') like @CoStrcName ");
            strSql.Append(" and ifnull(c1.CoName,'') like @CoName ");
            if (!string.IsNullOrEmpty(WHoleDAL.MdItems))
                strSql.Append(" and FIND_IN_SET(a.IsDefine,@MdItems)");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        public DataTable GetNotifyForPayList(int co_id)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Co_id = co_id,
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
            strSql.Append(" and a.Co_id=@Co_id");
            if (!string.IsNullOrEmpty(WHoleDAL.MdItems))
                strSql.Append(" and FIND_IN_SET(a.IsDefine,@MdItems)");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        ///// <summary>
        ///// 获取明细信息
        ///// </summary>
        ///// <param name="Co_id"></param>
        ///// <param name="Module_id"></param>
        ///// <param name="ModuleAddr"></param>
        ///// <returns></returns>
        //internal DataTable GetYdPostPayInMdOnV2Info(int Co_id, int Module_id, string ModuleAddr)
        //{
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //       new SQLParameter("@Ledger", this.Ledger),
        //       new SQLParameter("@Co_id", Co_id),      
        //       new SQLParameter("@Module_id", Module_id),      
        //       new SQLParameter("@ModuleAddr", ModuleAddr),      
        //    };
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select round(b.LastVal * a.Multiply, 2)as LastVal,b.LastTime,");
        //    strSql.Append("b.LastVal as CurVal,b.RdVal * a.Multiply as RdVal,b.DebtTime");
        //    strSql.Append(" from vp_mdinfo as a inner join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and b.FunType='E'");
        //    strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and a.ModuleAddr=@ModuleAddr and b.Co_id=@Co_id");
        //    DataSet ds = SQLUtilities.Query(strSql.ToString(), params01);
        //    return ds.Tables[0];
        //}

        ///// <summary>
        ///// 获取逐日用电量及日缴费信息
        ///// </summary>
        ///// <param name="Co_id"></param>
        ///// <param name="Module_id"></param>
        ///// <param name="ModuleAddr"></param>
        ///// <param name="Start"></param>
        ///// <param name="End"></param>
        ///// <param name="UseVal">全部用电量</param>
        ///// <param name="ChrgVal">全部已缴费电量</param>
        ///// <returns>明细信息</returns>
        //internal DataTable GetYdPostPayInMdOnV9Day(int Co_id, int Module_id, string ModuleAddr, DateTime Start, DateTime End)
        //{
        //    DataTable dtRst = new DataTable();
        //    dtRst.Columns.Add("TagTime", typeof(System.DateTime));/*时间*/
        //    dtRst.Columns.Add("Module_id", typeof(System.Int32));/*ID号*/
        //    dtRst.Columns.Add("ModuleAddr", typeof(System.String));/*地址*/
        //    dtRst.Columns.Add("Co_id", typeof(System.Int32));  /*客户ID号*/
        //    dtRst.Columns.Add("Fun_id", typeof(System.Int32));  /*功能ID号*/
        //    dtRst.Columns.Add("YT_id", typeof(System.Int32));  /*异常ID号*/
        //    dtRst.Columns.Add("UseVal", typeof(System.Decimal));/*使用电度*/
        //    dtRst.Columns.Add("ChrgVal", typeof(System.Decimal));/*已支付电度*/
        //    dtRst.Columns.Add("Multiply", typeof(System.Decimal));/*倍率*/

        //    //////////////////////////////////////////////////////
        //    DataTable dtMap = this.GetYdPostPayInMdOnMdMap(Module_id);
        //    int fun_id = 0, scale = 0;
        //    decimal multiply = 0;
        //    if (dtMap.Rows.Count > 0)
        //    {
        //        fun_id = CommFunc.ConvertDBNullToInt32(dtMap.Rows[0]["Fun_id"]);
        //        scale = CommFunc.ConvertDBNullToInt32(dtMap.Rows[0]["Scale"]);
        //        multiply = CommFunc.ConvertDBNullToDecimal(dtMap.Rows[0]["Multiply"]);
        //    }
        //    scale = scale == 0 ? 2 : scale;
        //    //
        //    DataTable dtSource = WholeBLL.GetCoreQueryData(this.Ledger, Module_id.ToString(), Start, End, "day");
        //    foreach (DataRow dr in dtSource.Rows)
        //    {
        //        if (CommFunc.ConvertDBNullToInt32(dr["Fun_id"]) != fun_id) continue;
        //        if (CommFunc.ConvertDBNullToInt32(dr["Co_id"]) != Co_id) continue;
        //        if (CommFunc.ConvertDBNullToInt32(dr["Module_id"]) != Module_id) continue;
        //        if (!CommFunc.ConvertDBNullToString(dr["ModuleAddr"]).Equals(ModuleAddr)) continue;
        //        //
        //        decimal uVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]) - CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
        //        decimal cVal = CommFunc.ConvertDBNullToDecimal(dr["ChrgVal"]);
        //        if (scale != 0)
        //        {
        //            uVal = Math.Round(uVal, scale, MidpointRounding.AwayFromZero);
        //            cVal = Math.Round(cVal, scale, MidpointRounding.AwayFromZero);
        //        }
        //        DataRow addDr = dtRst.NewRow();
        //        addDr["TagTime"] = dr["TagTime"];
        //        addDr["Module_id"] = dr["Module_id"];
        //        addDr["ModuleAddr"] = dr["ModuleAddr"];
        //        addDr["Co_id"] = dr["Co_id"];
        //        addDr["Fun_id"] = dr["Fun_id"];
        //        addDr["YT_id"] = dr["YT_id"];
        //        addDr["UseVal"] = uVal;
        //        addDr["ChrgVal"] = cVal;
        //        addDr["Multiply"] = multiply;
        //        dtRst.Rows.Add(addDr);
        //    }
        //    return dtRst;
        //}

        ///// <summary>
        ///// 获取使用者的全部信息
        ///// </summary>
        ///// <param name="Crm_id"></param>
        ///// <returns></returns>
        //internal DataTable GetYdPostPayInMdOnMdMap(int Module_id)
        //{
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //       new SQLParameter("@Ledger", this.Ledger),
        //       new SQLParameter("@Module_id", Module_id),      
        //    };
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select a.Module_id,a.Mm_id,a.Multiply,b.Fun_id,b.FunType,b.Scale,b.FunName");
        //    strSql.Append(" from vp_mdinfo as a inner join v0_fun as b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id");
        //    strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and b.FunType='E'");
        //    DataSet ds = SQLUtilities.Query(strSql.ToString(), params01);
        //    return ds.Tables[0];
        //}

        /////////////缴费
        /// <summary>
        /// 增加记录log
        /// </summary>
        /// <param name="pay"></param>
        /// <returns></returns>
        //internal int YdPostPayInMdOnAddPayLog(v4_pay_logVModel pay)
        //{
        //    SQLParameter[] params01 = {
        //            new SQLParameter("@Ledger", this.Ledger),
        //            new SQLParameter("@CDate", DateTime.Now),
        //            new SQLParameter("@Co_id", pay.Co_id),
        //            new SQLParameter("@Module_id", pay.Module_id),
        //            new SQLParameter("@ModuleAddr", pay.ModuleAddr),
        //            new SQLParameter("@PayVal", pay.PayVal),
        //            new SQLParameter("@PayAmt", pay.PayAmt),
        //            new SQLParameter("@PayType", pay.PayType),
        //            new SQLParameter("@IsWrong", pay.IsWrong),
        //            new SQLParameter("@IsPay", pay.IsPay),
        //            new SQLParameter("@Trade_no", pay.Trade_no),
        //            new SQLParameter("@PayStartTime", pay.PayStartTime),
        //            new SQLParameter("@PayEndTime", pay.PayEndTime),
        //            new SQLParameter("@IsCharg",pay.IsCharg),
        //            new SQLParameter("@SysUid",this.SysUid),
        //    };
        //    StringBuilder strSql = new StringBuilder();
        //    //
        //    strSql.Clear();
        //    strSql.Append("insert into v4_pay_log(");
        //    strSql.Append("Ledger,CDate,Co_id,Module_id,ModuleAddr,PayVal,PayAmt,PayType,IsWrong,Create_by,Create_dt,IsPay,Trade_no,PayStartTime,PayEndTime,IsCharg)");
        //    strSql.Append(" values (");
        //    strSql.Append("@Ledger,@CDate,@Co_id,@Module_id,@ModuleAddr,@PayVal,@PayAmt,@PayType,@IsWrong,@SysUid,now(),@IsPay,@Trade_no,@PayStartTime,@PayEndTime,@IsCharg);");
        //    strSql.Append("select last_insert_id();");
        //    object obj = SQLUtilities.ExecuteScalar(strSql.ToString(), params01);
        //    pay.Log_id = CommFunc.ConvertDBNullToLong(obj);
        //    if (pay.Log_id > 0) return 1;
        //    else return 0;
        //}

        /// <summary>
        /// 获取付费历史记录
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        //internal v4_pay_logVModel YdPostPayInMdOnV4_pay(long log_id)
        //{
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //        new SQLParameter("Ledger",this.Ledger),
        //        new SQLParameter("Log_id",log_id),
        //    };
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select Log_id,CDate,Co_id,Module_id,ModuleAddr,PayVal,PayAmt,PayType,IsWrong,Create_by,Create_dt,IsPay,Trade_no,PayStartTime,PayEndTime,IsCharg");
        //    strSql.Append(" from v4_pay_log where Log_id=@Log_id");
        //    DataSet ds = SQLUtilities.Query(strSql.ToString(), params01);
        //    v4_pay_logVModel payView = new v4_pay_logVModel();
        //    if (ds.Tables[0].Rows.Count == 1)
        //        payView = ModelHandler<v4_pay_logVModel>.FillModel(ds.Tables[0].Rows[0]);
        //    return payView;
        //}
        ///// <summary>
        ///// 更新付款信息
        ///// </summary>
        ///// <param name="pay"></param>
        //internal void YdPostPayInMdOnChrg(v4_pay_logVModel pay)
        //{
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //       new SQLParameter("@Ledger", this.Ledger),
        //       new SQLParameter("@Co_id", pay.Co_id),      
        //       new SQLParameter("@Module_id", pay.Module_id),      
        //       new SQLParameter("@ModuleAddr", pay.ModuleAddr),      
        //       new SQLParameter("@PayVal", pay.PayVal),     
        //       new SQLParameter("@SysUid", this.SysUid),
        //    };
        //    DataTable dtDay = this.GetYdPostPayInMdOnV9Day(pay.Co_id, pay.Module_id, pay.ModuleAddr, pay.PayStartTime, pay.PayEndTime);
        //    using (SQLConnection conn1 = new SQLConnection())
        //    {//更新数据库
        //        try
        //        {
        //            conn1.Open();
        //            using (SQLTransaction trans = conn1.BeginTransaction())
        //            {
        //                DateTime? debtTime = this.YdPostPayInMdOnChangeV9Day(conn1, trans, dtDay, pay.PayVal);/*先更新逐日用电量表*/
        //                this.YdPostPayInMdOnChangeMthBill(conn1, trans, pay.Co_id, pay.PayVal);/*更新组织逐月用电*/
        //                this.YdPostPayInMdOnChangeBill(conn1, trans, pay.Co_id, pay.PayVal, debtTime);/*更新组织用电*/
        //                this.YdPostPayInMdOnChangeV2Info(conn1, trans, pay.Co_id, pay.Module_id, pay.ModuleAddr, pay.PayVal, debtTime);/*更新设备用电信息*/
        //                this.YdPostPayInMdOnChangePayLog(conn1, trans, pay);
        //                trans.Commit();
        //            }
        //            conn1.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            pay.ErrCode = -1;
        //            pay.ErrTxt = ex.Message;
        //            this.UpV4_pay_log(pay);
        //            throw ex;
        //        }
        //    }
        //}

        ///// <summary>
        ///// 更新逐日用电量
        ///// </summary>
        ///// <param name="conn1"></param>
        ///// <param name="trans"></param>
        ///// <param name="dtSource"></param>
        ///// <param name="AllPayVal"></param>
        ///// <returns></returns>
        //private DateTime? YdPostPayInMdOnChangeV9Day(SQLConnection conn1, SQLTransaction trans, DataTable dtSource, decimal AllPayVal)
        //{
        //    DateTime? debtTime = null;
        //    foreach (DataRow dr in dtSource.Rows)
        //    {
        //        if (AllPayVal == 0) break;
        //        DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
        //        decimal useVal = CommFunc.ConvertDBNullToDecimal(dr["UseVal"]);/*使用电度*/
        //        decimal chrgVal = CommFunc.ConvertDBNullToDecimal(dr["ChrgVal"]);/*已付费电度*/
        //        decimal multiply = CommFunc.ConvertDBNullToDecimal(dr["Multiply"]);/*倍率*/
        //        //
        //        if (multiply == 0)
        //            throw new Exception("倍率为零");
        //        decimal allPayVal = 0 ;
        //        if (multiply != 0)
        //            allPayVal = AllPayVal / multiply;
        //        //
        //        decimal payVal = useVal - chrgVal;/*需要支付的电度*/
        //        decimal onPay = payVal > allPayVal ? allPayVal : payVal;
        //        debtTime = debtTime == null ? tagTime : (debtTime.Value > tagTime ? debtTime : tagTime);
        //        //
        //        this.YdPostPayInMdOnChangeV9Day(conn1, trans, dr, onPay);
        //        //if (this.YdPostPayInMdOnChangeV9Day(conn1, trans, dr, onPay) != 2)
        //        //    throw new Exception("更新需要支付的电度错误，请重试");
        //        AllPayVal = AllPayVal - (onPay * multiply);

        //        //
        //        if (AllPayVal == 0 && onPay == payVal)/*如果为零且全部电费支付完毕*/
        //            debtTime = debtTime.Value.AddDays(1);
        //    }
        //    if (AllPayVal > 1) /*损失的精度如果大于1，肯定有错误啦*/
        //        throw new Exception("计算需要支付的电度错误");
        //    return debtTime;
        //}
        ///// <summary>
        ///// 支付逐日用电量
        ///// </summary>
        ///// <param name="conn1"></param>
        ///// <param name="trans"></param>
        ///// <param name="curDr"></param>
        ///// <param name="PayVal"></param>
        ///// <returns></returns>
        //private int YdPostPayInMdOnChangeV9Day(SQLConnection conn1, SQLTransaction trans, DataRow curDr, decimal PayVal)
        //{
        //    DateTime tagTime = CommFunc.ConvertDBNullToDateTime(curDr["TagTime"]);
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //       new SQLParameter("@Ledger", this.Ledger),
        //       new SQLParameter("@TagTime", tagTime),
        //       new SQLParameter("@Month", new DateTime(tagTime.Year,tagTime.Month,1)),
        //       new SQLParameter("@Module_id", CommFunc.ConvertDBNullToInt32(curDr["Module_id"])), 
        //       new SQLParameter("@ModuleAddr", CommFunc.ConvertDBNullToString(curDr["ModuleAddr"])), 
        //       new SQLParameter("@Co_id", CommFunc.ConvertDBNullToInt32(curDr["Co_id"])), 
        //       new SQLParameter("@Fun_id", CommFunc.ConvertDBNullToInt32(curDr["Fun_id"])), 
        //       new SQLParameter("@YT_id", CommFunc.ConvertDBNullToInt32(curDr["YT_id"])),
        //       new SQLParameter("@PayVal", PayVal),
        //       new SQLParameter("@SysUid", this.SysUid),
        //    };
        //    SQLCommand cmd = new SQLCommand("", conn1, trans);
        //    cmd.Parameters.AddRange(params01);
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Clear();
        //    strSql.Append(string.Format("update v9_day{0} set ChrgVal=ChrgVal+@PayVal", tagTime.Year.ToString()));
        //    strSql.Append(" where TagTime=UNIX_TIMESTAMP(@TagTime) and Ledger=@Ledger and Module_id=@Module_id and ModuleAddr=@ModuleAddr and Co_id=@Co_id and Fun_id=@Fun_id and YT_id=@YT_id");
        //    strSql.Append(" and LastVal-FirstVal>=ChrgVal+@PayVal;");
        //    strSql.Append("update v8_month set ChrgVal=ChrgVal+@PayVal where TagTime=UNIX_TIMESTAMP(@Month) and Ledger=@Ledger and Module_id=@Module_id and ModuleAddr=@ModuleAddr and Co_id=@Co_id and Fun_id=@Fun_id and YT_id=@YT_id");
        //    strSql.Append(" and LastVal-FirstVal>=ChrgVal+@PayVal;");
        //    cmd.CommandText = strSql.ToString();
        //    return cmd.ExecuteNonQuery();
        //}
        ///// <summary>
        ///// 更新使用用能单位逐月付款信息表
        ///// </summary>
        ///// <param name="conn1"></param>
        ///// <param name="trans"></param>
        ///// <param name="Co_id"></param>
        ///// <param name="PayVal"></param>
        //private void YdPostPayInMdOnChangeMthBill(SQLConnection conn1, SQLTransaction trans, int Co_id, decimal PayVal)
        //{
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //       new SQLParameter("@Ledger", this.Ledger),
        //       new SQLParameter("@Month", new DateTime(DateTime.Now.Year,DateTime.Now.Month,1)),
        //       new SQLParameter("@Co_id", Co_id),  
        //       new SQLParameter("@PayVal", PayVal),
        //       new SQLParameter("@SysUid", this.SysUid),
        //    };
        //    //
        //    StringBuilder strSql = new StringBuilder();
        //    SQLCommand cmd = new SQLCommand("", conn1, trans);
        //    cmd.Parameters.AddRange(params01);
        //    strSql.Clear();
        //    strSql.Append("insert into v4_billmonth(Ledger,Month,Co_id,MthPayVal,MthBaseVal,MthBaseIsAcp,MthUseVal,MthShareVal,Create_by,Create_dt,Update_by,Update_dt,MthAcp)");
        //    strSql.Append("values(@Ledger,UNIX_TIMESTAMP(@Month),@Co_id,@PayVal,0,0,0,0,@SysUid,now(),@SysUid,now(),0)");
        //    strSql.Append("ON DUPLICATE KEY UPDATE MthPayVal=MthPayVal+@PayVal,Update_by=@SysUid,Update_dt=now()");
        //    cmd.CommandText = strSql.ToString();
        //    cmd.ExecuteNonQuery();
        //}
        ///// <summary>
        ///// 更新用用能单位付款信息表
        ///// </summary>
        ///// <param name="conn1"></param>
        ///// <param name="trans"></param>
        ///// <param name="Co_id"></param>
        ///// <param name="PayVal"></param>
        //private void YdPostPayInMdOnChangeBill(SQLConnection conn1, SQLTransaction trans, int Co_id, decimal PayVal, DateTime? DebtTime)
        //{
        //    SQLParameter pDebtTime = new SQLParameter("@DebtTime", null);
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //       new SQLParameter("@Ledger", this.Ledger),
        //       new SQLParameter("@Co_id", Co_id),  
        //       new SQLParameter("@PayVal", PayVal),
        //       new SQLParameter("@SysUid", this.SysUid),
        //    };
        //    List<SQLParameter> list = params01.ToList();
        //    list.Add(pDebtTime);
        //    //
        //    StringBuilder strSql = new StringBuilder();
        //    SQLCommand cmd = new SQLCommand("", conn1, trans);
        //    cmd.Parameters.AddRange(list.ToArray());
        //    SQLDataAdapter sda = new SQLDataAdapter(cmd);
        //    DataSet ds = new DataSet();
        //    decimal payVal = 0, useVal = 0;
        //    DateTime? debtTime = null;
        //    strSql.Clear();
        //    strSql.Append("select PayVal,UseVal,DebtTime from v4_bill where Ledger=@Ledger and Co_id=@Co_id");
        //    cmd.CommandText = strSql.ToString();
        //    sda.Fill(ds, "ds");
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        payVal = CommFunc.ConvertDBNullToDecimal(dr["PayVal"]);
        //        useVal = CommFunc.ConvertDBNullToDecimal(dr["UseVal"]);
        //        if (CommFunc.ConvertDBNullToDateTime(dr["DebtTime"]).Year != 1900)
        //            debtTime = CommFunc.ConvertDBNullToDateTime(dr["DebtTime"]);
        //    }
        //    //付款金额加总，并判断是否欠费
        //    payVal = payVal + PayVal;       
        //    if (DebtTime == null)
        //    {
        //        if (payVal < useVal)
        //            debtTime = debtTime == null ? DateTime.Now : debtTime;
        //        else
        //            debtTime = null;
        //    }
        //    else
        //    {
        //        debtTime = DebtTime;
        //    }
        //    //
        //    pDebtTime.Value = debtTime;
        //    //
        //    strSql.Clear();
        //    strSql.Append("insert into v4_bill(Ledger,Co_id,PayVal,BaseVal,PayUseVal,BaseVirUseVal,BaseUseVal,UseVal,ShareVal,DebtTime,Create_by,Create_dt,Update_by,Update_dt)");
        //    strSql.Append("values(@Ledger,@Co_id,@PayVal,0,0,0,0,0,0,@DebtTime,@SysUid,now(),@SysUid,now())");
        //    strSql.Append("ON DUPLICATE KEY UPDATE PayVal=PayVal+@PayVal,DebtTime=@DebtTime,Update_by=@SysUid,Update_dt=now()");
        //    cmd.CommandText = strSql.ToString();
        //    cmd.ExecuteNonQuery();
        //}
        ///// <summary>
        ///// 更新 设备采集状况表
        ///// </summary>
        ///// <param name="conn1"></param>
        ///// <param name="trans"></param>
        ///// <param name="Co_id"></param>
        ///// <param name="Module_id"></param>
        ///// <param name="ModuleAddr"></param>
        ///// <param name="PayVal"></param>
        //private void YdPostPayInMdOnChangeV2Info(SQLConnection conn1, SQLTransaction trans, int Co_id, int Module_id, string ModuleAddr, decimal PayVal, DateTime? DebtTime)
        //{
        //    SQLParameter pFun_id = new SQLParameter("@Fun_id", 0);
        //    SQLParameter pPayVal = new SQLParameter("@PayVal", (decimal)0);
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //       new SQLParameter("@Ledger", this.Ledger),
        //       new SQLParameter("@Co_id", Co_id),      
        //       new SQLParameter("@Module_id", Module_id),      
        //       new SQLParameter("@ModuleAddr", ModuleAddr), 
        //       new SQLParameter("@SysUid", this.SysUid),
        //    };
        //    List<SQLParameter> list = params01.ToList();
        //    list.Add(pFun_id);
        //    list.Add(pPayVal);
        //    StringBuilder strSql = new StringBuilder();
        //    SQLCommand cmd = new SQLCommand("", conn1, trans);
        //    cmd.Parameters.AddRange(list.ToArray());
        //    SQLDataAdapter sda = new SQLDataAdapter(cmd);
        //    DataSet ds = new DataSet();
        //    decimal chrgVal = 0, rdVal = 0, multiply = 0;
        //    int fun_id = 0;
        //    strSql.Clear();
        //    strSql.Append("select vf.Fun_id,b.ChrgVal,b.RdVal,b.DebtTime,a.Multiply");
        //    strSql.Append(" from vp_mdinfo as a inner join v0_fun as vf on a.Ledger=vf.Ledger and a.Mm_id=vf.Mm_id and vf.FunType='E'");
        //    strSql.Append(" left join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and vf.Fun_id=b.Fun_id");
        //    strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id");
        //    cmd.CommandText = strSql.ToString();
        //    sda.Fill(ds, "ds");
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]);
        //        chrgVal = CommFunc.ConvertDBNullToDecimal(dr["ChrgVal"]);
        //        rdVal = CommFunc.ConvertDBNullToDecimal(dr["RdVal"]);
        //        multiply = CommFunc.ConvertDBNullToDecimal(dr["Multiply"]);
        //    }
        //    if (multiply == 0)
        //        throw new Exception("倍率不能为零");
        //    //
        //    pFun_id.Value = fun_id;
        //    pPayVal.Value = PayVal / multiply;
        //    //
        //    strSql.Clear();
        //    strSql.Append("INSERT INTO v2_info (Ledger, Co_id, Module_id, ModuleAddr, Fun_id, YT_id, BeginVal, FirstVal, LastVal, MinVal, MaxVal, FirstTime, LastTime, MinTime ,MaxTime,");
        //    strSql.Append("Val1st,Val2nd,Val3rd,val4th,E4Time,ChrgVal,RdVal,DebtTime,Create_by, Create_dt, Update_by, Update_dt)");
        //    strSql.Append("VALUES (@Ledger, @Co_id, @Module_id, @ModuleAddr, @Fun_id, 0, 0, 0, 0, 0, 0, '1900-01-01', '1900-01-01', '1900-01-01', '1900-01-01',");
        //    strSql.Append("0, 0, 0, 0, null ,@PayVal, @PayVal , null, @SysUid, NOW(), @SysUid, NOW())");
        //    strSql.Append("ON DUPLICATE KEY UPDATE ChrgVal=ChrgVal+@PayVal,RdVal=RdVal+@PayVal,");
        //    strSql.Append("DebtTime=case when RdVal>=0 then null else case when DATE_FORMAT(ifnull(DebtTime,'1900-01-01'),'%Y')<2000 then now() else DebtTime end end");
        //    cmd.CommandText = strSql.ToString();
        //    cmd.ExecuteNonQuery();
        //}

        //private int YdPostPayInMdOnChangePayLog(SQLConnection conn1, SQLTransaction trans, v4_pay_logVModel pay)
        //{
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //       new SQLParameter("@Log_id", pay.Log_id),
        //       new SQLParameter("@ErrCode", pay.ErrCode),      
        //       new SQLParameter("@ErrTxt", pay.ErrTxt,256),      
        //       new SQLParameter("@PayTxt", pay.PayTxt,256),
        //       new SQLParameter("@SysUid", this.SysUid),
        //    };
        //    StringBuilder strSql = new StringBuilder();
        //    SQLCommand cmd = new SQLCommand("", conn1, trans);
        //    cmd.Parameters.AddRange(params01);
        //    strSql.Append("update v4_pay_log set ErrCode=@ErrCode,ErrTxt=@ErrTxt,EndTime=now(),PayTxt=@PayTxt where Log_id=@Log_id");
        //    cmd.CommandText = strSql.ToString();
        //    return cmd.ExecuteNonQuery();
        //}

        //internal int YdPostPayInMdOnChangePayLog(v4_pay_logVModel pay)
        //{
        //    SQLParameter[] params01 = new SQLParameter[]
        //    {
        //       new SQLParameter("@Log_id", pay.Log_id),
        //       new SQLParameter("@ErrCode", pay.ErrCode),      
        //       new SQLParameter("@ErrTxt", pay.ErrTxt,256),      
        //       new SQLParameter("@PayTxt", pay.PayTxt,256),
        //       new SQLParameter("@SysUid", this.SysUid),
        //    };
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("update v4_pay_log set ErrCode=@ErrCode,ErrTxt=@ErrTxt,EndTime=now(),PayTxt=@PayTxt where Log_id=@Log_id");
        //    return SQLUtilities.ExecuteNonQuery(strSql.ToString(), params01);
        //}
    }
}
