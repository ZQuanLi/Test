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
        /// 获取功能码类型数据
        /// </summary>
        /// <returns></returns>
        public APIRst GetMeterList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetMeterList();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Meter_id = CommFunc.ConvertDBNullToInt32(s1["Meter_id"]),
                               MeterName = CommFunc.ConvertDBNullToString(s1["MeterName"]),
                               MeterNo = CommFunc.ConvertDBNullToString(s1["MeterNo"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               MeterTypeId = CommFunc.ConvertDBNullToString(s1["Mm_id"]),
                               MeterTypeName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               Attrib = CommFunc.ConvertDBNullToInt32(s1["Attrib"]),
                               AttribName = CommFunc.GetEnumDisplay(typeof(MdAttrib), CommFunc.ConvertDBNullToInt32(s1["Attrib"]))
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取功能码类型数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置设备信息
        /// </summary>
        /// <returns></returns>
        public APIRst SetMeter(MdVModel md)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetMeter(md);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置设备信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public APIRst DelMeter(int id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.DelMeter(id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除设备信息信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取设备列表(PDU)
        /// </summary>
        /// <returns></returns>
        public APIRst GetMeterList_PDU()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetMeterList_PDU();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Meter_id = CommFunc.ConvertDBNullToInt32(s1["Meter_id"]),
                               MeterName = CommFunc.ConvertDBNullToString(s1["MeterName"]),
                               MeterNo = CommFunc.ConvertDBNullToString(s1["MeterNo"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               MeterTypeId = CommFunc.ConvertDBNullToString(s1["Mm_id"]),
                               MeterTypeName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               Co_id = CommFunc.ConvertDBNullToString(s1["Co_id"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Parent_id = CommFunc.ConvertDBNullToString(s1["Parent_id"]),
                               Parent_MeterName = CommFunc.ConvertDBNullToString(s1["Parent_MeterName"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取功能码类型数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置设备信息(PDU)
        /// </summary>
        /// <returns></returns>
        public APIRst SetMeter_PDU(MdVModel_PDU md)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetMeter_PDU(md);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置设备信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取父设备下拉列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetParentMeterCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetParentMeterCombox();
                DataRow addDr = dtSource.NewRow();
                addDr["Module_id"] = 0;
                addDr["MeterName"] = "请选择";
                dtSource.Rows.InsertAt(addDr, 0);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToString(s1["Module_id"]),
                               Text = CommFunc.ConvertDBNullToString(s1["MeterName"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取父设备下拉列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}