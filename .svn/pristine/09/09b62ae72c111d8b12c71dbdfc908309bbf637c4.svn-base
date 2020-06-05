using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YDS6000.WebApi.Areas.Platform.Controllers
{
    public partial class UserHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Platform.User.UserBLL bll = null;

        public UserHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Platform.User.UserBLL(user.Ledger, user.Uid);
        }



    }
}