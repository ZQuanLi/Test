using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Exp.Syscont
{
    public partial class ExpYdModuleDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public ExpYdModuleDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取集中器数据
        /// </summary>
        /// <param name="Gw_id"></param>
        /// <returns></returns>
        public DataTable GetYdModuleOnV1_gateway(int Gw_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Gw_id,a.GwName,a.GwAddr,a.GwIp,a.GwPort,a.Timeout,a.GwType,a.Inst_loc,a.Remark,a.Disabled");
            strSql.Append(" from v1_gateway as a");
            strSql.Append(" where a.Ledger=@Ledger");
            if (Gw_id != 0)
                strSql.Append(" and a.Gw_id=@Gw_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Gw_id = Gw_id });
        }

        /// <summary>
        /// 获取采集器数据
        /// </summary>
        /// <param name="Esp_id"></param>
        /// <returns></returns>
        public DataTable GetYdModuleOnV1_gateway_esp(int Esp_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select b.Esp_id,b.Gw_id,b.EspName,b.EspAddr,b.TransferType,b.EspIp,b.EspPort,b.ComPort,b.Baud,b.DataBit,b.StopBit,b.Parity,b.Timeout,b.EspType,b.Inst_loc,b.Remark,b.Disabled");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" where a.Ledger=@Ledger");
            if (Esp_id != 0)
                strSql.Append(" and b.Esp_id=@Esp_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Esp_id = Esp_id });
        }

        /// <summary>
        /// 获取电能表数据
        /// </summary>
        /// <param name="Module_id"></param>
        /// <returns></returns>
        public DataTable GetYdModuleOnV1_gateway_esp_module(int Module_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select d.Module_id,b.Esp_id,a.Gw_id,d.ModuleName,d.ModuleAddr,c.Inst_loc,d.Disabled,d.Remark,c.Multiply,c.EnergyItemCode,c.Mm_id,c.Co_id,");
            strSql.Append("v1.CoName,b.Timeout,vm.ModuleType,e.IsAlarm,e.MinPay,e.Price,e.Rate_id");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=B.Gw_id");
            strSql.Append(" inner join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
            strSql.Append(" inner join v1_gateway_esp_module as d on c.Ledger=d.Ledger and c.Meter_id=d.Meter_id");
            strSql.Append(" left join v1_gateway_esp_module_info as e on d.Ledger=e.Ledger and d.Module_id=e.Module_id");
            strSql.Append(" Left join v0_module as vm on c.Ledger=vm.Ledger and c.Mm_id=vm.Mm_id");
            strSql.Append(" left join v1_cust as v1 on c.Ledger=v1.Ledger and c.Co_id=v1.Co_id");
            strSql.Append(" where a.Ledger=@Ledger");
            if (Module_id == 0)
                strSql.Append(" and a.Disabled=0 and b.Disabled=0 and c.Disabled=0");
            else
                strSql.Append(" and d.Module_id=@Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id });
        }


        /// <summary>
        /// 获取采集列表数据
        /// </summary>
        /// <param name="Gw_id"></param>
        /// <returns></returns>
        public DataTable GetYdModuleOnDetail(int Gw_id, int Esp_id, int Meter_id, int Module_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select b.Esp_id,b.EspName,b.EspAddr,b.TransferType,b.EspIp,b.EspPort,b.Timeout,b.ComPort,b.Baud,b.DataBit,b.StopBit,b.Parity,b.EspType,b.Inst_loc as EspLoc,b.Remark as EspRmk,b.Disabled as EspDisabled,");
            strSql.Append("c.Meter_id,c.MeterName,c.MeterAddr,c.MeterNo,c.Multiply,c.Mm_id,c.Inst_loc as MeterLoc,c.Remark as MeterRmk,c.Disabled as MeterDisabled,vm.ModuleType,");
            strSql.Append("d.Module_id,d.ModuleName,d.ModuleAddr,d.EnergyItemCode,d.Co_id,d.ModuleNo,d.Disabled as ModuleDisabled,d.Remark as ModuleRmk,v1.CoName,");
            strSql.Append("e.MinPay,e.Price,e.IsAlarm,e.Rate_id");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            if (Gw_id > 0)
            {
                strSql.Append(" left join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
                strSql.Append(" left join v1_gateway_esp_module as d on c.Ledger=d.Ledger and c.Meter_id=d.Meter_id");
                strSql.Append(" left join v1_gateway_esp_module_info as e on d.Ledger=e.Ledger and d.Module_id=e.Module_id");
                strSql.Append(" left join v0_module as vm on c.Ledger=vm.Ledger and c.Mm_id=vm.Mm_id");
                strSql.Append(" left join v1_cust as v1 on c.Ledger=v1.Ledger and c.Co_id=v1.Co_id");
                strSql.Append(" where a.Ledger=@Ledger and a.Gw_id=@Gw_id");
            }
            else if (Esp_id > 0)
            {
                strSql.Append(" left join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id and c.Meter_id=-999");
                strSql.Append(" left join v1_gateway_esp_module as d on c.Ledger=d.Ledger and c.Meter_id=d.Meter_id");
                strSql.Append(" left join v1_gateway_esp_module_info as e on d.Ledger=e.Ledger and d.Module_id=e.Module_id");
                strSql.Append(" left join v0_module as vm on c.Ledger=vm.Ledger and c.Mm_id=vm.Mm_id");
                strSql.Append(" left join v1_cust as v1 on c.Ledger=v1.Ledger and c.Co_id=v1.Co_id");
                strSql.Append(" where a.Ledger=@Ledger and b.Esp_id=@Esp_id");
            }
            else if (Meter_id > 0)
            {
                strSql.Append(" left join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
                strSql.Append(" left join v1_gateway_esp_module as d on c.Ledger=d.Ledger and c.Meter_id=d.Meter_id and d.Module_id=-999");
                strSql.Append(" left join v1_gateway_esp_module_info as e on d.Ledger=e.Ledger and d.Module_id=e.Module_id");
                strSql.Append(" left join v0_module as vm on c.Ledger=vm.Ledger and c.Mm_id=vm.Mm_id");
                strSql.Append(" left join v1_cust as v1 on c.Ledger=v1.Ledger and c.Co_id=v1.Co_id");
                strSql.Append(" where a.Ledger=@Ledger and c.Meter_id=@Meter_id");
            }
            else
            {
                strSql.Append(" left join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
                strSql.Append(" left join v1_gateway_esp_module as d on c.Ledger=d.Ledger and c.Meter_id=d.Meter_id");
                strSql.Append(" left join v1_gateway_esp_module_info as e on d.Ledger=e.Ledger and d.Module_id=e.Module_id");
                strSql.Append(" left join v0_module as vm on c.Ledger=vm.Ledger and c.Mm_id=vm.Mm_id");
                strSql.Append(" left join v1_cust as v1 on c.Ledger=v1.Ledger and c.Co_id=v1.Co_id");
                strSql.Append(" where a.Ledger=@Ledger and d.Module_id=@Module_id");
            }
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Gw_id = Gw_id, Esp_id = Esp_id, Meter_id = Meter_id, Module_id = Module_id });
        }


        /// <summary>
        /// 保存集中器信息
        /// </summary>
        /// <param name="Gw"></param>
        /// <returns></returns>
        public int YdModuleOnSaveGw(v1_gatewayVModel Gw)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = new DataTable();
            if (Gw.Gw_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Gw_id) as cnt from v1_gateway where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                Gw.Gw_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1 : 1;
                //Gw.Gw_id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1;
                //Gw.Gw_id = CommFunc.ConvertDBNullToInt32(SQLUtilities.ExecuteScalar(strSql.ToString(), params01)) + 1;
                //params01[1].Value = Gw.Gw_id;
                strSql.Clear();
                strSql.Append("insert into v1_gateway(");
                strSql.Append("Ledger,Gw_id,GwName,GwAddr,GwIp,GwPort,Timeout,GwType,Inst_loc,Remark,Disabled,Create_by,Create_dt,Update_by,Update_dt)");
                strSql.Append(" values (");
                strSql.Append("@Ledger,@Gw_id,@GwName,@GwAddr,@GwIp,@GwPort,@Timeout,@GwType,@Inst_loc,@Remark,@Disabled,@Uid,now(),@Uid,now())");
                if (SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Gw_id = Gw.Gw_id, GwName = Gw.GwName, GwAddr = Gw.GwAddr, GwIp = Gw.GwIp, GwPort = Gw.GwPort, Timeout = Gw.Timeout, GwType = Gw.GwType, Inst_loc = Gw.Inst_loc, Remark = Gw.Remark, Disabled = Gw.Disabled, Uid = this.SysUid }) > 0)
                {
                    return Gw.Gw_id;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                strSql.Clear();
                strSql.Append("update v1_gateway set ");
                strSql.Append("GwName=@GwName,");
                strSql.Append("GwAddr=@GwAddr,");
                strSql.Append("GwIp=@GwIp,");
                strSql.Append("GwPort=@GwPort,");
                strSql.Append("Timeout=@Timeout,");
                strSql.Append("GwType=@GwType,");
                strSql.Append("Inst_loc=@Inst_loc,");
                strSql.Append("Remark=@Remark,");
                strSql.Append("Disabled=@Disabled,");
                strSql.Append("Update_by=@Uid,");
                strSql.Append("Update_dt=now()");
                strSql.Append(" where Ledger=@Ledger and Gw_id=@Gw_id ");
                if (SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Gw_id = Gw.Gw_id, GwName = Gw.GwName, GwAddr = Gw.GwAddr, GwIp = Gw.GwIp, GwPort = Gw.GwPort, Timeout = Gw.Timeout, GwType = Gw.GwType, Inst_loc = Gw.Inst_loc, Remark = Gw.Remark, Disabled = Gw.Disabled, Uid = this.SysUid }) > 0)
                {
                    return Gw.Gw_id;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 删除集中器
        /// </summary>
        /// <param name="Gw_id"></param>
        /// <returns></returns>
        public int YdModuleOnDelGw(int Gw_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("delete e,d,c,b,a");
            strSql.Append(" from v1_gateway as a left join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" left join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
            strSql.Append(" left join v1_gateway_esp_module as d on c.Ledger=d.Ledger and c.Meter_id=d.Meter_id");
            strSql.Append(" left join v1_gateway_esp_module_info as e on d.Ledger=e.Ledger and d.Module_id=e.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Gw_id=@Gw_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Gw_id = Gw_id });
        }

        /// <summary>
        /// 设置采集器信息
        /// </summary>
        /// <param name="Esp"></param>
        /// <returns></returns>
        public int YdModuleOnSaveEsp(v1_gateway_espVModel Esp)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = new DataTable();
            if (Esp.Esp_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Esp_id) as cnt from v1_gateway_esp where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                Esp.Esp_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1 : 1;
                //Esp.Esp_id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1;
                //Gw.Gw_id = CommFunc.ConvertDBNullToInt32(SQLUtilities.ExecuteScalar(strSql.ToString(), params01)) + 1;
                //params01[1].Value = Gw.Gw_id;
                strSql.Clear();
                strSql.Append("insert into v1_gateway_esp(");
                strSql.Append("Ledger,Esp_id,Gw_id,EspName,EspAddr,TransferType,EspIp,EspPort,Timeout,EspType,ComPort,Baud,DataBit,StopBit,Parity,Inst_loc,Remark,Create_by,Create_dt,Update_by,Update_dt)");
                strSql.Append(" values (");
                strSql.Append("@Ledger,@Esp_id,@Gw_id,@EspName,@EspAddr,@TransferType,@EspIp,@EspPort,@Timeout,@EspType,@ComPort,@Baud,@DataBit,@StopBit,@Parity,@Inst_loc,@Remark,@Uid,now(),@Uid,now())");
                if (SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Esp_id = Esp.Esp_id, Gw_id = Esp.Gw_id, EspName = Esp.EspName, EspAddr = Esp.EspAddr, TransferType = Esp.TransferType, EspIp = Esp.EspIp, EspPort = Esp.EspPort, Timeout = Esp.Timeout, EspType = Esp.EspType, ComPort = Esp.ComPort, Uid = this.SysUid, Baud = Esp.Baud, DataBit = Esp.DataBit, StopBit = Esp.StopBit, Parity = Esp.Parity, Inst_loc = Esp.Inst_loc, Remark = Esp.Remark }) > 0)
                {
                    return Esp.Esp_id;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                strSql.Clear();
                strSql.Append("update v1_gateway_esp set ");
                strSql.Append("EspName=@EspName,");
                strSql.Append("EspAddr=@EspAddr,");
                strSql.Append("TransferType=@TransferType,");
                strSql.Append("EspIp=@EspIp,");
                strSql.Append("EspPort=@EspPort,");
                strSql.Append("Timeout=@Timeout,");
                strSql.Append("EspType=@EspType,");
                strSql.Append("ComPort=@ComPort,");
                strSql.Append("Baud=@Baud,");
                strSql.Append("DataBit=@DataBit,");
                strSql.Append("StopBit=@StopBit,");
                strSql.Append("Parity=@Parity,");
                strSql.Append("Inst_loc=@Inst_loc,");
                strSql.Append("Remark=@Remark,");
                strSql.Append("Disabled=@Disabled,");
                strSql.Append("Update_by=@Uid,");
                strSql.Append("Update_dt=now()");
                strSql.Append(" where Ledger=@Ledger and Esp_id=@Esp_id ");
                if (SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Esp_id = Esp.Esp_id, EspName = Esp.EspName, EspAddr = Esp.EspAddr, TransferType = Esp.TransferType, EspIp = Esp.EspIp, EspPort = Esp.EspPort, Timeout = Esp.Timeout, EspType = Esp.EspType, ComPort = Esp.ComPort, Uid = this.SysUid, Baud = Esp.Baud, DataBit = Esp.DataBit, StopBit = Esp.StopBit, Parity = Esp.Parity, Inst_loc = Esp.Inst_loc, Remark = Esp.Remark, Disabled = string.IsNullOrEmpty(Esp.Disabled.ToString()) ? 0 : Esp.Disabled }) > 0)
                {
                    return Esp.Esp_id;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 删除采集器
        /// </summary>
        /// <param name="Gw_id"></param>
        /// <returns></returns>
        public int YdModuleOnDelEsp(int Esp_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("delete e,d,c,b");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" left join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
            strSql.Append(" left join v1_gateway_esp_module as d on c.Ledger=d.Ledger and c.Meter_id=d.Meter_id");
            strSql.Append(" left join v1_gateway_esp_module_info as e on d.Ledger=e.Ledger and d.Module_id=e.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and b.Esp_id=@Esp_id;");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Esp_id = Esp_id });
        }

        /// <summary>
        /// 获取类型类表
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdModuleOnModelList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select Mm_id,ModuleName,ModuleType,Disabled from v0_module where Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 保存设备数据
        /// </summary>
        /// <param name="md">对象：Meter_id,Esp_id,MeterName=设备名称,MeterAddr=设备地址,Mm_id=设备类型,Inst_loc=安装位置,Multiply=设备倍率,Remark=备注,Disabled=是否弃用</param>
        /// <returns></returns>
        public int YdModuleOnSaveMm(v1_gateway_esp_meterVModel Md)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = new DataTable();

            strSql.Append("select count(*) from v1_gateway_esp_meter where Ledger=@Ledger and Meter_id!=@Meter_id and Esp_id=@Esp_id and MeterAddr=@MeterAddr");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = Md.Meter_id, Esp_id = Md.Esp_id, MeterAddr = Md.MeterAddr });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["count(*)"]) > 0)
            {
                throw new Exception("同一采集器下设备地址重复");
            }

            obj = null;
            strSql.Clear();
            strSql.Append("select count(*) from v1_gateway_esp_meter where Ledger=@Ledger and Meter_id!=@Meter_id and Esp_id=@Esp_id and MeterName=@MeterName");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = Md.Meter_id, Esp_id = Md.Esp_id, MeterName = Md.MeterName });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["count(*)"]) > 0)
            {
                throw new Exception("同一采集器下设备名称重复");
            }

            obj = null;
            strSql.Clear();
            strSql.Append("select a.Mm_id as mm_id from v1_gateway_esp_meter as a where a.Ledger=@Ledger and a.Meter_id=@Meter_id ");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = Md.Meter_id });
            int mm_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["mm_id"]) : 0;
            if (mm_id != Md.Mm_id)
            {//删除关系
                strSql.Clear();
                strSql.Append("delete c");
                strSql.Append(" from v1_gateway_esp_meter as a inner join v1_gateway_esp_module as b on a.Ledger=b.Ledger and a.Meter_id=b.Meter_id");
                strSql.Append(" inner join v1_map as c on b.Ledger=c.Ledger and b.Module_id=c.Module_id");
                strSql.Append(" where a.Ledger=@Ledger and a.Meter_id=@Meter_id");
                SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = Md.Meter_id });
            }
            if (Md.Meter_id == 0)
            {
                obj = null;
                strSql.Clear();
                strSql.Append("select max(Meter_id) as cnt from v1_gateway_esp_meter where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                Md.Meter_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1 : 1;
                //Md.Meter_id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1;
            }
            strSql.Clear();
            strSql.Append("insert into v1_gateway_esp_meter(");
            strSql.Append("Ledger,Meter_id,Esp_id,MeterName,MeterAddr,MeterNo,Inst_loc,Disabled,Remark,Multiply,EnergyItemCode,Mm_id,Co_id,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Meter_id,@Esp_id,@MeterName,@MeterAddr,@MeterNo,@Inst_loc,@Disabled,@Remark,@Multiply,'',@Mm_id,0,@Uid,now(),@Uid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE MeterName=@MeterName,MeterAddr=@MeterAddr,MeterNo=@MeterNo,Inst_loc=@Inst_loc,Disabled=@Disabled,");
            strSql.Append("Remark=@Remark,Multiply=@Multiply,Mm_id=@Mm_id,Update_by=@Uid,Update_dt=now();");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = Md.Meter_id, Esp_id = Md.Esp_id, MeterName = Md.MeterName, MeterAddr = Md.MeterAddr, MeterNo = Md.MeterNo, Inst_loc = Md.Inst_loc, Disabled = Md.Disabled, Remark = Md.Remark, Multiply = Md.Multiply, Mm_id = Md.Mm_id, Uid = this.SysUid });
        }

        /// <summary>
        /// 删除设备数据
        /// </summary>
        /// <param name="Meter_id"></param>
        /// <returns></returns>
        public int YdModuleOnDelMm(int Meter_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("delete mp,e,d,c");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" inner join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
            strSql.Append(" left join v1_gateway_esp_module as d on c.Ledger=d.Ledger and c.Meter_id=d.Meter_id");
            strSql.Append(" left join v1_gateway_esp_module_info as e on d.Ledger=e.Ledger and d.Module_id=e.Module_id");
            strSql.Append(" left join v1_map as mp on d.Ledger=mp.Ledger and d.Module_id=mp.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and c.Meter_id=@Meter_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = Meter_id });
        }

        /// <summary>
        /// 设置回路信息(电能表信息)
        /// </summary>
        /// <param name="Md"></param>
        /// <returns></returns>
        public int YdModuleOnSaveMd(v1_gateway_esp_moduleVModel Md)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = new DataTable();
            int Esp_id = 0;
            strSql.Append("select Esp_id from v1_gateway_esp_meter where Ledger=@Ledger and Meter_id=@Meter_id");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = Md.Meter_id });
            Esp_id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Esp_id"]);

            obj = null;
            strSql.Clear();
            strSql.Append("select count(*) from v1_gateway_esp_meter as a inner join v1_gateway_esp_module as b on a.Ledger=b.Ledger and a.Meter_id=b.Meter_id");
            strSql.Append(" where a.Ledger=@Ledger and b.Module_id!=@Module_id and a.Esp_id=@Esp_id and b.ModuleAddr=@ModuleAddr");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Md.Module_id, Esp_id = Esp_id, ModuleAddr = Md.ModuleAddr });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["count(*)"]) > 0)
            {
                throw new Exception("同一采集器下回路地址重复");
            }

            obj = null;
            strSql.Clear();
            strSql.Append("select count(*) from v1_gateway_esp_meter as a inner join v1_gateway_esp_module as b on a.Ledger=b.Ledger and a.Meter_id=b.Meter_id");
            strSql.Append(" where a.Ledger=@Ledger and b.Module_id!=@Module_id and a.Esp_id=@Esp_id and b.ModuleName=@ModuleName");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Md.Module_id, Esp_id = Esp_id, ModuleName = Md.ModuleName });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["count(*)"]) > 0)
            {
                throw new Exception("同一采集器下回路名称重复");
            }
            if (Md.Module_id == 0)
            {
                obj = null;
                strSql.Clear();
                strSql.Append("select max(Module_id) as cnt from v1_gateway_esp_module where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                Md.Module_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1 : 1;
                //Md.Module_id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1;
            }

            strSql.Clear();
            strSql.Append("insert into v1_gateway_esp_module(");
            strSql.Append("Ledger,Module_id,Meter_id,ModuleName,ModuleAddr,Disabled,Remark,EnergyItemCode,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Module_id,@Meter_id,@ModuleName,@ModuleAddr,@Disabled,@Remark,@EnergyItemCode,@Uid,now(),@Uid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE ModuleName=@ModuleName,ModuleAddr=@ModuleAddr,Disabled=@Disabled,");
            strSql.Append("Remark=@Remark,Update_by=@Uid,Update_dt=now();");
            //
            strSql.Append("insert into v1_gateway_esp_module_info(");
            strSql.Append("Ledger,Module_id,IsAlarm,MinPay,Price,Rate_id,Update_by,Update_dt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Module_id,@IsAlarm,@MinPay,@Price,@Rate_id,@Uid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE IsAlarm=IsAlarm;");//MinPay=@MinPay,Price=@Price,Rate_id=@Rate_id,
            int cc = 0;
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Md.Module_id, Meter_id = Md.Meter_id, ModuleName = Md.ModuleName, ModuleAddr = Md.ModuleAddr, Disabled = Md.Disabled, Remark = Md.Remark, EnergyItemCode = Md.EnergyItemCode, Uid = this.SysUid, IsAlarm = Md.IsAlarm, MinPay = Md.MinPay, Price = Md.Price, Rate_id = Md.Rate_id });
        }

        /// <summary>
        /// 删除回路信息(电能表信息)
        /// </summary>
        /// <param name="Module_id"></param>
        /// <returns></returns>
        public int YdModuleOnDelMd(int Module_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("delete mp,e,d");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" inner join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
            strSql.Append(" inner join v1_gateway_esp_module as d on c.Ledger=d.Ledger and c.Meter_id=d.Meter_id");
            strSql.Append(" left join v1_gateway_esp_module_info as e on d.Ledger=e.Ledger and d.Module_id=e.Module_id");
            strSql.Append(" left join v1_map as mp on d.Ledger=mp.Ledger and d.Module_id=mp.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and d.Module_id=@Module_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id });
        }

        /// <summary>
        /// 获取映射信息
        /// </summary>
        /// <param name="Module_id"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        public DataTable GetYdModuleOnMapList(int Module_id, int Action)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select b.Fun_id,b.FunName,b.FunType,c.TagName,case when c.DataValue is null then b.DataValue else c.DataValue end as DataValue,b.Action,c.Status,c.Disabled");
            strSql.Append(" from vp_mdinfo as a inner join v0_fun as b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id");
            strSql.Append(" left join v1_map as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id and b.Fun_id=c.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and ifnull(b.Disabled,0)=0");
            if (Action == 0)
                strSql.Append(" and (b.Action=0 or b.Action=2)");
            else
                strSql.Append(" and (b.Action=@Action)");
            strSql.Append(" ORDER BY b.OrdNo,b.Mm_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id, Action = Action });
        }

        /// <summary>
        /// 设置映射关系
        /// </summary>
        /// <param name="Module_id"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public int YdModuleOnMapInSave(int Module_id, DataRow dr)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = new DataTable();

            int action = CommFunc.ConvertDBNullToInt32(dr["Action"]);


            if (action == 0 && string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(dr["TagName"])) && CommFunc.ConvertDBNullToInt32(dr["Disabled"]) == 0)
            {
                strSql.Clear();
                strSql.Append("delete from v1_map where Ledger=@Ledger and Module_id=@Module_id and Fun_id=@Fun_id");
                return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id, Fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]) });
            }
            else
            {
                strSql.Clear();
                strSql.Append("select count(*) as cnt from v1_map where Ledger=@Ledger and Module_id=@Module_id and Fun_id=@Fun_id");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id, Fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]) });
                int cnt = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) : 0;
                if (cnt == 0)
                {
                    bool isRun = true;
                    if (action == 1 && string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(dr["DataValue"])) && CommFunc.ConvertDBNullToInt32(dr["Disabled"]) == 0)
                        isRun = false;
                    if (isRun == true)
                    {
                        strSql.Clear();
                        strSql.Append("insert into v1_map(Ledger,Module_id,Fun_id,TagName,DataValue,Status,Rmk,Disabled,Update_by,Update_dt)");
                        strSql.Append("values(@Ledger,@Module_id,@Fun_id,@TagName,@DataValue,0,'',@Disabled,@Uid,now())");
                        return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id, Fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]), TagName = CommFunc.ConvertDBNullToString(dr["TagName"]), DataValue = CommFunc.ConvertDBNullToString(dr["DataValue"]), Disabled = CommFunc.ConvertDBNullToInt32(dr["Disabled"]), Uid = this.SysUid });
                    }
                }
                else
                {
                    strSql.Clear();
                    strSql.Append("update v1_map set TagName=@TagName,DataValue=@DataValue,Status=0,Disabled=@Disabled,Update_by=@Uid,Update_dt=now()");
                    strSql.Append(" where Ledger=@Ledger and Module_id=@Module_id and Fun_id=@Fun_id");
                    if (action == 1)
                        strSql.Append(" and (ifnull(Status,0)=0 or (ifnull(Status,0)=1 and DataValue!=@DataValue))");
                    return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id, Fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]), TagName = CommFunc.ConvertDBNullToString(dr["TagName"]), DataValue = CommFunc.ConvertDBNullToString(dr["DataValue"]), Disabled = CommFunc.ConvertDBNullToInt32(dr["Disabled"]), Uid = this.SysUid });
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取费率列表
        /// </summary>
        /// <param name="Attrib">费率=0,物业收费标准=1</param>
        /// <returns></returns>
        public DataTable GetYdModuleRateList(int Attrib)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select Rate_id,Descr from v1_rate where Ledger=@Ledger and Attrib=@Attrib");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = Attrib });
        }




        #region 验证重复

        /// <summary>
        ///判断集中器名称是否存在
        /// </summary>
        /// <param name="Gw_id"></param>
        /// <param name="Esp_id"></param>
        /// <param name="EspName"></param>
        /// <param name="contains"></param>
        /// <returns>存在，返回存在ID，否则返回0</returns>
        public int IsExistYdEspName(int Gw_id, int Esp_id, string EspName, bool contains)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select Esp_id from v1_gateway_esp where Ledger=@Ledger and EspName=@EspName and Gw_id=@Gw_id  ");
            if (!contains) strSql.Append(" and Esp_id!=@Esp_id ");
            DataTable obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Gw_id = Gw_id, Esp_id = Esp_id, EspName = EspName });
            int Id = 0;
            if (obj.Rows.Count > 0)
                Id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Esp_id"]);
            return Id;
        }

        /// <summary>
        ///判断集中器名称是否存在
        /// </summary>
        /// <param name="GwName"></param>
        /// <returns>存在，返回存在ID，否则返回0</returns>
        public int IsExistYdGwName(string GwName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select Gw_id from v1_gateway where Ledger=@Ledger and GwName=@GwName ");
            DataTable obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, GwName = GwName });
            int Id = 0;
            if (obj.Rows.Count > 0)
                Id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Gw_id"]);
            return Id;
        }

        /// <summary>
        ///判断集中器地址是否存在
        /// </summary>
        /// <param name="GwAddr"></param>
        /// <returns>存在，返回存在ID，否则返回0</returns>
        public int IsExistYdGwAddr(string GwAddr)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select Gw_id from v1_gateway where Ledger=@Ledger and GwAddr=@GwAddr ");
            DataTable obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, GwAddr = GwAddr });
            int Id = 0;
            if (obj.Rows.Count > 0)
                Id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Gw_id"]);
            return Id;
        }

        #endregion

        /// <summary>
        /// 获取首页仪表数
        /// </summary>
        /// <returns></returns>
        public DataTable GetHomeModule()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select * from v1_gateway_esp_meter where Ledger=@Ledger ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 获取首页故障电表数
        /// </summary>
        /// <returns></returns>
        public DataTable GetHomeModuleError()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select * from v1_gateway_esp_meter where Ledger=@Ledger and Disabled=1 ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }




    }
}
