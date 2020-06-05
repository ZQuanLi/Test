using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 系统监视Api
    /// </summary>
    [RoutePrefix("api/Exp/Monitor")]
    public class ExpMonitorController : ApiController
    {
        private MonitorHelper infoHelper = new MonitorHelper();


        /// <summary>
        /// 获取建筑列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdCustOnCoInfoList")]
        public APIRst GetYdCustOnCoInfoList()
        {
            return infoHelper.GetYdCustOnCoInfoList();
        }

        #region 抄表数据监视
        /// <summary>
        /// 抄表数据监视—获取抄表数据监视列表
        /// </summary>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <param name="ssr">拉合闸状态(可选，默认为空)</param>
        /// <param name="co_id">建筑节点</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdMonitorOnList")]
        public APIRst GetYdMonitorOnList(string strcName="",string coName="",string ssr="",int co_id =0)
        {
            return infoHelper.GetYdMonitorOnList(strcName, coName, ssr, co_id);
        }
        /// <summary>
        /// 抄表数据监视—获取列表的的详细信息
        /// </summary>
        /// <param name="co_id">房间id</param>
        /// <param name="module_id">回路id</param>
        /// <param name="moduleAddr">回路地址</param> 
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdMonitorOnInfo")]
        public APIRst GetYdMonitorOnInfo(int co_id, int module_id, string moduleAddr)
        {
            return infoHelper.GetYdMonitorOnInfo(co_id, module_id, moduleAddr);
        }

        /// <summary>
        /// 抄表数据监视—详情—获取图形数据
        /// </summary>
        /// <param name="co_id">房间id</param>
        /// <param name="module_id">回路id</param>
        /// <param name="moduleAddr">回路地址</param>
        /// <param name="dateType">分析类型(hour:日分析,day:月分析,month:年分析)</param>
        /// <param name="startTime">日期(2018-09-14:日分析,2018-09:月分析,2018:年分析)</param>
        [HttpGet, HttpOptions]
        [Route("GetYdMonitorInDetailOnCharts")]
        public APIRst GetYdMonitorInDetailOnCharts(int co_id,int module_id,string moduleAddr,string dateType,string startTime)
        {
            return infoHelper.GetYdMonitorInDetailOnCharts(co_id, module_id, moduleAddr, dateType, startTime);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //[HttpGet, HttpOptions]
        //[Route("GetYdMonitorInDetailOnBill")]
        //public APIRst GetYdMonitorInDetailOnBill(int co_id,int module_id,string moduleAddr,DateTime start,DateTime end)
        //{
        //    return infoHelper.GetYdMonitorInDetailOnBill(co_id, module_id, moduleAddr, start, end);
        //}

        ///// <summary>
        ///// 抄表数据监视—获取温湿度
        ///// </summary>
        ///// <param name="strcName">建筑名称</param>
        ///// <param name="coName">房间名称</param>
        //[HttpGet, HttpOptions]
        //[Route("GetTemperatureAndHumidity")]
        //public APIRst GetTemperatureAndHumidity(string strcName,string coName)
        //{
        //    return infoHelper.GetTemperatureAndHumidity(strcName, coName);
        //}

        /// <summary>
        /// 抄表数据监视—汇出excel
        /// </summary>
        /// <param name="data">表格json字符串</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("GetYdMonitorOnExport")]
        public APIRst GetYdMonitorOnExport([FromBody]string data)
        {
            return infoHelper.GetYdMonitorOnExport(data);
        }

        /// <summary>
        /// 控制列表
        /// </summary>
        /// <param name="module_id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetCtrlList")]
        public APIRst GetCtrlList(int module_id)
        {
            return infoHelper.GetCtrlList(module_id);
        }
        /// <summary>
        /// 下发控制
        /// </summary>
        /// <param name="pStr"></param>
        /// <param name="fun_id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SendCtrl")]
        public APIRst SendCtrl(string pStr, int fun_id)
        {
            return infoHelper.SendCtrl(pStr, fun_id);
        }
        ///// <summary>
        ///// 发送指令
        ///// </summary>
        ///// <param name="cmd"></param>
        ///// <param name="module_id"></param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetYdMonitorOfControl")]
        //public APIRst GetYdMonitorOfControl(string cmd,int module_id)
        //{
        //    return infoHelper.GetYdMonitorOfControl(cmd, module_id);
        //}
        /// <summary>
        /// 抄表数据监视—设置控制参数—保存自动控制设置
        /// </summary>
        /// <param name="module_id">电表id号</param>
        /// <param name="isAlarm">是否自动控制(是=1,否=0)</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SaveYdMonitorOfControl")]
        public APIRst SaveYdMonitorOfControl(int module_id,int isAlarm)
        {
            return infoHelper.SaveYdMonitorOfControl(module_id, isAlarm);
        }

        /// <summary>
        /// 抄表数据监视—获取批量控制列表
        /// </summary>
        /// <param name="module_id">电表id号(可选)</param>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdBatchControlOfList")]
        public APIRst GetYdBatchControlOfList(int module_id=0,string strcName="",string coName="")
        {
            return infoHelper.GetYdBatchControlOfList(module_id, strcName, coName);
        }
        /// <summary>
        /// 抄表数据监视—批量远程控制(合闸/拉闸/保电/非保电)
        /// </summary>
        /// <param name="v0Fun">功能类型(Ssr:闸，IsPaul:电)</param>
        /// <param name="dataValue">设置值(0:合闸/非保电，1:拉闸/保电)</param>
        /// <param name="pStr">电表id号，多个id号以逗号隔开</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("YdBatchControlOfSsr")]
        public APIRst YdBatchControlOfSsr(string v0Fun,string dataValue,string pStr)
        {
            return infoHelper.YdBatchControlOfSsr(v0Fun, dataValue, pStr);
        }
        /// <summary>
        /// 抄表数据监视—设置控制参数—获取继电器状态
        /// </summary>
        /// <param name="module_id">电表id号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("YdMonitorOfGetIsRelay")]
        public APIRst YdMonitorOfGetIsRelay(int module_id)
        {
            return infoHelper.YdMonitorOfGetIsRelay(module_id);
        }
        /// <summary>
        /// 抄表数据监视—设置控制参数—下发继电器状态
        /// </summary>
        /// <param name="v0Fun">IsRelay</param>
        /// <param name="isRelayVal">继电器类型值</param>
        /// <param name="module_id">电表id号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("YdMonitorOfIsRelay")]
        public APIRst YdMonitorOfIsRelay(string v0Fun,string isRelayVal,int module_id)
        {
            return infoHelper.YdMonitorOfIsRelay(v0Fun, isRelayVal, module_id);
        }
        /// <summary>
        /// 抄表数据监视—设置控制参数—获取定时断送电策略列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdMonitorOfSsr")]
        public APIRst GetYdMonitorOfSsr()
        {
            return infoHelper.GetYdMonitorOfSsr();
        }
        /// <summary>
        /// 抄表数据监视—设置控制参数—获取电表的定时断送电策略
        /// </summary>
        /// <param name="Module_id">电表id号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdMonitorOfM_Ssr")]
        public APIRst GetYdMonitorOfM_Ssr(int Module_id)
        {
            return infoHelper.GetYdMonitorOfM_Ssr(Module_id);
        }
        /// <summary>
        /// 抄表数据监视—设置控制参数—设置电表定时断送电策略
        /// </summary>
        /// <param name="Si_id">策略id号</param>
        /// <param name="module_id">电表id号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SetYdMonitorOfSsr")]
        public APIRst SetYdMonitorOfSsr(int Si_id,int module_id)
        {
            return infoHelper.SetYdMonitorOfSsr(Si_id, module_id);
        }
        /// <summary>
        /// 抄表数据监视—设置控制参数—获取短信类容
        /// </summary>
        /// <param name="co_id">房间id号</param>
        /// <param name="module_id">电表id号</param>
        /// <param name="moduleAddr">电表</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdSmsContent")]
        public APIRst GetYdSmsContent(int co_id, int module_id, string moduleAddr)
        {
            return infoHelper.GetYdSmsContent(co_id, module_id, moduleAddr);
        }
        /// <summary>
        /// 抄表数据监视—设置控制参数—发送短信
        /// </summary>
        /// <param name="mphone">手机号</param>
        /// <param name="crmName">用户名称</param>
        /// <param name="lastTime">截止日期</param>
        /// <param name="coName">房间名称</param>
        /// <param name="syAmt">剩余金额</param>
        /// <param name="syVal">剩余电量</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SendYdSmsContent")]
        public APIRst SendYdSmsContent(string mphone, string crmName, string lastTime, string coName, string syAmt, string syVal)
        {
            return infoHelper.SendYdSmsContent(mphone, crmName, lastTime, coName, syAmt, syVal);
        }
        /// <summary>
        /// 抄表数据监视—设置控制参数—发送邮件
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <param name="crmName">用户名称</param>
        /// <param name="lastTime">截止日期</param>
        /// <param name="coName">房间名称</param>
        /// <param name="syAmt">剩余金额</param>
        /// <param name="syVal">剩余电量</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SendYdMailContent")]
        public APIRst SendYdMailContent(string email, string crmName, DateTime lastTime, string coName, string syAmt, decimal syVal)
        {
            return infoHelper.SendYdMailContent(email, crmName, lastTime, coName, syAmt, syVal);
        }
        #endregion

        #region 设备参数监视
        /// <summary>
        /// 设备参数监视—获取设备参数监视列表
        /// </summary>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdParamsOfList")]
        public APIRst GetYdParamsOfList(string strcName="",string coName="")
        {
            return infoHelper.GetYdParamsOfList(strcName, coName);
        }
        /// <summary>
        /// 设备参数监视—获取参数项列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("YdParamsOnBatch_FunType")]
        public APIRst YdParamsOnBatch_FunType()
        {
            return infoHelper.YdParamsOnBatch_FunType();
        }
        /// <summary>
        /// 设备参数监视—设置参数值
        /// </summary>
        /// <param name="module_id">电表id号</param>
        /// <param name="fun_id">参数id号(可选)</param>
        /// <param name="funType">参数类型</param>
        /// <param name="dataValue">参数值</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SetYdParams")]
        public APIRst SetYdParams(int module_id,string funType,string dataValue,int fun_id=0)
        {
            return infoHelper.SetYdParams(module_id,fun_id,funType,dataValue);
        }
        #endregion

        #region 平面图

        /// <summary>
        /// 建筑下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetViewBuildList")]
        public APIRst GetViewBuildCombox()
        {
            return infoHelper.GetViewBuildCombox();
        }

        /// <summary>
        /// 用电单元下拉框
        /// </summary>
        /// <param name="id">建筑ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetViewCellCombox")]
        public APIRst GetViewCellCombox(int id)
        {
            return infoHelper.GetViewCellCombox(id);
        }
        /// <summary>
        /// 平面图监视列表
        /// </summary>
        /// <param name="id">用电单元ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetViewList")]
        public APIRst GetViewList(int id)
        {
            return infoHelper.GetViewList(id);
        }
        #endregion
    }
}
