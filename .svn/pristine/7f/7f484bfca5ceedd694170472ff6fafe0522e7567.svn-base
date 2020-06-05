using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    partial class AlarmHelper
    {
        public APIRst GetYdAlarmOfChargeList(string strcName,string coName,string isPay,string startTime,string endTime)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdAlarmOfChargeList(strcName, coName, isPay, CommFunc.ConvertDBNullToDateTime(startTime), CommFunc.ConvertDBNullToDateTime(endTime));
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               IsPay = CommFunc.ConvertDBNullToInt32(s1["IsPay"]),
                               IsPayS = CommFunc.ConvertDBNullToString(s1["IsPayS"]),
                               Trade_no = CommFunc.ConvertDBNullToString(s1["Trade_no"]),
                               PayAmt = CommFunc.ConvertDBNullToDecimal(s1["PayAmt"]).ToString("f2"),
                               PayVal = CommFunc.ConvertDBNullToDecimal(s1["PayVal"]).ToString("f2"),
                               ErrCode = CommFunc.ConvertDBNullToInt32(s1["ErrCode"]),
                               ErrTxt = ((CommFunc.ConvertDBNullToInt32(s1["IsPay"]) == 1 && CommFunc.ConvertDBNullToInt32(s1["ErrCode"]) == -2) || (CommFunc.ConvertDBNullToInt32(s1["IsPay"]) == 10 && CommFunc.ConvertDBNullToInt32(s1["ErrCode"]) == 0)) ? "用户取消" : CommFunc.ConvertDBNullToString(s1["ErrTxt"]),
                               Create_by = CommFunc.ConvertDBNullToString(s1["Create_by"]),
                               Create_dt = CommFunc.ConvertDBNullToDateTime(s1["Create_dt"]).ToString("yyyy-MM-dd HH:mm:ss"),
                               PayType = CommFunc.ConvertDBNullToInt32(s1["PayType"]),
                               PayTypeS = CommFunc.ConvertDBNullToString(s1["PayTypeS"]),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取充值用电告警错误(YdAlarmOnList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIRst GetYdAlarmOfChargeChkPay(string pTrade_no,string pPayType)
        {
            APIRst rst = new APIRst();
            string status = "";
            string out_trade_no = CommFunc.ConvertDBNullToString(pTrade_no);
            int payType = CommFunc.ConvertDBNullToInt32(pPayType);
            if (string.IsNullOrEmpty(out_trade_no))
            {
                status = "订单号为空";
            }
            else
            {
                try
                {

                    bool pass = false;
                    if (payType == 1)
                        pass = new WxPayAPI.WxPay().GetResult(out_trade_no, out status);
                    else if (payType == 2)
                        pass = new WxPayAPI.WxPay().GetRefundQuery(out_trade_no, out status);
                    else
                        status = "支付类型错误";
                    
                    rst.rst = pass;
                    rst.err.msg = status;
                    rst.data = status;
                    //if (pass == true)
                    //    status = "检测到已经成功支付到微信账号中";
                    //else
                    //    status = "未成功支付到微信账号";
                }
                catch (Exception ex)
                {
                    status = "检测错误:" + ex.Message + " 请重试!";
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = status;
                }
            }
            return rst;
        }
    }
}
