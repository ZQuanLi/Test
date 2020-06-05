using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Alarm
{
    partial class AlarmBLL
    {
        /// <summary>
        /// 采集异常告警
        /// </summary>
        /// <param name="CoStrcName"></param>
        /// <param name="CoName"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        public DataTable GetYdAlarmOfUnusualList(string CoStrcName, string CoName, DateTime Start, DateTime End)
        {
            DataTable dtSource = dal.GetYdAlarmOfUnusualList(CoStrcName, CoName, Start, End);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["RowId"] = ++RowId;
            }
            return dtSource;
        }
        /// <summary>
        /// 保存修改错误数据
        /// </summary>
        /// <returns></returns>
        public int SetV2_infoInVal(int Log_id, int Co_id, int Module_id, string ModuleAddr, int Fun_id, decimal FirstVal, decimal LastVal)
        {
            return dal.SetV2_infoInVal(Log_id, Co_id, Module_id, ModuleAddr, Fun_id, FirstVal, LastVal);
        }
        public DataTable GetTabVal(int Co_id, string ModuleAddr)
        {
            return dal.GetTabVal(Co_id, ModuleAddr);
        }
        /// <summary>
        /// 保存修改错误时间
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="ModuleAddr"></param>
        /// <param name="LastTime"></param>
        /// <returns></returns>
        public int SetV2_infoInTime(int Log_id, int Co_id, int Module_id, string ModuleAddr, int Fun_id, DateTime LastTime)
        {
            return dal.SetV2_infoInTime(Log_id, Co_id, Module_id, ModuleAddr, Fun_id, LastTime);
        }

        /// <summary>
        /// 隐患列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdAlarmList(string CoStrcName, string CoName, int status, DateTime Start, DateTime End)
        {
            return dal.GetYdAlarmList(CoStrcName, CoName, status, Start, End);
        }
        /// <summary>
        /// 忽略此隐患
        /// </summary>
        /// <param name="log_id">隐患编号</param>
        /// <returns></returns>
        public int IgnoreAlarm(long log_id)
        {
            return dal.IgnoreAlarm(log_id);
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdUserList(int uid)
        {
            return dal.GetYdUserList(uid);
        }

        /// <summary>
        /// 派工单
        /// </summary>
        /// <param name="log_id">隐患编号</param>
        /// <param name="uid">指派人员ID</param>
        /// <returns></returns>
        public int JobOrder(long log_id, int uid)
        {
            return dal.JobOrder(log_id, uid);
        }

        /// <summary>
        /// 工单列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetSolutionList(string CoStrcName, string CoName, int status, DateTime Start, DateTime End)
        {
            return dal.GetSolutionList(CoStrcName, CoName, status, Start, End);
        }
        /// <summary>
        /// 工单状态修改
        /// </summary>
        /// <param name="log_id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int SolutionStatus(long log_id, int status)
        {
            return dal.SolutionStatus(log_id, status);
        }

        public DataTable GetIdxList(DateTime startTime)
        {
            return dal.GetIdxList(startTime);
        }

        public int UpdateIdx(int co_id, DateTime startTime, int idx)
        {
            return dal.UpdateIdx(co_id, startTime, idx);
        }
        //public DataTable GetSysLog()
        //{
        //    return dal.GetSysLog();
        //}
    }
}
