using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.PDU.Opertion.Mgr
{
    public partial class MgrHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.PDU.Mgr.MgrBLL bll = null;
        public MgrHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.PDU.Mgr.MgrBLL(user.Ledger, user.Uid);
        }

        /// <summary>
        /// PDU在线控制
        /// </summary>
        /// <returns></returns>
        public APIRst GetMgrStatus()
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetMgrStatus(user.CacheKey);
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
        /// PDU在线控制
        /// </summary>
        /// <returns></returns>
        public APIRst GetMgrCtrl(string moduleName)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetMgrCtrl(user.CacheKey, moduleName);
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
        /// PDU在线限制设置
        /// </summary>
        /// <returns></returns>
        public APIRst GetMgrLimit(string moduleName)
        {
            APIRst rst = new APIRst();
            try
            {
                //rst.data = bll.GetMgrLimit();
                DataTable dtSource = bll.GetMgrLimit(moduleName);
                foreach (DataRow dr in dtSource.Rows)
                {
                    dr["Status"] = this.GetStatus(CommFunc.ConvertDBNullToString(dr["Tag"]), CommFunc.ConvertDBNullToString(dr["DataValue"]), CommFunc.ConvertDBNullToDateTime(dr["Update_dt"]));
                }
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               Tag = CommFunc.ConvertDBNullToString(s1["Tag"]),
                               DataValue = CommFunc.ConvertDBNullToDecimal(s1["DataValue"]),
                               Status = CommFunc.ConvertDBNullToInt32(s1["Status"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("PDU在线限制设置错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private int GetStatus(string tag, string dataValue, DateTime update_dt)
        {
            string key = user.CacheKey + tag;
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
                value = CommFunc.ConvertDBNullToDecimal(var.lpszVal); // 值
            }
            if (!string.IsNullOrEmpty(dataValue))
            {
                decimal realVal = value == null ? -1 : value.Value;
                if (realVal == CommFunc.ConvertDBNullToDecimal(dataValue))
                {/*值相等设置成功*/
                    realStatus = 1;
                }
                else
                {
                    if (update_dt.AddMinutes(1) > DateTime.Now)
                    {/*正在设置*/
                        realStatus = 0;
                    }
                    else
                    {/*超过两分钟设置全部按缓存值*/
                        realStatus = -1;
                    }
                }            
            }
            return realStatus;
        }


    }
}