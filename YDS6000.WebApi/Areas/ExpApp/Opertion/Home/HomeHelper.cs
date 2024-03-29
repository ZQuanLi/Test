﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using System.IO;

namespace YDS6000.WebApi.Areas.ExpApp.Opertion.Home
{
    /// <summary>
    /// 首页页面
    /// </summary>
    public class HomeHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.ExpApp.Home.HomeBLL bll = null;
        /// <summary>
        /// 
        /// </summary>
        public HomeHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.ExpApp.Home.HomeBLL(user.Ledger, user.Uid);
        }


        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="ledger"></param>
        /// <param name="uSign"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public APIRst Login(string uSign, string pwd)
        {
            int ledger = 0;
            APIRst rst = new APIRst();
            try
            {
                //DataTable dt = new YDS6000.BLL.ExpApp.Home.HomeBLL(9999, 0).GetProjectList();
                //foreach (DataRow dr in dt.Rows)
                //    ledger = CommFunc.ConvertDBNullToInt32(dr["Rule"]);
                bll = new YDS6000.BLL.ExpApp.Home.HomeBLL(ledger, user.Uid);

                if (string.IsNullOrEmpty(uSign))
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "用户名不能为空";
                    return rst;
                }
                if (string.IsNullOrEmpty(pwd))
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "密码不能为空";
                    return rst;
                }
                WebConfig.GetSysConfig();
                DataTable dtSource = bll.GetUser(uSign);
                int nRows = dtSource.Rows.Count;
                if (nRows == 0)
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "没有此用户";
                    return rst;
                }
                else if (nRows != 1)
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "此用户有多个";
                    return rst;
                }

                int uid = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Uid"]);
                string uName = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["UName"]);
                string dbPwd = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["UPasswd"]);
                string project = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["Project"]);
                int role_id = 0;
                if (!pwd.Trim().Equals(dbPwd))
                {
                    //查询最后最后的登录时间，判断如果少于十五分钟且错误次数已经为五次，提示十五分钟后再次登录
                    //int num = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["LoginInt"]);
                    //DateTime dt = CommFunc.ConvertDBNullToDateTime(dtSource.Rows[0]["LoginDate"]);
                    //TimeSpan ts = new TimeSpan();
                    //ts = DateTime.Now - dt; //现在时间-数据库时间
                    //int Result = Convert.ToInt32(ts.TotalMinutes); //转换时间间隔为 分钟  Double型转化成Int型
                    //if (Result < 15 && num > 4)
                    //{
                    //    rst.err.code = (int)ResultCodeDefine.Error;
                    //    rst.err.msg = "登录的次数超过了规定次数，请十五分钟后再试";
                    //    return rst;
                    //}
                    ////登录密码错误，将登录错误次数+1
                    //bll.UpdateLoginErr(uSign);
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "密码错误";
                    return rst;
                }
                ////
                DataTable dtMd = bll.GetV3_UserOfModule(uid);
                int mid = 0;
                decimal price = 0;
                foreach (DataRow dr in dtMd.Rows)
                {
                    mid = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);
                    price = Math.Round(CommFunc.ConvertDBNullToDecimal(dr["Price"]), 4, MidpointRounding.AwayFromZero);
                }
                dynamic attach = new { mid = mid, price = price, step = new { s1 = 200, s2 = 400, p1 = price, p2 = price } };
                //////
                WebConfig.SysProject = project;
                //////
                int seed = new Random(Guid.NewGuid().GetHashCode()).Next(65, 91);
                string zm = CommFunc.NunberToChar(seed);
                zm = string.IsNullOrEmpty(zm) ? "APP" : zm;
                string ticket = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10) + zm + (ledger + seed).ToString() + zm + (seed + uid).ToString();
                string ccKey = ledger.ToString() + "APP" + uid.ToString();
                CacheUser cacheUser = new CacheUser();
                cacheUser.Ledger = ledger;
                cacheUser.Uid = uid;
                cacheUser.USign = uSign;
                cacheUser.Role_id = role_id;
                cacheUser.CacheKey = project + "->" + ledger + "->";
                //随机数前10位+ 种子数+ (种子数+用户数之和)
                cacheUser.Ticket = ticket;// Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10) + (seed + cacheUser.Uid).ToString().PadLeft(7, '0');
                cacheUser.Attach = attach;
                //
                //bll.UpdateLoginSue(uSign);
                rst.err.code = 0;
                rst.data = new { Ticket = cacheUser.Ticket, Name = uName };
                //
                int timeSpan = CommFunc.ConvertDBNullToInt32(ConfigHelper.GetAppSettings("Cached:Time"));
                timeSpan = timeSpan == 0 ? 15 : timeSpan;
                HttpRuntime.Cache.Insert(ccKey, cacheUser, null, DateTime.MaxValue, TimeSpan.FromMinutes(timeSpan));
                HttpContext.Current.Session["CacheUser"] = cacheUser;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("登录错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取用户编号信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetUserNumber()
        {
            APIRst rst = new APIRst() ;
            try
            {
                DataTable dtSource = bll.GetV3_User(user.Uid);
                int nRows = dtSource.Rows.Count;
                if (nRows == 0)
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "没有此用户,请重新登陆";
                    return rst;
                }
                string uName = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["CrmName"]);
                string code = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["Contract"]);
                string roomName = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["CoFullName"]);
                string moduleName = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["ModuleName"]);
                rst.err.code = 0;
                rst.data = new { Name = uName, RoomName = roomName, moduleName = moduleName, Code = code, };
                //
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("用户编号信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetUserInfo()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetV3_User(user.Uid);
                int nRows = dtSource.Rows.Count;
                if (nRows == 0)
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "没有此用户,请重新登陆";
                    return rst;
                }
                string uName = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["CrmName"]);
                string code = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["Contract"]);
                int co_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Co_id"]);
                string roomName = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["CoFullName"]);
                string moduleName = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["ModuleName"]);
                rst.err.code = 0;
                rst.data = new { Name = uName, RoomName = roomName, moduleName = moduleName, Code = code, Co_id = co_id };
                //
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取用户信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取快速缴费信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public APIRst GetPayQuick(string code)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetPayQuick(code);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Name = CommFunc.ConvertDBNullToString(s1["CrmName"]),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               Code = CommFunc.ConvertDBNullToString(s1["Contract"]),
                               SyAmt = CommFunc.ConvertDBNullToDecimal(s1["RdVal"]) <= 0 ? 0 : Math.Round(CommFunc.ConvertDBNullToDecimal(s1["RdVal"]) * CommFunc.ConvertDBNullToDecimal(s1["Price"]), 2, MidpointRounding.AwayFromZero),/*剩余金额*/
                               PayAmt = CommFunc.ConvertDBNullToDecimal(s1["RdVal"]) >= 0 ? 0 : Math.Round(Math.Abs(CommFunc.ConvertDBNullToDecimal(s1["RdVal"])) * CommFunc.ConvertDBNullToDecimal(s1["Price"]), 2, MidpointRounding.AwayFromZero),/*应缴金额*/
                               Penalty = 0,
                           };
                rst.err.code = 0;
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取获取快速缴费信息:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取拉合闸报表
        /// </summary>
        /// <param name="UName">操作人</param>
        /// <param name="StartTime">日期开始</param>
        /// <param name="EndTime">日期结束</param>
        /// <returns></returns>
        public APIRst GetYdSysLogOfCmd(string UName, DateTime StartTime, DateTime EndTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdSysLogOfList(UName, StartTime, EndTime);
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               UName = CommFunc.ConvertDBNullToString(s1["UName"]),
                               Content = CommFunc.ConvertDBNullToString(s1["Content"]),
                               Result = "访问成功",
                               Ctime = CommFunc.ConvertDBNullToDateTime(s1["Ctime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取系统操作日志内容错误(GetYdSysLogOfCmd):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取系统控制日志
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdSysLogCtrl(string UName, DateTime StartTime, DateTime EndTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdSysLogCtrl(UName, StartTime, EndTime);
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               UName = CommFunc.ConvertDBNullToString(s1["UName"]),
                               Content = CommFunc.ConvertDBNullToString(s1["Descr"]),
                               Result = CommFunc.ConvertDBNullToInt32(s1["ErrCode"]) == 0 ? "已下发" : (CommFunc.ConvertDBNullToInt32(s1["ErrCode"]) == 1 ? "执行成功" : CommFunc.ConvertDBNullToString(s1["ErrTxt"])),
                               Ctime = CommFunc.ConvertDBNullToDateTime(s1["Create_dt"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取系统操作日志内容错误(GetYdSysLogCtrl):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 历史数据
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="funType">采集项目</param>
        /// <param name="dataType">查询类型</param>
        /// <param name="startTime">开始日期</param>
        /// <returns></returns>
        public APIRst GetHisData(int module_id, string funType, string dataType, DateTime startTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DateTime start = startTime, end = startTime;
                if (dataType.Equals("hour"))
                {
                    start = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
                    end = new DateTime(start.Year, start.Month, start.Day, 23, 0, 0);
                }
                if (dataType.Equals("day"))
                {
                    start = new DateTime(start.Year, start.Month, 1);
                    end = start.AddMonths(1).AddDays(-1);
                }
                if (dataType.Equals("month"))
                {
                    start = new DateTime(start.Year, 1, 1);
                    end = start.AddYears(1).AddDays(-1);
                }
                DataTable dtSource = bll.GetHisData(module_id, funType, dataType, start, end);
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               TagTime = CommFunc.ConvertDBNullToDateTime(s1["TagTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               Fun_id = CommFunc.ConvertDBNullToInt32(s1["Fun_id"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               TagTimeS = CommFunc.ConvertDBNullToString(s1["TagTimeS"]),
                               Cnt = CommFunc.ConvertDBNullToInt32(s1["Cnt"]),
                               NewValue = s1["NewValue"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["NewValue"]).ToString(),
                               Value = s1["Value"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["Value"]).ToString(),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("查询类型下拉列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 数据对比
        /// </summary>
        /// <param name="areaId">区域ID号</param>
        /// <param name="unitId">单元ID号</param>
        /// <param name="projectId">项目ID号</param>
        /// <returns></returns>
        public APIRst GetCompare(int areaId, int unitId, int projectId)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetCompare(areaId, unitId, projectId);
                int curTime = CommFunc.ConvertDBNullToInt32(dtSource.Compute("sum(CurTims)", "true"));
                int preTime = CommFunc.ConvertDBNullToInt32(dtSource.Compute("sum(PreTims)", "true"));
                int per = preTime == 0 ? 0 : (int)((curTime - preTime) / (decimal)preTime) * 100;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               TagTime = CommFunc.ConvertDBNullToDateTime(s1["TagTime"]).ToString("yyyy年MM月"),
                               CurTims = CommFunc.ConvertDBNullToInt32(s1["CurTims"]),
                               PreTims = CommFunc.ConvertDBNullToInt32(s1["PreTims"]),
                               Per = CommFunc.ConvertDBNullToInt32(s1["Per"]),
                           };
                rst.data = new { list = res1.ToList(), per = per };
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("数据对比:" + ex.Message + ex.StackTrace);
            }
            return rst;
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
                object obj = new { total = total, rows = rows };
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
            dtSource.Columns.Add("SsrS", typeof(System.String));
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
                dr["SsrS"] = "未知";
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
                    {
                        int ssr = (int)CommFunc.ConvertDBNullToDecimal(var.lpszVal);
                        //ssr = ssr == 0 ? 0 : 1;
                        dr["Ssr"] = ssr;
                        if (ssr == 0)
                            dr["SsrS"] = "合闸";
                        else if (ssr == 129)
                            dr["SsrS"] = "恶性负载跳闸";
                        else if (ssr == 130)
                            dr["SsrS"] = "总功率超限跳闸";
                        else if (ssr == 132)
                            dr["SsrS"] = "移相器或半波跳闸";
                        else if (ssr == 136)
                            dr["SsrS"] = "定时断电拉闸";
                        else if (ssr == 144)
                            dr["SsrS"] = "远程拉闸";
                        else if (ssr == 160)
                            dr["SsrS"] = "过流超限跳闸";
                        else if (ssr == 192)
                            dr["SsrS"] = "单次功率超限跳闸";
                        else
                            dr["SsrS"] = "跳闸";

                    }
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
                           SsrS = CommFunc.ConvertDBNullToString(s1["SsrS"]),
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
        /// 下发控制
        /// </summary>
        /// <param name="pStr">module_id</param>
        /// <param name="fun_id"></param>
        /// <returns></returns>
        public APIRst SendCtrl(string pStr, int fun_id)
        {
            APIRst rst = new APIRst();
            YdToGw gw = new YdToGw(user.Ledger, user.Uid);
            try
            {
                foreach (string s in pStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    gw.YdToGwCmd(CommFunc.ConvertDBNullToInt32(s), fun_id);
                }
                rst.data = "";
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("批量下发命令错误(SendCtrl)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 区域下拉列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetAearCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetVp_coinfo(CoAttribV2_1.Area);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               text = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("区域下拉列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 单位下拉列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetUnitCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetVp_coinfo(CoAttribV2_1.Unit);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               text = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("单位下拉列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 项目下拉列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetProjectCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetVp_coinfo(CoAttribV2_1.Project);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               text = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("项目下拉列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 用电单元
        /// </summary>
        /// <returns></returns>
        public APIRst GetCellCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetVp_coinfo(CoAttribV2_1.Cell);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               text = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("用电单元:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 回路下拉列表
        /// </summary>
        /// <param name="co_id">用电单元ID号</param>
        /// <returns></returns>
        public APIRst GetModuleCombox(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetModuleCombox(co_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               text = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("回路下拉列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 采集项下拉列表
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <returns></returns>
        public APIRst GetFunCombox(int module_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetFunCombox(module_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToString(s1["FunType"]),
                               text = CommFunc.ConvertDBNullToString(s1["FunName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("采集项下拉列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 查询类型下拉列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetDataTypeCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                List<object> dd = new List<object>();
                dd.Add(new { id = "hour", text = "逐时" });
                dd.Add(new { id = "day", text = "逐日" });
                dd.Add(new { id = "month", text = "逐月" });
                rst.data = dd;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("查询类型下拉列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 控制列表
        /// </summary>
        /// <param name="module_id"></param>
        /// <returns></returns>
        public APIRst GetCtrlList(int module_id)
        {
            APIRst rst = new APIRst();
            try
            {
                List<object> list = new List<object>();
                DataTable dtSource = bll.GetCtrlList(module_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           group s1 by new { Alias = CommFunc.ConvertDBNullToString(s1["Alias"]) } into g1
                           select new
                           {
                               Alias = g1.Key.Alias,
                               LpszDbVarName = CommFunc.ConvertDBNullToString(g1.First()["LpszDbVarName"]),
                           };
                foreach (var o1 in res1)
                {
                    var res2 = from s1 in dtSource.AsEnumerable()
                               where CommFunc.ConvertDBNullToString(s1["Alias"]).Equals(o1.Alias)
                               select new
                               {
                                   Fun_id = CommFunc.ConvertDBNullToString(s1["Fun_id"]),
                                   FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               };
                    List<object> dispaly = new List<object>();
                    if (o1.Alias.Equals("Ssr"))
                    {
                        dispaly.Add(new { Value = 0, Descr = "合闸" });
                        dispaly.Add(new { Value = 1, Descr = "拉闸" });
                    }
                    list.Add(new { Alias = o1.Alias, TagName = o1.LpszDbVarName, Dispaly = dispaly, List = res2.ToList() });
                }
                rst.data = list;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获控制列表错误(GetCtrlList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        #region 隐患管理
        /// <summary>
        /// 采集异常告警—获取采集异常告警列表
        /// </summary>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="status">操作状态(可选)</param>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <returns></returns>
        public APIRst GetYdAlarmList(string startTime, string endTime, int status = 0, string strcName = "", string coName = "")
        {
            APIRst rst = new APIRst();
            try
            {
                if ("{strcName}".Equals(strcName))
                    strcName = "";
                if ("{coName}".Equals(coName))
                    coName = "";
                DataTable dtSource = bll.GetYdAlarmList(strcName, coName, status, CommFunc.ConvertDBNullToDateTime(startTime), CommFunc.ConvertDBNullToDateTime(endTime));
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Log_id = CommFunc.ConvertDBNullToInt32(s1["Log_id"]),
                               Status = CommFunc.ConvertDBNullToInt32(s1["Status"]),
                               Create_dt = CommFunc.ConvertDBNullToDateTime(s1["Create_dt"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CustAddr = CommFunc.ConvertDBNullToString(s1["CustAddr"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               Content = CommFunc.ConvertDBNullToString(s1["Content"]),
                               Appr_by = CommFunc.ConvertDBNullToInt32(s1["Appr_by"]),
                               CollectValue = CommFunc.ConvertDBNullToString(s1["CollectValue"]),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取告警内容错误(GetYdAlarmList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 工单列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetSolutionList(int status = 0, string strcName = "", string coName = "")
        {
            APIRst rst = new APIRst();
            try
            {
                if ("{strcName}".Equals(strcName))
                    strcName = "";
                if ("{coName}".Equals(coName))
                    coName = "";
                DataTable dtSource = bll.GetSolutionList(strcName, coName, status);
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Log_id = CommFunc.ConvertDBNullToInt32(s1["Log_id"]),
                               Status = CommFunc.ConvertDBNullToInt32(s1["Status"]),
                               SolutionName = CommFunc.ConvertDBNullToString(s1["SolutionName"]),
                               AlarmTime = CommFunc.ConvertDBNullToDateTime(s1["AlarmTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               SolutionImage = CommFunc.ConvertDBNullToString(s1["SolutionImage"]),
                               Content = CommFunc.ConvertDBNullToString(s1["Content"]),
                               ApprDt = CommFunc.ConvertDBNullToDateTime(s1["Appr_dt"]).ToString("yyyy-MM-dd HH:mm:ss"),//工单派发时间                               
                               CreateTime = CommFunc.ConvertDBNullToDateTime(s1["Create_dt"]).ToString("yyyy-MM-dd HH:mm:ss"),//隐患告警时间
                               CollectTime = CommFunc.ConvertDBNullToDateTime(s1["CollectTime"]).ToString("yyyy-MM-dd HH:mm:ss"),//隐患采集(生成)时间    
                               SolutionTime = CommFunc.ConvertDBNullToDateTime(s1["SolutionTime"]).ToString("yyyy-MM-dd HH:mm:ss"),//工单处理时间
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               CollectValue = CommFunc.ConvertDBNullToString(s1["CollectValue"]),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取告警内容错误(GetSolutionList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        #endregion
        /// <summary>
        /// 修改工单
        /// </summary>
        /// <returns></returns>
        public APIRst SolutionUpdate(int logId, string attached, string content)
        {
            APIRst rst = new APIRst();
            try
            {
                int num = 0;
                if (attached.Equals("{attached}"))
                    attached = "";
                if (content.Equals("{content}"))
                    content = "";
                num = bll.SetPic(logId, attached, content);

                //DataTable dtSource = JsonHelper.ToDataTable(data.Data);
                //string path = GetYdCollectOnExport(data.Data);
                rst.data = num;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("工单详情-完成工单设置错误(SetPic)" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 工单详情上传图片
        /// </summary>
        /// <returns></returns>
        public APIRst SetSolutionImage()
        {
            APIRst rst = new APIRst();
            try
            {
                var fileCollectionBase = HttpContext.Current.Request.Files;
                string attached = "";
                if (fileCollectionBase.Count > 0)
                {
                    var fileUploadPic = fileCollectionBase[0];

                    string ename = System.IO.Path.GetExtension(fileUploadPic.FileName).ToLower();
                    if (ename != ".jpg" && ename != ".jpeg" && ename != ".gif" && ename != ".png")
                        throw new Exception("不允许上传的文件类型(允许的类型：.jpg/.jpeg/.gif/.png)");
                    //判断附件大小是否符合不大于20MB
                    //double fileLength = fileUploadPic.ContentLength / (1024.0 * 1024.0);
                    //if (fileLength > 1.0)
                        //throw new Exception("图片最大不能超过1MB");
                    //获取文件名（或者重命名）
                    var uploadResult = ToUpload(fileUploadPic, "files/img/solution/", "image");
                    if (uploadResult.error > 0)
                    {
                        FileLog.WriteLog("上载图片错误:", uploadResult.message);
                        throw new Exception("上载图片错误:" + uploadResult.message);
                        //return Json(new { rst = false, msg = uploadResult.message, data = "" });
                    }
                    else
                    {
                        attached = uploadResult.url;//取出服务器虚拟路径,存储上传文件 
                    }
                }
                attached = string.IsNullOrEmpty(attached) ? CommFunc.ConvertDBNullToString(HttpContext.Current.Request["pAttached"]) : attached;
                rst.data = attached;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("工单详情-上传图片错误(SetSolutionImage)" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fb">HttpPostedFileBase</param>
        /// <param name="directoryUrl">directoryUrl 目录URL 如“img/adr”</param>
        /// <param name="dirType">dirType文件类型</param>
        /// <returns></returns>
        public static dynamic ToUpload(HttpPostedFile fb, string directoryUrl, string dirType)
        {

            Stream sm = null;
            FileStream fsm = null;
            int bufferLen = 1024;
            byte[] buffer = new byte[bufferLen];
            int contentLen = 0;
            string relativeUrl = "";//相对路径
            try
            {
                string fileName = Path.GetFileName(fb.FileName);
                string baseUrl = HttpContext.Current.Server.MapPath("/");
                relativeUrl = directoryUrl + fileName;
                string uploadPath = baseUrl + "/" + directoryUrl;//目录路径

                if (!System.IO.Directory.Exists(uploadPath)) //判断目录路径是否存在
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string fullPath = uploadPath + fileName;
                //if (File.Exists(fullPath) == true)
                //{
                //    return new { error = 1, message = "服务器上已经有了你正在上传的文件" };
                //}                
                sm = fb.InputStream;
                fsm = new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite);
                while ((contentLen = sm.Read(buffer, 0, bufferLen)) != 0)
                {
                    fsm.Write(buffer, 0, bufferLen);
                    fsm.Flush();
                }
            }
            catch (Exception ex)
            {
                return new { error = 1, message = ex.Message };
            }

            finally
            {
                if (fsm != null)
                {
                    fsm.Dispose();
                }
                if (sm != null)
                {
                    sm.Close();
                }
            }

            return new { error = 0, message = "上传成功", url = "/" + relativeUrl, dir = dirType };
        }

        /// <summary>
        /// 获取用户用电单元权限
        /// </summary>
        /// <returns></returns>
        public APIRst GetAreaPowerAPP()
        {
            APIRst rst = new APIRst();
            try
            {
                string AreaPowerStr = "";
                bool IsCheckAreaPower = bll.GetAreaPowerAPP(out AreaPowerStr);
                rst.err.code = 0;
                rst.data = new {  Co_id = AreaPowerStr };
                //
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取用户信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}