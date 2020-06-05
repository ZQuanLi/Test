using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Monitor
{
    public partial class MonitorBLL
    {
        /// <summary>
        /// 获取费率信息
        /// </summary>
        /// <param name="Co_id"></param>
        /// <returns></returns>
        public List<Treeview> GetYdCustOnCoInfoList(out int total)
        {
            DataTable dtSource = dal.GetYdCustOnCoInfoList();
            total = dtSource.Rows.Count;
            return this.GetMenuList(dtSource);
        }
        private List<Treeview> GetMenuList(DataTable dtSource)
        {
            List<Treeview> rst = new List<Treeview>();
            DataRow[] pArr = dtSource.Select("Parent_id=0", "Co_id");
            foreach (DataRow dr in pArr)
            {
                Treeview pTr = new Treeview();
                pTr.id = CommFunc.ConvertDBNullToString(dr["Co_id"]);
                pTr.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                //pTr.attributes = CommFunc.ConvertDBNullToInt32(dr["power"]);
                pTr.nodes = new List<Treeview>();
                this.GetMenuList(ref dtSource, ref pTr, CommFunc.ConvertDBNullToString(dr["Co_id"]));
                rst.Add(pTr);
            }
            return rst;
        }
        private void GetMenuList(ref DataTable dtSource, ref Treeview pTr, string Co_id)
        {
            DataRow[] pArr = dtSource.Select("Parent_id=" + Co_id, "Co_id");
            int pRows = pArr.Count();
            if (pRows > 0)
                pTr.nodes = new List<Treeview>();
            foreach (DataRow dr in pArr)
            {
                Treeview cTr = new Treeview();
                cTr.id = CommFunc.ConvertDBNullToString(dr["Co_id"]);
                cTr.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                //cTr.attributes = CommFunc.ConvertDBNullToInt32(dr["power"]);
                pTr.nodes.Add(cTr);
                this.GetMenuList(ref dtSource, ref cTr, CommFunc.ConvertDBNullToString(dr["Co_id"]));
            }
        }
        /// <summary>
        /// 运行状况管理列表
        /// </summary>
        /// <param name="coName"></param>
        /// <param name="moduleAddr"></param>
        /// <returns></returns>
        public DataTable GetYdMonitorOnList(string CoStrcName, string CoName, string Ssr, int co_id)
        {
            DataTable dtSource = dal.GetYdMonitorOnList(0, CoStrcName, CoName, Ssr, co_id);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
                dr["RowId"] = ++RowId;
            dtSource.Columns["Ssr"].MaxLength = 10;
            return dtSource;
        }

        /// <summary>
        /// 运行状况管理列表
        /// </summary>
        /// <param name="coName"></param>
        /// <param name="moduleAddr"></param>
        /// <returns></returns>
        public DataTable GetYdMonitorOnList(int module_id, string CoStrcName, string CoName, string Ssr)
        {
            DataTable dtSource = dal.GetYdMonitorOnList(module_id, CoStrcName, CoName, Ssr, 0);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
                dr["RowId"] = ++RowId;
            dtSource.Columns["Ssr"].MaxLength = 10;
            return dtSource;
        }

        /// <summary>
        /// 日用电量
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Module_id"></param>
        /// <param name="ModuleAddr"></param>
        /// <returns></returns>
        public decimal GetYdMonitorOnDayUseVal(int Co_id, int Module_id, string ModuleAddr)
        {
            return dal.GetYdMonitorOnDayUseVal(Co_id, Module_id, ModuleAddr);
        }
        /// <summary>
        /// 月用电量
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Module_id"></param>
        /// <param name="ModuleAddr"></param>
        /// <returns></returns>
        public decimal GetYdMonitorOnMthUseVal(int Co_id, int Module_id, string ModuleAddr)
        {
            return dal.GetYdMonitorOnMthUseVal(Co_id, Module_id, ModuleAddr);
        }

        /// <summary>
        /// 按小时显示图形
        /// </summary>
        /// <param name="Room_id"></param>
        /// <param name="dtTime"></param>
        /// <returns></returns>
        public DataTable GetYdMonitorInDetailOnChartsData(int Co_id, int Module_id, string ModuleAddr, DateTime Start, DateTime End, string DateType)
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("TagTime", typeof(System.DateTime));
            dtRst.Columns.Add("Time", typeof(System.String));
            dtRst.Columns.Add("UseVal", typeof(System.Decimal));
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["TagTime"] };
            StringBuilder strKey1 = new StringBuilder();

            DataTable dtEz = dal.GetYdMonitorOnEzInfo(Module_id);
            int fun_id = 0, scale = 0;
            decimal multiply = 0;
            if (dtEz.Rows.Count > 0)
            {
                fun_id = CommFunc.ConvertDBNullToInt32(dtEz.Rows[0]["Fun_id"]);
                scale = CommFunc.ConvertDBNullToInt32(dtEz.Rows[0]["Scale"]);
                multiply = CommFunc.ConvertDBNullToDecimal(dtEz.Rows[0]["Multiply"]);
            }
            DateTime start = new DateTime(Start.Year, Start.Month, Start.Day, Start.Hour, 0, 0), end = new DateTime(End.Year, End.Month, End.Day, End.Hour, 0, 0);
            #region 填充主数据
            while (start <= end)
            {
                strKey1.Clear();
                if (DateType.Equals("hour"))
                    strKey1.Append(start.Hour.ToString().PadLeft(2, '0') + "时");
                if (DateType.Equals("day"))
                    strKey1.Append(start.Day.ToString().PadLeft(2, '0') + "日");
                if (DateType.Equals("month"))
                    strKey1.Append(start.Month.ToString().PadLeft(2, '0') + "月");
                if (DateType.Equals("year"))
                    strKey1.Append(start.Month.ToString().PadLeft(2, '0') + "月");
                DataRow addDr = dtRst.Rows.Find(new object[] { start });
                if (addDr == null)
                { //增加需显示的表
                    addDr = dtRst.NewRow();
                    addDr["TagTime"] = start;
                    addDr["Time"] = strKey1.ToString();
                    dtRst.Rows.Add(addDr);
                }
                if (DateType.Equals("hour"))
                    start = start.AddHours(1);
                if (DateType.Equals("day"))
                    start = start.AddDays(1);
                if (DateType.Equals("month"))
                    start = start.AddMonths(1);
                if (DateType.Equals("year"))
                    start = start.AddMonths(1);
            }
            #endregion
            DataTable dtSource = WholeBLL.GetCoreQueryData(this.Ledger, Module_id.ToString(), Start, End, DateType);
            foreach (DataRow dr in dtSource.Rows)
            {
                if (CommFunc.ConvertDBNullToInt32(dr["Fun_id"]) != fun_id) continue;
                if (CommFunc.ConvertDBNullToInt32(dr["Co_id"]) != Co_id) continue;
                if (!CommFunc.ConvertDBNullToString(dr["ModuleAddr"]).Equals(ModuleAddr)) continue;

                DataRow curDr = dtRst.Rows.Find(dr["TagTime"]);
                if (curDr == null) continue;
                decimal useVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]) - CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                useVal = useVal * multiply;
                if (scale != 0)
                    useVal = Math.Round(useVal, scale, MidpointRounding.AwayFromZero);
                curDr["UseVal"] = useVal;
            }
            return dtRst;
        }

        /// <summary>
        /// 房间月用电量
        /// </summary>
        /// <param name="Room_id"></param>
        /// <param name="dtFm"></param>
        /// <param name="dtTo"></param>
        /// <returns></returns>
        public DataTable GetYdMonitorInDetailOnBill(int Co_id, DateTime dtFm, DateTime dtTo)
        {
            DataTable dtSource = dal.GetYdMonitorInDetailOnBill(Co_id, dtFm, dtTo);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["RowId"] = ++RowId;
            }
            return dtSource;
        }


        //        public List<TemperatureAndHumidityVModel> GetTemperatureAndHumidity(string CoStrcName, string CoName)
        //        {
        //            var dtModules = dal.GetModuleListByOrg(CoStrcName, CoName, new string[] { "TPWD", "TPLDS" });

        //            var dtModulesGroup = dtModules.AsEnumerable().GroupBy(c => new { Co_id = CommFunc.ConvertDBNullToInt32(c["Co_id"]), CoName = CommFunc.ConvertDBNullToString(c["CoName"]), CoStrcName = CommFunc.ConvertDBNullToString(c["CoStrcName"]), Module_id = CommFunc.ConvertDBNullToInt32(c["Module_id"]), ModuleAddr = CommFunc.ConvertDBNullToString(c["ModuleAddr"]) }).Select(c => new { Co_id = c.Key.Co_id, CoName = c.Key.CoName, CoStrcName = c.Key.CoStrcName, Module_id = c.Key.Module_id, ModuleAddr = c.Key.ModuleAddr }).ToList();

        //            var dtFun = dtModules.AsEnumerable().GroupBy(c => new { Fun_id = CommFunc.ConvertDBNullToInt32(c["Fun_id"]), FunType = CommFunc.ConvertDBNullToString(c["FunType"]) }).Select(c => new { Fun_id = c.Key.Fun_id, FunType = c.Key.FunType });
        //            HashSet<int> ds = new HashSet<int>();
        //            for (int i = 0; i < dtModules.Rows.Count; i++)
        //            {
        //                ds.Add(CommFunc.ConvertDBNullToInt32(dtModules.Rows[i]["Module_id"]));
        //            }
        //            var currDate = DateTime.Now.AddDays(-3);
        //            var dtsource = YDS6001.Logic.WholeBLL.GetCoreQueryData(this.Ledger, string.Join(",", ds), currDate
        //, currDate, "month");

        //            var dtSourceGroup = from a in dtsource.AsEnumerable()
        //                                join b in dtFun on new { Fun_id = CommFunc.ConvertDBNullToInt32(a["Fun_id"]) } equals new { Fun_id = b.Fun_id }
        //                                where CommFunc.ConvertDBNullToString(b.FunType) == "TPWD" || CommFunc.ConvertDBNullToString(b.FunType) == "TPLDS"
        //                                group new { Co_id = CommFunc.ConvertDBNullToInt32(a["Co_id"]), Module_id = CommFunc.ConvertDBNullToInt32(a["Module_id"]), ModuleAddr = CommFunc.ConvertDBNullToString(a["ModuleAddr"]), FunType = b.FunType, MaxVal = CommFunc.ConvertDBNullToDecimal(a["MaxVal"]) }
        //                                       by new { Co_id = CommFunc.ConvertDBNullToInt32(a["Co_id"]), Module_id = CommFunc.ConvertDBNullToInt32(a["Module_id"]), ModuleAddr = CommFunc.ConvertDBNullToString(a["ModuleAddr"]), FunType = b.FunType, Fun_id = b.Fun_id }
        //                                    into g
        //                                    select new { Co_id = g.Key.Co_id, FunType = g.Key.FunType, Module_id = g.Key.Module_id, ModuleAddr = g.Key.ModuleAddr, MaxVal = g.Max(c => c.MaxVal) };
        //            List<TemperatureAndHumidityVModel> list = new List<TemperatureAndHumidityVModel>();
        //            for (int i = 0; i < dtModulesGroup.Count; i++)
        //            {
        //                TemperatureAndHumidityVModel model = new TemperatureAndHumidityVModel();
        //                model.ID = i + 1;
        //                model.Co_id = dtModulesGroup[i].Co_id;
        //                model.CoName = dtModulesGroup[i].CoName;
        //                model.CoStrcName = dtModulesGroup[i].CoStrcName;
        //                model.Module_id = dtModulesGroup[i].Module_id;
        //                model.ModuleAddr = dtModulesGroup[i].ModuleAddr;
        //                var tpwdObj = dtSourceGroup.Where(c => (c.Co_id == dtModulesGroup[i].Co_id && c.Module_id == dtModulesGroup[i].Module_id && c.ModuleAddr == dtModulesGroup[i].ModuleAddr && c.FunType == "TPWD")).ToList().FirstOrDefault();
        //                if (tpwdObj == null)
        //                {
        //                    model.TVal = 0;
        //                }
        //                else
        //                {
        //                    model.TVal = CommFunc.ConvertDBNullToDecimal(tpwdObj.MaxVal);
        //                }

        //                var tpsdObj = dtSourceGroup.Where(c => (c.Co_id == dtModulesGroup[i].Co_id && c.Module_id == dtModulesGroup[i].Module_id && c.ModuleAddr == dtModulesGroup[i].ModuleAddr && c.FunType == "TPLDS")).ToList().FirstOrDefault();
        //                if (tpsdObj == null)
        //                {
        //                    model.HVal = 0;
        //                }
        //                else
        //                {
        //                    model.HVal = CommFunc.ConvertDBNullToDecimal(tpsdObj.MaxVal);
        //                }

        //                list.Add(model);
        //            }
        //            return list;

        //        }


        /// <summary>

        /// </summary>
        /// <returns></returns>
        public DataTable GetModuleListByOrg(string CoStrcName, string CoName, params string[] funTypeParams)
        {
            var dtModules = dal.GetModuleListByOrg(CoStrcName, CoName, funTypeParams);
            return dtModules;
        }

        public DataTable GetTemperatureAndHumidity(string CoStrcName, string CoName)
        {
            DataTable dtSource = dal.GetTemperatureAndHumidity(CoStrcName, CoName);
            dtSource.Columns.Add("Leak1", typeof(System.String));
            dtSource.Columns.Add("Leak2", typeof(System.String));
            dtSource.Columns.Add("Leak3", typeof(System.String));
            dtSource.Columns.Add("Leak4", typeof(System.String));
            dtSource.Columns.Add("Leak5", typeof(System.String));
            dtSource.Columns.Add("Leak6", typeof(System.String));
            dtSource.Columns.Add("Leak7", typeof(System.String));
            dtSource.Columns.Add("Leak8", typeof(System.String));
            dtSource.Columns.Add("Temp1", typeof(System.String));
            dtSource.Columns.Add("Temp2", typeof(System.String));
            return dtSource;
        }
        /// <summary>
        /// 下发命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public long YdModuleOnAddCmd(int module_id, int fun_id, string funType, string dataValue)
        {
            return dal.YdModuleOnAddCmd(module_id, fun_id, funType, dataValue);
        }

        /// <summary>
        /// 控制列表
        /// </summary>
        /// <param name="module_id"></param>
        /// <returns></returns>
        public DataTable GetCtrlList(int module_id)
        {
            return dal.GetCtrlList(module_id);
        }
        /// <summary>
        /// 获取命令
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        public CommandVModel GetYdModuleOnSendCmd(long log_id)
        {
            return dal.GetYdModuleOnSendCmd(log_id);
        }

        public int SaveYdMonitorOfControl(int module_id, int isAlarm)
        {
            return dal.SaveYdMonitorOfControl(module_id, isAlarm);
        }

        public DataTable GetYdMonitorOfGetIsRelay(int module_id)
        {
            return dal.GetYdMonitorOfGetIsRelay(module_id);
        }

        public int? GetYdMonitorOfGetIsPaul(int module_id)
        {
            return dal.GetYdMonitorOfGetIsPaul(module_id);
        }

        //查询mm_id值
        public DataTable GetMm_idBymodule_id(int module_id)
        {
            return dal.GetMm_idBymodule_id(module_id);
        }
        //保存继电器状态
        public int SaveIsRelayInfo(int module_id, int fun_id, string dataValue)
        {
            return dal.SaveIsRelayInfo(module_id, fun_id, dataValue);
        }
        /// <summary>
        ///  获取定时断送电策略列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdMonitorOfSsr()
        {
            DataTable dtSource = dal.GetYdMonitorOfSsr();
            DataRow addDr = dtSource.NewRow();
            addDr["Si_id"] = 0;
            addDr["Descr"] = "不设置策略";
            dtSource.Rows.InsertAt(addDr, 0);
            return dtSource;
        }
        /// <summary>
        /// 获取电表的定时断送电策略
        /// </summary>
        /// <param name="Module_id"></param>
        /// <returns></returns>
        public DataTable GetYdMonitorOfM_Ssr(int Module_id)
        {
            return dal.GetYdMonitorOfM_Ssr(Module_id);
        }
        /// <summary>
        /// 设置定时断送电策略
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="Si_id"></param>
        /// <returns></returns>
        public int SetYdMonitorOfSsr(int module_id, int Si_id)
        {
            return dal.SetYdMonitorOfSsr(module_id, Si_id);
        }
        //判断用户是否具有批量控制权限
        public bool GetPower(int Ledger, int Role_id, string prog_id)
        {
            return dal.GetPower(Ledger, Role_id, prog_id);
        }

        public DataTable GetViewBuildCombox()
        {
            return dal.GetViewBuildCombox();
        }
        public DataTable GetViewCellCombox(int parent_id)
        {
            return dal.GetViewCellCombox(parent_id);
        }
        public DataTable GetViewList(int co_id)
        {
            return dal.GetViewList(co_id);
        }
    }
}
