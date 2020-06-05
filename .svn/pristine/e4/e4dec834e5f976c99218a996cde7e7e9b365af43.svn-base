using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YDS6000.Models;

namespace DataProcess.YdProcess
{
    internal class YdDbMid
    {
        YDS6000.BLL.DataProcess.YdDbMidBLL bll = new YDS6000.BLL.DataProcess.YdDbMidBLL(0, 1);
        public void Run()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                try
                {
                    if (YdDbMidRun() == false) continue;
                }
                catch (Exception ex)
                {
                    FileLog.WriteLog("中间库运行错误:" + ex.Message + ex.StackTrace);
                }
                System.Threading.Thread.Sleep(1000 * 10);
            }
        }


        private bool YdDbMidRun()
        {
            DataTable dtSource = bll.GetDk_buy();
            if (dtSource == null) return false;

            foreach (DataRow dr in dtSource.Rows)
            {
                long recno = CommFunc.ConvertDBNullToLong(dr["recno"]);
                int co_id = CommFunc.ConvertDBNullToInt32(dr["FJID"]);
                decimal payAmt = CommFunc.ConvertDBNullToDecimal(dr["tranamt"]) / 100;
                DateTime endatatime = CommFunc.ConvertDBNullToDateTime(dr["endatatime"]);

                if (co_id == 0)
                {
                    FileLog.WriteLog("中间库房间ID号为空");
                    continue;
                }
                if (payAmt == 0)
                {
                    FileLog.WriteLog("中间库充值金额为0");
                    continue;
                }
                DataTable dtPay = bll.GetPayInfo(co_id);
                if (dtPay.Rows.Count == 0)
                {
                    FileLog.WriteLog("中间库的房间ID：" + co_id + "没有表信息存在");
                    continue;
                }
                if (dtPay.Rows.Count > 1)
                {
                    FileLog.WriteLog("中间库的房间ID：" + co_id + "存在多个表");
                    continue;
                }
                decimal price = CommFunc.ConvertDBNullToDecimal(dtPay.Rows[0]["Price"]);
                if (price == 0)
                {
                    FileLog.WriteLog("中间库的房间ID：" + co_id + "单价不能为零");
                    continue;
                }
                v4_pay_logVModel pay = new v4_pay_logVModel();
                pay.Co_id = CommFunc.ConvertDBNullToInt32(dtPay.Rows[0]["Co_id"]);
                pay.Module_id = CommFunc.ConvertDBNullToInt32(dtPay.Rows[0]["Module_id"]);
                pay.ModuleAddr = CommFunc.ConvertDBNullToString(dtPay.Rows[0]["ModuleAddr"]); ; // CommFunc.ConvertDBNullToString(Request["ModuleAddr"]);
                pay.Fun_id = CommFunc.ConvertDBNullToInt32(dtPay.Rows[0]["Fun_id"]);
                pay.PayAmt = payAmt;
                pay.PayVal = Math.Round(pay.PayAmt / price, 2, MidpointRounding.AwayFromZero);
                pay.PayType = 1;
                pay.IsWrong = 0;
                pay.IsPay = 30;
                pay.PayStartTime = DateTime.Now;
                pay.PayEndTime = DateTime.Now;
                pay.Price = price;
                bll.YdDbMidAddPayLog(pay, endatatime, recno);
                string msg = "";
                long cmd_log_id = 0;
                bll.PayVal(pay.Log_id, recno, 1, "", out msg, out cmd_log_id);
                if (cmd_log_id != 0)//立刻发送命令到网关
                {
                    CommandVModel cmd = bll.GetYdToGwOfSendCmd(cmd_log_id);
                    DataProcess.YdDrive.Collection.CollectionHelper.Instance(cmd);
                }
                DataTable dtMM = bll.GetYdPostPayInMdOnList(pay.Module_id);
                decimal chrgVal = 0, useVal = 0, rdVal = 0, syZsVal = 0;
                if (dtMM.Rows.Count > 0)
                {
                    chrgVal = CommFunc.ConvertDBNullToDecimal(dtMM.Rows[0]["ChrgVal"]);
                    useVal = CommFunc.ConvertDBNullToDecimal(dtMM.Rows[0]["UseVal"]);
                    rdVal = CommFunc.ConvertDBNullToDecimal(dtMM.Rows[0]["RdVal"]);
                    syZsVal = CommFunc.ConvertDBNullToDecimal(dtMM.Rows[0]["SyZsVal"]);
                }

                //判断是否拉闸
                YDS6000.BLL.Exp.Syscont.ParameterBLL sysBll = new YDS6000.BLL.Exp.Syscont.ParameterBLL(0, 1);
                var dt = sysBll.GetAlarmCfg();
                if (dt != null && dt.Rows.Count > 0)
                {
                    decimal odValue = CommFunc.ConvertDBNullToDecimal(dt.Rows[0]["CfValue"]);
                    int isClosed = CommFunc.ConvertDBNullToInt32(dt.Rows[0]["Rule"]);
                    if (isClosed == 1)
                    {
                        if (rdVal <= odValue)
                        {
                            this.YdBatchControlOfSsr(pay.Module_id, "Ssr", "1", 30);
                        }
                        else
                        {
                            this.YdBatchControlOfSsr(pay.Module_id, "Ssr", "0", 30);
                        }
                    }
                }
            }
            return true;
        }
        // 批量下发命令
        private bool YdBatchControlOfSsr(int module_id, string funType, string dataValue, int isRn)
        {
            int fun_id = bll.GetYdToFun_id(module_id, funType);
            long log_id = bll.YdToGwOfAddCmd(module_id, fun_id, funType.ToString(), dataValue, isRn);
            CommandVModel cmd = bll.GetYdToGwOfSendCmd(log_id);
            DataProcess.YdDrive.Collection.CollectionHelper.Instance(cmd);
            return true;
        }
    }
}
