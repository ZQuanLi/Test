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
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetRoleList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetRoleList();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Role_id = CommFunc.ConvertDBNullToInt32(s1["Role_id"]),
                               Descr = CommFunc.ConvertDBNullToString(s1["Descr"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code =(int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取角色列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置角色信息
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns></returns>
        public APIRst SetRole(sys_role role)
        {
            APIRst rst = new APIRst();
            try
            {
                bll.SetRole(role);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置角色信息错误(SetRole):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="role_id">角色ID号</param>
        /// <returns></returns>
        public APIRst DelRole(int role_id)
        {
            APIRst rst = new APIRst();
            try
            {
                bll.DelRole(role_id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除角色信息错误(DelRole):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取角色权限列表
        /// </summary>
        /// <param name="role_id">角色ID号</param>
        /// <returns></returns>
        public APIRst GetMenuOnOperateList(int role_id)
        {
            APIRst rst = new APIRst();
            try
            {
                int total = 0;
                List<Treeview> tr = bll.GetMenuOnOperateList(role_id, WebConfig.SysProject, out total);
                object obj = new { total = total, rows = tr };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取区域树形列表错误(GetMenuOnOperateList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 保存角色权限列表
        /// </summary>
        /// <param name="role_id">角色ID号</param>
        /// <param name="ids">权限ID号</param>
        /// <returns></returns>
        public APIRst SaveMenuOnOperateList(int role_id,string ids)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SaveMenuOnOperateList(role_id, ids);              
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("保存角色权限列表(SaveMenuOnOperateList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}