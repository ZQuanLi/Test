using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using YDS6000.BLL.DataProcess;
using YDS6000.Models;

namespace DataProcess.YdProcess
{
    public class Helper
    {
        private static Helper help = null;
        //private static string RangeUse = ""; // 路灯使用电量告警
        //private static DateTime LoadTime = DateTime.Now;
        private static SyscontBLL sysBll = new SyscontBLL(Config.Systems);
        private static object wholeObj = new Object();
        //private static DataTable DtMap = new DataTable();
        private Helper()
        {
            Thread th1 = new Thread((ThreadStart)delegate { this.DbProcess(); });
            th1.Name = "DataProcess";
            th1.IsBackground = true;
            th1.Start();
            //
            Thread th2 = new Thread((ThreadStart)delegate { new DataProcess.Yd.Alarm.Collect().Run(); });
            th2.Name = "DataProcess.DbHand";
            th2.IsBackground = true;
            th2.Start();
            ////
            //Thread th3 = new Thread((ThreadStart)delegate { new Alarm().Run(); });
            //th3.Name = "Db.Alarm";
            //th3.IsBackground = true;
            //th3.Start();
            ////
            Thread th4 = new Thread((ThreadStart)delegate { new ZpSi().Run(); });
            th4.Name = "Db.ZpSi";
            th4.IsBackground = true;
            th4.Start();
            ////
            //Thread th5 = new Thread((ThreadStart)delegate { new YdDbMid().Run(); });
            //th5.Name = "Db.Mid";
            //th5.IsBackground = true;
            //th5.Start();
        }

        /// <summary>
        /// 开启单例模式
        /// </summary>
        /// <returns></returns>
        internal static Helper Start()
        {
            if (help == null)
                help = new Helper();
            return help;
        }

