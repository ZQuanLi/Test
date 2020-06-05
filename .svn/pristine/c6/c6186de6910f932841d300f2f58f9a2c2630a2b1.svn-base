using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.IFSMgr.Opertion.Monitor
{
    public partial class MonitorHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.IFSMgr.Monitor.MonitorBLL bll = null;
        public MonitorHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.IFSMgr.Monitor.MonitorBLL(user.Ledger, user.Uid);
        }

        public APIRst GetRealVal(string tag)
        {
            //CacheUser user = WebConfig.GetSession();
            string key = (user == null || user.Uid == 0) ? "" : user.CacheKey;
            if (string.IsNullOrEmpty(key))
                key = WebConfig.MemcachKey;
            key = key + tag;
            //FileLog.WriteLog("key:" + key);
            int i = 0;
            RstVar var = null;
            while (++i <= 2)
            {
                var = MemcachedMgr.GetVal<RstVar>(key);
                if (var != null) break;
                System.Threading.Thread.Sleep(50);
            }
            APIRst rst = new APIRst() { rst = true };
            if (var == null)
            {
                rst.rst = false;
                rst.data = null;
                rst.err = new APIErr() { code = -1, msg = "无数据" };
            }
            else
            {
                rst.data = new { lpszVal = var.lpszVal, lpszdateTime = var.lpszdateTime };
            }
            return rst;
        }

        /// <summary>
        /// 批量获取实时数据
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public APIRst GetBatchRealVal(List<string> tags)
        {
            //List<string> tag = new List<string>();
            //CacheUser user = WebConfig.GetSession();
            string key = (user == null || user.Uid == 0) ? "" : user.CacheKey;

            if (string.IsNullOrEmpty(key))
                key = WebConfig.MemcachKey;
            string ccKey = "";
            List<object> dd = new List<object>();
            //foreach (var k in tags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            foreach (var k in tags)
            {
                ccKey = key + k;
                int i = 0;
                RstVar var = null;
                if (k.Equals("2.WD01.Val"))
                {
                    //FileLog.WriteLog(ccKey);
                }
                while (++i <= 2)
                {
                    var = MemcachedMgr.GetVal<RstVar>(ccKey);
                    if (var != null) break;
                    System.Threading.Thread.Sleep(50);
                }
                if (var == null)
                {
                    dd.Add(new { tag = k, value = "" });
                    //dd.Add(new { tag = k, value = new { lpszVal = (new Random(Guid.NewGuid().GetHashCode()).Next(100,250)).ToString(), lpszdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }});
                }
                else
                {
                    dd.Add(new { tag = k, value = new { lpszVal = var.lpszVal, lpszdateTime = var.lpszdateTime } });
                }
            }
            APIRst rst = new APIRst() { rst = true, data = dd };
            return rst;
        }

        /// <summary>
        /// 下发命令数据
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public APIRst SendVal(List<string> tags,string dataValue)
        {
            List<object> dd = new List<object>();
            APIRst rst = new APIRst() { rst = true };
            try
            {
                DataTable dtInfo = bll.GetTagInfo(string.Join(",", tags.ToArray()));
                foreach (var k in tags)
                {
                    DataRow[] arr = dtInfo.Select("LpszDbVarName='" + k + "'");
                    if (arr.Count() == 0)
                    {
                        dd.Add(new { tag = k, rst = false, msg = "没有此采集点" });
                        continue;
                    }                  
                    CommandVModel cmd = ModelHandler<CommandVModel>.FillModel(arr[0]);
                    cmd.Action = 1;
                    cmd.LpszDbVarName = CommFunc.ConvertDBNullToString(arr[0]["TagName"]);
                    cmd.DataValue = dataValue;
                    cmd.IsNDb = true;
                    bll.UpdateMapDataVal(cmd.Module_id, cmd.Fun_id, cmd.DataValue);
                    //
                    ListenVModel vm = new ListenVModel() { cfun = ListenCFun.cmd.ToString(), content = JsonHelper.Serialize(cmd) };
                    string msg = "";
                    bool rr = CacheMgr.SendCollectVal(vm, out msg);
                    dd.Add(new { tag = k, rst = rr, msg = msg });
                }
                rst.data = dd;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.data = "";
                rst.err = new APIErr() { code = -1, msg = ex.Message };
            }
            return rst;
        }

        /// <summary>
        /// 获取控制状态信息是否成功
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public APIRst GetSuccess(List<string> tags)
        {
            List<object> dd = new List<object>();
            APIRst rst = new APIRst() { rst = true };
            try
            {
                DataTable dtInfo = bll.GetTagInfo(string.Join(",", tags.ToArray()));
                foreach (var k in tags)
                {
                    DataRow[] arr = dtInfo.Select("LpszDbVarName='" + k + "'");
                    if (arr.Count() == 0)
                    {
                        dd.Add(new { tag = k, rst = false, msg = "没有此采集点" });
                        continue;
                    }
                    string dataValue = CommFunc.ConvertDBNullToString(arr[0]["DataValue"]);
                    int status = CommFunc.ConvertDBNullToInt32(arr[0]["Status"]);
                    DateTime update_dt = CommFunc.ConvertDBNullToDateTime(arr[0]["Update_dt"]);
                    ////////////////////////////////////
                    var var = this.GetRstVar(k);
                    if (var == null)
                    {
                        dd.Add(new { tag = k, rst = false, msg = "不成功" });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dataValue))
                        {
                            if (CommFunc.ConvertDBNullToDecimal(var.lpszVal) == CommFunc.ConvertDBNullToDecimal(dataValue))
                            {
                                dd.Add(new { tag = k, rst = true, msg = "成功" });
                            }
                            else
                            {
                                if (DateTime.Now >= update_dt.AddMinutes(2))
                                    dd.Add(new { tag = k, rst = false, msg = "失败" });
                                else
                                    dd.Add(new { tag = k, rst = false, msg = "正在执行" });
                            }
                        }
                        else
                        {
                            dd.Add(new { tag = k, rst = false, msg = "无控制值" });
                        }
                    }
                }
                rst.data = dd;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.data = "";
                rst.err = new APIErr() { code = -1, msg = ex.Message };
            }
            return rst;
        }


        private RstVar GetRstVar(string tag)
        {
            string key = (user == null || user.Uid == 0) ? "" : user.CacheKey;
            if (string.IsNullOrEmpty(key))
                key = WebConfig.MemcachKey;
            key = key + tag;
            RstVar var = null;
            int i = 0;
            while (++i <= 2)
            {
                var = MemcachedMgr.GetVal<RstVar>(key);
                if (var != null) break;
                System.Threading.Thread.Sleep(50);
            }
            return var;
        }


        private static object lockObj = new Object();

        /// <summary>
        /// 微信回调
        /// </summary>
        /// <returns></returns>
        public APIRst ResultNotify()
        {
            APIRst dd = new APIRst() { rst = true, data = "" };
            bool rst = false;
            string out_trade_no = "", errTxt = "";
            try
            {
                WxPayAPI.ResultNotify resultNotify = new WxPayAPI.ResultNotify(System.Web.HttpContext.Current);
                rst = resultNotify.GetProcessNotify(out out_trade_no, out errTxt);
                if (rst == true)
                {
                    lock (lockObj)
                    {/*一条一条来*/
                        new YDS6000.WebApi.ResultNotify().Pay(out_trade_no);
                    }
                }
                if (rst == false)
                    FileLog.WriteLog("微信支付回调未支付成功,商户单号:", out_trade_no);

            }
            catch (Exception ex)
            {
                errTxt = ex.Message;
                FileLog.WriteLog("微信支付回调错误ResultNotify", ex.Message + ex.StackTrace);
            }
            return dd;
        }
    }
}