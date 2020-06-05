using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ComponentModel.DataAnnotations;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.SystemMgr.Controllers
{
    /// <summary>
    /// 基础信息管理
    /// </summary>
    [AuthorizeAttribute]
    [RoutePrefix("api/BaseInfo")]
    public class BaseInfoController : ApiController
    {
        private BaseInfoHelper baseInfoHelper = new BaseInfoHelper();

        #region 区域信息
        /// <summary>
        /// 获取区域信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAreaList")]
        public APIResult GetAreaList()
        {
            return baseInfoHelper.GetAreaTreeList(0);
        }
        /// <summary>
        /// 获取区域信息
        /// </summary>
        /// <param name="name">区域名称</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAreaData")]
        public APIResult GetAreaData(string name = "")
        {
            return baseInfoHelper.GetAreaList(0, name);
        }

        /// <summary>
        /// 新增区域信息
        /// </summary>
        /// <param name="area">区域信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddArea")]
        public APIResult AddArea(AreaVModel area)
        {
            area.AreaId = 0;
            return baseInfoHelper.SetArea(area);
        }
        /// <summary>
        /// 修改区域信息
        /// </summary>
        /// <param name="area">区域信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetArea")]
        public APIResult SetArea(AreaVModel area)
        {
            return baseInfoHelper.SetArea(area);
        }
        /// <summary>
        /// 删除区域信息
        /// </summary>
        /// <param name="id">区域ID号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DelArea")]
        public APIResult DelArea(int id)
        {
            return baseInfoHelper.DelCoInfo(id);
        }
        #endregion

        #region 机房信息
        /// <summary>
        /// 获取机房信息
        /// </summary>
        /// <param name="name">机房名称</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetStationList")]
        public APIResult GetStationList(string name = "")
        {
            return baseInfoHelper.GetStationList(0, CommFunc.ConvertDBNullToString(name));
        }

        /// <summary>
        /// 获取机房类型信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetStationType")]
        public APIResult GetStationType()
        {
            return baseInfoHelper.GetStationType();
        }

        /// <summary>
        /// 新增机房信息
        /// </summary>
        /// <param name="station">机房信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddStation")]
        public APIResult AddStation(StationVModel station)
        {
            station.StationId = 0;
            return baseInfoHelper.SetStation(station);
        }

        /// <summary>
        /// 修改机房信息
        /// </summary>
        /// <param name="station">机房信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetStation")]
        public APIResult SetStation(StationVModel station)
        {           
            return baseInfoHelper.SetStation(station);
        }
        /// <summary>
        /// 删除机房信息
        /// </summary>
        /// <param name="id">机房ID号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DelStation")]
        public APIResult DelStation(int id)
        {
            return baseInfoHelper.DelCoInfo(id);
        }
        #endregion

        #region 站点信息
        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="name">站点名称</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRoomList")]
        public APIResult GetRoomList(string name = "")
        {
            return baseInfoHelper.GetRoomList(0,CommFunc.ConvertDBNullToString(name));
        }

        /// <summary>
        /// 获取站点场景信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSelectRoomSight")]
        public APIResult GetSelectRoomSight()
        {
            return baseInfoHelper.GetSelectRoomSight();
        }

        /// <summary>
        /// 获取站点供电类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSelectSwitch")]
        public APIResult GetSelectSwitch()
        {
            return baseInfoHelper.GetSelectSwitch("Rm");
        }

        /// <summary>
        /// 获取站点类型(区分宏站和室分)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSelectRoomType")]
        public APIResult GetSelectRoomType()
        {
            return baseInfoHelper.GetSelectRoomType();
        }
        /// <summary>
        /// 新增站点信息
        /// </summary>
        /// <param name="room">站点信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddRoom")]
        public APIResult AddRoom(RoomVModel room)
        {
            room.RoomId = 0;
            return baseInfoHelper.SetRoom(room);
        }
        /// <summary>
        /// 修改站点信息
        /// </summary>
        /// <param name="room">站点信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetRoom")]
        public APIResult SetRoom(RoomVModel room)
        {
            return baseInfoHelper.SetRoom(room);
        }
        /// <summary>
        /// 删除站点信息
        /// </summary>
        /// <param name="id">站点ID号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DelRoom")]
        public APIResult DelRoom(int id)
        {
            return baseInfoHelper.DelCoInfo(id);
        }
        #endregion

        #region 集中器信息
        /// <summary>
        /// 获取集中器信息
        /// </summary>
        /// <param name="name">集中器名称</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEspList")]
        public APIResult GetEspList(string name = "")
        {
            return baseInfoHelper.GetEspList(0,CommFunc.ConvertDBNullToString(name));
        }
        /// <summary>
        /// 新增集中器信息
        /// </summary>
        /// <param name="esp">集中器信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddEsp")]
        public APIResult AddEsp(EspVModel esp)
        {
            esp.Esp_id = 0;
            return baseInfoHelper.SetEsp(esp);
        }
        /// <summary>
        /// 修改集中器信息
        /// </summary>
        /// <param name="esp">集中器信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetEsp")]
        public APIResult SetEsp(EspVModel esp)
        {
            return baseInfoHelper.SetEsp(esp);
        }
        /// <summary>
        /// 删除集中器信息
        /// </summary>
        /// <param name="id">集中器ID号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DelEsp")]
        public APIResult DelEsp(int id)
        {
            return baseInfoHelper.DelEsp(id);
        }
        #endregion

        #region 设备型号信息
        /// <summary>
        /// 获取电表类型下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMeterTypeComBox")]
        public APIResult GetMeterTypeComBox()
        {
            return baseInfoHelper.GetMeterTypeComBox();
        }
        /// <summary>
        /// 获取电表类型列表
        /// </summary>
        /// <param name="name">电表类型名称</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMeterTypeList")]
        public APIResult GetMeterTypeList(string name = "")
        {
            return baseInfoHelper.GetMmList(0, CommFunc.ConvertDBNullToString(name));
        }
        /// <summary>
        /// 新增电表类型列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddMeterType")]
        public APIResult AddMeterType(MeterTypeVModel meterType)
        {
            meterType.ModuleTypeId = 0;
            return baseInfoHelper.SetMm(meterType);
        }
        /// <summary>
        /// 修改电表类型列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SetMeterType")]
        public APIResult SetMeterType(MeterTypeVModel meterType)
        {
            return baseInfoHelper.SetMm(meterType);
        }
        /// <summary>
        /// 删除电表类型列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DelMeterType")]
        public APIResult DelMeterType(int id)
        {
            return baseInfoHelper.DelMm(id);
        }
        #endregion

        #region 设备信息
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="name">设备名称</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMeterList")]
        public APIResult GetMeterList(string name = "")
        {
            return baseInfoHelper.GetMeterList(0, CommFunc.ConvertDBNullToString(name));
        }
        /// <summary>
        /// 新增设备信息
        /// </summary>
        /// <param name="meter">设备信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddMeter")]
        public APIResult AddMeter(MeterVModel meter)
        {
            meter.Meter_id = 0;
            return baseInfoHelper.SetMeter(meter);
        }
        /// <summary>
        /// 修改设备信息
        /// </summary>
        /// <param name="meter">设备信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetMeter")]
        public APIResult SetMeter(MeterVModel meter)
        {
            return baseInfoHelper.SetMeter(meter);
        }
        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="id">设备信息ID号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DelMeter")]
        public APIResult DelMeter(int id)
        {
            return baseInfoHelper.DelMeter(id);
        }
        #endregion

        #region 回路管理
        /// <summary>
        /// 获取缴费方式列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSelectChrgType")]
        public APIResult GetSelectChrgType()
        {
            return baseInfoHelper.GetChrgTypeList();
        }
        /// <summary>
        /// 获取供电方式列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSelectPSWay")]
        public APIResult GetSelectPSWay()
        {
            return baseInfoHelper.GetSelectSwitch("Md");
        }

        /// <summary>
        /// 获取回路信息
        /// </summary>
        /// <param name="name">设备名称</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetModuleList")]
        public APIResult GetModuleList(string name = "")
        {
            return baseInfoHelper.GetModuleList(0, CommFunc.ConvertDBNullToString(name));
        }
        /// <summary>
        /// 新增回路信息
        /// </summary>
        /// <param name="module">回路信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddModule")]
        public APIResult AddModule(ModuleVModel module)
        {
            module.Module_id = 0;
            return baseInfoHelper.SetModule(module);
        }
        /// <summary>
        /// 修改回路信息
        /// </summary>
        /// <param name="module">回路信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetModule")]
        public APIResult SetModule(ModuleVModel module)
        {
            return baseInfoHelper.SetModule(module);
        }
        /// <summary>
        /// 删除回路信息
        /// </summary>
        /// <param name="id">回路ID号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DelModule")]
        public APIResult DelModule(int id)
        {
            return baseInfoHelper.DelModule(id);
        }
        #endregion

        #region 回路采集点映射
        /// <summary>
        /// 获取采集码列表
        /// </summary>
        /// <param name="id">回路ID号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMapFunTypeList")]
        public APIResult GetMapFunTypeList(int id = 0)
        {
            return baseInfoHelper.GetMapFunTypeList(id, 0);
        }
        /// <summary>
        /// 映射采集码对应的TagName
        /// </summary>
        /// <param name="tag">映射信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetMapFunType")]
        public APIResult SetMapFunType(MapVModel tag)
        {
            return baseInfoHelper.SetMapFunType(tag);
        }
        #endregion

        #region 业主信息
        /// <summary>
        /// 获取业主信息
        /// </summary>
        /// <param name="name">业主名称</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCrmList")]
        public APIResult GetCrmList(string name = "")
        {
            return baseInfoHelper.GetCrmList(0, CommFunc.ConvertDBNullToString(name));
        }
        /// <summary>
        /// 新增业主信息
        /// </summary>
        /// <param name="crm">业主信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddCrm")]
        public APIResult AddCrm(CrmVModel crm)
        {
            crm.Crm_id = 0;
            return baseInfoHelper.SetCrm(crm);
        }
        /// <summary>
        /// 修改业主信息
        /// </summary>
        /// <param name="crm">业主信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetCrm")]
        public APIResult SetCrm(CrmVModel crm)
        {
            return baseInfoHelper.SetCrm(crm);
        }
        /// <summary>
        /// 删除业主信息
        /// </summary>
        /// <param name="id">业主ID信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DelCrm")]
        public APIResult DelCrm(int id)
        {
            return baseInfoHelper.DelCrm(id);
        }

        /// <summary>
        /// 获取业主的站点信息
        /// </summary>
        /// <param name="id">业主ID号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCrmOfRoomList")]
        public APIResult GetCrmOfRoomList(int id)
        {
            return baseInfoHelper.GetCrmOfRoomList(id);
        }
        /// <summary>
        /// 获取没有业主信息的站点信息
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRoomNotExisCrmList")]
        public APIResult GetRoomNotExisCrmList(string roomName = "")
        {
            return baseInfoHelper.GetRoomNotExisCrmList(roomName);
        }

        /// <summary>
        /// 设置业主的站点信息
        /// </summary>
        /// <param name="crm_id">业主ID号</param>
        /// <param name="roomStrlist">站点ID号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetCrmOfRoom")]
        public APIResult SetCrmOfRoom(int crm_id,string roomStrlist)
        {
            return baseInfoHelper.SetCrmOfRoom(crm_id, roomStrlist);
        }
        /// <summary>
        /// 删除业主的站点信息
        /// </summary>
        /// <param name="crm_id">业主ID号</param>
        /// <param name="roomStrlist">站点ID号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DelCrmOfRoom")]
        public APIResult DelCrmOfRoom(int crm_id, string roomStrlist)
        {
            return baseInfoHelper.DelCrmOfRoom(crm_id, roomStrlist);
        }
        #endregion
    }
}
