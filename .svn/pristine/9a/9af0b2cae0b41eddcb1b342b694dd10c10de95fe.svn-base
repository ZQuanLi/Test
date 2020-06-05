using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using YDS6000.Models.Tables;

namespace YDS6000.WebApi.Areas.ExpAdminApp.Opertion.Monitor
{
    public partial class ExpAdminMonitorHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.ExpAdminApp.Monitor.ExpAdminMonitorBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpAdminMonitorHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.ExpAdminApp.Monitor.ExpAdminMonitorBLL(WebConfig.SysProject, user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 获取抄表数据监视列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdMonitorOnList(string strcName, string coName, string ssr, int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdMonitorOnList(strcName, coName, ssr, co_id);
                this.GetYdStatusOnLine(dtSource);
                int total = dtSource.Rows.Count;
                object rows = this.GetYdMonitorOnList(dtSource);
                int ClosingCount = 0; //合闸
                int OpeningCount = 0; //分闸
                if (dtSource.Rows.Count > 0)
                {
                    foreach (DataRow row in dtSource.Rows)
                    {
                        if (Convert.ToInt32(row["Ssr"]) == 0)
                        {
                            ClosingCount = ClosingCount + 1;//合闸
                        }
                        if (Convert.ToInt32(row["Ssr"]) == 1)
                        {
                            ClosingCount = OpeningCount + 1;//分闸
                        }
                    }
                }
                object obj = new { total = total, ClosingCount = ClosingCount, OpeningCount = OpeningCount, rows = rows };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("系统监视-抄表数据监视(GetYdMonitorOnList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private void GetYdStatusOnLine(DataTable dtSource)
        {
            foreach (DataRow dr in dtSource.Rows)
            {
                RstVar var = null;
                //RstVar varSsr = null;
                int frMd = CommFunc.ConvertDBNullToInt32(dr["FrMd"]);
                //int ssr = -1;
                string memcachKey = CommFunc.ConvertDBNullToString(dr["MemcachKey"]);
                string memcachKeySsr = memcachKey.Replace(".E", ".Ssr");
                string cachComm = CommFunc.ConvertDBNullToString(dr["CachComm"]);
                DateTime lastTime = CommFunc.ConvertDBNullToDateTime(dr["LastTime"]);
                frMd = frMd == 0 ? 15 : frMd;
                dr["OnLine"] = 0;
                dr["Ssr"] = -1;
                if (lastTime.AddDays(1) < DateTime.Now)
                    continue;
                #region 是否在线
                if (!string.IsNullOrEmpty(cachComm))
                {
                    var = null; int i = 0;
                    while (var == null && ++i < 2)
                    {
                        var = MemcachedMgr.GetVal<RstVar>(WebConfig.MemcachKey + cachComm);
                        if (var != null)
                            break;
                        System.Threading.Thread.Sleep(2);
                    }
                    if (var != null)
                        dr["OnLine"] = ((int)CommFunc.ConvertDBNullToDecimal(var.lpszVal) == 0) ? 1 : 0;
                }
                else if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(dr["MemcachKey"])))
                {
                    var = null; int i = 0;
                    while (var == null && ++i < 2)
                    {
                        var = MemcachedMgr.GetVal<RstVar>(WebConfig.MemcachKey + memcachKey);
                        if (var != null)
                        {
                            //FileLog.WriteLog("key:" + Common.GetMemcachKey + memcachKey + " Val:" + JsonHelper.Serialize(var));
                            break;
                        }
                        else
                        {
                            //FileLog.WriteLog("空值 key:" + Common.GetMemcachKey + memcachKey);
                            //FileLog.WriteLog("空值 SSr key:" + Common.GetMemcachKey + memcachKeySsr);
                        }
                        System.Threading.Thread.Sleep(2);
                    }
                    if (var != null)
                    {
                        DateTime callTime = var.lpszdateTime;
                        if (callTime >= DateTime.Now.AddMinutes(0 - frMd))
                            dr["OnLine"] = 1;
                    }
                }
                #endregion
                #region 拉合闸状态
                if (!string.IsNullOrEmpty(memcachKeySsr))
                {
                    var = null; int i = 0;
                    while (var == null && ++i < 2)
                    {
                        var = MemcachedMgr.GetVal<RstVar>(WebConfig.MemcachKey + memcachKeySsr);
                        if (var != null)
                            break;
                        System.Threading.Thread.Sleep(2);
                    }
                    if (var != null)
                        dr["Ssr"] = (int)CommFunc.ConvertDBNullToDecimal(var.lpszVal);
                }
                #endregion
                //int i = 0;
                //int onLine = -1;
                //if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(dr["MemcachKey"])))
                //{
                //    while (var == null && ++i < 2)
                //    {
                //        var = MemcachedMgr.GetVal<RstVar>(WebConfig.MemcachKey + memcachKey);
                //        varSsr = MemcachedMgr.GetVal<RstVar>(WebConfig.MemcachKey + memcachKeySsr);
                //        if (var != null || varSsr != null)
                //        {
                //            //FileLog.WriteLog("key:" + WebConfig.MemcachKey + memcachKey + " Val:" + JsonHelper.Serialize(var));
                //            break;
                //        }
                //        else
                //        {
                //            //FileLog.WriteLog("空值 key:" + WebConfig.MemcachKey + memcachKey);
                //            //FileLog.WriteLog("空值 SSr key:" + WebConfig.MemcachKey + memcachKeySsr);
                //        }
                //        System.Threading.Thread.Sleep(2);
                //    }
                //}
                //onLine = 0;
                //if (var != null)
                //{
                //    DateTime callTime = var.lpszdateTime;
                //    if (callTime >= DateTime.Now.AddMinutes(0 - frMd))
                //        onLine = 1;
                //}
                //if (varSsr != null)
                //    ssr = CommFunc.ConvertDBNullToInt32(varSsr.lpszVal);
                //dr["OnLine"] = onLine;
                //dr["Ssr"] = ssr;
            }
        }

        private object GetYdMonitorOnList(DataTable dtSource)
        {
            var res1 = from s1 in dtSource.AsEnumerable()
                       select new
                       {
                           RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                           Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                           ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                           ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                           Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                           CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                           CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           Multiply = CommFunc.ConvertDBNullToDecimal(s1["Multiply"]).ToString("f2"),
                           LastVal = Convert.IsDBNull(s1["LastVal"]) ? "" : CommFunc.ConvertDBNullToDecimal(s1["LastVal"]).ToString("f2"),
                           CurVal = Convert.IsDBNull(s1["CurVal"]) ? "" : CommFunc.ConvertDBNullToDecimal(s1["CurVal"]).ToString("f2"),
                           RdVal = Convert.IsDBNull(s1["RdVal"]) ? "" : CommFunc.ConvertDBNullToDecimal(s1["RdVal"]).ToString("f2"),
                           SyZsVal = Convert.IsDBNull(s1["SyZsVal"]) ? "" : CommFunc.ConvertDBNullToDecimal(s1["SyZsVal"]).ToString("f2"),
                           LastTime = (CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).Year == 1900 || CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).Year == 0) ? "" : CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           Ssr = s1["Ssr"] == DBNull.Value ? -1 : CommFunc.ConvertDBNullToInt32(s1["Ssr"]),
                           DayUseVal = "",
                           MthUseVal = "",
                           HandledBY = CommFunc.ConvertDBNullToString(s1["HandledBY"]),
                           OnLine = CommFunc.ConvertDBNullToInt32(s1["OnLine"]),
                           IsAlarm = CommFunc.ConvertDBNullToInt32(s1["IsAlarm"]),
                           RdAmt = (CommFunc.ConvertDBNullToDecimal(s1["RdVal"]) * CommFunc.ConvertDBNullToDecimal(s1["Price"])).ToString("f2"),
                           SyVal = (CommFunc.ConvertDBNullToDecimal(s1["RdVal"]) - CommFunc.ConvertDBNullToDecimal(s1["SyZsVal"])).ToString("f2"),
                           PicType = CommFunc.ConvertDBNullToString(s1["ModuleType"]).Contains("-NB") ? 1 : 0,
                           @checked = 0
                       };
            return res1.ToList();
        }




        /// <summary>
        /// 获取城市查询参数
        /// </summary>
        /// <param name="Co_id">id</param>
        /// <returns></returns>
        public APIRst GetCity(int Co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetCity(5000, Co_id);
                object obj = new { Total = dtSource.Rows.Count, rows = dtSource };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页建筑数:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取区域查询参数
        /// </summary>
        /// <param name="Co_id">id</param>
        /// <returns></returns>
        public APIRst GetRegion(int Co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetCity(6000, Co_id);
                object obj = new { Total = dtSource.Rows.Count, rows = dtSource };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页建筑数:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取单位查询参数
        /// </summary>
        /// <param name="Co_id">id</param>
        /// <returns></returns>
        public APIRst GetUnit(int Co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetCity(7000, Co_id);
                object obj = new { Total = dtSource.Rows.Count, rows = dtSource };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页建筑数:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取建筑查询参数
        /// </summary>
        /// <param name="Co_id">id</param>
        /// <returns></returns>
        public APIRst GetBuilding(int Co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetCity(100, Co_id);
                object obj = new { Total = dtSource.Rows.Count, rows = dtSource };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页建筑数:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取房间查询参数
        /// </summary>
        /// <param name="Co_id">id</param>
        /// <returns></returns>
        public APIRst GetRoom(int Co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetRoom(9000, Co_id);
                object obj = new { Total = dtSource.Rows.Count, rows = dtSource };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页建筑数:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        // 批量下发命令
        public APIRst YdBatchControlOfSsr(string v0Fun, string dataValue, string pStr)
        {
            APIRst rst = new APIRst();
            YdToGw gw = new YdToGw(user.Ledger, user.Uid);
            V0Fun fun = V0Fun.E;
            if (Enum.TryParse<V0Fun>(v0Fun, out fun) == false)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "控制命令错误";
                return rst;
            }
            //V0Fun fun = V0Fun.Ssr;
            try
            {
                foreach (string s in pStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    gw.YdToGwCmd(CommFunc.ConvertDBNullToInt32(s), fun, dataValue);
                }
                rst.data = "";
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("批量下发命令错误(YdBatchControlOfSsr)", ex.Message + ex.StackTrace);
            }
            return rst;
        }


    }
}