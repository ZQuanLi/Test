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
    public class AlarmDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public AlarmDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取告警设置数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetAlarmConfig()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.CfKey,a.CfValue,a.Rule,a.Unit from sys_config as a where a.Ledger=@Ledger and a.CfType='Alarm' order by a.CfKey");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 获取告警数据
        /// </summary>
        /// <param name="alarmVal"></param>
        /// <returns></returns>
        public DataTable GetAlarmData(decimal alarmVal,string unit)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select m1.Esp_id,m1.ComPort,m1.Baud,m1.DataBit,m1.Parity,m1.StopBit,m1.Ip,m1.TcpPort,m1.TimeOut,m1.HandledBY,m1.ModuleType,m1.ModulePwd,m1.ModuleUid,");
            strSql.Append("m1.TransferType,m1.Module_id,m1.ModuleAddr,m1.Fun_id,m1.FunName,m1.FunType,m1.DataValue,m1.Co_id,m1.Mm_id,");
            strSql.Append("b.CoName,c.LastTime,round(c.LastVal * m1.Multiply,m1.Scale)as LastVal,CONCAT(m1.Module_id,'.',m1.ModuleAddr,'.Ssr')as LpszSsr,s1.ChrgType,");
            strSql.Append("round(if(ifnull(vm.price, 0) > 0, vm.price, s1.price), 3) as Price,m1.ModuleName,");
            strSql.Append("round(case when s1.ChrgType<=1 then a.SyVal else ifnull(c.RdVal,0)*m1.Multiply end,m1.Scale) as RdVal,");
            strSql.Append("round(case when s1.ChrgType<=1 then a.SyVal else ifnull(c.RdVal,0)*m1.Multiply end * round(if(ifnull(vm.price, 0) > 0, vm.price, s1.price), 3) ,m1.Scale) as RdAmt,");
            strSql.Append("case when ifnull(vm.AlarmVal,0)=0 then s1.OdValue else vm.AlarmVal end as OdValue,vm.IsAlarm"); //b.TagName
            strSql.Append(" from vp_cobill as a inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" inner join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" inner join vp_funinfo as m1 on a.Ledger=m1.Ledger and a.Fun_id=m1.Fun_id and a.Co_id=m1.Co_id and m1.FunType='E'");
            strSql.Append(" inner join vp_mdinfo as md on m1.Ledger = md.Ledger and m1.Module_id = md.Module_id");
            strSql.Append(" inner join v0_fun as vf on a.Ledger = vf.Ledger and a.Fun_id = vf.Fun_id");
            strSql.Append(" inner join v1_gateway_esp_module_info as vm on m1.Ledger=vm.Ledger and m1.Module_id=vm.Module_id");
            strSql.Append(" left join vp_v2info as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id and m1.Module_id=c.Module_id and m1.ModuleAddr=c.ModuleAddr and c.Fun_id=m1.Fun_id and m1.Mm_id=c.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger");
            //strSql.Append(" and m1.Parent_id=case when s1.Zb=0 then 0 else m1.Parent_id end");
            strSql.Append(" and md.ModuleAddr=case when s1.Zb=0 then md.MeterAddr else md.ModuleAddr end");
            if (string.IsNullOrEmpty(unit) || unit.ToLower().Equals("kWh".ToLower()))
            {
                //strSql.Append(" and round(case when s1.ChrgType<=1 then a.SyVal else ifnull(c.RdVal,0)*m1.Multiply end,m1.Scale) <= @AlarmVal");
                strSql.Append(" and case when s1.ChrgType<=1 then a.SyVal else ifnull(c.RdVal,0)*m1.Multiply end <= case when s1.ChrgType<=1 or ifnull(vm.AlarmVal,0)=0 then @AlarmVal else vm.AlarmVal end");
            }
            else
            {
                //strSql.Append(" and round(case when s1.ChrgType<=1 then a.SyVal else ifnull(c.RdVal,0)*m1.Multiply end * round(if(ifnull(vm.price, 0) > 0, vm.price, s1.price), 3) ,m1.Scale) <= @AlarmVal");
                strSql.Append(" and round(case when s1.ChrgType<=1 then a.SyVal else ifnull(c.RdVal,0)*m1.Multiply end * round(if(ifnull(vm.price, 0) > 0, vm.price, s1.price), 3) ,m1.Scale) <= case when s1.ChrgType<=1 or ifnull(vm.AlarmVal,0)=0 then @AlarmVal else vm.AlarmVal end");
            }
            strSql.Append(" order by m1.Co_id,m1.Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, AlarmVal= alarmVal });
        }

        /// <summary>
        /// 获取告警信息
        /// </summary>
        /// <param name="co_id"></param>
        /// <param name="module_id"></param>
        /// <param name="moduleAddr"></param>
        /// <param name="aType"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public v2_alarm_logVModel GetAlarmModel(int co_id, int module_id, string moduleAddr, string aType, decimal rule)
        {
            //double hh = (double)(0 - (rule * 24));
            //int hh = (int)rule + 1;
            int hd = (int)Math.Ceiling(rule) - 1;
            DateTime cTime = DateTime.Now.AddDays(0 - hd);
            object params01 = new
            {
                Ledger = this.Ledger,
                CDate = new DateTime(cTime.Year, cTime.Month, cTime.Day),
                Co_id = co_id,
                Module_id = module_id,
                ModuleAddr = moduleAddr,
                AType = aType,
                CTime = new DateTime(cTime.Year, cTime.Month, cTime.Day, cTime.Hour, 59, 59),
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Log_id,CDate,Co_id,Module_id,ModuleAddr,AType,Content,str_to_date(CONCAT(CDate,' ',CTime), '%Y-%m-%d %H:%i:%s') as CTime,ErrCode,ErrTxt");
            strSql.Append(" from v2_alarm_log");
            strSql.Append(" where CDate>=@CDate and Co_id=@Co_id and Module_id=@Module_id and ModuleAddr=@ModuleAddr and AType=@AType");
            //strSql.Append(" and str_to_date(CONCAT(CDate,' ',CTime), '%Y-%m-%d %H:%i:%s')>=@CTime");
            strSql.Append(" order by Log_id desc limit 1");
            DataTable tab = SQLHelper.Query(strSql.ToString(), params01);
            v2_alarm_logVModel mm = null;
            if (tab.Rows.Count > 0)
            {
                DateTime cdt = CommFunc.ConvertDBNullToDateTime(tab.Rows[0]["CTime"]);
                decimal dd = ((int)(DateTime.Now - cdt).TotalDays) * 24;
                decimal hh = (decimal)(DateTime.Now - cdt).TotalHours;
                /////////////////////////////////////////////////////////////////////
                decimal last = dd > hh ? dd : hh;
                if ((rule * 24) > last)
                    mm = ModelHandler<v2_alarm_logVModel>.FillModel(tab.Rows[0]);
            }
            return mm;
        }
        public DataTable GetAlarm_log_hand(long log_id, string hdType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Aid,a.HdType,a.Content,a.ErrCode,a.ErrTxt");
            strSql.Append(" from v2_alarm_log_hand as a ");
            strSql.Append(" where a.Log_id=@Log_id and a.HdType=@HdType ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Log_id = log_id , HdType =hdType});
        }

        public DataTable GetCrmOfCo(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.CrmName,a.MPhone,a.Email");
            strSql.Append(" from v3_user as a inner join v3_userinfo as b on a.Ledger=b.Ledger and a.Crm_id=b.Crm_id");
            strSql.Append(" where a.Ledger=@Ledger and b.Co_id=@Co_id ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
        }

        public DataTable GetOpenidList(int co_id, long log_id, string hdType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.CrmName,c.OpenID,d.Aid,d.ErrCode,d.ErrTxt");
            strSql.Append(" from v3_user as a inner join v3_userinfo as b on a.Ledger=b.Ledger and a.Crm_id=b.Crm_id");
            strSql.Append(" inner join v3_openid as c on a.Ledger=c.Ledger and a.Crm_id=c.Crm_id");
            strSql.Append(" left join v2_alarm_log_hand d on d.Log_id=@Log_id and d.HdType=@HdType and d.HdKey=c.OpenID");
            strSql.Append(" where a.Ledger=@Ledger and b.Co_id=@Co_id and c.OpenID!=''");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id, Log_id = log_id, HdType = hdType });
        }

        //public DataTable GetOpenidList(int co_id)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("select a.CrmName,c.OpenID");
        //    strSql.Append(" from v3_user as a inner join v3_userinfo as b on a.Ledger=b.Ledger and a.Crm_id=b.Crm_id");
        //    strSql.Append(" inner join v3_openid as c on a.Ledger=c.Ledger and a.Crm_id=c.Crm_id");
        //    strSql.Append(" where a.Ledger=@Ledger and b.Co_id=@Co_id");
        //    return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
        //}

        public int AddAlarm(v2_alarm_logVModel am)
        {
            //object params01 = new { Ledger = this.Ledger, Log_id = am.Log_id, Co_id = am.Co_id, Module_id = am.Module_id, ModuleAddr = am.ModuleAddr, AType = am.AType, Content = am.Content, ErrCode = am.ErrCode, ErrTxt= am.ErrTxt };          
            //StringBuilder strSql = new StringBuilder();
            //if (am.Log_id == 0)
            //{
            //    strSql.Clear();
            //    strSql.Append("insert into v2_alarm_log(");
            //    strSql.Append("CDate,Ledger,Co_id,Module_id,ModuleAddr,Fun_id,AlarmType,AlarmName,Content,Create_by,Create_dt,CollectTime,CollectValue,Grade,Total,DataCfg,ErrCode,ErrTxt)");
            //    strSql.Append(" values (");
            //    strSql.Append("@CDate,@Ledger,@Co_id,@Module_id,@ModuleAddr,@Fun_id,@FunType,@FunName,@Content,@SysUid,now(),@CollectTime,@CollectValue,@Grade,1,@DataCfg,@ErrCode,@ErrTxt)");


            //    strSql.Append("insert into v2_alarm_log(");
            //    strSql.Append("CDate,Ledger,Co_id,Module_id,ModuleAddr,AType,Content,CTime,CollectTime,ErrCode,ErrTxt)");
            //    strSql.Append(" values (");
            //    strSql.Append("now(),@Ledger,@Co_id,@Module_id,@ModuleAddr,@AType,@Content,now(),now(),@ErrCode,@ErrTxt)");
            //    return SQLHelper.Execute(strSql.ToString(), params01);
            //}
            //else
            //{
            //    strSql.Append("update v2_alarm_log set Content=@Content,ErrCode=@ErrCode,ErrTxt=@ErrTxt where Log_id=@Log_id");
            //    return SQLHelper.Execute(strSql.ToString(), params01);
            //}
            return 0;
        }


        /// <summary>
        /// 拉闸数据
        /// </summary>
        /// <returns></returns>
        public CommandVModel GetSsr1(int module_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into v2_command(");
            strSql.Append("CDate,Ledger,Co_id,Module_id,ModuleAddr,Fun_id,FunType,Action,DataValue,Create_by,Create_dt,ErrCode,IsRn)");
            strSql.Append("select now(),a.Ledger,a.Co_id,a.Module_id,a.ModuleAddr,a.Fun_id,a.FunType,a.Action,1,@SysUid,now(),0,1");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and a.FunType=@FunType;");
            strSql.Append(" select * from vp_cmdinfo where  Log_id = last_insert_id();");
            DataTable dtSource = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, FunType = V0Fun.Ssr.ToString(), SysUid = this.SysUid });
            CommandVModel cmd = null;
            if (dtSource.Rows.Count > 0)
                cmd = ModelHandler<CommandVModel>.FillModel(dtSource.Rows[0]);
            return cmd;
        }

        /// <summary>
        /// 拉闸数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetSsr1_Bk(int module_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Ledger,a.Esp_id,a.ComPort,a.Baud,a.DataBit,a.Parity,a.StopBit,");
            strSql.Append("a.Ip,a.TcpPort,a.TimeOut,a.HandledBY,a.ModuleType,a.ModulePwd,a.ModuleUid,a.TransferType,");
            strSql.Append("a.Module_id,a.ModuleAddr,a.Fun_id,a.FunName,a.FunType,a.Protocol,a.Co_id,a.Action,case when a.TransferType=4 then a.TagName else a.LpszDbVarName end as LpszDbVarName");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" inner join vp_mdinfo as md on a.Ledger=md.Ledger and a.Module_id=md.Module_id");
            strSql.Append(" inner join v1_gateway_esp_module_info as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and a.FunType=@Ssr");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, Ssr = V0Fun.Ssr.ToString() });
        }

        public string GetSSrIOService(int module_id, string funType, out string dataValue)
        {
            dataValue = "";
            object params01 = new
            {
                Ledger = this.Ledger,
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
        public int AddAV2_alarm_log_hand(long log_id, int aid, string hdType, string hdKey, string content, int errCode, string errTxt)
        {
            StringBuilder strSql = new StringBuilder();
            if (aid == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Aid) from v2_alarm_log_hand where Log_id=@Log_id");
                object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Log_id = log_id });
                aid = CommFunc.ConvertDBNullToInt32(obj) + 1;
            }
            object params01 = new { Ledger = this.Ledger, Log_id = log_id, Aid = aid, HdType = hdType, HdKey = hdKey, Content = content, ErrCode = errCode, ErrTxt = errTxt, SysUid = this.SysUid };
            strSql.Clear();
            strSql.Append("insert into v2_alarm_log_hand(Log_id,Aid,HdType,HdKey,Content,Create_by,Create_dt,ErrCode,ErrTxt,HdTime)");
            strSql.Append("values(@Log_id,@Aid,@HdType,@HdKey,@Content,@SysUid,now(),@ErrCode,@ErrTxt,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE Content=@Content,ErrCode=@ErrCode,HdTime=now()");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }
    }
}
