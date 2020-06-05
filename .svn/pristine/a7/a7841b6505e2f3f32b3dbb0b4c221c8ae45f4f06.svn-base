using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.ExpApp.SysMgr
{
    public class SysMgrBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private YDS6000.DAL.ExpApp.SysMgr.SysMgrDAL dal = null;

        public SysMgrBLL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            dal = new YDS6000.DAL.ExpApp.SysMgr.SysMgrDAL(ledger, uid);
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="crm_id">证件号（登陆号码）</param>
        /// <returns></returns>
        public DataTable GetV3_User(int crm_id)
        {
            return dal.GetV3_User(crm_id);
        }


        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="crm_id"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool SetPasspwd(int crm_id,string pwd)
        {
            return dal.SetPasspwd(crm_id, pwd);
        }

        /// <summary>
        /// 获取房间列表
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public DataTable GetRoomList(string roomName)
        {
            return dal.GetRoomList(roomName);
        }

        /// <summary>
        /// 账号管理
        /// </summary>
        /// <param name="crmName">名称</param>
        /// <param name="co_id">房间ID</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public int MgrUpdate(string crmName, int co_id, string pwd)
        {
            return dal.MgrUpdate(crmName, co_id, pwd);
        }

        /// <summary>
        /// 获取客户登陆号码是否存在
        /// </summary>
        /// <param name="crmNo">证件号（登陆号码）</param>
        /// <returns></returns>
        public bool GetExistsCrmNo(string crmNo)
        {
            return dal.GetExistsCrmNo(crmNo);
        }

        public int Register(string crmNo,string pwd)
        {
            return dal.Register(crmNo, pwd);
        }
    }
}