        // 重新更新配置
        private void GetProcessOfConfig()
        {
            if (NCSys.Reset == true)
            {
                NCSys.Pro.Clear();
                NCSys.Result.Clear();
                NCSys.Reset = false;
                NCSys.ResetSiTime = false;
                //addBaseValTime = null;
                //updatePayEstTime = null;

                //Helper.LoadTime = DateTime.Now;
                //Helper.RangeUse = CommFunc.ConvertDBNullToString(IniHepler.GetConfig(Config.lpFileName, "Alarm", "RangeUse"));

                #region 获取基本配置信息
                DataTable dtInfo = sysBll.GetSyscont();
                foreach (DataRow drInfo in dtInfo.Rows)
                {
                    SysPro pro = new SysPro();
                    int ledger = CommFunc.ConvertDBNullToInt32(drInfo["Ledger"]);
                    pro.ProjectKey = CommFunc.ConvertDBNullToString(drInfo["Project"]) + "->" + CommFunc.ConvertDBNullToInt32(drInfo["Ledger"]) + "->";
                    pro.Project = CommFunc.ConvertDBNullToString(drInfo["Project"]);
                    pro.FrMd = CommFunc.ConvertDBNullToInt32(drInfo["FrMd"]);
                    pro.MaxVal = CommFunc.ConvertDBNullToInt32(drInfo["MaxVal"]);
                    pro.Auto = CommFunc.ConvertDBNullToInt32(drInfo["Auto"]);
                    pro.FrMd = pro.FrMd == 0 ? 15 : pro.FrMd;
                    NCSys.Pro.Add(ledger, pro);
                    /////        
                    #region 获取各种参数配置信息
                    //DataProcessBLL bll = new DataProcessBLL(ledger, Config.Uid);
                    //DataTable dtConfig = bll.GetSysConfig("");
                    //AliSmsConfig sms = new AliSmsConfig();
                    //ComSmsConfig comConfig = new ComSmsConfig();
                    //WxConfig wx = new WxConfig();
                    //EmailConfig em = new EmailConfig();
                    //System.Reflection.PropertyInfo[] smsInfo = sms.GetType().GetProperties();
                    //System.Reflection.PropertyInfo[] comInfo = comConfig.GetType().GetProperties();
                    //System.Reflection.PropertyInfo[] wxInfo = wx.GetType().GetProperties();
                    //System.Reflection.PropertyInfo[] emInfo = em.GetType().GetProperties();
                    //foreach (DataRow dr in dtConfig.Rows)
                    //{
                    //    foreach (System.Reflection.PropertyInfo f_key in smsInfo)
                    //    {
                    //        if (CommFunc.ConvertDBNullToString(dr["CfKey"]).Equals(f_key.Name))
                    //            f_key.SetValue(sms, dr["CfValue"], null);
                    //    }
                    //    foreach (System.Reflection.PropertyInfo f_key in wxInfo)
                    //    {
                    //        if (CommFunc.ConvertDBNullToString(dr["CfKey"]).Equals(f_key.Name))
                    //            f_key.SetValue(wx, dr["CfValue"], null);
                    //    }
                    //    foreach (System.Reflection.PropertyInfo f_key in emInfo)
                    //    {
                    //        if (CommFunc.ConvertDBNullToString(dr["CfKey"]).Equals(f_key.Name))
                    //            f_key.SetValue(em, dr["CfValue"], null);
                    //    }
                    //    foreach (System.Reflection.PropertyInfo f_key in comInfo)
                    //    {
                    //        if (CommFunc.ConvertDBNullToString(dr["CfKey"]).Equals(f_key.Name))
                    //            f_key.SetValue(comConfig, dr["CfValue"], null);
                    //    }
                    //}
                    //SmsFun.AliSmsConfig = sms;
                    //pro.Wx.WxConfig = wx;
                    //pro.Sms.ComSmsConfig = comConfig;
                    //EmailUtilities.EmConfig = em;
                    //dtConfig.Dispose();
                    #endregion

                    #region 更新配置
                    //lock (wholeObj)
                    //{/*操作datatable线程非安全*/
                    //    DataTable dtTmp = bll.GetMapInfo("");
                    //    if (DtMap == null)
                    //        DtMap = dtTmp;
                    //    else
                    //        DtMap.Merge(dtTmp.Copy(), true, MissingSchemaAction.Ignore);
                    //    dtTmp.Dispose();
                    //}
                    #endregion
                }
                //Helper.FrMd = Helper.FrMd == 0 ? 15 : Helper.FrMd;
                //Helper.SendSpan = Helper.SendSpan == 0 ? 15 : Helper.SendSpan;
                #endregion

                #region 获取IOService变量
                System.Data.DataTable dtSource = sysBll.GetMapInfo();
                Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
                foreach (System.Data.DataRow dr in dtSource.Rows)
                {
                    int ledger = CommFunc.ConvertDBNullToInt32(dr["Ledger"]);
                    int action = CommFunc.ConvertDBNullToInt32(dr["Action"]);
                    string handledBY = CommFunc.ConvertDBNullToString(dr["HandledBY"]);
                    string nKey = CommFunc.ConvertDBNullToString(dr["NKey"]);
                    string tagName = CommFunc.ConvertDBNullToString(dr["TagName"]);
                    if (!string.IsNullOrEmpty(Config.IOHandledBY))
                        handledBY = Config.IOHandledBY;
                    List<string> tag = null;
                    if (dic.TryGetValue(handledBY, out tag) == false)
                    {
                        tag = new List<string>();
                        dic.Add(handledBY, tag);
                    }
                    if (action == 0 || action == 1)
                    {
                        tag.Add(tagName);
                    }
                    #region 更新内存
                    SysPro pro = null;
                    NCSys.Pro.TryGetValue(ledger, out pro);
                    if (pro == null)
                    {
                        FileLog.Error("KEY:" + nKey + "没有此账目: " + ledger);
                        continue;
                    }
                    CollectVModel collect = ModelHandler<CollectVModel>.FillModel(dr);
                    collect.CachedKey = pro.ProjectKey + collect.CachedKey;
                    NCSys.Result.TryAdd(pro.ProjectKey + nKey, collect);
                    #endregion
                }
                foreach (var kk in dic)
                {
                    DataProcess.IOService.Collection.CollectionHelper.Instance(kk.Key, kk.Value);
                }
                dtSource.Dispose();
                #endregion

                FileLog.WriteLog("更新配置成功");
            }
        }

