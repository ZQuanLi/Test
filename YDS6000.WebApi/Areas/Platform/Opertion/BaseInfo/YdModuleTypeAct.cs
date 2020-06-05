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
        public APIRst GetMmDefineList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetMmDefineList();
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
                FileLog.WriteLog("获取功能码类型数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取设备型号信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetModuleTypeList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetModuleTypeList();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               ModuleTypeId = CommFunc.ConvertDBNullToInt32(s1["Mm_id"]),
                               ModuleTypeName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               IsDefine = CommFunc.ConvertDBNullToInt32(s1["IsDefine"]),
                               IsDefineName = CommFunc.GetEnumDisplay(typeof(MmDefine), CommFunc.ConvertDBNullToInt32(s1["IsDefine"]))
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取设备型号信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置设备型号信息
        /// </summary>
        /// <returns></returns>
        public APIRst SetModuleType(ModuleTypeVModel mtype)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetModuleType(mtype);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置设备型号信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除设备型号信息
        /// </summary>
        /// <returns></returns>
        public APIRst DelModuleType(int id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.DelModuleType(id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除设备型号信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 获取设备型号采集点信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetModuleFunList(int mm_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetModuleFunList(mm_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               ModuleTypeId = CommFunc.ConvertDBNullToInt32(s1["Mm_id"]),
                               Fun_id = CommFunc.ConvertDBNullToInt32(s1["Fun_id"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               FunType = CommFunc.ConvertDBNullToString(s1["FunType"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取设备型号采集点信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置采集点信息
        /// </summary>
        /// <returns></returns>
        public APIRst SetModuleFun(ModuleFunVModel fun)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetModuleFun(fun);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置采集点信息:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除设备型号采集点信息
        /// </summary>
        /// <returns></returns>
        public APIRst DelModuleFun(int id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.DelModuleFun(id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除设备型号采集点信息:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

    }
}