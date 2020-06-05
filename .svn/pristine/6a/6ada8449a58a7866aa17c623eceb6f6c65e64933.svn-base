using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YDS6000.Models;
using System.Data;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    partial class MonitorHelper
    {
        public APIRst GetYdParamsOfList(string strcName,string coName)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdParamsOfList(strcName, coName);
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               Fun_id = CommFunc.ConvertDBNullToInt32(s1["Fun_id"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               DataValue = CommFunc.ConvertDBNullToString(s1["DataValue"]),
                               Status = CommFunc.ConvertDBNullToInt32(s1["Status"]),
                               FunType = CommFunc.ConvertDBNullToString(s1["FunType"]),
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CoStrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           };                
                object obj = new { total = total, rows = res1.ToList() };
                rst.data = obj; 
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("电表设备参数列表错误 (GetYdParamsOfList)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIRst YdParamsOnBatch_FunType()
        {
            APIRst rst = new APIRst();            
            try
            {
                DataTable dtSource = bll.GetYdParamsOfFunType();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToString(s1["FunType"]),
                               text = CommFunc.ConvertDBNullToString(s1["FunName"])
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取参数项错误(YdParamsOnBatch_FunType)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIRst SetYdParams(int module_id,int fun_id,string funType,string dataValue)
        {
            APIRst rst = new APIRst();
            V0Fun vfun = V0Fun.E;
            if (module_id == 0)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "设备不能为空";
                return rst;
            }
            if (fun_id == 0 && string.IsNullOrEmpty(funType))
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "参数ID不能为空";
                return rst;
            }
            if (string.IsNullOrEmpty(dataValue))
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "设置参数不能为空";
                return rst;
            }
            if (!string.IsNullOrEmpty(funType) && Enum.TryParse(funType, false, out vfun) == false)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = "设置参数类型错误";
                return rst;
            }

            string msg = "";
            bool upDb = false;
            try
            {
                if (fun_id != 0)
                    rst.rst = new YdToGw(user.Ledger, user.Uid).YdToGwCmdOfSingle(module_id, fun_id, dataValue, out upDb, out msg);
                else
                    rst.rst = new YdToGw(user.Ledger, user.Uid).YdToGwCmd(module_id, vfun, dataValue);
                rst.data = upDb;
                rst.err.msg = msg;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("电表设备参数设置错误 (SetYdParams)", ex.Message + ex.StackTrace);
            }
            return rst;
        }

    }
}
