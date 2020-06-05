using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using YDS6000.BLL.DataProcess;
using YDS6000.Models;

namespace DataProcess
{
    public class ZpSi
    {
        private ZpSiBLL bll = new ZpSiBLL(Config.Systems, Config.Uid);
        #region 定时拉合闸
        public void Run()
        {
            DateTime? dtRun = null, dtTab = null;
            DataTable dtSi = null;
            while (true)
            {
                Thread.Sleep(1000);/*每一秒*/
                if (NCSys.IsRun == false)
                {
                    dtSi = null; dtTab = null;
                    continue;
                }
                DateTime Now = DateTime.Now;
                if (dtRun != null)
                { /*以分钟为一个单位执行*/
                    if (dtRun.Value.Minute == Now.Minute)
                        continue;
                }
                dtRun = DateTime.Now;
                //
                try
                {
                    if (NCSys.ResetSiTime == false || dtSi == null || dtTab == null || dtTab.Value.AddHours(1) < DateTime.Now)
                    {
                        dtSi = bll.GetV1_si_ssr();
                        dtTab = DateTime.Now;
                        NCSys.ResetSiTime = true;
                    }
                    foreach (DataRow dr in dtSi.Rows)
                    {
                        int ledger = CommFunc.ConvertDBNullToInt32(dr["Ledger"]);
                        int si_id = CommFunc.ConvertDBNullToInt32(dr["Si_id"]);
                        int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
                        int module_id = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);
                        string moduleAddr = CommFunc.ConvertDBNullToString(dr["ModuleAddr"]);
                        string moduleType = CommFunc.ConvertDBNullToString(dr["ModuleType"]);
                        //
                        int meter_id = CommFunc.ConvertDBNullToInt32(dr["Meter_id"]);
                        string meterAddr = CommFunc.ConvertDBNullToString(dr["MeterAddr"]);
                        int ssr = IsRunSi(Now, dr);
                        //
                        if (ssr == 0) continue;
                        bool isQf = false;

                        //if (moduleType.Equals("YD-ZB20-4PL2-SB"))
                        //{/*这款用于河北工业大学的SB设备，特殊定制化*/
                        //    isQf = bll.GetMeterQianFei(ledger,co_id, meter_id, meterAddr);/*是否欠费*/
                        //}
                        //else
                        //{
                            isQf = bll.GetModuleQianFei(ledger,co_id, module_id, moduleAddr);/*是否欠费*/
                        //}


                        V0Fun fun = V0Fun.Ssr;
                        int fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]);
                        string funName = "定时合闸";
                        int nowSsr = 0;
                        string descr = "定时合闸";
                        string dataVale = "0";
                        if (ssr == 3355)
                        {
                            fun = V0Fun.Ssr;
                            fun_id = 0;
                            funName = "定时拉闸";
                            nowSsr = 1;
                            dataVale = "1";
                            descr = "定时拉闸";
                        }
                        //////////////
                        SysPro pro;
                        NCSys.Pro.TryGetValue(ledger, out pro);
                        if (pro == null) continue;
                        string key = pro.ProjectKey + CommFunc.ConvertDBNullToString(dr["LpszDbVarName"]);
                        RstVar rstVar = MemcachedMgr.GetVal<RstVar>(key);
                        int curSsr = -1;
                        if (rstVar != null)
                            curSsr = CommFunc.ConvertDBNullToInt32(rstVar.lpszVal);
                        //if (nowSsr == curSsr) continue;
                        ///////////////////////////////////////////////////////////////////////////////////////////
                        if (ssr == 9966)
                        {/*合闸*/
                            if (isQf == true) continue;/*欠费不合闸*/
                        }
                        if (nowSsr != curSsr)
                        {/*不一致执行命令*/
                            //CommandVModel cmd = bll.GetSsrInfo(ledger, module_id, dataVale);
                            CommandVModel cmd = null;
                            int transferType = bll.GetTransferType(ledger, module_id);
                            if (transferType == 4)
                                cmd = bll.GetSsrInfo(ledger, module_id, V0Fun.Ssr.ToString() + dataVale, dataVale);
                            else
                                cmd = bll.GetSsrInfo(ledger, module_id, V0Fun.Ssr.ToString(), dataVale);
                            if (cmd == null)
                            {
                                FileLog.Error("定时操作没有数据");
                                continue;
                            }
                            cmd.Fun_id = fun_id;
                            cmd.FunType = fun.ToString();
                            cmd.FunName = funName;
                            cmd.DataValue = dataVale;
                            cmd.Descr = descr;
                            cmd.IsUI = true;
                            cmd.IsNDb = true;
                            cmd.Create_by = Config.Uid;
                            if (cmd.TransferType == 4)
                            {
                                //string fs = cmd.FunType + cmd.DataValue;
                                //string tagName = "", dataValue = "";
                                //tagName = bll.GetSSrIOService(ledger, module_id, fs, out dataValue);
                                //if (!string.IsNullOrEmpty(tagName) && !string.IsNullOrEmpty(dataVale))
                                //{
                                //    cmd.LpszDbVarName = tagName;
                                //    cmd.DataValue = dataVale;
                                //}
                                DataProcess.IOService.Collection.CollectionHelper.WriteVar(cmd);
                            }
                            else
                            {
                                DataProcess.YdDrive.Collection.CollectionHelper.Instance(cmd);
                            }
                            FileLog.Debug("电表ID:" + cmd.Module_id + "地址:" + cmd.ModuleAddr + "动作:" + cmd.Descr);
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    FileLog.Error("拉合闸策略错误:" + ex.Message + ex.StackTrace);
                }
            }
        }
        private static int IsRunSi(DateTime now, DataRow dr)
        {
            JsonSiModel siSSR = ModelHandler<JsonSiModel>.jsonTextToModel(CommFunc.ConvertDBNullToString(dr["SiSSR"]));
            JsonSiMdModel md = ModelHandler<JsonSiMdModel>.jsonTextToModel(CommFunc.ConvertDBNullToString(dr["Md"]));
            JsonSiWkModel wk = ModelHandler<JsonSiWkModel>.jsonTextToModel(CommFunc.ConvertDBNullToString(dr["Wk"]));
            JsonSiTsModel ts = ModelHandler<JsonSiTsModel>.jsonTextToModel(CommFunc.ConvertDBNullToString(dr["Ts"]));
            DateTime today = CommFunc.ConvertDBNullToDateTime(now.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo));
            int dWk = (int)now.DayOfWeek;
            if (dWk == 0) dWk = 7;
            int si = 0;
            if (si == 0)
            {/*特殊日期的策略*/
                for (int i = 1; i <= 7; i++)
                {
                    System.Reflection.PropertyInfo tsInfo = ts.GetType().GetProperty("ts" + i.ToString().PadLeft(2, '0').ToString());
                    JsonSiTsModel.Value value = (JsonSiTsModel.Value)tsInfo.GetValue(ts, null);
                    DateTime dtTs = DateTime.Now.AddDays(-300);
                    DateTime.TryParseExact(value.dt, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out dtTs);
                    int dtSi = CommFunc.ConvertDBNullToInt32(value.si);
                    if (dtTs == today && dtSi != 0)
                    {
                        si = dtSi;
                        break;
                    }
                }
            }
            if (si == 0)
            { /*星期的策略*/
                System.Reflection.PropertyInfo wkDis = wk.GetType().GetProperty("di" + dWk.ToString().PadLeft(2, '0').ToString());
                object objDis = wkDis == null ? null : wkDis.GetValue(wk, null);
                //
                if (CommFunc.ConvertDBNullToInt32(objDis) == 0)
                {
                    System.Reflection.PropertyInfo wkInfo = wk.GetType().GetProperty("si" + dWk.ToString().PadLeft(2, '0').ToString());
                    object obj = wkInfo.GetValue(wk, null);
                    si = CommFunc.ConvertDBNullToInt32(obj);
                }
            }
            if (si == 0)
            {/*日期范围*/
                for (int i = 1; i <= 7; i++)
                {
                    System.Reflection.PropertyInfo mdInfo = md.GetType().GetProperty("md" + i.ToString().PadLeft(2, '0').ToString());
                    JsonSiMdModel.Value value = (JsonSiMdModel.Value)mdInfo.GetValue(md, null);
                    DateTime dtMd = today.AddDays(100);
                    DateTime.TryParseExact(today.Year.ToString() + value.md, "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out dtMd);
                    int dtSi = CommFunc.ConvertDBNullToInt32(value.si);
                    if (today >= dtMd && dtSi != 0)
                    {
                        si = dtSi;
                        break;
                    }
                }
            }
            //
            if (si == 0)
                return 0;
            //
            int rstSr = 0;
            System.Reflection.PropertyInfo siInfo = siSSR.GetType().GetProperty("d" + si.ToString().PadLeft(2, '0').ToString());
            JsonSiModel.time time = (JsonSiModel.time)siInfo.GetValue(siSSR, null);
            for (int i = 8; i >= 1; i--)
            {
                System.Reflection.PropertyInfo tInfo = time.GetType().GetProperty("t" + i.ToString().PadLeft(2, '0').ToString());
                JsonSiModel.Value value = (JsonSiModel.Value)tInfo.GetValue(time, null);
                string[] arr = value.hm.Split(':');
                int hour = CommFunc.ConvertDBNullToInt32(arr[0]);
                int minute = CommFunc.ConvertDBNullToInt32(arr[1]);
                int sr = CommFunc.ConvertDBNullToInt32(value.sr);
                if (sr != 0 && (hour != 0 || minute != 0))
                {
                    DateTime siTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
                    if (now >= siTime)
                    {
                        rstSr = sr;
                        break;
                    }
                }
                //if (hour == now.Hour && minute == now.Minute && sr != 0)
                //{
                //    rstSr = sr;
                //    break;
                //}
            }
            return rstSr;

        }
        #endregion
    }
}
