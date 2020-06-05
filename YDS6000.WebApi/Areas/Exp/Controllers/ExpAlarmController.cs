using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 告警日志Api
    /// </summary>
    [RoutePrefix("api/Exp/Alarm")]
    public class ExpAlarmController : ApiController
    {
        private AlarmHelper infoHelper = new AlarmHelper();

        /// <summary>
        /// 通信/设备告警—获取通信/设备告警列表
        /// </summary>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <param name="aType">告警类型(Err_Tx:通信告警，Err_Sb:设备告警)</param>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdAlarmOfGwList")]
        public APIRst GetYdAlarmOfGwList(string aType,string startTime,string endTime,string strcName="",string coName="")
        {
            return infoHelper.GetYdAlarmOfGwList(strcName, coName, aType, startTime, endTime);
        }
        /// <summary>
        /// 充值告警—获取充值告警列表
        /// </summary>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <param name="isPay">缴费方式(0:现金，1:微信)</param>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdAlarmOfChargeList")]
        public APIRst GetYdAlarmOfChargeList(string isPay,string startTime,string endTime,string strcName="",string coName="")
        {
            return infoHelper.GetYdAlarmOfChargeList(strcName, coName, isPay, startTime, endTime);
        }
        /// <summary>
        /// 充值告警—检测支付情况
        /// </summary>
        /// <param name="pTrade_no">支付商品号</param>
        /// <param name="pPayType">支付类型</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdAlarmOfChargeChkPay")]
        public APIRst GetYdAlarmOfChargeChkPay(string pTrade_no,string pPayType)
        {
            return infoHelper.GetYdAlarmOfChargeChkPay(pTrade_no, pPayType);
        }
        /// <summary>
        /// 实时控制告警—获取实时控制告警列表
        /// </summary>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdAlarmOfCmdList")]
        public APIRst GetYdAlarmOfCmdList(string startTime,string endTime,string strcName="",string coName="")
        {
            return infoHelper.GetYdAlarmOfCmdList(strcName, coName, startTime, endTime);
        }
        /// <summary>
        /// 实时控制告警—远程控制
        /// </summary>
        /// <param name="log_id">控制记录id号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdAlarmOfCmd_Ctrl")]
        public APIRst GetYdAlarmOfCmd_Ctrl(int log_id)
        {
            return infoHelper.GetYdAlarmOfCmd_Ctrl(log_id);
        }
        /// <summary>
        /// 电量使用告警—判断电量使用告警列表是否显示电表名称和电表地址
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdAlarmOfChrgType")]
        public APIRst GetYdAlarmOfChrgType()
        {
            return infoHelper.GetYdAlarmOfChrgType();
        }
        /// <summary>
        /// 电量使用告警—获取电量使用告警列表
        /// </summary>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdAlarmOfValList")]
        public APIRst GetYdAlarmOfValList(string strcName="",string coName="")
        {
            return infoHelper.GetYdAlarmOfValList(strcName, coName);
        }
        /// <summary>
        /// 电量使用告警—汇出Excel
        /// </summary>
        /// <param name="data">json数据</param>
        /// <param name="show">是否显示电表(1:显示,0:不显示)</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdAlarmOfValOnExport")]
        public APIRst GetYdAlarmOfValOnExport(string data,int show)
        {
            return infoHelper.GetYdAlarmOfValOnExport(data, show);
        }
        /// <summary>
        /// 采集异常告警—获取采集异常告警列表
        /// </summary>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdAlarmOfUnusualList")]
        public APIRst GetYdAlarmOfUnusualList(string startTime,string endTime,string strcName="",string coName="")
        {
            return infoHelper.GetYdAlarmOfUnusualList(strcName, coName, startTime, endTime);
        }
        /// <summary>
        /// 采集异常告警—保存修改错误数据
        /// </summary>
        /// <param name="RowId">序号</param>
        /// <param name="Co_id">房间ID号</param>
        /// <param name="Log_id">日志ID号</param>
        /// <param name="Module_id">电表ID号</param>
        /// <param name="Fun_id">功能ID号</param>
        /// <param name="ModuleAddr">电表地址</param>
        /// <param name="LastValOld">最后通信电量值</param>
        /// <param name="LastVal">上次采集表码值</param>
        /// <param name="FirstVal">开始电量值</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SaveVal")]
        public APIRst SaveVal(int RowId,int Co_id,int Log_id,int Module_id,int Fun_id,string ModuleAddr,decimal LastValOld,decimal LastVal,decimal FirstVal)
        {
            return infoHelper.SaveVal(RowId, Co_id, Log_id, Module_id, Fun_id, ModuleAddr, LastValOld, LastVal, FirstVal);
        }
        /// <summary>
        /// 采集异常告警—保存修改错误时间
        /// </summary>
        /// <param name="RowId">序号</param>
        /// <param name="Co_id">房间ID号</param>
        /// <param name="Log_id">日志ID号</param>
        /// <param name="Module_id">电表ID号</param>
        /// <param name="Fun_id">功能ID号</param>
        /// <param name="ModuleAddr">电表地址</param>
        /// <param name="LastTime">最后通信时间</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SaveTime")]
        public APIRst SaveTime(int RowId,int Co_id,int Log_id,int Module_id,int Fun_id,string ModuleAddr,string LastTime)
        {
            return infoHelper.SaveTime(RowId, Co_id, Log_id, Module_id, Fun_id, ModuleAddr, LastTime);
        }
        /// <summary>
        /// 恶性负载告警—获取恶性负载告警列表
        /// </summary>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdAlarmOfLoadList")]
        public APIRst GetYdAlarmOfLoadList(string startTime,string endTime,string strcName="",string coName="")
        {
            return infoHelper.GetYdAlarmOfLoadList(strcName, coName, startTime, endTime);
        }
        /// <summary>
        /// 电气火灾告警—获取电气火灾告警列表
        /// </summary>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="aType">告警类型</param>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdAlarmOfEnvirList")]
        public APIRst GetYdAlarmOfEnvirList(string startTime,string endTime,string aType,string strcName="",string coName="")
        {
            return infoHelper.GetYdAlarmOfEnvirList(strcName, coName, aType, startTime, endTime);
        }

        /// <summary>
        /// 获取首页欠费户数
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetHomeUserAlarm")]
        public APIRst GetHomeUserAlarm()
        {
            return infoHelper.GetHomeUserAlarm();
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
        public APIRst GetYdAlarmList(string startTime, string endTime,int status = 0, string strcName = "", string coName = "")
        {
            return infoHelper.GetYdAlarmList(startTime, endTime, status, strcName, coName);
        }
        /// <summary>
        /// 忽略此隐患
        /// </summary>
        /// <param name="log_id">隐患编号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("IgnoreAlarm")]
        public APIRst IgnoreAlarm(long log_id)
        {
            return infoHelper.IgnoreAlarm(log_id);
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="uid">=0 全部</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdUserList")]
        public APIRst GetYdUserList(int uid = 0)
        {
            return infoHelper.GetYdUserList(uid);
        }

        /// <summary>
        /// 派工单
        /// </summary>
        /// <param name="log_id">隐患编号</param>
        /// <param name="uid">指派人员ID</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("JobOrder")]
        public APIRst JobOrder(long log_id, int uid)
        {
            return infoHelper.JobOrder(log_id,uid);
        }

        /// <summary>
        /// 工单列表
        /// </summary>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="status">操作状态(可选)</param>
        /// <param name="strcName">建筑名称(可选)</param>
        /// <param name="coName">房间名称(可选)</param>
        /// <returns></returns>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetSolutionList")]
        public APIRst GetSolutionList(string startTime, string endTime, int status = 0, string strcName = "", string coName = "")
        {
            return infoHelper.GetSolutionList(startTime, endTime, status, strcName, coName);
        }

        /// <summary>
        /// 工单状态修改
        /// </summary>
        /// <param name="log_id">工单编号</param>
        /// <param name="status">工单状态</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SolutionStatus")]
        public APIRst SolutionStatus(long log_id, int status)
        {
            return infoHelper.SolutionStatus(log_id, status);
        }
        /// <summary>
        /// 安全评分列表
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetIdxList")]
        public APIRst GetIdxList(DateTime startTime)
        {
            return infoHelper.GetIdxList(startTime);
        }
        /// <summary>
        /// 安全评分
        /// </summary>
        /// <param name="id">用电单元ID号</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="idx">评分</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("UpdateIdx")]
        public APIRst UpdateIdx(int id,DateTime startTime,int idx)
        {
            return infoHelper.UpdateIdx(id, startTime, idx);
        }
        #endregion
    }
}
