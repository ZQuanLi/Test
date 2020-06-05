using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.PDU.Opertion.Monitor
{
    public class MonitorHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.PDU.Monitor.MonitorBLL bll = null;
        public MonitorHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.PDU.Monitor.MonitorBLL(user.Ledger, user.Uid);
        }

        /// <summary>
        /// 获取监测数据信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetMonitorInfo(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetMonitorInfo(co_id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取监测数据信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取监测数据信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetMonitorList(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                List<object> rb = new List<object>();
                DataTable dtSource = bll.GetMonitorList(user.CacheKey, co_id);
                foreach (DataRow s1 in dtSource.Rows)
                {
                    object list = new
                    {
                        E_Tag = new
                        {
                            name = "电能采集点",
                            tag = CommFunc.ConvertDBNullToString(s1["E_Tag"]),
                            unit = CommFunc.ConvertDBNullToString(s1["E_Unit"]),
                        },
                        Ua_Tag = new
                        {
                            name = "电压采集点",
                            tag = CommFunc.ConvertDBNullToString(s1["U_Tag"]),
                            unit = CommFunc.ConvertDBNullToString(s1["U_Unit"]),
                        },
                        Ia_Tag = new
                        {
                            name = "电流采集点",
                            tag = CommFunc.ConvertDBNullToString(s1["I_Tag"]),
                            unit = CommFunc.ConvertDBNullToString(s1["I_Unit"]),
                        },
                        Pa_Tag = new
                        {
                            name = "有功功率",
                            tag = CommFunc.ConvertDBNullToString(s1["P_Tag"]),
                            unit = CommFunc.ConvertDBNullToString(s1["P_Unit"]),
                        },
                    };
                    object obj = new
                    {
                        RowId = dtSource.Rows.IndexOf(s1) + 1,
                        Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                        ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                        Status = CommFunc.ConvertDBNullToString(s1["Status"]),
                        FrMd = CommFunc.ConvertDBNullToInt32(s1["FrMd"]),
                        List = list,
                    };
                    rb.Add(obj);
                }
                //var res1 = from s1 in dtSource.AsEnumerable()
                //           select new
                //           {
                //               RowId = dtSource.Rows.IndexOf(s1) + 1,
                //               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                //               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                //               E_Tag = CommFunc.ConvertDBNullToString(s1["E_Tag"]),
                //               Ua_Tag = CommFunc.ConvertDBNullToString(s1["Ua_Tag"]),
                //               Ia_Tag = CommFunc.ConvertDBNullToString(s1["Ia_Tag"]),
                //               Pa_Tag = CommFunc.ConvertDBNullToString(s1["Pa_Tag"]),
                //               E_Unit = CommFunc.ConvertDBNullToString(s1["E_Unit"]),
                //               Ua_Unit = CommFunc.ConvertDBNullToString(s1["Ua_Unit"]),
                //               Ia_Unit = CommFunc.ConvertDBNullToString(s1["Ia_Unit"]),
                //               Pa_Unit = CommFunc.ConvertDBNullToString(s1["Pa_Unit"]),
                //               Status = CommFunc.ConvertDBNullToString(s1["Status"]),
                //               FrMd = CommFunc.ConvertDBNullToInt32(s1["FrMd"]),
                //           };
                rst.data = rb; // res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取监测数据信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取第一层数据
        /// </summary>     
        /// <param name="co_id">Pdu ID号</param>
        /// <returns></returns>
        public APIRst GetLayer01(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetLayer01(co_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new {
                               Id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               Name = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Addr = CommFunc.ConvertDBNullToString(s1["CustAddr"]),
                               Number = CommFunc.ConvertDBNullToInt32(s1["Number"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                           }; 
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取监测数据信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取第二层数据
        /// </summary>     
        /// <param name="co_id">层级2的id号</param>
        /// <returns></returns>
        public APIRst GetLayer02(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                List<object> dd = new List<object>();
                DataTable dtSource = bll.GetLayer02(co_id);
                foreach (DataRow dr in dtSource.Rows)
                {
                    //string keyWD = user.CacheKey + CommFunc.ConvertDBNullToString(dr["TagNameWD"]);
                    //string keySD = user.CacheKey + CommFunc.ConvertDBNullToString(dr["TagNameSD"]);
                    //RstVar var1 = this.GetRstVar(keyWD);
                    //RstVar var2 = this.GetRstVar(keySD);
                    //if (var1 != null)
                    //    dr["TagValWD"] = var1.lpszVal;
                    //if (var2 != null)
                    //    dr["TagValSD"] = var1.lpszVal;
                    List<object> cc = new List<object>();
                    DataTable dtR3= bll.GetLayer02(CommFunc.ConvertDBNullToInt32(dr["Co_id"]));
                    foreach (DataRow dr3 in dtR3.Rows)
                    {
                        string keyWD = user.CacheKey + CommFunc.ConvertDBNullToString(dr["TagNameWD"]);
                        string keySD = user.CacheKey + CommFunc.ConvertDBNullToString(dr["TagNameSD"]);
                        RstVar var1 = this.GetRstVar(keyWD);
                        RstVar var2 = this.GetRstVar(keySD);
                        if (var1 != null)
                            dr3["TagValWD"] = var1.lpszVal;
                        if (var2 != null)
                            dr3["TagValSD"] = var1.lpszVal;
                        object objC = new
                        {
                            Id = CommFunc.ConvertDBNullToInt32(dr3["Co_id"]),
                            Name = CommFunc.ConvertDBNullToString(dr3["CoName"]),
                            Number = CommFunc.ConvertDBNullToInt32(dr3["Number"]),
                            Addr = CommFunc.ConvertDBNullToString(dr3["CustAddr"]),
                            WD = CommFunc.ConvertDBNullToDecimal(dr3["TagValWD"]).ToString("f2"),
                            SD = CommFunc.ConvertDBNullToDecimal(dr3["TagValSD"]).ToString("f2"),
                            Disabled = CommFunc.ConvertDBNullToInt32(dr3["Disabled"]),
                        };
                        cc.Add(objC);
                    }
                    object obj = new
                    {
                        Id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]),
                        Name = CommFunc.ConvertDBNullToString(dr["CoName"]),
                        Number = CommFunc.ConvertDBNullToInt32(dr["Number"]),
                        Addr = CommFunc.ConvertDBNullToString(dr["CustAddr"]),
                        WD = CommFunc.ConvertDBNullToDecimal(dr["TagValWD"]).ToString("f2"),
                        SD = CommFunc.ConvertDBNullToDecimal(dr["TagValSD"]).ToString("f2"),
                        Disabled = CommFunc.ConvertDBNullToInt32(dr["Disabled"]),
                        nodes = cc,
                    };
                    dd.Add(obj);
                }
                //var res1 = from s1 in dtSource.AsEnumerable()
                //           select new
                //           {
                //               Id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                //               Name = CommFunc.ConvertDBNullToString(s1["CoName"]),
                //               Number = CommFunc.ConvertDBNullToInt32(s1["Number"]),
                //               Addr = CommFunc.ConvertDBNullToString(s1["CustAddr"]),
                //               WD = CommFunc.ConvertDBNullToDecimal(s1["TagValWD"]).ToString("f2"),
                //               SD = CommFunc.ConvertDBNullToDecimal(s1["TagValSD"]).ToString("f2"),
                //               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                //           };
                rst.data = dd; // res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取第二层数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private RstVar GetRstVar(string key)
        {
            int i = 0;
            RstVar var = null;
            while (++i <= 2)
            {
                var = MemcachedMgr.GetVal<RstVar>(key);
                if (var != null) break;
                System.Threading.Thread.Sleep(50);
            }
            return var;
        }


        /// <summary>
        /// 获取第二层数据
        /// </summary>     
        /// <param name="co_id">层级2的id号</param>
        /// <returns></returns>
        public APIRst GetLayer03(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                List<object> dd = new List<object>();
                DataTable dtSource = bll.GetLayer02(co_id);
                foreach (DataRow dr in dtSource.Rows)
                {
                    List<object> cc = new List<object>();
                    object obj = new
                    {
                        Id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]),
                        Name = CommFunc.ConvertDBNullToString(dr["CoName"]),
                        Number = CommFunc.ConvertDBNullToInt32(dr["Number"]),
                        Addr = CommFunc.ConvertDBNullToString(dr["CustAddr"]),
                        WD = CommFunc.ConvertDBNullToDecimal(dr["TagValWD"]).ToString("f2"),
                        SD = CommFunc.ConvertDBNullToDecimal(dr["TagValSD"]).ToString("f2"),
                        Disabled = CommFunc.ConvertDBNullToInt32(dr["Disabled"]),
                    };
                    dd.Add(obj);
                }
                rst.data = dd; // res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取第二层数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取第三层数据
        /// </summary>     
        /// <param name="id">层级3的id号</param>
        /// <returns></returns>
        public APIRst GetLayer04(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetLayer03(co_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               Name = CommFunc.ConvertDBNullToString(s1["CoName"]),                               
                               Number = CommFunc.ConvertDBNullToInt32(s1["Number"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               //Addr = CommFunc.ConvertDBNullToString(s1["CustAddr"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取第三层数据信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        #region 2019.02.25
        /// <summary>
        /// 获取监测数据信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetMonitorInfo200(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetMonitorInfo200(co_id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取监测数据信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取监测数据信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetMonitorList200(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                List<object> rb = new List<object>();
                DataTable dtSource = bll.GetMonitorList(user.CacheKey, co_id);
                foreach (DataRow s1 in dtSource.Rows)
                {
                    object list = new
                    {
                        Ssr_Tag = new {
                            name = "插座状态",
                            tag = CommFunc.ConvertDBNullToString(s1["Ssr_Tag"]),
                            unit = CommFunc.ConvertDBNullToString(s1["Ssr_Unit"]),
                        },
                        IMax_Tag = new {
                            name = "负载上限",
                            tag = CommFunc.ConvertDBNullToString(s1["IMax_Tag"]),
                            unit = CommFunc.ConvertDBNullToString(s1["IMax_Unit"]),
                            value = CommFunc.ConvertDBNullToString(s1["IMax_Val"]),
                        } ,
                        IMin_Tag = new
                        {
                            name = "负载下限",
                            tag = CommFunc.ConvertDBNullToString(s1["IMin_Tag"]),
                            unit = CommFunc.ConvertDBNullToString(s1["IMin_Unit"]),
                            value = CommFunc.ConvertDBNullToString(s1["IMin_Val"]),
                        },
                        E_Tag = new
                        {
                            name = "电能采集点",
                            tag = CommFunc.ConvertDBNullToString(s1["E_Tag"]),
                            unit = CommFunc.ConvertDBNullToString(s1["E_Unit"]),
                        },
                        Ua_Tag = new
                        {
                            name = "电压采集点",
                            tag = CommFunc.ConvertDBNullToString(s1["U_Tag"]),
                            unit = CommFunc.ConvertDBNullToString(s1["U_Unit"]),
                        },
                        Ia_Tag = new
                        {
                            name = "电流采集点",
                            tag = CommFunc.ConvertDBNullToString(s1["I_Tag"]),
                            unit = CommFunc.ConvertDBNullToString(s1["I_Unit"]),
                        },
                        Pa_Tag = new
                        {
                            name = "有功功率",
                            tag = CommFunc.ConvertDBNullToString(s1["P_Tag"]),
                            unit = CommFunc.ConvertDBNullToString(s1["P_Unit"]),
                        },
                    };
                    object obj = new
                    {
                        RowId = dtSource.Rows.IndexOf(s1) + 1,
                        Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                        ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                        Status = CommFunc.ConvertDBNullToString(s1["Status"]),
                        FrMd = CommFunc.ConvertDBNullToInt32(s1["FrMd"]),
                        List = list,
                    };
                    rb.Add(obj);
                }
                //var res1 = from s1 in dtSource.AsEnumerable()
                //           select new
                //           {
                //               RowId = dtSource.Rows.IndexOf(s1) + 1,
                //               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                //               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                //               E_Tag = CommFunc.ConvertDBNullToString(s1["E_Tag"]),
                //               Ua_Tag = CommFunc.ConvertDBNullToString(s1["Ua_Tag"]),
                //               Ia_Tag = CommFunc.ConvertDBNullToString(s1["Ia_Tag"]),
                //               Pa_Tag = CommFunc.ConvertDBNullToString(s1["Pa_Tag"]),
                //               E_Unit = CommFunc.ConvertDBNullToString(s1["E_Unit"]),
                //               Ua_Unit = CommFunc.ConvertDBNullToString(s1["Ua_Unit"]),
                //               Ia_Unit = CommFunc.ConvertDBNullToString(s1["Ia_Unit"]),
                //               Pa_Unit = CommFunc.ConvertDBNullToString(s1["Pa_Unit"]),
                //               Status = CommFunc.ConvertDBNullToString(s1["Status"]),
                //               FrMd = CommFunc.ConvertDBNullToInt32(s1["FrMd"]),
                //           };
                rst.data = rb; // res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取监测数据信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 获取传感器数据列表
        /// </summary>     
        /// <returns></returns>
        public APIRst GetMonitorSensor200(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                List<object> rb = new List<object>();
                DataTable dtSource = bll.GetMonitorSensor200(user.CacheKey, co_id);
                foreach (DataRow s1 in dtSource.Rows)
                {
                    object list = new
                    {
                        Val_Tag = new
                        {
                            name = "缓存key",
                            tag = CommFunc.ConvertDBNullToString(s1["Val_Tag"]),
                            unit = CommFunc.ConvertDBNullToString(s1["Val_Unit"]),
                        }
                    };
                    object obj = new
                    {
                        RowId = dtSource.Rows.IndexOf(s1) + 1,
                        Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                        ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                        //FrMd = CommFunc.ConvertDBNullToInt32(s1["FrMd"]),
                        List = list,
                    };
                    rb.Add(obj);
                }
                rst.data = rb; // res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取监测数据信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        #endregion
    }
}