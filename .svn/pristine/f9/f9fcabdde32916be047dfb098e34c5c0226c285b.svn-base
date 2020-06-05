using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YDS6000.DAL.Exp.Charge;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Charge
{
    public partial class ChargeBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";
        private ChargeDAL dal = null;
        public ChargeBLL(string project, int ledger, int uid)
        {
            this.Project = project;
            this.Ledger = ledger;
            this.SysUid = uid;
            dal = new ChargeDAL(project, ledger, uid);
        }

        /// <summary>
        /// 获取付费历史记录
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        public bool AddV4_pay_log(v4_pay_logVModel pay)
        {
            return dal.AddV4_pay_log(pay);
        }
        /// <summary>
        /// 获取付费历史记录
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        public v4_pay_logVModel GetV4_pay_log(long log_id)
        {
            return dal.GetV4_pay_log(log_id);
        }
        //判断用户是否具有选择打印小票权限
        public bool GetPower(int Ledger, int Role_id, string Prog_id)
        {
            return dal.GetPower(Ledger, Role_id, Prog_id);
        }
    }
}
