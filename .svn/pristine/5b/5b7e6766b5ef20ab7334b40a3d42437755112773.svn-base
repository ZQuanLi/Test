using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.DataProcess
{
    public class YdDbMidBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.DataProcess.YdDbMidDAL dal = null;
        public YdDbMidBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.DataProcess.YdDbMidDAL(_ledger, _uid);
        }

        public DataTable GetDk_buy()
        {
            return dal.GetDk_buy();
        }
        public DataTable GetPayInfo(int co_id)
        {
            return dal.GetPayInfo(co_id);
        }
        /// <summary>
        /// 增加记录log
        /// </summary>
        /// <param name="pay"></param>
        /// <returns></returns>
        public bool YdDbMidAddPayLog(v4_pay_logVModel pay,DateTime endatatime,long recno)
        {
            return dal.YdDbMidAddPayLog(pay, endatatime, recno);
        }

        public bool PayVal(long log_id,long recno, int errCode, string errTxt, out string msg, out long cmd_log_id)
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
                msg = "请选择房间";
            if (string.IsNullOrEmpty(msg) && payView.PayVal == 0)
                msg = "请输入电量";
            if (string.IsNullOrEmpty(msg) && payView.PayAmt == 0)
                msg = "请输入金额";
            if (string.IsNullOrEmpty(msg) && payView.PayType != 1 && payView.PayType != 2 && payView.PayType != 3 && payView.PayType != 4 && payView.PayType != 5)
                msg = "类型错误:" + payView.PayType;

            if (!string.IsNullOrEmpty(msg))
                return false;
            //
            if (errCode > 0)
            {
                payView.ErrCode = errCode;
                payView.ErrTxt = "";
                if (payView.PayType == 1 || payView.PayType == 2 || payView.PayType == 5)
                    cmd_log_id = dal.YdPrePayInMdOnChrg(payView, recno);
                else if (payView.PayType == 3 || payView.PayType == 4)
                    throw new Exception("类型错误");
            }
            else
            {
                payView.ErrCode = errCode;
                payView.ErrTxt = errTxt;
                dal.UpV4_pay_log(payView);
            }
            return true;
        }

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        public CommandVModel GetYdToGwOfSendCmd(long log_id)
        {
            return dal.GetYdToGwOfSendCmd(log_id);
        }

        /// <summary>
        /// 获取设备后付费信息
        /// </summary>
        /// <param name="CoStrcName"></param>
        /// <param name="CoName"></param>
        /// <returns></returns>
        public DataTable GetYdPostPayInMdOnList(int Module_id)
        {
            return dal.GetYdPostPayInMdOnList(Module_id);
        }

        public int GetYdToFun_id(int module_id, string funType)
        {
            return dal.GetYdToFun_id(module_id, funType);
        }
        /// <summary>
        /// 下发命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public long YdToGwOfAddCmd(int module_id, int fun_id, string funType, string dataValue, int isRn)
        {
            return dal.YdToGwOfAddCmd(module_id, fun_id, funType, dataValue, isRn);
        }
    }
}