        private void DbProcess()
        {
            while (true)
            {
                Thread.Sleep(1000);/*每一秒*/
                if (NCSys.IsRun == false) continue;

                #region 配置信息
                try
                {
                    this.GetProcessOfConfig();
                }
                catch (Exception ex)
                {
                    FileLog.Error("读取采集配置数据错误:" + ex.Message + ex.StackTrace);
                }
                #endregion

                #region 先入库存储
                try
                {
                    bool bIsNext = true;
                    var enumer = NCSys.Result.GetEnumerator();
                    while (bIsNext == true)
                    {
                        bIsNext = enumer.MoveNext();
                        if (bIsNext == false) break;
                        var s0 = enumer.Current;
                        CollectVModel s1 = s0.Value;
                        SysPro pro = null;
                        NCSys.Pro.TryGetValue(s1.Ledger, out pro);
                        if (pro == null)
                        {
                            FileLog.Error("KEY:" + s0.Key + "没有此账目: " + s1.Ledger);
                            continue;
                        }
                        RstVar rst = MemcachedMgr.GetVal<RstVar>(s1.CachedKey);
                        if (rst == null)
                        {
                            //FileLog.Error("KEY:" + s0.Key + "没有缓存值");
                            continue;
                        }
                        if (s1.Module_id == 0 || s1.Action != 0)
                            continue;
                        if (s1.IsDb == false)
                            if (s1.UpTime.AddMinutes(pro.FrMd) > rst.lpszdateTime) continue;
                        try
                        {
                            string msg;
                            DataProcessBLL bll = new DataProcessBLL(s1.Ledger, Config.Uid);
                            int errcode = bll.DataProcess(s1.Module_id, s1.ModuleAddr, s1.Fun_id, rst.lpszVal, rst.lpszdateTime, out msg, 90);
                            if (errcode < 0)
                            {
                                FileLog.Error("KEY:" + s0.Key + "值:" + rst.lpszVal + "数据处理结果错误:" + msg);
                            }
                            else
                            {
                                FileLog.Debug("KEY:" + s0.Key + "值:" + rst.lpszVal + "保存成功");
                            }
                        }
                        catch (Exception ex)
                        {
                            FileLog.Error("KEY:" + s0.Key + "值:" + rst.lpszVal + "采集时间:" + rst.lpszdateTime.ToString() + "数据处理结果错误:" + ex.Message);
                        }
                        s1.UpTime = DateTime.Now;/*入库时间*/
                        s1.IsDb = false;
                        Thread.Sleep(20);/*每20毫秒*/
                    }
                }
                catch (Exception ex)
                {
                    FileLog.Error("数据处理错误:" + ex.Message + ex.StackTrace);
                }
                #endregion
            }
        }



        /// <summary>
        /// 保存采集项数据
        /// </summary>
        /// <param name="lpszDbVarName"></param>
        /// <param name="lpszVal"></param>
        /// <param name="lpszdateTime"></param>

        internal static void IOServiceSaveCollectCache(string lpszDbVarName, string lpszVal, DateTime lpszdateTime, UInt32 dwUserData, bool rst, AlarmType errCode, string errTxt)
        {
            foreach (var s1 in NCSys.Pro)
            {
                SaveCollectCache(s1.Key, lpszDbVarName, lpszVal, lpszdateTime, dwUserData);
                string mkey = s1.Value.ProjectKey + lpszDbVarName;
                CollectVModel collect = null;
                if (NCSys.Result.TryGetValue(mkey, out collect) == false) continue;
                if (collect.Module_id == 0) continue;
                AddUIResult(s1.Key, collect.Module_id, collect.ModuleAddr, "IOService", "IOService", collect.FunType, collect.FunName, lpszDbVarName, lpszVal, lpszdateTime, rst, null, null, errCode, "");
            }
        }

        /// <summary>
        /// 获取IOService的事件数据
        /// </summary>
        /// <param name="lpszDbVarName"></param>
        /// <param name="lpszVal"></param>
        /// <param name="lpszdateTime"></param>
        /// <param name="errTxt"></param>
        internal static void IOServiceEventCollect(DataProcess.Rdc.Package.RdcMethod.IOEvent result)
        {
            foreach (var s1 in NCSys.Pro)
            {
                string mkey = s1.Value.ProjectKey + result.lpszDbVarName;
                CollectVModel collect = null;
                if (NCSys.Result.TryGetValue(mkey, out collect) == false) continue;
                if (collect.Module_id == 0) continue;/*没有映射关系不存储告警*/
                v2_alarm_logVModel alarm = new v2_alarm_logVModel();
                alarm.Ledger = collect.Ledger;
                alarm.Module_id = collect.Module_id;
                alarm.ModuleAddr = collect.ModuleAddr;
                alarm.Fun_id = collect.Fun_id;
                alarm.FunType = collect.FunType;
                alarm.FunName = result.lpszType;
                alarm.CollectTime = result.lpszdateTime;
                alarm.CollectValue = result.lpszVal;
                alarm.Content = result.content;
                alarm.ErrCode = 1;
                alarm.ErrTxt = "";
                alarm.IsMqtt = true;
                NCSys.AlarmCollect.Enqueue(alarm); /*更新数据库*/
            }
        }

