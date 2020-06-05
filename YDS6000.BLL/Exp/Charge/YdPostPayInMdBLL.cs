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
        public DataTable GetYdPostPayInMdOnList(string CoStrcName, string CoName)
        {
            return dal.GetYdPostPayInMdOnList(0, CoStrcName, CoName);
        }
        /// <summary>
        /// 获取设备后付费信息
        /// </summary>
        /// <param name="CoStrcName"></param>
        /// <param name="CoName"></param>
        /// <returns></returns>
        public DataTable GetYdPostPayInMdOnList(int Module_id)
        {
            return dal.GetYdPostPayInMdOnList(Module_id, "", "");
        }
        public DataTable GetNotifyForPayList(int co_id)
        {
            return dal.GetNotifyForPayList(co_id);
        }
        ///// <summary>
        ///// 获取逐日用电量及日缴费信息
        ///// </summary>
        ///// <param name="Co_id"></param>
        ///// <param name="Module_id"></param>
        ///// <param name="ModuleAddr"></param>
        ///// <param name="Start"></param>
        ///// <param name="End"></param>
        ///// <returns></returns>
        //public DataTable GetYdPostPayInMd_01OnInfo(int Co_id, int Module_id, string ModuleAddr, out DateTime Start, out DateTime End)
        //{
        //    Start = DateTime.Now; End = DateTime.Now;
        //    //////////////////////////////////////////////////////
        //    DataTable dtV2Info = dal.GetYdPostPayInMdOnV2Info(Co_id, Module_id, ModuleAddr);
        //    if (dtV2Info.Rows.Count > 0)
        //    {
        //        DateTime debtTime = CommFunc.ConvertDBNullToDateTime(dtV2Info.Rows[0]["DebtTime"]);
        //        if (debtTime.Year != 1900 && debtTime.Year != 0)
        //            Start = debtTime;
        //    }
        //    return dal.GetYdPostPayInMdOnV9Day(Co_id, Module_id, ModuleAddr, Start, End);
        //}
        ///// <summary>
        ///// 获取逐日用电量及日缴费信息
        ///// </summary>
        ///// <param name="Co_id"></param>
        ///// <param name="Module_id"></param>
        ///// <param name="ModuleAddr"></param>
        ///// <param name="Start"></param>
        ///// <param name="End"></param>
        ///// <param name="UseVal"></param>
        ///// <param name="ChrgVal"></param>
        ///// <returns></returns>
        //public DataTable GetYdPostPayInMd_01OnInfo(int Co_id, int Module_id, string ModuleAddr, DateTime Start, DateTime End)
        //{
        //    return dal.GetYdPostPayInMdOnV9Day(Co_id, Module_id, ModuleAddr, Start, End);
        //}
        ///// <summary>
        ///// 获取付费历史记录
        ///// </summary>
        ///// <param name="log_id"></param>
        ///// <returns></returns>
        ////public v4_pay_logVModel YdPostPayInMdOnV4_pay(long log_id)
        ////{
        ////    return dal.YdPostPayInMdOnV4_pay(log_id);
        ////}

        ///// <summary>
        ///// 增加记录log
        ///// </summary>
        ///// <param name="pay"></param>
        ///// <returns></returns>
        ////public long YdPostPayInMdOnAddPayLog(v4_pay_logVModel pay)
        ////{
        ////    return dal.YdPostPayInMdOnAddPayLog(pay);
        ////}

        ///// <summary>
        ///// 回调付款
        ///// </summary>
        ///// <param name="log_id"></param>
        ///// <param name="nAct"></param>
        ///// <param name="rMsg"></param>
        ///// <param name="msg"></param>
        ///// <returns></returns>
        //public bool YdPostPayInMdOnRecallPay(long log_id, int ErrCode, string rMsg, out string msg)
        //{
        //    msg = "";
        //    if (log_id == 0)
        //    {
        //        msg = "ID不能为空";
        //        return false;
        //    }
        //    v4_pay_logVModel payView = dal.GetV4_pay_log(log_id);
        //    if (payView.Log_id == 0)
        //    {
        //        msg = "没有此充值记录";
        //        return false;
        //    }
        //    if (string.IsNullOrEmpty(msg) && payView.ErrCode != 0)
        //        msg = "已处理";
        //    if (string.IsNullOrEmpty(msg) && payView.Co_id == 0)
        //        msg = "请选择组织";
        //    if (payView.IsWrong != 1)
        //    {
        //        if (string.IsNullOrEmpty(msg) && payView.PayVal == 0)
        //            msg = "请输入电量";
        //        if (string.IsNullOrEmpty(msg) && payView.PayAmt == 0)
        //            msg = "请输入金额";
        //    }
        //    if (string.IsNullOrEmpty(msg) && (payView.PayType != 1))
        //        msg = "类型错误:" + payView.PayType;
        //    if ((payView.PayStartTime.Year == 1900 || payView.PayEndTime.Year == 1900 || payView.PayStartTime > payView.PayEndTime))
        //        msg = "付费日期错误";
        //    if (!string.IsNullOrEmpty(msg))
        //        return false;
        //    //
        //    if (ErrCode > 0)
        //    {
        //        payView.ErrCode = ErrCode;
        //        payView.ErrTxt = "";
        //        dal.YdPostPayInMdOnChrg(payView);
        //    }
        //    else
        //    {
        //        payView.ErrCode = ErrCode;
        //        payView.ErrTxt = rMsg;
        //        dal.UpV4_pay_log(payView);
        //    }
        //    return true;
        //}



    }
}
