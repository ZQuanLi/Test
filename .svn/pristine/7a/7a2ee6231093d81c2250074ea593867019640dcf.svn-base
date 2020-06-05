using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.SystemMgr.Controllers
{
    partial class UserHelper
    {
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="role_id"></param>
        /// <returns></returns>
        public APIResult GetRoleList(int role_id)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetRoleList(role_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Role_id = CommFunc.ConvertDBNullToInt32(s1["Role_id"]),
                               RoleName = CommFunc.ConvertDBNullToString(s1["Descr"]),
                               UserType = CommFunc.ConvertDBNullToInt32(s1["UserType"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = obj;
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取角色列表错误(GetRoleList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 设置角色信息
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns></returns>
        public APIResult SetRole(sys_role role)
        {
            APIResult rst = new APIResult();
            try
            {
                rst.Data = bll.SetRole(role);
                rst.Code = 0;
                rst.Msg = "";
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("设置角色信息错误(SetRole):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="role_id">角色ID号</param>
        /// <returns></returns>
        public APIResult DelRole(int role_id)
        {
            APIResult rst = new APIResult();
            try
            {
                rst.Data = bll.DelRole(role_id);
                rst.Code = 0;
                rst.Msg = "";
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("删除角色信息错误(DelRole):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

    }
}