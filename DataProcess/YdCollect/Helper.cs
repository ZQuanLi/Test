using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Concurrent;
using System.Threading;
using YDS6000.BLL.DataProcess;
using YDS6000.Models;

namespace DataProcess.YdCollect
{
    public class Helper
    {
        private static Helper help = null;
        private static string ProjectKey = Config.Project + "->" + Config.Ledger + "->";
        private static int FrMd = 15;         /// 入库频率
        private static DataProcessBLL processBll = new DataProcessBLL(Config.Ledger, Config.Uid);

        private Helper()
        {
            Thread th1 = new Thread((ThreadStart)delegate { this.Process(); });
            th1.Name = "DataProcess";
            th1.IsBackground = true;
            th1.Start();
            //
            try
            {
                ProcessSevice.Start(new ICEFactory(), Config.ListenPort);
            }
            catch (Exception ex)
            {
                FileLog.Error("启动通讯服务错误:" + ex.Message + ex.StackTrace);
            }
        }

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
                NCSys.Result.Clear();
                Helper.FrMd = 15;
                NCSys.Reset = false;
            }
        }

        private void Process()
        {
            while (true)
            {
                Thread.Sleep(1000);/*每一秒*/
                if (NCSys.IsRun == false) continue;

                try
                {
                    this.GetProcessOfConfig();
                }
                catch (Exception ex)
                {
                    FileLog.Error("读取采集配置数据错误:" + ex.Message + ex.StackTrace);
                }
                try
                {
                    foreach (var s0 in NCSys.Result)
                    {
                        CollectVModel s1 = s0.Value;
                        if (DateTime.Now < s1.UpTime.AddMinutes(Helper.FrMd)) continue;

                        RstVar rst = s1.RstVar;
                        if (rst == null) continue;
                        try
                        {
                            string msg;
                            int errcode = processBll.DataProcess(s1.Module_id, s1.ModuleAddr, s1.Fun_id, rst.lpszVal, rst.lpszdateTime, out msg);
                            if (errcode < 0)
                            {
                                FileLog.Error("KEY:" + s0.Key + "值:" + rst.lpszVal + "数据处理结果错误:" + msg);
                            }
                            FileLog.Debug("KEY:" + s0.Key + "值:" + rst.lpszVal + "保存成功");         
                        }
                        catch (Exception ex)
                        {
                            FileLog.Error("KEY:" + s0.Key + "值:" + rst.lpszVal + "采集时间:" + rst.lpszdateTime.ToString() + "数据处理结果错误:" + ex.Message);
                        }
                        s1.UpTime = DateTime.Now;/*入库时间*/
                    }
                }
                catch (Exception ex)
                {
                    FileLog.Error("数据处理错误:" + ex.Message + ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// 保存采集项数据
        /// </summary>
        /// <param name="lpszDbVarName"></param>
        /// <param name="lpszVal"></param>
        /// <param name="lpszdateTime"></param>
        internal static void SaveCollectCache(string lpszDbVarName, string lpszVal, DateTime lpszdateTime)
        {
            string mkey = Helper.ProjectKey + lpszDbVarName;
            CollectVModel collect = null;
            if (NCSys.Result.TryGetValue(mkey, out collect) == false)
            {
                try
                {
                    collect = new CollectVModel();
                    System.Data.DataTable dtSource = processBll.GetMapInfo(lpszDbVarName);
                    if (dtSource.Rows.Count != 1)
                    {
                        FileLog.Debug("KEY:" + lpszDbVarName + "值信息不存在数据库中");
                        return;
                    }
                    collect.RstVar = new RstVar() { dwUserData = 0 };
                    foreach (DataRow dr in dtSource.Rows)
                    {
                        collect.Module_id = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);
                        collect.ModuleAddr = CommFunc.ConvertDBNullToString(dr["ModuleAddr"]);
                        collect.Fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]);
                        collect.UpTime = CommFunc.ConvertDBNullToDateTime(dr["LastTime"]);
                        collect.RstVar.lpszVal = dr["LastVal"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(dr["LastVal"]).ToString();
                        collect.RstVar.lpszdateTime = collect.UpTime;
                    }
                    NCSys.Result.TryAdd(mkey, collect);
                }
                catch (Exception ex)
                {
                    FileLog.Debug("获取DB数据错误:" + ex.Message + ex.StackTrace);
                    return;/*采集值反转*/
                }
            }
            if (CommFunc.ConvertDBNullToDateTime(collect.RstVar.lpszdateTime) > lpszdateTime)
            {
                FileLog.Debug("KEY:" + lpszDbVarName + "值采集时间反转");
                return;/*时间反转*/
            }
            if (CommFunc.ConvertDBNullToDecimal(collect.RstVar.lpszVal) > CommFunc.ConvertDBNullToDecimal(lpszVal))
            {
                FileLog.Debug("KEY:" + lpszDbVarName + "值采集数据反转");
                return;/*采集值反转*/
            }
            collect.RstVar.lpszVal = lpszVal;
            collect.RstVar.lpszdateTime = lpszdateTime;
            collect.RstVar.dwUserData = 0;
        }
    }
}
