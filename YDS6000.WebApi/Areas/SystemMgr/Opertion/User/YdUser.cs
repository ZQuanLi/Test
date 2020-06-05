using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Http;
using System.Reflection;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.SystemMgr.Controllers
{
    partial class UserHelper
    {
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public APIResult GetUserList(int uid)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetUserList(uid);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Uid = CommFunc.ConvertDBNullToInt32(s1["Uid"]),
                               USign = CommFunc.ConvertDBNullToString(s1["USign"]),
                               UPasswd = CommFunc.ConvertDBNullToString(s1["UPasswd"]),
                               UName = CommFunc.ConvertDBNullToString(s1["UName"]),
                               Role_id = CommFunc.ConvertDBNullToInt32(s1["Role_id"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               RoleName = CommFunc.ConvertDBNullToString(s1["Descr"]),
                               CreateName = CommFunc.ConvertDBNullToString(s1["CreateName"]),
                               CreateTime = CommFunc.ConvertDBNullToDateTime(s1["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { total = dtSource.Rows.Count, Rows = res1.ToList() };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取角色列表(GetRoleList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIResult SetUser(sys_user user)
        {
            APIResult rst = new APIResult();
            try
            {
                bll.SetUser(user);
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = "成功";
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("设置角色列表(SetRole):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="uid">用户ID号</param>
        /// <returns></returns>
        public APIResult DelUser(int uid)
        {
            APIResult rst = new APIResult();
            try
            {
                rst.Data = bll.DelUser(uid);
                rst.Code = 0;
                rst.Msg = "";
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("删除用户信息错误(DelUser):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}