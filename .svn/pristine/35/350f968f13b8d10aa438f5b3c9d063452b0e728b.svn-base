using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Opertion.Dqhz
{
    public class DqhzHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Dqhz.DqhzBLL bll = null;
        public DqhzHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Dqhz.DqhzBLL(user.Ledger, user.Uid);
        }

        /// <summary>
        /// 电气火灾列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetDqhzList()
        {
            APIRst rst = new APIRst();
            try
            {
                List<object> list = new List<object>();
                DataTable dtSource = bll.GetDqhzList();
                var res1 = from s1 in dtSource.AsEnumerable()
                           group s1 by new { Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]) } into g1
                           select new
                           {
                               Module_id = g1.Key.Module_id,
                               ModuleName = CommFunc.ConvertDBNullToString(g1.First()["ModuleName"]),
                               MeterName = CommFunc.ConvertDBNullToString(g1.First()["MeterName"]),
                           };
                foreach (var o1 in res1)
                {
                    
                    List<object> rv = new List<object>();
                    foreach (DataRow dr in dtSource.Select("Module_id=" + o1.Module_id))
                    {
                        string lpszDbVarName = CommFunc.ConvertDBNullToString(dr["LpszDbVarName"]);
                        string funType = CommFunc.ConvertDBNullToString(dr["FunType"]);
                        string funName = CommFunc.ConvertDBNullToString(dr["FunName"]);
                        int scale = CommFunc.ConvertDBNullToInt32(dr["Scale"]);
                        RstVar vv = MemcachedMgr.GetVal<RstVar>(user.CacheKey + lpszDbVarName);
                        if (vv != null)
                        {
                            vv.lpszVal = CommFunc.ConvertDBNullToDecimal(vv.lpszVal).ToString("f" + scale);
                            rv.Add(new { FunType = funType, FunName = funName, Data = new { lpszVal = vv.lpszVal, lpszdateTime = vv.lpszdateTime } });
                        }
                        else
                        {
                            rv.Add(new { FunType = funType, FunName = funName, Data = "" });
                        }
                    }
                    list.Add(new { Module_id = o1.Module_id, MouleName = o1.ModuleName, MeterName = o1.MeterName, List = rv });
                }
                rst.data = list;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取电气火灾列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 参数下拉框
        /// </summary>
        /// <returns></returns>
        public APIRst GetFunTypes()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetFunTypes();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToString(s1["FunType"]),
                               text = CommFunc.ConvertDBNullToString(s1["FunName"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取参数下拉框:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取参数详情
        /// </summary>
        /// <returns></returns>
        public APIRst GetFunData(int module_id, DateTime date, string dateType, string funType)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetFunData(module_id,date,dateType, funType);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new { label = CommFunc.ConvertDBNullToString(s1["Time"]), value = s1["UseVal"] == DBNull.Value ? "" : CommFunc.ConvertDBNullToDecimal(s1["UseVal"]).ToString() };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取参数详情:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}