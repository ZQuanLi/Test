using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    public partial class AlarmHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Alarm.AlarmBLL bll = null;
        public AlarmHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Alarm.AlarmBLL(WebConfig.SysProject, user.Ledger, user.Uid);
        }

    }
}