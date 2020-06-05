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
    public class ZpRaValDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public ZpRaValDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 赠电
        /// </summary>
        /// <returns></returns>
        public bool BaseVal()
        {
            DateTime month = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);           
            DataTable dtSyscont = this.GetSyscont();
            if (dtSyscont.Rows.Count != 1) return false;
            decimal mthBaseVal = CommFunc.ConvertDBNullToDecimal(dtSyscont.Rows[0]["MthBaseRoom"]);
            decimal mthBaseCrm = CommFunc.ConvertDBNullToDecimal(dtSyscont.Rows[0]["MthBaseCrm"]);
            string exCfg = CommFunc.ConvertDBNullToString(dtSyscont.Rows[0]["ExCfg"]);
            //月份白名单过滤
            string s = "m" + month.Month.ToString().PadLeft(2,'0') + ":1";
            if (!exCfg.Contains(s))
                return false;

            if (mthBaseVal == 0 && mthBaseCrm == 0) return false;

            DataTable dtSource = this.GetRaValList();
            foreach (DataRow dr in dtSource.Rows)
            {
                int module_id = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);
                int fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]);
                string moduleAddr = CommFunc.ConvertDBNullToString(dr["ModuleAddr"]);
                int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
                string subTab = CommFunc.ConvertDBNullToString(dr["SubTab"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(dr["Multiply"]);
                decimal price = CommFunc.ConvertDBNullToDecimal(dr["Price"]);
                int yt_id = CommFunc.ConvertDBNullToInt32(dr["YT_id"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                if (mthBaseCrm != 0 && mthBaseVal == 0)
                {
                    int cc = this.GetUserCnt(co_id);
                    mthBaseVal = CommFunc.ConvertDBNullToDecimal(mthBaseCrm) * cc;
                }
                if (mthBaseVal == 0) continue;

                if (GetIsBaseVal(month, module_id, fun_id, moduleAddr, co_id, subTab) == true) continue;

                using (IDbConnection dbConnection = SQLHelper.GetDBConnection())
                {                    
                    try
                    {
                        dbConnection.Open();
                        IDbTransaction trans = dbConnection.BeginTransaction();
                        this.AddRaValStep1(dbConnection, month, module_id, fun_id, moduleAddr, co_id, subTab);
                        this.AddRaValStep2(dbConnection, month, module_id, fun_id, moduleAddr, co_id, yt_id, subTab, lastVal, mthBaseVal, price, multiply);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        this.AddErrLog(month, module_id, fun_id, moduleAddr, co_id, mthBaseVal, price, ex.Message);
                    }
                }
            }

            return true;
        }

        private DataTable GetSyscont()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.MthBaseRoom,a.MthBaseCrm,a.MthBaseIsAcp,a.ExCfg");
            strSql.Append(" from syscont as a");
            strSql.Append(" where a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 获取赠电列表
        /// </summary>
        /// <returns></returns>
        private DataTable GetRaValList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.Fun_id,a.Multiply,a.Co_id,a.SubTab,rp.YT_id,rp.LastVal,");
            strSql.Append("case when ifnull(mp.Price,0)=0 then s1.Price else mp.Price end as Price");
            strSql.Append(" from vp_funinfo as a inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" inner join vp_mdinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            strSql.Append(" inner join v2_info as rp on a.Ledger=rp.Ledger and a.Co_id=rp.Co_id and a.Module_id=rp.Module_id and a.ModuleAddr=rp.ModuleAddr and a.Fun_id=rp.Fun_id");
            strSql.Append(" left join v1_gateway_esp_module_info as mp on a.Ledger=mp.Ledger and a.Module_id=mp.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and (a.IsChrg=1 or a.FunType=@FunType) and mp.DisBaseVal=0");
            strSql.Append(" and b.ModuleAddr=case when s1.Zb=0 then b.MeterAddr else b.ModuleAddr end");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, FunType = V0Fun.E.ToString() });
        }
        /// <summary>
        /// 获取房间的人数
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        private int GetUserCnt(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(*)as cnt from v3_userinfo where Ledger=@Ledger and Co_id=@Co_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
            return CommFunc.ConvertDBNullToInt32(obj);
        }
        /// <summary>
        /// 获取是否已经有赠电
        /// </summary>
        /// <param name="month"></param>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="moduleAddr"></param>
        /// <param name="co_id"></param>
        /// <param name="subTab"></param>
        /// <returns></returns>
        private bool GetIsBaseVal(DateTime tagTime, int module_id, int fun_id, string moduleAddr, int co_id,string subTab)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append(string.Format("select count(*) from information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @TabName LIMIT 1", tagTime.Year));
            var obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { TabName = "v9_month_" + subTab + tagTime.Year });
            if (CommFunc.ConvertDBNullToInt32(obj) == 0) return false;
            strSql.Clear();
            strSql.Append(string.Format("select count(*) from v9_month_{0}{1}", subTab, tagTime.Year));
            strSql.Append(" where TagTime=UNIX_TIMESTAMP(@TagTime) and Ledger=@Ledger and Module_id=@Module_id and Fun_id=@Fun_id and Co_id=@Co_id and RaVal>0"); //and ModuleAddr=@ModuleAddr
            obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, TagTime = tagTime, Module_id = module_id, Fun_id = fun_id, ModuleAddr = moduleAddr, Co_id = co_id });
            return CommFunc.ConvertDBNullToInt32(obj) > 0 ? true : false;
        }

        /// <summary>
        /// 赠送总电度(每月未用完电的不累计值)
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="trans"></param>
        /// <param name="month"></param>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="moduleAddr"></param>
        /// <param name="co_id"></param>
        /// <param name="subTab"></param>
        private void AddRaValStep1(IDbConnection dbConnection,  DateTime month, int module_id, int fun_id, string moduleAddr, int co_id, string subTab)
        {
            StringBuilder strSql = new StringBuilder();
            int i = 0;
            decimal mthUseVal = 0, mthRaVal = 0;
            while (i <= 6)
            {
                DateTime tagTime = month.AddMonths(0 - (++i));
                strSql.Clear();
                strSql.Append(string.Format("select count(*) from information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @TabName LIMIT 1", tagTime.Year));
                object obj = dbConnection.ExecuteScalar(strSql.ToString(), new { TabName = "v9_month_" + subTab + tagTime.Year });
                if (CommFunc.ConvertDBNullToInt32(obj) == 0) continue;
                DataTable dtRst = new DataTable();
                /*电表上月使用电量情况*/
                strSql.Clear();
                strSql.Append(string.Format("select sum(LastVal-FirstVal)as UseVal,sum(RaVal)as RaVal from v9_month_{0}{1}", subTab, tagTime.Year));
                strSql.Append(" where TagTime=UNIX_TIMESTAMP(@TagTime) and Ledger=@Ledger and Module_id=@Module_id and Fun_id=@Fun_id and ModuleAddr=@ModuleAddr and Co_id=@Co_id");
                var reader = dbConnection.ExecuteReader(strSql.ToString(), new { Ledger = this.Ledger, TagTime = tagTime, Module_id = module_id, Fun_id = fun_id, ModuleAddr = moduleAddr, Co_id = co_id });
                dtRst.Load(reader);
                if (dtRst.Rows.Count > 0)
                {
                    mthUseVal = CommFunc.ConvertDBNullToDecimal(dtRst.Rows[0]["UseVal"]);
                    mthRaVal = CommFunc.ConvertDBNullToDecimal(dtRst.Rows[0]["RaVal"]);
                }
                /*电表的建筑上月使用电量情况*/
                break;
            }
            decimal mthRealVal = mthUseVal >= mthRaVal ? mthRaVal : mthUseVal;
            strSql.Clear();
            strSql.Append("update v2_info set RaVal = case when RaVal - @MthRaVal +  @MthRealVal < 0 then 0 else RaVal - @MthRaVal +  @MthRealVal end");
            strSql.Append(" where Ledger=@Ledger and Co_id=@Co_id and Module_id=@Module_id and ModuleAddr=@ModuleAddr and Fun_id=@Fun_id;");
            strSql.Append("update v4_bill set RaVal = case when RaVal - @MthRaVal +  @MthRealVal < 0 then 0 else RaVal - @MthRaVal +  @MthRealVal end ");
            strSql.Append(" where Ledger=@Ledger and Co_id=@Co_id and Fun_id=@Fun_id;");
            dbConnection.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, Fun_id = fun_id, ModuleAddr = moduleAddr, Co_id = co_id, MthRaVal = mthRaVal, MthRealVal = mthRealVal });
        }

        private void AddRaValStep2(IDbConnection dbConnection, DateTime tagTime, int module_id, int fun_id, string moduleAddr, int co_id, int yt_id, string subTab, decimal lastVal, decimal raVal, decimal price, decimal multiply)
        {
            StringBuilder strSql = new StringBuilder();
            object params01 = new
            {
                TagTime = tagTime,
                Ledger = this.Ledger,
                Module_id = module_id,
                Fun_id = fun_id,
                ModuleAddr = moduleAddr,
                Co_id = co_id,
                YT_id = yt_id,
                LastVal = lastVal,
                Multiply = multiply,
                RaVal = raVal,
                SysUid = this.SysUid,
            };
            strSql.Clear();
            strSql.Append(string.Format("select count(*) from information_schema.TABLES WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @TabName LIMIT 1", tagTime.Year));
            object obj = dbConnection.ExecuteScalar(strSql.ToString(), new { TabName = "v9_month_" + subTab + tagTime.Year });
            if (CommFunc.ConvertDBNullToInt32(obj) == 0)
                SQLHelper.ExecuteStoredProcedure("sp_DataProcessInSchema", new { _mData = tagTime, _mSubTab = subTab });
            /*新增到月表*/
            strSql.Clear();
            strSql.Append(string.Format("insert into v9_month_{0}{1}", subTab, tagTime.Year));
            strSql.Append("(TagTime,Ledger,Module_id,Fun_id,ModuleAddr,Co_id,YT_id,");
            strSql.Append("FirstVal, LastVal, MinVal, MaxVal, FirstTime, LastTime, MinTime, MaxTime, CurVal, Multiply,`Interval`,");
            strSql.Append("FirstVal1st,FirstVal2nd,FirstVal3rd,FirstVal4th,LastVal1st,LastVal2nd,LastVal3rd,LastVal4th,");
            strSql.Append("DataCfg,ChrgVal,Create_by,Create_dt,Update_by,Update_dt,RaVal)");
            strSql.Append("value(UNIX_TIMESTAMP(@TagTime),@Ledger,@Module_id,@Fun_id,@ModuleAddr,@Co_id,@YT_id,");
            strSql.Append("@LastVal,@LastVal,@LastVal,@LastVal,now(),now(),now(),now(),@LastVal,@Multiply,1,");
            strSql.Append("0,0,0,0,0,0,0,0,'',0,@SysUid,now(),@SysUid,now(),@RaVal)");
            strSql.Append(" ON DUPLICATE KEY UPDATE RaVal=@RaVal,Update_by=@SysUid,Update_dt=now();");
            //新增到信息表
            strSql.Append("update v2_info set BaseVal=BaseVal+@RaVal,RaVal=RaVal+@RaVal");
            strSql.Append(" where Ledger=@Ledger and Co_id=@Co_id and Module_id=@Module_id and ModuleAddr=@ModuleAddr and Fun_id=@Fun_id;");
            dbConnection.Execute(strSql.ToString(), params01);
            /*新增到建筑*/
            strSql.Clear();
            strSql.Append("insert into v4_billmonth (Ledger,`Month`,Co_id,`Fun_id`,MthBaseVal ,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append("value(@Ledger,UNIX_TIMESTAMP(@TagTime),@Co_id,@Fun_id,@RaVal,@SysUid,now(),@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE MthBaseVal=MthBaseVal+@RaVal,Update_by=@SysUid,Update_dt=now();");
            strSql.Append("insert into v4_bill(Ledger,Co_id,Fun_id,BaseVal,RaVal,Create_by,Create_dt,Update_by,Update_dt)value(@Ledger,@Co_id,@Fun_id,@RaVal,@RaVal,@SysUid,now(),@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE BaseVal=BaseVal+@RaVal,RaVal=RaVal+@RaVal,Update_by=@SysUid,Update_dt=now()");
            dbConnection.Execute(strSql.ToString(), params01);
            //赠电记录
            object params02 = new
            {
                TagTime = tagTime,
                Ledger = this.Ledger,
                Co_id = co_id,
                Module_id = module_id,
                ModuleAddr = moduleAddr,
                Fun_id = fun_id,
                PayVal = raVal,
                PayAmt = Math.Round(raVal * price, 2, MidpointRounding.AwayFromZero),
                PayType = (int)PayType.present,
                PayStartTime = tagTime,
                PayEndTime = tagTime.AddMonths(1).AddDays(-1),
                Price = price,
                SysUid = this.SysUid,
            };
            strSql.Clear();
            strSql.Append("insert into v4_pay_log(");
            strSql.Append("Ledger,CDate,Co_id,Module_id,ModuleAddr,Fun_id,PayVal,PayAmt,PayType,IsWrong,Create_by,Create_dt,IsPay,Trade_no,PayStartTime,PayEndTime,IsCharg,Price,ErrCode,ErrTxt,EndTime,Month)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,now(),@Co_id,@Module_id,@ModuleAddr,@Fun_id,@PayVal,@PayAmt,@PayType,0,@SysUid,now(),0,'',@PayStartTime,@PayEndTime,0,@Price,1,'',now(),UNIX_TIMESTAMP(@TagTime));");
            dbConnection.Execute(strSql.ToString(), params02);
        }

        private void AddErrLog(DateTime tagTime, int module_id, int fun_id, string moduleAddr, int co_id, decimal raVal, decimal price, string errTxt)
        {
            StringBuilder strSql = new StringBuilder();
            object params01 = new
            {
                TagTime = tagTime,
                Ledger = this.Ledger,
                Co_id = co_id,
                Module_id = module_id,
                ModuleAddr = moduleAddr,
                Fun_id = fun_id,
                PayVal = raVal,
                PayAmt = Math.Round(raVal * price, 2, MidpointRounding.AwayFromZero),
                PayType = (int)PayType.present,
                PayStartTime = tagTime,
                PayEndTime = tagTime.AddMonths(1).AddDays(-1),
                Price = price,
                ErrTxt = errTxt,
                SysUid = this.SysUid,
            };
            strSql.Clear();
            strSql.Append("insert into v4_pay_log(");
            strSql.Append("Ledger,CDate,Co_id,Module_id,ModuleAddr,Fun_id,PayVal,PayAmt,PayType,IsWrong,Create_by,Create_dt,IsPay,Trade_no,PayStartTime,PayEndTime,IsCharg,Price,ErrCode,ErrTxt,EndTime,Month)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,now(),@Co_id,@Module_id,@ModuleAddr,@Fun_id,@PayVal,@PayAmt,@PayType,0,@SysUid,now(),0,'',@PayStartTime,@PayEndTime,0,@Price,-1,@ErrTxt,now(),UNIX_TIMESTAMP(@TagTime));");
            SQLHelper.Execute(strSql.ToString(), params01);
        }
    }
}
