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
        private static string RangeUse = ""; // 路灯使用电量告警
        private static DateTime LoadTime = DateTime.Now;
        private static SyscontBLL sysBll = new SyscontBLL(Config.Systems);
        private static object wholeObj = new Object();
        private static DataTable DtMap = new DataTable();
        private Helper()
        {
            Thread th1 = new Thread((ThreadStart)delegate { this.DbProcess(); });
            th1.Name = "DataProcess";
            th1.IsBackground = true;
            th1.Start();
            //
            Thread th2 = new Thread((ThreadStart)delegate { this.DbHand(); });
            th2.Name = "DataProcess.DbHand";
            th2.IsBackground = true;
            th2.Start();
            //
            Thread th3 = new Thread((ThreadStart)delegate { new Alarm().Run(); });
            th3.Name = "Db.Alarm";
            th3.IsBackground = true;
            th3.Start();
            //
            Thread th4 = new Thread((ThreadStart)delegate { new ZpSi().Run(); });
            th4.Name = "Db.ZpSi";
            th4.IsBackground = true;
            th4.Start();
            try
            {/**/
                //ProcessSevice.Start(new ICEFactory(), Config.ListenPort);
            }
            catch (Exception ex)
            {
                FileLog.Error("启动通讯服务错误:" + ex.Message + ex.StackTrace);
            }
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
                addBaseValTime = null;
                updatePayEstTime = null;

                Helper.LoadTime = DateTime.Now;
                Helper.RangeUse = CommFunc.ConvertDBNullToString(IniHepler.GetConfig(Config.lpFileName, "Alarm", "RangeUse"));
                Helper.DtMap = null;

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
                    DataProcessBLL bll = new DataProcessBLL(ledger, Config.Uid);
                    DataTable dtConfig = bll.GetSysConfig("");
                    AliSmsConfig sms = new AliSmsConfig();
                    ComSmsConfig comConfig = new ComSmsConfig();
                    WxConfig wx = new WxConfig();
                    EmailConfig em = new EmailConfig();
                    System.Reflection.PropertyInfo[] smsInfo = sms.GetType().GetProperties();
                    System.Reflection.PropertyInfo[] comInfo = comConfig.GetType().GetProperties();
                    System.Reflection.PropertyInfo[] wxInfo = wx.GetType().GetProperties();
                    System.Reflection.PropertyInfo[] emInfo = em.GetType().GetProperties();
                    foreach (DataRow dr in dtConfig.Rows)
                    {
                        foreach (System.Reflection.PropertyInfo f_key in smsInfo)
                        {
                            if (CommFunc.ConvertDBNullToString(dr["CfKey"]).Equals(f_key.Name))
                                f_key.SetValue(sms, dr["CfValue"], null);
                        }
                        foreach (System.Reflection.PropertyInfo f_key in wxInfo)
                        {
                            if (CommFunc.ConvertDBNullToString(dr["CfKey"]).Equals(f_key.Name))
                                f_key.SetValue(wx, dr["CfValue"], null);
                        }
                        foreach (System.Reflection.PropertyInfo f_key in emInfo)
                        {
                            if (CommFunc.ConvertDBNullToString(dr["CfKey"]).Equals(f_key.Name))
                                f_key.SetValue(em, dr["CfValue"], null);
                        }
                        foreach (System.Reflection.PropertyInfo f_key in comInfo)
                        {
                            if (CommFunc.ConvertDBNullToString(dr["CfKey"]).Equals(f_key.Name))
                                f_key.SetValue(comConfig, dr["CfValue"], null);
                        }
                    }
                    SmsFun.AliSmsConfig = sms;
                    SmsFun.ComSmsConfig = comConfig;
                    pro.Wx.WxConfig = wx;
                    EmailUtilities.EmConfig = em;
                    #endregion

                    #region 更新配置
                    DataTable dtTmp = bll.GetMapInfo("");
                    if (DtMap == null)
                        DtMap = dtTmp;
                    else
                        DtMap.Merge(dtTmp.Copy(), true, MissingSchemaAction.Ignore);
                    #endregion
                }
                //Helper.FrMd = Helper.FrMd == 0 ? 15 : Helper.FrMd;
                //Helper.SendSpan = Helper.SendSpan == 0 ? 15 : Helper.SendSpan;
                #endregion

                #region 获取IOService变量
                System.Data.DataTable dtSource = sysBll.GetMapInfo();
                Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
                //List<string> tag = new List<string>();
                foreach (System.Data.DataRow dr in dtSource.Rows)
                {
                    string handledBY = CommFunc.ConvertDBNullToString(dr["HandledBY"]);
                    if (!string.IsNullOrEmpty(Config.IOHandledBY))
                        handledBY = Config.IOHandledBY;
                    List<string> tag = null;
                    if (dic.TryGetValue(handledBY, out tag) == false)
                    {
                        tag = new List<string>();
                        dic.Add(handledBY, tag);
                    }
                    tag.Add(CommFunc.ConvertDBNullToString(dr["TagName"]));
                }
                foreach (var kk in dic)
                {
                    DataProcess.IOService.Collection.CollectionHelper.Instance(kk.Key, kk.Value);
                }
                //DataProcess.IOService.Collection.CollectionHelper.Instance("192.168.5.214:8109",tag);
                #endregion


            }
        }

        DateTime? addBaseValTime = null;
        DateTime? updatePayEstTime = null;
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
                        RstVar rst = s1.RstVar;
                        SysPro pro = null;
                        NCSys.Pro.TryGetValue(s1.Ledger, out pro);
                        if (pro == null) continue;
                        if (rst == null) continue;
                        if (DateTime.Now > rst.lpszdateTime.AddMinutes(pro.FrMd * 6))
                        {/*6的倍数还未采集过数据上来，移除对象*/
                            NCSys.Result.TryRemove(s0.Key, out s1);
                            continue;
                        }
                        if (s1.Module_id == 0 || s1.Action == 2) continue;

                        if (pro.Project.Equals("YdXip") && !s1.FunType.Equals("E")) continue; /*拉合闸不保存*/

                        if (s1.IsDb == false)
                        {
                            if (s1.UpTime.AddMinutes(pro.FrMd) > rst.lpszdateTime) continue;
                            //if (DateTime.Now.Minute % pro.FrMd != 0) continue;/*非剩余电量整数*/
                        }

                        try
                        {
                            string msg;
                            DataProcessBLL bll = new DataProcessBLL(s1.Ledger, Config.Uid);
                            int errcode = bll.DataProcess(s1.Module_id, s1.ModuleAddr, s1.Fun_id, rst.lpszVal, rst.lpszdateTime, out msg);
                            if (errcode < 0)
                            {
                                FileLog.Error("KEY:" + s0.Key + "值:" + rst.lpszVal + "数据处理结果错误:" + msg);
                            }
                            else
                            {
                                FileLog.Debug("KEY:" + s0.Key + "值:" + rst.lpszVal + "保存成功");
                                #region 检查是否调整用剩余用电
                                if (s1.FunType.Equals(V0Fun.RdVal.ToString()))
                                {
                                    try
                                    {
                                        DataTable dtRd = bll.GetArmAndRdVal(s1.Module_id, s1.ModuleAddr);
                                        bool isRun = true;
                                        decimal rdVal = 0, mRdVal = 0;
                                        DateTime rdTime = DateTime.Now, mRdTime = DateTime.Now;
                                        if (dtRd.Rows.Count != 1) isRun = false;
                                        if (dtRd.Rows.Count == 1)
                                        {
                                            rdVal = CommFunc.ConvertDBNullToDecimal(dtRd.Rows[0]["RdVal"]);
                                            mRdVal = CommFunc.ConvertDBNullToDecimal(dtRd.Rows[0]["MRdVal"]);
                                            rdTime = CommFunc.ConvertDBNullToDateTime(dtRd.Rows[0]["RdTime"]);
                                            mRdTime = CommFunc.ConvertDBNullToDateTime(dtRd.Rows[0]["MRdTime"]);
                                        }
                                        int spt = (int)(mRdTime - rdTime).TotalMinutes;
                                        decimal useVal = rdVal - mRdVal;
                                        if (useVal == 0) isRun = false;
                                        if (useVal < 0)
                                        {/*退费*/
                                            if (mRdVal < 0)
                                            {/*都是负数不退电；原因设备退电失败*/
                                                isRun = false;
                                                if (mRdVal < -10)// && Math.Abs(useVal) > 3
                                                {/*如果剩余电量小于-10度(透支电量)，则直接更新数据库同步*/
                                                    bll.UpdateV2FirstVal(s1.Module_id, s1.ModuleAddr, useVal);
                                                }
                                            }
                                            else
                                            {
                                                decimal a1 = Math.Abs(useVal);
                                                decimal a2 = Math.Abs(mRdVal);
                                                if (a1 > a2)
                                                {/**/
                                                    useVal = 0 - a2;/*退尽电表剩余电量*/
                                                }
                                            }
                                        }

                                        //if (rdVal < 0 && mRdVal < 0)
                                        //    isRun = false;/*都小于0*/
                                        //if (rdVal < 0 && mRdVal > 0)
                                        //{/*退费*/
                                        //    useVal = 0 - mRdVal;/*全退*/
                                        //}

                                        //decimal useVal = rdVal - mRdVal;                                   
                                        //if (useVal < 0)
                                        //    useVal = Math.Abs(mRdVal) > Math.Abs(useVal) ? (0 - Math.Abs(mRdVal)) : useVal;
                                        //
                                        if (isRun == true)
                                        {
                                            long cmd_id = bll.AddPayCommmand(s1.Module_id, s1.ModuleAddr, useVal);
                                            if (cmd_id != 0)
                                            {
                                                CommandVModel cmd = bll.GetCommmand(cmd_id);
                                                if (cmd != null)
                                                {
                                                    cmd.IsUI = false;
                                                    DataProcess.YdDrive.Collection.CollectionHelper.Instance(cmd);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        FileLog.Error("KEY:" + s0.Key + "调整剩余电量错误:" + ex.Message + ex.Message);
                                    }
                                }
                                else if (s1.FunType.Equals(V0Fun.E.ToString()) && s1.ModuleType.Equals("YD-ZB20-4PL2-SB") && s1.ModuleAddr.Equals(s1.MeterAddr))
                                {
                                    try
                                    {
                                        DataTable dtRd = bll.GetArmAndRdVal2(s1.Module_id, s1.ModuleAddr);
                                        bool isRun = true;
                                        decimal rdVal = 0, syZsVal = 0;
                                        DateTime rdTime = DateTime.Now, mRdTime = DateTime.Now;
                                        if (dtRd.Rows.Count != 1) isRun = false;
                                        if (dtRd.Rows.Count == 1)
                                        {
                                            rdVal = CommFunc.ConvertDBNullToDecimal(dtRd.Rows[0]["RdVal"]);
                                            syZsVal = CommFunc.ConvertDBNullToDecimal(dtRd.Rows[0]["SyZsVal"]);
                                        }
                                        //
                                        if (isRun == true)
                                        {
                                            long cmd_id = bll.AddPayCommmand(s1.Module_id, s1.ModuleAddr, rdVal - syZsVal, V0Fun.PaySb1);
                                            if (cmd_id != 0)
                                            {
                                                CommandVModel cmd = bll.GetCommmand(cmd_id);
                                                if (cmd != null)
                                                {
                                                    cmd.IsUI = false;
                                                    DataProcess.YdDrive.Collection.CollectionHelper.Instance(cmd);
                                                }
                                            }
                                            cmd_id = bll.AddPayCommmand(s1.Module_id, s1.ModuleAddr, syZsVal, V0Fun.PaySb2);
                                            if (cmd_id != 0)
                                            {
                                                CommandVModel cmd = bll.GetCommmand(cmd_id);
                                                if (cmd != null)
                                                {
                                                    cmd.IsUI = false;
                                                    DataProcess.YdDrive.Collection.CollectionHelper.Instance(cmd);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        FileLog.Error("KEY:" + s0.Key + "调整剩余电量错误:" + ex.Message + ex.Message);
                                    }
                                }
                                #endregion
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


                #region 赠电
                try
                {
                    if (addBaseValTime == null || (DateTime.Now.Hour == 0 && new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) > addBaseValTime.Value))
                    {
                        foreach (var s1 in NCSys.Pro)
                        {
                            ZpRaVal.BaseVal(s1.Key);
                        }
                        addBaseValTime = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    FileLog.Error("赠电处理错误:" + ex.Message + ex.StackTrace);
                }
                #endregion

                #region 计算物业费
                try
                {
                    if (updatePayEstTime == null || (DateTime.Now.Hour == 2 && new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) > updatePayEstTime.Value))
                    {
                        foreach (var s1 in NCSys.Pro)
                        {
                            YdPayEst.UpdatePayEst(s1.Key);
                        }
                        updatePayEstTime = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    FileLog.Error("物业费处理错误:" + ex.Message + ex.StackTrace);
                }
                #endregion


            }
        }

        private void DbHand()
        {
            while (true)
            {
                Thread.Sleep(100);/*每100毫秒*/
                AlarmCollect cc = null;
                if (NCSys.AlarmCollect.TryDequeue(out cc) == false) continue;
                try
                {
                    ZpAlarm.DbCmdRefresh(cc.cmd, cc.lpszVal, cc.lpszdateTime, cc.errCode, cc.errTxt);
                    if (cc.cmd != null && cc.cmd.FunType.Equals(V0Fun.EventWpf.ToString()))
                        ZpAlarm.DbEventWpf(cc.cmd, cc.lpszVal, cc.lpszdateTime, cc.errCode, cc.errTxt);/*恶性负载*/
                    else
                        ZpAlarm.DbAddAlarm(cc.cmd, cc.lpszVal, cc.lpszdateTime, cc.errCode, cc.errTxt, cc.isSend == 1 ? true : false);
                }
                catch (Exception ex)
                {
                    FileLog.Error("KEY:" + cc.cmd.LpszDbVarName + "记录告警错误:" + ex.Message + ex.StackTrace);
                }
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
        internal static void IOServiceEventCollect(string lpszDbVarName, string lpszVal, DateTime lpszdateTime, string errTxt)
        {
            foreach (var s1 in NCSys.Pro)
            {
                string mkey = s1.Value.ProjectKey + lpszDbVarName;
                CollectVModel collect = null;
                if (NCSys.Result.TryGetValue(mkey, out collect) == false) continue;
                if (collect.Module_id == 0) continue;/*没有映射关系不存储告警*/
                CommandVModel cmd = new CommandVModel() { Ledger = s1.Key, Module_id = collect.Module_id, ModuleAddr = collect.ModuleAddr, Fun_id = collect.Fun_id, FunType = collect.FunType, FunName = collect.FunName, LpszDbVarName = lpszDbVarName };
                AlarmCollect collectInfo = new AlarmCollect() { cmd = cmd, lpszVal = lpszVal, lpszdateTime = lpszdateTime, errCode = AlarmType.Err_OFlow, errTxt = errTxt };
                NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
            }
        }
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
            AlarmCollect collectInfo = new AlarmCollect() { cmd = cmd, lpszVal = cmd.DataValue, lpszdateTime = lpszdateTime, errCode = errCode, errTxt = errTxt };
            NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
            //}
            AddUIResult(cmd.Ledger, cmd.Module_id, cmd.ModuleAddr, "IOService", "IOService", cmd.FunType, cmd.FunName + "(写值)", cmd.LpszDbVarName, cmd.DataValue, lpszdateTime, rst, null, null, errCode, "");
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

            if (rst == false || command.Log_id != 0 || command.IsNDb == true
                || (command.Action == 1 && command.Fun_id != 0) || command.FunType.Equals(V0Fun.EventWpf.ToString()))
            {
                if (rst == false && (errCode == AlarmType.Err_Tx || errCode == AlarmType.Err_Sb))
                    collectInfo.isSend = 0;
                bool isAddQueue = true;
                if (rst == false && errCode == AlarmType.Err_Tx)
                {/*铜通讯错误*//*通讯异常,需要发告警,并且距离上次30分钟还是连接不上出告警*/
                    string mkey = pro.ProjectKey + command.LpszDbVarName;
                    collect = null;
                    NCSys.Result.TryGetValue(mkey, out collect);
                    DateTime collectTime = collect == null ? Helper.LoadTime : collect.RstVar.lpszdateTime;
                    if (collectTime.AddMinutes(30) > lpszdateTime)
                    {/*不加入队列*/
                        isAddQueue = false;
                    }
                }
                if (isAddQueue == true)
                    NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
            }

            if (rst == true)
            {
                RefreshCached(command, lpszVal, errCode);/*更新缓存，需优化*/
                //if (command.Fun_id != 0 && (command.Action == 0 || command.Action == 3))
                if (command.Fun_id != 0 && (command.Action != 1))
                {/*写、跟事件不存缓存*/
                    //SaveCollectCache(command.Ledger,command.LpszDbVarName, lpszVal, lpszdateTime, dwUserData, ref command);
                    collect = SaveCollectCache(command.Ledger, command.LpszDbVarName, lpszVal, lpszdateTime, dwUserData);
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
            #region 西安培华(如果是合闸且有欠费拉闸标示则立刻查询是否有欠费)
            if (collect != null && command != null && command.FunType.Equals(V0Fun.Ssr.ToString()) && pro.Project.Equals("YdXip"))
            {
                if (CommFunc.ConvertDBNullToInt32(lpszVal) == 0)
                {/*合闸*/
                    if (collect.SueCnt == 1 || collect.IsRn == 1)
                    {/*检查是否欠费*/
                        lock (wholeObj)
                        {
                            DataProcessBLL bll = new DataProcessBLL(command.Ledger, Config.Uid);
                            DataTable dtQf = bll.GetIsQiFei(command.Module_id);
                            if (dtQf.Rows.Count > 0)
                            {/*欠费拉闸*/
                                CommandVModel reset = command.Clone();
                                if (string.IsNullOrEmpty(command.HandledBY))
                                {
                                    FileLog.Error("YdXip:欠费拉闸队列键值为空" + JsonHelper.Serialize(reset));
                                    return;
                                }
                                reset.DataValue = "1";
                                reset.IsNDb = true;
                                reset.Create_by = Config.Uid;
                                reset.IsRn = 1;
                                DataProcess.YdDrive.Collection.CollectionHelper.Instance(reset);
                            }
                        }
                    }
                }
            }
            #endregion
            AddUIResult(command.Ledger, command.Module_id, command.ModuleAddr, command.HandledBY, command.ModuleType, command.FunType, command.FunName, command.ModuleAddr, lpszVal, lpszdateTime, rst, send, result, errCode, msg);/*输出到界面信息*/
        }

        private static CollectVModel SaveCollectCache(int ledger, string lpszDbVarName, string lpszVal, DateTime lpszdateTime, UInt32 dwUserData)
        {
            SysPro pro = null;
            NCSys.Pro.TryGetValue(ledger, out pro);
            if (pro == null) return null;

            string mkey = pro.ProjectKey + lpszDbVarName;
            CollectVModel collect = null;
            if (NCSys.Result.TryGetValue(mkey, out collect) == false)
            {
                collect = new CollectVModel() { Ledger = ledger, Module_id = 0 };
            }
            if (collect.Module_id == 0 && DateTime.Now >= collect.UpTime.AddMinutes(pro.FrMd))
            {
                try
                {
                    DataRow[] curDr = null;
                    if (DtMap != null)
                        curDr = DtMap.Select("Ledger=" + ledger + " and TagName='" + lpszDbVarName + "'");
                    if (curDr == null)
                    {
                        lock (wholeObj)
                        {
                            DataProcessBLL bll = new DataProcessBLL(ledger, Config.Uid);
                            System.Data.DataTable dtSource = bll.GetMapInfo(lpszDbVarName);
                            curDr = dtSource.Select();
                        }
                    }
                    int ccc = curDr == null ? 0 : curDr.Count();
                    //DataProcessBLL bll = new DataProcessBLL(ledger, Config.Uid);
                    //System.Data.DataTable dtSource = bll.GetMapInfo(lpszDbVarName);
                    //int ccc = dtSource.Rows.Count;
                    if (ccc == 1)
                    {
                        collect = new CollectVModel() { Ledger = ledger, RstVar = new RstVar() { dwUserData = dwUserData } };
                        foreach (DataRow dr in curDr)
                        {
                            collect.CachedKey = CommFunc.ConvertDBNullToString(dr["CachedKey"]);
                            collect.Module_id = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);
                            collect.ModuleAddr = CommFunc.ConvertDBNullToString(dr["ModuleAddr"]);
                            collect.Fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]);
                            collect.FunType = CommFunc.ConvertDBNullToString(dr["FunType"]);
                            collect.IsChrg = CommFunc.ConvertDBNullToInt32(dr["IsChrg"]);
                            collect.FunName = CommFunc.ConvertDBNullToString(dr["FunName"]);
                            collect.UpTime = CommFunc.ConvertDBNullToDateTime(dr["LastTime"]);
                            collect.RstVar.lpszVal = dr["LastVal"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(dr["LastVal"]).ToString();
                            collect.RstVar.lpszdateTime = CommFunc.ConvertDBNullToDateTime(dr["LastTime"]);
                            collect.ModuleType = CommFunc.ConvertDBNullToString(dr["ModuleType"]);
                            collect.MeterAddr = CommFunc.ConvertDBNullToString(dr["MeterAddr"]);
                            collect.Action = CommFunc.ConvertDBNullToInt32(dr["Action"]);
                            if (collect.RstVar.lpszdateTime.Year < 2000)
                                collect.IsDb = true;
                            if (collect.ModuleType.Equals("YD-ZB20-4PL2-SB"))
                            {/*重启网关的时候，能不能写一次剩余购电和赠电*/
                                collect.IsDb = true;
                            }
                        }
                        NCSys.Result.TryAdd(mkey, collect);
                    }
                    else
                    {
                        if (ccc == 0)
                            FileLog.Debug("tagName:" + lpszDbVarName + "不存在数据库映射表中");
                        else
                            FileLog.Debug("tagName:" + lpszDbVarName + "数据库映射表中存在多个");
                    }
                }
                catch (Exception ex)
                {
                    collect = null;
                    FileLog.Debug("获取DB数据错误:" + ex.Message + ex.StackTrace);
                }
            }

            if (collect == null)
            {/*正常不出现此错误，除非上面数据库或逻辑错误*/
                FileLog.Debug("获取采集数据错误");
                return null;
            }
            if (collect.Module_id == 0) return null;/*无数据内容*/
            collect.SueCnt = collect.SueCnt + 1;
            CommandVModel cmd = new CommandVModel() { Ledger = ledger, Module_id = collect.Module_id, ModuleAddr = collect.ModuleAddr, Fun_id = collect.Fun_id, FunType = collect.FunType, FunName = collect.FunName, LpszDbVarName = lpszDbVarName, };
            AlarmCollect collectInfo = new AlarmCollect() { cmd = cmd, lpszVal = lpszVal, lpszdateTime = lpszdateTime, errCode = AlarmType.Err_Nd, errTxt = "" };
            /////////////////////////////////////////////////////////////////
            bool isUpCached = true; // 是否更新内存

            if ((collect.FunType.Equals("E") || collect.IsChrg == 1) && pro.Auto == 0)
            {
                decimal curLpszVal = CommFunc.ConvertDBNullToDecimal(lpszVal);
                decimal chdLpszVal = CommFunc.ConvertDBNullToDecimal(collect.RstVar.lpszVal);
                if (decimal.TryParse(lpszVal, out curLpszVal) == false)
                {
                    string[] arr = lpszVal.Split(new char[] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Count() < 2)
                    {
                        FileLog.Debug("KEY:" + lpszDbVarName + "采集数据" + lpszVal + "格式异常");
                    }
                    else
                    {
                        curLpszVal = CommFunc.ConvertDBNullToDecimal(arr[1]);
                    }
                    if (!string.IsNullOrEmpty(collect.RstVar.lpszVal))
                    {
                        arr = collect.RstVar.lpszVal.Split(new char[] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (arr.Count() < 2)
                        {
                            FileLog.Debug("KEY:" + lpszDbVarName + "缓存的数据" + collect.RstVar.lpszVal + "格式异常");
                        }
                        else
                        {
                            chdLpszVal = CommFunc.ConvertDBNullToDecimal(arr[1]);
                        }
                    }
                }
                if (chdLpszVal > curLpszVal)
                {
                    collectInfo.errCode = AlarmType.Ez_1;
                    collectInfo.errTxt = "上期采集值:" + CommFunc.ConvertDBNullToDecimal(collect.RstVar.lpszVal) + "本次采集值:" + CommFunc.ConvertDBNullToDecimal(lpszVal);
                    collectInfo.isSend = 0;
                    NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
                    isUpCached = false;
                    FileLog.Debug("KEY:" + lpszDbVarName + "采集数据反转" + " 上期采集值:" + CommFunc.ConvertDBNullToDecimal(collect.RstVar.lpszVal) + "本次采集值:" + CommFunc.ConvertDBNullToDecimal(lpszVal));
                }
                else
                {
                    if (pro.MaxVal != 0)
                    {
                        decimal useVal = curLpszVal - chdLpszVal;/*当前使用量*/
                        decimal maxUseVal = (decimal)((lpszdateTime - collect.RstVar.lpszdateTime).TotalMinutes / pro.FrMd) * pro.MaxVal;
                        if (useVal > maxUseVal)
                        {
                            collectInfo.errCode = AlarmType.Err_OFlow;
                            collectInfo.errTxt = "上期采集值:" + CommFunc.ConvertDBNullToDecimal(collect.RstVar.lpszVal) + "时间" + collect.RstVar.lpszdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "本次采集值:" + CommFunc.ConvertDBNullToDecimal(lpszVal) + "时间" + lpszdateTime.ToString("yyyy-MM-dd HH:mm:ss");
                            collectInfo.isSend = 0;
                            NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
                            isUpCached = false;
                            FileLog.Debug("KEY:" + lpszDbVarName + "采集数据越限" + "上期采集值:" + CommFunc.ConvertDBNullToDecimal(collect.RstVar.lpszVal) + "时间" + collect.RstVar.lpszdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "本次采集值:" + CommFunc.ConvertDBNullToDecimal(lpszVal) + "时间" + lpszdateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                }
            }

            if (CommFunc.ConvertDBNullToDateTime(collect.RstVar.lpszdateTime) > lpszdateTime)
            {/*上海本特勒采集回来的数据为零则认为通讯异常*/
                collectInfo.errCode = AlarmType.Ez_2;
                collectInfo.errTxt = "上期采集值:" + collect.RstVar.lpszdateTime + "本次采集值:" + lpszVal;
                collectInfo.isSend = 0;
                NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
                isUpCached = false;
                FileLog.Debug("KEY:" + lpszDbVarName + "值采集时间反转");
            }

            if (isUpCached == true)
            {/*路灯判断是否*正常路灯在早上7点至下午6电之间，正常情况下是不会用电，这期间若有用电量，需提示告警。*/
                if ((collect.FunType.Equals("E") || collect.IsChrg == 1))
                {
                    decimal useVal = CommFunc.ConvertDBNullToDecimal(lpszVal) - CommFunc.ConvertDBNullToDecimal(collect.RstVar.lpszVal);
                    string[] ru = Helper.RangeUse.Split(new char[] { ':', '-' }, StringSplitOptions.RemoveEmptyEntries);
                    int fmHour = ru.Count() == 4 ? CommFunc.ConvertDBNullToInt32(ru[0]) : 0;
                    int fmMin = ru.Count() == 4 ? CommFunc.ConvertDBNullToInt32(ru[1]) : 0;
                    int toHour = ru.Count() == 4 ? CommFunc.ConvertDBNullToInt32(ru[2]) : 0;
                    int toMin = ru.Count() == 4 ? CommFunc.ConvertDBNullToInt32(ru[3]) : 0;
                    DateTime fmTime = new DateTime(lpszdateTime.Year, lpszdateTime.Month, lpszdateTime.Day, fmHour, fmMin, 0);
                    DateTime toTime = new DateTime(lpszdateTime.Year, lpszdateTime.Month, lpszdateTime.Day, toHour, toMin, 0);
                    if (lpszdateTime >= fmTime && lpszdateTime <= toTime && useVal > 0 && fmTime != toTime)
                    {
                        collectInfo.errCode = AlarmType.Al_Use;
                        collectInfo.errTxt = "本次用电量:" + useVal + " 本期采集时间:" + lpszdateTime + "上期采集时间:" + collect.RstVar.lpszdateTime;
                        collectInfo.isSend = 1;
                        NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
                    }
                }
            }

            int _errCode = 0;
            #region 上海本特勒异常告警
            if (CommFunc.ConvertDBNullToDecimal(lpszVal) == 0 && pro.Project.Equals("EngShBen") && _errCode == 0)
            {
                if (collect.FunType.Equals(V0Fun.Ua.ToString()))
                {
                    if (lpszdateTime > collect.RstVar.lpszdateTime.AddMinutes(60))
                    {
                        collectInfo.errCode = AlarmType.Err_Tx;
                        collectInfo.errTxt = "采集值为零:通讯中断";
                        collectInfo.isSend = 0;
                        _errCode = -1;
                        collect.IsDb = true;
                        NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
                        FileLog.Debug("KEY:" + lpszDbVarName + "采集值为零:通讯中断");
                    }
                    else
                    {/*疑似出现告警不更新缓存*/
                        isUpCached = false;
                    }
                    if (CommFunc.ConvertDBNullToDecimal(collect.RstVar.lpszVal) != CommFunc.ConvertDBNullToDecimal(lpszVal))
                        collect.IsDb = true;
                }
            }
            if (collect.FunType.Equals(V0Fun.Ua.ToString()) && pro.Project.Equals("EngShBen") && _errCode == 0)
            {
                if (CommFunc.ConvertDBNullToDecimal(collect.RstVar.lpszVal) == CommFunc.ConvertDBNullToDecimal(lpszVal))
                {
                    if (lpszdateTime >= collect.RstVar.lpszdateTime.AddMinutes(60))
                    {/*如果电压15分钟内不变*/
                        collectInfo.errCode = AlarmType.Err_Tx;
                        collectInfo.errTxt = "电压15分钟内没有变化:通讯中断";
                        collectInfo.isSend = 0;
                        _errCode = -1;
                        collect.IsDb = true;/*立即更新入库*/
                        NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
                        FileLog.Debug("KEY:" + lpszDbVarName + "电压15分钟内没有变化:通讯中断");
                    }
                    else
                    {/*疑似出现告警不更新缓存*/
                        isUpCached = false;
                    }
                }
            }


            if (collect.FunType.Equals(V0Fun.Comm.ToString()))
            {  /*通讯告警*/
               //if (CommFunc.ConvertDBNullToInt32(collect.RstVar.lpszVal) != CommFunc.ConvertDBNullToInt32(lpszVal))
               //{/*通讯状态有变化*/
                if (CommFunc.ConvertDBNullToDecimal(lpszVal) > 0)
                {/*通讯变化中断*/
                    if (lpszdateTime > collect.RstVar.lpszdateTime.AddMinutes(60))
                    {
                        collect.IsDb = true;
                        collectInfo.errCode = AlarmType.Err_Tx;
                        collectInfo.errTxt = "通讯中断";
                        collectInfo.isSend = 0;
                        _errCode = -1;
                        NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
                        FileLog.Debug("KEY:" + lpszDbVarName + "采集值为1:通讯中断 时间:" + lpszdateTime);
                    }
                    else
                    {/*疑似出现告警不更新缓存*/
                        isUpCached = false;
                    }
                    //if (collect.CachedKey.Contains("118.R3B1.1.2.ZXYG.Comm"))
                    //    FileLog.Debug("KEY:" + lpszDbVarName + "采集值为1:通讯中断 时间:" + lpszdateTime);
                }

                if (CommFunc.ConvertDBNullToDecimal(collect.RstVar.lpszVal) != CommFunc.ConvertDBNullToDecimal(lpszVal))
                    collect.IsDb = true;
            }

            #endregion


            if (isUpCached == true)
            {
                DateTime lastTime = collect.RstVar.lpszdateTime;
                collect.RstVar.lpszVal = lpszVal;
                collect.RstVar.lpszdateTime = lpszdateTime;
                collect.RstVar.errCode = _errCode;
                bool last = MemcachedMgr.SetVal(pro.ProjectKey + collect.CachedKey, collect.RstVar);/*加入memcached*/
                if (last == false)
                {
                    if (lpszdateTime > lastTime.AddMinutes(15))
                    {/*相隔15分钟入队列*/
                        collectInfo.errCode = AlarmType.Err_Nd;
                        collectInfo.errTxt = "更新Memcahed错误";
                        collectInfo.isSend = 0;
                        NCSys.AlarmCollect.Enqueue(collectInfo); /*更新数据库*/
                    }
                    /////////////////////////////////////////
                    string msg = "变量" + pro.ProjectKey + collect.CachedKey + "更新Memcahed错误";
                    FileLog.WriteLog(msg);
                }
                else
                {
                    FileLog.Debug("KEY:" + pro.ProjectKey + collect.CachedKey + " 保存缓存成功值:" + collect.RstVar.lpszVal);
                }
            }
            if (isUpCached == true)
                return collect;
            else return null;
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
                    collect.RstVar = rst;
                    collect.IsDb = true;
                    collect.IsRn = command.IsRn;
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
                    collect.RstVar = rst;
                    collect.IsDb = true;
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

        private static void AddUIResult(int ledger, int module_id, string moduleAddr, string handleBy, string moduleType, string funType, string funName, string lpszdbVarName, string lpszVal, DateTime lpszdateTime, bool rst, byte[] send, byte[] result, AlarmType errCode, string msg)
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
            UIRst.ErrTxt = "";
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
            NCSys.UIResult.Enqueue(UIRst);/*页面输入*/
            #endregion
        }

        //private static void AddUIResult(CommandVModel command,string lpszVal,DateTime lpszdateTime,bool rst, byte[] send, byte[] result, AlarmType errCode, string msg)
        //{
        //    #region 放结果集
        //    CmdResult UIRst = new CmdResult();
        //    UIRst.Module_id = command.Module_id;
        //    UIRst.ModuleAddr = command.ModuleAddr;
        //    UIRst.BiludBY = command.HandledBY;
        //    UIRst.Update = lpszdateTime;
        //    UIRst.ErrCode = 1;
        //    UIRst.ErrTxt = "";
        //    UIRst.Value = lpszVal;
        //    if (rst == false)
        //    {
        //        UIRst.Update = DateTime.Now;
        //        UIRst.ErrCode = (int)errCode;
        //        UIRst.ErrTxt = msg + " ";
        //        UIRst.Value = "";
        //    }
        //    if (send == null)
        //    {              
        //        send = new byte[] { };
        //    }
        //    if (result == null)
        //    {
        //        result = new byte[] { };
        //    }
        //    //
        //    if (errCode == AlarmType.Err_Tx)
        //    {
        //        UIRst.Result.Add("连接端口失败;" + command.HandledBY + ";" + "" + ";" +
        //        "" + ";" + UIRst.Update.ToString("yyyy-MM-dd HH:mm:ss fff") + ";" +
        //        "Send:" + BitConverter.ToString(send) + " Result:" + BitConverter.ToString(result));
        //    }
        //    else if (errCode == AlarmType.Err_Pz)
        //    {
        //        UIRst.Result.Add("设备类型错误;" + command.ModuleType + ";" + "" + ";" +
        //        "" + ";" + UIRst.Update.ToString("yyyy-MM-dd HH:mm:ss fff") + ";" +
        //        "Send:" + BitConverter.ToString(send) + " Result:" + BitConverter.ToString(result));
        //    }
        //    else if (errCode == AlarmType.Err_Nd)
        //    {
        //        UIRst.Result.Add("没有定义此功能码;" + command.FunType + ";" + "" + ";" +
        //        "" + ";" + UIRst.Update.ToString("yyyy-MM-dd HH:mm:ss fff") + ";" +
        //        "Send:" + BitConverter.ToString(send) + " Result:" + BitConverter.ToString(result));
        //    }
        //    else
        //    {
        //        string funName = command.FunName;
        //        string lpName = command.LpszDbVarName;
        //        if (string.IsNullOrEmpty(command.FunName))
        //            funName = CommFunc.GetEnumDisplay(typeof(V0Fun), command.FunType);
        //        if (string.IsNullOrEmpty(lpName))
        //            lpName = command.ModuleAddr;
        //        UIRst.Result.Add(funName + ";地址:" + lpName + ";值:" + UIRst.Value + ";通信方式:" +
        //        UIRst.BiludBY + ";" + UIRst.Update.ToString("yyyy-MM-dd HH:mm:ss fff") + ";" +
        //        UIRst.ErrTxt + "Send:" + BitConverter.ToString(send) + " Result:" + BitConverter.ToString(result));
        //    }
        //    NCSys.UIResult.Enqueue(UIRst);/*页面输入*/
        //    #endregion
        //}

        ///// <summary>
        ///// 更新下发命令
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="lpszVal"></param>
        ///// <param name="lpszdateTime"></param>
        ///// <param name="errCode"></param>
        ///// <param name="errTxt"></param>
        //private static void DbCmdRefresh(CommandVModel command, string lpszVal, DateTime lpszdateTime, AlarmType errCode, string errTxt)
        //{
        //    if (command == null || lpszdateTime.Year < 2000) return;

        //    if (command.Log_id != 0)
        //    {
        //        processBll.UpdataCmd(command.Log_id, (int)errCode, errTxt);
        //    }
        //    else
        //    {
        //        if (command.IsNDb == true)
        //        { /*更新记录*/
        //            processBll.AddCmdLog(command.Module_id, command.Fun_id, command.FunType, command.CollectTime.Value, command.DataValue, command.Descr, (int)errCode, errTxt);
        //        }
        //    }
        //}
        ///// <summary>
        ///// 记录恶性负载
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="cTime"></param>
        ///// <param name="lpszVal"></param>
        //private static void DbEventWpf(string mkey, CommandVModel command, string lpszVal, DateTime lpszdateTime, AlarmType errCode, string errTxt)
        //{
        //    if (lpszVal == null || lpszdateTime.Year < 2000) return;
        //    if (!command.FunType.Equals(V0Fun.EventWpf.ToString())) return;
        //    RstVar rst = MemcachedMgr.GetVal<RstVar>(mkey);
        //    bool isAdd = true;
        //    if (rst != null && !string.IsNullOrEmpty(rst.lpszVal))
        //    {
        //        MalignantVModel pp = JsonHelper.Deserialize<MalignantVModel>(rst.lpszVal);
        //        if (pp.CTime == lpszdateTime) isAdd = false;
        //    }
        //    if (isAdd == true)
        //    { /*增加到数据库中*/
        //        processBll.AddAlarmOnCollect(command.Co_id, command.Module_id, command.ModuleAddr, command.FunType, command.Fun_id, lpszVal, (int)errCode, errTxt, lpszdateTime);
        //    }
        //}

        ///// <summary>
        ///// 增加告警到数据库
        ///// </summary>
        ///// <param name="lpszDbVarName"></param>
        ///// <param name="lpszVal"></param>
        ///// <param name="lpszdateTime"></param>
        ///// <param name="errCode"></param>
        ///// <param name="errTxt"></param>
        //private static void DbAddAlarm(string lpszDbVarName, string lpszVal, DateTime lpszdateTime, AlarmType errCode, string errTxt,bool isSend)
        //{
        //    StringBuilder strContent = new StringBuilder();
        //    DataTable dtSource = processBll.GetMapInfo(lpszDbVarName);
        //    if (dtSource.Rows.Count != 1)
        //    {
        //        FileLog.WriteLog("变量:"+lpszDbVarName+"不存在映射表中");
        //        return;
        //    }
        //    int module_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Module_id"]);
        //    int co_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Co_id"]);
        //    int fun_id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Fun_id"]);
        //    string moduleAddr = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["ModuleAddr"]);
        //    string funType = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["FunType"]);

        //    if (funType.Equals(V0Fun.LeakAlarm.ToString()))
        //    {
        //        if (lpszVal.Substring(0, 12).Contains("1"))
        //        {
        //            int cnt = 0;
        //            foreach (char c in lpszVal.Substring(0, 12).ToCharArray())
        //            {
        //                string content = "";
        //                ++cnt;
        //                if (c.ToString().Equals("1"))
        //                {
        //                    if (cnt <= 8)
        //                        content = "漏电流" + cnt + "发生告警";
        //                    else
        //                        content = "温度" + (cnt - 8) + "发生告警";
        //                }
        //                if (!string.IsNullOrEmpty(content))
        //                {
        //                    if (!string.IsNullOrEmpty(strContent.ToString()))
        //                        strContent.Append(";");
        //                    strContent.Append(content);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        string content = "";
        //        System.Reflection.FieldInfo info = typeof(V0Fun).GetField(funType);
        //        if (info != null)
        //        {
        //            var obj = info.GetCustomAttributes(typeof(DisplayAttribute), false);
        //            if (obj != null)
        //            {
        //                foreach (DisplayAttribute md in obj)
        //                    content = md.Name;
        //            }
        //        }
        //        strContent.Append(content);
        //    }
        //    long log_id = processBll.AddAlarm(co_id, module_id, moduleAddr, errCode.ToString(), fun_id, strContent.ToString(), lpszVal, lpszdateTime, (int)errCode, errTxt);
        //    if (isSend == true)
        //    {/*发送告警*/
        //        SendHd(log_id, errCode, HdType.AL_Sms, errTxt, module_id);
        //        SendHd(log_id, errCode, HdType.AL_Email, errTxt, module_id);
        //    }
        //}

        //private static void SendHd(long log_id, AlarmType alarmType, HdType hdType, string content, int module_id)
        //{
        //    v2_alarm_logHandVModel hand = processBll.GetAlarmHand(log_id, hdType.ToString());
        //    hand.Content = content;

        //    if (hand.ErrCode == 1)
        //    {
        //        if (hand.HdTime.AddMinutes(Helper.SendSpan) > DateTime.Now)
        //            return;
        //        else
        //            hand.Aid = 0;/*新增一条记录*/
        //    }
        //    DataTable dtSource = processBll.GetSendInfo(module_id);
        //    List<string> mpReceipts = new List<string>();
        //    List<string> emReceipts = new List<string>();

        //    string coName = "", moduleName = "";
        //    foreach (DataRow drCrm in dtSource.Rows)
        //    {
        //        if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(drCrm["MPhone"])))
        //            mpReceipts.Add(CommFunc.ConvertDBNullToString(drCrm["MPhone"]));
        //        if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(drCrm["Email"])))
        //            emReceipts.Add(CommFunc.ConvertDBNullToString(drCrm["Email"]));
        //        coName = CommFunc.ConvertDBNullToString(drCrm["CoName"]);
        //        moduleName = CommFunc.ConvertDBNullToString(drCrm["ModuleName"]);
        //    }
        //    bool sue = false;
        //    string msg = "";
        //    try
        //    {
        //        if (hdType == HdType.AL_Sms)
        //        {
        //            if (mpReceipts.Count > 0)
        //            {
        //                if (alarmType == AlarmType.Al_Use)
        //                {/*感知万物演示用*/
        //                    string ccc = JsonHelper.Serialize(new { moduleName = moduleName });
        //                    if (!string.IsNullOrEmpty(SmsFun.ComSmsConfig.ComSms))
        //                        sue = SmsFun.SendMessage(mpReceipts, ccc, out msg);
        //                    else if (!string.IsNullOrEmpty(SmsFun.AliSmsConfig.AliAppKey))
        //                        sue = SmsFun.SmsMessage(mpReceipts, ccc, out msg, "SMS_137657141");
        //                    else
        //                        msg = "没有发现短信接口";
        //                }
        //                else
        //                {
        //                    msg = "感知演示,暂不开放";
        //                }
        //            }
        //            else
        //            {
        //                msg = "无手机号码";
        //            }
        //        }
        //        else if (hdType == HdType.AL_Email)
        //        {
        //            if (mpReceipts.Count > 0)
        //            {
        //                StringBuilder mailcont1 = new StringBuilder();
        //                mailcont1.Append("你好:" + coName);
        //                mailcont1.Append("<br>");
        //                mailcont1.Append("告警内容:" + hand.Content);
        //                mailcont1.Append("<br>");
        //                mailcont1.Append("告警时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo));
        //                YDS6000.Models.EmailUtilities.SendMail("告警通知", mailcont1.ToString(), "", emReceipts, null, null);
        //                sue = true;
        //            }
        //            else
        //            {
        //                msg = "无邮箱地址";
        //            }
        //        }
        //        hand.ErrCode = sue == true ? 1 : -1;
        //    }
        //    catch (Exception ex)
        //    {
        //        hand.ErrCode = -1;
        //        msg = ex.Message;
        //    }
        //    hand.ErrTxt = msg;

        //    processBll.UpAlarmHand(hand);
        //}
    }
}
