using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YDS6000.WebApi.Areas.Platform.Controllers
{
    public partial class BaseInfoHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Platform.BaseInfo.BaseInfoBLL bll = null;

        public BaseInfoHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Platform.BaseInfo.BaseInfoBLL(user.Ledger, user.Uid);
        }
    }
}