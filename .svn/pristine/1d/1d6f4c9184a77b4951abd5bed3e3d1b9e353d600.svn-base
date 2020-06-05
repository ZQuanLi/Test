using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.PayCharg
{
    public class PayChargBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";
        private readonly YDS6000.DAL.Exp.PayCharg.PayChargDAL dal = null;
        public PayChargBLL(string _project, int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            this.Project = _project;
            dal = new YDS6000.DAL.Exp.PayCharg.PayChargDAL(_project, _ledger, _uid);
        }

        public DataTable GetChargList()
        {
            return dal.GetChargList();
        }

        /// <summary>
        /// 新增物业列表
        /// </summary>
        /// <returns></returns>
        public int AddCharg(int co_id, decimal firstVal, decimal lastVal, DateTime firstTime, DateTime lastTime, decimal price, decimal chargAmt)
        {
            return dal.AddCharg(co_id, firstVal, lastVal, firstTime, lastTime, price, chargAmt);
        }
        /// <summary>
        /// 新增物业列表
        /// </summary>
        /// <returns></returns>
        public int UpdateCharg(long log_id, decimal firstVal, decimal lastVal, DateTime firstTime, DateTime lastTime, decimal price, decimal chargAmt)
        {
            return dal.UpdateCharg(log_id, firstVal, lastVal, firstTime, lastTime, price, chargAmt);
        }
        public int DeleteCharg(long log_id)
        {
            return dal.DeleteCharg(log_id);
        }
        public int ToLeadCharg(ref DataTable dtSource)
        {
            return dal.ToLeadCharg(ref dtSource);
        }
    }
}
