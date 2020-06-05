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
        public DataTable GetModuleList(int module_id, string moduleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select b.Module_id,b.Meter_id,b.ModuleName,b.ModuleAddr,b.ModuleNo,b.Disabled,b.Remark,a.MeterName,c.Switch,c.ChrgType");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" inner join v1_gateway_esp_module as b on a.Ledger=b.Ledger and a.Meter_id=b.Meter_id");
            strSql.Append(" left join v1_gateway_esp_module_info as c on b.Ledger=c.Ledger and b.Module_id=c.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and b.ModuleName like @ModuleName");
            if (module_id != 0)
                strSql.Append(" and b.Module_id=@Module_id");
            strSql.Append(" order by b.Meter_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, ModuleName = moduleName + "%" });
        }

        /// <summary>
        /// 设置回路信息
        /// </summary>
        /// <param name="module">回路信息</param>
        /// <returns></returns>
        public int SetModule(ModuleVModel module)
        {
            StringBuilder strSql = new StringBuilder();
            if (module.Module_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Module_id)as cnt from v1_gateway_esp_module where Ledger=@Ledger");
                module.Module_id = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger })) + 1;
            }
            object params01 = new
            {
                Ledger = this.Ledger,
                Module_id = module.Module_id,
                Meter_id = module.Meter_id,
                ModuleName = module.ModuleName,
                ModuleAddr = module.ModuleAddr,
                ModuleNo = "",
                Switch = module.PSWay,
                ChrgType = module.ChrgType,
                EnergyItemCode = "0100",
                Disabled = module.Disabled,
                Remark = module.Remark,
                SysUid = this.SysUid
            };
            strSql.Clear();
            strSql.Append("insert into v1_gateway_esp_module(");
            strSql.Append("Ledger,Module_id,Meter_id,ModuleName,ModuleAddr,ModuleNo,EnergyItemCode,Disabled,Remark,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Module_id,@Meter_id,@ModuleName,@ModuleAddr,@ModuleNo,@EnergyItemCode,@Disabled,@Remark,@SysUid,now(),@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE ");
            strSql.Append("Modulename=@Modulename,");
            strSql.Append("ModuleAddr=@ModuleAddr,");
            strSql.Append("ModuleNo=@ModuleNo,");
            strSql.Append("Meter_id=@Meter_id,");
            strSql.Append("Disabled=@Disabled,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("Update_by=@SysUid,");
            strSql.Append("Update_dt=now();");
            strSql.Append("insert into v1_gateway_esp_module_info (Ledger,Module_id,Rate_id,MinPay,Si_id,IsAlarm,Update_by,Update_dt,Switch,ChrgType)values(@Ledger,@Module_id,0,0,0,0,@SysUid,now(),@Switch,@ChrgType)");
            strSql.Append("ON DUPLICATE KEY UPDATE Update_by=@SysUid,Update_dt=now(),Switch=@Switch,ChrgType=@ChrgType;");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }

        public int DelModule(int module_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete e,d");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" inner join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
            strSql.Append(" inner join v1_gateway_esp_module as d on c.Ledger=d.Ledger and c.Meter_id=d.Meter_id");
            strSql.Append(" left join v1_gateway_esp_module_info as e on d.Ledger=e.Ledger and d.Module_id=e.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and d.Module_id=@Module_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id });
        }

        /// <summary>
        /// 获取设备功能码
        /// </summary>
        /// <param name="module_id">设备ID号</param>
        /// <returns></returns>
        public DataTable GetMapFunTypeList(int module_id, int fun_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select c.Module_id,b.Fun_id,b.FunType,b.FunName,d.TagName");
            strSql.Append(" from vp_mdinfo as a inner join v0_fun as b on a.Ledger = b.Ledger and a.Mm_id = b.Mm_id");
            strSql.Append(" inner join v1_gateway_esp_module as c on a.Ledger=c.Ledger and a.Meter_id=c.Meter_id");
            strSql.Append(" left join v1_map as d on c.Ledger = d.Ledger and c.Module_id = d.Module_id and b.Fun_id = d.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and c.Module_id=@Module_id");
            if (fun_id != 0)
                strSql.Append(" and b.Fun_id=@Fun_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, Fun_id = fun_id });
        }
        /// <summary>
        /// 映射采集码对应的TagName
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public int SetMapFunType(MapVModel tag)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into v1_map(Ledger,Module_id,Fun_id,TagName,DataValue,Status,Disabled,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,@Module_id,@Fun_id,@TagName,'',0,@Disabled,@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE TagName=@TagName,Disabled=@Disabled,Update_by=@SysUid,Update_dt=now();");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = tag.Module_id, Fun_id = tag.Fun_id, TagName = tag.TagName, Disabled = tag.Disabled, SysUid = this.SysUid });
        }
    }
}
