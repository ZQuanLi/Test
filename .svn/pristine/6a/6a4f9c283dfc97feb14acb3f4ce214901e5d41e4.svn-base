using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 项目管理Api
    /// </summary>
    [RoutePrefix("api/Exp/Mgr")]
    public class ExpMgrController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.Mgr.MgrHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.Mgr.MgrHelper();

        /// <summary>
        /// 区域列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAreaList")]
        public APIRst GetAreaList()
        {
            return infoHelper.GetAreaList();
        }

        /// <summary>
        /// 单位列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetUnitList")]
        public APIRst GetUnitList()
        {
            return infoHelper.GetUnitList();
        }
        /// <summary>
        /// 项目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetProjectList")]
        public APIRst GetProjectList()
        {
            return infoHelper.GetProjectList();
        }
        /// <summary>
        /// 用电单元
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetCellList")]
        public APIRst GetCellList()
        {
            return infoHelper.GetCellList();
        }

        /// <summary>
        /// 更新区域列表 id=0 新增
        /// </summary>
        /// <param name="id">区域ID号</param>
        /// <param name="no">编码</param>
        /// <param name="name">名称</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("UpdateArea")]
        public APIRst UpdateArea(int id,string no,string name,string remark)
        {
            return infoHelper.UpdateArea(id, no,name, remark);
        }
        /// <summary>
        /// 更新单位 id=0 新增
        /// </summary>
        /// <param name="id">单位ID号</param>
        /// <param name="no">编码</param>
        /// <param name="name">名称</param>
        /// <param name="remark">备注</param>
        /// <param name="areaId">区域ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("UpdateUnit")]
        public APIRst UpdateUnit(int id, string no, string name, string remark,int areaId)
        {
            return infoHelper.UpdateUnit(id, no, name, remark, areaId);
        }
        /// <summary>
        /// 更新项目 id=0 新增
        /// </summary>
        /// <param name="id">项目ID号</param>
        /// <param name="no">编码</param>
        /// <param name="name">项目名称</param>
        /// <param name="addr">地址</param>
        /// <param name="mobile">电话号码</param>
        /// <param name="remark">备注</param>
        /// <param name="disabled">是否弃用</param>
        /// <param name="unitId">单位ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("UpdateProject")]
        public APIRst UpdateProject(int id, string no, string name, string addr,string mobile, string remark,int disabled, int unitId)
        {
            return infoHelper.UpdateProject(id, no, name,addr, mobile,remark, disabled, unitId);
        }
        /// <summary>
        /// 更新用电单元 id=0 新增
        /// </summary>
        /// <param name="id">用电单元ID号</param>
        /// <param name="no">编码</param>
        /// <param name="name">名称</param>
        /// <param name="addr">地址</param>
        /// <param name="mobile">电话号码</param>
        /// <param name="remark">备注</param>
        /// <param name="projectId">项目ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("UpdateCell")]
        public APIRst UpdateCell(int id, string no,string name, string addr, string mobile, string remark, int projectId)
        {
            return infoHelper.UpdateCell(id, no, name, addr, mobile, remark, projectId);
        }

        /// <summary>
        /// 删除区域、单位、项目、用电单元
        /// </summary>
        /// <param name="id">ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("DelCoInfo")]
        public APIRst DelCoInfo(int id)
        {
            return infoHelper.DelCoInfo(id);
        }

        /// <summary>
        /// 设备信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMdList")]
        public APIRst GetMdList()
        {
            return infoHelper.GetMdList();
        }

        /// <summary>
        /// 设备采集项信息
        /// </summary>
        /// <param name="id">设备ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMapList")]
        public APIRst GetMapList(int id)
        {
            return infoHelper.GetMapList(id);
        }

        /// <summary>
        /// 保存设备信息
        /// </summary>
        /// <param name="id">设备ID号</param>
        /// <param name="name">设备名称</param>
        /// <param name="cellId">用电单元id号</param>
        /// <param name="moduleId">设备型号id号</param>
        /// <param name="disabled">设备状态=0正常=1弃用</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SaveMdInfo")]
        public APIRst SaveMdInfo(int id,string name,int cellId,int moduleId,int disabled)
        {
            return infoHelper.SaveMdInfo(id, name, cellId, moduleId, disabled);
        }

        /// <summary>
        /// 更新设备位置信息
        /// </summary>
        /// <param name="id">设备ID号</param>
        /// <param name="inst_loc">设备安装地址</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("UpdateInst_loc")]
        public APIRst UpdateInst_loc(int id,string inst_loc)
        {
            return infoHelper.UpdateInst_loc(id, inst_loc);
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("DelMdInfo")]
        public APIRst DelMdInfo(int id)
        {
            return infoHelper.DelMdInfo(id);
        }
        /// <summary>
        /// 保存设备信息
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="fun_id">采集项id号</param>
        /// <param name="tagName">采集项映射变量</param>
        /// <param name="dataValue">采集项映射值</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SaveMapInfo")]
        public APIRst SaveMapInfo(int module_id, int fun_id, string tagName = "", string dataValue = "")
        {
            return infoHelper.SaveMapInfo(module_id, fun_id, tagName, dataValue);
        }

        /// <summary>
        /// 一键清零
        /// </summary>
        /// <param name="module_id">回路ID号 =0 全部</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SetClear")]
        public APIRst SetClear(int module_id = 0)
        {
            return infoHelper.SetClear(module_id);
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="id">用电单元ID号</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("UpdateImg")]
        public APIRst UpdateImg(int id)
        {
            return infoHelper.UpdateImg(id);
        }

        /// <summary>
        /// 从ioserver导入变量
        /// </summary>
        /// <param name="module_id">设备id</param>
        /// <param name="busname">总线名称</param>
        /// <param name="devname">设备名称</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("ImportTagsFromIOServer")]
        public APIRst ImportTagsFromIOServer(int module_id, string busname, string devname)
        {
            return infoHelper.ImportTagsFromIOServer(module_id, busname, devname);
        }
    }
}
