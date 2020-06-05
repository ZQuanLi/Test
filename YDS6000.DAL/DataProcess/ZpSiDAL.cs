using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.DataProcess
{
    public class ZpSiDAL
    {
        private string Ledgers = "";
        private int SysUid = 1;
        public ZpSiDAL(string _ledgers, int uid)
        {
            this.Ledgers = _ledgers;
            this.SysUid = uid;
        }

        /// <summary>
        /// 定时拉合闸数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetV1_si_ssr()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Ledger,a.Esp_id,a.ComPort,a.Baud,a.DataBit,a.Parity,a.StopBit,");
            strSql.Append("a.Ip,a.TcpPort,a.TimeOut,a.HandledBY,a.ModuleType,a.ModulePwd,a.ModuleUid,a.TransferType,");
            strSql.Append("a.Module_id,a.ModuleAddr,a.Fun_id,a.FunName,a.FunType,a.Protocol,a.Co_id,a.Action,a.LpszDbVarName,");
            strSql.Append("c.Si_id,c.SiSSR,c.Md,c.Wk,c.Ts,md.Meter_id,md.MeterAddr"); //
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" inner join vp_mdinfo as md on a.Ledger=md.Ledger and a.Module_id=md.Module_id");
            strSql.Append(" inner join v1_gateway_esp_module_info as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            strSql.Append(" inner join v1_si_ssr as c on b.Ledger=c.Ledger and b.Si_id=c.Si_id");
            strSql.Append(" where a.FunType=@Ssr and c.Disabled=0");
            if (!string.IsNullOrEmpty(this.Ledgers))
                strSql.Append(" and FIND_IN_SET(a.Ledger,@Strlist)");
            return SQLHelper.Query(strSql.ToString(), new { Strlist = this.Ledgers, Ssr = V0Fun.Ssr.ToString() });
        }
        
        /// <summary>
        /// 拉闸数据
        /// </summary>
        /// <returns></returns>
        public CommandVModel GetSsrInfo(int ledger, int module_id,string funType,string dataValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into v2_command(");
            strSql.Append("CDate,Ledger,Co_id,Module_id,ModuleAddr,Fun_id,FunType,Action,DataValue,Create_by,Create_dt,ErrCode,IsRn)");
            strSql.Append("select now(),a.Ledger,a.Co_id,a.Module_id,a.ModuleAddr,a.Fun_id,a.FunType,a.Action,@DataValue,@SysUid,now(),0,3");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and a.FunType=@FunType;");
            strSql.Append(" select * from vp_cmdinfo where Log_id = last_insert_id();");
            DataTable dtSource = SQLHelper.Query(strSql.ToString(), new { Ledger = ledger, Module_id = module_id, FunType = funType, DataValue = dataValue, SysUid = this.SysUid });
            CommandVModel cmd = null;
            if (dtSource.Rows.Count > 0)
                cmd = ModelHandler<CommandVModel>.FillModel(dtSource.Rows[0]);
            return cmd;
        }

        public int GetTransferType(int ledger, int module_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.TransferType from vp_mdinfo as a where a.Ledger=@Ledger and a.Module_id=@Module_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = ledger, Module_id = module_id });
            return CommFunc.ConvertDBNullToInt32(obj);
        }


        /// <summary>
        /// 拉闸数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetSsr1(int ledger,int meter_id, string meterAddr)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Ledger,a.Esp_id,a.ComPort,a.Baud,a.DataBit,a.Parity,a.StopBit,");
            strSql.Append("a.Ip,a.TcpPort,a.TimeOut,a.HandledBY,a.ModuleType,a.ModulePwd,a.ModuleUid,a.TransferType,");
            strSql.Append("a.Module_id,a.ModuleAddr,a.Fun_id,a.FunName,a.FunType,a.Protocol,a.Co_id,a.Action,a.LpszDbVarName");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" inner join vp_mdinfo as md on a.Ledger=md.Ledger and a.Module_id=md.Module_id");
            strSql.Append(" inner join v1_gateway_esp_module_info as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and a.FunType=@Ssr and md.Meter_id=@Meter_id and a.ModuleAddr=@MeterAddr");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = ledger, Ssr = V0Fun.Ssr.ToString(), Meter_id = meter_id, MeterAddr = meterAddr });
        }

        public DataTable GetSsr0(int ledger,int meter_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Ledger,a.Esp_id,a.ComPort,a.Baud,a.DataBit,a.Parity,a.StopBit,");
            strSql.Append("a.Ip,a.TcpPort,a.TimeOut,a.HandledBY,a.ModuleType,a.ModulePwd,a.ModuleUid,a.TransferType,");
            strSql.Append("a.Module_id,a.ModuleAddr,a.Fun_id,a.FunName,a.FunType,a.Protocol,a.Co_id,a.Action,a.LpszDbVarName");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" inner join vp_mdinfo as md on a.Ledger=md.Ledger and a.Module_id=md.Module_id");
            strSql.Append(" inner join v1_gateway_esp_module_info as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and a.FunType=@Ssr and md.Meter_id=@Meter_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = ledger, Ssr = V0Fun.Ssr.ToString(), Meter_id = meter_id });
        }


        /// <summary>
        /// 设备是否欠费,看回路的地址与设备的地址一致
        /// </summary>
        /// <param name="co_id"></param>
        /// <param name="meter_id"></param>
        /// <param name="meterAddr"></param>
        /// <returns></returns>
        public bool GetMeterQianFei(int ledger,int co_id, int meter_id, string meterAddr)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(a.Ledger)");
            strSql.Append(" from vp_v2info as a");
            strSql.Append(" inner join vp_mdinfo as md on a.Ledger=md.Ledger and a.Module_id=md.Module_id and a.ModuleAddr=md.ModuleAddr and a.Co_id=md.Co_id and a.Mm_id=md.Mm_id");
            strSql.Append(" inner join sys_config as b on a.Ledger = b.Ledger and b.CfKey = 'AlarmValOd' and ifnull(b.Rule,0)='1'");
            strSql.Append(" where a.Ledger=@Ledger and md.Meter_id = @Meter_id and a.ModuleAddr = @MeterAddr and a.FunType = 'E' and a.RdVal <= ifnull(b.CfValue, 0)"); //and a.Co_id=@Co_id
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = ledger, Co_id = co_id, Meter_id = meter_id, MeterAddr = meterAddr });
            return CommFunc.ConvertDBNullToInt32(obj) > 0 ? true : false;
        }

        public bool GetModuleQianFei(int ledger, int co_id, int module_id, string moduleAddr)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(a.Ledger)");
            strSql.Append(" from vp_v2info as a ");
            strSql.Append(" inner join vp_mdinfo as md on a.Ledger=md.Ledger and a.Module_id=md.Module_id and a.ModuleAddr=md.ModuleAddr and a.Co_id=md.Co_id and a.Mm_id=md.Mm_id");
            strSql.Append(" inner join sys_config as b on a.Ledger = b.Ledger and b.CfKey = 'AlarmValOd' and ifnull(b.Rule,0)='1'");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id = @Module_id and a.ModuleAddr = @ModuleAddr and a.FunType = 'E' and a.RdVal <= ifnull(b.CfValue, 0)");//and a.Co_id=@Co_id 
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = ledger, Co_id = co_id, Module_id = module_id, ModuleAddr = moduleAddr });
            return CommFunc.ConvertDBNullToInt32(obj) > 0 ? true : false;
        }

        public string GetSSrIOService(int ledger, int module_id, string funType, out string dataValue)
        {
            dataValue = "";
            object params01 = new
            {
                Ledger = ledger,
                Module_id = module_id,
                FunType = funType.ToString(),
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select b.Fun_id,c.TagName,case when ifnull(c.DataValue,'')='' then b.DataValue else c.DataValue end as DataValue");
            strSql.Append(" from vp_mdinfo as a inner join v0_fun as b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id");
            strSql.Append(" inner join v1_map as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id and b.Fun_id=c.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and b.FunType=@FunType and b.Disabled=0 and c.Disabled=0");
            DataTable dtSource = SQLHelper.Query(strSql.ToString(), params01);
            string tagName = "";
            foreach (DataRow dr in dtSource.Rows)
            {
                tagName = CommFunc.ConvertDBNullToString(dr["TagName"]);
                dataValue = CommFunc.ConvertDBNullToString(dr["DataValue"]);
            }
            return tagName;
        }
    }
}
