using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;
using YDS6000.DAL.DataProcess;

namespace YDS6000.BLL.DataProcess
{
    public class PayEstBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly PayEstDAL dal = null;
        public PayEstBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new PayEstDAL(_ledger, _uid);
        }

        public DataTable GetRateInfo()
        {
            return dal.GetRateInfo();
        }

        public DataTable GetPayEstInfo()
        {
            return dal.GetPayEstInfo();
        }

        public int UpdatePayEst(DataTable dtSource)
        {
            return dal.UpdatePayEst(dtSource);
        }

        public DataTable GetV4_pay_charg()
        {
            return dal.GetV4_pay_charg();
        }


        public int UpV4_pay_charg(long log_id, int co_id, decimal useAmt)
        {
            return dal.UpV4_pay_charg(log_id, co_id, useAmt);
        }

        //public DataTable GetCoEleAmt(int co_id)
        //{
        //    return dal.GetCoEleAmt(co_id);
        //}
        //public decimal GetCoRdAmt(int co_id)
        //{
        //    return dal.GetCoRdAmt(co_id);
        //}
    }
}
