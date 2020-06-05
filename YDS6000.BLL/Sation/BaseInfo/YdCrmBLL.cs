using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using YDS6000.Models;
using YDS6000.DAL.BaseInfo;

namespace YDS6000.BLL.BaseInfo
{
    partial class BaseInfoBLL
    {
        public DataTable GetCrmList(int crm_id, string crmName)
        {
            return dal.GetCrmList(crm_id, crmName);
        }
        public int SetCrm(CrmVModel crm)
        {
            return dal.SetCrm(crm);
        }
        public int DelCrm(int crm_id)
        {
            return dal.DelCrm(crm_id);
        }

        public DataTable GetCrmOfRoomList(int crm_id)
        {
            return dal.GetCrmOfRoomList(crm_id);
        }
        /// <summary>
        /// 获取没有业主信息的站点信息
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public DataTable GetRoomNotExisCrmList(string roomName)
        {
            return dal.GetRoomNotExisCrmList(roomName);
        }
        public int SetCrmOfRoom(int crm_id, string roomStrlist)
        {
            return dal.SetCrmOfRoom(crm_id, roomStrlist);
        }

        public int DelCrmOfRoom(int crm_id, string roomStrlist)
        {
            return dal.DelCrmOfRoom(crm_id, roomStrlist);
        }
    }
}
