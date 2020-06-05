using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.ExpApp.Home
{
    public class HomeDAL
    {
        private static string EncryptPwd = "DiLidili";
        private int Ledger = 0;
        private int SysUid = 0;
        //private string Project = "";
        public HomeDAL(int ledger, int uid)
        {
            //this.Project = project;
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetProjectList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT a.Rule ");
            strSql.Append(" FROM sys_config as a");
            strSql.Append(" WHERE a.Ledger=@Ledger and a.CfKey='App' and a.CfType='Login'");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }
        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="crmNo">证件号（登陆号码）</param>
        /// <returns></returns>
        public DataTable GetUser(string crmNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT a.UName,a.USign,a.Uid,cast(GetDECODE(a.UPasswd,@EncryptPwd) as char) UPasswd , s1.Project"); //b.Co_id,
            strSql.Append(" FROM sys_user as a");
            //strSql.Append(" inner JOIN v3_userinfo as b on a.Ledger=b.Ledger and a.Crm_id=b.Crm_id");
            strSql.Append(" inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" WHERE a.Ledger=@Ledger and a.USign=@CrmNo");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, CrmNo = crmNo, EncryptPwd = WHoleDAL.EncryptPwd });
        }
        /// <summary>
        /// 获取app客户信息
        /// </summary>
        /// <param name="crmNo">证件号（登陆号码）</param>
        /// <returns></returns>
        public DataTable GetV3_User(string crmNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT a.CrmName,a.CrmNo,a.Crm_id,cast(GetDECODE(a.Passwd,@EncryptPwd) as char) Passwd ,a.Contract , s1.Project"); //b.Co_id,
            strSql.Append(" FROM v3_user as a");
            //strSql.Append(" inner JOIN v3_userinfo as b on a.Ledger=b.Ledger and a.Crm_id=b.Crm_id");
            strSql.Append(" inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" WHERE a.Ledger=@Ledger and a.CrmNo=@CrmNo");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, CrmNo = crmNo, EncryptPwd = WHoleDAL.EncryptPwd });
        }

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="crm_id">证件号（登陆号码）</param>
        /// <returns></returns>
        public DataTable GetV3_User(int crm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT a.CrmName,a.CrmNo,a.Crm_id,cast(GetDECODE(a.Passwd,@EncryptPwd) as char) Passwd ,a.Contract , s1.Project,b.Co_id,c.CoFullName,d.ModuleName,d.MeterNo");
            strSql.Append(" FROM v3_user as a");
            strSql.Append(" inner JOIN v3_userinfo as b on a.Ledger=b.Ledger and a.Crm_id=b.Crm_id");
            strSql.Append(" inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" left join vp_coinfo as c on b.Ledger=c.Ledger and b.Co_id=c.Co_id");
            strSql.Append(" left join vp_mdinfo as d on b.Ledger=d.Ledger and b.Co_id=d.Co_id");
            strSql.Append(" WHERE a.Ledger=@Ledger and a.Crm_id=@Crm_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = crm_id, EncryptPwd = WHoleDAL.EncryptPwd });
        }
        /// <summary>
        /// 根据客户获取电表信息
        /// </summary>
        /// <param name="crm_id"></param>
        /// <returns></returns>
        public DataTable GetV3_UserOfModule(int crm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT d.Module_id,d.ModuleAddr,case when ifnull(vm.Price,0)=0 then s1.Price else vm.Price end as Price");
            strSql.Append(" from v3_user as a");
            strSql.Append(" inner join v3_userinfo as b on a.Ledger=b.Ledger and a.Crm_id=b.Crm_id");
            strSql.Append(" inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" inner join vp_coinfo as c on b.Ledger=c.Ledger and b.Co_id=c.Co_id");
            strSql.Append(" inner join vp_mdinfo as d on b.Ledger=d.Ledger and b.Co_id=d.Co_id");
            strSql.Append(" left join v1_gateway_esp_module_info as vm on d.Ledger=vm.Ledger and d.Module_id=vm.Module_id");
            strSql.Append(" WHERE a.Ledger=@Ledger and a.Crm_id=@Crm_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = crm_id });
        }

        /// <summary>
        /// 查找快速购电信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetPayQuick(string code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT d.Module_id,d.ModuleAddr,d.ModuleName,d.Co_id,d.Fun_id,d.FunType,d.Multiply,d.Scale,case when ifnull(vm.Price,0)=0 then s1.Price else vm.Price end as Price,");
            strSql.Append("v2.LastVal,v2.RdVal,c.CoFullName,a.Contract,a.CrmName");
            strSql.Append(" from v3_user as a");
            strSql.Append(" inner join v3_userinfo as b on a.Ledger=b.Ledger and a.Crm_id=b.Crm_id");
            strSql.Append(" inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" inner join vp_coinfo as c on b.Ledger=c.Ledger and b.Co_id=c.Co_id");
            strSql.Append(" inner join vp_funinfo as d on b.Ledger=d.Ledger and b.Co_id=d.Co_id and d.FunType='E'");
            strSql.Append(" left join v1_gateway_esp_module_info as vm on d.Ledger=vm.Ledger and d.Module_id=vm.Module_id");
            strSql.Append(" left join vp_v2info as v2 on d.Ledger=v2.Ledger and d.Co_id=v2.Co_id and d.Module_id=v2.Module_id and d.ModuleAddr=v2.ModuleAddr and d.Fun_id=v2.Fun_id");
            strSql.Append(" WHERE a.Ledger=@Ledger and a.Contract=@Code");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Code = code });
        }
        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="UName"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        public DataTable GetYdSysLogOfCmd(string UName, DateTime Start, DateTime End)
        {
            if (string.IsNullOrEmpty(UName) || UName == "{UName}" || UName == "null")
                UName = string.Empty;

            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select b.UName,a.Content,a.CTime");
            strSql.Append(" from sys_log a LEFT JOIN sys_user b ON a.Uid=b.Uid");
            strSql.Append(" where a.Ledger=@Ledger and (a.CDate BETWEEN @Start AND @End)");
            //if (!string.IsNullOrWhiteSpace(UName))
            //    strSql.Append(" and IFNULL(b.UName,'') LIKE @UName");

            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, UName = "%" + UName + "%", Start = Start.ToString("yyyy-MM-dd 00:00:00"), End = End.ToString("yyyy-MM-dd 23:59:59") });
        }
        /// <summary>
        /// 控制日志
        /// </summary>
        /// <param name="UName"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        public DataTable GetYdSysLogCtrl(string UName, DateTime Start, DateTime End)
        {
            if (string.IsNullOrEmpty(UName) || UName == "{UName}" || UName == "null")
                UName = string.Empty;

            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select b.UName,case when ifnull(a.Descr,'')='' then c.FunName else a.Descr end as Descr,a.Create_dt,a.ErrCode,a.ErrTxt");
            strSql.Append(" from v2_command a LEFT JOIN sys_user b ON a.Ledger=b.Ledger and a.Create_by=b.Uid");
            strSql.Append(" left join v0_fun as c ON a.Ledger=b.Ledger and a.Fun_id=c.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and (a.CDate BETWEEN @Start AND @End)");
            //if (!string.IsNullOrWhiteSpace(UName))
            //    strSql.Append(" and IFNULL(b.UName,'') LIKE @UName");

            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, UName = "%" + UName + "%", Start = Start.ToString("yyyy-MM-dd 00:00:00"), End = End.ToString("yyyy-MM-dd 23:59:59") });
        }
        /// <summary>
        /// 历史记录
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="funType"></param>
        /// <returns></returns>
        public DataTable GetYdMontionOnList(int module_id, string funType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Parent_id,a.Co_id,a.Multiply,a.FullStruc,vf.Fun_id,c1.CoName,c1.CoStrcName,vf.Fun_id,vf.FunName,vf.FunType,vf.Scale,vf.OrdNo");
            strSql.Append(" from vp_mdinfo as a inner join vp_coinfo as c1 on a.Ledger=c1.Ledger and a.Co_id=c1.Co_id");
            strSql.Append(" inner join v0_fun as vf on a.Ledger=vf.Ledger and a.Mm_id=vf.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and FIND_IN_SET(vf.FunType,@FunType)");
            strSql.Append(" order by a.Module_id,vf.OrdNo,vf.Fun_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, FunType = funType });
        }
        /// <summary>
        /// 数据对比
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="unitId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataTable GetCompare(int areaId, int unitId, int projectId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Log_id,a.CDate from v2_alarm_log as a where a.CDate>=@CDate and a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, CDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-23) });
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
            object params01 = new
            {
                Ledger = this.Ledger,
                Module_id = module_id,
                CoStrcName = "%" + CoStrcName + "%",
                CoName = "%" + CoName + "%",
                Ssr = Ssr,
                SplitException = WHoleDAL.splitException,
                AreaPowerStr = AreaPowerStr,
                MdItems = WHoleDAL.MdItems,
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
            strSql.Append(" where a.Ledger=@Ledger and not FIND_IN_SET(a.ModuleType,@SplitException)");
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
        /// 获取用电单元信息
        /// </summary>
        /// <param name="attrib"></param>
        /// <returns></returns>
        public DataTable GetVp_coinfo(CoAttribV2_1 attrib)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Co_id,a.CoNo,a.CoName,a.CustAddr,a.Mobile,a.Parent_id,a.Remark,a.CoStrcName,a.Disabled,b.CoName as ParentName");
            strSql.Append(" from vp_coinfo as a");
            strSql.Append(" left join v1_cust as b on a.Ledger=b.Ledger and a.Parent_id=b.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Attrib=@Attrib");
            if (IsCheckAreaPower == true)
            {
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            }
            else
            {
                strSql.Append(" and FIND_IN_SET(a.Co_id,'-1')");//给-1:是无效值   为false时无权限
            }
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = (int)attrib, AreaPowerStr = AreaPowerStr, SysUid=this.SysUid });
        }

        public DataTable GetModuleCombox(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Module_id,a.ModuleName from vp_mdinfo as a where a.Ledger=@Ledger and a.Co_id=@Co_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
        }
        public DataTable GetFunCombox(int module_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select b.Fun_id,b.FunType,b.FunName");
            strSql.Append(" from vp_mdinfo as a inner join v0_fun as b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id });
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
        /// 隐患列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdAlarmList(string CoStrcName, string CoName, int status, DateTime Start, DateTime End)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            object params01 = new
            {
                Ledger = this.Ledger,
                AreaPowerStr = AreaPowerStr,
                CoStrcName = "%" + CoStrcName + "%",
                CoName = "%" + CoName + "%",
                Start = Start.ToString("yyyy-MM-dd"),
                End = End.ToString("yyyy-MM-dd"),
                Status = status,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT a.Log_id,a.Co_id,a.Module_id,a.ModuleAddr,a.Fun_id,a.FunType,a.FunName,a.Content,a.CollectTime,a.CollectValue,a.Create_dt,a.Status,");
            strSql.Append("b.CoStrcName,b.CustAddr,b.CoName,m1.ModuleName,a.Appr_by");
            strSql.Append(" FROM v2_alarm_log as a");
            strSql.Append(" inner JOIN vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" inner join vp_mdinfo as m1 on a.Ledger=m1.Ledger and a.Module_id=m1.Module_id and a.ModuleAddr=m1.ModuleAddr and a.Co_id=m1.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.CDate>=@Start and a.CDate<=@End");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            if (!string.IsNullOrEmpty(CoStrcName))
                strSql.Append(" and ifnull(b.CoStrcName,'') like @CoStrcName ");
            if (!string.IsNullOrEmpty(CoName))
                strSql.Append(" and ifnull(b.CoName,'') like @CoName ");
            if (status >= 0)
                strSql.Append(" and a.Status=@Status");
            strSql.Append(" ORDER BY a.Log_id DESC");
            return SQLHelper.Query(strSql.ToString(), params01);
        }
        /// <summary>
        /// 工单列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetSolutionList(string CoStrcName, string CoName, int status)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            object params01 = new
            {
                Ledger = this.Ledger,
                AreaPowerStr = AreaPowerStr,
                CoStrcName = "%" + CoStrcName + "%",
                CoName = "%" + CoName + "%",
                Status = status,
                SysUid = this.SysUid,
            };

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Log_id,a.Src_id,a.SolutionNo,a.Status,b.Create_dt,a.SolutionTime,b.Co_id,b.Module_id,b.ModuleAddr,b.Fun_id,b.FunType,b.FunName,a.SolutionImage,a.Content,b.CollectValue,b.CollectTime,b.Appr_dt,");
            strSql.Append("b.Create_dt as AlarmTime,c1.CoStrcName,c1.CoName,m1.ModuleName,u1.UName as SolutionName");
            strSql.Append(" from v2_solution_log as a inner join v2_alarm_log as b on a.Src_id=b.Log_id");
            strSql.Append(" inner JOIN vp_coinfo as c1 on b.Ledger=c1.Ledger and b.Co_id=c1.Co_id");
            strSql.Append(" inner join vp_mdinfo as m1 on b.Ledger=m1.Ledger and b.Module_id=m1.Module_id and b.ModuleAddr=m1.ModuleAddr ");
            strSql.Append(" left join vp_userinfo as u1 on a.Ledger=u1.Ledger and a.SolutionBy=u1.Uid");
            strSql.Append(" where b.Ledger=@Ledger");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(b.Co_id,@AreaPowerStr)");
            if (!string.IsNullOrEmpty(CoStrcName))
                strSql.Append(" and ifnull(c1.CoStrcName,'') like @CoStrcName ");
            if (!string.IsNullOrEmpty(CoName))
                strSql.Append(" and ifnull(c1.CoName,'') like @CoName ");
            if (status >= 0)
                strSql.Append(" and a.Status=@Status");
            if (SysUid != 1) //管理员可以查看全部
                strSql.Append(" and a.SolutionBy=@SysUid");
            strSql.Append(" ORDER BY a.Log_id DESC");
            return SQLHelper.Query(strSql.ToString(), params01);
        }
        /// <summary>
        /// 参数下拉框
        /// </summary>
        /// <returns></returns>
        public DataTable GetFunTypes(int module_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.Co_id,a.Fun_id,a.FunName,a.FunType,a.Scale");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" where a.Ledger = @Ledger and a.Module_id=@Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id });
        }
        /// <summary>
        /// 完成工单
        /// </summary>  
        /// <param name="Attached"></param>
        /// <returns></returns>
        public int SetPic(int logId,string Attached, string content)
        {
            DateTime SolutionTime = DateTime.Now;
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append(" update v2_solution_log set SolutionImage=@Attached,content=@content,SolutionTime=@SolutionTime,Status=2 where Log_id=@logId");
            object obj = SQLHelper.Execute(strSql.ToString(), new { logId = logId, Attached = Attached , content = content , SolutionTime = SolutionTime });
            return CommFunc.ConvertDBNullToInt32(obj);
        }

        /// <summary>
        /// 获取用户用电单元权限
        /// </summary>
        /// <param name="Area"></param>
        /// <returns></returns>
        public bool GetAreaPowerAPP(out string Area)
        {
            Area = "";
            object params01 = new
            {
                Ledger = this.Ledger,
                Uid = SysUid,
            };
            bool isPower = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select s1.role_id,s1.UserType,s2._area");
            strSql.Append(" from sys_user as s1 inner join syscont as m on s1.Ledger=m.Ledger");
            strSql.Append(" left join sys_user_prog as s2 on s1.Ledger=s2.Ledger and s1.role_id=s2.Role_Id and s2.prog_id=m.Project");
            strSql.Append(" where s1.Ledger=@Ledger and s1.Uid=@Uid");
            DataTable dt = SQLHelper.Query(strSql.ToString(), params01);
            foreach (DataRow dr in dt.Rows)
            {
                Area = CommFunc.ConvertDBNullToString(dr["_area"]);
                if (string.IsNullOrEmpty(Area))/*如果没有任何区域权限，且是超级管理员不检测此权限/*内置角色不检测区域*/
                    isPower = CommFunc.ConvertDBNullToInt32(dr["UserType"]) == 1 ? false : true;
            }
            if (string.IsNullOrEmpty(Area))
                isPower = false; //不需要区域权限这功能了，因此返回false
            return isPower;
        }
    }
}
