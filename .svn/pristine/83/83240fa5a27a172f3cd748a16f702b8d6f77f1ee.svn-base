using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi
{
    public class ResultNotify
    {
        private int ledger = 0, uid = 1;
        private YdToGw gwBll = null;

        public ResultNotify()
        {                  
        }

        public void Pay(string out_trade_no)
        {          
            DataTable dtSource = new YDS6000.BLL.IFSMgr.Monitor.MonitorBLL(this.ledger, this.uid).GetYdPayListResult(out_trade_no);
            int cnt = dtSource.Rows.Count;
            if (cnt == 0)
            {
                FileLog.WriteLog("微信支付回调数据库信息未找到,商户单号:", out_trade_no);
                return;
            }
            int ledger = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Ledger"]);
            long log_id = CommFunc.ConvertDBNullToLong(dtSource.Rows[0]["Log_id"]);
            int module_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Module_id"]);
            int uid = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Create_by"]);
            int co_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Co_id"]);
            this.ledger = ledger;
            this.uid = uid;
            gwBll = new YdToGw(ledger, uid);
            if (log_id == 0)
            {
                FileLog.WriteLog("微信支付回调数据库ID号未找到,商户单号:", out_trade_no);
            }
            else
            {
                if (module_id !=0)
                    Notify(log_id, module_id);
                if (co_id != 0)
                    NotifyForPay(log_id, co_id);

                //FileLog.WriteLog("微信支付回调数商户单号:", out_trade_no + "数据库ID号:" + log_id);
            }

        }

        public void Refund(long log_id)
        {
            Notify(log_id, 0);
        }

        private void Notify(long log_id,int module_id)
        {
            string msg = "";
            long cmd_log_id = 0;
            YDS6000.BLL.Exp.Charge.ChargeBLL chrage = new YDS6000.BLL.Exp.Charge.ChargeBLL("",this.ledger,this.uid);
            chrage.YdPrePayInMdOnRecallPay(log_id, 1, "", out msg, out cmd_log_id);
            if (cmd_log_id != 0)
            {
                try
                {
                    CommandVModel cmd = gwBll.GetYdToGwOfSendCmd(cmd_log_id);
                    cmd.IsUI = true;
                    gwBll.SendCmd(cmd, true);/*发送充值命令*/
                    if (cmd.FunType.Equals(V0Fun.Pay.ToString()))
                        gwBll.BeginYdToGwCmd(cmd.Module_id, V0Fun.Ssr, "0"); /*发送合闸命令*/
                    else if (cmd.FunType.Equals(V0Fun.Refund.ToString()))
                        gwBll.BeginYdToGwCmd(cmd.Module_id, V0Fun.Ssr, "1"); /*发送拉闸命令*/
                    //
                    gwBll.BeginYdToGwCmd(cmd.Module_id, V0Fun.RdVal, ""); /*发送读取剩余电量*/
                }
                catch (Exception ex)
                {
                    FileLog.WriteLog("ResultNotify : 发送远程命令错误:", ex.Message + ex.StackTrace);
                }
            }
            else
            {
                DataTable dtSource = chrage.GetYdPostPayInMdOnList(module_id);
                bool isClosed = false;
                decimal odValue = 0, syVal = 0;
                if (dtSource.Rows.Count > 0)
                {
                    isClosed = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["IsClosed"]) == 1 ? true : false;
                    odValue = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["OdValue"]);
                    syVal = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["RdVal"]);
                }
                FileLog.WriteLog("isClosed:" + isClosed + " syVal:" + syVal + " odValue:" + odValue);
                if (isClosed == true)
                {
                    if (syVal <= odValue)/*拉闸*/
                        gwBll.BeginYdToGwCmd(module_id, V0Fun.Ssr, "1", 1); /*发送拉闸命令*/
                    else /*合闸*/
                        gwBll.BeginYdToGwCmd(module_id, V0Fun.Ssr, "0", 2); /*发送合闸命令*/
                }
            }
        }

        private void NotifyForPay(long log_id, int co_id)
        {
            YDS6000.BLL.Exp.Charge.ChargeBLL chrage = new YDS6000.BLL.Exp.Charge.ChargeBLL("", this.ledger, this.uid);
            string msg = "";
            if (chrage.NotifyForPay(log_id, out msg) == false)
            {
                FileLog.WriteLog("NotifyForPay错误:", msg);
                return;
            }
            DataTable dtSource = chrage.GetNotifyForPayList(co_id);
            foreach (DataRow dr in dtSource.Rows)
            {
                bool isClosed = CommFunc.ConvertDBNullToInt32(dr["IsClosed"]) == 1 ? true : false;
                decimal odValue = CommFunc.ConvertDBNullToDecimal(dr["OdValue"]);
                decimal syVal = CommFunc.ConvertDBNullToDecimal(dr["RdVal"]);
                int module_id = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);

                FileLog.WriteLog("isClosed:" + isClosed + " syVal:" + syVal + " odValue:" + odValue);
                if (isClosed == true)
                {
                    if (syVal <= odValue)/*拉闸*/
                    {
                        //gwBll.BeginYdToGwCmd(module_id, V0Fun.Ssr, "1", 1); /*发送拉闸命令*/
                    }
                    else /*合闸*/
                    {
                        gwBll.BeginYdToGwCmd(module_id, V0Fun.Ssr, "0", 2); /*发送合闸命令*/
                    }
                }
            }
        }
    }
}