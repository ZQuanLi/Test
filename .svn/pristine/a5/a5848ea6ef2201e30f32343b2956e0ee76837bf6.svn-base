using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Syscont
{
    public partial class ExpYdCustomerBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Exp.Syscont.ExpYdCustomerDAL dal = null;
        public ExpYdCustomerBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Exp.Syscont.ExpYdCustomerDAL(_ledger, _uid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="crmid"></param>
        /// <param name="crmName"></param>
        /// <param name="crmNo"></param>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public DataTable GetCustomerList(int crmid, string crmName = "", string crmNo = "", string roomName = "")
        {
            return dal.GetCustomerList(crmid, crmName, crmNo, roomName);
        }

        /// <summary>
        /// 获取首页用户数（月数and总数）
        /// </summary>
        /// <returns></returns>
        public DataTable GetHomeCustomer(int or)
        {
            return dal.GetHomeCustomer(or);
        }

        /// <summary>
        /// 判断学号是否存在
        /// </summary>
        /// <param name="crmNo"></param>
        /// <returns></returns>
        public bool IsExistSameCrmNo(string crmNo)
        {
            return dal.IsExistSameCrmNo(crmNo);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Crm_id">主键ID号</param>
        /// <returns></returns>
        public int DelCustomer(int Crm_id)
        {
            return dal.DelCustomer(Crm_id);
        }

        public void EditCustomer(v3_userVModel model, int nAct)
        {
            dal.EditCustomer(model);
        }

        /// <summary>
        /// 获取组织
        /// </summary>
        /// <param name="coid">组织ID</param>
        /// <param name="isRoom">是查房间，还是查询建筑</param>
        /// <param name="bname">建筑</param>
        /// <param name="rname">房间名</param>
        /// <param name="strcName">树节点选中筛选</param>
        /// <returns></returns>
        public DataTable GetOrgs(int coid, bool isRoom = false, string bname = "", string rname = "", string strcName = "")
        {
            return dal.GetOrgs(coid, isRoom, bname, rname, strcName);
        }

        public DataTable GetYdCustomerRoomInfo(int crm_id)
        {
            return dal.GetYdCustomerRoomInfo(crm_id);
        }

        public DataTable GetYdCustomerRoomFind(string coName)
        {
            return dal.GetYdCustomerRoomFind(coName);
        }
        public int SaveYdCustomerOpenRoom(int crm_id, string coIdStr)
        {
            return dal.SaveYdCustomerOpenRoom(crm_id, coIdStr);
        }
        public int SaveYdCustomerOpenWd(int crm_id, string passwd)
        {
            return dal.SaveYdCustomerOpenWd(crm_id, passwd);
        }




    }
}