        /// <summary>
        /// 获取IOService的事件数据
        /// </summary>
        /// <param name="lpszDbVarName"></param>
        /// <param name="lpszVal"></param>
        /// <param name="lpszdateTime"></param>
        /// <param name="errTxt"></param>
        //internal static void IOServiceEventCollect(string lpszDbVarName, string lpszVal, DateTime lpszdateTime, string errTxt)
        //{
        //    foreach (var s1 in NCSys.Pro)
        //    {
        //        string mkey = s1.Value.ProjectKey + lpszDbVarName;
        //        CollectVModel collect = null;
        //        if (NCSys.Result.TryGetValue(mkey, out collect) == false) continue;
        //        if (collect.Module_id == 0) continue;/*没有映射关系不存储告警*/
        //        CommandVModel cmd = new CommandVModel() { Ledger = s1.Key, Module_id = collect.Module_id, ModuleAddr = collect.ModuleAddr, Fun_id = collect.Fun_id, FunType = collect.FunType, FunName = collect.FunName, LpszDbVarName = lpszDbVarName };
        //        AlarmCollect collectInfo = new AlarmCollect() { cmd = cmd, lpszVal = lpszVal, lpszdateTime = lpszdateTime, errCode = AlarmType.Err_OFlow, errTxt = errTxt };
        //        NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
        //    }
        //}
        internal static void IOServiceWriteFail(CommandVModel cmd, DateTime lpszdateTime, UInt32 dwUserData, bool rst, AlarmType errCode, string errTxt)
        {/*读写存在*/
            SysPro pro;
            NCSys.Pro.TryGetValue(cmd.Ledger, out pro);
            if (pro == null)
            {
                FileLog.Error("IOServiceWriteFail 没有此项目ID号:" + cmd.Ledger);
                return;
            }
            //string mkey = pro.ProjectKey + cmd.LpszDbVarName;
            //CollectVModel collect = null;
            //if (NCSys.Result.TryGetValue(mkey, out collect) == false)
            //{
            //    collect = new CollectVModel() { Ledger = cmd.Ledger };
            //    FileLog.Error("IOServiceWriteFail 没有采集码:" + mkey);
            //}
            //else
            //{
            //CommandVModel cmd = new CommandVModel() { Ledger = ledger, Module_id = collect.Module_id, ModuleAddr = collect.ModuleAddr, Fun_id = collect.Fun_id, FunType = collect.FunType, FunName = collect.FunName, LpszDbVarName = lpszDbVarName, DataValue = lpszVal, CollectTime = lpszdateTime, IsNDb = true };
            if (cmd.Action == 1 && cmd.Fun_id != 0)
            {/*成功记录写的值*/
                if (rst == true)
                    new DataProcessBLL(cmd.Ledger, Config.Uid).UpdataMap(cmd.Module_id, cmd.Fun_id, cmd.DataValue);
            }
            //AlarmCollect collectInfo = new AlarmCollect() { cmd = cmd, lpszVal = cmd.DataValue, lpszdateTime = lpszdateTime, errCode = errCode, errTxt = errTxt };
            //NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
            //}
            AddUIResult(cmd.Ledger, cmd.Module_id, cmd.ModuleAddr, "IOService", "IOService", cmd.FunType, cmd.FunName + "(写值)", cmd.LpszDbVarName, cmd.DataValue, lpszdateTime, rst, null, null, errCode, errTxt);
        }


