using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using YDS6000.BLL.BaseInfo;

namespace YDS6000.WebApi.Areas.SystemMgr.Controllers
{
    public partial class BaseInfoHelper
    {
        private CacheUser user = null;
        private BaseInfoBLL bll = null;
        public BaseInfoHelper()
        {
            user = WebConfig.GetSession();
            bll = new BaseInfoBLL(user.Ledger, user.Uid);
        }
    }
}