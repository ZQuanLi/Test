using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Opertion.User
{
    public partial class ExpRoleHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Syscont.ExpRoleBLL bll = null;
        public ExpRoleHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Syscont.ExpRoleBLL(user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetRole()
        {
            APIRst rst = new APIRst();
            try
            {
                var dt = bll.GetRole(0);
                var dtsource = from s1 in dt.AsEnumerable()
                               select new
                               {
                                   RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                                   UserType = CommFunc.ConvertDBNullToInt32(s1["UserType"]),
                                   role_id = CommFunc.ConvertDBNullToString(s1["role_id"]),
                                   role_idS = CommFunc.ConvertDBNullToString(s1["descr"]),
                               };
                rst.data = dtsource.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取角色信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置角色信息
        /// </summary>
        /// <param name="Role_id">权限ID</param>
        /// <param name="nAct">操作类型：增加=1，修改=2，删除=3</param>
        /// <param name="role_idS">角色名称</param>
        /// <returns></returns>
        public APIRst SetRole(int Role_id, int nAct, string role_idS)
        {
            APIRst rst = new APIRst();
            try
            {
                sys_role model = new sys_role();
                model.Role_id = Role_id;
                model.Descr = role_idS;
                if (nAct != 1 && nAct != 2 && nAct != 3)
                    throw new Exception("操作类型错误");
                if (nAct != 1 && model.Role_id == 0)
                    throw new Exception("角色ID错误");
                if (nAct != 3 && string.IsNullOrEmpty(model.Descr))
                    throw new Exception("请输入角色名称");

                if (nAct == 3)
                    bll.DelRole(model.Role_id);
                else
                    bll.EditRole(model, nAct);
                DataTable dtSource = bll.GetRole(model.Role_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               role_id = CommFunc.ConvertDBNullToString(s1["Role_id"]),
                               role_idS = CommFunc.ConvertDBNullToString(s1["Descr"]),
                           };

                rst.data = res1.ToList();

            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置角色信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取基本权限
        /// </summary>
        /// <param name="id">权限ID</param>
        /// <returns></returns>
        public APIRst GetPowerById(int id)
        {
            APIRst rst = new APIRst();
            try
            {
                int total = 0;

                List<Treeview> tr = bll.GetPowerById(id, out total);
                object obj = new { total = total, rows = tr };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取区域树形列表错误(GetPowerById):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置基本权限
        /// </summary>
        /// <param name="pRole_id">权限ID</param>
        /// <param name="ids">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        public APIRst SetPower(int pRole_id, string ids)
        {
            APIRst rst = new APIRst();
            try
            {
                if (pRole_id <= 0)
                    throw new Exception("参数错误，请刷新重试...");
                if (ids.Length <= 2)
                    throw new Exception("请选择基本权限菜单，再保存...");

                rst.data = bll.SetPower(pRole_id, ids);

            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置基本权限信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取区域权限信息
        /// </summary>
        /// <param name="id">权限ID</param>
        /// <returns></returns>
        public APIRst GetAreaById(int id)
        {
            APIRst rst = new APIRst();
            try
            {
                int total = 0;
                int role_id = CommFunc.ConvertDBNullToInt32(id);
                //var dt2 = bll.GetAreaById2();
                List<Treeview> dt = bll.GetAreaById(role_id, WebConfig.SysProject, out total);
                object obj = new { total = total, rows = dt };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取区域树形列表错误(GetAreaById):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置区域权限信息
        /// </summary>
        /// <param name="pRole_id">权限ID</param>
        /// <param name="ids">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        public APIRst SetPowerArea(int pRole_id, string ids)
        {
            APIRst rst = new APIRst();
            try
            {
                if (pRole_id <= 0)
                    throw new Exception("参数错误，请刷新重试...");
                if (ids.Length <= 1)
                    throw new Exception("请选择基本权限菜单，再保存...");

                rst.data = bll.SetPowerArea(pRole_id, ids, WebConfig.SysProject);

            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("置区域权限信息信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


    }
}