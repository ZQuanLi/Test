using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.ExpApp.Opertion.SysMgr
{
    public class SysMgrHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.ExpApp.SysMgr.SysMgrBLL bll = null;
        public SysMgrHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.ExpApp.SysMgr.SysMgrBLL(user.Ledger, user.Uid);
        }

        /// <summary>
        /// 账号管理（修改密码）
        /// </summary>
        /// <param oldPwd="旧密码"></param>
        /// <param newPwd="新密码"></param>
        /// <param confirmPwd="确认密码"></param>
        /// <returns></returns>
        public APIRst SetPasspwd(string oldPwd, string newPwd, string confirmPwd)
        {
            APIRst rst = new APIRst();
            if (string.IsNullOrEmpty(oldPwd))
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "旧密码不能为空";
                return rst;
            }
            if (string.IsNullOrEmpty(newPwd))
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "密码不能为空";
                return rst;
            }
            if (!newPwd.Equals(confirmPwd))
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "密码与确认密码不一致";
                return rst;
            }
            try
            {
                DataTable dtSource = bll.GetV3_User(user.Uid);
                int nRows = dtSource.Rows.Count;
                if (nRows == 0)
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "没有此用户,请重新登录";
                    return rst;
                }
                else if (nRows != 1)
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "此用户有多个,请重新登录";
                    return rst;
                }

                int uid = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Crm_id"]);
                string uName = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["CrmName"]);
                string dbPwd = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["Passwd"]);
                string project = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["Project"]);
                if (!oldPwd.Trim().Equals(dbPwd))
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "密码错误";
                    return rst;
                }
                rst.rst = bll.SetPasspwd(user.Uid, newPwd);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("修改密码错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 房间列表
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public APIRst GetRoomList(string roomName)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetRoomList(roomName);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               CoFullName = CommFunc.ConvertDBNullToString(s1["CoFullName"]),
                           };
                rst.rst = true;
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("房间列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 账号管理
        /// </summary>
        /// <param name="crmName">名称</param>
        /// <param name="co_id">房间ID</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public APIRst MgrUpdate(string crmName, int co_id, string pwd)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.MgrUpdate(crmName,co_id, pwd);
                rst.rst = true;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("账号管理错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 注册信息
        /// </summary>
        /// <param name="name">登陆名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public APIRst Register(string name, string pwd)
        {
            APIRst rst = new APIRst();
            try
            {
                int ledger = 0;
                DataTable dt = new YDS6000.BLL.ExpApp.Home.HomeBLL(9999, 0).GetProjectList();
                foreach (DataRow dr in dt.Rows)
                    ledger = CommFunc.ConvertDBNullToInt32(dr["Rule"]);
                //
                bll = new YDS6000.BLL.ExpApp.SysMgr.SysMgrBLL(ledger, 1);
                bool existName = bll.GetExistsCrmNo(name);
                if (existName == true)
                {
                    rst.rst = false;
                    rst.data = "";
                    rst.err = new APIErr() { code = -1, msg = "登录名已存在" };
                    return rst;
                }
                rst.data = bll.Register(name, pwd);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("注册信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

    }
}