using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using WxPayAPI;

namespace YDS6000.WebApi.Areas.ExpApp.Opertion.Energy
{
    public class EnergyHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.ExpApp.Energy.EnergyBLL bll = null;
        public EnergyHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.ExpApp.Energy.EnergyBLL(user.Ledger, user.Uid);
        }

        /// <summary>
        /// 本月用电信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetMonthUseVal()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetMonthUseVal(user.Attach.mid);
                decimal useVal = 0, useAmt = 0;
                int p = 0;
                foreach (DataRow dr in dtSource.Rows)
                {
                    useVal = CommFunc.ConvertDBNullToDecimal(dr["UseVal"]);
                    useAmt = CommFunc.ConvertDBNullToDecimal(dr["UseAmt"]);
                }
                if (useVal <= user.Attach.step.s1)
                    p = 1;
                else if (useVal <= user.Attach.step.s2)
                    p = 2;
                else p = 3;
                rst.data = new { UseVal = useVal, UseAmt = useAmt, Step = p };
                rst.rst = true;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取本月用电信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 支付购电信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetPayUseVal()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetPayUseVal(user.Attach.mid);
                int module_id = 0, scale = 0;
                string moduleAddr = "";
                decimal multiply = 0,  price = 0, lastVal = 0, rdVal = 0;
                foreach (DataRow dr in dtSource.Rows)
                {
                    module_id = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);
                    moduleAddr = CommFunc.ConvertDBNullToString(dr["ModuleAddr"]);
                    multiply = CommFunc.ConvertDBNullToDecimal(dr["Multiply"]);
                    price = CommFunc.ConvertDBNullToDecimal(dr["Price"]);
                    scale = CommFunc.ConvertDBNullToInt32(dr["Scale"]);
                    lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                    rdVal = CommFunc.ConvertDBNullToDecimal(dr["RdVal"]);
                }
                decimal payAmt = rdVal >= 0 ? 0 : Math.Round(Math.Abs(rdVal) * price, 2, MidpointRounding.AwayFromZero);/*应缴金额*/
                decimal syAmt = rdVal <= 0 ? 0 : Math.Round(rdVal * price, 2, MidpointRounding.AwayFromZero);/*剩余金额*/
                rst.data = new { Module_id = module_id, SyAmt = syAmt, PayAmt = payAmt, Penalty = 0 };
                rst.rst = true;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取本月用电信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取电量电费
        /// </summary>
        /// <returns></returns>
        public APIRst GetElectricity()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetElectricity(user.Attach.mid);
                decimal useVal = 0, useAmt = 0;
                decimal upUseVal = 0, upUseAmt = 0;
                decimal lastVal = 0;
                decimal pri1 = 0, pri2 = 0; ;
                foreach (DataRow dr in dtSource.Rows)
                {
                    useVal = CommFunc.ConvertDBNullToDecimal(dr["UseVal"]);
                    useAmt = CommFunc.ConvertDBNullToDecimal(dr["UseAmt"]);
                    upUseVal = CommFunc.ConvertDBNullToDecimal(dr["UpUseVal"]);
                    upUseAmt = CommFunc.ConvertDBNullToDecimal(dr["UpUseAmt"]);
                    lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                }

                if (useVal <= user.Attach.step.s1)
                    pri1 = user.Attach.step.p1;
                else if (useVal <= user.Attach.step.s2)
                    pri1 = user.Attach.step.p2;
                else pri1 = user.Attach.step.p3;
                //
                if (upUseVal <= user.Attach.step.s1)
                    pri2 = user.Attach.step.p1;
                else if (upUseVal <= user.Attach.step.s2)
                    pri2 = user.Attach.step.p2;
                else pri2 = user.Attach.step.p3;
                //
                useAmt = Math.Round(useVal * pri1, 2, MidpointRounding.AwayFromZero);
                upUseAmt = Math.Round(upUseVal * pri2, 2, MidpointRounding.AwayFromZero);

                rst.data = new { UseVal = useVal, UseAmt = useAmt, UpUseVal = upUseVal, UpUseAmt = upUseAmt, LastVal = lastVal ,Step = user.Attach.step };
                rst.rst = true;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取本月用电信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 购电记录
        /// </summary>
        /// <returns></returns>
        public APIRst GetPayLog(DateTime startTime, DateTime endTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetPayLog(user.Attach.mid, startTime, endTime);
                int total = dtSource.Rows.Count;
                decimal payAmt = CommFunc.ConvertDBNullToDecimal(dtSource.Compute("sum(PayAmt)", "true"));
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Log_id = CommFunc.ConvertDBNullToLong(s1["Log_id"]),
                               Create_dt = CommFunc.ConvertDBNullToDateTime(s1["Create_dt"]),
                               PayVal = CommFunc.ConvertDBNullToDecimal(s1["PayVal"]),
                               PayAmt = CommFunc.ConvertDBNullToDecimal(s1["PayAmt"]),
                               PayModel = CommFunc.ConvertDBNullToInt32(s1["IsPay"]) != 0 ? "微信支付" : "现金支付",
                           };
                rst.data = new { row = res1.ToList(), total = total, payAmt = payAmt };
                rst.rst = true;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("购电记录信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 月份用能分析
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public APIRst GetMonthAnalysis(DateTime time)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetMonthAnalysis(user.Attach.mid, time);               
                foreach (DataRow dr in dtSource.Rows)
                {
                    if (CommFunc.ConvertDBNullToInt32(dr["Step"]) == 0) continue;/*不存在月份*/
                    decimal useVal = CommFunc.ConvertDBNullToDecimal(dr["UseVal"]);
                    decimal pri1 = 0;
                    if (useVal <= user.Attach.step.s1)
                    {
                        pri1 = user.Attach.step.p1;
                        dr["Step"] = 1;
                    }
                    else if (useVal <= user.Attach.step.s2)
                    {
                        pri1 = user.Attach.step.p2;
                        dr["Step"] = 2;
                    }
                    else
                    {
                        pri1 = user.Attach.step.p3;
                        dr["Step"] = 3;
                    }
                    //dr["UseAmt"] = Math.Round(useVal * pri1, 2, MidpointRounding.AwayFromZero);
                }
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Month = CommFunc.ConvertDBNullToInt32(s1["Month"]),
                               UseVal = CommFunc.ConvertDBNullToDecimal(s1["UseVal"]),
                               UseAmt = CommFunc.ConvertDBNullToDecimal(s1["UseAmt"]),
                               Step = CommFunc.ConvertDBNullToInt32(s1["Step"]),
                           };
                rst.data = new { row = res1.ToList(), total = dtSource.Rows.Count };
                rst.rst = true;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("月份用能分析错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 支付购电
        /// </summary>
        /// <param name="module_id">电表ID号</param>
        /// <param name="payAmt">充值金额</param>        
        /// <returns></returns>
        public APIRst ForPay(int module_id, decimal payAmt)
        {
            APIRst rst = new APIRst();
            if (module_id == 0)
            {
                rst.rst = false;
                rst.err = new APIErr() { code = 100, msg = "设备编号不能为空" };
                return rst;
            }
            if (payAmt <= 0)
            {
                rst.rst = false;
                rst.err = new APIErr() { code = 100, msg = "充值金额不能为零" };
                return rst;
            }
            //App支付预处理
            try
            {
                v4_pay_logVModel pay = new v4_pay_logVModel();
                #region 登记付款log记录
                DataTable dtSource = bll.GetPayUseVal(module_id);
                if (dtSource.Rows.Count != 1)
                {
                    rst.rst = false;
                    rst.err = new APIErr() { code = 100, msg = "设备信息不存在" };
                    return rst;
                }
                pay.Price = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["Price"]);
                if (pay.Price == 0)
                {
                    rst.rst = false;
                    rst.err = new APIErr() { code = 100, msg = "单价为零" };
                    return rst;
                }                  
                pay.Co_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Co_id"]);
                pay.Module_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Module_id"]);
                pay.ModuleAddr = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["ModuleAddr"]); ; // CommFunc.ConvertDBNullToString(Request["ModuleAddr"]);
                pay.Fun_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Fun_id"]);
                pay.PayAmt = payAmt;
                pay.PayVal = Math.Round(pay.PayAmt / pay.Price, 2, MidpointRounding.AwayFromZero);
                pay.PayType = 1;
                pay.IsWrong = 0;
                pay.IsPay = 10;
                pay.PayStartTime = DateTime.Now;
                pay.PayEndTime = DateTime.Now;
                if (pay.Co_id == 0 || pay.Module_id == 0 || string.IsNullOrEmpty(pay.ModuleAddr))
                {
                    rst.rst = false;
                    rst.err = new APIErr() { code = 100, msg = "充值信息缺失" };
                    return rst;
                }
                if (pay.PayAmt <= 0 || pay.PayVal <= 0)
                {
                    rst.rst = false;
                    rst.err = new APIErr() { code = 100, msg = "充值金额" + pay.PayAmt + "单价:" + pay.Price + "转换电量:" + pay.PayVal + "不能为零" };
                    return rst;
                }
                if (pay.PayStartTime.Year == 1900 || pay.PayEndTime.Year == 1900 || pay.PayEndTime < pay.PayStartTime)
                {
                    rst.rst = false;
                    rst.err = new APIErr() { code = 100, msg = "充值日期错误" };
                    return rst;
                }
                #endregion

                #region 发起微信支付
                string strBody = string.Format("【{0}{1}】充值", CommFunc.ConvertDBNullToString(dtSource.Rows[0]["CoFullName"]), pay.ModuleAddr);//商品描述
                string out_tran_no = "";
                new WxPayAPI.WxPay();/*初始下微信支付环境*/
                JsApiPay jsApiPay = new JsApiPay();
                jsApiPay.openid = "";
                jsApiPay.total_fee = (int)(payAmt * 100);
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(strBody, "附件信息:智能用电", "APP", out out_tran_no);
                //FileLog.WriteLog("步骤一:", "单号:" + out_tran_no);
                WxPayData wxJsApiParam = jsApiPay.GetAppApiParameters();//获取H5调起JS API参数
                object order = new
                {
                    appid = wxJsApiParam.GetValue("appid").ToString(),
                    partnerid = wxJsApiParam.GetValue("partnerid").ToString(),
                    prepayid = wxJsApiParam.GetValue("prepayid").ToString(),
                    package = "Sign=WXPay",
                    noncestr = wxJsApiParam.GetValue("noncestr").ToString(),
                    timestamp = CommFunc.ConvertDBNullToInt32(wxJsApiParam.GetValue("timestamp")),
                    sign = wxJsApiParam.GetValue("sign").ToString(),
                };
                pay.Trade_no = out_tran_no;
                #endregion

                if (string.IsNullOrEmpty(pay.Trade_no))
                {
                    rst.rst = false;
                    rst.err = new APIErr() { code = 100, msg = "微信支付产生单号为空" };
                    return rst;
                }
                bll.AddPayLog(pay);

                rst.rst = true;
                rst.data = order;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("支付失败:", ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}