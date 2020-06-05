using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;
using YDS6000.DAL.BaseInfo;

namespace YDS6000.BLL.BaseInfo
{
    public partial class BaseInfoBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly BaseInfoDAL dal = null;
        public BaseInfoBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new BaseInfoDAL(_ledger, _uid);
        }
    }
}
