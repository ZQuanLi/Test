using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Linq;
using System.Text;
using YADA.Utilities;
using YDS6000.Models;

namespace DataProcess.YdDrive.Collection
{
    public class CollectionHelper
    {
        /// <summary>
        /// 线程锁
        /// </summary>
        private static Object wholeLocker = new Object();
        /// <summary>
        /// 线程个数
        /// </summary>
        private static int ThreadCnt = 0;/*线程个数*/
        //
        private static ConcurrentDictionary<string, CollectionHelper> instance = new ConcurrentDictionary<string, CollectionHelper>();

        private static bool isCollect = false;

        /// <summary>
        /// 线程是否运行
        /// </summary>
        private bool threadRun = false;

        // 本次命令
        private SortedList<ulong, CommandVModel> ThreadCmd = new SortedList<ulong, CommandVModel>();
        /// <summary>
        /// 本对象线程锁
        /// </summary>
        private Object locker = new Object();

        private Thread thread;
        /// <summary>
        /// 生成唯一的命令ID号
        /// </summary>
        private ulong CmdId = 0;
        //
        private CollectionHelper()
        {
            if (isCollect == true) return;
            isCollect = true;
            Thread th0 = new Thread((ThreadStart)delegate { this.CollectManage(); });
            th0.Name = "CollectManage";
            th0.IsBackground = true;
            th0.Start();
        }

        //参数
        // 采集器ID号
        private string HandledBY;
        private int Transfer;
        /// <summary>
        /// 网络设备资源
        /// </summary>
        private NetworkResource Network;

