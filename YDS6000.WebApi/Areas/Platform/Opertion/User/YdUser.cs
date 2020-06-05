using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Platform.Controllers
{
    partial class UserHelper
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetUserList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetUserList();
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
                               TelNo = CommFunc.ConvertDBNullToString(s1["TelNo"]),
                               CreateName = CommFunc.ConvertDBNullToString(s1["CreateName"]),
                               CreateTime = CommFunc.ConvertDBNullToDateTime(s1["CreateTime"]).ToString("yyyy-MM-dd"),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取用户列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 设置用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public APIRst SetUser(sys_user user)
        {
            APIRst rst = new APIRst();
            try
            {
                bll.SetUser(user);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置用户列表(SetUser):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="uid">用户ID号</param>
        /// <returns></returns>
        public APIRst DelUser(int uid)
        {
            APIRst rst = new APIRst();
            try
            {
                bll.DelUser(uid);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除用户信息错误(DelUser):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}