using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YDS6000.BLL.Platform.BaseInfo
{
    public partial class BaseInfoBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Platform.BaseInfo.BaseInfoDAL dal = null;
        public BaseInfoBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Platform.BaseInfo.BaseInfoDAL(_ledger, _uid);
        }
    }
}
