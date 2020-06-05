using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Syscont
{
    public partial class ExpYdModuleBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Exp.Syscont.ExpYdModuleDAL dal = null;
        public ExpYdModuleBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Exp.Syscont.ExpYdModuleDAL(_ledger, _uid);
        }


        /// <summary>
        /// 获取集中器数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdModuleOnV1_gateway(int Gw_id)
        {
            return dal.GetYdModuleOnV1_gateway(Gw_id);
        }

        /// <summary>
        /// 获取集中器详细信息
        /// </summary>
        /// <param name="Gw_id"></param>
        /// <returns></returns>
        public DataTable GetYdModuleOnDetail(int Gw_id, int Esp_id, int Meter_id, int Module_id)
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("Gw_id", typeof(System.Int32));
            dtRst.Columns.Add("Esp_id", typeof(System.Int32));
            dtRst.Columns.Add("Meter_id", typeof(System.Int32));
            dtRst.Columns.Add("Module_id", typeof(System.Int32));
            dtRst.Columns.Add("Id", typeof(System.Int32));
            dtRst.Columns.Add("Pid", typeof(System.Int32));
            dtRst.Columns.Add("EmName", typeof(System.String));
            dtRst.Columns.Add("EmAddr", typeof(System.String));
            dtRst.Columns.Add("EmNo", typeof(System.String));
            dtRst.Columns.Add("TransferType", typeof(System.Int32));
            dtRst.Columns.Add("EspIp", typeof(System.String));
            dtRst.Columns.Add("EspPort", typeof(System.Int32));
            dtRst.Columns.Add("Timeout", typeof(System.Int32));
            dtRst.Columns.Add("ComPort", typeof(System.String));
            dtRst.Columns.Add("Baud", typeof(System.Int32));
            dtRst.Columns.Add("DataBit", typeof(System.Int32));
            dtRst.Columns.Add("StopBit", typeof(System.Int32));
            dtRst.Columns.Add("Parity", typeof(System.Int32));
            dtRst.Columns.Add("Mm_id", typeof(System.Int32));
            dtRst.Columns.Add("EmType", typeof(System.String));
            dtRst.Columns.Add("Inst_loc", typeof(System.String));
            dtRst.Columns.Add("Disabled", typeof(System.Int32));
            dtRst.Columns.Add("Remark", typeof(System.String));
            dtRst.Columns.Add("Multiply", typeof(System.Decimal));
            dtRst.Columns.Add("EnergyItemCode", typeof(System.String));
            dtRst.Columns.Add("Co_id", typeof(System.Int32));
            dtRst.Columns.Add("CoName", typeof(System.String));
            dtRst.Columns.Add("IsAlarm", typeof(System.Int32));
            dtRst.Columns.Add("MinPay", typeof(System.Int32));
            dtRst.Columns.Add("Price", typeof(System.Decimal));
            dtRst.Columns.Add("Rate_id", typeof(System.Int32));
            DataTable dtSource = dal.GetYdModuleOnDetail(Gw_id, Esp_id, Meter_id, Module_id);

            if (Gw_id > 0)
            {
                #region 树形结构数据
                var res1 = from s1 in dtSource.AsEnumerable()
                           group s1 by new { Esp_id = CommFunc.ConvertDBNullToInt32(s1["Esp_id"]) } into g1
                           select new
                           {
                               Esp_id = g1.Key.Esp_id,
                               EspAddr = CommFunc.ConvertDBNullToString(g1.First()["EspAddr"]),
                               EspName = CommFunc.ConvertDBNullToString(g1.First()["EspName"]),
                               TransferType = CommFunc.ConvertDBNullToInt32(g1.First()["TransferType"]),
                               EspIp = CommFunc.ConvertDBNullToString(g1.First()["EspIp"]),
                               EspPort = CommFunc.ConvertDBNullToString(g1.First()["EspPort"]),
                               Timeout = CommFunc.ConvertDBNullToInt32(g1.First()["Timeout"]),
                               ComPort = CommFunc.ConvertDBNullToString(g1.First()["ComPort"]),
                               Baud = CommFunc.ConvertDBNullToInt32(g1.First()["Baud"]),
                               DataBit = CommFunc.ConvertDBNullToInt32(g1.First()["DataBit"]),
                               StopBit = CommFunc.ConvertDBNullToInt32(g1.First()["StopBit"]),
                               Parity = CommFunc.ConvertDBNullToInt32(g1.First()["Parity"]),
                               EspType = CommFunc.ConvertDBNullToString(g1.First()["EspType"]),
                               Inst_loc = CommFunc.ConvertDBNullToString(g1.First()["EspLoc"]),
                               Remark = CommFunc.ConvertDBNullToString(g1.First()["EspRmk"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(g1.First()["EspDisabled"]),
                           };
                int id = 0;
                foreach (var e1 in res1)
                {
                    #region 新增集中器
                    DataRow addEsp = dtRst.NewRow();
                    addEsp["Gw_id"] = Gw_id;
                    addEsp["Esp_id"] = e1.Esp_id;
                    addEsp["Meter_id"] = 0;
                    addEsp["Module_id"] = 0;
                    addEsp["Id"] = ++id;
                    addEsp["Pid"] = 0;
                    addEsp["EmName"] = e1.EspName;
                    addEsp["EmAddr"] = e1.EspAddr;
                    addEsp["TransferType"] = e1.TransferType;
                    addEsp["EspIp"] = e1.EspIp;
                    addEsp["EspPort"] = e1.EspPort;
                    addEsp["Timeout"] = e1.Timeout;
                    addEsp["ComPort"] = e1.ComPort;
                    addEsp["Baud"] = e1.Baud;
                    addEsp["DataBit"] = e1.DataBit;
                    addEsp["StopBit"] = e1.StopBit;
                    addEsp["Parity"] = e1.Parity;
                    addEsp["Mm_id"] = 0;
                    addEsp["EmType"] = e1.EspType;
                    addEsp["Inst_loc"] = e1.Inst_loc;
                    addEsp["Disabled"] = e1.Disabled;
                    addEsp["Remark"] = e1.Remark;
                    addEsp["Multiply"] = 0;
                    addEsp["EnergyItemCode"] = "";
                    addEsp["Co_id"] = 0;
                    addEsp["CoName"] = "";
                    addEsp["IsAlarm"] = 0;
                    addEsp["MinPay"] = 0;
                    addEsp["Price"] = 0;
                    addEsp["Rate_id"] = 0;
                    dtRst.Rows.Add(addEsp);
                    #endregion
                    var res2 = from s1 in dtSource.AsEnumerable()
                               where CommFunc.ConvertDBNullToInt32(s1["Esp_id"]) == e1.Esp_id && CommFunc.ConvertDBNullToInt32(s1["Meter_id"]) > 0
                               group s1 by new { Meter_id = CommFunc.ConvertDBNullToInt32(s1["Meter_id"]) } into g1
                               select new
                               {
                                   Meter_id = g1.Key.Meter_id,
                                   MeterAddr = CommFunc.ConvertDBNullToString(g1.First()["MeterAddr"]),
                                   MeterName = CommFunc.ConvertDBNullToString(g1.First()["MeterName"]),
                                   MeterNo = CommFunc.ConvertDBNullToString(g1.First()["MeterNo"]),
                                   Multiply = CommFunc.ConvertDBNullToDecimal(g1.First()["Multiply"]),
                                   Mm_id = CommFunc.ConvertDBNullToInt32(g1.First()["Mm_id"]),
                                   Inst_loc = CommFunc.ConvertDBNullToString(g1.First()["MeterLoc"]),
                                   Remark = CommFunc.ConvertDBNullToString(g1.First()["MeterRmk"]),
                                   Disabled = CommFunc.ConvertDBNullToString(g1.First()["MeterDisabled"]),
                                   ModuleType = CommFunc.ConvertDBNullToString(g1.First()["ModuleType"]),
                               };
                    foreach (var m2 in res2)
                    {
                        #region 设备
                        DataRow addMm = dtRst.NewRow();
                        addMm["Gw_id"] = Gw_id;
                        addMm["Esp_id"] = e1.Esp_id;
                        addMm["Meter_id"] = m2.Meter_id;
                        addMm["Module_id"] = 0;
                        addMm["Id"] = ++id;
                        addMm["Pid"] = addEsp["Id"];
                        addMm["EmName"] = m2.MeterName;
                        addMm["EmAddr"] = m2.MeterAddr;
                        addMm["EmNo"] = m2.MeterNo;
                        addMm["TransferType"] = e1.TransferType;
                        addMm["EspIp"] = e1.EspIp;
                        addMm["EspPort"] = e1.EspPort;
                        addMm["Timeout"] = e1.Timeout;
                        addMm["ComPort"] = e1.ComPort;
                        addMm["Baud"] = e1.Baud;
                        addMm["DataBit"] = e1.DataBit;
                        addMm["StopBit"] = e1.StopBit;
                        addMm["Parity"] = e1.Parity;
                        addMm["Mm_id"] = m2.Mm_id;
                        addMm["EmType"] = m2.ModuleType;
                        addMm["Inst_loc"] = m2.Inst_loc;
                        addMm["Disabled"] = m2.Disabled;
                        addMm["Remark"] = m2.Remark;
                        addMm["Multiply"] = m2.Multiply;
                        addMm["EnergyItemCode"] = "";
                        addMm["Co_id"] = 0;
                        addMm["CoName"] = "";
                        addMm["IsAlarm"] = 0;
                        addMm["MinPay"] = 0;
                        addMm["Price"] = 0;
                        addMm["Rate_id"] = 0;
                        dtRst.Rows.Add(addMm);
                        #endregion
                        foreach (DataRow dr in dtSource.Select("Esp_id=" + e1.Esp_id + " and Meter_id=" + m2.Meter_id + " and Module_id>0"))
                        {
                            #region 回路
                            DataRow addMd = dtRst.NewRow();
                            addMd["Gw_id"] = Gw_id;
                            addMd["Esp_id"] = e1.Esp_id;
                            addMd["Meter_id"] = m2.Meter_id;
                            addMd["Module_id"] = dr["Module_id"];
                            addMd["Id"] = ++id;
                            addMd["Pid"] = addMm["Id"];
                            addMd["EmName"] = dr["ModuleName"];
                            addMd["EmAddr"] = dr["ModuleAddr"];
                            addMd["TransferType"] = e1.TransferType;
                            addMd["EspIp"] = e1.EspIp;
                            addMd["EspPort"] = e1.EspPort;
                            addMd["Timeout"] = e1.Timeout;
                            addMd["ComPort"] = e1.ComPort;
                            addMd["Baud"] = e1.Baud;
                            addMd["DataBit"] = e1.DataBit;
                            addMd["StopBit"] = e1.StopBit;
                            addMd["Parity"] = e1.Parity;
                            addMd["Mm_id"] = m2.Mm_id;
                            addMd["EmType"] = m2.ModuleType;
                            addMd["Inst_loc"] = m2.Inst_loc;
                            addMd["Disabled"] = dr["ModuleDisabled"];
                            addMd["Remark"] = dr["ModuleRmk"];
                            addMd["Multiply"] = m2.Multiply;
                            addMd["EnergyItemCode"] = dr["EnergyItemCode"];
                            addMd["Co_id"] = dr["Co_id"];
                            addMd["CoName"] = dr["CoName"];
                            addMd["IsAlarm"] = dr["IsAlarm"];
                            addMd["MinPay"] = dr["MinPay"];
                            addMd["Price"] = dr["Price"];
                            addMd["Rate_id"] = dr["Rate_id"];
                            dtRst.Rows.Add(addMd);
                            #endregion
                        }

                    }

                }
                #endregion
            }
            else
            {
                foreach (DataRow dr in dtSource.Rows)
                {
                    #region 数据
                    DataRow addMd = dtRst.NewRow();
                    addMd["Gw_id"] = Gw_id;
                    addMd["Esp_id"] = dr["Esp_id"];
                    addMd["Meter_id"] = dr["Meter_id"];
                    addMd["Module_id"] = dr["Module_id"];
                    addMd["Id"] = 0;
                    addMd["Pid"] = 0;
                    addMd["TransferType"] = dr["TransferType"];
                    addMd["EspIp"] = dr["EspIp"];
                    addMd["EspPort"] = dr["EspPort"];
                    addMd["Timeout"] = dr["Timeout"];
                    addMd["ComPort"] = dr["ComPort"];
                    addMd["Baud"] = dr["Baud"];
                    addMd["DataBit"] = dr["DataBit"];
                    addMd["StopBit"] = dr["StopBit"];
                    addMd["Parity"] = dr["Parity"];
                    addMd["Mm_id"] = dr["Mm_id"];
                    addMd["Multiply"] = dr["Multiply"];
                    addMd["EnergyItemCode"] = dr["EnergyItemCode"];
                    addMd["Co_id"] = dr["Co_id"];
                    addMd["CoName"] = dr["CoName"];
                    addMd["IsAlarm"] = dr["IsAlarm"];
                    addMd["MinPay"] = dr["MinPay"];
                    addMd["Price"] = dr["Price"];
                    addMd["Rate_id"] = dr["Rate_id"];
                    if (Esp_id > 0)
                    {
                        addMd["EmName"] = dr["EspName"];
                        addMd["EmAddr"] = dr["EspAddr"];
                        addMd["EmType"] = dr["EspType"];
                        addMd["Inst_loc"] = dr["EspLoc"];
                        addMd["Disabled"] = dr["EspDisabled"];
                        addMd["Remark"] = dr["EspRmk"];
                    }
                    else if (Meter_id > 0)
                    {
                        addMd["EmName"] = dr["MeterName"];
                        addMd["EmAddr"] = dr["MeterAddr"];
                        addMd["EmNo"] = dr["MeterNo"];
                        addMd["EmType"] = dr["ModuleType"];
                        addMd["Inst_loc"] = dr["MeterLoc"];
                        addMd["Disabled"] = dr["MeterDisabled"];
                        addMd["Remark"] = dr["MeterRmk"];
                    }
                    else
                    {
                        addMd["EmName"] = dr["ModuleName"];
                        addMd["EmAddr"] = dr["ModuleAddr"];
                        addMd["EmType"] = dr["ModuleType"];
                        addMd["Inst_loc"] = dr["MeterLoc"];
                        addMd["Disabled"] = dr["ModuleDisabled"];
                        addMd["Remark"] = dr["ModuleRmk"];
                    }
                    dtRst.Rows.Add(addMd);
                    #endregion
                }
            }
            return dtRst;
        }


        /// <summary>
        /// 设置集中器信息
        /// </summary>
        /// <param name="Gw"></param>
        /// <returns></returns>
        public int YdModuleOnSaveGw(v1_gatewayVModel Gw)
        {
            return dal.YdModuleOnSaveGw(Gw);
        }

        /// <summary>
        /// 获取采集器数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdModuleOnV1_gateway_esp(int Esp_id)
        {
            return dal.GetYdModuleOnV1_gateway_esp(Esp_id);
        }

        /// <summary>
        /// 获取电能表数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdModuleOnV1_gateway_esp_module(int Module_id)
        {
            return dal.GetYdModuleOnV1_gateway_esp_module(Module_id);
        }

        /// <summary>
        /// 删除集中器
        /// </summary>
        /// <param name="Gw_id"></param>
        /// <returns></returns>
        public int YdModuleOnDelGw(int Gw_id)
        {
            return dal.YdModuleOnDelGw(Gw_id);
        }

        /// <summary>
        /// 设置采集器信息
        /// </summary>
        /// <param name="Gw"></param>
        /// <returns></returns>
        public int YdModuleOnSaveEsp(v1_gateway_espVModel Esp)
        {
            return dal.YdModuleOnSaveEsp(Esp);
        }

        /// <summary>
        /// 删除采集器
        /// </summary>
        /// <param name="Gw_id"></param>
        /// <returns></returns>
        public int YdModuleOnDelEsp(int Esp_id)
        {
            return dal.YdModuleOnDelEsp(Esp_id);
        }

        /// <summary>
        /// 获取类型类表
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdModuleOnModelList()
        {
            return dal.GetYdModuleOnModelList();
        }

        /// <summary>
        /// 保存设备数据
        /// </summary>
        /// <param name="md">对象：Meter_id,Esp_id,MeterName=设备名称,MeterAddr=设备地址,Mm_id=设备类型,Inst_loc=安装位置,Multiply=设备倍率,Remark=备注,Disabled=是否弃用</param>
        /// <returns></returns>
        public int YdModuleOnSaveMm(v1_gateway_esp_meterVModel Md)
        {
            return dal.YdModuleOnSaveMm(Md);
        }

        /// <summary>
        /// 删除设备数据
        /// </summary>
        /// <param name="Meter_id"></param>
        /// <returns></returns>
        public int YdModuleOnDelMm(int Meter_id)
        {
            return dal.YdModuleOnDelMm(Meter_id);
        }

        /// <summary>
        /// 设置回路信息(电能表信息)
        /// </summary>
        /// <param name="Gw"></param>
        /// <returns></returns>
        public int YdModuleOnSaveMd(v1_gateway_esp_moduleVModel Md)
        {
            return dal.YdModuleOnSaveMd(Md);
        }

        /// <summary>
        /// 删除回路信息(电能表信息)
        /// </summary>
        /// <param name="Module_id"></param>
        /// <returns></returns>
        public int YdModuleOnDelMd(int Module_id)
        {
            return dal.YdModuleOnDelMd(Module_id);
        }

        /// <summary>
        /// 获取映射信息
        /// </summary>
        /// <param name="Module_id"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        public DataTable GetYdModuleOnMapList(int Module_id, int Action)
        {
            return dal.GetYdModuleOnMapList(Module_id, Action);
        }

        /// <summary>
        /// 设置映射关系
        /// </summary>
        /// <param name="Module_id"></param>
        /// <param name="Fun_id"></param>
        /// <param name="TagName"></param>
        /// <returns></returns>
        public int YdModuleOnMapInSave(int Module_id, DataRow dr)
        {
            return dal.YdModuleOnMapInSave(Module_id, dr);
        }

        /// <summary>
        /// 获取费率列表
        /// </summary>
        /// <param name="Attrib">费率=0,物业收费标准=1</param>
        /// <returns></returns>
        public DataTable GetYdModuleRateList(int attrib)
        {
            return dal.GetYdModuleRateList(attrib);
        }


        #region 验证重复

        /// <summary>
        /// 判断采集器名称是否存在
        /// </summary>
        /// <param name="Gw_id"></param>
        /// <param name="Esp_id"></param>
        /// <param name="EspName"></param>
        /// <param name="contains">是否包括自己</param>
        /// <returns></returns>
        public int IsExistYdEspName(int Gw_id, int Esp_id, string EspName, bool contains)
        {
            return dal.IsExistYdEspName(Gw_id, Esp_id, EspName, contains);
        }

        /// <summary>
        /// 判断集中器名称是否存在
        /// </summary>
        /// <param name="GwName"></param>
        /// <returns></returns>
        public int IsExistYdGwName(string GwName)
        {
            return dal.IsExistYdGwName(GwName);
        }
        /// <summary>
        /// 判断集中器地址是否存在
        /// </summary>
        /// <param name="GwName"></param>
        /// <returns></returns>
        public int IsExistYdGwAddr(string GwAddr)
        {
            return dal.IsExistYdGwAddr(GwAddr);
        }

        #endregion

        /// <summary>
        /// 获取首页仪表数
        /// </summary>
        /// <returns></returns>
        public DataTable GetHomeModule()
        {
            return dal.GetHomeModule();
        }

        /// <summary>
        /// 获取首页故障电表数
        /// </summary>
        /// <returns></returns>
        public DataTable GetHomeModuleError()
        {
            return dal.GetHomeModuleError();
        }

    }
}
