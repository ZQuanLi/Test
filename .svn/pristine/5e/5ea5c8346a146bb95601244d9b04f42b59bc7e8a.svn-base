using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YDS6000.WebApi.Areas.Energy.Controllers
{
    public partial class MonitorHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Energy.Monitor.MonitorBLL bll = null;
        public MonitorHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Energy.Monitor.MonitorBLL(user.Ledger, user.Uid);
        }
    }
}