using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using WxPayAPI;

namespace YDS6000.WebApi.Areas.ExpApp.Opertion.Estate
{
    public partial class EstateHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.ExpApp.Estate.EstateBLL bll = null;
        public EstateHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.ExpApp.Estate.EstateBLL(user.Ledger, user.Uid);
        }


        public APIRst GetAlarmInfo()
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetAlarmInfo();
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
        /// 获取用户编号信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public APIRst GetUserInfo()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetUserInfo();
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
                string rdAmt = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["RdAmt"]).ToString("f2");
                rst.err.code = 0;
                rst.data = new { Name = uName, RoomName = roomName, moduleName = moduleName, Code = code, Co_id = co_id , RdAmt = rdAmt };
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
        /// 账单
        /// </summary>
        /// <returns></returns>
        public APIRst GetBill()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetBill();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Log_id = CommFunc.ConvertDBNullToLong(s1["Log_id"]),
                               CDate = CommFunc.ConvertDBNullToDateTime(s1["CDate"]).ToString("yyyy-MM-dd"),
                               WaterUseVal = CommFunc.ConvertDBNullToDecimal(s1["WaterUseVal"]).ToString("f2"),
                               WaterPrice = CommFunc.ConvertDBNullToDecimal(s1["Price"]).ToString("f2"),
                               WaterAmt = CommFunc.ConvertDBNullToDecimal(s1["LastAmt"]).ToString("f2"),
                               EleUseVal = CommFunc.ConvertDBNullToDecimal(s1["EleUseVal"]).ToString("f2"),
                               ElePrice = CommFunc.ConvertDBNullToDecimal(s1["ElePrice"]).ToString("f2"),
                               EleUseAmt = CommFunc.ConvertDBNullToDecimal(s1["EleUseAmt"]).ToString("f2"),
                               ChargAmt = CommFunc.ConvertDBNullToDecimal(s1["ChargVal"]).ToString("f2"),
                               Status = CommFunc.ConvertDBNullToInt32(s1["Status"]),
                               PayTime = CommFunc.ConvertDBNullToDateTime(s1["PayTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               //LastTime = CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).ToString("yyyy-MM-dd"),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取账单错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 账单明细
        /// </summary>
        /// <param name="start">开始日期</param>
        /// <param name="end">介绍日期</param>
        /// <param name="type">=0 ，全部记录=1扣费记录 =2 充值记录</param>
        /// <returns></returns>
        public APIRst GetBillDetail(DateTime start, DateTime end, int type = 0)
        {
            APIRst rst = new APIRst();
            List<object> data = new List<object>();

            try
            {
                DataTable dt01 = null, dt02 = null;
                if (type == 0 || type == 1)
                    dt01 = bll.GetBillDetail_01(start, end);
                if (type == 0 || type == 2)
                    dt02 = bll.GetBillDetail_02(start, end);
                if (dt01 != null)
                {
                    var res1 = from s1 in dt01.AsEnumerable()
                               select new
                               {
                                   Log_id = CommFunc.ConvertDBNullToLong(s1["Log_id"]),
                                   CDate = CommFunc.ConvertDBNullToDateTime(s1["CDate"]).ToString("yyyy-MM-dd"),
                                   WaterUseVal = CommFunc.ConvertDBNullToDecimal(s1["WaterUseVal"]).ToString("f2"),
                                   WaterPrice = CommFunc.ConvertDBNullToDecimal(s1["Price"]).ToString("f2"),
                                   WaterAmt = CommFunc.ConvertDBNullToDecimal(s1["LastAmt"]).ToString("f2"),
                                   EleUseVal = CommFunc.ConvertDBNullToDecimal(s1["EleUseVal"]).ToString("f2"),
                                   ElePrice = CommFunc.ConvertDBNullToDecimal(s1["ElePrice"]).ToString("f2"),
                                   EleUseAmt = CommFunc.ConvertDBNullToDecimal(s1["EleUseAmt"]).ToString("f2"),
                                   ChargAmt = CommFunc.ConvertDBNullToDecimal(s1["ChargVal"]).ToString("f2"),
                                   Status = CommFunc.ConvertDBNullToInt32(s1["Status"]),
                                   PayTime = CommFunc.ConvertDBNullToDateTime(s1["PayTime"]).ToString("yyyy-MM-dd HH:mm:ss"),                                
                                   SyAmt = CommFunc.ConvertDBNullToDecimal(s1["SyAmt"]).ToString("f2"),
                               };
                    data.Add(res1.ToList());
                }
                if (dt02 != null)
                {
                    var res2 = from s1 in dt02.AsEnumerable()
                               select new
                               {
                                   Log_id = CommFunc.ConvertDBNullToLong(s1["Log_id"]),
                                   CDate = CommFunc.ConvertDBNullToDateTime(s1["CDate"]).ToString("yyyy-MM-dd"),
                                   PayAmt = CommFunc.ConvertDBNullToDecimal(s1["PayAmt"]).ToString("f2"),
                                   SyAmt = CommFunc.ConvertDBNullToDecimal(s1["SyAmt"]).ToString("f2"),
                                   PayTime = CommFunc.ConvertDBNullToDateTime(s1["PayTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               };
                    data.Add(res2.ToList());
                }
                rst.data = data;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("账单明细:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 支付购电
        /// </summary>
        /// <param name="co_id">房间ID号</param>
        /// <param name="payAmt">充值金额</param>        
        /// <returns></returns>
        public APIRst ForPay(int co_id, decimal payAmt)
        {
            APIRst rst = new APIRst();
            if (co_id == 0)
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
                DataTable dtSource = bll.GetUserInfo();
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
                decimal rdAmt = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["RdAmt"]);

                pay.Co_id = co_id;
                pay.Module_id = 0;
                pay.ModuleAddr = "" ; // CommFunc.ConvertDBNullToString(Request["ModuleAddr"]);
                pay.Fun_id = 0;
                pay.PayAmt = payAmt;
                pay.PayVal = payAmt; //Math.Round(pay.PayAmt / pay.Price, 2, MidpointRounding.AwayFromZero);
                pay.PayType = 1;
                pay.IsWrong = 0;
                pay.IsPay = 10;
                pay.PayStartTime = DateTime.Now;
                pay.PayEndTime = DateTime.Now;
                if (pay.Co_id == 0)
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
                string strBody = string.Format("【{0}{1}】充值", roomName, pay.ModuleAddr);//商品描述
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
                bll.AddPayLog(pay,rdAmt);

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