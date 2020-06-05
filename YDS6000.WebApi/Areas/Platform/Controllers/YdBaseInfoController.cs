using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Platform.Controllers
{
    /// <summary>
    /// 平台基础信息管理
    /// </summary>
    [RoutePrefix("api/Platform/BaseInfo")]
    public class YdBaseInfoController : ApiController
    {
        private BaseInfoHelper infoHelper = new BaseInfoHelper();

        #region 项目信息
        /// <summary>
        /// 获取项目信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetProList")]
        public APIRst GetProList()
        {
            return infoHelper.GetProList();
        }

        /// <summary>
        /// 新增项目
        /// </summary>
        /// <param name="pro">项目信息</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("AddPro")]
        public APIRst AddPro(ProVModel pro)
        {
            pro.Id = 0;
            return infoHelper.SetPro(pro);
        }
        /// <summary>
        /// 修改项目
        /// </summary>
        /// <param name="pro">项目信息</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetPro")]
        public APIRst SetPro(ProVModel pro)
        {
            return infoHelper.SetPro(pro);
        }
        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="id">项目ID号</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("DelPro")]
        public APIRst DelPro(int id)
        {
            return infoHelper.DelPro(id);
        }
        #endregion

        #region 建筑信息
        /// <summary>
        /// 获取建筑信息树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetBuildList")]
        public APIRst GetBuildList()
        {
            return infoHelper.GetBuildList();
        }

        /// <summary>
        /// 新增建筑信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("AddBuild")]
        public APIRst AddBuild(BuildVModel build)
        {
            build.Id = 0;
            return infoHelper.SetBuild(build);
        }
        /// <summary>
        /// 修改建筑信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetBuild")]
        public APIRst SetBuild(BuildVModel build)
        {
            return infoHelper.SetBuild(build);
        }
        /// <summary>
        /// 删除建筑信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("DelBuild")]
        public APIRst DelBuild(int id)
        {
            return infoHelper.DelBuild(id);
        }
        #endregion

        #region 设备类型信息

        /// <summary>
        /// 获取功能码类型数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMmDefineList")]
        public APIRst GetMmDefineList()
        {
            return infoHelper.GetMmDefineList();
        }


        /// <summary>
        /// 获取设备类型数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetModuleTypeList")]
        public APIRst GetModuleTypeList()
        {
            return infoHelper.GetModuleTypeList();
        }

        /// <summary>
        /// 新增设备类型数据
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("AddModuleType")]
        public APIRst AddModuleType(ModuleTypeVModel mtype)
        {
            mtype.ModuleTypeId = 0;
            return infoHelper.SetModuleType(mtype);
        }

        /// <summary>
        /// 修改设备类型数据
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetModuleType")]
        public APIRst SetModuleType(ModuleTypeVModel mtype)
        {
            return infoHelper.SetModuleType(mtype);
        }
        /// <summary>
        /// 删除设备类型数据
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("DelModuleType")]
        public APIRst DelModuleType(int id)
        {
            return infoHelper.DelModuleType(id);
        }

        /// <summary>
        /// 获取设备类型下的采集点数据
        /// </summary>
        /// <param name="id">设备型号ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetModuleFunList")]
        public APIRst GetModuleFunList(int id)
        {
            return infoHelper.GetModuleFunList(id);
        }
        /// <summary>
        /// 获取设备类型下的采集点数据
        /// </summary>
        /// <param name="fun">采集点信息</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("AddModuleFun")]
        public APIRst AddModuleFun(ModuleFunVModel fun)
        {
            fun.Fun_id = 0;
            return infoHelper.SetModuleFun(fun);
        }

        /// <summary>
        /// 获取设备类型下的采集点数据
        /// </summary>
        /// <param name="fun">采集点信息</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetModuleFun")]
        public APIRst SetModuleFun(ModuleFunVModel fun)
        {            
            return infoHelper.SetModuleFun(fun);
        }
        /// <summary>
        /// 获取设备类型下的采集点数据
        /// </summary>
        /// <param name="id">采集点ID号</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("DelModuleFun")]
        public APIRst DelModuleFun(int id)
        {
            return infoHelper.DelModuleFun(id);
        }

        #endregion

        #region 设备信息
        /// <summary>
        /// 获取设备信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMeterList")]
        public APIRst GetMeterList()
        {
            return infoHelper.GetMeterList();
        }
        /// <summary>
        /// 新增设备信息列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("AddMeter")]
        public APIRst AddMeter(MdVModel md)
        {
            md.Meter_id = 0;
            return infoHelper.SetMeter(md);
        }
        /// <summary>
        /// 修改设备信息列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetMeter")]
        public APIRst SetMeter(MdVModel md)
        {
            return infoHelper.SetMeter(md);
        }
        /// <summary>
        /// 删除设备信息列表
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("DelMeter")]
        public APIRst DelMeter(int id)
        {
            return infoHelper.DelMeter(id);
        }

        /// <summary>
        /// 获取建筑下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetBuildCombox")]
        public APIRst GetBuildCombox()
        {
            return infoHelper.GetBuildCombox();
        }

        /// <summary>
        /// 获取能耗下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetEnergyCombox")]
        public APIRst GetEnergyCombox()
        {
            return infoHelper.GetEnergyCombox();
        }
        /// <summary>
        /// 获取父设备下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetParentMeterCombox")]
        public APIRst GetParentMeterCombox()
        {
            return infoHelper.GetParentMeterCombox();
        }

        /// <summary>
        /// 获取回路信息列表
        /// </summary>
        /// <param name="id">设备信息ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetModuleList")]
        public APIRst GetModuleList(int id)
        {
            return infoHelper.GetModuleList(id);
        }

        /// <summary>
        /// 设备属性下拉数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAttribCombox")]
        public APIRst GetAttribCombox()
        {
            return infoHelper.GetAttribCombox();
        }

        /// <summary>
        /// 获取进线下拉列表数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetIncomingCombox")]
        public APIRst GetIncomingCombox()
        {
            return infoHelper.GetIncomingCombox();
        }

        /// <summary>
        /// 设置回路信息列表
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="moduleName">回路名称</param>
        /// <param name="buildId">建筑ID号</param>
        /// <param name="energyItemCode">能源ID号</param>
        /// <param name="parent_id">父ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SetModuleList")]
        public APIRst SetModuleList(int module_id, string moduleName, int buildId, string energyItemCode = "", int parent_id = 0)
        {
            return infoHelper.SetModuleList(module_id, moduleName, buildId, energyItemCode, parent_id);
        }

        /// <summary>
        /// 获取回路采集项列表
        /// </summary>
        /// <param name="id">设备信息ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetModuleOfMapList")]
        public APIRst GetModuleOfMapList(int id)
        {
            return infoHelper.GetModuleOfMapList(id);
        }

        /// <summary>
        /// 设置回路采集项列表
        /// </summary>
        /// <param name="module_id">设备信息ID号</param>
        /// <param name="fun_id">设备采集项ID号</param>
        /// <param name="tagName">设备采集项映射变量</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SetModuleOfMapList")]
        public APIRst SetModuleOfMapList(int module_id,int fun_id,string tagName = "")
        {
            return infoHelper.SetModuleOfMapList(module_id, fun_id, tagName);
        }


        /// <summary>
        /// 获取设备信息列表(PDU)
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMeterList_PDU")]
        public APIRst GetMeterList_PDU()
        {
            return infoHelper.GetMeterList_PDU();
        }
        /// <summary>
        /// 新增设备信息列表(PDU)
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("AddMeter_PDU")]
        public APIRst AddMeter_PDU(MdVModel_PDU md)
        {
            md.Meter_id = 0;
            return infoHelper.SetMeter_PDU(md);
        }
        /// <summary>
        /// 修改设备信息列表(PDU)
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetMeter_PDU")]
        public APIRst SetMeter_PDU(MdVModel_PDU md)
        {
            return infoHelper.SetMeter_PDU(md);
        }
        /// <summary>
        /// 设置回路信息列表(PDU)
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="moduleName">回路名称</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SetModuleList_PDU")]
        public APIRst SetModuleList_PDU(int module_id, string moduleName)
        {
            return infoHelper.SetModuleList_PDU(module_id, moduleName);
        }

        #endregion
    }
}
