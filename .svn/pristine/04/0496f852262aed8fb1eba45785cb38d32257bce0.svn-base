using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Platform.BaseInfo
{
    public partial class BaseInfoDAL
    {

        public DataTable GetModuleOfBuildList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Co_id,a.CoName from vp_coinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Attrib=@Attrib and a.CoName!=a.CoFullName");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = (int)CoAttrib.Build });
        }

        /// <summary>
        /// 获取能耗列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetEnergyCombox()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.EnergyItemCode,a.EnergyItemName from v0_energyitemdict as a");
            strSql.Append(" where FIND_IN_SET(a.ParentItemCode,'01000,02000,03000,07000')");
            return SQLHelper.Query(strSql.ToString(), null);
        }

        /// <summary>
        /// 获取进线列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetIncomingCombox()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleName");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Attrib=1");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 获取回路信息
        /// </summary>
        /// <param name="meter_id"></param>
        /// <returns></returns>
        public DataTable GetModuleList(int meter_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Meter_id,a.MeterName,a.Module_id,a.ModuleName,a.EnergyItemCode,a.Parent_id,a.Co_id,b.CoName,c.ModuleName as ParentName");
            strSql.Append(" from vp_mdinfo as a left join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v1_gateway_esp_module as c on a.Ledger=c.Ledger and a.Parent_id=c.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Meter_id=@Meter_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = meter_id });
        }
        /// <summary>
        /// 设置回路信息列表
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="moduleName">回路名称</param>
        /// <param name="buildId">建筑ID号</param>
        /// <returns></returns>
        public int SetModuleList(int module_id, string moduleName, int buildId, string energyItemCode, int parent_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update v1_gateway_esp_meter as a inner join v1_gateway_esp_module as b on a.Ledger=b.Ledger and a.Meter_id=b.Meter_id");
            strSql.Append(" set a.Co_id=@Co_id,a.EnergyItemCode=@EnergyItemCode,a.Update_by=@SysUid,a.Update_dt=now(),");
            strSql.Append(" b.Co_id=@Co_id,b.EnergyItemCode=@EnergyItemCode,b.ModuleName=@ModuleName,b.Parent_id=case when b.Parent_id=@Parent_id then b.Parent_id else @Parent_id end,");
            strSql.Append(" b.Update_by=@SysUid,b.Update_dt=now()");
            strSql.Append(" where a.Ledger=@Ledger and b.Module_id=@Module_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, ModuleName = moduleName, Co_id = buildId, EnergyItemCode = energyItemCode, Parent_id = parent_id, SysUid = this.SysUid, });
        }
        /// <summary>
        /// 设置回路信息列表(PDU)
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="moduleName">回路名称</param>
        /// <returns></returns>
        public int SetModuleList_PDU(int module_id, string moduleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update v1_gateway_esp_module");
            strSql.Append(" set ModuleName=@ModuleName,Update_by=@SysUid,Update_dt=now()");
            strSql.Append("  where Ledger=@Ledger and Module_id=@Module_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, ModuleName = moduleName, SysUid = this.SysUid, });
        }

        /// <summary>
        /// 获取设备采集点信息
        /// </summary>
        /// <param name="meter_id"></param>
        /// <returns></returns>
        public DataTable GetModuleOfMapList(int meter_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.Fun_id,b.MeterName,a.ModuleName,a.FunType,a.FunName,c.TagName");
            strSql.Append(" from vp_funinfo as a inner join vp_mdinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            strSql.Append(" left join v1_map as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id and a.Fun_id=c.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Meter_id=@Meter_id");
            strSql.Append(" ORDER BY a.Module_id,a.Fun_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = meter_id });
        }

        /// <summary>
        /// 设置回路采集项列表
        /// </summary>
        /// <param name="module_id">设备信息ID号</param>
        /// <param name="fun_id">设备采集项ID号</param>
        /// <param name="tagName">设备采集项映射变量</param>
        /// <returns></returns>
        public int SetModuleOfMapList(int module_id, int fun_id, string tagName)
        {
            StringBuilder strSql = new StringBuilder();
            if (string.IsNullOrEmpty(tagName))
            {
                strSql.Clear();
                strSql.Append("delete from v1_map where Ledger=@Ledger and Module_id=@Module_id and Fun_id=@Fun_id");
            }
            else
            {
                strSql.Clear();
                strSql.Append("insert into v1_map(Ledger,Module_id,Fun_id,TagName,Update_by,Update_dt)values(@Ledger,@Module_id,@Fun_id,@TagName,@SysUid,now())");
                strSql.Append("ON DUPLICATE KEY UPDATE TagName=@TagName,Update_by=@SysUid,Update_dt=now()");
            }
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, Fun_id = fun_id, TagName = tagName, SysUid = this.SysUid, });
        }
    }
}
