using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using YDS6000.BLL.Exp;

namespace YDS6000.WebApi
{
    public class YdToGw
    {
        private static object ydToGwlock = new object();

        private int Ledger = 0;
        private int SysUid = 0;
        YdToGwBLL bll;
        public YdToGw(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            bll = new YdToGwBLL(ledger, uid);
        }
        /// <summary>
        /// // 异步发送命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public bool BeginYdToGwCmd(int module_id, V0Fun funType, string dataValue, int isRn = 0)
        {
            int fun_id = bll.GetYdToFun_id(module_id, funType);
            return this.YdToGwCmd(module_id, fun_id, funType, dataValue, true, isRn);
        }

        /// <summary>
        /// // 异步发送命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public bool BeginYdToGwCmd(int module_id, int fun_id, V0Fun funType, string dataValue, int isRn = 0)
        {
            return this.YdToGwCmd(module_id, fun_id, funType, dataValue, true, isRn);
        }
        /// <summary>
        /// // 异步发送命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        //public bool BeginYdToGwCmd(int module_id, V0Fun funType, string dataValue)
        //{
        //    int fun_id = bll.GetYdToFun_id(module_id, funType);
        //    return this.YdToGwCmd(module_id, fun_id, funType, dataValue, true);
        //}

        /// <summary>
        ///  // 同步发送命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <returns></returns>
        public bool YdToGwCmd(int module_id, int fun_id)
        {
            DataTable dtSource = bll.GetYdToFun_id(module_id, fun_id);
            bool rst = false;
            foreach (DataRow dr in dtSource.Rows)
            {
                rst = this.YdToGwCmd(module_id, fun_id, CommFunc.ConvertDBNullToString(dr["FunType"]), CommFunc.ConvertDBNullToString(dr["DataValue"]), false);
            }
            return rst;
        }

        /// <summary>
        ///  // 同步发送命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public bool YdToGwCmd(int module_id, V0Fun funType, string dataValue)
        {
            int fun_id = bll.GetYdToFun_id(module_id, funType);
            return this.YdToGwCmd(module_id, fun_id, funType, dataValue, false);
        }

        /// <summary>
        /// // 同步发送命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public bool YdToGwCmd(int module_id, int fun_id, V0Fun funType, string dataValue)
        {
            return this.YdToGwCmd(module_id, fun_id, funType, dataValue, false);
        }

        /// <summary>
        /// 下发命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="dataValue"></param>
        /// <param name="upDb"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool YdToGwCmdOfSingle(int module_id, int fun_id, string dataValue,out bool upDb,out string msg)
        {
            long log_id = bll.YdToGwOfAddCmd(module_id, fun_id, dataValue);
            return this.YdToGwAgain(log_id, out upDb, out msg);
        }

        public bool YdToGwConfig(string content)
        {
            ListenVModel vm = new ListenVModel();            
            vm.cfun = ListenCFun.addvar.ToString();
            vm.content = content;
            string msg = "";
            return CacheMgr.SendCollectVal(vm,out msg);
        }

        public bool BeginYdToGwConfig(string content)
        {
            ListenVModel vm = new ListenVModel();
            vm.cfun = ListenCFun.addvar.ToString();
            vm.content = content;
            CacheMgr.BeginSend(vm);
            return true;
        }
        /// <summary>
        /// 重发命令
        /// </summary>
        /// <param name="log_id"></param>
        /// <param name="upDb">是否成功更新数据</param>
        /// <param name="msg">错误信息</param>
        /// <returns>是否成功下发命令</returns>
        public bool YdToGwAgain(long log_id, out bool upDb, out string msg)
        {
            upDb = false;
            msg = "";
            v2_commandVModel rstCmd = bll.GetYdToGwOfCmd(log_id);
            if (rstCmd == null)
            {
                msg = "远程命令已撤销";
                return false;
            }
            if (rstCmd.ErrCode == 1)
            {
                msg = "远程命令已执行";
                return false;
            }

            CommandVModel cmd = bll.GetYdToGwOfSendCmd(log_id);
            bool rst = false;
            try
            {
                rst = this.SendCmd(cmd, false);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                FileLog.WriteLog("发送到设备错误(YdToGwAgain)", ex.Message + ex.StackTrace);
                bll.UpErrYdToGwOfCmd(log_id, msg);
            }

            int timeout = 1000;
            while (rst == true && timeout <= 5000)
            {/*等待是否更新成功*/
                System.Threading.Thread.Sleep(1000);
                rstCmd = bll.GetYdToGwOfCmd(log_id);
                if (rstCmd != null && rstCmd.ErrCode == 1)
                {
                    upDb = true;
                    break;
                }
                timeout = timeout + 1000;
            }
            return rst;
        }

        public CommandVModel GetYdToGwOfSendCmd(long log_id)
        {
            return bll.GetYdToGwOfSendCmd(log_id);
        }

        /// <summary>
        /// 重发命令
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        public bool BeginYdToGwAgain(long log_id)
        {
            CommandVModel cmd = bll.GetYdToGwOfSendCmd(log_id);
            return this.SendCmd(cmd, true);
        }

        private bool YdToGwCmd(int module_id, int fun_id, string funType, string dataValue, bool backgroup, int isRn = 0)
        {
            long log_id = bll.YdToGwOfAddCmd(module_id, fun_id, funType.ToString(), dataValue, isRn);
            CommandVModel cmd = bll.GetYdToGwOfSendCmd(log_id);
            return this.SendCmd(cmd, backgroup);
        }

        private bool YdToGwCmd(int module_id, int fun_id, V0Fun funType, string dataValue, bool backgroup, int isRn = 0)
        {
            long log_id = bll.YdToGwOfAddCmd(module_id, fun_id, funType.ToString(), dataValue, isRn);
            CommandVModel cmd = bll.GetYdToGwOfSendCmd(log_id);
            return this.SendCmd(cmd, backgroup);
        }

        public bool SendCmd(CommandVModel cmd, bool backgroup = true)
        {
            string msg = "";
            if (cmd == null)
                throw new Exception("远程控制为空");
            //目的适应NB表
            if (cmd.TransferType == 4 && !string.IsNullOrEmpty(cmd.DataValue))
            {
                string fs = cmd.FunType + cmd.DataValue;
                string tagName = "", dataValue = "";
                tagName = bll.GetYdToFun_idIOService(cmd.Module_id, fs, out dataValue);
                if (!string.IsNullOrEmpty(tagName) && !string.IsNullOrEmpty(dataValue))
                {
                    cmd.LpszDbVarName = tagName;
                    cmd.DataValue = dataValue;
                }
            }
            //
            cmd.IsUI = true;
            ListenVModel vm = new ListenVModel();
            cmd.IsUI = true;
            vm.cfun = ListenCFun.cmd.ToString();
            vm.content = JsonHelper.Serialize(cmd);
            lock (ydToGwlock)
            {
                if (backgroup == true)
                    CacheMgr.BeginSend(vm);
                else
                    CacheMgr.SendCollectVal(vm, out msg);
            }
            return true;
        }

         
    }
}