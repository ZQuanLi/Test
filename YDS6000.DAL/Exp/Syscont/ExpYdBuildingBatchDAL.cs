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
    public partial class ExpYdBuildingBatchDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public ExpYdBuildingBatchDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取全部建筑信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetCoInfo(string coFullName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT a.Co_id,a.CoName,a.Parent_id,a.Layer,a.CoFullName");
            strSql.Append(" FROM vp_coinfo as a where a.Ledger=@Ledger and a.CoFullName like @CoFullName");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, CoFullName = coFullName + "%" });
        }

        /// <summary>
        /// 更新建筑信息
        /// </summary>
        /// <param name="co_id"></param>
        /// <param name="coName"></param>
        /// <param name="parent_id"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public int UpdateCoList(int co_id, string coName, int parent_id, int layer)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = new DataTable();
            if (co_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Co_id) as cnt from v1_cust where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                co_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1 : 1;
            }
            //更新名称
            strSql.Clear();
            strSql.Append("insert into v1_cust(");
            strSql.Append("Ledger,Co_id,CoNo,CoName,Disabled,Parent_id,Attrib,Layer,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Co_id,@Co_id,@CoName,0,@Parent_id,0,@Layer,@SysUid,now(),@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE Layer=@Layer,CoName=@CoName,Update_by=@SysUid,Update_dt=now();");
            //更新附件信息
            strSql.Append("insert into v1_custinfo(");
            strSql.Append("Ledger,Co_id,CustAddr,Office_tel,Mobile,Email,IsDefine,Update_by,Update_dt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Co_id,'','','','',0,@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE Update_by=@SysUid,Update_dt=now();");
            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id, CoName = coName, Parent_id = parent_id, Layer = layer, SysUid = this.SysUid });

            //更新全名
            obj = null;
            strSql.Clear();
            strSql.Append("select GetCoChildList(@Ledger,@Co_id) as ids;");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
            string Ids = CommFunc.ConvertDBNullToString(obj.Rows[0]["ids"]);

            strSql.Clear();
            strSql.Append("update v1_cust as a inner join v1_custinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" set b.StrucName=GetCoOnStrucName(a.Ledger,a.Co_id)"); //a.Layer=case when a.Layer=@Layer then a.Layer else a.Layer+1 end,
            strSql.Append(" where a.Ledger=@Ledger and FIND_IN_SET(a.Co_id,@Ids);");

            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Layer = layer, Ids = Ids });

            return co_id;
        }

        /// <summary>
        /// 获取全部设备信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetMdInfo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT a.Gw_id,a.GwAddr,a.GwName,a.GwIp,b.Esp_id,b.EspAddr,b.EspName,c.Meter_id,c.MeterAddr,c.MeterName,d.Module_id,d.ModuleAddr,d.ModuleName,d.Co_id,co.CoFullName,");
            strSql.Append("b.TransferType,b.EspPort,b.ComPort,b.Baud,b.DataBit,b.Parity,b.StopBit,c.Multiply,mdl.ModuleType");
            strSql.Append(" from v1_gateway as a left join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" left join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
            strSql.Append(" left join v1_gateway_esp_module as d on c.Ledger=d.Ledger and c.Meter_id=d.Meter_id");
            strSql.Append(" left join vp_coinfo as co on d.Ledger=co.Ledger and d.Co_id=co.Co_id");
            strSql.Append(" left join v0_module as mdl on c.Ledger=mdl.Ledger and c.Mm_id=mdl.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        public DataTable GetMdInfo(string gwName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT a.Gw_id,a.GwIp");
            strSql.Append(" from v1_gateway as a");
            strSql.Append(" where a.Ledger=@Ledger and a.GwName=@GwName limit 1");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, gwName = gwName });
        }

        public int UpdateGwInfo(int gw_id, string gwName, string gwIp, out string msg)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = new DataTable();
            msg = "";
            if (gw_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Gw_id) as cnt from v1_gateway where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                gw_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1 : 1;
            }
            obj = null;
            strSql.Clear();
            strSql.Append("select count(*) as cnt from v1_gateway where Ledger=@Ledger and Gw_id!=@Gw_id and GwName=@GwName");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Gw_id = gw_id, GwName = gwName });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) > 0)
            {
                msg = "集中器名称" + gwName + "重复";
                throw new Exception("集中器名称" + gwName + "重复");
            }
            strSql.Clear();
            strSql.Append("insert into v1_gateway(");
            strSql.Append("Ledger,Gw_id,GwName,GwAddr,GwIp,GwPort,Timeout,GwType,Inst_loc,Remark,Disabled,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Gw_id,@GwName,@GwIp,@GwIp,1000,1000,'','','',0,@SysUid,now(),@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE GwIp=@GwIp,Update_by=@SysUid,Update_dt=now();");
            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Gw_id = gw_id, GwName = gwName, GwIp = gwIp, SysUid = this.SysUid });
            return gw_id;
        }

        public DataTable GetEspInfo(int gw_id, string espName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT b.Esp_id,b.EspAddr,b.EspName,");
            strSql.Append("b.TransferType,b.EspPort,b.ComPort,b.Baud,b.DataBit,b.Parity,b.StopBit");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Gw_id=@Gw_id and b.EspName=@EspName limit 1");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Gw_id = gw_id, EspName = espName });
        }

        public int UpdateEspInfo(v1_gateway_espVModel Esp, out string msg)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = new DataTable();
            msg = "";
            if (Esp.Esp_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Esp_id) as cnt from v1_gateway_esp where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                Esp.Esp_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1 : 1;
            }
            obj = null;
            strSql.Clear();
            strSql.Append("select count(*) as cnt from v1_gateway_esp where Ledger=@Ledger and Esp_id!=@Esp_id and Gw_id=@Gw_id and (EspName=@EspName or EspAddr=@EspAddr)");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Esp_id = Esp.Esp_id, Gw_id = Esp.Gw_id, EspName = Esp.EspName, EspAddr = Esp.EspAddr });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) > 0)
            {
                msg = "采集器名称" + Esp.EspName + "或地址" + Esp.EspAddr + "重复";
                throw new Exception("采集器名称" + Esp.EspName + "或地址" + Esp.EspAddr + "重复");
            }

            obj = null;
            strSql.Clear();
            strSql.Append("select GwIp from v1_gateway where Ledger=@Ledger and Gw_id=@Gw_id");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Gw_id = Esp.Gw_id });
            string EspIp = CommFunc.ConvertDBNullToString(obj.Rows[0]["GwIp"]);

            strSql.Clear();
            strSql.Append("insert into v1_gateway_esp(");
            strSql.Append("Ledger,Esp_id,Gw_id,EspName,EspAddr,TransferType,EspIp,EspPort,Timeout,EspType,ComPort,Baud,DataBit,StopBit,Parity,Inst_loc,Remark,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Esp_id,@Gw_id,@EspName,@EspAddr,@TransferType,@EspIp,@EspPort,1000,'',@ComPort,@Baud,@DataBit,@StopBit,@Parity,'','',@SysUid,now(),@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE EspAddr=@EspAddr,TransferType=@TransferType,EspIp=@EspIp,EspPort=@EspPort,ComPort=@ComPort,Baud=@Baud,DataBit=@DataBit,StopBit=@StopBit,Parity=@Parity,Update_by=@SysUid,Update_dt=now();");
            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Esp_id = Esp.Esp_id, Gw_id = Esp.Gw_id, EspName = Esp.EspName, EspAddr = Esp.EspAddr, TransferType = Esp.TransferType, EspIp = EspIp, EspPort = Esp.EspPort, ComPort = Esp.ComPort, Baud = Esp.Baud, DataBit = Esp.DataBit, StopBit = Esp.StopBit, Parity = Esp.Parity, SysUid = this.SysUid });
            return Esp.Esp_id;
        }

        public DataTable GetMeterInfo(int esp_id, string meterAddr)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT c.Meter_id,c.MeterAddr,c.MeterName,c.Multiply,mdl.ModuleType");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" inner join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
            strSql.Append(" left join v0_module as mdl on c.Ledger=mdl.Ledger and c.Mm_id=mdl.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger and c.Esp_id=@Esp_id and c.MeterAddr=@MeterAddr limit 1");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Esp_id = esp_id, MeterAddr = meterAddr });
        }

        public int UpdateMeterInfo(v1_gateway_esp_meterVModel meter, string moduleType, out string msg)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = new DataTable();
            msg = "";

            strSql.Append("select Mm_id from v0_module where Ledger=@Ledger and ModuleType=@ModuleType");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, ModuleType = moduleType });
            meter.Mm_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Mm_id"]) : 0;
            if (meter.Mm_id == 0)
            {
                msg = "电表型号" + moduleType + "不存在";
                throw new Exception("电表型号" + moduleType + "不存在");
            }

            if (meter.Meter_id == 0)
            {
                obj = null;
                strSql.Clear();
                strSql.Append("select max(Meter_id) as cnt from v1_gateway_esp_meter where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                meter.Meter_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1 : 1;
            }
            obj = null;
            strSql.Clear();
            strSql.Append("select count(*) as cnt from v1_gateway_esp_meter where Ledger=@Ledger and Meter_id!=@Meter_id and Esp_id=@Esp_id and MeterAddr=@MeterAddr");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Esp_id = meter.Esp_id, Meter_id = meter.Meter_id, MeterAddr = meter.MeterAddr });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) > 0)
            {
                msg = "设备地址:" + meter.MeterAddr + "重复";
                throw new Exception("设备地址:" + meter.MeterAddr + "重复");
            }

            strSql.Clear();
            strSql.Append("insert into v1_gateway_esp_meter(");
            strSql.Append("Ledger,Meter_id,Esp_id,MeterName,MeterAddr,MeterNo,Inst_loc,Disabled,Remark,Multiply,EnergyItemCode,Mm_id,Co_id,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Meter_id,@Esp_id,@MeterAddr,@MeterAddr,NULL,'',0,'',@Multiply,NULL,@Mm_id,0,@SysUid,now(),@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE MeterAddr=@MeterAddr,Multiply=@Multiply,Mm_id=@Mm_id,Update_by=@SysUid,Update_dt=now();");
            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Esp_id = meter.Esp_id, Meter_id = meter.Meter_id, MeterAddr = meter.MeterAddr, Multiply = meter.Multiply, Mm_id = meter.Mm_id, SysUid = this.SysUid });
            return meter.Meter_id;
        }

        public DataTable GetMdInfo(int meter_id, string moduleAddr)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT d.Module_id,d.ModuleAddr,d.ModuleName,d.Co_id,co.CoFullName");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" inner join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
            strSql.Append(" inner join v1_gateway_esp_module as d on c.Ledger=d.Ledger and c.Meter_id=d.Meter_id");
            strSql.Append(" left join vp_coinfo as co on d.Ledger=co.Ledger and d.Co_id=co.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and d.Meter_id=@Meter_id and d.ModuleAddr=@ModuleAddr limit 1");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = meter_id, ModuleAddr = moduleAddr });
        }

        public int UpdateModuleInfo(v1_gateway_esp_moduleVModel Md, string coFullName, out string msg)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = new DataTable();
            msg = "";

            strSql.Append("select Co_id from vp_coinfo where Ledger=@Ledger and CoFullName=@CoFullName");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, CoFullName = coFullName });
            Md.Co_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Co_id"]) : 0;
            if (Md.Co_id == 0)
            {
                msg = "房间" + coFullName + "不存在";
                throw new Exception("房间" + coFullName + "不存在");
            }

            if (Md.Module_id == 0)
            {
                obj = null;
                strSql.Clear();
                strSql.Append("select max(Module_id) as cnt from v1_gateway_esp_module where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                Md.Module_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1 : 1;
            }
            obj = null;
            strSql.Clear();
            strSql.Append("select Esp_id from v1_gateway_esp_meter where Ledger=@Ledger and Meter_id=@Meter_id");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = Md.Meter_id });
            string Esp_id = CommFunc.ConvertDBNullToString(obj.Rows[0]["Esp_id"]);

            obj = null;
            strSql.Clear();
            strSql.Append("select count(*) as cnt from v1_gateway_esp_meter as a inner join v1_gateway_esp_module as b on a.Ledger=b.Ledger and a.Meter_id=b.Meter_id");
            strSql.Append(" where a.Ledger=@Ledger and b.Module_id!=@Module_id and a.Esp_id=@Esp_id and b.ModuleAddr=@ModuleAddr");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Md.Module_id, Esp_id = Esp_id, ModuleAddr = Md.ModuleAddr });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) > 0)
            {
                msg = "同一采集器下回路地址" + Md.ModuleAddr + "重复";
                throw new Exception("同一采集器下回路地址" + Md.ModuleAddr + "重复");
            }

            strSql.Clear();
            strSql.Append("insert into v1_gateway_esp_module(");
            strSql.Append("Ledger,Module_id,Meter_id,ModuleName,ModuleAddr,Co_id,Disabled,Remark,EnergyItemCode,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Module_id,@Meter_id,@ModuleAddr,@ModuleAddr,@Co_id,0,'','01000',@SysUid,now(),@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE ModuleAddr=@ModuleAddr,Co_id=@Co_id,Update_by=@SysUid,Update_dt=now();");
            ///
            strSql.Append("insert into v1_gateway_esp_module_info(");
            strSql.Append("Ledger,Module_id,IsAlarm,MinPay,Price,Rate_id,Update_by,Update_dt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Module_id,0,0,0,0,@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE Update_by=@SysUid,Update_dt=now();");//MinPay=@MinPay,Price=@Price,Rate_id=@Rate_id,
            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Md.Module_id, Meter_id = Md.Meter_id, ModuleAddr = Md.ModuleAddr, Co_id = Md.Co_id, SysUid = this.SysUid });
            return Md.Module_id;
        }


    }
}
