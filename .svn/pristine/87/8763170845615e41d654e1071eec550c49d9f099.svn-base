using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YDS6000.WebApi.Areas.Energy.Opertion.Report
{
    public partial class ReportHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Energy.Report.ReportBLL bll = null;
        public ReportHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Energy.Report.ReportBLL(user.Ledger, user.Uid);
        }
    }
}