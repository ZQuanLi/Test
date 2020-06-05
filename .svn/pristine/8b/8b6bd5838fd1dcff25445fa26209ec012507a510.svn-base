using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Threading.Tasks;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.SystemMgr.Controllers
{
    partial class BaseInfoHelper
    {
        public APIResult GetCrmList(int crm_id,string crmName)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetCrmList(0, crmName);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Crm_id = CommFunc.ConvertDBNullToInt32(s1["Crm_id"]),
                               CrmNo = CommFunc.ConvertDBNullToString(s1["CrmNo"]),
                               CrmName = CommFunc.ConvertDBNullToString(s1["CrmName"]),
                               MPhone = CommFunc.ConvertDBNullToString(s1["MPhone"]),
                               Email = CommFunc.ConvertDBNullToString(s1["Email"]),
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
                FileLog.WriteLog("获取业主信息错误(GetCrmList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIResult SetCrm(CrmVModel crm)
        {
            APIResult rst = new APIResult();
            try
            {
                bll.SetCrm(crm);
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = this.GetCrmList(crm.Crm_id, "");
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("设置业主信息错误(SetCrm):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIResult DelCrm(int crm_id)
        {
            APIResult rst = new APIResult();
            try
            {                
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = bll.DelCrm(crm_id);
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("删除业主信息错误(DelCrm):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIResult GetCrmOfRoomList(int crm_id)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetCrmOfRoomList(crm_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Crm_id = CommFunc.ConvertDBNullToInt32(s1["Crm_id"]),
                               RoomId = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               RoomName = CommFunc.ConvertDBNullToString(s1["CoFullName"]),
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
                FileLog.WriteLog("获取业主站点信息错误(GetCrmOfRoomList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取没有业主信息的站点信息
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public APIResult GetRoomNotExisCrmList(string roomName)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetRoomNotExisCrmList(roomName);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Crm_id = 0,
                               RoomId = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               RoomName = CommFunc.ConvertDBNullToString(s1["CoFullName"]),
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
                FileLog.WriteLog("获取没有业主信息的站点信息(GetRoomNotExisCrmList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        public APIResult SetCrmOfRoom(int crm_id, string roomStrlist)
        {
            APIResult rst = new APIResult();
            try
            {                
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = bll.SetCrmOfRoom(crm_id, roomStrlist); 
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("设置业主的站点信息错误(SetCrmOfRoom):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIResult DelCrmOfRoom(int crm_id, string roomStrlist)
        {
            APIResult rst = new APIResult();
            try
            {
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = bll.DelCrmOfRoom(crm_id, roomStrlist);
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("删除业主的站点信息错误(DelCrmOfRoom):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}