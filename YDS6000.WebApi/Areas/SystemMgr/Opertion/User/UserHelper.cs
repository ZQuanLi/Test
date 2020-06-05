using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using YDS6000.BLL.User;

namespace YDS6000.WebApi.Areas.SystemMgr.Controllers
{
    /// <summary>
    /// 用户操作类
    /// </summary>
    public partial class UserHelper
    {
        private CacheUser user = null;
        private UserBLL bll = null;
        public UserHelper()
        {
            user = WebConfig.GetSession();
            bll = new UserBLL(user.Ledger, user.Uid);
        }

        /// <summary>
        /// 登录用户
        /// </summary>
        /// <param name="usign">登录名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public APIResult Login(string usign, string pwd)
        {
            APIResult rst = new APIResult();
            try
            {
                rst.Code = -1;
                rst.Msg = "";
                DataTable dtSource = bll.GetSysUser(usign);
                int nRows = dtSource.Rows.Count;

                if (nRows == 0)
                {
                    rst.Msg = "没有此用户";
                }
                else if (nRows != 1)
                {
                    rst.Msg = "此用户有多个";
                }
                else
                {
                    int Uid = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Uid"]);
                    string UName = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["UName"]);
                    string dbPwd = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["UPasswd"]);
                    int Role_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Role_id"]);
                    //登录密码错误
                    if (!pwd.Trim().Equals(dbPwd))
                    {
                        rst.Msg = "密码错误";
                    }
                    else
                    {
                        user = new CacheUser();
                        user.Ledger = WebConfig.Ledger;
                        user.Uid = Uid;
                        user.USign = usign;
                        user.Role_id = Role_id;
                        //HttpRuntime.Cache.Insert(WebConfig.Ledger + "->" + Uid, user, null, token.ExpireTime, TimeSpan.Zero);
                        HttpContext.Current.Session["CacheUser"] = user;
                        rst.Code = 0;
                        rst.Msg = "";
                    }
                }
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("登陆API数据错误(Login):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

    }
}