        /// <summary>
        /// 保存采集项数据
        /// </summary>
        /// </summary>
        /// <param name="command">下发的命令</param>
        /// <param name="lpszVal">采集值</param>
        /// <param name="lpszdateTime">采集时间</param>
        /// <param name="rst">采集结果</param>
        /// <param name="errCode">采集错误码</param>
        /// <param name="errTxt">采集错误信息</param>
        internal static void YdDriveSaveCollectCache(CommandVModel command, string lpszVal, DateTime lpszdateTime, UInt32 dwUserData, bool rst, AlarmType errCode, string errTxt, byte[] send, byte[] result)
        {
            AlarmCollect collectInfo = new AlarmCollect() { cmd = command, lpszVal = lpszVal, lpszdateTime = lpszdateTime, errCode = errCode, errTxt = errTxt };
            string msg = errTxt;
            CollectVModel collect = null;
            SysPro pro;
            NCSys.Pro.TryGetValue(command.Ledger, out pro);
            if (pro == null)
            {
                FileLog.Error("YdDriveSaveCollectCache模块没有还未有本项目:" + command.Ledger);
                return;
            }

            //if (command.LpszDbVarName == ("5419.166610222091.EventWpf"))
            //{
            //    FileLog.Error("YdDriveSaveCollectCache:" + command.LpszDbVarName);
            //}

            if (rst == false || command.FunType.Equals(V0Fun.EventWpf.ToString()) || 
                command.Log_id != 0 || command.IsNDb == true || (command.Action == 1 && command.Fun_id != 0))
            {
                if (rst == false && (errCode == AlarmType.Err_Tx || errCode == AlarmType.Err_Sb))
                    collectInfo.isSend = 0;
                bool isAddQueue = true;
                if (rst == false && errCode == AlarmType.Err_Tx)
                {/*铜通讯错误*//*通讯异常,需要发告警,并且距离上次30分钟还是连接不上出告警*/
                    string mkey = pro.ProjectKey + command.LpszDbVarName;
                    collect = null;
                    NCSys.Result.TryGetValue(mkey, out collect);
                    //DateTime collectTime = collect == null ? Helper.LoadTime : collect.RstVar.lpszdateTime;
                    //if (collectTime.AddMinutes(30) > lpszdateTime)
                    //{/*不加入队列*/
                    //    isAddQueue = false;
                    //}
                }
                if (isAddQueue == true)
                {
                    if (command.FunType.Equals(V0Fun.Ssr.ToString()) && !string.IsNullOrEmpty(command.DataValue))
                    {
                        //if (command.Log_id == 0 && command.IsNDb != true)
                        //    FileLog.WriteLog("没有入库拉合闸命令:" + JsonHelper.Serialize(command));

                        //if (command.Log_id > 0)
                        //    FileLog.WriteLog("入库拉合闸命令2:" + JsonHelper.Serialize(command));
                    }
                    //NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
                }
            }

            if (rst == true)
            {
                RefreshCached(command, lpszVal, errCode);/*更新缓存，需优化*/
                //if (command.Fun_id != 0 && (command.Action == 0 || command.Action == 3))
                if (command.Fun_id != 0 && (command.Action != 1))
                {/*写、跟事件不存缓存*/
                    //SaveCollectCache(command.Ledger,command.LpszDbVarName, lpszVal, lpszdateTime, dwUserData, ref command);
                    SaveCollectCache(command.Ledger, command.LpszDbVarName, lpszVal, lpszdateTime, dwUserData);
                }
            }
            else
            {
                if (lpszdateTime.Year > 2000)
                { /*有采集信息*//*如果命令错误重新下发命令*/
                    if (CommFunc.ConvertDBNullToInt32(command.ErrCnt) < 3)
                    {
                        CommandVModel restCmd = command.Clone();
                        restCmd.ErrCnt = CommFunc.ConvertDBNullToInt32(restCmd.ErrCnt) + 1;
                        DataProcess.YdDrive.Collection.CollectionHelper.Instance(restCmd);
                    }
                }
            }

            //if (command.Log_id != 0 || command.IsNDb == true || (command.Action == 1 && command.Fun_id != 0))
            if (command.FunType.Equals(V0Fun.Ssr.ToString()) && !string.IsNullOrEmpty(command.DataValue))
            {/*立刻更新*/
                //ZpAlarm.DbCmdRefresh(command, lpszVal, lpszdateTime, errCode, errTxt);
                //if (command.Log_id == 0 && command.IsNDb != true)
                //    FileLog.WriteLog("没有入库拉合闸命令1:" + JsonHelper.Serialize(command));

                //if (command.Log_id >= 0 && command.IsNDb != true)
                //    FileLog.WriteLog("没有入库拉合闸命令2:" + JsonHelper.Serialize(command));
            }

            AddUIResult(command.Ledger, command.Module_id, command.ModuleAddr, command.HandledBY, command.ModuleType, command.FunType,
                command.FunName, command.ModuleAddr, lpszVal, lpszdateTime, rst, send, result, errCode, msg, command.IsUI, command.IsNDb);/*输出到界面信息*/
        }

