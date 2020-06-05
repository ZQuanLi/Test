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
        public APIRst GetBuildCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetModuleOfBuildList();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               Text = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取回路数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取能耗下拉列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetEnergyCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetEnergyCombox();
                DataRow addDr = dtSource.NewRow();
                addDr["EnergyItemCode"] = "";
                addDr["EnergyItemName"] = "请选择";
                dtSource.Rows.InsertAt(addDr, 0);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToString(s1["EnergyItemCode"]),
                               Text = CommFunc.ConvertDBNullToString(s1["EnergyItemName"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取能耗下拉列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取回路数据
        /// </summary>
        /// <returns></returns>
        public APIRst GetModuleList(int meter_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetModuleList(meter_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Meter_id = CommFunc.ConvertDBNullToInt32(s1["Meter_id"]),
                               MeterName = CommFunc.ConvertDBNullToString(s1["MeterName"]),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               EnergyItemCode = CommFunc.ConvertDBNullToString(s1["EnergyItemCode"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               BuildId = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               BuildName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Parent_id = CommFunc.ConvertDBNullToInt32(s1["Parent_id"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取回路数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设备属性下拉数据
        /// </summary>
        /// <returns></returns>
        public APIRst GetAttribCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = new DataTable();
                dtSource.Columns.Add("Id", typeof(System.Int32));
                dtSource.Columns.Add("Text", typeof(System.String));
                System.Reflection.FieldInfo[] fields = typeof(MdAttrib).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                foreach (System.Reflection.FieldInfo field in fields)
                {
                    MdAttrib aa = (MdAttrib)Enum.Parse(typeof(MdAttrib), field.Name);
                    var obj = field.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false);
                    if (obj != null && obj.Count() != 0)
                    {
                        System.ComponentModel.DataAnnotations.DisplayAttribute md = obj[0] as System.ComponentModel.DataAnnotations.DisplayAttribute;
                        dtSource.Rows.Add(new object[] { (int)aa, md.Name });
                    }
                }

                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToInt32(s1["Id"]),
                               Text = CommFunc.ConvertDBNullToString(s1["Text"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设备属性下拉数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取进行下拉列表数据
        /// </summary>
        /// <returns></returns>
        public APIRst GetIncomingCombox()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetIncomingCombox();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               Text = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取回路数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置回路信息列表
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="moduleName">回路名称</param>
        /// <param name="buildId">建筑ID号</param>
        /// <returns></returns>
        public APIRst SetModuleList(int module_id, string moduleName, int buildId, string energyItemCode,int parent_id = 0)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetModuleList(module_id,moduleName, buildId, energyItemCode, parent_id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取回路数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置回路信息列表(PDU)
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="moduleName">回路名称</param>
        /// <returns></returns>
        public APIRst SetModuleList_PDU(int module_id, string moduleName)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetModuleList_PDU(module_id,moduleName);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取回路数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取回路采集项列表
        /// </summary>
        /// <param name="meter_id"></param>
        /// <returns></returns>
        public APIRst GetModuleOfMapList(int meter_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetModuleOfMapList(meter_id);
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
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取回路采集项列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置回路采集项列表
        /// </summary>
        /// <param name="module_id">设备信息ID号</param>
        /// <param name="fun_id">设备采集项ID号</param>
        /// <param name="tagName">设备采集项映射变量</param>
        /// <returns></returns>
        public APIRst SetModuleOfMapList(int module_id, int fun_id, string tagName)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetModuleOfMapList(module_id, fun_id, tagName);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置回路采集项列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

    }
}