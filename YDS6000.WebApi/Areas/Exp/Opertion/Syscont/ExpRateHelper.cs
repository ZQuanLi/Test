using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Opertion.Syscont
{
    public partial class ExpRateHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Syscont.ExpRateBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpRateHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Syscont.ExpRateBLL(user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 获取费率信息
        /// </summary>
        /// <param name="Descr">筛选条件：费率描述</param>
        /// <returns></returns>
        public APIRst GetYdRateList(string Descr)
        {
            APIRst rst = new APIRst();
            try
            {

                var dt = this.GetYdRateList(0, Descr);
                rst.data = dt;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取费率列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private object GetYdRateList(int rate_id, string descr)
        {
            DataTable dtSource = bll.GetYdRateList(rate_id, descr);
            var res1 = from s1 in dtSource.AsEnumerable()
                       select new
                       {
                           Rate_id = CommFunc.ConvertDBNullToInt32(s1["Rate_id"]),
                           Descr = CommFunc.ConvertDBNullToString(s1["Descr"]),
                           Pri1st = CommFunc.ConvertDBNullToDecimal(s1["Pri1st"]),
                           Pri2nd = CommFunc.ConvertDBNullToDecimal(s1["Pri2nd"]),
                           Pri3rd = CommFunc.ConvertDBNullToDecimal(s1["Pri3rd"]),
                           Pri4th = CommFunc.ConvertDBNullToDecimal(s1["Pri4th"]),
                           T1st = CommFunc.ConvertDBNullToString(s1["T1st"]),
                           T2nd = CommFunc.ConvertDBNullToString(s1["T2nd"]),
                           T3rd = CommFunc.ConvertDBNullToString(s1["T3rd"]),
                           T4th = CommFunc.ConvertDBNullToString(s1["T4th"]),
                       };
            object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
            return obj;
        }

        /// <summary>
        /// 设置保存费率信息
        /// </summary>
        /// <param name="T1st">尖单价-对应的开始时间</param>
        /// <param name="T2nd">峰单价-对应的开始时间</param>
        /// <param name="T3rd">平单价-对应的开始时间</param>
        /// <param name="T4th">谷单价-对应的开始时间</param>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="Descr">费率描述</param>
        /// <param name="Pri1st">尖单价</param>
        /// <param name="Pri2nd">峰单价</param>
        /// <param name="Pri3rd">平单价</param>
        /// <param name="Pri4th">谷单价</param>
        /// <returns></returns>
        public APIRst SetSaveYdRate(string T1st, string T2nd, string T3rd, string T4th, int Rate_id, string Descr, decimal Pri1st, decimal Pri2nd, decimal Pri3rd, decimal Pri4th)
        {
            APIRst rst = new APIRst();
            try
            {
                v1_rateVModel rv = new v1_rateVModel();
                v1_rateCfg cfg = new v1_rateCfg();
                cfg.T1st = T1st;
                cfg.T2nd = T2nd;
                cfg.T3rd = T3rd;
                cfg.T4th = T4th;

                rv.Rate_id = Rate_id;
                rv.Descr = Descr;
                rv.Pri1st = Pri1st;
                rv.Pri2nd = Pri2nd;
                rv.Pri3rd = Pri3rd;
                rv.Pri4th = Pri4th;

                rv.DataCfg = JsonHelper.Serialize(cfg);

                int cnt = bll.SaveYdRate(rv);
                object obj = GetYdRateList(rv.Rate_id, "");
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取费率列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除费率信息
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <returns></returns>
        public APIRst GetDelYdRate(int Rate_id)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Rate_id == 0)
                    throw new Exception("费率ID号不能为空");
                int cc = bll.GetDelYdRate(Rate_id);
                rst.data = cc;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取费率列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

    }
}