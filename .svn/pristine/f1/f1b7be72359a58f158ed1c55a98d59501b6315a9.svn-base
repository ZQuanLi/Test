using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Charge
{
    partial class ChargeBLL
    {
        /// <summary>
        /// 获取设备后付费信息
        /// </summary>
        /// <param name="CoStrcName"></param>
        /// <param name="CoName"></param>
        /// <returns></returns>
        public DataTable GetYdPrePayInMdOnList(string CoStrcName, string CoName, int co_id)
        {
            return dal.GetYdPrePayInMdOnList(0, CoStrcName, CoName, co_id);
        }

        /// <summary>
        ///  预付费信息
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Module_id"></param>
        /// <param name="ModuleAddr"></param>
        /// <returns></returns>
        public DataTable GetYdPrepPayInMd_01OnInfo(int Co_id, int Module_id, string ModuleAddr)
        {
            return dal.GetYdPrePayInMdOnV2Info(Co_id, Module_id, ModuleAddr);
        }

        /// <summary>
        /// 回调付款
        /// </summary>
        /// <param name="log_id"></param>
        /// <param name="nAct"></param>
        /// <param name="rMsg"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool YdPrePayInMdOnRecallPay(long log_id, int ErrCode, string rMsg, out string msg, out long cmd_log_id)
        {
            msg = "";
            cmd_log_id = 0;
            if (log_id == 0)
            {
                msg = "ID不能为空";
                return false;
            }
            v4_pay_logVModel payView = dal.GetV4_pay_log(log_id);
            if (payView.Log_id == 0)
            {
                msg = "没有此缴费记录";
                return false;
            }
            if (string.IsNullOrEmpty(msg) && payView.ErrCode != 0)
                msg = "已处理";
            if (string.IsNullOrEmpty(msg) && payView.Co_id == 0)
                msg = "请选择组织";
            if (payView.IsWrong != 1)
            {
                if (string.IsNullOrEmpty(msg) && payView.PayVal == 0)
                    msg = "请输入电量";
                if (string.IsNullOrEmpty(msg) && payView.PayAmt == 0)
                    msg = "请输入金额";
            }
            if (string.IsNullOrEmpty(msg) && payView.PayType != 1 && payView.PayType != 2 && payView.PayType != 3 && payView.PayType != 4)
                msg = "类型错误:" + payView.PayType;
            if ((payView.PayStartTime.Year == 1900 || payView.PayEndTime.Year == 1900 || payView.PayStartTime > payView.PayEndTime))
                msg = "预付费日期错误";
            if (!string.IsNullOrEmpty(msg))
                return false;
            //
            if (ErrCode > 0)
            {
                payView.ErrCode = ErrCode;
                payView.ErrTxt = "";
                if (payView.PayType == 1 || payView.PayType == 2)
                    cmd_log_id = dal.YdPrePayInMdOnChrg(payView);
                else if (payView.PayType == 3 || payView.PayType == 4)
                    cmd_log_id = dal.YdPrePayInMdOnTmp(payView);
            }
            else
            {
                payView.ErrCode = ErrCode;
                payView.ErrTxt = rMsg;
                dal.UpV4_pay_log(payView);
            }
            return true;
        }

        public long GetYdPayListResult(string out_trade_no)
        {
            return dal.GetYdPayListResult(out_trade_no);
        }

        //public bool SendSms(long log_id)
        //{
        //    List<string> listReceipts = new List<string>();
        //    string moduleName = "";
        //    decimal payAmt = 0;
        //    DataTable dtCrm = dal.SendSms(log_id);
        //    foreach (DataRow drCrm in dtCrm.Rows)
        //    {
        //        if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(drCrm["MPhone"])))
        //        {
        //            listReceipts.Add(CommFunc.ConvertDBNullToString(drCrm["MPhone"]));
        //        }
        //        moduleName = CommFunc.ConvertDBNullToString(drCrm["ModuleName"]);
        //        payAmt = CommFunc.ConvertDBNullToDecimal(drCrm["PayAmt"]);
        //    }
        //    if (listReceipts.Count == 0) return false;

        //    object obj = new
        //    {
        //        moduleName = moduleName,
        //        rdAmt = payAmt.ToString("f2") + "元",
        //    };
        //    string content = JsonHelper.Serialize(obj);
        //    string msg;
        //    bool sue = false;
        //    if (!string.IsNullOrEmpty(SmsFun.AliSmsConfig.AliAppKey))
        //        sue = SmsFun.SmsMessage(listReceipts, content, out msg, "SMS_137667152");/*感知万物做演示*/
        //    else
        //        msg = "没有发现短信接口";         
        //    return sue;
        //}

        //*这款用于河北工业大学的SB设备，特殊定制化
        public DataTable GetGwPayData(int module_id)
        {
            return dal.GetGwPayData(module_id);
        }

        /// <summary>
        /// 清零（）
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <returns></returns>
        public int YdPrepPayInMd_Reset(int module_id, int fun_id)
        {
            return dal.YdPrepPayInMd_Reset(module_id, fun_id);
        }

        /// <summary>
        /// 导出充值表
        /// </summary>
        /// <param name="Room_id"></param>
        /// <returns></returns>        
        public DataTable GetYdPayBill(long Log_id)
        {
            DataTable dtSource = dal.GetPayList(Log_id);
            dtSource.Columns.Add("payTypeS", typeof(System.String));//充值类型 payType=1充值=2退电=3赠送电
            dtSource.Columns.Add("strAmt", typeof(System.String));//金额（大写）
            dtSource.Columns.Add("IsPayS", typeof(System.String));//支付方式          
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["Multiply"] = CommFunc.ConvertDBNullToDecimal(dr["Multiply"]).ToString("f2");
                dr["PayVal"] = CommFunc.ConvertDBNullToDecimal(dr["PayVal"]).ToString("f2");
                dr["PayAmt"] = CommFunc.ConvertDBNullToDecimal(dr["PayAmt"]).ToString("f2");
                dr["strAmt"] = CommFunc.convertMoneytoRMB(CommFunc.ConvertDBNullToDecimal(dr["PayAmt"]));
                if (CommFunc.ConvertDBNullToInt32(dr["PayType"]) == 1)
                    dr["payTypeS"] = "充值";
                if (CommFunc.ConvertDBNullToInt32(dr["PayType"]) == 2)
                {
                    dr["payTypeS"] = "退电";
                    dr["PayVal"] = "-" + CommFunc.ConvertDBNullToDecimal(dr["PayVal"]).ToString("f2");
                    dr["PayAmt"] = "-" + CommFunc.ConvertDBNullToDecimal(dr["PayAmt"]).ToString("f2");
                }
                if (CommFunc.ConvertDBNullToInt32(dr["PayType"]) == 3)
                    dr["payTypeS"] = "赠送电";
                dr["Create_dt"] = CommFunc.ConvertDBNullToDateTime(dr["Create_dt"]).ToString("yyyy-MM-dd HH:mm:ss");
                int isPay = CommFunc.ConvertDBNullToInt32(dr["IsPay"]);
                if (isPay == 0)
                    dr["IsPayS"] = "现金支付";
                else if (isPay == 1)
                    dr["IsPayS"] = "PC端微信支付";
                else if (isPay == 10)
                    dr["IsPayS"] = "手机微信支付";
            }
            return dtSource;
        }

        public DataTable GetPayList(long Room_id, string PayAmt, int IsPay)
        {
            DataTable dtSource = dal.GetPayList(Room_id);
            dtSource.Columns.Add("payTypeS", typeof(System.String));//充值类型 payType=1充值=2退电=3赠送电
            dtSource.Columns.Add("strAmt", typeof(System.String));//金额（大写）
            dtSource.Columns.Add("IsPayS", typeof(System.String));//支付方式          
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["Multiply"] = CommFunc.ConvertDBNullToDecimal(dr["Multiply"]).ToString("f2");
                dr["PayVal"] = CommFunc.ConvertDBNullToDecimal(dr["PayVal"]).ToString("f2");
                dr["PayAmt"] = CommFunc.ConvertDBNullToDecimal(dr["PayAmt"]).ToString("f2");
                dr["strAmt"] = CommFunc.convertMoneytoRMB(CommFunc.ConvertDBNullToDecimal(dr["PayAmt"]));
                dr["payTypeS"] = "充值";
                dr["Create_dt"] = CommFunc.ConvertDBNullToDateTime(dr["Create_dt"]).ToString("yyyy-MM-dd HH:mm:ss");
                int isPay = IsPay;
                if (isPay == 0)
                    dr["IsPayS"] = "现金支付";
                else if (isPay == 1)
                    dr["IsPayS"] = "PC端微信支付";
                else if (isPay == 10)
                    dr["IsPayS"] = "手机微信支付";
            }
            return dtSource;
        }


        public bool NotifyForPay(long log_id, out string msg)
        {
            msg = "";
            if (log_id == 0)
            {
                msg = "ID不能为空";
                return false;
            }
            v4_pay_logVModel payView = dal.GetV4_pay_log(log_id);
            if (payView.Log_id == 0)
            {
                msg = "没有此缴费记录";
                return false;
            }
            if (string.IsNullOrEmpty(msg) && payView.ErrCode != 0)
                msg = "已处理";
            if (string.IsNullOrEmpty(msg) && payView.Co_id == 0)
                msg = "请选择组织";
            if (payView.IsWrong != 1)
            {
                if (string.IsNullOrEmpty(msg) && payView.PayVal == 0)
                    msg = "请输入电量";
                if (string.IsNullOrEmpty(msg) && payView.PayAmt == 0)
                    msg = "请输入金额";
            }
            if (string.IsNullOrEmpty(msg) && payView.PayType != 1 && payView.PayType != 2 && payView.PayType != 3 && payView.PayType != 4)
                msg = "类型错误:" + payView.PayType;
            if ((payView.PayStartTime.Year == 1900 || payView.PayEndTime.Year == 1900 || payView.PayStartTime > payView.PayEndTime))
                msg = "预付费日期错误";
            if (!string.IsNullOrEmpty(msg))
                return false;
            //
            payView.ErrCode = 1;
            payView.ErrTxt = "";
            dal.UpV4_pay_log(payView);
            return true;
        }
    }
}
