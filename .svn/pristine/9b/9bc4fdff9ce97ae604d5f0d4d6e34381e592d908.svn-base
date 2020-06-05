using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Opertion.User
{
    public partial class ExpUserHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Syscont.ExpUserBLL bll = null;
        public ExpUserHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Syscont.ExpUserBLL(user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetUser()
        {
            APIRst rst = new APIRst();
            try
            {
                var dt = bll.GetUser(0);
                var dtsource = from s1 in dt.AsEnumerable()
                               select new
                               {
                                   RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                                   Rid = CommFunc.ConvertDBNullToInt32(s1["Role_id"]),
                                   Ledger = CommFunc.ConvertDBNullToInt32(s1["Ledger"]),
                                   Uid = CommFunc.ConvertDBNullToInt32(s1["Uid"]),
                                   Name = CommFunc.ConvertDBNullToString(s1["UName"]),
                                   Pwd = CommFunc.ConvertDBNullToString(s1["UPasswd"]),
                                   SName = CommFunc.ConvertDBNullToString(s1["USign"]),
                                   RName = CommFunc.ConvertDBNullToString(s1["Descr"]),
                                   TelNo = CommFunc.ConvertDBNullToString(s1["TelNo"]),
                                   Create_by = CommFunc.ConvertDBNullToInt32(s1["Create_by"]),
                                   Create_byName = CommFunc.ConvertDBNullToString(s1["Create_byName"]),
                                   Create_dt = CommFunc.ConvertDBNullToDateTime(s1["Create_dt"]).ToString("yyyy-MM-dd")
                               };
                rst.data = dtsource.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取用户信息:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <param name="Uid">用户ID</param>
        /// <param name="sAct">操作类型：增加=1，修改=2，删除=3</param>
        /// <param name="Rid">用户权限</param>
        /// <param name="Name">用户名称</param>
        /// <param name="SName">登录名</param>
        /// <param name="Pwd">登录密码</param>
        /// <returns></returns>
        public APIRst SetUser(int Uid, int sAct, int Rid, string Name, string SName, string Pwd, string TelNo)
        {
            APIRst rst = new APIRst();
            try
            {
                int id = CommFunc.ConvertDBNullToInt32(Uid);//用户ID号
                int nAct = CommFunc.ConvertDBNullToInt32(sAct);
                sys_user uu = new sys_user();
                uu.Uid = id;
                uu.TelNo = TelNo;
                if (nAct == 2 || nAct == 3)
                {
                    if (Uid == 0) throw new Exception(" 数据出错，请选择行再重试..");

                }
                if (nAct == 1 || nAct == 2)
                {
                    uu.Role_id = Rid;//用户权限
                    uu.UName = Name;//用户名称
                    uu.USign = SName;//登录名
                    uu.UPasswd = Pwd;//登录密码
                }
                if (nAct != 1 && nAct != 2 && nAct != 3)
                    throw new Exception(" 操作类型错误");
                if (nAct != 3 && string.IsNullOrEmpty(uu.UName))
                    throw new Exception(" 请输入用户名称");
                if (nAct != 3 && uu.Role_id == 0)
                    throw new Exception(" 请选择用户角色");
                if (nAct != 3 && string.IsNullOrEmpty(uu.UPasswd))
                    throw new Exception(" 请输入用户密码");

                if (nAct == 3)
                    bll.DelUser(Uid);
                else
                    bll.EditUser(uu, nAct);
                DataTable dtSource = bll.GetUser(uu.Uid);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               Rid = CommFunc.ConvertDBNullToInt32(s1["Role_id"]),
                               Ledger = CommFunc.ConvertDBNullToInt32(s1["Ledger"]),
                               Uid = CommFunc.ConvertDBNullToInt32(s1["Uid"]),
                               Name = CommFunc.ConvertDBNullToString(s1["UName"]),
                               Pwd = CommFunc.ConvertDBNullToString(s1["UPasswd"]),
                               SName = CommFunc.ConvertDBNullToString(s1["USign"]),
                               RName = CommFunc.ConvertDBNullToString(s1["Descr"]),
                               TelNo = CommFunc.ConvertDBNullToString(s1["TelNo"]),
                               Create_by = CommFunc.ConvertDBNullToInt32(s1["Create_by"]),
                               Create_byName = CommFunc.ConvertDBNullToString(s1["Create_byName"]),
                               Create_dt = CommFunc.ConvertDBNullToDateTime(s1["Create_dt"]).ToString("yyyy-MM-dd")
                           };

                rst.data = res1.ToList();

            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取充值参数信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


    }
}