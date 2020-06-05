using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.DataProcess
{
    public class ZpRaValBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.DataProcess.ZpRaValDAL dal = null;
        public ZpRaValBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.DataProcess.ZpRaValDAL(_ledger, _uid);
        }

        public bool BaseVal()
        {
            return dal.BaseVal();
        }
    }
}