        /// <summary>
        /// 采集项目管理
        /// </summary>
        private void CollectManage()
        {
            while (true)
            {
                Thread.Sleep(1000);/*每一秒*/
                if (NCSys.IsRun == false) continue;
                try
                {
                    bool bIsNext = true;
                    var enumer = DataProcess.YdDrive.Collection.CollectionHelper.instance.GetEnumerator();
                    while (bIsNext == true)
                    {
                        Thread.Sleep(500);
                        bIsNext = enumer.MoveNext();
                        if (bIsNext == false) break;
                        DataProcess.YdDrive.Collection.CollectionHelper threadHelper = enumer.Current.Value;
                        if (threadHelper.threadRun == true) continue;
                        if (DataProcess.YdDrive.Collection.CollectionHelper.ThreadCnt <= 20)
                        {/*默认 30 个线程*/
                            lock (DataProcess.YdDrive.Collection.CollectionHelper.wholeLocker)
                            {
                                threadHelper.threadRun = true;
                                threadHelper.thread = new Thread((ThreadStart)delegate { threadHelper.Run(); });
                                threadHelper.thread.Name = enumer.Current.Key;
                                threadHelper.thread.IsBackground = true;
                                threadHelper.thread.Start();
                                DataProcess.YdDrive.Collection.CollectionHelper.ThreadCnt = DataProcess.YdDrive.Collection.CollectionHelper.ThreadCnt + 1;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLog.Error("启动采集线程错误:" + ex.Message + ex.StackTrace);
                }
            }
        }

        //初始化数据
        internal static CollectionHelper Instance(CommandVModel command)
        {
            CollectionHelper ThreadHelper = null;
            try
            {
                V0Fun vfun ;
                int priority = 0;
                if (Enum.TryParse<V0Fun>(command.FunType, out vfun) == true)
                    priority = (int)vfun;
                ulong rid = 0;
                bool isRun = true;
                //命令功能运行次序(数字越小越先级越大)(内容来自FunPriority枚举) FirstDefine = 10 设置参数 ； SecondSsr0or1
                lock (CollectionHelper.wholeLocker)
                {
                    if (CollectionHelper.instance.TryGetValue(command.HandledBY, out ThreadHelper) == false)
                    {
                        ThreadHelper = new CollectionHelper();
                        ThreadHelper.HandledBY = command.HandledBY;
                        ThreadHelper.Transfer = command.TransferType;
                        ThreadHelper.Network = new NetworkResource(command.HandledBY, command.TransferType, command.TimeOut);
                        if (CollectionHelper.instance.TryAdd(command.HandledBY, ThreadHelper) == false)
                        {
                            isRun = false;
                            FileLog.Error("点：" + command.LpszDbVarName + "命令," + "队列：" + command.HandledBY + "新增内存失败");
                        }
                    }
                    if (isRun == true)
                    {
                        ThreadHelper.CmdId = ThreadHelper.CmdId + 1;
                        rid = (command.IsUI == false ? (ulong)(priority * 100000000) : (ulong)(priority * 10000)) + ThreadHelper.CmdId;
                        if (command.IsUI == false)
                            NCSys.BackgroundCount = NCSys.BackgroundCount + 1;
                        lock (ThreadHelper.locker)
                        {
                            ThreadHelper.ThreadCmd.Add(rid, command);
                        }
                        if (ThreadHelper.threadRun == false && command.IsUI == true)
                        {
                            isRun = true;
                            ThreadHelper.threadRun = true;
                            ThreadHelper.thread = new Thread((ThreadStart)delegate { ThreadHelper.Run(); });
                            ThreadHelper.thread.Name = command.HandledBY;
                            ThreadHelper.thread.IsBackground = true;
                            ThreadHelper.thread.Start();
                            CollectionHelper.ThreadCnt = CollectionHelper.ThreadCnt + 1;
                        }
                    }
                    //ThreadHelper.CmdId = ThreadHelper.CmdId + 1;
                    //ulong rid = (command.IsUI == false ? (ulong)(priority * 100000000) : (ulong)(priority * 10000)) + ThreadHelper.CmdId;
                    //lock (ThreadHelper.locker)
                    //{
                    //    if (command.IsUI == false)
                    //        NCSys.BackgroundCount = NCSys.BackgroundCount + 1;
                    //    ThreadHelper.ThreadCmd.Add(rid, command);
                    //}
                    ////if (isAdd == true)//启动线程
                    //if (ThreadHelper.threadRun == false && command.IsUI == true)
                    //{
                    //    ThreadHelper.threadRun = true;
                    //    Thread th = new Thread((ThreadStart)delegate { ThreadHelper.Run(); });
                    //    th.Name = command.HandledBY + "Th";
                    //    th.IsBackground = true;
                    //    th.Start();
                    //    CollectionHelper.ThreadCnt = CollectionHelper.ThreadCnt + 1;
                    //}
                }
            }
            catch (Exception ex)
            {
                FileLog.Error("点：" + command.LpszDbVarName + "命令," + "队列：" + command.HandledBY + "反复错误次数:" + command.ErrCnt + "新增错误:" + ex.Message + ex.StackTrace);
            }
            return ThreadHelper;
        }

        /// <summary>
        /// 线程运行
        /// </summary>
        internal void Run()
        {
            Thread.Sleep(100);
            DateTime runningTime = DateTime.Now;/*有命令的运行时间*/
            #region 打开连接
            bool isOnLine = false;
            try
            {
                isOnLine = this.Network.Open(3);/*打开端口*/
            }
            catch
            {
                FileLog.WriteLog("处理ID号:" + this.HandledBY + " 方式:" + this.Transfer + " 打开失败");
                isOnLine = false;
            }
            #endregion
            bool restCache = true;
            int errTime = 0;            
            while (true)
            {
                Thread.Sleep(500);
                CommandVModel cmd = null;
                #region 获取一个命令
                try
                {
                    lock (this.locker)
                    {
                        foreach (KeyValuePair<ulong, CommandVModel> item in this.ThreadCmd.Take(1))
                        {
                            cmd = item.Value;
                            ThreadCmd.Remove(item.Key);//移除命令
                        }
                    }
                    
                    lock (CollectionHelper.wholeLocker)
                    {
                        if (cmd != null && cmd.IsUI == false)
                            NCSys.BackgroundCount = NCSys.BackgroundCount - 1;
                    }

                    if (cmd != null && isOnLine == false)
                    {/*连接不上*/
                        //if (restCache==true)
                        //    Helper.SaveCollectCache(cmd, null, new DateTime(1900, 1, 1), false, AlarmType.Err_Tx, "连接错误", null, null);
                        if (restCache == true)
                            DataProcess.YdProcess.Helper.YdDriveSaveCollectCache(cmd, "", DateTime.Now, 0, false, AlarmType.Err_Tx, "连接错误", null, null);
                        restCache = false;
                    }
                    errTime = 0;
                }
                catch (Exception ex)
                {
                    if (errTime <= 5)
                        FileLog.Error("读取内存数据错误:" + ex.Message + ex.StackTrace);
                    ++errTime;
                }
                #endregion
                //////////////////////////////////////////////////////////////////////
                #region 处理命令
                if (cmd != null && isOnLine==true)
                {
                    if (NCSys.IsRun == true)
                    {
                        try
                        {
                            this.HandleCommand(cmd, this.Network);
                            errTime = 0;
                        }
                        catch (Exception ex)
                        {
                            if (errTime <= 5)
                                FileLog.Error("ID:" + cmd.Module_id + "地址:" + cmd.ModuleAddr + "采集码:" + cmd.FunType + "采集ID:" + cmd.Fun_id + "采集名:" + cmd.FunName + "命令运行错误:" + ex.Message + ex.StackTrace);
                            ++errTime;
                        }
                    }
                }
                #endregion
                #region 移除资源

                int cnt = this.ThreadCmd.Count;
                if (cnt == 0)
                {
                    Thread.Sleep(3000);/*等3秒*/
                    lock (CollectionHelper.wholeLocker)
                    {
                        cnt = this.ThreadCmd.Count;
                        if (cnt == 0)
                        {
                            try
                            {
                                #region 移除资源
                                CollectionHelper.ThreadCnt = CollectionHelper.ThreadCnt - 1;
                                CollectionHelper del = null;
                                this.Network.RemoveInstance();/*关闭端口并移除出内存*/
                                int i = 0;
                                while (++i <= 5)
                                {
                                    if (CollectionHelper.instance.TryRemove(this.HandledBY, out del) == false)
                                    {
                                        FileLog.WriteLog("处理ID号:" + this.HandledBY + "第" + i + "次移除命令失败");
                                        Thread.Sleep(1000);
                                    }
                                    else
                                    {
                                        //FileLog.WriteLog("处理ID号:" + this.HandledBY + "第" + i + "移除命令成功");
                                        break;
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                FileLog.WriteLog("移除资源错误:" + ex.Message + ex.StackTrace);
                            }
                            break;
                        }
                    }
                }
                #endregion
            }
        }
        private void HandleCommand(CommandVModel command, NetworkResource Network)
        {
            if (Network == null) {
                FileLog.Error("ID:" + command.Module_id + "地址:" + command.ModuleAddr + "采集码:" + command.FunType + "采集ID:" + command.Fun_id + "采集名:" + command.FunName + "网络资源错误");
                return;
            }
            string msg = "";
            byte[] send = null, result = null;
            string lpszVal = null;
            AlarmType errCode = AlarmType.Sue;
            bool rst = false;
            command.CollectTime = DateTime.Now;
            DateTime lpszdateTime =  DateTime.Now;

            if (command.FunType.Equals(V0Fun.E.ToString()))/*有功总电能*/
            {
                rst = Drive.ARM(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.RdVal.ToString()))/*当前电表剩余电量*/
            {
                rst = Drive.RdVal(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Ia.ToString()))/*当前电表剩余电量*/
            {
                rst = Drive.GetIa(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Ib.ToString()))
            {
                rst = Drive.GetIb(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Ic.ToString()))
            {
                rst = Drive.GetIc(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Ua.ToString()))
            {
                rst = Drive.GetUa(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Ub.ToString()))
            {
                rst = Drive.GetUb(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Uc.ToString()))
            {
                rst = Drive.GetUc(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Psum.ToString()))
            {
                rst = Drive.GetPsum(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Pa.ToString()))
            {
                rst = Drive.GetPa(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Pb.ToString()))
            {
                rst = Drive.GetPb(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Pc.ToString()))
            {
                rst = Drive.GetPc(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Qsum.ToString()))
            {
                rst = Drive.GetQsum(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Qa.ToString()))
            {
                rst = Drive.GetQa(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Qb.ToString()))
            {
                rst = Drive.GetQb(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Qc.ToString()))
            {
                rst = Drive.GetQc(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Pfav.ToString()))
            {
                rst = Drive.GetPfav(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.PFa.ToString()))
            {
                rst = Drive.GetPFa(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.PFb.ToString()))
            {
                rst = Drive.GetPFb(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.PFc.ToString()))
            {
                rst = Drive.GetPFc(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Ssr.ToString()) && string.IsNullOrEmpty(command.DataValue))/*拉合闸状态*/
            {
                rst = Drive.Ssr(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Ssr.ToString()) && CommFunc.ConvertDBNullToInt32(command.DataValue) == 0)/*合闸*/
            {
                rst = Drive.Ssr0(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Ssr.ToString()) && CommFunc.ConvertDBNullToInt32(command.DataValue) == 1)/*拉闸*/
            {
                rst = Drive.Ssr1(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Pay.ToString()))/*充值*/
            {
                rst = Drive.Pay(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Refund.ToString()))/*退费*/
            {
                rst = Drive.Refund(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.PaySb1.ToString()))/*充值*/
            {
                rst = Drive.PaySb1(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.PaySb2.ToString()))/*充值*/
            {
                rst = Drive.PaySb2(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.IsPaul.ToString()) && string.IsNullOrEmpty(command.DataValue))/*保电状态*/
            {
                rst = Drive.SetPaul(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.IsPaul.ToString()) && !string.IsNullOrEmpty(command.DataValue))/*保电设置*/
            {
                rst = Drive.SetPaul(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Nrp.ToString()))/*有功功率负荷限值(kW)*/
            {
                rst = Drive.SetNrp(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.McNrp.ToString()))/*恶性负载判定有功功率增量(kW)(瞬时额定功率)*/
            {
                rst = Drive.SetMcNrp(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Wpf.ToString()))/*恶性负载判定无功功率(kW)*/
            {
                rst = Drive.SetWpf(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Uep.ToString()))/*违规用电判定功能的功率下限(kW)*/
            {
                rst = Drive.SetUep(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Miss.ToString()))/*一天内违规断电次数限值(0~255)*/
            {
                rst = Drive.SetMiss(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Rest.ToString()))/*违规断电后重启时间(S)*/
            {
                rst = Drive.SetRest(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Saf.ToString()))/*单次功率增量限值(kW)*/
            {
                rst = Drive.SetSaf(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Avlm.ToString()))/*过流门限值(A)*/
            {
                rst = Drive.SetAvlm(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Phase.ToString()))/*启用移相器监测功率下限(kW)*/
            {
                rst = Drive.SetPhase(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.Resist.ToString()))/*设置阻行负载(kW)*/
            {
                rst = Drive.SetResist(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.IsPhase.ToString()))/*移相器(=1启用)*/
            {
                rst = Drive.SetIsPhase(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.IsRelay.ToString()))/*继电器参数(=0电平式A路继电器设置;=1电平式B路继电器设置;=127脉冲式设置)*/
            {
                rst = Drive.SetIsRelay(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.IsStdby.ToString()))/*待机检测=0 不检测 =1 检测*/
            {
                rst = Drive.SetStandby(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Equals(V0Fun.TiVal.ToString()))/*获取定时断送电表*/
            {
                rst = Drive.SetTiVal(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.ToLower().Length == 5 && command.FunType.ToLower().Contains("leak") && CommFunc.ConvertDBNullToInt32(command.FunType.Substring(command.FunType.Length - 1, 1)) > 0)/*漏电流*/
            {
                rst = Drive.GetMeterLeak(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.ToLower().Length == 5 && command.FunType.ToLower().Contains("temp") && CommFunc.ConvertDBNullToInt32(command.FunType.Substring(command.FunType.Length - 1, 1)) > 0)/*漏电流*/
            {
                rst = Drive.GetMeterTemp(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.ToLower().Length == 8 && command.FunType.ToLower().Contains("maxleak") && CommFunc.ConvertDBNullToInt32(command.FunType.Substring(command.FunType.Length - 1, 1)) > 0)/*电流阀值*/
            {
                rst = Drive.SetMaxLeak(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.ToLower().Length == 8 && command.FunType.ToLower().Contains("maxtemp") && CommFunc.ConvertDBNullToInt32(command.FunType.Substring(command.FunType.Length - 1, 1)) > 0)/*温度阀值*/
            {
                rst = Drive.SetMaxTemp(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.ToLower().Length >= 8 && command.FunType.ToLower().Contains("StatusLeak".ToLower()) && CommFunc.ConvertDBNullToInt32(command.FunType.Substring(command.FunType.Length - 1, 1)) > 0)/*电流使能*/
            {
                rst = Drive.SetStatusLeak(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.ToLower().Length >= 8 && command.FunType.ToLower().Contains("StatusTemp".ToLower()) && CommFunc.ConvertDBNullToInt32(command.FunType.Substring(command.FunType.Length - 1, 1)) > 0)/*温度使能*/
            {
                rst = Drive.SetStatusTemp(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }
            else if (command.FunType.Contains(V0Fun.LeakAlarm.ToString()))/*漏电设备告警*/
            {
                rst = Drive.GetLeakAlarm(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }

            else if (command.FunType.Equals(V0Fun.Alarm1.ToString()))/*获取报警电量1限值*/
            {
                rst = Drive.GetAlarm1(command, Network, out send, out result, out lpszVal, out errCode, out msg);
            }


            else if (command.FunType.Equals(V0Fun.EventWpf.ToString()))/*记录恶性负载事件*/
            {
                #region 记录恶性负载事件
                Dictionary<string, string> list = new Dictionary<string, string>();
                rst = Drive.GetEventWpf(command, Network, out send, out result, out list, out errCode, out msg);
                if (rst == true)
                {
                    MalignantVModel mv = new MalignantVModel();
                    mv.CTime = new DateTime(1900, 1, 1);
                    foreach (var item in list)
                    {
                        if (item.Key.Equals("CTime"))
                            mv.CTime = CommFunc.ConvertDBNullToDateTime(item.Value);
                        else if (item.Key.Equals("SsrS"))
                            mv.Status = item.Value;
                        else if (item.Key.Equals("Ua"))
                            mv.Ua = CommFunc.ConvertDBNullToDecimal(item.Value);
                        else if (item.Key.Equals("Ia"))
                            mv.Ia = CommFunc.ConvertDBNullToDecimal(item.Value);
                        else if (item.Key.Equals("Psum1"))
                            mv.Psum1 = CommFunc.ConvertDBNullToDecimal(item.Value);
                        else if (item.Key.Equals("Psum2"))
                            mv.Psum2 = CommFunc.ConvertDBNullToDecimal(item.Value);
                        else if (item.Key.Equals("Psum3"))
                            mv.Psum3 = CommFunc.ConvertDBNullToDecimal(item.Value);
                        else if (item.Key.Equals("Psum4"))
                            mv.Psum4 = CommFunc.ConvertDBNullToDecimal(item.Value);
                    }
                    lpszVal = null;
                    lpszdateTime = mv.CTime;
                    if (mv.CTime.Year != 1900)
                    {
                        lpszVal = JsonHelper.Serialize(mv);
                        lpszdateTime = mv.CTime;
                    }
                }
                #endregion
            }
            else
            {
                errCode = AlarmType.Err_Nd;
            }

            try
            {
                DataProcess.YdProcess.Helper.YdDriveSaveCollectCache(command, lpszVal, lpszdateTime, 0, rst, errCode, msg, send, result);
            }
            catch (Exception ex)
            {
                FileLog.Error("处理ID号:" + this.HandledBY + "处理采集数据错误:" + ex.Message + ex.StackTrace);
            }         
        }

    } 
}
