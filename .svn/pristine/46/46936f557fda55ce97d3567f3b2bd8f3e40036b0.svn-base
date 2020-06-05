using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Threading.Tasks;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.SystemMgr.Controllers
{
    partial class BaseInfoHelper
    {
        /// <summary>
        /// 获取集中器信息
        /// </summary>
        /// <returns></returns>
        public APIResult GetEspList(int esp_id, string espName)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetEspList(esp_id, espName);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Esp_id = CommFunc.ConvertDBNullToInt32(s1["Esp_id"]),
                               EspName = CommFunc.ConvertDBNullToString(s1["EspName"]),
                               EspAddr = CommFunc.ConvertDBNullToString(s1["EspAddr"]),
                               //EspIp = CommFunc.ConvertDBNullToString(s1["EspIp"]),
                               //EspPort = CommFunc.ConvertDBNullToInt32(s1["EspPort"]),
                               //TransferType = CommFunc.ConvertDBNullToInt32(s1["TransferType"]),
                               //TransferTypeName = CommFunc.GetEnumDisplay(typeof(TfAttrib), CommFunc.ConvertDBNullToInt32(s1["TransferType"]))
                               EspType = CommFunc.ConvertDBNullToString(s1["EspType"]),
                               Inst_loc = CommFunc.ConvertDBNullToString(s1["Inst_loc"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               Supplier = CommFunc.ConvertDBNullToString(s1["Supplier"]),
                               ActiveTime = CommFunc.ConvertDBNullToDateTime(s1["ActiveTime"]).ToString("yyyy-MM-dd HH:mm:ss")
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = obj;
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取获取集中器信息错误(GetEspList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置集中器信息
        /// </summary>
        /// <param name="esp">集中器信息</param>
        /// <returns></returns>
        public APIResult SetEsp(EspVModel esp)
        {
            APIResult rst = new APIResult();
            try
            {
                bll.SetEsp(esp);
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = this.GetEspList(esp.Esp_id,"");
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("设置集中器信息错误(SetEsp):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置集中器信息
        /// </summary>
        /// <param name="esp_id">集中器ID信息</param>
        /// <returns></returns>
        public APIResult DelEsp(int esp_id)
        {
            APIResult rst = new APIResult();
            try
            {
                bll.DelEsp(esp_id);
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = esp_id;
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("删除集中器信息错误(DelEsp):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        #region 设备型号信息
        /// <summary>
        /// 获取设备类型下拉列表
        /// </summary>
        /// <returns></returns>
        public APIResult GetMeterTypeComBox()
        {           
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = XMLCofig.GetDrive();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToString(s1["type"]),
                               Text = CommFunc.ConvertDBNullToString(s1["type"]),
                           };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { total = dtSource.Rows.Count, rows = res1.ToList() };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取设备类型数据信息错误(GetModuleTypeData):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取设备类型列表
        /// </summary>
        /// <returns></returns>
        public APIResult GetMmList(int mm_id,string moduleTypeName)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetMmList(mm_id, moduleTypeName);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               ModuleTypeId = CommFunc.ConvertDBNullToInt32(s1["Mm_id"]),
                               ModuleType = CommFunc.ConvertDBNullToString(s1["ModuleType"]),
                               ModuleTypeName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               Supplier = CommFunc.ConvertDBNullToString(s1["Fty_prod"]),// 生产厂商 供应商
                               Spec = CommFunc.ConvertDBNullToString(s1["Spec"]),//
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                           };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { total = dtSource.Rows.Count, rows = res1.ToList() };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取设备类型列表信息错误(GetMmList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置设备类型
        /// </summary>
        /// <returns></returns>
        public APIResult SetMm(MeterTypeVModel mmType)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtFun = this.GetMmFunTypeList(mmType.ModuleTypeId, mmType.ModuleType);
                bll.SetMm(mmType, dtFun);
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = this.GetMmList(mmType.ModuleTypeId, "");
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取设备类型列表信息错误(GetMmList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 删除型号
        /// </summary>
        /// <param name="mm_id"></param>
        /// <returns></returns>
        public APIResult DelMm(int mm_id)
        {
            APIResult rst = new APIResult();
            try
            {
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = bll.DelMm(mm_id);
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("删除型号信息错误(DelMm):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取设备类型采集码列表
        /// </summary>
        /// <returns></returns>
        public APIResult GetMmFunTypeList(int mm_id)
        {
            APIResult rst = new APIResult();
            try
            {
                string moduleType = bll.GetMmType(mm_id);
                DataTable dtSource = XMLCofig.GetDrive(moduleType);
                dtSource.Columns.Add("Fun_id", typeof(System.Int32));
                dtSource.Columns.Add("Mm_id", typeof(System.Int32));
                dtSource.Columns.Add("DataValue", typeof(System.String));
                dtSource.Columns.Add("Disabled", typeof(System.Int32));
                DataTable dtDb = bll.GetMmFunTypeList(mm_id);
                foreach (DataRow dr in dtSource.Rows)
                {
                    dr["Fun_id"] = 0;
                    dr["Mm_id"] = mm_id;
                    foreach (DataRow drDb in dtDb.Select("FunType='" + CommFunc.ConvertDBNullToString(dr["FunType"]) + "'"))
                    {
                        dr["Fun_id"] = drDb["Fun_id"];
                        dr["DataValue"] = drDb["DataValue"];
                        dr["Disabled"] = drDb["Disabled"];
                    }
                }
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Fun_id = CommFunc.ConvertDBNullToInt32(s1["Fun_id"]),
                               Mm_id = CommFunc.ConvertDBNullToInt32(s1["Mm_id"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               FunType = CommFunc.ConvertDBNullToString(s1["FunType"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                           };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { total = dtSource.Rows.Count, rows = res1.ToList() };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取设备类型采集码列表信息错误(GetMmFunTypeList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private DataTable GetMmFunTypeList(int mm_id,string moduleType)
        {
            DataTable dtSource = XMLCofig.GetDrive(moduleType);
            dtSource.Columns.Add("Fun_id", typeof(System.Int32));
            dtSource.Columns.Add("Mm_id", typeof(System.Int32));
            dtSource.Columns.Add("DataValue", typeof(System.String));
            dtSource.Columns.Add("Disabled", typeof(System.Int32));
            DataTable dtDb = bll.GetMmFunTypeList(mm_id);
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["Fun_id"] = 0;
                dr["Mm_id"] = mm_id;
                foreach (DataRow drDb in dtDb.Select("FunType='" + CommFunc.ConvertDBNullToString(dr["FunType"]) + "'"))
                {
                    dr["Fun_id"] = drDb["Fun_id"];
                    dr["DataValue"] = drDb["DataValue"];
                    dr["Disabled"] = drDb["Disabled"];
                }
            }
            return dtSource;
        }
        #endregion

        #region 设备信息
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="meter_id">设备信息ID号</param>
        /// <param name="meterName">设备名称</param>
        /// <returns></returns>
        public APIResult GetMeterList(int meter_id, string meterName)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetMeterList(meter_id, meterName);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Meter_id = CommFunc.ConvertDBNullToInt32(s1["Meter_id"]),
                               Esp_id = CommFunc.ConvertDBNullToString(s1["Esp_id"]),
                               MeterAddr = CommFunc.ConvertDBNullToString(s1["MeterAddr"]),
                               MeterName = CommFunc.ConvertDBNullToString(s1["MeterName"]),
                               Inst_loc = CommFunc.ConvertDBNullToString(s1["Inst_loc"]),
                               Supplier = CommFunc.ConvertDBNullToString(s1["Supplier"]),
                               //Switch = CommFunc.ConvertDBNullToString(s1["Switch"]),
                               //ChrgType = CommFunc.ConvertDBNullToInt32(s1["ChrgType"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               Multiply = CommFunc.ConvertDBNullToDecimal(s1["Multiply"]),
                               RoomId = CommFunc.ConvertDBNullToInt32(s1["RoomId"]),
                               MeterTypeId = CommFunc.ConvertDBNullToInt32(s1["MeterTypeId"]),
                               MeterType = CommFunc.ConvertDBNullToString(s1["MeterType"]),
                               MeterTypeName = CommFunc.ConvertDBNullToString(s1["MeterTypeName"]),
                               RoomName = CommFunc.ConvertDBNullToString(s1["RoomName"]),
                               //ChrgTypeName = CommFunc.GetEnumDisplay(typeof(ChargAttrib), CommFunc.ConvertDBNullToInt32(s1["ChrgType"])),
                               EspName = CommFunc.ConvertDBNullToString(s1["EspName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = obj;
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取设备信息信息错误(GetMeterList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置设备信息
        /// </summary>
        /// <param name="meter">设备信息</param>
        /// <returns></returns>
        public APIResult SetMeter(MeterVModel meter)
        {
            APIResult rst = new APIResult();
            try
            {
                int cnt = bll.SetMeter(meter);
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = this.GetMeterList(meter.Meter_id, "");
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("设置设备信息错误(SetMeter):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置设备信息
        /// </summary>
        /// <param name="meter_id">设备ID信息</param>
        /// <returns></returns>
        public APIResult DelMeter(int meter_id)
        {
            APIResult rst = new APIResult();
            try
            {                
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = bll.DelMeter(meter_id);
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("删除设备信息错误(DelMeter):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取付款方式列表
        /// </summary>
        /// <returns></returns>
        public APIResult GetChrgTypeList()
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetChrgTypeList();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToInt32(s1["Id"]),
                               Text = CommFunc.ConvertDBNullToString(s1["Text"]),
                           };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { total = dtSource.Rows.Count, rows = res1.ToList() };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取支付方式列表信息错误(GetChrgTypeList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        #endregion

        #region  回路信息
        /// <summary>
        /// 获取回路信息
        /// </summary>
        /// <param name="module_id">设备信息ID号</param>
        /// <param name="moduleName">设备名称</param>
        /// <returns></returns>
        public APIResult GetModuleList(int module_id, string moduleName)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetModuleList(module_id, moduleName);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),                            
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               PSWay = CommFunc.ConvertDBNullToString(s1["Switch"]),
                               ChrgType = CommFunc.ConvertDBNullToInt32(s1["ChrgType"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               Meter_id = CommFunc.ConvertDBNullToString(s1["Meter_id"]),
                               MeterName = CommFunc.ConvertDBNullToString(s1["MeterName"]),
                               PSWayName = CommFunc.GetEnumDisplay(typeof(Switch), CommFunc.ConvertDBNullToString(s1["Switch"])),
                               ChrgTypeName = CommFunc.GetEnumDisplay(typeof(ChargAttrib), CommFunc.ConvertDBNullToInt32(s1["ChrgType"])),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = obj;
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取回路信息信息错误(GetModuleList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 设置回路信息
        /// </summary>
        /// <param name="module">回路信息</param>
        /// <returns></returns>
        public APIResult SetModule(ModuleVModel module)
        {
            APIResult rst = new APIResult();
            try
            {
                int cnt = bll.SetModule(module);
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = this.GetModuleList(module.Meter_id, "");
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("设置设备信息错误(SetMeter):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 删除回路信息
        /// </summary>
        /// <param name="module_id">回路ID信息</param>
        /// <returns></returns>
        public APIResult DelModule(int module_id)
        {
            APIResult rst = new APIResult();
            try
            {
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = bll.DelModule(module_id);
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("删除回路信息错误(DelModule):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        #endregion

        #region 回路变量映射

        /// <summary>
        /// 获取采集码列表
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <returns></returns>
        public APIResult GetMapFunTypeList(int module_id,int fun_id)
        {
            APIResult rst = new APIResult();
            try
            {
                DataTable dtSource = bll.GetMapFunTypeList(module_id, fun_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               Fun_id = CommFunc.ConvertDBNullToInt32(s1["Fun_id"]),
                               FunType = CommFunc.ConvertDBNullToString(s1["FunType"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               TagName = CommFunc.ConvertDBNullToString(s1["TagName"]),
                           };
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = new { total = dtSource.Rows.Count, rows = res1.ToList() };
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("获取采集码列表信息错误(GetMapFunTypeList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 映射采集码对应的TagName
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public APIResult SetMapFunType(MapVModel tag)
        {
            APIResult rst = new APIResult();
            try
            {
                int cnt = bll.SetMapFunType(tag);
                rst.Code = 0;
                rst.Msg = "";
                rst.Data = this.GetMapFunTypeList(tag.Module_id,tag.Fun_id);
            }
            catch (Exception ex)
            {
                rst.Code = -1;
                rst.Msg = ex.Message;
                FileLog.WriteLog("映射采集码对应的TagName信息错误(SetMapFunType):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        #endregion





    }
}