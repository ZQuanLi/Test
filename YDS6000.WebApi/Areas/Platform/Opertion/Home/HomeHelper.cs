using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Platform.Controllers
{
    public partial class HomeHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Platform.Home.HomeBLL bll = null;

        public HomeHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Platform.Home.HomeBLL(user.Ledger, user.Uid);
        }

        public APIRst Login(int ledger,string uSign, string pwd)
        {
            bll = new YDS6000.BLL.Platform.Home.HomeBLL(ledger, user.Uid);
            APIRst rst = new APIRst();
            if (string.IsNullOrEmpty(uSign))
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "用户名不能为空";
                return rst;
            }
            if (string.IsNullOrEmpty(pwd))
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "密码不能为空";
                return rst;

            }

            try
            {
                DataTable dtSource = bll.GetSys_user(uSign);
                int nRows = dtSource.Rows.Count;
                if (nRows == 0)
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "没有此用户";
                    return rst;
                }
                else if (nRows != 1)
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "此用户有多个";
                    return rst;
                }

                int uid = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Uid"]);
                string uName = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["UName"]);
                string dbPwd = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["UPasswd"]);
                string project = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["Project"]);
                int role_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Role_id"]);
                if (!pwd.Trim().Equals(dbPwd))
                {
                    //查询最后最后的登录时间，判断如果少于十五分钟且错误次数已经为五次，提示十五分钟后再次登录
                    int num = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["LoginInt"]);
                    DateTime dt = CommFunc.ConvertDBNullToDateTime(dtSource.Rows[0]["LoginDate"]);
                    TimeSpan ts = new TimeSpan();
                    ts = DateTime.Now - dt; //现在时间-数据库时间
                    int Result = Convert.ToInt32(ts.TotalMinutes); //转换时间间隔为 分钟  Double型转化成Int型
                    if (Result < 15 && num > 4)
                    {
                        rst.err.code = (int)ResultCodeDefine.Error;
                        rst.err.msg = "登录的次数超过了规定次数，请十五分钟后再试";
                        return rst;
                    }
                    //登录密码错误，将登录错误次数+1
                    bll.UpdateLoginErr(uSign);
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "密码错误";
                    return rst;
                }
                //////
                //WebConfig.Ledger = ledger;
                WebConfig.SysProject = project;
                //////
                int seed = new Random(Guid.NewGuid().GetHashCode()).Next(65, 91);
                string zm = CommFunc.NunberToChar(seed);
                zm = string.IsNullOrEmpty(zm) ? "A" : zm;
                string ticket = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10) + zm + (ledger + seed).ToString() + zm + (seed + uid).ToString();
                string ccKey = ledger.ToString() + "A" + uid.ToString();
                CacheUser cacheUser = new CacheUser();
                cacheUser.Ledger = ledger;
                cacheUser.Uid = uid;
                cacheUser.USign = uSign;
                cacheUser.Role_id = role_id;
                cacheUser.CacheKey = project + "->" + ledger + "->";
                //随机数前10位+ 种子数+ (种子数+用户数之和)
                cacheUser.Ticket = ticket;// Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10) + (seed + cacheUser.Uid).ToString().PadLeft(7, '0');
                //
                bll.UpdateLoginSue(uSign);
                rst.err.code = 0;
                rst.data = new { Ticket = cacheUser.Ticket ,Name = uName };
                //
                int timeSpan = CommFunc.ConvertDBNullToInt32(ConfigHelper.GetAppSettings("Cached:Time"));
                timeSpan = timeSpan == 0 ? 15 : timeSpan;
                HttpRuntime.Cache.Insert(ccKey, cacheUser, null, DateTime.MaxValue, TimeSpan.FromMinutes(timeSpan));
                HttpContext.Current.Session["CacheUser"] = cacheUser;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("登录错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        public APIRst GetSubSystem()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetSubSystem();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               ProId = CommFunc.ConvertDBNullToInt32(s1["ProId"]),
                               Project = CommFunc.ConvertDBNullToString(s1["Project"]),
                               ProName = CommFunc.ConvertDBNullToString(s1["ProName"]),
                               Parent_id = CommFunc.ConvertDBNullToInt32(s1["Parent_id"]),
                               Path = CommFunc.ConvertDBNullToString(s1["Path"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                rst.data = null;
            }
            return rst;
        }

        public APIRst GetMenuList()
        {
            APIRst rst = new APIRst();
            try
            {
                int total = 0;
                List<Treeview> tr = bll.GetMenuList(WebConfig.SysProject, out total);
                rst.data = new { total = total, rows = tr };
            }
            catch (Exception ex)
            {
                rst.err.code = -1;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取区域树形列表错误(GetMenuList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

    }
}