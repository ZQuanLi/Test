using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using YDS6000.Models;

namespace DataProcess
{
    internal class NCSys
    {
        internal static Dictionary<int, SysPro> Pro = new Dictionary<int, SysPro>(); /// 各系统参数

        internal static ConcurrentDictionary<string, CollectVModel> Result = new ConcurrentDictionary<string, CollectVModel>(); /// 内容值
        internal static ConcurrentQueue<CmdResult> UIResult = new ConcurrentQueue<CmdResult>(); //采集结果集
        //internal static ConcurrentQueue<AlarmCollect> AlarmCollect = new ConcurrentQueue<AlarmCollect>(); //采集告警集
        internal static ConcurrentQueue<v2_alarm_logVModel> AlarmCollect = new ConcurrentQueue<v2_alarm_logVModel>(); //采集告警集
        
        //internal static ConcurrentQueue<AlarmResult> AlarmResult = new ConcurrentQueue<AlarmResult>(); //采集告警值

        /// <summary>
        /// 重设配置信息
        /// </summary>
        internal static bool Reset = true;
        /// <summary>
        /// 后台运行记录数
        /// </summary>
        internal static int BackgroundCount = 0;
        /// <summary>
        /// 是否继续运行
        /// </summary>
        internal static bool IsRun = true;
        /// <summary>
        /// 重设定时拉合闸数据
        /// </summary>
        internal static bool ResetSiTime = false;

        //internal static WxConfig WxConfig = new WxConfig();        
        /// <summary>
        /// 运行总数
        /// </summary>
        internal static long TotalCount = 0;
        /// <summary>
        /// 错误数
        /// </summary>
        internal static long ErrCount = 0;

        /// <summary>
        /// 是否输出页面
        /// </summary>
        internal static bool IsUIResult = true;

    }
}
