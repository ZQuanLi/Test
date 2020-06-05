using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using YDS6000.BLL.Report;

namespace YDS6000.WebApi.Areas.SystemMgr.Controllers
{
    /// <summary>
    /// 报表定制帮助类
    /// </summary>
    public partial class ReportHelper
    {
        private CacheUser user = null;
        private ReportBLL bll = null;
        /// <summary>
        /// 对象
        /// </summary>
        public ReportHelper()
        {
            user = WebConfig.GetSession();
            bll = new ReportBLL(user.Ledger, user.Uid);
        }

        /// <summary>
        /// 获取对象选择树形列表
        /// </summary>
        /// <returns></returns>
        public APIResult GetSelectObject()
        {
            APIResult rst = new APIResult();
            try
            {
                int total = 0;
                List<Treeview> list = bll.GetSelectObject(out total);
                object obj = new { total = total, rows = list };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = obj;
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取对象选择列表错误(GetSelectObject):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}