using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.PDU.Monitor
{
    public partial class MonitorBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.PDU.Monitor.MonitorDAL dal = null;
        public MonitorBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.PDU.Monitor.MonitorDAL(_ledger, _uid);
        }

        public object GetMonitorInfo(int co_id)
        {
            DataTable dtSource = dal.GetMonitorInfo(co_id);
            //int module_id = 0, fun_id = 0, frMd=0;
            //string moduleAddr = "";
            //foreach (DataRow dr in dtSource.Select("IsDefine=0 and (FunType='E')"))
            //{
            //    frMd = CommFunc.ConvertDBNullToInt32(dr["FrMd"]);
            //    module_id = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);
            //    moduleAddr = CommFunc.ConvertDBNullToString(dr["ModuleAddr"]);
            //    fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]);
            //    break;
            //}
            //decimal useVal = dal.GetMonitorUseVal(co_id, module_id, moduleAddr, fun_id);
            object s = this.GetTagName(dtSource.Select("IsDefine=0 and (FunType='E' or FunType='Eb'or FunType='Ec')"), 3, "设备状态", true);/*设备状态*/
            object u = this.GetTagName(dtSource.Select("IsDefine=0 and (FunType='Ua' or FunType='Ub' or FunType='Uc')"), 3, "电压", true);/*相电压*/
            object i = this.GetTagName(dtSource.Select("IsDefine=0 and (FunType='Ia' or FunType='Ib' or FunType='Ic')"), 3, "电流", true);/*相电压*/
            object p = this.GetTagName(dtSource.Select("IsDefine=0 and (FunType='Pa' or FunType='Pb' or FunType='Pc')"), 3, "功率", true);/*相电压*/
            object pf = this.GetTagName(dtSource.Select("IsDefine=0 and (FunType='PFa' or FunType='PFb' or FunType='PFc')"), 3, "功率因素", true);/*功率因素*/
            object wd = this.GetTagName(dtSource.Select("IsDefine=10 and FunType='Val'"), 3, "温度传感器");/*温度*/
            object sd = this.GetTagName(dtSource.Select("IsDefine=20 and FunType='Val'"), 2, "湿度传感器");/*湿度*/
            object yg = this.GetTagName(dtSource.Select("IsDefine=30 and FunType='Val'"), 2, "传感器接口");/*烟感*/
            object mk = this.GetTagName(dtSource.Select("IsDefine=40 and FunType='Val'"), 4, "开关输入");/*门控*/
            //return new { u = u, i = i, p = p, pf = pf, wd = wd, sd = sd, yg = yg, mk = mk, status = s, useVal = useVal, frMd = frMd };
            return new { u = u, i = i, p = p, pf = pf, wd = wd, sd = sd, yg = yg, mk = mk, status = s };
        }

        
        public object GetMonitorInfo200(int co_id)
        {
            DataTable dtSource = dal.GetMonitorInfo(co_id);
            object l1 = this.GetTagName(dtSource.Select("IsDefine=0 and (FunType='E' or FunType='Ua'or FunType='Ia' or FunType='Pa' or FunType='PFa')", "FunType"), 5, "L1输入状态", true);
            object l2 = this.GetTagName(dtSource.Select("IsDefine=0 and (FunType='Eb' or FunType='Ub'or FunType='Ib' or FunType='Pb' or FunType='PFb')", "FunType"), 5, "L2输入状态", true);
            object l3 = this.GetTagName(dtSource.Select("IsDefine=0 and (FunType='Ec' or FunType='Uc'or FunType='Ic' or FunType='Pc' or FunType='PFc')", "FunType"), 5, "L3输入状态", true);
            return new { l1 = l1, l2 = l2, l3 = l3 };
        }

        private object GetTagName(DataRow[] arr, int count, string descr,bool isEng = false)
        {
            List<object> list = new List<object>();
            object obj = new { name = descr, tag = "" };
            string unit = "";
            foreach (DataRow dr in arr)
            {
                unit = CommFunc.ConvertDBNullToString(dr["Unit"]);
                obj = new { name = CommFunc.ConvertDBNullToString(dr["FunName"]), tag = CommFunc.ConvertDBNullToString(dr["LpszDbVarName"]), Unit = unit };
                list.Add(obj);
                if (list.Count == count) break;
            }
            int cc = list.Count;
            while (cc < count)
            {
                list.Add(new { name = descr + (isEng == true ? CommFunc.NunberToChar((++cc) + 63) : (++cc).ToString().PadLeft(2, '0')), tag = "", Unit = unit });
            }
            //if (count == 1)
            //    return obj;
            return list;
        }

        public DataTable GetMonitorList(string pk,int co_id)
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("Module_id", typeof(System.Int32));
            dtRst.Columns.Add("ModuleName", typeof(System.String));
            dtRst.Columns.Add("Status", typeof(System.String));            
            dtRst.Columns.Add("Ssr_Tag", typeof(System.String));
            dtRst.Columns.Add("E_Tag", typeof(System.String));
            dtRst.Columns.Add("U_Tag", typeof(System.String));
            dtRst.Columns.Add("I_Tag", typeof(System.String));
            dtRst.Columns.Add("P_Tag", typeof(System.String));
            dtRst.Columns.Add("IMax_Tag", typeof(System.String));
            dtRst.Columns.Add("IMin_Tag", typeof(System.String));
            dtRst.Columns.Add("IMax_Val", typeof(System.String));
            dtRst.Columns.Add("IMin_Val", typeof(System.String));
            dtRst.Columns.Add("IMax_Unit", typeof(System.String));
            dtRst.Columns.Add("IMin_Unit", typeof(System.String));
            dtRst.Columns.Add("Ssr_Unit", typeof(System.String));
            dtRst.Columns.Add("E_Unit", typeof(System.String));
            dtRst.Columns.Add("U_Unit", typeof(System.String));
            dtRst.Columns.Add("I_Unit", typeof(System.String));
            dtRst.Columns.Add("P_Unit", typeof(System.String));
            dtRst.Columns.Add("FrMd", typeof(System.Int32));
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["Module_id"] };
            DataTable dtSource = dal.GetMonitorList(co_id);
            foreach (DataRow dr in dtSource.Rows)
            {
                int frMd = CommFunc.ConvertDBNullToInt32(dr["FrMd"]);
                frMd = frMd == 0 ? 15 : frMd;
                DataRow addDr = dtRst.Rows.Find(dr["Module_id"]);                
                if (addDr == null)
                {
                    addDr = dtRst.NewRow();
                    addDr["Module_id"] = dr["Module_id"];
                    addDr["ModuleName"] = dr["ModuleName"];
                    addDr["FrMd"] = dr["FrMd"];
                    dtRst.Rows.Add(addDr);
                }

                //E,U,I,P,IMax,IMin
                string cloTag = CommFunc.ConvertDBNullToString(dr["FunType"]) + "_Tag";
                string cloUnit = CommFunc.ConvertDBNullToString(dr["FunType"]) + "_Unit";
                string cloVal = CommFunc.ConvertDBNullToString(dr["FunType"]) + "_Val";
                if (dtRst.Columns.Contains(cloTag))              
                    addDr[cloTag] = CommFunc.ConvertDBNullToString(dr["LpszDbVarName"]);
                if (dtRst.Columns.Contains(cloUnit))
                    addDr[cloUnit] = CommFunc.ConvertDBNullToString(dr["Unit"]);
                if (dtRst.Columns.Contains(cloVal))
                    addDr[cloVal] = CommFunc.ConvertDBNullToString(dr["DataValue"]);

                if (CommFunc.ConvertDBNullToString(dr["FunType"]).Equals("E"))
                {
                    string key = pk + CommFunc.ConvertDBNullToString(dr["LpszDbVarName"]);
                    int i = 0;
                    RstVar var = null;
                    while (++i <= 2)
                    {
                        var = MemcachedMgr.GetVal<RstVar>(key);
                        if (var != null) break;
                        System.Threading.Thread.Sleep(50);
                    }
                    DateTime lastTime = var == null ? new DateTime(1900, 1, 1) : var.lpszdateTime;
                    if (DateTime.Now > lastTime.AddMinutes(frMd))
                        addDr["Status"] = "0";
                    else
                        addDr["Status"] = "1";
                    //if (DateTime.Now > lastTime.AddMinutes(frMd))
                    //    addDr["Status"] = "异常";
                    //else
                    //    addDr["Status"] = "在线";
                }
            }
            return dtRst;
        }

        /// <summary>
        /// 获取层级1的数据
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public DataTable GetLayer01(int co_id)
        {
            return dal.GetLayer01(co_id);
        }

        /// <summary>
        /// 获取层级2的数据
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public DataTable GetLayer02(int co_id)
        {
            DataTable dtSource = dal.GetLayer02(co_id);
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Co_id"] };
            dtSource.Columns.Add("TagNameWD", typeof(System.String));/*获取温暖的tagName*/
            dtSource.Columns.Add("TagValWD", typeof(System.Decimal));/*获取温暖的最后值(数据库值)*/
            dtSource.Columns.Add("TagNameSD", typeof(System.String));/*获取湿度的tagName*/
            dtSource.Columns.Add("TagValSD", typeof(System.Decimal));/*获取湿度的最后值(数据库值)*/
            DataTable dtInfo = dal.GetLastInfo(co_id);
            foreach (DataRow dr in dtInfo.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(dr["Co_id"]);
                if (curDr == null) continue;
                if (CommFunc.ConvertDBNullToString("ModuleType").Equals("WSD"))
                {/*温度*/
                    curDr["TagNameWD"] = dr["MemcachKey"];
                    curDr["TagValWD"] = dr["LastVal"];
                }
                else if (CommFunc.ConvertDBNullToString("ModuleType").Equals("SD"))
                {
                    curDr["TagNameSD"] = dr["MemcachKey"];
                    curDr["TagValSD"] = dr["LastVal"];
                }
            }
            return dtSource;
        }

        public DataTable GetLayer03(int co_id)
        {
            return dal.GetLayer03(co_id);
        }
        ////////////////////////////////////////////////
        
        public DataTable GetMonitorSensor200(string pk, int co_id)
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("Module_id", typeof(System.Int32));
            dtRst.Columns.Add("ModuleName", typeof(System.String));
            dtRst.Columns.Add("Val_Tag", typeof(System.String));
            dtRst.Columns.Add("Val_Unit", typeof(System.String));
            dtRst.Columns.Add("FrMd", typeof(System.Int32));
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["Module_id"] };
            DataTable dtSource = dal.GetMonitorInfo(co_id);
            foreach (DataRow dr in dtSource.Select("(IsDefine = 10 or IsDefine = 20 or IsDefine = 30 or IsDefine = 40) and FunType = 'Val'"))
            {
                int frMd = CommFunc.ConvertDBNullToInt32(dr["FrMd"]);
                frMd = frMd == 0 ? 15 : frMd;
                DataRow addDr = dtRst.Rows.Find(dr["Module_id"]);
                if (addDr == null)
                {
                    addDr = dtRst.NewRow();
                    addDr["Module_id"] = dr["Module_id"];
                    addDr["ModuleName"] = dr["ModuleName"];
                    addDr["FrMd"] = dr["FrMd"];
                    dtRst.Rows.Add(addDr);
                }
                addDr[CommFunc.ConvertDBNullToString(dr["FunType"]) + "_Tag"] = CommFunc.ConvertDBNullToString(dr["LpszDbVarName"]);
                addDr[CommFunc.ConvertDBNullToString(dr["FunType"]) + "_Unit"] = CommFunc.ConvertDBNullToString(dr["Unit"]);
            }
            return dtRst;
        }
    }
}