        private static void SaveCollectCache(int ledger, string lpszDbVarName, string lpszVal, DateTime lpszdateTime, UInt32 dwUserData)
        {
            SysPro pro = null;
            NCSys.Pro.TryGetValue(ledger, out pro);
            if (pro == null)
            {
                FileLog.Debug("获取账目数据错误");
                return;
            }
            string mkey = pro.ProjectKey + lpszDbVarName;
            CollectVModel collect = null;
            if (NCSys.Result.TryGetValue(mkey, out collect) == false)
            {
                //collect = new CollectVModel() { Ledger = ledger, Module_id = 0 };
                //return null;
            }
            if (collect == null || collect.Module_id == 0)
            {/*正常不出现此错误，除非上面数据库或逻辑错误*/
                FileLog.Debug("获取采集数据错误");
                return ;
            }
            RstVar rstVar = MemcachedMgr.GetVal<RstVar>(collect.CachedKey);
            if (rstVar == null)
            {
                rstVar = new RstVar() { lpszVal = lpszVal, lpszdateTime = lpszdateTime, dwUserData = dwUserData };
            }
            /////////////////////////////////////////////////////////////////
            bool isUpCached = true; // 是否更新内存
            if ((collect.FunType.Equals("E")) && pro.Auto == 0)
            {
                decimal curLpszVal = CommFunc.ConvertDBNullToDecimal(lpszVal);
                decimal chdLpszVal = CommFunc.ConvertDBNullToDecimal(rstVar.lpszVal);
                //if (decimal.TryParse(lpszVal, out curLpszVal) == false)
                //{
                //    string[] arr = lpszVal.Split(new char[] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                //    if (arr.Count() < 2)
                //    {
                //        FileLog.Debug("KEY:" + lpszDbVarName + "采集数据" + lpszVal + "格式异常");
                //    }
                //    else
                //    {
                //        curLpszVal = CommFunc.ConvertDBNullToDecimal(arr[1]);
                //    }
                //    if (!string.IsNullOrEmpty(collect.RstVar.lpszVal))
                //    {
                //        arr = collect.RstVar.lpszVal.Split(new char[] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                //        if (arr.Count() < 2)
                //        {
                //            FileLog.Debug("KEY:" + lpszDbVarName + "缓存的数据" + collect.RstVar.lpszVal + "格式异常");
                //        }
                //        else
                //        {
                //            chdLpszVal = CommFunc.ConvertDBNullToDecimal(arr[1]);
                //        }
                //    }
                //}
                if (chdLpszVal > curLpszVal)
                {
                    //collectInfo.errCode = AlarmType.Ez_1;
                    //collectInfo.errTxt = "上期采集值:" + CommFunc.ConvertDBNullToDecimal(collect.RstVar.lpszVal) + "本次采集值:" + CommFunc.ConvertDBNullToDecimal(lpszVal);
                    //collectInfo.isSend = 0;
                    //NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
                    isUpCached = false;
                    FileLog.Debug("KEY:" + lpszDbVarName + "采集数据反转" + " 上期采集值:" + chdLpszVal + "本次采集值:" + curLpszVal);
                }
                else
                {
                    if (pro.MaxVal != 0)
                    {
                        decimal useVal = curLpszVal - chdLpszVal;/*当前使用量*/
                        decimal maxUseVal = (decimal)((lpszdateTime - rstVar.lpszdateTime).TotalMinutes / pro.FrMd) * pro.MaxVal;
                        if (useVal > maxUseVal)
                        {
                            //collectInfo.errCode = AlarmType.Err_OFlow;
                            //collectInfo.errTxt = "上期采集值:" + CommFunc.ConvertDBNullToDecimal(collect.RstVar.lpszVal) + "时间" + collect.RstVar.lpszdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "本次采集值:" + CommFunc.ConvertDBNullToDecimal(lpszVal) + "时间" + lpszdateTime.ToString("yyyy-MM-dd HH:mm:ss");
                            //collectInfo.isSend = 0;
                            //NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
                            isUpCached = false;
                            FileLog.Debug("KEY:" + lpszDbVarName + "采集数据越限" + "上期采集值:" + chdLpszVal + "时间" + rstVar.lpszdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "本次采集值:" + curLpszVal + "时间" + lpszdateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                }
            }

            if (CommFunc.ConvertDBNullToDateTime(rstVar.lpszdateTime) > lpszdateTime)
            {
                //collectInfo.errCode = AlarmType.Ez_2;
                //collectInfo.errTxt = "上期采集值:" + collect.RstVar.lpszdateTime + "本次采集值:" + lpszVal;
                //collectInfo.isSend = 0;
                //NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
                isUpCached = false;
                FileLog.Debug("KEY:" + lpszDbVarName + "值采集时间反转");
            }

            if (isUpCached == true)
            {
                bool isDb = false;
                if (collect.FunType.ToLower().Equals("ssr") && !rstVar.lpszVal.Equals(lpszVal))
                    isDb = true;/*更新存储入库标记*/
                DateTime lastTime = rstVar.lpszdateTime;
                rstVar.lpszVal = lpszVal;
                rstVar.lpszdateTime = lpszdateTime;
                rstVar.dwUserData = dwUserData;
                if (collect.Action == 0)
                {/*事件的数据不缓存*/
                    bool last = MemcachedMgr.SetVal(collect.CachedKey, rstVar);/*加入memcached*/
                    if (last == false)
                    {
                        if (lpszdateTime > lastTime.AddMinutes(15))
                        {/*相隔15分钟入队列*/
                            //collectInfo.errCode = AlarmType.Err_Nd;
                            //collectInfo.errTxt = "更新Memcahed错误";
                            //collectInfo.isSend = 0;
                            //NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
                        }
                        /////////////////////////////////////////
                        FileLog.WriteLog("变量" + collect.CachedKey + "更新Memcahed错误");
                    }
                    else
                    {
                        if (isDb == true)
                            collect.IsDb = true;
                        FileLog.Debug("KEY:" + collect.CachedKey + " 保存缓存成功值:" + rstVar.lpszVal);
                    }
                }
               
            }
        }

        /// <summary>
        /// 更新缓存信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="errCode"></param>
        /// <param name="errTxt"></param>
        private static void RefreshCached(CommandVModel command, string lpszVal, AlarmType errCode)
        {
            if (errCode != AlarmType.Sue) return;/*不成功则返回；成功则继续往下操作*/

            if (command.Action == 1 && command.Fun_id != 0)
            {/*记录写的值*/
                new DataProcessBLL(command.Ledger, Config.Uid).UpdataMap(command.Module_id, command.Fun_id, lpszVal);
            }
            SysPro pro = null;
            NCSys.Pro.TryGetValue(command.Ledger, out pro);
            if (pro == null) return;
            string ss1 = pro.ProjectKey + command.Module_id.ToString() + "." + command.ModuleAddr + ".";

            if (command.FunType.Equals(V0Fun.Ssr.ToString()) && !string.IsNullOrEmpty(command.DataValue))
            { // 立刻更新拉合闸
                string key = ss1 + V0Fun.Ssr.ToString();
                RstVar rst = MemcachedMgr.GetVal<RstVar>(key);
                if (rst == null) return;
                rst.lpszVal = command.DataValue;
                rst.lpszdateTime = command.CollectTime.Value;
                rst.dwUserData = 0;
                ///////////////
                CollectVModel collect = null;
                if (NCSys.Result.TryGetValue(key, out collect) == true)
                {
                    //collect.RstVar = rst;
                    //collect.IsDb = true;
                    //collect.IsRn = command.IsRn;
                }
                ///////////////
                MemcachedMgr.SetVal(key, rst);
            }
            if (command.FunType.Equals(V0Fun.Pay.ToString()) || command.FunType.Equals(V0Fun.Refund.ToString()))
            { // 充值;退费
                string key = ss1 + V0Fun.RdVal.ToString();
                RstVar rst = MemcachedMgr.GetVal<RstVar>(key);
                if (rst == null) return;
                decimal lpszPay = CommFunc.ConvertDBNullToDecimal(rst.lpszVal);
                if (command.FunType.Equals(V0Fun.Pay.ToString()))
                    lpszPay = lpszPay + CommFunc.ConvertDBNullToDecimal(command.DataValue);
                else if (command.FunType.Equals(V0Fun.Refund.ToString()))
                    lpszPay = lpszPay - CommFunc.ConvertDBNullToDecimal(command.DataValue);
                rst.lpszVal = lpszPay.ToString();
                rst.lpszdateTime = command.CollectTime.Value;
                rst.dwUserData = 0;
                ///////////////
                CollectVModel collect = null;
                if (NCSys.Result.TryGetValue(key, out collect) == true)
                {
                    //collect.RstVar = rst;
                    //collect.IsDb = true;
                }
                ///////////////
                MemcachedMgr.SetVal(key, rst);
                #region 2018.07.02
                //if (command.ModuleType.Equals("YD-ZB20-4PL2-SB"))
                //{
                //    key = ss1 + V0Fun.E.ToString();
                //    collect = null;
                //    if (NCSys.Result.TryGetValue(key, out collect) == true)
                //        collect.IsDb = true;
                //}
                #endregion
            }
        }

        private static void AddUIResult(int ledger, int module_id, string moduleAddr, string handleBy, string moduleType, string funType,
            string funName, string lpszdbVarName, string lpszVal, DateTime lpszdateTime, bool rst, byte[] send, byte[] result, AlarmType errCode, string msg, bool isUI = false,bool isNDb = false)
        {
            #region 放结果集
            CmdResult UIRst = new CmdResult();
            UIRst.Ledger = ledger;
            UIRst.Module_id = module_id;
            UIRst.ModuleAddr = moduleAddr;
            UIRst.FunType = funType;
            UIRst.FunName = funName;
            UIRst.BiludBY = handleBy;
            UIRst.Update = lpszdateTime;
            UIRst.ErrCode = 1;
            UIRst.ErrTxt = msg;
            UIRst.Value = lpszVal;

            if (rst == false)
            {
                UIRst.Update = DateTime.Now;
                UIRst.ErrCode = (int)errCode;
                UIRst.ErrTxt = msg + " ";
                UIRst.Value = "";
            }
            if (send == null)
            {
                send = new byte[] { };
            }
            if (result == null)
            {
                result = new byte[] { };
            }
            //
            if (errCode == AlarmType.Err_Tx)
            {
                UIRst.Result.Add("连接端口失败;" + handleBy + ";" + "" + ";" +
                "" + ";" + UIRst.Update.ToString("yyyy-MM-dd HH:mm:ss fff") + ";" +
                "Send:" + BitConverter.ToString(send) + " Result:" + BitConverter.ToString(result));
            }
            else if (errCode == AlarmType.Err_Pz)
            {
                UIRst.Result.Add("设备类型错误;" + moduleType + ";" + "" + ";" +
                "" + ";" + UIRst.Update.ToString("yyyy-MM-dd HH:mm:ss fff") + ";" +
                "Send:" + BitConverter.ToString(send) + " Result:" + BitConverter.ToString(result));
            }
            else if (errCode == AlarmType.Err_Nd)
            {
                UIRst.Result.Add("没有定义此功能码;" + funType + ";" + "" + ";" +
                "" + ";" + UIRst.Update.ToString("yyyy-MM-dd HH:mm:ss fff") + ";" +
                "Send:" + BitConverter.ToString(send) + " Result:" + BitConverter.ToString(result));
            }
            else
            {
                //string funName = command.FunName;
                string lpName = lpszdbVarName;
                if (string.IsNullOrEmpty(funName))
                    funName = CommFunc.GetEnumDisplay(typeof(V0Fun), funType);
                if (string.IsNullOrEmpty(lpName))
                    lpName = moduleAddr;
                UIRst.Result.Add(funName + ";地址:" + lpName + ";值:" + UIRst.Value + ";通信方式:" +
                UIRst.BiludBY + ";" + UIRst.Update.ToString("yyyy-MM-dd HH:mm:ss fff") + ";" +
                UIRst.ErrTxt + "Send:" + BitConverter.ToString(send) + " Result:" + BitConverter.ToString(result));
            }

            lock (wholeObj)
            {
                NCSys.TotalCount = NCSys.TotalCount + 1;
                NCSys.ErrCount = NCSys.ErrCount + (rst == false ? 1 : 0);
            }

            bool isAdd = false;
            if (NCSys.IsUIResult == false)
            {
                if (rst == false)
                {
                    int ccc = NCSys.UIResult.Count;
                    if (ccc < 1000)
                        isAdd = true;
                }
            }
            if (NCSys.IsUIResult == true || isUI == true || isNDb == true)
                isAdd = true;

            if (isAdd == true)
                NCSys.UIResult.Enqueue(UIRst);/*页面输入*/
            #endregion
        }
       
    }
}
