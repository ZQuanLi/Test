using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using System.Text;
using YDS6000.Models.Tables;

namespace YDS6000.WebApi.Areas.Exp.Opertion.Syscont
{
    public partial class ExpYdModuleHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Syscont.ExpYdModuleBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpYdModuleHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Syscont.ExpYdModuleBLL(user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 获取集中器数据
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdModuleOnGatewayList()
        {
            APIRst rst = new APIRst();
            try
            {
                int total = 0;
                object rows = this.GetYdModuleOnGatewayList(0, out total);
                object obj = new { total = total, rows = rows };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取集中器数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取采集列表
        /// </summary>
        /// <param name="Gw_id"></param>
        /// <returns></returns>
        public APIRst GetYdModuleOnDetail(int Gw_id)
        {
            APIRst rst = new APIRst();
            try
            {
                int total = 0;
                object rows = this.GetYdModuleOnDetail(Gw_id, 0, 0, 0, out total);
                object obj = new { total = total, rows = rows };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取采集数据据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 保存集中器信息
        /// </summary>
        /// <param name="gw">对象</param>
        /// <returns></returns>
        public APIRst YdModuleOnSaveGw(v1_gatewayVModel gw)
        {
            APIRst rst = new APIRst();
            try
            {
                if (string.IsNullOrEmpty(gw.GwName))
                    throw new Exception("集中器名称不能为空");
                var gwid = bll.IsExistYdGwName(gw.GwName);
                if (gwid > 0 && (gw.Gw_id == 0 || (gw.Gw_id > 0 && gw.Gw_id != gwid)))
                    throw new Exception("集中器名称：" + gw.GwName + "重复，请查询后再操作");
                if (string.IsNullOrEmpty(gw.GwAddr))
                    throw new Exception("集中器地址不能为空");
                gwid = bll.IsExistYdGwAddr(gw.GwAddr);
                if (gwid > 0 && (gw.Gw_id == 0 || (gw.Gw_id > 0 && gw.Gw_id != gwid)))
                    throw new Exception("集中器地址：" + gw.GwAddr + "重复，请查询后再操作");
                if (gw.Timeout == 0)
                    throw new Exception("集中器延时设置(毫秒)不能为零");

                int total = 0;
                bll.YdModuleOnSaveGw(gw);
                object rows = this.GetYdModuleOnGatewayList(gw.Gw_id, out total);

                object obj = new { total = total, rows = rows };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置集中器数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除集中器信息
        /// </summary>
        /// <param name="Gw_id"></param>
        /// <returns></returns>
        public APIRst YdModuleOnDelGw(int Gw_id)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Gw_id == 0)
                    throw new Exception("Gw_ID不能为空");
                int cc = bll.YdModuleOnDelGw(Gw_id);
                rst.data = cc;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除集中器信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 保存采集器数据
        /// </summary>
        /// <param name="ComPortNum"></param>
        /// <param name="ep">对象：Esp_id=采集器编号,Gw_id=集中器序号,EspName=采集器名称,EspAddr=采集器地址,TransferType=通讯方式,Baud=波特率,DataBit=数据位,StopBit=停止位,Parity=校验方式,EspPort=端口,Timeout=超时秒,Inst_loc=安装位置,Remark=备注</param>
        /// <returns></returns>
        public APIRst YdModuleOnSaveEsp(int ComPortNum, v1_gateway_espVModel ep)
        {
            APIRst rst = new APIRst();
            try
            {
                if (ep.TransferType == 0)
                    ep.ComPort = "COM" + ComPortNum;//COM口
                else
                    ep.ComPort = "";

                if (ep.Gw_id == 0)
                    throw new Exception("所属集中器错误");
                if (string.IsNullOrEmpty(ep.EspName))
                    throw new Exception("采集器名称不能为空");

                int espid = bll.IsExistYdEspName(ep.Gw_id, ep.Esp_id, ep.EspName, ep.Esp_id > 0);

                if (espid > 0 && (ep.Esp_id == 0 || (ep.Esp_id > 0 && ep.Esp_id != espid)))
                    throw new Exception("采集器名称" + ep.EspName + "重复，请查询后再操作");
                if (string.IsNullOrEmpty(ep.EspAddr))
                    throw new Exception("采集器地址不能为空");
                if (ep.Timeout == 0)
                    throw new Exception("采集器延时设置(毫秒)不能为零");
                if (ep.TransferType == 0 && ComPortNum == 0)
                    throw new Exception("通讯方式为COM口,不能为空");
                if (ep.TransferType != 0 && ep.EspPort == 0)
                    throw new Exception("通讯方式为网络方式,TCP端口不能为空");

                int total = 0;
                bll.YdModuleOnSaveEsp(ep);
                object rows = this.GetYdModuleOnDetail(0, ep.Esp_id, 0, 0, out total);

                object obj = new { total = total, rows = rows };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("保存采集器数据:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除采集器数据
        /// </summary>
        /// <param name="Esp_id"></param>
        /// <returns></returns>
        public APIRst YdModuleOnDelEsp(int Esp_id)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Esp_id == 0)
                    throw new Exception("Esp_id不能为空");
                int cc = bll.YdModuleOnDelEsp(Esp_id);
                rst.data = cc;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除采集器数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获设备信息列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdModuleOnModelList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdModuleOnModelList();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Mm_id"]),
                               text = CommFunc.ConvertDBNullToInt32(s1["Mm_id"]) == 0 ? "请选择" : CommFunc.ConvertDBNullToString(s1["ModuleType"]),

                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取类型列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 保存设备数据
        /// </summary>
        /// <param name="md">对象：Meter_id,Esp_id,MeterName=设备名称,MeterAddr=设备地址,MeterNo=设备编号,Mm_id=设备类型,Inst_loc=安装位置,Multiply=设备倍率,Remark=备注,Disabled=是否弃用</param>
        /// <returns></returns>
        public APIRst YdModuleOnSaveMm(v1_gateway_esp_meterVModel md)
        {
            APIRst rst = new APIRst();
            try
            {
                if (md.Esp_id == 0)
                    throw new Exception("所属采集器错误");
                if (string.IsNullOrEmpty(md.MeterName))
                    throw new Exception("设备名称不能为空");
                //if (string.IsNullOrEmpty(md.MeterAddr))
                //    throw new Exception("设备地址不能为空");
                //if (md.Multiply == 0)
                //    throw new Exception("设备倍率不能为零");
                //if (md.Mm_id == 0)
                //    throw new Exception("设备类型不能为空");

                int total = 0;
                bll.YdModuleOnSaveMm(md);
                object rows = this.GetYdModuleOnDetail(0, 0, md.Meter_id, 0, out total);
                object obj = new { total = total, rows = rows };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("保存设备数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除设备数据
        /// </summary>
        /// <param name="Meter_id"></param>
        /// <returns></returns>
        public APIRst YdModuleOnDelMm(int Meter_id)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Meter_id == 0)
                    throw new Exception("Meter_id不能为空");
                int cc = bll.YdModuleOnDelMm(Meter_id);
                rst.data = cc;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除设备数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置回路信息(电能表信息)
        /// </summary>
        /// <param name="md">对象：Module_id,Meter_id=设备ID号,ModuleName=电能表名称,ModuleAddr=设备地址(电能表),Disabled=是否弃用(默认0),Remark=备注,EnergyItemCode=分类分项编码(01000),Create_by=建立人,Create_dt=建立时间,Update_by=更新人,Update_dt=更新人</param>
        /// <returns></returns>
        public APIRst YdModuleOnSaveMd(v1_gateway_esp_moduleVModel md)
        {
            APIRst rst = new APIRst();
            try
            {
                if(md.Meter_id == 0)
                    throw new Exception("所属设备错误");
                if (string.IsNullOrEmpty(md.ModuleName))
                    throw new Exception("回路称不能为空");
                if (string.IsNullOrEmpty(md.ModuleAddr))
                    throw new Exception("回路地址不能为空");

                int total = 0;
                bll.YdModuleOnSaveMd(md);
                object rows = this.GetYdModuleOnDetail(0, 0, 0, md.Module_id, out total);
                object obj = new { total = total, rows = rows };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置回路信息(电能表信息)错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除回路信息(电能表信息)
        /// </summary>
        /// <param name="Module_id"></param>
        /// <returns></returns>
        public APIRst YdModuleOnDelMd(int Module_id)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Module_id == 0)
                    throw new Exception("Module_id不能为空");
                int cc = bll.YdModuleOnDelMd(Module_id);
                rst.data = cc;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除回路信息(电能表信息):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取映射
        /// </summary>
        /// <param name="Module_id"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        public APIRst GetYdModuleOnMapList(int Module_id, int Action)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdModuleOnMapList(Module_id, Action);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = (dtSource.Rows.IndexOf(s1) + 1),
                               Fun_id = CommFunc.ConvertDBNullToInt32(s1["Fun_id"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               FunType = CommFunc.ConvertDBNullToString(s1["FunType"]),
                               TagName = CommFunc.ConvertDBNullToString(s1["TagName"]),
                               DataValue = CommFunc.ConvertDBNullToString(s1["DataValue"]),
                               Action = CommFunc.ConvertDBNullToInt32(s1["Action"]),
                               Status = CommFunc.ConvertDBNullToInt32(s1["Status"]),
                               StatusS = CommFunc.ConvertDBNullToInt32(s1["Status"]) == 1 ? "已设置" : "未设置",
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               @checked = CommFunc.ConvertDBNullToInt32(s1["Disabled"])
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取映射类型错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 保存映射
        /// </summary>
        /// <param name="Module_id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public APIRst YdModuleOnMapInSave(int Module_id, DataModels data)
        {
            APIRst rst = new APIRst();
            StringBuilder tagName = new StringBuilder();
            try
            {
                int cnt = 0;
                YdToGw gw = new YdToGw(user.Ledger, user.Uid);
                DataTable dtSource = JsonHelper.ToDataTable(data.Data);
                foreach (DataRow dr in dtSource.Rows)
                {
                    int action = CommFunc.ConvertDBNullToInt32(dr["Action"]);
                    cnt = bll.YdModuleOnMapInSave(Module_id, dr);
                    if (action == 0)
                    {
                        if (!string.IsNullOrEmpty(tagName.ToString()))
                            tagName.Append(",");
                        tagName.Append(CommFunc.ConvertDBNullToString(dr["TagName"]));
                    }
                    else
                    {
                        if (cnt == 1)
                        {
                            V0Fun fun = V0Fun.E;
                            if (Enum.TryParse<V0Fun>(CommFunc.ConvertDBNullToString(dr["FunType"]), out fun) == true)
                                gw.BeginYdToGwCmd(Module_id, CommFunc.ConvertDBNullToInt32(dr["Fun_id"]), fun, CommFunc.ConvertDBNullToString(dr["DataValue"]));
                        }
                    }
                }
                if (!string.IsNullOrEmpty(tagName.ToString()))
                    gw.BeginYdToGwConfig(tagName.ToString());                
                
                rst.data = cnt;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("映射保存错误(YdModuleOnMapInSave):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }    
        
        private object GetYdModuleOnGatewayList(int Gw_id, out int total)
        {
            DataTable dtSource = bll.GetYdModuleOnV1_gateway(Gw_id);
            total = dtSource.Rows.Count;
            var res1 = from s1 in dtSource.AsEnumerable()
                       select new
                       {
                           RowId = (dtSource.Rows.IndexOf(s1) + 1),
                           Gw_id = CommFunc.ConvertDBNullToInt32(s1["Gw_id"]),
                           GwName = CommFunc.ConvertDBNullToString(s1["GwName"]),
                           GwAddr = CommFunc.ConvertDBNullToString(s1["GwAddr"]),
                           GwIp = CommFunc.ConvertDBNullToString(s1["GwIp"]),
                           GwPort = CommFunc.ConvertDBNullToInt32(s1["GwPort"]),
                           Timeout = CommFunc.ConvertDBNullToInt32(s1["Timeout"]),
                           GwType = CommFunc.ConvertDBNullToString(s1["GwType"]),
                           Inst_loc = CommFunc.ConvertDBNullToString(s1["Inst_loc"]),
                           Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                           Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                       };
            return res1.ToList();
        }

        // 采集器数据集合
        private object GetYdModuleOnDetail(int Gw_id, int Esp_id, int Meter_id, int Module_id, out int total)
        {
            DataTable dtSource = bll.GetYdModuleOnDetail(Gw_id, Esp_id, Meter_id, Module_id);
            total = dtSource.Rows.Count;
            var res1 = from s1 in dtSource.AsEnumerable()
                       orderby CommFunc.ConvertDBNullToInt32(s1["Id"])
                       select new
                       {
                           id = CommFunc.ConvertDBNullToInt32(s1["Id"]),
                           _parentId = CommFunc.ConvertDBNullToInt32(s1["Pid"]),
                           Gw_id = CommFunc.ConvertDBNullToInt32(s1["Gw_id"]),
                           Esp_id = CommFunc.ConvertDBNullToInt32(s1["Esp_id"]),
                           Meter_id = CommFunc.ConvertDBNullToInt32(s1["Meter_id"]),
                           Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                           EmName = CommFunc.ConvertDBNullToString(s1["EmName"]),
                           EmAddr = CommFunc.ConvertDBNullToString(s1["EmAddr"]),
                           EmNo = CommFunc.ConvertDBNullToString(s1["EmNo"]),
                           TransferType = CommFunc.ConvertDBNullToInt32(s1["TransferType"]),
                           EspIp = CommFunc.ConvertDBNullToString(s1["EspIp"]),
                           EspPort = CommFunc.ConvertDBNullToInt32(s1["EspPort"]),
                           Timeout = CommFunc.ConvertDBNullToInt32(s1["Timeout"]),
                           ComPort = CommFunc.ConvertDBNullToString(s1["ComPort"]),
                           ComPortNum = CommFunc.ConvertDBNullToString(s1["ComPort"]).Replace("COM", ""),
                           Baud = CommFunc.ConvertDBNullToInt32(s1["Baud"]),
                           DataBit = CommFunc.ConvertDBNullToInt32(s1["DataBit"]),
                           StopBit = CommFunc.ConvertDBNullToInt32(s1["StopBit"]),
                           Parity = CommFunc.ConvertDBNullToInt32(s1["Parity"]),
                           Mm_id = CommFunc.ConvertDBNullToInt32(s1["Mm_id"]),
                           EmType = CommFunc.ConvertDBNullToString(s1["EmType"]),
                           Inst_loc = CommFunc.ConvertDBNullToString(s1["Inst_loc"]),
                           Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                           Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                           Multiply = CommFunc.ConvertDBNullToDecimal(s1["Multiply"]),
                           EnergyItemCode = CommFunc.ConvertDBNullToString(s1["EnergyItemCode"]),
                           Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                           CoName = CommFunc.ConvertDBNullToInt32(s1["CoName"]),
                           IsAlarm = CommFunc.ConvertDBNullToInt32(s1["IsAlarm"]),
                           MinPay = CommFunc.ConvertDBNullToInt32(s1["MinPay"]),
                           Price = CommFunc.ConvertDBNullToDecimal(s1["Price"]),
                           Rate_id = CommFunc.ConvertDBNullToInt32(s1["Rate_id"]),
                       };
            return res1.ToList();
        }

        private object GetYdModuleOnDetail_bk(int Gw_id, int Esp_id, int Module_id, out int total)
        {
            if (Gw_id != 0)
            {
                DataTable dtSource = bll.GetYdModuleOnDetail(Gw_id, 0, 0, 0);
                total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           orderby CommFunc.ConvertDBNullToInt32(s1["Id"])
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Id"]),
                               _parentId = CommFunc.ConvertDBNullToInt32(s1["Pid"]),
                               Gw_id = CommFunc.ConvertDBNullToInt32(s1["Gw_id"]),
                               Esp_id = CommFunc.ConvertDBNullToInt32(s1["Esp_id"]),
                               Meter_id = CommFunc.ConvertDBNullToInt32(s1["Meter_id"]),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               EmName = CommFunc.ConvertDBNullToString(s1["EmName"]),
                               EmAddr = CommFunc.ConvertDBNullToString(s1["EmAddr"]),
                               TransferType = CommFunc.ConvertDBNullToInt32(s1["TransferType"]),
                               EspIp = CommFunc.ConvertDBNullToString(s1["EspIp"]),
                               EspPort = CommFunc.ConvertDBNullToInt32(s1["EspPort"]),
                               Timeout = CommFunc.ConvertDBNullToInt32(s1["Timeout"]),
                               ComPort = CommFunc.ConvertDBNullToString(s1["ComPort"]),
                               ComPortNum = CommFunc.ConvertDBNullToString(s1["ComPort"]).Replace("COM", ""),
                               Baud = CommFunc.ConvertDBNullToInt32(s1["Baud"]),
                               DataBit = CommFunc.ConvertDBNullToInt32(s1["DataBit"]),
                               StopBit = CommFunc.ConvertDBNullToInt32(s1["StopBit"]),
                               Parity = CommFunc.ConvertDBNullToInt32(s1["Parity"]),
                               Mm_id = CommFunc.ConvertDBNullToInt32(s1["Mm_id"]),
                               EmType = CommFunc.ConvertDBNullToString(s1["EmType"]),
                               Inst_loc = CommFunc.ConvertDBNullToString(s1["Inst_loc"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                               Multiply = CommFunc.ConvertDBNullToDecimal(s1["Multiply"]),
                               EnergyItemCode = CommFunc.ConvertDBNullToString(s1["EnergyItemCode"]),
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CoName = CommFunc.ConvertDBNullToInt32(s1["CoName"]),
                               IsAlarm = CommFunc.ConvertDBNullToInt32(s1["IsAlarm"]),
                               MinPay = CommFunc.ConvertDBNullToInt32(s1["MinPay"]),
                               Price = CommFunc.ConvertDBNullToDecimal(s1["Price"]),
                               Rate_id = CommFunc.ConvertDBNullToInt32(s1["Rate_id"]),
                           };
                return res1.ToList();
            }
            else if (Esp_id != 0)
            {
                DataTable dtSource = bll.GetYdModuleOnV1_gateway_esp(Esp_id);
                total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = (CommFunc.ConvertDBNullToInt32(s1["Esp_id"]) * 10).ToString() + "0".PadLeft(5, '0'),
                               _parentId = 0,
                               Gw_id = CommFunc.ConvertDBNullToInt32(s1["Gw_id"]),
                               Esp_id = CommFunc.ConvertDBNullToInt32(s1["Esp_id"]),
                               Meter_id = 0,
                               Module_id = 0,
                               EmName = CommFunc.ConvertDBNullToString(s1["EspName"]),
                               EmAddr = CommFunc.ConvertDBNullToString(s1["EspAddr"]),
                               TransferType = CommFunc.ConvertDBNullToInt32(s1["TransferType"]),
                               ComPort = CommFunc.ConvertDBNullToString(s1["ComPort"]),
                               ComPortNum = CommFunc.ConvertDBNullToString(s1["ComPort"]).Replace("COM", ""),
                               Baud = CommFunc.ConvertDBNullToInt32(s1["Baud"]),
                               DataBit = CommFunc.ConvertDBNullToInt32(s1["DataBit"]),
                               StopBit = CommFunc.ConvertDBNullToInt32(s1["StopBit"]),
                               Parity = CommFunc.ConvertDBNullToInt32(s1["Parity"]),
                               EspIp = CommFunc.ConvertDBNullToString(s1["EspIp"]),
                               EspPort = CommFunc.ConvertDBNullToInt32(s1["EspPort"]),
                               Timeout = CommFunc.ConvertDBNullToInt32(s1["Timeout"]),
                               EmType = CommFunc.ConvertDBNullToString(s1["EspType"]),
                               Inst_loc = CommFunc.ConvertDBNullToString(s1["Inst_loc"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                               Multiply = 0,
                               EnergyItemCode = "",
                               Co_id = 0,
                               CoName = "",
                               IsAlarm = 0,
                               MinPay = 0,
                               Price = 0,
                               Rate_id = 0,
                           };
                return res1.ToList();
            }
            else if (Module_id != 0)
            {
                DataTable dtSource = bll.GetYdModuleOnV1_gateway_esp_module(Module_id);
                total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = (CommFunc.ConvertDBNullToInt32(s1["Esp_id"]) * 10).ToString() + CommFunc.ConvertDBNullToString(s1["Module_id"]).PadLeft(5, '0'),
                               _parentId = CommFunc.ConvertDBNullToInt32(((CommFunc.ConvertDBNullToInt32(s1["Esp_id"]) * 10).ToString() + "0".PadLeft(5, '0'))),
                               Gw_id = CommFunc.ConvertDBNullToInt32(s1["Gw_id"]),
                               Esp_id = CommFunc.ConvertDBNullToInt32(s1["Esp_id"]),
                               Meter_id = CommFunc.ConvertDBNullToInt32(s1["Meter_id"]),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               EmName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               EmAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               TransferType = "",
                               EspIp = "",
                               EspPort = 0,
                               Timeout = CommFunc.ConvertDBNullToInt32(s1["Timeout"]),
                               Mm_id = CommFunc.ConvertDBNullToInt32(s1["Mm_id"]),
                               EmType = CommFunc.ConvertDBNullToString(s1["ModuleType"]),
                               Inst_loc = CommFunc.ConvertDBNullToString(s1["Inst_loc"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                               Multiply = CommFunc.ConvertDBNullToDecimal(s1["Multiply"]),
                               EnergyItemCode = CommFunc.ConvertDBNullToString(s1["EnergyItemCode"]),
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CoName = CommFunc.ConvertDBNullToInt32(s1["CoName"]),
                               IsAlarm = CommFunc.ConvertDBNullToInt32(s1["IsAlarm"]),
                               MinPay = CommFunc.ConvertDBNullToInt32(s1["MinPay"]),
                               Price = CommFunc.ConvertDBNullToDecimal(s1["Price"]),
                               Rate_id = CommFunc.ConvertDBNullToInt32(s1["Rate_id"]),
                           };
                return res1.ToList();
            }
            else
            {
                total = 0;
                return "";
            }
        }

        /// <summary>
        /// 获取费率列表
        /// </summary>
        /// <param name="Attrib">费率=0,物业收费标准=1</param>
        /// <returns></returns>
        public APIRst GetYdModuleRateList(int Attrib)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdModuleRateList(Attrib);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Rate_id"]),
                               text = CommFunc.ConvertDBNullToInt32(s1["Rate_id"]) == 0 ? "请选择" : CommFunc.ConvertDBNullToString(s1["Descr"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取费率列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取首页仪表数
        /// </summary>
        /// <returns></returns>
        public APIRst GetHomeModule()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetHomeModule();
                object obj = new { Total = dtSource.Rows.Count };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页仪表数错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取首页故障电表数
        /// </summary>
        /// <returns></returns>
        public APIRst GetHomeModuleError()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetHomeModuleError();
                object obj = new { Total = dtSource.Rows.Count };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页仪表数错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


    }
}