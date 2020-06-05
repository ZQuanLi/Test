using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Energy.Controllers
{
    public partial class MonitorHelper
    {
        /// <summary>
        /// 获取实时列表数据
        /// </summary>
        /// <returns></returns>
        public APIRst GetRealList(int co_id,string moduleName)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetRealList(co_id, moduleName);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               TagName = CommFunc.ConvertDBNullToString(s1["LpszDbVarName"]),
                               BuildName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               LpszVal = CommFunc.ConvertDBNullToString(s1["LastVal"]),
                               LpszdateTime = s1["LastTime"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               Status = "",
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取实时列表数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取采集点实时数据错误
        /// </summary>
        /// <param name="tag">采集点号</param>
        /// <returns></returns>
        public APIRst GetRealTime(string tag)
        {
            //保留两种状态，正常和异常。通讯异常的，暂无数据的，都显示为异常
            APIRst rst = new APIRst();
            try
            {

                string key = (user == null || user.Uid == 0) ? "" : user.CacheKey + tag;
                //if (string.IsNullOrEmpty(key))
                //    key = WebConfig.MemcachKey;                    
                string[] arr = key.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                string ns = string.Join(".", arr, 0, arr.Length - 1);
                string status = "正常";

                RstVar var = this.GetStatus(key);
               
                RstVar Ua = this.GetStatus(ns + ".Ua");
                if (Ua == null)
                {
                    RstVar com = this.GetStatus(ns + ".Comm");
                    if (com == null)
                    {
                        status = "异常";
                        //FileLog.WriteLog("key:" + tag + " 无状态信息");
                    }
                    else
                    {
                        if (CommFunc.ConvertDBNullToDecimal(com.lpszVal) > 0)
                        {
                            status = "异常";
                            //FileLog.WriteLog("key:" + tag + " Com: " + JsonHelper.Serialize(com));
                        }
                    }

                    if (tag.Contains("R2B1.2.3"))
                    {
                        //FileLog.WriteLog("key:" + ns + ".Comm " + " com: " + JsonHelper.Serialize(com));
                    }
                }
                else
                {
                    if (CommFunc.ConvertDBNullToDecimal(Ua.lpszVal) == 0)
                    {
                        status = "异常";
                        // FileLog.WriteLog("key:" + tag + "Ua: " + JsonHelper.Serialize(Ua));
                    }
                    else
                    {
                        //if (CommFunc.ConvertDBNullToDecimal(Ua.errCode) == -1)
                        //    status = "异常";
                    }
                }


                if (tag.Contains("R2B1.2.3"))
                {
                    //FileLog.WriteLog("key:" + tag +  "  tag: " + JsonHelper.Serialize(var));
                    //FileLog.WriteLog("key:" + ns + ".Ua " + "  Ua: " + JsonHelper.Serialize(Ua));
                }

                if (!user.CacheKey.Contains("EngShBen"))
                {
                    status = "正常";
                    if (var == null)
                        status = "异常";
                    else
                    {
                        if (DateTime.Now > var.lpszdateTime.AddMinutes(1))
                            status = "异常";
                    }
                }
                //FileLog.WriteLog("key:" + key + "  值: " + JsonHelper.Serialize(var));
                if (var != null)
                    rst.data = new { LpszVal = var.lpszVal, LpszdateTime = var.lpszdateTime.ToString("yyyy-MM-dd HH:mm:ss"), Status = status };
                else
                    rst.data = new { LpszVal = "", LpszdateTime = "", Status = status };

                //RstVar var = null;
                //int i = 0;
                //while (++i <= 3)
                //{
                //    var = MemcachedMgr.GetVal<RstVar>(key);
                //    if (var != null) break;
                //    System.Threading.Thread.Sleep(50);
                //}
                //if (var == null)
                //{
                //    rst.rst = true;
                //    rst.data = new { LpszVal = "", LpszdateTime = "", Status = "异常" };
                //}
                //else
                //{
                //    string ss = "异常";
                //    if (CommFunc.ConvertDBNullToDecimal(var.lpszVal) == 0)
                //    {
                //        ss = "断线";
                //    }
                //    else
                //    {
                //        if (var.lpszdateTime.AddMinutes(15) > DateTime.Now)
                //        {
                //            ss = "正常";
                //        }
                //    }
                //    rst.data = new { LpszVal = var.lpszVal, LpszdateTime = var.lpszdateTime.ToString("yyyy-MM-dd HH:mm:ss"), Status = ss };
                //}
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取采集点实时数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        private RstVar GetStatus(string key)
        {
            RstVar var = null;
            int i = 0;
            while (++i <= 3)
            {
                var = MemcachedMgr.GetVal<RstVar>(key);
                if (var != null) break;
                System.Threading.Thread.Sleep(50);
            }
            return var;
        }

        /// <summary>
        /// 获取回路的实时曲线数据
        /// </summary>
        /// <param name="co_id">回路ID号</param>
        /// <returns></returns>
        public APIRst GetRealChart(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetRealChart(co_id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取回路的实时曲线数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}