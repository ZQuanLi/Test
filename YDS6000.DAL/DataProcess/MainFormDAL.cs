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
    public class MainFormDAL
    {
        private string Ledgers = "";
        public MainFormDAL(string _ledgers)
        {
            this.Ledgers = _ledgers;
            //this.SysUid = uid;
        }

        public DataTable GetV1_gateway()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Ledger,Gw_id,GwName,GwAddr,GwIp,GwPort,Timeout,GwType,Inst_loc,Remark,Disabled from v1_gateway");
            strSql.Append(" where Disabled=0");
            if (!string.IsNullOrEmpty(this.Ledgers))
                strSql.Append(" and FIND_IN_SET(Ledger,@Strlist)");                
            return SQLHelper.Query(strSql.ToString(), new { Strlist = this.Ledgers });
        }

        public DataTable GetV1_gateway_esp()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Ledger,Esp_id,Gw_id,EspName,EspAddr,TransferType,EspIp,EspPort,Timeout,EspType,Inst_loc,ComPort,Baud,DataBit,Parity,StopBit,Remark,Disabled from v1_gateway_esp");
            strSql.Append(" where Disabled=0");
            if (!string.IsNullOrEmpty(this.Ledgers))
                strSql.Append(" and FIND_IN_SET(Ledger,@Strlist)  and Disabled=0");
            return SQLHelper.Query(strSql.ToString(), new { Strlist = this.Ledgers });
        }
        public DataTable GetV1_gateway_esp_module()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Ledger,b.Module_id,a.Esp_id,b.ModuleName,b.ModuleAddr,a.Inst_loc,a.Disabled,a.Remark,a.Multiply,a.EnergyItemCode,a.Mm_id,b.Co_id,b.Parent_id,b.Layer,c.CoFullName");
            strSql.Append(" from v1_gateway_esp_meter as a inner join v1_gateway_esp_module as b on a.Ledger=b.Ledger and a.Meter_id=b.Meter_id");
            strSql.Append(" left join vp_coinfo as c on b.Ledger=c.Ledger and b.Co_id=c.Co_id");
            strSql.Append(" where a.Disabled=0 and b.Disabled=0");
            if (!string.IsNullOrEmpty(this.Ledgers))
                strSql.Append(" and FIND_IN_SET(a.Ledger,@Strlist)  ");              
            return SQLHelper.Query(strSql.ToString(), new { Strlist = this.Ledgers });
        }

        public DataTable GetArmData()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Ledger,a.Gw_id,a.GwAddr,a.Esp_id,a.EspAddr,a.EspIp,a.EspPort,a.ComPort,a.Baud,a.DataBit,a.Parity,a.StopBit,");
            strSql.Append("a.GwIp as Ip,a.EspPort as TcpPort,a.EspTimeout as TimeOut,a.HandledBY,a.`MeterType` as ModuleType, a.`MeterPwd` as ModulePwd,a.MeterUid as  ModuleUid,a.TransferType,");
            strSql.Append("a.Module_id,a.ModuleAddr,a.ModuleName,a.Co_id,b.Fun_id,b.FunName,b.FunType,b.Action,b.LpszDbVarName,");
            strSql.Append("c.CoFullName as StrucName,a.Protocol,v2.LastVal,v2.LastTime,s1.FrMd,a.MeterAddr");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" inner join vp_funinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and b.Action!=1 and b.Disabled=0");
            strSql.Append(" inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" left join vp_coinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" left join v2_info as v2 on a.Ledger=v2.Ledger and a.Co_id=v2.Co_id and a.Module_id=v2.Module_id and a.ModuleAddr=v2.ModuleAddr and b.Fun_id=v2.Fun_id");
            strSql.Append(" left join v1_map as mp on b.Ledger=mp.Ledger and b.Module_id=mp.Module_id and b.Fun_id=mp.Fun_id");
            strSql.Append(" where ifnull(mp.TagName,'')='' and a.TransferType!=4 and ifnull(mp.Disabled,0)=0");
            strSql.Append(" and a.GwDisabled=0 and a.EpDisabled=0 and a.MeterDisabled=0 and a.MdDisabled=0");
            //strSql.Append(" and b.FunType!='EventWpf'");/*测试用*/
            //strSql.Append(" and (a.ModuleAddr='166610222091' or a.ModuleAddr='166610223341' or a.ModuleAddr='152810206129')");
            if (!string.IsNullOrEmpty(this.Ledgers))
                strSql.Append(" and FIND_IN_SET(a.Ledger,@Strlist)");
            strSql.Append(" order by a.Module_id,b.OrdNo,b.Fun_id");
            //strSql.Append(" limit 100");
            return SQLHelper.Query(strSql.ToString(), new { Strlist = this.Ledgers });
        }

        public DataTable GetCurData(int ledger, int module_id, string funType, string dataValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Ledger,a.Esp_id,b.CoStrcName as StrucName,a.ComPort,a.Baud,a.DataBit,a.Parity,a.StopBit,a.Ip,a.TcpPort,");
            strSql.Append("a.TimeOut,a.HandledBY,a.ModuleType,a.ModulePwd,a.ModuleUid,a.TransferType,");
            strSql.Append("a.Module_id,a.ModuleAddr,a.Fun_id,a.FunName,a.FunType,a.Protocol,a.Co_id,a.Action,a.LpszDbVarName,");
            strSql.Append("case when ifnull(@DataValue,'')='' then ifnull(vf.DataValue,'') else @DataValue end as DataValue");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" inner join v0_fun as vf on a.Ledger=vf.Ledger and a.Fun_id=vf.Fun_id and vf.Disabled=0");
            strSql.Append(" left join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v1_map as mp on a.Ledger=mp.Ledger and a.Module_id=mp.Module_id and a.Fun_id=mp.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and ifnull(mp.TagName,'')='' and a.TransferType!=4 and ifnull(mp.Disabled,0)=0");
            if (funType.Equals("SginSet"))
                strSql.Append(" and a.Action=1");
            else
                strSql.Append(" and a.FunType=@FunType");
            strSql.Append(" order by a.Module_id,a.OrdNo,a.Fun_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = ledger, Module_id = module_id, FunType = funType, DataValue = dataValue });
        }

        public DataTable GetWrData()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Ledger,a.Esp_id,b.CoStrcName as StrucName,a.ComPort,a.Baud,a.DataBit,a.Parity,a.StopBit,a.Ip,a.TcpPort,");
            strSql.Append("a.TimeOut,a.HandledBY,a.ModuleType,a.ModulePwd,a.ModuleUid,a.TransferType,");
            strSql.Append("a.Module_id,a.ModuleAddr,a.Fun_id,a.FunName,a.FunType,a.Protocol,ifnull(mp.DataValue,'') as DataValue,a.Co_id,a.Action,a.LpszDbVarName");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" inner join v0_fun as vf on a.Ledger=vf.Ledger and a.Fun_id=vf.Fun_id and vf.Disabled=0");
            strSql.Append(" left join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v1_map as mp on a.Ledger=mp.Ledger and a.Module_id=mp.Module_id and a.Fun_id=mp.Fun_id");
            strSql.Append(" where a.Action=1");
            if (!string.IsNullOrEmpty(this.Ledgers))
                strSql.Append(" and FIND_IN_SET(a.Ledger,@Strlist)");
            strSql.Append(" order by a.Module_id,a.OrdNo,a.Fun_id");
            return SQLHelper.Query(strSql.ToString(), new { Strlist = this.Ledgers });
        }

        /// <summary>
        /// 登陆用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public DataTable GetSys_user(int ledger,string uSign)
        {
            object params01 = new { Ledger = ledger, USign = uSign, EncryptPwd = "DiLidili" };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Uid,a.USign,GetDECODE(a.UPasswd,@EncryptPwd) UPasswd");
            strSql.Append(" from sys_user as a");
            strSql.Append(" where a.Ledger=@Ledger and a.USign=@USign");
            return SQLHelper.Query(strSql.ToString(), params01);
        }
    }
}
