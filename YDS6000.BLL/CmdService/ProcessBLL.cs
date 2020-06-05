using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.CmdService
{
    public class ProcessBLL
    {

        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.CmdService.ProcessDAL dal = null;
        public ProcessBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.CmdService.ProcessDAL(_ledger, _uid);
        }

        /// <summary>
        /// 获取设备测试ID号
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="funType"></param>
        /// <returns></returns>
        public int GetYdToFun_id(int module_id, V0Fun funType)
        {
            return dal.GetYdToFun_id(module_id, funType);
        }

        /// <summary>
        /// 增加命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public long YdToGwOfAddCmd(int module_id, int fun_id, string funType, string dataValue)
        {
            return dal.YdToGwOfAddCmd(module_id, fun_id, funType, dataValue);
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
    }
}
