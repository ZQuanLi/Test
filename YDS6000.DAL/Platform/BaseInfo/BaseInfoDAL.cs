using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YDS6000.DAL.Platform.BaseInfo
{
    public partial class BaseInfoDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public BaseInfoDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }
    }
}
