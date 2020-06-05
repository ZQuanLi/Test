using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    partial class MonitorHelper
    {
        /// <summary>
        /// 获取建筑列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdCustOnCoInfoList()
        {
            APIRst rst = new APIRst();
            try
            {
                int total = 0;
                List<Treeview> dt = bll.GetYdCustOnCoInfoList(out total);
                object obj = new { total = total, rows = dt };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取组织树形结构错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取抄表数据监视列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdMonitorOnList(string strcName,string coName,string ssr,int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdMonitorOnList(strcName, coName, ssr, co_id);
                this.GetYdStatusOnLine(dtSource);
                int total = dtSource.Rows.Count;
                object rows = this.GetYdMonitorOnList(dtSource);
                object obj = new { total = total, rows = rows };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("系统监视-抄表数据监视(GetYdMonitorOnList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private void GetYdStatusOnLine(DataTable dtSource)
        {
            dtSource.Columns.Add("SsrS", typeof(System.String));
            foreach (DataRow dr in dtSource.Rows)
            {
                RstVar var = null;
                //RstVar varSsr = null;
                int frMd = CommFunc.ConvertDBNullToInt32(dr["FrMd"]);
                int mid = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);
                //int ssr = -1;
                string memcachKey = CommFunc.ConvertDBNullToString(dr["MemcachKey"]);
                string memcachKeySsr = memcachKey.Replace(".E", ".Ssr");
                string cachComm = CommFunc.ConvertDBNullToString(dr["CachComm"]);
                DateTime lastTime = CommFunc.ConvertDBNullToDateTime(dr["LastTime"]);
                frMd = frMd == 0 ? 15 : frMd;
                dr["OnLine"] = 1;//因演示需要修改,原来是0
                dr["Ssr"] = 0;//原来是-1
                dr["SsrS"] = "合闸";//原来是未知
                if (lastTime.AddDays(50) < DateTime.Now)
                    continue;
                #region 是否在线
                if (!string.IsNullOrEmpty(cachComm))
                {
                    var = null; int i = 0;
                    while (var == null && ++i < 2)
                    {
                        var = MemcachedMgr.GetVal<RstVar>(WebConfig.MemcachKey + cachComm);
                        if (var != null)
                            break;
                        System.Threading.Thread.Sleep(2);
                    }
                    if (var != null)
                        dr["OnLine"] = ((int)CommFunc.ConvertDBNullToDecimal(var.lpszVal) == 0) ? 1 : 0;
                }
                else if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(dr["MemcachKey"])))
                {
                    var = null; int i = 0;
                    while (var == null && ++i < 2)
                    {
                        var = MemcachedMgr.GetVal<RstVar>(WebConfig.MemcachKey + memcachKey);
                        if (var != null)
                        {
                            //FileLog.WriteLog("key:" + Common.GetMemcachKey + memcachKey + " Val:" + JsonHelper.Serialize(var));
                            break;
                        }
                        else
                        {
                            //FileLog.WriteLog("空值 key:" + Common.GetMemcachKey + memcachKey);
                            //FileLog.WriteLog("空值 SSr key:" + Common.GetMemcachKey + memcachKeySsr);
                        }
                        System.Threading.Thread.Sleep(2);
                    }
                    if (var != null)
                    {
                        DateTime callTime = var.lpszdateTime;
                        if (callTime >= DateTime.Now.AddMinutes(0 - frMd))
                            dr["OnLine"] = 1;
                    }
                }
                #endregion
                #region 拉合闸状态
                //if (mid == 228)
                //{
                //    FileLog.WriteLog("电能状态:" + WebConfig.MemcachKey + memcachKey);
                //    FileLog.WriteLog("拉合闸状态:" + WebConfig.MemcachKey + memcachKeySsr);
                //}
                if (!string.IsNullOrEmpty(memcachKeySsr))
                {                    
                    var = null; int i = 0;
                    while (var == null && ++i < 2)
                    {
                        var = MemcachedMgr.GetVal<RstVar>(WebConfig.MemcachKey + memcachKeySsr);
                        if (var != null)
                            break;
                        System.Threading.Thread.Sleep(2);
                    }
                    if (var != null)
                    {
                        int ssr = (int)CommFunc.ConvertDBNullToDecimal(var.lpszVal);
                        //ssr = ssr == 0 ? 0 : 1;
                        dr["Ssr"] = ssr;
                        if (ssr==0)
                            dr["SsrS"] = "合闸";
                        else if(ssr == 129)
                            dr["SsrS"] = "恶性负载跳闸";
                        else if (ssr == 130)
                            dr["SsrS"] = "总功率超限跳闸";
                        else if (ssr == 132)
                            dr["SsrS"] = "移相器或半波跳闸";
                        else if (ssr == 136)
                            dr["SsrS"] = "定时断电拉闸";
                        else if (ssr == 144)
                            dr["SsrS"] = "远程拉闸";
                        else if (ssr == 160)
                            dr["SsrS"] = "过流超限跳闸";
                        else if (ssr == 192)
                            dr["SsrS"] = "单次功率超限跳闸";
                        else
                            dr["SsrS"] = "跳闸";

                    }
                }
                #endregion
                //int i = 0;
                //int onLine = -1;
                //if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(dr["MemcachKey"])))
                //{
                //    while (var == null && ++i < 2)
                //    {
                //        var = MemcachedMgr.GetVal<RstVar>(WebConfig.MemcachKey + memcachKey);
                //        varSsr = MemcachedMgr.GetVal<RstVar>(WebConfig.MemcachKey + memcachKeySsr);
                //        if (var != null || varSsr != null)
                //        {
                //            //FileLog.WriteLog("key:" + WebConfig.MemcachKey + memcachKey + " Val:" + JsonHelper.Serialize(var));
                //            break;
                //        }
                //        else
                //        {
                //            //FileLog.WriteLog("空值 key:" + WebConfig.MemcachKey + memcachKey);
                //            //FileLog.WriteLog("空值 SSr key:" + WebConfig.MemcachKey + memcachKeySsr);
                //        }
                //        System.Threading.Thread.Sleep(2);
                //    }
                //}
                //onLine = 0;
                //if (var != null)
                //{
                //    DateTime callTime = var.lpszdateTime;
                //    if (callTime >= DateTime.Now.AddMinutes(0 - frMd))
                //        onLine = 1;
                //}
                //if (varSsr != null)
                //    ssr = CommFunc.ConvertDBNullToInt32(varSsr.lpszVal);
                //dr["OnLine"] = onLine;
                //dr["Ssr"] = ssr;
            }
        }

        private object GetYdMonitorOnList(DataTable dtSource)
        {
            var res1 = from s1 in dtSource.AsEnumerable()
                       select new
                       {
                           RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                           Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                           ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                           ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                           Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                           CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                           CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           Multiply = CommFunc.ConvertDBNullToDecimal(s1["Multiply"]).ToString("f2"),
                           LastVal = Convert.IsDBNull(s1["LastVal"]) ? "" : CommFunc.ConvertDBNullToDecimal(s1["LastVal"]).ToString("f2"),
                           CurVal = Convert.IsDBNull(s1["CurVal"]) ? "" : CommFunc.ConvertDBNullToDecimal(s1["CurVal"]).ToString("f2"),
                           RdVal = Convert.IsDBNull(s1["RdVal"]) ? "" : CommFunc.ConvertDBNullToDecimal(s1["RdVal"]).ToString("f2"),
                           SyZsVal = Convert.IsDBNull(s1["SyZsVal"]) ? "" : CommFunc.ConvertDBNullToDecimal(s1["SyZsVal"]).ToString("f2"),
                           LastTime = (CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).Year == 1900 || CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).Year == 0) ? "" : CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           Ssr = s1["Ssr"] == DBNull.Value ? -1 : CommFunc.ConvertDBNullToInt32(s1["Ssr"]),
                           SsrS = CommFunc.ConvertDBNullToString(s1["SsrS"]),
                           DayUseVal = "",
                           MthUseVal = "",
                           HandledBY = CommFunc.ConvertDBNullToString(s1["HandledBY"]),
                           OnLine = CommFunc.ConvertDBNullToInt32(s1["OnLine"]),
                           IsAlarm = CommFunc.ConvertDBNullToInt32(s1["IsAlarm"]),
                           RdAmt = (CommFunc.ConvertDBNullToDecimal(s1["RdVal"]) * CommFunc.ConvertDBNullToDecimal(s1["Price"])).ToString("f2"),
                           SyVal = (CommFunc.ConvertDBNullToDecimal(s1["RdVal"]) - CommFunc.ConvertDBNullToDecimal(s1["SyZsVal"])).ToString("f2"),
                           PicType = CommFunc.ConvertDBNullToString(s1["ModuleType"]).Contains("-NB") ? 1 : 0,
                           @checked = 0
                       };
            return res1.ToList();
        }
        /// <summary>
        /// 获取列表的的详细信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdMonitorOnInfo(int co_id,int module_id,string moduleAddr)
        {
            APIRst rst = new APIRst();
            try
            {
                decimal dayUseVal = bll.GetYdMonitorOnDayUseVal(co_id, module_id, moduleAddr);
                decimal mthUseVal = bll.GetYdMonitorOnMthUseVal(co_id, module_id, moduleAddr);                
                object obj = new { DayUseVal = dayUseVal.ToString("f2"), MthUseVal = mthUseVal.ToString("f2") };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("系统监视-抄表数据监视(GetYdMonitorOnList)", ex.Message + ex.StackTrace); 
            }
            return rst;
        }
        /// <summary>
        /// 获取图形数据
        /// </summary>
        public APIRst GetYdMonitorInDetailOnCharts(int co_id,int module_id,string moduleAddr,string dateType,string startTime)
        {
            APIRst rst = new APIRst();
            dateType = dateType.ToLower();
            string time_str = CommFunc.ConvertDBNullToString(startTime) + (dateType.Equals("month") ? "-01" : dateType.Equals("year") ? "-01-01" : "");
            DateTime start = CommFunc.ConvertDBNullToDateTime(time_str);
            DateTime end = start;
            if (dateType.Equals("hour"))
            {
                start = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
                end = new DateTime(start.Year, start.Month, start.Day, 23, 0, 0);
            }
            if (dateType.Equals("day"))
            {
                //start = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
                //end = new DateTime(start.Year, start.Month, start.Day, 23, 0, 0);
                start = new DateTime(start.Year, start.Month, start.Day);
                end = start.AddMonths(1).AddDays(-1);
            }
            if (dateType.Equals("month"))
            {
                //start = new DateTime(start.Year, start.Month, start.Day);
                //end = start.AddMonths(1).AddDays(-1);
                start = new DateTime(start.Year, start.Month, 1);
                end = start.AddYears(1).AddDays(-1);
            }
            if (dateType.Equals("year"))
            {
                start = new DateTime(start.Year, start.Month, 1);
                end = start.AddYears(1).AddDays(-1);
            }
            //if (string.IsNullOrEmpty(msg) && (co_id == 0 || start.Year == 1900))              
            //    return Json(new { rst = false, msg = "获取图形参数错误", data = "" }, JsonRequestBehavior.AllowGet);
            //
            try
            {
                DataTable dtSource = bll.GetYdMonitorInDetailOnChartsData(co_id, module_id, moduleAddr, start, end, dateType);
                string title = "";
                if (dateType.ToLower().Equals("hour"))
                    title = "能耗小时分析' yAxisName='单位:kWh";
                else if (dateType.ToLower().Equals("day"))
                    title = "能耗日分析' yAxisName='单位:kWh";
                else if (dateType.ToLower().Equals("month"))
                    title = "能耗月分析' yAxisName='单位:kWh";
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new { label = CommFunc.ConvertDBNullToString(s1["Time"]), value = s1["UseVal"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["UseVal"]).ToString() };
                object obj = new { title = title, data = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取监视图像界面数据错误：(GetYdMonitorInDetailOnCharts)", ex.Message + ex.StackTrace); 
            }
            return rst;
        }

        public APIRst GetYdMonitorInDetailOnBill(int co_id,int module_id,string moduleAddr,DateTime start,DateTime end)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdMonitorInDetailOnBill(co_id, start, end);
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               PayVal = CommFunc.ConvertDBNullToDecimal(s1["PayVal"]).ToString("f2"),
                               UseVal = CommFunc.ConvertDBNullToDecimal(s1["UseVal"]).ToString("f2"),
                               Month = CommFunc.ConvertDBNullToInt32(s1["Month"]),
                               MthPayVal = CommFunc.ConvertDBNullToDecimal(s1["MthPayVal"]).ToString("f2"),
                               MthUseVal = CommFunc.ConvertDBNullToDecimal(s1["MthUseVal"]).ToString("f2"),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取数据错误(GetYdMonitorInDetailOnBill):", ex.Message + ex.StackTrace); 
            }
            return rst;
        }

        /// <summary>
        /// 获取温湿度
        /// </summary>
        public APIRst GetTemperatureAndHumidity(string strcName,string coName)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetTemperatureAndHumidity(strcName, coName);
                int total = dtSource.Rows.Count;
                foreach (DataRow dr in dtSource.Rows)
                {
                    string lpszDbVar = CommFunc.ConvertDBNullToString(dr["LpszDbVar"]);
                    int leakScale = CommFunc.ConvertDBNullToInt32(dr["LeakScale"]);
                    int tempScale = CommFunc.ConvertDBNullToInt32(dr["TempScale"]);
                    RstVar rstVal = null, rstAlarm = null;

                    rstAlarm = MemcachedMgr.GetVal<RstVar>(WebConfig.MemcachKey + lpszDbVar + "LeakAlarm");

                    for (int i = 1; i <= 8; i++)
                    {
                        System.Threading.Thread.Sleep(10);
                        rstVal = MemcachedMgr.GetVal<RstVar>(WebConfig.MemcachKey + lpszDbVar + "Leak" + i);
                        if (rstVal != null)
                        {
                            dr["Leak" + i] = CommFunc.ConvertDBNullToDecimal(rstVal.lpszVal).ToString("f" + leakScale);
                            if (rstAlarm != null)
                            {
                                if (!string.IsNullOrEmpty(rstAlarm.lpszVal))
                                    if (rstAlarm.lpszVal.ToString().Substring((i - 1), 1) == "1")
                                        dr["Leak" + i] = CommFunc.ConvertDBNullToString(dr["Leak" + i]) + "(告警)";
                            }
                        }
                        if (i <= 2)
                        {
                            rstVal = MemcachedMgr.GetVal<RstVar>(WebConfig.MemcachKey + lpszDbVar + "Temp" + i);
                            if (rstVal != null)
                            {
                                dr["Temp" + i] = CommFunc.ConvertDBNullToDecimal(rstVal.lpszVal).ToString("f" + tempScale);
                                if (rstAlarm != null)
                                {
                                    if (!string.IsNullOrEmpty(rstAlarm.lpszVal))
                                        if (rstAlarm.lpszVal.ToString().Substring((i + 7), 1) == "1")
                                            dr["Temp" + i] = CommFunc.ConvertDBNullToString(dr["Temp" + i]) + "(告警)";
                                }
                            }
                        }
                    }
                }
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               LeakScale = CommFunc.ConvertDBNullToInt32(s1["LeakScale"]),
                               TempScale = CommFunc.ConvertDBNullToInt32(s1["TempScale"]),
                               Leak1 = CommFunc.ConvertDBNullToString(s1["Leak1"]),
                               Leak2 = CommFunc.ConvertDBNullToString(s1["Leak2"]),
                               Leak3 = CommFunc.ConvertDBNullToString(s1["Leak3"]),
                               Leak4 = CommFunc.ConvertDBNullToString(s1["Leak4"]),
                               Leak5 = CommFunc.ConvertDBNullToString(s1["Leak5"]),
                               Leak6 = CommFunc.ConvertDBNullToString(s1["Leak6"]),
                               Leak7 = CommFunc.ConvertDBNullToString(s1["Leak7"]),
                               Leak8 = CommFunc.ConvertDBNullToString(s1["Leak8"]),
                               Temp1 = CommFunc.ConvertDBNullToString(s1["Temp1"]),
                               Temp2 = CommFunc.ConvertDBNullToString(s1["Temp2"]),
                           };
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取数据错误(GetTemperatureAndHumidity):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 汇出excel
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdMonitorOnExport(string data)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = JsonHelper.ToDataTable(data);
                string path = GetYdMonitorOnExport(dtSource);
                rst.data = path;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取数据错误(GetYdMontionOnExport):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 控制列表
        /// </summary>
        /// <param name="module_id"></param>
        /// <returns></returns>
        public APIRst GetCtrlList(int module_id)
        {
            APIRst rst = new APIRst();
            try
            {
                List<object> list = new List<object>();
                DataTable dtSource = bll.GetCtrlList(module_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           group s1 by new { Alias = CommFunc.ConvertDBNullToString(s1["Alias"]) } into g1
                           select new
                           {
                               Alias = g1.Key.Alias,
                               LpszDbVarName = CommFunc.ConvertDBNullToString(g1.First()["LpszDbVarName"]),
                           };
                foreach (var o1 in res1)
                {
                    var res2 = from s1 in dtSource.AsEnumerable()
                               where CommFunc.ConvertDBNullToString(s1["Alias"]).Equals(o1.Alias)
                               select new
                               {
                                   Fun_id = CommFunc.ConvertDBNullToString(s1["Fun_id"]),
                                   FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               };
                    List<object> dispaly = new List<object>();
                    if (o1.Alias.Equals("Ssr"))
                    {
                        dispaly.Add(new { Value = 0, Descr = "合闸" });
                        dispaly.Add(new { Value = 1, Descr = "拉闸" });
                    }
                    list.Add(new { Alias = o1.Alias, TagName = o1.LpszDbVarName , Dispaly = dispaly, List = res2.ToList() });
                }
                rst.data = list;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获控制列表错误(GetCtrlList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 下发控制
        /// </summary>
        /// <param name="pStr"></param>
        /// <param name="fun_id"></param>
        /// <returns></returns>
        public APIRst SendCtrl(string pStr, int fun_id)
        {
            APIRst rst = new APIRst();
            YdToGw gw = new YdToGw(user.Ledger, user.Uid);
            try
            {
                foreach (string s in pStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    gw.YdToGwCmd(CommFunc.ConvertDBNullToInt32(s), fun_id);
                }
                rst.data = "";
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("批量下发命令错误(SendCtrl)", ex.Message + ex.StackTrace);
            }
            return rst;

        }
        private string GetYdMonitorOnExport(DataTable dtSource)
        {
            string fn = "/XTemp/监视数据报表.xls";
            string filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"/XTemp");
            if (System.IO.Directory.Exists(filePath) == false)
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            string filename = System.Web.Hosting.HostingEnvironment.MapPath(fn);
            if (System.IO.File.Exists(filename))/*先删除已存在的文件，再汇出Excel*/
                System.IO.File.Delete(filename);
            if (dtSource == null || dtSource.Rows.Count == 0)
                throw new Exception("没有数据");

            Excel.ExcelCellStyle columnCellStyle0 = new Excel.ExcelCellStyle();
            columnCellStyle0 = new Excel.ExcelCellStyle()
            {
                DataFormart = "0.00",
                HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT
            };
            Excel.ExcelCellStyle columnCellStyle1 = new Excel.ExcelCellStyle();
            columnCellStyle1 = new Excel.ExcelCellStyle()
            {
                DataFormart = "yyyy-MM-dd HH:mm:ss",
            };
            dtSource.Columns.Add("OnLineS", typeof(System.String));
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["OnLineS"] = CommFunc.ConvertDBNullToInt32(dr["OnLine"]) == 1 ? "正常" : "异常";
            }
            Excel.ExcelColumnCollection columns = new Excel.ExcelColumnCollection();
            columns.Add(new Excel.ExcelColumn("序号", "RowId", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("建筑", "CoStrcName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("房间", "CoName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("通信方式", "HandledBY", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("电表地址", "ModuleAddr", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("表码值", "CurVal", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("表剩余电量", "RdVal", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("在线状态", "OnLineS", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("最后通信时间", "LastTime", 15) { IsSetWith = true, DefaultExcelCellStyle = columnCellStyle1 });
            //columns.Add(new Excel.ExcelColumn("E", "E", 15) { IsSetWith = true });
            Excel.ExcelOparete excel = new Excel.ExcelOparete("监视数据报表");
            excel.SetObjectValue("监视数据报表", 0, 0, 3);
            excel.SetColumnName(columns, 1, 0);
            excel.SetColumnValue(columns, dtSource.Select(), 2, 0);
            excel.SaveExcelByFullFileName(filename);
            return fn;
        }

        //
        public APIRst GetYdMonitorOfControl(string cmd,int module_id)
        {
            APIRst rst = new APIRst();
            if (string.IsNullOrEmpty(cmd))
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "请发送指令";
                return rst;
            }
            try
            {
                //long log_id = bll.YdModuleOnAddCmd(module_id, 0, V0Fun.Ssr1.ToString(), cmd);
                long log_id = bll.YdModuleOnAddCmd(module_id, 0, V0Fun.Ssr.ToString(), cmd);

                CommandVModel command = bll.GetYdModuleOnSendCmd(log_id);
                ListenVModel vm = new ListenVModel();
                vm.cfun = ListenCFun.cmd.ToString();
                vm.content = JsonHelper.Serialize(command);
                string msg = "";
                rst.rst = CacheMgr.SendCollectVal(vm, out msg);
                rst.err.msg = msg;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("下发指令错误(GetYdMonitorOfControl):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIRst SaveYdMonitorOfControl(int module_id,int isAlarm)
        {
            APIRst rst = new APIRst();
            try
            {
                int cnt = bll.SaveYdMonitorOfControl(module_id, isAlarm);
                if (cnt == 0)
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "无数据更新";
                }
                rst.data = "";
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("更新数据错误(SaveYdMonitorOfControl):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /////////////
        public APIRst GetYdBatchControlOfList(int module_id,string strcName,string coName)
        {
            string ssr = "";
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdMonitorOnList(module_id, strcName, coName, ssr);
                this.GetYdStatusOnLine(dtSource);
                int total = dtSource.Rows.Count;
                object rows = this.GetYdMonitorOnList(dtSource);
                object obj = new { total = total, rows = rows };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取状态列表错误 (GetYdBatchControlOfList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        // 批量下发命令
        public APIRst YdBatchControlOfSsr(string v0Fun,string dataValue,string pStr)
        {
            APIRst rst = new APIRst();
            YdToGw gw = new YdToGw(user.Ledger, user.Uid);
            V0Fun fun = V0Fun.E;
            if (Enum.TryParse<V0Fun>(v0Fun, out fun) == false)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "控制命令错误";
                return rst;
            }
            //V0Fun fun = V0Fun.Ssr;
            try
            {
                foreach (string s in pStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    gw.YdToGwCmd(CommFunc.ConvertDBNullToInt32(s), fun, dataValue);
                }
                rst.data = "";
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("批量下发命令错误(YdBatchControlOfSsr)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取继电器历史状态
        /// </summary>
        /// <returns></returns>
        public APIRst YdMonitorOfGetIsRelay(int module_id)
        {
            APIRst rst = new APIRst();
            string DataValue = "", IsPaulKey = "";
            int Status = 0;

            try
            {
                DataTable dt = bll.GetYdMonitorOfGetIsRelay(module_id);
                int? isPaul = null;
                if (dt.Rows.Count > 0)
                {
                    DataValue = CommFunc.ConvertDBNullToString(dt.Rows[0]["DataValue"]);
                    Status = CommFunc.ConvertDBNullToInt32(dt.Rows[0]["Status"]);
                    IsPaulKey = CommFunc.ConvertDBNullToString(dt.Rows[0]["IsPaulKey"]);
                    if (dt.Rows[0]["IsPaul"] != DBNull.Value)
                        isPaul = CommFunc.ConvertDBNullToInt32(dt.Rows[0]["IsPaul"]);
                }
                // int? isPaul = null; = bll.GetYdMonitorOfGetIsPaul(module_id);
                if (!string.IsNullOrEmpty(IsPaulKey))
                {
                    YDS6000.WebApi.Areas.IFSMgr.Opertion.Monitor.MonitorHelper rd = new YDS6000.WebApi.Areas.IFSMgr.Opertion.Monitor.MonitorHelper();
                    APIRst objIsPaul = rd.GetRealVal(IsPaulKey);
                    if (objIsPaul != null && objIsPaul.rst == true)
                    {
                        //RstVar var = objIsPaul.data as RstVar;
                        object lpszVal = CommFunc.GetPropertyValue("lpszVal", objIsPaul.data);
                        if (lpszVal != null)
                            isPaul = (int)CommFunc.ConvertDBNullToDecimal(lpszVal);
                    }
                    else
                    {
                        isPaul = bll.GetYdMonitorOfGetIsPaul(module_id);
                    }
                }
                else
                {
                    isPaul = bll.GetYdMonitorOfGetIsPaul(module_id);
                }
                object obj = new { DataValue = DataValue, Status = Status, IsPaul = isPaul == null ? "" : isPaul.Value.ToString() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取继电器状态列表错误 (YdMonitorOfGetIsRelay)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 下发继电器状态
        /// </summary>
        /// <returns></returns>
        public APIRst YdMonitorOfIsRelay(string v0Fun,string dataValue,int module_id)
        {
            APIRst rst = new APIRst();
            YdToGw gw = new YdToGw(user.Ledger, user.Uid);
            V0Fun fun = V0Fun.E;
            if (Enum.TryParse<V0Fun>(v0Fun, out fun) == false)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "控制命令错误";
                return rst;
            }
            try
            {
                //根据Module_id找到对应的表v0_fun中继电器状态，保存设置的继电器状态
                DataTable dt = bll.GetMm_idBymodule_id(module_id);
                int cnt = 0;
                if (dt.Rows.Count != 0)
                {
                    int mm_id = CommFunc.ConvertDBNullToInt32(dt.Rows[0]["Mm_id"]);
                    int fun_id = CommFunc.ConvertDBNullToInt32(dt.Rows[0]["Fun_id"]);
                    cnt = bll.SaveIsRelayInfo(module_id, fun_id, dataValue);
                }
                else
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "该功能不存在与这个类型的电表中。";
                    return rst;
                }
                if (cnt == 0)
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "更新继电器状态错误";
                    return rst;
                }
                //向网关下发指令
                gw.YdToGwCmd(module_id, fun, dataValue);
                rst.data = "";
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取下发继电器状态错误 (YdMonitorOfIsRelay)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取定时断送电策略列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdMonitorOfSsr()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdMonitorOfSsr();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToString(s1["Si_id"]),
                               Descr = CommFunc.ConvertDBNullToString(s1["Descr"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("/YdSep/Monitor/GetYdMonitorOfSsr", ex.Message);
            }
            return rst;
        }
        /// <summary>
        /// 获取电表的定时断送电策略
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdMonitorOfM_Ssr(int Module_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdMonitorOfM_Ssr(Module_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToString(s1["Si_id"]),
                               Descr = CommFunc.ConvertDBNullToString(s1["Descr"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("/YdSep/Monitor/GetYdMonitorOfM_Ssr", ex.Message);
            }
            return rst;
        }
        /// <summary>
        /// 设置电表定时断送电策略
        /// </summary>
        /// <returns></returns>
        public APIRst SetYdMonitorOfSsr(int Si_id,int module_id)
        {
            //if (Si_id == 0)
            //return Json(new { rst = false, msg = "请正确选择定时断送电策略", data = "" }, JsonRequestBehavior.AllowGet);
            APIRst rst = new APIRst();
            try
            {
                int cnt = 0;
                cnt = bll.SetYdMonitorOfSsr(module_id, Si_id);
                if (cnt == 0)
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "更新电表定时断送电策略错误";
                    return rst;
                }
                rst.data = "";
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置定时断送电策略错误 (SetYdMonitorOfSsr)", ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 建筑下拉框
        /// </summary>
        /// <returns></returns>
        public APIRst GetViewBuildCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetViewBuildCombox();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               text = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("建筑下拉框 (GetViewBuildList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 用电单元下拉框
        /// </summary>
        /// <returns></returns>
        public APIRst GetViewCellCombox(int parent_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetViewCellCombox(parent_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               text = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("用电单元下拉框 (GetViewCellCombox)", ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 平面图监视列表
        /// </summary>
        /// <param name="co_id">楼层ID号</param>
        /// <returns></returns>
        public APIRst GetViewList(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                List<object> list = new List<object>();
                DataTable dtSource = bll.GetViewList(co_id);
                string path = dtSource.Rows.Count == 0 ? "" : CommFunc.ConvertDBNullToString(dtSource.Rows[0]["Path"]);
                var res1 = from s1 in dtSource.AsEnumerable()
                           group s1 by new { Module_id= CommFunc.ConvertDBNullToInt32(s1["Module_id"]) }into g1
                           select new
                           {
                               mid = g1.Key.Module_id,
                               name = CommFunc.ConvertDBNullToString(g1.First()["ModuleName"]),
                               inst_loc = CommFunc.ConvertDBNullToString(g1.First()["Inst_loc"]),
                           };
                foreach (var s1 in res1)
                {
                    var res2 = from s2 in dtSource.AsEnumerable()
                               where CommFunc.ConvertDBNullToInt32(s2["Module_id"]) == s1.mid
                               select new
                               {
                                   tag = CommFunc.ConvertDBNullToString(s2["TagName"]),
                                   name = CommFunc.ConvertDBNullToString(s2["FunName"]),                                   
                               };
                    list.Add(new { mid = s1.mid, name = s1.name, inst_loc = s1.inst_loc, list = res2.ToList() });
                }
                rst.data = new { path = path, data = list };
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("平面图监视列表 (GetViewList)", ex.Message + ex.StackTrace);
            }
            return rst;          
            //List<object> list = new List<object>();
            //if (id == 1)
            //{
            //    List<object> tag01 = new List<object>();
            //    tag01.Add(new { tag = "HZ1.A1.E", name = "电能" });
            //    tag01.Add(new { tag = "HZ1.A1.Comm", name = "故障" });
            //    list.Add(new { mid = 1, name = "8_01", list = tag01 });
            //    //
            //    List<object> tag02 = new List<object>();
            //    tag02.Add(new { tag = "HZ1.A1.E", name = "电能" });
            //    tag02.Add(new { tag = "HZ1.A1.Comm", name = "故障" });
            //    list.Add(new { mid = 2, name = "8_02", list = tag02 });
            //}
            //else if (id == 2)
            //{
            //    List<object> tag01 = new List<object>();
            //    tag01.Add(new { tag = "HZ1.A1.E", name = "电能" });
            //    tag01.Add(new { tag = "HZ1.A1.Comm", name = "故障" });
            //    list.Add(new { mid = 1, name = "8_01", list = tag01 });
            //    //
            //    List<object> tag02 = new List<object>();
            //    tag02.Add(new { tag = "HZ1.A1.E", name = "电能" });
            //    tag02.Add(new { tag = "HZ1.A1.Comm", name = "故障" });
            //    list.Add(new { mid = 2, name = "8_02", list = tag02 });
            //}
            //else
            //{
            //    List<object> tag01 = new List<object>();
            //    tag01.Add(new { tag = "HZ1.A1.E", name = "电能" });
            //    tag01.Add(new { tag = "HZ1.A1.Comm", name = "故障" });
            //    list.Add(new { mid = 1, name = "8_01", list = tag01 });
            //    //
            //    List<object> tag02 = new List<object>();
            //    tag02.Add(new { tag = "HZ1.A1.E", name = "电能" });
            //    tag02.Add(new { tag = "HZ1.A1.Comm", name = "故障" });
            //    list.Add(new { mid = 2, name = "8_02", list = tag02 });
            //}
            //rst.data = list;
            ////
            //return rst;
        }
    }
}