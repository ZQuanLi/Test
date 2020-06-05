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
        #region 区域信息
        /// <summary>
        /// 获取区域树形列表
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public APIResult GetAreaTreeList(int co_id)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetAreaList(co_id, "");
                List<Treeview> tr = this.GetAreaTreeList(dtSource);
                object obj = new { total = dtSource.Rows.Count, rows = tr };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = obj;
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取区域树形列表错误(GetAreaList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        //

        /// <summary>
        /// 获取区域树形列表
        /// </summary>
        /// <param name="co_id"></param>
        /// <param name="coName"></param>
        /// <returns></returns>
        public APIResult GetAreaList(int co_id,string coName)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetAreaList(co_id, coName);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               AreaId = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               AreaName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Parent_id = CommFunc.ConvertDBNullToInt32(s1["Parent_id"]),
                               ParentName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
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
                FileLog.WriteLog("获取区域列表错误(GetAreaList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private List<Treeview> GetAreaTreeList(DataTable dtSource)
        {
            List<Treeview> rst = new List<Treeview>();
            DataRow[] pArr = dtSource.Select("Parent_id=0", "Co_id");
            foreach (DataRow dr in pArr)
            {
                int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
                Treeview pTr = new Treeview();
                pTr.id = co_id.ToString();
                pTr.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                pTr.attributes = 0; //;new { areaId = co_id, attrib = CoAttrib.Area, parent_id = 0 };
                this.GetAreaTreeList(co_id, ref pTr, ref dtSource);
                rst.Add(pTr);
            }
            return rst;
        }
        private void GetAreaTreeList(int parent_id, ref Treeview pTr, ref DataTable dtSource)
        {
            DataRow[] pArr = dtSource.Select("Parent_id=" + parent_id, "Co_id");
            int pRows = pArr.Count();
            if (pRows > 0)
                pTr.nodes = new List<Treeview>();
            foreach (DataRow dr in pArr)
            {
                int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
                Treeview cTr = new Treeview();
                cTr.id = co_id.ToString();
                cTr.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                cTr.attributes = parent_id; // new { areaId = co_id, attrib = CoAttrib.Area, parent_id = parent_id };
                pTr.nodes.Add(cTr);
                this.GetAreaTreeList(co_id, ref cTr, ref dtSource);
            }
        }

        /// <summary>
        /// 设置区域信息
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public APIResult SetArea(AreaVModel area)
        {
            APIResult rst = new APIResult();
            try
            {
                rst.Code = 0;
                rst.Msg = "";
                bll.SetArea(area);
                rst.Data = area.AreaId;
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("设置区域信息错误(SetArea):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="co_id">ID号</param>
        /// <returns></returns>
        public APIResult DelCoInfo(int co_id)
        {
            APIResult rst = new APIResult();
            try
            {
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = bll.DelCoInfo(co_id);
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("删除建筑信息错误(DelArea):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        #endregion

        #region 机房信息
        /// <summary>
        /// 获取区域下机房，站点个数
        /// </summary>
        /// <param name="areaId">区域ID号</param>
        /// <returns></returns>
        public APIResult GetAreaCountInfo(int areaId)
        {
            APIResult rst = new APIResult();
            try
            {
                int sCnt = bll.GetStationCount(areaId);
                int rCnt = bll.GetRoomCount(areaId);
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { stationCount = sCnt,roomCount = rCnt };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取区域下机房，站点个数错误(GetAreaCountInfo):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取机房列表
        /// </summary>
        /// <param name="stationId">机房Id号</param>
        /// <param name="name">机房名称</param>
        /// <returns></returns>
        public APIResult GetStationList(int stationId, string name)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetStationList(stationId,name);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,//需要
                               StationId = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),//机房ID号
                               StationName = CommFunc.ConvertDBNullToString(s1["CoName"]),// 机房名称
                               StationNo = CommFunc.ConvertDBNullToString(s1["CoNo"]),// 机房编码
                               StationType = CommFunc.ConvertDBNullToInt32(s1["Cic_id"]),//机房类型
                               Address = CommFunc.ConvertDBNullToString(s1["CustAddr"]),//机房地址
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),   // 备注
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),// 是否弃用
                               AreaId = CommFunc.ConvertDBNullToInt32(s1["Parent_id"]), // 区域ID号
                               RoomCnt = 0, //直连站点数量
                               MeterCnt = 0, //下挂（直连站点）电表总数
                               AreaName = CommFunc.ConvertDBNullToString(s1["AreaName"]), // 区域名称
                               StationTypeS = CommFunc.ConvertDBNullToString(s1["CicName"]), // 机房类型描述
                           };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { total = dtSource.Rows.Count, rows = res1.ToList() };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取机房列表错误(GetStationList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 机房类型
        /// </summary>
        /// <returns></returns>
        public APIResult GetStationType()
        {
            return GetCicList(CicAttrib.StationType);
        }

        /// <summary>
        /// 设置机房信息
        /// </summary>
        /// <param name="station">机房信息</param>
        /// <returns></returns>
        public APIResult SetStation(StationVModel station)
        {
            APIResult rst = new APIResult();
            try
            {
                rst.Code = 0;
                rst.Msg = "";
                bll.SetStation(station);
                rst.Data = this.GetStationList(station.StationId, "");
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("设置机房信息错误(SetStation):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        #endregion

        #region 站点信息
        /// <summary>
        /// 获取站点列表
        /// </summary>
        public APIResult GetRoomList(int roomId,string roomName)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetRoomList(roomId, roomName);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               RoomId = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               RoomName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               RoomNo = CommFunc.ConvertDBNullToString(s1["CoNo"]),
                               Address = CommFunc.ConvertDBNullToString(s1["CustAddr"]),
                               RoomSight = CommFunc.ConvertDBNullToInt32(s1["Cic_id"]),//站点类型（场景）
                               Switch = CommFunc.ConvertDBNullToString(s1["Switch"]),//供电类型ID（区分转供电，直供电）
                               RoomType = CommFunc.ConvertDBNullToString(s1["CoType"]),//站点类型（区分宏站和室分）
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               StationId = CommFunc.ConvertDBNullToInt32(s1["Parent_id"]),
                               StationName = CommFunc.ConvertDBNullToString(s1["StationName"]),
                               RoomSightName = CommFunc.ConvertDBNullToString(s1["CicName"]), //站点类型名称
                               SwitchName = CommFunc.GetEnumDisplay(typeof(Switch), CommFunc.ConvertDBNullToString(s1["Switch"])),//供电类型（区分转供电，直供电）
                               RoomTypeName = CommFunc.GetEnumDisplay(typeof(CoType), CommFunc.ConvertDBNullToString(s1["CoType"])),//站点类型（区分宏站和室分）
                               MeterCnt = 0, //下挂（直连站点）电表总数
                           };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { total = dtSource.Rows.Count, rows = res1.ToList() };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取站点列表错误(GetRoomList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 站点类型（场景）
        /// </summary>
        /// <returns></returns>
        public APIResult GetSelectRoomSight()
        {
            return GetCicList(CicAttrib.Scene);
        }

        /// <summary>
        /// 获取站点供电类型
        /// </summary>
        /// <returns></returns>
        public APIResult GetSelectSwitch(string type)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetSelectSwitch(type);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToString(s1["Id"]),
                               Text = CommFunc.ConvertDBNullToString(s1["Text"]),
                           };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { total = dtSource.Rows.Count, rows = res1.ToList() };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取站点供电类型错误(GetSelectSwitch):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取站点类型
        /// </summary>
        /// <returns></returns>
        public APIResult GetSelectRoomType()
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetSelectRoomType();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToString(s1["Id"]),
                               Text = CommFunc.ConvertDBNullToString(s1["Text"]),
                           };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { total = dtSource.Rows.Count, rows = res1.ToList() };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取站点类型错误(GetSelectRoomType):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置站点信息
        /// </summary>
        /// <param name="room">站点信息</param>
        /// <returns></returns>
        public APIResult SetRoom(RoomVModel room)
        {
            APIResult rst = new APIResult();
            try
            {
                rst.Code = 0;
                rst.Msg = "";
                int cnt = bll.SetRoom(room);
                rst.Data = this.GetRoomList(room.RoomId, "");
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("设置站点信息错误(SetRoom):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        #endregion

        #region 场景、公司、机房类型列表
        /// <summary>
        /// 获取场景列表
        /// </summary>
        /// <returns></returns>
        private APIResult GetCicList(CicAttrib cicAttrib)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetCicList(cicAttrib);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToInt32(s1["Cic_id"]),
                               Text = CommFunc.ConvertDBNullToString(s1["CicName"]),
                           };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { total = dtSource.Rows.Count, rows = res1.ToList() };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取场景列表错误(GetCicList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        #endregion
    }
}