using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YDS6000.Models;
using YDS6000.DAL;

namespace YDS6000.BLL.Exp
{
    public class YdToGwBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private DAL.Exp.YdToGwDAL dal = null;
        public YdToGwBLL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            dal = new DAL.Exp.YdToGwDAL(ledger, uid);
        }

        public string GetYdToFun_idIOService(int module_id, string funType,out string dataValue)
        {
            return dal.GetYdToFun_idIOService(module_id, funType,out dataValue);
        }

        public int GetYdToFun_id(int module_id, V0Fun funType)
        {
            return dal.GetYdToFun_id(module_id, funType);
        }

        public DataTable GetYdToFun_id(int module_id, int fun_id)
        {
            return dal.GetYdToFun_id(module_id, fun_id);
        }
        /// <summary>
        /// 下发命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public long YdToGwOfAddCmd(int module_id, int fun_id, string funType, string dataValue,int isRn)
        {
            return dal.YdToGwOfAddCmd(module_id, fun_id, funType, dataValue, isRn);
        }

        /// <summary>
        /// 下发命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public long YdToGwOfAddCmd(int module_id, int fun_id, string dataValue)
        {
            return dal.YdToGwOfAddCmd(module_id, fun_id, dataValue);
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
        /// 获取远程命令
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        public v2_commandVModel GetYdToGwOfCmd(long log_id)
        {
            return dal.GetYdToGwOfCmd(log_id);
        }

                /// <summary>
        /// 更新远程命令
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        public int UpErrYdToGwOfCmd(long log_id, string ErrTxt)
        {
            return dal.UpErrYdToGwOfCmd(log_id, ErrTxt);
        }
    }
}
