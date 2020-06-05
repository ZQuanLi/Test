using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.ExpApp.Controllers
{
    /// <summary>
    /// 手机APP
    /// </summary>
    [RoutePrefix("api/ExpApp/Home")]
    public class ExpAppHomeController : ApiController
    {
        private YDS6000.WebApi.Areas.ExpApp.Opertion.Home.HomeHelper infoHelper = new YDS6000.WebApi.Areas.ExpApp.Opertion.Home.HomeHelper();

        /// <summary>
        /// 登录用户
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [SecurityCtrl(false, false)]
        [Route("Login")]
        public APIRst Login(HomeLogin obj)
        {
            return infoHelper.Login(CommFunc.ConvertDBNullToString(obj.name), CommFunc.ConvertDBNullToString(obj.pwd));
        }
        /// <summary>
        /// 获取用户编号信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetUserNumber")]
        public APIRst GetUserNumber()
        {
            return infoHelper.GetUserNumber();
        }
        /// <summary>
        /// 获取用户编号信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetUserInfo")]
        public APIRst GetUserInfo()
        {
            return infoHelper.GetUserInfo();
        }
        /// <summary>
        /// 获取快速缴费信息
        /// </summary>
        /// <param code="用户编号"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetPayQuick")]
        public APIRst GetPayQuick(string code)
        {
            return infoHelper.GetPayQuick(code);
        }
        /// <summary>
        /// 获取系统操作日志
        /// </summary>
        /// <param name="UName">操作人</param>
        /// <param name="StartTime">日期开始</param>
        /// <param name="EndTime">日期结束</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdSysLogOfCmd")]
        public APIRst GetYdSysLogOfCmd(string UName, DateTime StartTime, DateTime EndTime)
        {
            return infoHelper.GetYdSysLogOfCmd(UName, StartTime, EndTime);
        }

        /// <summary>
        /// 获取系统控制日志
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdSysLogCtrl")]
        public APIRst GetYdSysLogCtrl(string UName, DateTime StartTime, DateTime EndTime)
        {
            return infoHelper.GetYdSysLogCtrl(UName, StartTime, EndTime);
        }
        /// <summary>
        /// 日志查询
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="funType">采集项目</param>
        /// <param name="dataType">查询类型</param>
        /// <param name="startTime">开始日期</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetHisData")]
        public APIRst GetHisData(int module_id, string funType, string dataType, DateTime startTime)
        {
            return infoHelper.GetHisData(module_id, funType, dataType, startTime);
        }
        /// <summary>
        /// 数据对比分析
        /// </summary>
        /// <param name="areaId">区域ID号</param>
        /// <param name="unitId">单元ID号</param>
        /// <param name="projectId">项目ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetCompare")]
        public APIRst GetCompare(int areaId, int unitId, int projectId)
        {
            return infoHelper.GetCompare(areaId, unitId, projectId);
        }
        /// <summary>
        /// 远程控制参数
        /// </summary>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <param name="ssr">拉合闸状态(可选，默认为空)</param>
        /// <param name="co_id">建筑节点</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdMonitorOnList")]
        public APIRst GetYdMonitorOnList(string strcName = "", string coName = "", string ssr = "", int co_id = 0)
        {
            return infoHelper.GetYdMonitorOnList(strcName, coName, ssr, co_id);
        }
        /// <summary>
        /// 下发控制
        /// </summary>
        /// <param name="pStr">设备ID</param>
        /// <param name="fun_id">设备类型ID</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SendCtrl")]
        public APIRst SendCtrl(string pStr, int fun_id)
        {
            return infoHelper.SendCtrl(pStr, fun_id);
        }
        /// <summary>
        /// 区域下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAearCombox")]
        public APIRst GetAearCombox()
        {
            return infoHelper.GetAearCombox();
        }
        /// <summary>
        /// 单位下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetUnitCombox")]
        public APIRst GetUnitCombox()
        {
            return infoHelper.GetUnitCombox();
        }
        /// <summary>
        /// 项目下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetProjectCombox")]
        public APIRst GetProjectCombox()
        {
            return infoHelper.GetProjectCombox();
        }

        /// <summary>
        /// 用电单元下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetCellCombox")]
        public APIRst GetCellCombox()
        {
            return infoHelper.GetCellCombox();
        }
        /// <summary>
        /// 回路下拉列表
        /// </summary>
        /// <param name="id">用电单元ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetModuleCombox")]
        public APIRst GetModuleCombox(int id)
        {
            return infoHelper.GetModuleCombox(id);
        }
        /// <summary>
        /// 采集项下拉列表
        /// </summary>
        /// <param name="id">回路ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetFunCombox")]
        public APIRst GetFunCombox(int id)
        {
            return infoHelper.GetFunCombox(id);
        }
        /// <summary>
        /// 查询类型下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetDataTypeCombox")]
        public APIRst GetDataTypeCombox()
        {
            return infoHelper.GetDataTypeCombox();
        }
        /// <summary>
        /// 控制列表
        /// </summary>
        /// <param name="module_id">设备类型ID</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetCtrlList")]
        public APIRst GetCtrlList(int module_id)
        {
            return infoHelper.GetCtrlList(module_id);
        }
        #region 隐患管理
        /// <summary>
        /// 隐患列表
        /// </summary>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="status">操作状态(可选)</param>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdAlarmList")]
        public APIRst GetYdAlarmList(string startTime, string endTime, int status = 0, string strcName = "", string coName = "")
        {
            return infoHelper.GetYdAlarmList(startTime, endTime, status, strcName, coName);
        }
        /// <summary>
        /// 工单列表
        /// </summary>
        /// <param name="status">操作状态(可选)</param>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetSolutionList")]
        public APIRst GetSolutionList( int status = 0, string strcName = "", string coName = "")
        {
            return infoHelper.GetSolutionList(status, strcName, coName);
        }
        /// <summary>
        /// 修改工单
        /// </summary>
        /// <param name="logId">工单编号</param>
        /// <param name="attached">图片地址</param>
        /// <param name="content">工单报告内容</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SolutionUpdate")]
        public APIRst SolutionUpdate(int logId = 0, string attached = "", string content="")
        {
            return infoHelper.SolutionUpdate(logId, attached,content);
        }
        /// <summary>
        /// 上传工单图片
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetSolutionImage")]
        public APIRst SetSolutionImage()
        {
            return infoHelper.SetSolutionImage();
        }

        /// <summary>
        /// 获取用户用电单元权限
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAreaPowerAPP")]
        public APIRst GetAreaPowerAPP()
        {
            return infoHelper.GetAreaPowerAPP();
        }
        #endregion
    }
}
