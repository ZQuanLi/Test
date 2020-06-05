using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.PDU.Mgr
{
    public partial class MgrBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.PDU.Mgr.MgrDAL dal = null;
        public MgrBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.PDU.Mgr.MgrDAL(_ledger, _uid);
        }

        public object GetMgrStatus(string pk)
        {
            DataTable dtSource = dal.GetMgrStatus();
            List<object> dd = new List<object>();
            int sc = 0;
            int oc = 0;
            int ec = 0;
            foreach (DataRow drp in dtSource.Rows)
            {
                string key = pk + CommFunc.ConvertDBNullToString(drp["LpszDbVarName"]);
                int i = 0;
                RstVar var = null;
                while (++i <= 2)
                {
                    var = MemcachedMgr.GetVal<RstVar>(key);
                    if (var != null) break;
                    System.Threading.Thread.Sleep(50);
                }
                decimal? status = null;
                if (var != null)
                {
                    status = CommFunc.ConvertDBNullToDecimal(var.lpszVal);
                    //sc = sc + (CommFunc.ConvertDBNullToDecimal(var.lpszVal) == 0 ? 1 : 0);
                    if (status == 0)
                    {
                        sc++;
                    }
                    else
                    {
                        oc++;
                    }
                }
                else
                {
                    ec++;
                }
                dd.Add(new { moduleName = CommFunc.ConvertDBNullToString(drp["CoName"]), status = status });
            }
            return new { status = new { count = dtSource.Rows.Count, online = sc, offline = oc, errCnt = ec }, list = dd };
        }


        public object GetMgrCtrl(string pk, string moduleName)
        {
            DataTable dtSource = dal.GetMgrCtrl(moduleName);
            List<object> dd = new List<object>();
            foreach (DataRow drp in dtSource.Select("Parent_id=0"))
            {
                int cc = 0, sc = 0; ;
                List<object> cp = new List<object>();
                foreach (DataRow dr in dtSource.Select("Parent_id=" + CommFunc.ConvertDBNullToInt32(drp["Module_id"])))
                {
                    string key = pk + CommFunc.ConvertDBNullToString(dr["LpszDbVarName"]);
                    string dataValue = CommFunc.ConvertDBNullToString(dr["DataValue"]);
                    int status = CommFunc.ConvertDBNullToInt32(dr["Status"]);
                    DateTime update_dt = CommFunc.ConvertDBNullToDateTime(dr["Update_dt"]);

                    int i = 0;
                    RstVar var = null;
                    while (++i <= 2)
                    {
                        var = MemcachedMgr.GetVal<RstVar>(key);
                        if (var != null) break;
                        System.Threading.Thread.Sleep(50);
                    }
                    decimal? value = null;// "未知";
                    int realStatus = 1;
                    if (var != null)
                    {
                        value = CommFunc.ConvertDBNullToDecimal(var.lpszVal); // == 0 ? "合闸" : "拉闸";
                        sc = sc + (CommFunc.ConvertDBNullToInt32(var.lpszVal) == 0 ? 1 : 0);
                    }
                    if (!string.IsNullOrEmpty(dataValue))
                    {
                        if (update_dt.AddMinutes(1) > DateTime.Now)
                        {/*在两分钟内重新监测一次,超过两分钟设置全部按缓存值*/
                            decimal realVal = value == null ? -1 : value.Value;
                            if (realVal == CommFunc.ConvertDBNullToDecimal(dataValue))
                            {/*值相等设置成功*/
                                realStatus = 1;
                            }
                            else
                            {
                                realStatus = 0;
                            }
                            value = CommFunc.ConvertDBNullToDecimal(dataValue);
                        }
                    }
                    cp.Add(new { rowId = ++cc, moduleName = CommFunc.ConvertDBNullToString(dr["ModuleName"]), tag = CommFunc.ConvertDBNullToString(dr["LpszDbVarName"]), value = value, status = realStatus });
                }
                dd.Add(new { moduleName = CommFunc.ConvertDBNullToString(drp["ModuleName"]), count = cc, online = sc, list = cp });
            }
            return dd;
        }
    }
}
