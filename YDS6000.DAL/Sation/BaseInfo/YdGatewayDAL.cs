using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.BaseInfo
{
    partial class BaseInfoDAL
    {
        /// <summary>
        /// 获取集中器列表
        /// </summary>
        /// <param name="esp_id">id号</param>
        /// <returns></returns>
        public DataTable GetEspList(int esp_id, string espName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select b.Esp_id,b.EspName,b.EspAddr,b.EspIp,b.EspPort,b.EspType,b.TransferType,b.Inst_loc,b.Remark,b.Disabled,b.Supplier,b.ActiveTime");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" where a.Ledger=@Ledger and b.EspName like @EspName");
            if (esp_id != 0)
                strSql.Append(" and b.Esp_id=@esp_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Esp_id = esp_id, EspName = espName + "%" });
        }
        /// <summary>
        /// 设置集中器信息
        /// </summary>
        /// <param name="esp"></param>
        /// <returns></returns>
        public int SetEsp(EspVModel esp)
        {
            int gw_id = 1;
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into v1_gateway(");
            strSql.Append("Ledger,Gw_id,GwName,GwAddr,GwIp,GwPort,Timeout,GwType,GPRSNum,Inst_loc,Remark,Disabled,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append(" values (@Ledger,@Gw_id,'0.0.0.0','0.0.0.0','0.0.0.0',0,1000,'','','','',0,@SysUid,now(),@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE Update_by=@SysUid,Update_dt=now()");
            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Gw_id = gw_id, SysUid = this.SysUid });            
            if (esp.Esp_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Esp_id)as cnt from v1_gateway_esp where Ledger=@Ledger");
                esp.Esp_id = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger })) + 1;
            }
            object params01 = new { Ledger=this.Ledger, Esp_id=esp.Esp_id, Gw_id=gw_id, EspName =esp.EspName, EspAddr =esp.EspAddr,
                EspIp= "0.0.0.0" ,// esp.EspIp,
                EspPort= 1000, //esp.EspPort,
                Timeout = 1000,
                EspType = esp.EspType,
                Inst_loc = esp.Inst_loc,
                Remark = "",
                Disabled=esp.Disabled,
                Supplier=esp.Supplier,
                SysUid = this.SysUid
            };
            strSql.Clear();
            strSql.Append("insert into v1_gateway_esp(");
            strSql.Append("Ledger,Esp_id,Gw_id,EspName,EspAddr,EspIp,EspPort,Timeout,EspType,Inst_loc,Remark,Disabled,Create_by,Create_dt,Update_by,Update_dt,Supplier,ActiveTime)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Esp_id,@Gw_id,@EspName,@EspAddr,@EspIp,@EspPort,@Timeout,@EspType,@Inst_loc,@Remark,@Disabled,@SysUid,now(),@SysUid,now(),@Supplier,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE ");
            strSql.Append(" EspName=@EspName,");
            strSql.Append("EspAddr=@EspAddr,");
            strSql.Append("EspIp=@EspIp,");
            strSql.Append("EspPort=@EspPort,");
            strSql.Append("Timeout=@Timeout,");
            strSql.Append("EspType=@EspType,");
            strSql.Append("Inst_loc=@Inst_loc,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("Disabled=@Disabled,");
            strSql.Append("Supplier=@Supplier,");
            strSql.Append("Update_by =@SysUid,Update_dt=now()");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }
        /// <summary>
        /// 删除集中器
        /// </summary>
        /// <param name="esp_id"></param>
        /// <returns></returns>
        public int DelEsp(int esp_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete e,d,c,b");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" left join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
            strSql.Append(" left join v1_gateway_esp_module as d on c.Ledger=d.Ledger and c.Meter_id=d.Meter_id");
            strSql.Append(" left join v1_gateway_esp_module_info as e on d.Ledger=e.Ledger and d.Module_id=e.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and b.Esp_id=@Esp_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Esp_id=esp_id });
        }

        #region 设备型号信息
        /// <summary>
        /// 设备类型列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetMmList(int mm_id, string moduleTypeName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Mm_id,ModuleName,ModuleType,Disabled,ModulePwd,ModuleUid,Level,Spec,Fty_prod,IsCharg,Protocol,IsDefine");
            strSql.Append(" from v0_module");
            strSql.Append(" where Ledger=@Ledger and ModuleName like @ModuleName");
            if (mm_id != 0)
                strSql.Append(" and Mm_id=@Mm_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = mm_id, ModuleName = moduleTypeName + "%" });
        }

        /// <summary>
        /// 根据ID获取型号
        /// </summary>
        /// <param name="mm_id"></param>
        /// <returns></returns>
        public string GetMmType(int mm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ModuleType from v0_module where Ledger=@Ledger and Mm_id=@Mm_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = mm_id });
            return CommFunc.ConvertDBNullToString(obj);
        }
        /// <summary>
        /// 获取设备类型采集码列表
        /// </summary>
        /// <param name="mm_id"></param>
        /// <returns></returns>
        public DataTable GetMmFunTypeList(int mm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select b.Fun_id,b.Mm_id,b.FunName,b.FunType,b.Action,b.DataValue,b.DataType,b.Scale,b.Disabled,b.OrdNo");
            strSql.Append(" from v0_module as a inner join v0_fun as b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Mm_id=@Mm_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = mm_id });
        }
        /// <summary>
        /// 设置设备类型
        /// </summary>
        /// <param name="mmType">设备信息</param>
        /// <param name="v0_fun"></param>
        /// <returns></returns>
        public int SetMm(MeterTypeVModel mmType, DataTable v0_fun)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(*) from v0_module where Ledger=@Ledger and Mm_id!=@Mm_id and ModuleType=@ModuleType");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = mmType.ModuleTypeId, ModuleType = mmType.ModuleType });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("已存在此型号:" + mmType.ModuleType);
            if (mmType.ModuleTypeId == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Mm_id) from v0_module where  Ledger=@Ledger ");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger });
                mmType.ModuleTypeId = CommFunc.ConvertDBNullToInt32(obj) + 1;
            }
            object params01 = new { Ledger = this.Ledger, Mm_id = mmType.ModuleTypeId, ModuleName = mmType.ModuleName, ModuleType = mmType.ModuleType, Disabled = mmType.Disabled, Spec = mmType.Spec, Fty_prod = mmType.Fty_prod };
            strSql.Clear();
            strSql.Append("insert into v0_module(");
            strSql.Append("Ledger,Mm_id,ModuleName,ModuleType,Disabled,ModulePwd,ModuleUid,Level,Spec,Fty_prod,IsCharg,Protocol)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Mm_id,@ModuleName,@ModuleType,@Disabled,'','','',@Spec,@Fty_prod,0,'')");
            strSql.Append("ON DUPLICATE KEY UPDATE ModuleName=@ModuleName,ModuleType=@ModuleType,Disabled=@Disabled,Spec=@Spec,Fty_prod=@Fty_prod;");
            int cnt = SQLHelper.Execute(strSql.ToString(), params01);
            ////////////////////////////////////////////////////
            foreach (DataRow dr in v0_fun.Rows)
            {
                strSql.Clear();
                strSql.Append("select count(*) from v0_fun where Ledger=@Ledger and Fun_id!=@Fun_id and Mm_id=@Mm_id and FunType=@FunType");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]), Mm_id = mmType.ModuleTypeId, FunType = CommFunc.ConvertDBNullToString(dr["FunType"]) });
                if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                    continue;
                    //throw new Exception("已存在此型号功能码:" + CommFunc.ConvertDBNullToString(dr["FunType"]));
                //
                int fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]);
                if (fun_id == 0)
                {
                    strSql.Clear();
                    strSql.Append("select max(Fun_id)as cnt from v0_fun where Ledger=@Ledger");
                    obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger });
                    fun_id = CommFunc.ConvertDBNullToInt32(obj) + 1;
                    dr["Fun_id"] = fun_id;
                }
                object params_fn = new
                {
                    Ledger = this.Ledger,
                    Fun_id = fun_id,
                    Mm_id = mmType.ModuleTypeId,
                    FunName = CommFunc.ConvertDBNullToString(dr["FunName"]),
                    FunType = CommFunc.ConvertDBNullToString(dr["FunType"]),
                    Action = CommFunc.ConvertDBNullToInt32(dr["Action"]),
                    DataValue = CommFunc.ConvertDBNullToString(dr["DataValue"]),
                    DataType = CommFunc.ConvertDBNullToInt32(dr["DataType"]),
                    Scale = CommFunc.ConvertDBNullToInt32(dr["Scale"]),
                    Disabled = CommFunc.ConvertDBNullToInt32(dr["Disabled"]),
                    OrdNo = CommFunc.ConvertDBNullToInt32(dr["OrdNo"]),
                    OrdGrp = CommFunc.ConvertDBNullToInt32(dr["OrdGrp"]),
                    Unit = CommFunc.ConvertDBNullToString(dr["Unit"]),
                    PointType = CommFunc.ConvertDBNullToInt32(dr["PointType"]),
                    AlarmModel = CommFunc.ConvertDBNullToInt32(dr["AlarmModel"]),
                    SubTab = CommFunc.ConvertDBNullToString(dr["SubTab"]),
                };
                strSql.Clear();
                strSql.Append("insert into v0_fun(");
                strSql.Append("Ledger,Fun_id,Mm_id,FunName,FunType,Action,DataValue,DataType,Scale,Disabled,OrdNo,OrdGrp,Unit,PointType,AlarmModel,SubTab)");
                strSql.Append(" values (");
                strSql.Append("@Ledger,@Fun_id,@Mm_id,@FunName,@FunType,@Action,@DataValue,@DataType,@Scale,@Disabled,@OrdNo,@OrdGrp,@Unit,@PointType,@AlarmModel,@SubTab)");
                strSql.Append(" ON DUPLICATE KEY UPDATE FunName=@FunName,DataType=@DataType,OrdNo=@OrdNo,OrdGrp=@OrdGrp,Unit=@Unit,PointType=@PointType,AlarmModel=@AlarmModel");
                cnt = cnt + SQLHelper.Execute(strSql.ToString(), params_fn);
            }
            return cnt;
        }
        /// <summary>
        /// 删除类型设备
        /// </summary>
        /// <param name="mm_id">类型ID号</param>
        /// <returns></returns>
        public int DelMm(int mm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete b,a");
            strSql.Append(" from v0_module as a left join v0_fun as b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Mm_id=@Mm_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = mm_id });
        }
        #endregion


        #region 设备信息
        /// <summary>
        /// 获取电表列表
        /// </summary>
        /// <param name="esp_id"></param>
        /// <returns></returns>
        public DataTable GetMeterList(int meter_id, string meterName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Meter_id,a.Esp_id,a.MeterAddr,a.MeterName,a.MeterInst_loc as Inst_loc,a.EspName,a.Co_id as RoomId,a.MeterDisabled as Disabled,a.MeterRmk as Remark,");
            strSql.Append("a.Multiply,a.Mm_id as MeterTypeId,a.MeterType,a.MeterTypeName,a.ActiveTime,a.Supplier,b.CoName as RoomName");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" left join v1_cust as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.MeterName like @MeterName");
            if (meter_id != 0)
                strSql.Append(" and a.Meter_id=@Meter_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = meter_id, MeterName = meterName + "%" });
        }
        /// <summary>
        /// 设置集中器信息
        /// </summary>
        /// <param name="esp"></param>
        /// <returns></returns>
        public int SetMeter(MeterVModel meter)
        {
            StringBuilder strSql = new StringBuilder();
            if (meter.Meter_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Meter_id)as cnt from v1_gateway_esp_meter where Ledger=@Ledger");
                meter.Meter_id = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger })) + 1;
            }
            object params01 = new
            {
                Ledger = this.Ledger,
                Meter_id = meter.Meter_id,
                Esp_id = meter.Esp_id,
                MeterName = meter.MeterName,
                MeterAddr = meter.MeterAddr,
                Inst_loc = meter.Inst_loc,
                //Supplier = meter.Supplier,
                //Switch = meter.Switch,
                //ChrgType = meter.ChrgType,
                Disabled = meter.Disabled,
                Remark = meter.Remark,
                Multiply = meter.Multiply,
                EnergyItemCode = "01000",
                Mm_id = meter.MeterTypeId,
                Co_id = meter.RoomId,
                SysUid = this.SysUid
            };
            strSql.Clear();
            strSql.Append("insert into v1_gateway_esp_meter(");
            strSql.Append("Ledger,Meter_id,Esp_id,MeterName,MeterAddr,Inst_loc,Disabled,Remark,Multiply,EnergyItemCode,Mm_id,Co_id,Create_by,Create_dt,Update_by,Update_dt,Attrib,ActiveTime)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Meter_id,@Esp_id,@MeterName,@MeterAddr,@Inst_loc,@Disabled,@Remark,@Multiply,@EnergyItemCode,@Mm_id,@Co_id,@SysUid,now(),@SysUid,now(),0,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE ");
            strSql.Append("Esp_id=@Esp_id,");
            strSql.Append("MeterName=@MeterName,");
            strSql.Append("MeterAddr=@MeterAddr,");
            strSql.Append("Inst_loc=@Inst_loc,");
            strSql.Append("Disabled=@Disabled,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("Multiply=@Multiply,");
            strSql.Append("Mm_id=@Mm_id,");
            strSql.Append("Co_id=@Co_id,");
            strSql.Append("Update_by=@SysUid,");
            strSql.Append("Update_dt=now();");
            //strSql.Append("insert into v1_gateway_esp_module_info (Ledger,Module_id,ChrgType,Update_by,Update_dt)values(@Ledger,@Module_id,@ChrgType,@SysUid,now())");
            //strSql.Append("ON DUPLICATE KEY UPDATE ChrgType=@ChrgType,Update_by=@SysUid,Update_dt=now();");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }
        public int DelMeter(int meter_id)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append("update v1_gateway_esp_module");
            //strSql.Append(" set Co_id=0,Parent_id=0,Layer=0");
            //strSql.Append(" where Ledger=@Ledger and FIND_IN_SET(Module_id,GetMdChildList(Ledger,@Meter_id));");
            //strSql.Append("update v1_gateway_esp_module set Parent_id=0,Layer=0 where Ledger=@Ledger and Meter_id=@Meter_id and FIND_IN_SET(Module_id,GetMdChildList(Ledger,Module_id))");
            //
            strSql.Append("delete e,d,c");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" inner join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
            strSql.Append(" left join v1_gateway_esp_module as d on c.Ledger=d.Ledger and c.Meter_id=d.Meter_id");
            strSql.Append(" left join v1_gateway_esp_module_info as e on d.Ledger=e.Ledger and d.Module_id=e.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and c.Meter_id=@Meter_id;");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = meter_id });
        }

        #endregion


    }
}
