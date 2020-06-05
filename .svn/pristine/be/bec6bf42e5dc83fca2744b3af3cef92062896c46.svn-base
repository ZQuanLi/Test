using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;


namespace YDS6000.DAL.Exp.Monitor
{
    public partial class MonitorDAL
    {
        /// <summary>
        /// 获取全部客户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdCustOnCoInfoList()
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Co_id,a.CoNo,a.CoName,a.Disabled,a.Parent_id,a.Attrib,");
            strSql.Append("b.CustAddr,b.Office_tel,b.Mobile,b.Email,b.IsDefine,m1.CoName as PCoName");
            strSql.Append(" from v1_cust as a left join v1_custinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v1_cust as m1 on a.Ledger=m1.Ledger and a.Parent_id=m1.Co_id");
            strSql.Append(" where a.Ledger=@Ledger");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, AreaPowerStr });
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="CoStrcName"></param>
        /// <param name="CoName"></param>
        /// <param name="Ssr"></param>
        /// <returns></returns>
        public DataTable GetYdMonitorOnList(int module_id, string CoStrcName, string CoName, string Ssr, int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            if (string.IsNullOrEmpty(CoStrcName) || CoStrcName == "{strcName}" || CoStrcName == "null")
                CoStrcName = string.Empty;
            if (string.IsNullOrEmpty(CoName) || CoName == "{coName}" || CoName == "null")
                CoName = string.Empty;
            if (string.IsNullOrEmpty(Ssr) || Ssr == "{ssr}" || Ssr == "null")
                Ssr = string.Empty;
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            string ids = "";
            if (co_id != 0)
            {
                strSql.Clear();
                strSql.Append("select GetCoChildList(@Ledger,@Co_id)as ids");
                object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
                ids = CommFunc.ConvertDBNullToString(obj);
            }
            object params01 = new {
                Ledger=this.Ledger,
                Module_id=module_id,
                CoStrcName="%"+CoStrcName+"%",
                CoName="%"+CoName+"%",
                Ssr=Ssr,
                SplitException= WHoleDAL.splitException,
                AreaPowerStr=AreaPowerStr,
                MdItems= WHoleDAL.MdItems,
                Ids = ids,
            };
            strSql.Clear();
            strSql.Append("select a.Ledger,a.Module_id,a.ModuleAddr,a.ModuleName,a.Co_id,a.Multiply,b.LastVal as CurVal,");
            strSql.Append("round(b.RdVal * a.Multiply,b.Scale)as RdVal,round(b.SyZsVal * a.Multiply,b.Scale)as SyZsVal,");
            strSql.Append("round(b.LastVal * a.Multiply,b.Scale)as LastVal,a.ModuleType,");
            strSql.Append("b.LastTime,c1.CoName,c1.CoStrcName,''as Ssr,s1.FrMd,b.MemcachKey,a.HandledBY,");
            strSql.Append("case when TIMESTAMPDIFF(MINUTE,b.LastTime,NOW()) < s1.FrMd then 1 else 0 end as `OnLine`,mp.IsAlarm,");
            strSql.Append("case when ifnull(mp.Price,0)=0 then s1.Price else mp.Price end as Price,fp.LpszDbVarName as CachComm");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" left join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and a.Mm_id=b.Mm_id and b.FunType='E' and b.Hist_id=0");
            strSql.Append(" left join vp_coinfo as c1 on a.Ledger=c1.Ledger and a.Co_id=c1.Co_id");
            strSql.Append(" left join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" left join v1_gateway_esp_module_info as mp on a.Ledger=mp.Ledger and a.Module_id=mp.Module_id");
            strSql.Append(" left join vp_funinfo as fp on a.Ledger=fp.Ledger and a.Module_id=fp.Module_id and a.Mm_id=fp.Mm_id and fp.FunType='Comm' and fp.Disabled=0");/*在线状态*/
            strSql.Append(" where a.Ledger=@Ledger and not FIND_IN_SET(a.ModuleType,@SplitException) and a.IsDefine = 100");
            if (module_id != 0)
                strSql.Append(" and a.Module_id=@Module_id");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            if (!string.IsNullOrEmpty(WHoleDAL.MdItems))
                strSql.Append(" and FIND_IN_SET(a.IsDefine,@MdItems)");
            //strSql.Append(string.Format(" and a.IsDefine in ({0})", WHoleDAL.MdItems));            
            strSql.Append(" and ifnull(c1.CoStrcName,'') like @CoStrcName ");
            strSql.Append(" and ifnull(c1.CoName,'') like @CoName ");
            if (!string.IsNullOrEmpty(ids))
                strSql.Append(" and FIND_IN_SET(a.Co_id,@Ids)");
            return SQLHelper.Query(strSql.ToString(), params01);
        }
        /// <summary>
        /// 日用电量
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Module_id"></param>
        /// <param name="ModuleAddr"></param>
        /// <returns></returns>
        public decimal GetYdMonitorOnDayUseVal(int Co_id, int Module_id, string ModuleAddr)
        {
            DateTime TagTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            decimal useVal = 0;
            DataTable dtUse = WHoleDAL.GetCoreQueryData(this.Ledger, Module_id.ToString(), TagTime, TagTime, "day", "E");
            foreach (DataRow dr in dtUse.Rows)
            {
                if (CommFunc.ConvertDBNullToInt32(dr["Co_id"]) == Co_id)
                    useVal = useVal + ((CommFunc.ConvertDBNullToDecimal(dr["LastVal"]) - CommFunc.ConvertDBNullToDecimal(dr["FirstVal"])) * CommFunc.ConvertDBNullToDecimal(dr["Multiply"]));
            }
            return useVal;
        }
        /// <summary>
        /// 月用电量
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Module_id"></param>
        /// <param name="ModuleAddr"></param>
        /// <returns></returns>
        public decimal GetYdMonitorOnMthUseVal(int Co_id, int Module_id, string ModuleAddr)
        {
            DateTime TagTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            decimal useVal = 0;
            DataTable dtUse = WHoleDAL.GetCoreQueryData(this.Ledger, Module_id.ToString(), TagTime, TagTime, "month", "E");
            foreach (DataRow dr in dtUse.Rows)
            {
                if (CommFunc.ConvertDBNullToInt32(dr["Co_id"]) == Co_id)
                    useVal = useVal + ((CommFunc.ConvertDBNullToDecimal(dr["LastVal"]) - CommFunc.ConvertDBNullToDecimal(dr["FirstVal"])) * CommFunc.ConvertDBNullToDecimal(dr["Multiply"]));
            }
            return useVal;
        }

        public DataTable GetYdMonitorOnEzInfo(int Module_id)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Module_id = Module_id,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select vf.Fun_id,vf.Scale,a.Multiply from vp_mdinfo as a");
            strSql.Append(" inner join v0_fun as vf on a.Ledger=vf.Ledger and a.Mm_id=vf.Mm_id and vf.FunType='E'");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        public DataTable GetYdMonitorInDetailOnBill(int Co_id, DateTime dtFm, DateTime dtTo)
        {
            object params01 = new 
            {
               Ledger=this.Ledger,
               Co_id=Co_id,
               MthFm=dtFm.ToString("yyyy-MM-01",System.Globalization.DateTimeFormatInfo.InvariantInfo),
               MthTo=dtTo.ToString("yyyy-MM-01",System.Globalization.DateTimeFormatInfo.InvariantInfo),
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Ledger,a.Co_id,b1.PayVal,b1.UseVal,DATE_FORMAT(FROM_UNIXTIME(b2.Month),'%Y%m')as Month,b2.MthPayVal,b2.MthUseVal");
            strSql.Append(" from vp_coinfo as a");
            strSql.Append(" inner join v4_bill as b1 on a.Ledger=b1.Ledger and a.Co_id=b1.Co_id");
            strSql.Append(" inner join v4_billmonth as b2 on a.Ledger=b2.Ledger and a.Co_id=b2.Co_id and b2.Month>=UNIX_TIMESTAMP(@MthFm) and b2.Month<=UNIX_TIMESTAMP(@MthTo)");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id");
            return SQLHelper.Query(strSql.ToString(), params01);
        }


        /// <summary>
        /// 根据组织获取设备列表
        /// </summary>
        /// <param name="CoStrcName"></param>
        /// <param name="CoName"></param>
        /// <returns></returns>
        public DataTable GetModuleListByOrg(string CoStrcName, string CoName, params string[] funTypeParams)
        {
            object params01 = new {
                Ledger=this.Ledger,
                CoStrcName="%"+CoStrcName+"%",
                CoName="%"+CoName+"%",
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select c.Co_id,c.CoName,c.CoFullName,c.CoStrcName,a.ModuleAddr,a.Module_id,b.Fun_id,b.FunType,d.TagName,b.Scale from vp_mdinfo a  INNER JOIN  v0_fun b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id and a.MdDisabled=0 and b.Disabled=0 INNER JOIN vp_coinfo c on  a.Ledger=c.Ledger and a.Co_id=c.Co_id and c.Disabled=0 left JOIN v1_map d on a.Ledger=d.Ledger and a.Module_id=d.Module_id and b.Fun_id=d.Fun_id and b.Disabled=0 ");
            strSql.Append(" where a.Ledger=@Ledger ");
            if (funTypeParams != null && funTypeParams.Length > 0)
            {
                strSql.Append(" and ( ");
                for (int i = 0; i < funTypeParams.Length; i++)
                {
                    if (i > 0) strSql.Append(" or ");
                    strSql.Append(" b.FunType='" + funTypeParams[i] + "'");
                }
                strSql.Append(" ) ");
            }

            strSql.Append(" and ifnull(c.CoStrcName,'') like @CoStrcName ");
            strSql.Append(" and ifnull(c.CoName,'') like @CoName ");
            strSql.Append(" order by a.Module_id,b.Fun_id");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        public DataTable GetTemperatureAndHumidity(string CoStrcName, string CoName)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);

            object params01 = new {
                Ledger=this.Ledger,
                AreaPowerStr=AreaPowerStr,
                CoStrcName="%"+CoStrcName+"%",
                CoName="%"+CoName+"%",
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select c.Co_id,c.CoName,c.CoFullName,c.CoStrcName,a.ModuleAddr,a.Module_id,CONCAT(a.Module_id,'.',a.ModuleAddr,'.')as LpszDbVar,");
            strSql.Append("max(case when b.FunType like 'Leak%' then b.Scale else 0 end)as LeakScale,");
            strSql.Append("max(case when b.FunType like 'Temp%' then b.Scale else 0 end)as TempScale");
            strSql.Append(" from vp_mdinfo a INNER JOIN v0_fun b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id and (b.FunType like 'Leak%' or b.FunType like 'Temp%')");
            strSql.Append(" left JOIN vp_coinfo c on  a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.ModuleType='YDH20D'");
            strSql.Append(" and ifnull(c.CoStrcName,'') like @CoStrcName ");
            strSql.Append(" and ifnull(c.CoName,'') like @CoName ");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            strSql.Append(" group by a.Module_id");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        /// <summary>
        /// 下发命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public long YdModuleOnAddCmd(int module_id, int fun_id, string funType, string dataValue)
        {
            object params01 = new {
                Ledger=this.Ledger,
                Module_id=module_id,
                Fun_id=fun_id,
                FunType=funType,
                DataValue=dataValue,
                SysUid=this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into v2_command(");
            strSql.Append("CDate,Ledger,Co_id,Module_id,ModuleAddr,Fun_id,FunType,DataValue,Create_by,Create_dt,ErrCode)");
            strSql.Append("select now(),a.Ledger,a.Co_id,a.Module_id,a.ModuleAddr,@Fun_id,@FunType,@DataValue,@SysUid,now(),0");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id;");
            strSql.Append("select last_insert_id();");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            return CommFunc.ConvertDBNullToLong(obj);
        }

        /// <summary>
        /// 控制列表
        /// </summary>
        /// <param name="module_id"></param>
        /// <returns></returns>
        public DataTable GetCtrlList(int module_id)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Module_id = module_id,
                SysUid = this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Fun_id,a.FunType,a.FunName,a.Alias,b.TagName,b.DataValue,c.LpszDbVarName");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" inner join v1_map as b on a.Ledger=b.Ledger and a.Fun_id=b.Fun_id");
            strSql.Append(" left join vp_funinfo as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id and a.Alias=c.FunType");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and a.Action=1");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id });
        }


        /// <summary>
        /// 获取命令
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        public CommandVModel GetYdModuleOnSendCmd(long log_id)
        {
            object params01 = new 
            {
               Log_id= log_id,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from vp_cmdinfo where Log_id=@Log_id");
            DataTable dt = SQLHelper.Query(strSql.ToString(), params01);
            CommandVModel cmd = null;
            if (dt.Rows.Count > 0)
                cmd = ModelHandler<CommandVModel>.FillModel(dt.Rows[0]);
            return cmd;
        }

        public int SaveYdMonitorOfControl(int module_id, int isAlarm)
        {
            object params01 = new {
                Ledger=this.Ledger,
                Module_id=module_id,
                IsAlarm=isAlarm,
                SysUid=this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into v1_gateway_esp_module_info(Ledger,Module_id,IsAlarm,Update_by,Update_dt)values(@Ledger,@Module_id,@IsAlarm,@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE IsAlarm=@IsAlarm,Update_by=@SysUid,Update_dt=now()");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }
        ////查询mm_id值
        public DataTable GetYdMonitorOfGetIsRelay(int Module_id)
        {
            object params01 = new {
                Ledger=this.Ledger,
                Module_id=Module_id,
                FunType=V0Fun.IsRelay.ToString(),
                IsRelay = V0Fun.IsRelay.ToString(),
                IsPaul = V0Fun.IsPaul.ToString(),
            };
            StringBuilder strSql = new StringBuilder();
            //strSql.Clear();
            //strSql.Append("select b.`Status`,b.DataValue");
            //strSql.Append(" from vp_mdinfo as a inner join v1_map as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            //strSql.Append(" inner join v0_fun as c on b.Ledger=c.Ledger and b.Fun_id=c.Fun_id");
            //strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and c.FunType=@FunType");

            strSql.Clear();
            strSql.Append("select c.`Status`,c.DataValue ,d.LpszDbVarName as IsPaulKey, v1.LastVal as IsPaul");
            strSql.Append(" from vp_mdinfo as a left join vp_funinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and b.FunType=@IsRelay");
            strSql.Append(" left join v1_map as c on b.Ledger=c.Ledger and b.Module_id=c.Module_id and b.Fun_id=c.Fun_id");
            strSql.Append(" left join vp_funinfo as d on a.Ledger=d.Ledger and a.Module_id=d.Module_id and d.FunType=@IsPaul");
            strSql.Append(" left join v2_info as v1 on d.Ledger=v1.Ledger and d.Module_id=v1.Module_id and d.ModuleAddr=v1.ModuleAddr and d.Co_id=v1.Co_id and d.Fun_id=v1.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        ////查询mm_id值
        public int? GetYdMonitorOfGetIsPaul(int Module_id)
        {
            object params01 = new {
                Ledger=this.Ledger,
                Module_id=Module_id,
                FunType=V0Fun.IsPaul.ToString(),
            };
            StringBuilder strSql = new StringBuilder();
            //strSql.Clear();
            //strSql.Append("select b.DataValue from vp_mdinfo as a inner join v1_map as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            //strSql.Append(" inner join v0_fun as c on b.Ledger=c.Ledger and b.Fun_id=c.Fun_id");
            //strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and c.FunType=@FunType and  b.`Status`=1");
            strSql.Clear();
            strSql.Append("select case when ifnull(d.DataValue,'')='' then c.LastVal else d.DataValue end as LastVal");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" left join vp_funinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and b.FunType=@FunType");
            strSql.Append(" left join vp_v2info as c on b.Ledger=c.Ledger and b.Module_id=c.Module_id and b.ModuleAddr=c.ModuleAddr and b.Co_id=c.Co_id and b.Fun_id=c.Fun_id");
            strSql.Append(" left join v1_map as d on b.Ledger=d.Ledger and b.Module_id=d.Module_id and b.Fun_id=d.Fun_id and  d.`Status`=1");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            if (obj == null) return null;
            return CommFunc.ConvertDBNullToInt32(obj);
        }

        ////查询mm_id值
        public DataTable GetMm_idBymodule_id(int Module_id)
        {
            object params01 = new {
                Ledger=this.Ledger,
                Module_id=Module_id,
                FunType=V0Fun.IsRelay.ToString(),
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  a.Mm_id,b.Fun_id,b.DataValue FROM vp_mdinfo  as a ");
            strSql.Append(" INNER JOIN v0_fun as b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id ");
            strSql.Append("WHERE a.Ledger=@Ledger and a.Module_id=@Module_id and b.FunType=@FunType");
            return SQLHelper.Query(strSql.ToString(), params01);
        }
        //保存继电器状态
        public int SaveIsRelayInfo(int module_id, int fun_id, string dataValue)
        {
            //object params01 = new {
            //    Ledger=this.Ledger,
            //    Mm_id=Mm_id,              
            //    FunType=FunType,
            //    DataValue=DataValue,
            //};
            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("UPDATE v0_fun set DataValue=@DataValue WHERE Ledger=@Ledger and Mm_id=@Mm_id and FunType=@FunType;");

            object params01 = new 
            {
                Ledger=this.Ledger,
                Module_id=module_id,
                Fun_id=fun_id,
                DataValue=dataValue,
                SysUid=this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into v1_map(Ledger,Module_id,Fun_id,TagName,DataValue,Status,Rmk,Disabled,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,@Module_id,@Fun_id,'',@DataValue,0,'',0,@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE Status=0,DataValue=@DataValue,Update_by=@SysUid,Update_dt=now()");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }

        /// <summary>
        /// 获取定时断送电策略列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdMonitorOfSsr()
        {
            object params01 = new 
            {
                Ledger=this.Ledger,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT Si_id,Descr FROM v1_si_ssr ");
            strSql.Append(" WHERE Ledger =@Ledger ;");
            return SQLHelper.Query(strSql.ToString(), params01);
        }


        /// <summary>
        /// 获取电表的定时断送电策略
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdMonitorOfM_Ssr(int Module_id)
        {
            int Si_id = 0;
            object params01 = new 
            {
                Ledger=this.Ledger,
                Module_id=Module_id,
                Si_id=Si_id,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT Si_id FROM v1_gateway_esp_module_info where Ledger=@Ledger and Module_id=@Module_id ;");
            var obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            params01 = new 
            {
                Ledger=this.Ledger,
                Module_id=Module_id,
                Si_id=CommFunc.ConvertDBNullToInt32(obj),
            };
            strSql.Clear();
            strSql.Append("SELECT * FROM v1_si_ssr ");
            strSql.Append(" WHERE Ledger =@Ledger ");
            strSql.Append(" and Si_id=@Si_id ;");
            //if (CommFunc.ConvertDBNullToInt32(obj) != 0)
            //strSql.Append(" and si_id=@si_id ;");
            return SQLHelper.Query(strSql.ToString(), params01);
        }
        /// <summary>
        /// 设置定时断送电策略
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="Si_id"></param>
        /// <returns></returns>
        public int SetYdMonitorOfSsr(int module_id, int Si_id)
        {
            object params01 = new 
            {
                Ledger=this.Ledger,
                Module_id=module_id,
                Si_id=Si_id,
                SysUid=this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT COUNT(Module_id) FROM v1_gateway_esp_module_info where Ledger=@Ledger and Module_id=@Module_id ;");
            var obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            if (CommFunc.ConvertDBNullToInt32(obj) == 0)
            {
                strSql.Clear();
                strSql.Append("INSERT INTO v1_gateway_esp_module_info (Ledger,Module_id,Update_by,Update_dt,Si_id)");
                strSql.Append("values(@Ledger,@Module_id,@SysUid,now(),@Si_id)");
            }
            else
            {
                strSql.Clear();
                strSql.Append("update v1_gateway_esp_module_info set Si_id=@Si_id,Update_by=@SysUid,Update_dt=now() where Ledger=@Ledger and Module_id=@Module_id");
            }
            return SQLHelper.Execute(strSql.ToString(), params01);
        }
        //判断用户是否具有批量控制权限
        public bool GetPower(int Ledger, int Role_id, string prog_id)
        {
            object params01 = new 
            {
                Ledger=Ledger,
                Role_id=Role_id,
                Prog_id=prog_id,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(a.Ledger) as cnt");
            strSql.Append(" from sys_user_prog as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Role_id=@Role_id and a.Prog_id=@Prog_id"); //'Sep01_B01_C01'
            strSql.Append(" and a._read + a._write + a._delete + a._app > 0 ");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            return CommFunc.ConvertDBNullToInt32(obj) > 0 ? true : false;
        }

        public DataTable GetViewBuildCombox()
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select b.Co_id,b.CoName from vp_coinfo as a inner join v1_cust as b on a.Ledger=b.Ledger and a.Parent_id=b.Co_id");
            //strSql.Append(" where a.Ledger=@Ledger and a.Attrib=@Attrib and a.IsDefine=1");
            //strSql.Append(" group by b.Co_id");
            strSql.Append("select a.Co_id,a.CoName from vp_coinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Attrib=@Attrib");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = CoAttribV2_1.Project, AreaPowerStr = AreaPowerStr });
        }

        public DataTable GetViewCellCombox(int parent_id)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Co_id,a.CoName from vp_coinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Parent_id=@Parent_id");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Parent_id = parent_id, AreaPowerStr = AreaPowerStr });
        }
        public DataTable GetViewList(int co_id)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Fun_id,a.FunName,a.LpszDbVarName as TagName,a.Co_id,a.Inst_loc,b.CoName,b.Path from vp_funinfo as a");
            strSql.Append(" left join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id and a.Action=0 and ifnull(a.TagName,'')!=''");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id, AreaPowerStr = AreaPowerStr });
        }
    }
}
