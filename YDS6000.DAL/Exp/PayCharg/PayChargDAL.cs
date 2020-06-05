using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Exp.PayCharg
{
    public class PayChargDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";

        public PayChargDAL(string project, int ledger, int uid)
        {
            this.Project = project;
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        public DataTable GetChargList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Log_id,a.CDate,a.Co_id,a.FirstVal,a.LastVal,a.FirstTime,a.LastTime,a.Price,(a.LastVal-a.FirstVal)*a.Price as LastAmt,a.ChargVal,b.CoName");
            strSql.Append(" from v4_pay_charg as a inner join vp_coinfo as b on a.Ledger = b.Ledger and a.Co_id = b.Co_id");
            strSql.Append(" where a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 新增物业列表
        /// </summary>
        /// <returns></returns>
        public int AddCharg(int co_id, decimal firstVal, decimal lastVal, DateTime firstTime, DateTime lastTime, decimal price, decimal chargAmt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select RdAmt from v1_custinfo where Ledger=@Ledger and Co_id=@Co_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
            decimal syAmt = CommFunc.ConvertDBNullToDecimal(obj);
            strSql.Clear();
            strSql.Append("insert into v4_pay_charg(CDate,Ledger,Co_id,FirstVal,LastVal,FirstTime,LastTime,Price,ChargVal,Cretae_by,Create_dt,Update_by,Update_dt,SyAmt)");
            strSql.Append("values(@LastTime,@Ledger,@Co_id,@FirstVal,@LastVal,@FirstTime,@LastTime,@Price,@ChargVal,@SysUid,now(),@SysUid,now(),@SyAmt)");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id, FirstVal = firstVal, LastVal = lastVal, FirstTime = firstTime, LastTime = lastTime, Price = price, ChargVal = chargAmt, SysUid = this.SysUid, SyAmt = syAmt });
        }
        /// <summary>
        /// 新增物业列表
        /// </summary>
        /// <returns></returns>
        public int UpdateCharg(long log_id, decimal firstVal, decimal lastVal, DateTime firstTime, DateTime lastTime, decimal price, decimal chargAmt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v4_pay_charg set CDate=@LastTime,FirstVal=@FirstVal,LastVal=@LastVal,FirstTime=@FirstTime,LastTime=@LastTime,Price=@Price,ChargVal=@ChargVal,Update_by=@SysUid,Update_dt=now() where Log_id=@Log_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Log_id = log_id, FirstVal = firstVal, LastVal = lastVal, FirstTime = firstTime, LastTime = lastTime, Price = price, ChargVal = chargAmt, SysUid = this.SysUid });
        }
        public int DeleteCharg(long log_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(*)as cnt from v4_pay_charg where Log_id=@Log_id and `Status`=1");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Log_id = log_id });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("已付款不能删除");
            strSql.Clear();
            strSql.Append("delete from v4_pay_charg where Log_id=@Log_id");
            return SQLHelper.Execute(strSql.ToString(), new { Log_id = log_id });
        }
        public int ToLeadCharg(ref DataTable dtSource)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select Co_id,CoName from v1_cust where Ledger=@Ledger");
            DataTable dtCo = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
            //dtCo.PrimaryKey = new DataColumn[] { dtCo.Columns["Co_id"] };
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["ErrCode"] = 1;
                dr["ErrTxt"] = "";
                DataRow[] co = dtCo.Select("CoName='" + CommFunc.ConvertDBNullToString(dr["CoName"]) + "'");
                if (co.Count() != 1)
                {
                    dr["ErrCode"] = -1;
                    dr["ErrTxt"] = "房间号有错误";
                    continue;
                }
                int co_id = CommFunc.ConvertDBNullToInt32(co[0]["Co_id"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal chargVal = CommFunc.ConvertDBNullToDecimal(dr["ChargVal"]);
                decimal price = CommFunc.ConvertDBNullToDecimal(dr["Price"]);
                DateTime firstTime = CommFunc.ConvertDBNullToDateTime(CommFunc.ConvertDBNullToDateTime(dr["FirstTime"]).ToString("yyyy-MM-dd"));
                DateTime lastTime = CommFunc.ConvertDBNullToDateTime(CommFunc.ConvertDBNullToDateTime(dr["LastTime"]).ToString("yyyy-MM-dd"));
                #region
                if (firstVal < 0)
                {
                    dr["ErrCode"] = -1;
                    dr["ErrTxt"] = "上期计数小于零";
                    continue;
                }
                if (lastVal < 0)
                {
                    dr["ErrCode"] = -1;
                    dr["ErrTxt"] = "本期计数小于零";
                    continue;
                }
                if (firstVal > lastVal)
                {
                    dr["ErrCode"] = -1;
                    dr["ErrTxt"] = "上期计数大于本期计数";
                    continue;
                }
                if (price <= 0)
                {
                    dr["ErrCode"] = -1;
                    dr["ErrTxt"] = "水费单价小于零";
                    continue;
                }
                if (chargVal <= 0)
                {
                    dr["ErrCode"] = -1;
                    dr["ErrTxt"] = "物业费小于零";
                    continue;
                }
                if (firstTime.Year <= 2000)
                {
                    dr["ErrCode"] = -1;
                    dr["ErrTxt"] = "上期时间有异常";
                    continue;
                }

                if (lastTime.Year <= 2000)
                {
                    dr["ErrCode"] = -1;
                    dr["ErrTxt"] = "本期时间有异常";
                    continue;
                }
                if (firstTime > lastTime)
                {
                    dr["ErrCode"] = -1;
                    dr["ErrTxt"] = "上期时间大于本期时间";
                    continue;
                }
                #endregion
                if (CommFunc.ConvertDBNullToInt32(dr["Code"]) < 0) continue;
                //
                strSql.Clear();
                strSql.Append("select RdAmt from v1_custinfo where Ledger=@Ledger and Co_id=@Co_id");
                object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
                decimal syAmt = CommFunc.ConvertDBNullToDecimal(obj);
                //
                strSql.Clear();
                strSql.Append("select Log_id from v4_pay_charg where CDate=@CDate and Ledger=@Ledger and Co_id=@Co_id order by Log_id desc limit 1");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { CDate = lastTime, Ledger = this.Ledger, Co_id = co_id });
                long log_id = CommFunc.ConvertDBNullToLong(obj);
                //
                object param = new
                {
                    Log_id = log_id,
                    Ledger = this.Ledger,
                    Co_id = co_id,
                    FirstVal = firstVal,
                    LastVal = lastVal,
                    FirstTime = firstTime,
                    LastTime = lastTime,
                    Price = price,
                    ChargVal = chargVal,
                    SysUid = this.SysUid,
                    SyAmt = syAmt
                };
                if (log_id == 0)
                {
                    strSql.Clear();
                    strSql.Append("insert into v4_pay_charg(CDate,Ledger,Co_id,FirstVal,LastVal,FirstTime,LastTime,Price,ChargVal,Cretae_by,Create_dt,Update_by,Update_dt,SyAmt)");
                    strSql.Append("values(@LastTime,@Ledger,@Co_id,@FirstVal,@LastVal,@FirstTime,@LastTime,@Price,@ChargVal,@SysUid,now(),@SysUid,now(),@SyAmt)");
                    return SQLHelper.Execute(strSql.ToString(), param);
                }
                else
                {
                    strSql.Clear();
                    strSql.Append("update v4_pay_charg set CDate=@LastTime,FirstVal=@FirstVal,LastVal=@LastVal,FirstTime=@FirstTime,LastTime=@LastTime,Price=@Price,ChargVal=@ChargVal,Update_by=@SysUid,Update_dt=now() where Log_id=@Log_id");
                    return SQLHelper.Execute(strSql.ToString(), param);
                }
            }
            return 1;
        }
    }
}
