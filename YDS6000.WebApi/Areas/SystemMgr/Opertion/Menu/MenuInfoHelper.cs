using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using YDS6000.BLL.Menu;

namespace YDS6000.WebApi.Areas.SystemMgr.Controllers
{
    public partial class MenuInfoHelper
    {
        private CacheUser user = null;
        private MenuBLL bll = null;
        public MenuInfoHelper()
        {
            user = WebConfig.GetSession();
            bll = new MenuBLL(user.Ledger, user.Uid);
        }

        public APIResult GetMenuList()
        {
            APIResult rst = new APIResult();
            try
            {
                int total = 0;
                List<Treeview> tr = bll.GetMenuList(WebConfig.SysProject, out total);
                object obj = new { total = total, rows = tr };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = obj;
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取区域树形列表错误(GetMenuList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}