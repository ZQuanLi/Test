using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    public partial class MonitorHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Monitor.MonitorBLL bll = null;
        public MonitorHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Monitor.MonitorBLL(WebConfig.SysProject, user.Ledger, user.Uid);
        }

    }
}