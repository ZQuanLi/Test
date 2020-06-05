using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Energy.Controllers
{
    public partial class MonitorHelper
    {

        /// <summary>
        /// 更换电表
        /// </summary>
        /// <param name="meter_id">设备信息ID号</param>
        /// <returns></returns>
        public APIRst GetModuleOfMapCollect(int meter_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetModuleOfMapCollect(meter_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               Fun_id = CommFunc.ConvertDBNullToInt32(s1["Fun_id"]),
                               MeterName = CommFunc.ConvertDBNullToString(s1["MeterName"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               FunType = CommFunc.ConvertDBNullToString(s1["FunType"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               TagName = CommFunc.ConvertDBNullToString(s1["TagName"]),
                               LastVal = CommFunc.ConvertDBNullToDecimal(s1["LastVal"]).ToString(),
                               LastTime = CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).ToString("yyyy-MM-dd HH:mm:ss"),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("更换电表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 保存更换电表
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="fun_id">采集ID号</param>
        /// <param name="lastVal">最后值</param>
        /// <returns></returns>
        public APIRst UpdateLastVal(int module_id, int fun_id, decimal lastVal)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.UpdateLastVal(module_id, fun_id, lastVal);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("保存更换电表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}