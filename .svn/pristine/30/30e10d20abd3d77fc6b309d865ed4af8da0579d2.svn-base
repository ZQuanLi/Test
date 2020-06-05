using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Opertion.Home
{
    public class HomeHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Home.HomeBLL bll = null;
        public HomeHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Home.HomeBLL(user.Ledger, user.Uid);
        }

        /// <summary>
        /// 首页设备数
        /// </summary>
        /// <returns></returns>
        public APIRst GetEquInfo()
        {
            APIRst rst = new APIRst();
            try
            {             
                DataTable dtSource = bll.GetEquCount();
                dtSource.Columns.Add("IsGz", typeof(System.Int32));
                int allCount = dtSource.Rows.Count;
                int mdCount=0,gzzs = 0, dbgzzs = 0;
                foreach (DataRow dr in dtSource.Rows)
                {
                    string key = user.CacheKey + CommFunc.ConvertDBNullToInt32(dr["Module_id"]) + "." + CommFunc.ConvertDBNullToString(dr["ModuleAddr"]) + ".Comm";
                    int isDefine = CommFunc.ConvertDBNullToInt32(dr["IsDefine"]);
                  
                    RstVar var = MemcachedMgr.GetVal<RstVar>(key);
                    int gz = (var == null ? 1 : (int)CommFunc.ConvertDBNullToDecimal(var.lpszVal) == 0 ? 0 : 1);
                    gzzs = gzzs + gz;
                    if (isDefine == 100)
                    {
                        mdCount = mdCount + 1;
                        dbgzzs = dbgzzs + gz;
                    }
                }
                rst.data = new { Count = allCount, MdCount = mdCount, Gzzs = gzzs, Dbgzzs = dbgzzs };
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页设备数:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIRst GetEquAlarm()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetEquAlarm();
                int allCount = dtSource.Rows.Count;
                int mdCount = dtSource.Select("IsDefine=100").Count();
                rst.data = new { Count = allCount, MdCount = mdCount };
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页告警数:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 首页获取首页最新告警
        /// </summary>
        /// <returns></returns>
        public APIRst GetAlarmFisrt()
        {
            APIRst rst = new APIRst();
            try
            {
                object obj = new { id = 0, Content = 0 }; 
                DataTable dtSource = bll.GetAlarmFisrt();
                if (dtSource.Rows.Count > 0)
                    obj = new { id = CommFunc.ConvertDBNullToLong(dtSource.Rows[0]["Log_id"]), Content= CommFunc.ConvertDBNullToString(dtSource.Rows[0]["Content"])+",地点:" + CommFunc.ConvertDBNullToString(dtSource.Rows[0]["MeterName"])+ ",当前值:"+ CommFunc.ConvertDBNullToString(dtSource.Rows[0]["CollectValue"]) };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("首页获取首页最新告警:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 项目导航
        /// </summary>
        /// <returns></returns>
        public APIRst GetProjectList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetProjectList();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               Name = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Addr = CommFunc.ConvertDBNullToString(s1["CustAddr"]),
                               Gps = CommFunc.ConvertDBNullToString(s1["Gps"]),
                               Area = CommFunc.ConvertDBNullToDecimal(s1["Area"]),
                               AreaType = "",
                               Layer = 0
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("项目导航:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 隐患信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetAlarmCompare()
        {
            APIRst rst = new APIRst();
            try
            {
                DateTime day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                DateTime mth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                DataTable dtSource = bll.GetAlarmCompare();
                int today = dtSource.Select("CDate=#" + day + "#").Count();
                int yesterday  = dtSource.Select("CDate=#" + day.AddDays(-1) + "#").Count();
                int month = dtSource.Select("CDate>=#" + mth + "#").Count();
                int prvmth = dtSource.Select("CDate<#" + mth + "#").Count();
                //环比增长率 =（本期数 - 上期数）/ 上期数×100 %
                int hb_day = yesterday == 0 ? 0 : (int)(((today - yesterday) / (decimal)yesterday) * 100);
                int hb_mth = prvmth == 0 ? 0 : (int)(((month - prvmth) / (decimal)prvmth) * 100);
                rst.data = new { today = today, yesterday = yesterday, hb_day= hb_day, month = month, prvmth = prvmth , hb_mth = hb_mth };
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("隐患信息:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}