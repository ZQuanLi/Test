using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Platform.Controllers
{
    partial class BaseInfoHelper
    {
        /// <summary>
        /// 获取项目信息列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetProList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetProList();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1)+1,
                               Id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               ProName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               Area = CommFunc.ConvertDBNullToDecimal(s1["Area"]),
                               ProAddr = CommFunc.ConvertDBNullToString(s1["CustAddr"]),
                               Person = CommFunc.ConvertDBNullToString(s1["CustName"]),
                               TelNo = CommFunc.ConvertDBNullToString(s1["Mobile"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取项目信息列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置项目信息信息
        /// </summary>
        /// <param name="pro">角色信息</param>
        /// <returns></returns>
        public APIRst SetPro(ProVModel pro)
        {
            APIRst rst = new APIRst();
            try
            {
                bll.SetPro(pro);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置项目信息信息错误(SetPro):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除项目信息
        /// </summary>
        /// <param name="co_id">角色ID号</param>
        /// <returns></returns>
        public APIRst DelPro(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                bll.DelPro(co_id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除项目信息错误(DelPro):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}