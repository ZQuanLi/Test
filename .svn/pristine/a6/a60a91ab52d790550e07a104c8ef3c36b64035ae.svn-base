using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Exp.Alarm
{
    partial class AlarmDAL
    {        
        /// <summary>
        /// 采集异常告警
        /// </summary>
        /// <param name="CoStrcName"></param>
        /// <param name="CoName"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        public DataTable GetYdAlarmOfUnusualList(string CoStrcName, string CoName, DateTime Start, DateTime End)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);

            object params01 = new {
                Ledger=this.Ledger,          
                AreaPowerStr=AreaPowerStr,
                CoStrcName="%"+CoStrcName+"%",
                CoName="%"+CoName+"%",
                Start=Start.ToString("yyyy-MM-dd"),
                End=End.ToString("yyyy-MM-dd"),
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT a.Log_id,b.CoStrcName,b.CoName,a.ModuleAddr,d.ModuleName,a.AType,a.Content,a.CollectTime,");
            strSql.Append("CASE WHEN a.ErrCode = '1' THEN '是' else '否' END as ErrCode,c.Co_id,c.Module_id,c.Fun_id,c.FirstVal,c.LastVal,c.LastTime");
            strSql.Append(" FROM v2_alarm_log as a");
            strSql.Append(" inner JOIN vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" inner JOIN v2_info as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id and a.Module_id=c.Module_id and a.ModuleAddr=c.ModuleAddr and a.Fun_id=c.Fun_id");
            strSql.Append(" inner join vp_mdinfo as d on a.Ledger=d.Ledger and a.Module_id=d.Module_id and a.ModuleAddr=d.ModuleAddr and a.Co_id=d.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.CDate>=@Start and a.CDate<=@End and a.AType like 'Ez_%'");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            if (!string.IsNullOrEmpty(CoStrcName))
                strSql.Append(" and ifnull(b.CoStrcName,'') like @CoStrcName ");
            if (!string.IsNullOrEmpty(CoName))
                strSql.Append(" and ifnull(b.CoName,'') like @CoName ");
            strSql.Append(" GROUP BY a.Log_id ORDER BY a.Log_id DESC");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        public int SetV2_infoInVal(int Log_id, int Co_id, int Module_id, string ModuleAddr, int Fun_id, decimal FirstVal, decimal LastVal)
        {
            object params01 = new {
                Ledger=this.Ledger,
                Log_id=Log_id, 
                Co_id=Co_id, 
                Module_id=Module_id, 
                Fun_id=Fun_id, 
                ModuleAddr=ModuleAddr,              
                LastVal=LastVal,
                FirstVal=FirstVal,
                SysUid=this.SysUid, 
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update v2_info as a ,v2_alarm_log as b  set ");
            strSql.Append("a.YT_id=a.YT_id+1,");
            strSql.Append("a.LastVal=@LastVal,");
            strSql.Append("a.FirstVal=@FirstVal, ");
            strSql.Append("b.ErrCode=1, ");
            strSql.Append("b.Appr_by=@SysUid,");
            strSql.Append("b.Appr_dt=now()");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id = @Co_id and a.Module_id=@Module_id and a.ModuleAddr = @ModuleAddr and a.Fun_id=@Fun_id and b.Log_id=@Log_id and  b.Ledger = @Ledger and b.ModuleAddr = @ModuleAddr and b.Co_id = @Co_id ");

            return SQLHelper.Execute(strSql.ToString(), params01);
        }

        public DataTable GetTabVal(int Co_id, string ModuleAddr)
        {
            object params01 = new {
                Ledger=this.Ledger,
                Co_id=Co_id,
                ModuleAddr=ModuleAddr
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT a.Log_id,b.CoStrcName,b.CoName,a.ModuleAddr,a.AType,a.Content,a.CollectTime,CASE WHEN a.ErrCode = '0' THEN '否' else '是' END as ErrCode,c.Co_id,c.Module_id,c.Fun_id,c.FirstVal,c.LastVal,c.LastTime");
            strSql.Append(" FROM v2_alarm_log as a");
            strSql.Append(" LEFT JOIN vp_coinfo as b on a.Ledger=b.Ledger and  a.Co_id=b.Co_id ");
            strSql.Append(" LEFT JOIN v2_info as c on a.ledger=c.ledger and a.Co_id=c.Co_id and a.Module_id=c.Module_id and a.ModuleAddr=c.ModuleAddr");
            strSql.Append(" where a.Ledger=@Ledger and a.AType like 'Ez_%' and c.Co_id=@Co_id");
            if (!string.IsNullOrEmpty(ModuleAddr))
                strSql.Append(" and a.ModuleAddr=@ModuleAddr ");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        public int SetV2_infoInTime(int Log_id, int Co_id, int Module_id, string ModuleAddr, int Fun_id, DateTime LastTime)
        {
            object params01 = new {
                Ledger=this.Ledger,
                Log_id=Log_id, 
                Co_id=Co_id, 
                Module_id=Module_id, 
                Fun_id=Fun_id, 
                ModuleAddr=ModuleAddr,
                LastTime=LastTime, 
                SysUid=this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update v2_info as a ,v2_alarm_log as b set ");
            strSql.Append("a.YT_id=a.YT_id+1,");
            strSql.Append("a.LastTime=@LastTime,");
            strSql.Append("b.ErrCode=1, ");
            strSql.Append("b.Appr_by=@SysUid ,");
            strSql.Append("b.Appr_dt=now()");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id = @Co_id and a.Module_id=@Module_id and a.ModuleAddr = @ModuleAddr and a.Fun_id=@Fun_id and b.Log_id=@Log_id and  b.Ledger = @Ledger and b.ModuleAddr = @ModuleAddr and b.Co_id = @Co_id ");

            return SQLHelper.Execute(strSql.ToString(), params01);
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
            strSql.Append("b.CoStrcName,b.CoName,m1.ModuleName,a.Appr_by,vf.FunName as CollectName");
            strSql.Append(" FROM v2_alarm_log as a");
            strSql.Append(" inner JOIN vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" inner join vp_mdinfo as m1 on a.Ledger=m1.Ledger and a.Module_id=m1.Module_id and a.ModuleAddr=m1.ModuleAddr and a.Co_id=m1.Co_id");
            strSql.Append(" left join v0_fun as vf on a.Ledger=vf.Ledger and a.Fun_id=vf.Fun_id");
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
        /// 忽略此隐患
        /// </summary>
        /// <param name="log_id">隐患编号</param>
        /// <returns></returns>
        public int IgnoreAlarm(long log_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v2_alarm_log set Status=1 where Log_id=@Log_id");
            return SQLHelper.Execute(strSql.ToString(), new { Log_id = log_id });
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdUserList(int uid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Uid,a.UName,a.TelNo,a.Role_id,b.Descr as RoleName");
            strSql.Append(" from vp_userinfo as a left join sys_role as b on a.Ledger = b.Ledger and a.Role_id = b.Role_id ");
            strSql.Append(" where a.Ledger=@Ledger");
            if (uid == 0)
                strSql.Append(" and a.Disabled=0");
            else
                strSql.Append(" and a.Uid=@Uid");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Uid = uid });
        }
        /// <summary>
        /// 派工单
        /// </summary>
        /// <param name="log_id">隐患编号</param>
        /// <param name="uid">指派人员ID</param>
        /// <returns></returns>
        public int JobOrder(long log_id, int uid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(*) from v2_alarm_log where Log_id=@Log_id and ifnull(Status,0)!=0");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Log_id = log_id });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0) throw new Exception("此单已处理不能继续派工单");

            strSql.Clear();
            strSql.Append("insert into v2_solution_log(Ledger,CDate,SolutionNo,Src_id,SrcType,SolutionBy,SolutionTime,Status,Create_by,Create_dt)values");
            strSql.Append("(@Ledger,now(),'',@Src_id,0,@Uid,now(),0,@SysUid,now());");
            strSql.Append("update v2_alarm_log set Status=2,Appr_by=@Uid,Appr_dt=now() where Log_id=@Src_id;");
            return SQLHelper.Execute(strSql.ToString(), new { Src_id = log_id, Uid = uid, Ledger = this.Ledger, SysUid = this.SysUid });
        }

        /// <summary>
        /// 工单列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetSolutionList(string CoStrcName, string CoName, int status, DateTime Start, DateTime End)
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
                SysUid = this.SysUid,
            };

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Log_id,a.Src_id,a.SolutionNo,a.Status,b.Create_dt,a.SolutionTime,b.Co_id,b.Module_id,b.ModuleAddr,b.Fun_id,b.FunType,b.FunName,a.SolutionImage,a.Content,b.CollectValue,b.CollectTime,b.Appr_dt,");
            strSql.Append("b.Create_dt as AlarmTime,c1.CoStrcName,c1.CoName,m1.ModuleName,u1.UName as SolutionName,a.SolutionImage");
            strSql.Append(" from v2_solution_log as a inner join v2_alarm_log as b on a.Src_id=b.Log_id");
            strSql.Append(" inner JOIN vp_coinfo as c1 on b.Ledger=c1.Ledger and b.Co_id=c1.Co_id");
            strSql.Append(" inner join vp_mdinfo as m1 on b.Ledger=m1.Ledger and b.Module_id=m1.Module_id and b.ModuleAddr=m1.ModuleAddr and b.Co_id=m1.Co_id");
            strSql.Append(" left join vp_userinfo as u1 on a.Ledger=u1.Ledger and a.SolutionBy=u1.Uid");
            strSql.Append(" where b.Ledger=@Ledger");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(b.Co_id,@AreaPowerStr)");
            if (!"{strcName}".Equals(CoStrcName)) 
                strSql.Append(" and ifnull(c1.CoStrcName,'') like @CoStrcName ");
            if (!"{coName}".Equals(CoName)) 
                strSql.Append(" and ifnull(c1.CoName,'') like @CoName ");
            if (status >= 0)
                strSql.Append(" and a.Status=@Status");
            if (SysUid!=1) //管理员可以查看全部
                strSql.Append(" and a.SolutionBy=@SysUid");
            strSql.Append(" ORDER BY a.Log_id DESC");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        /// <summary>
        /// 工单状态修改
        /// </summary>
        /// <param name="log_id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int SolutionStatus(long log_id, int status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v2_solution_log set Status=@Status,SolutionTime=@SolutionTime where Log_id=@Log_id");
            return SQLHelper.Execute(strSql.ToString(), new { Log_id = log_id, Status = status });
        }

        public DataTable GetIdxList(DateTime startTime)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Co_id,a.CoName as CellName,b.CoName as ProjectName,c.CoName as UnitName,d.CoName as AreaName,ix.Idx");
            strSql.Append(" from v1_cust as a left join v1_cust as b on a.Ledger=b.Ledger and a.Parent_id=b.Co_id");
            strSql.Append(" left join v1_cust as c on b.Ledger=c.Ledger and b.Parent_id=c.Co_id");
            strSql.Append(" left join v1_cust as d on c.Ledger=d.Ledger and c.Parent_id=d.Co_id");
            strSql.Append(" left join v1_cust_idx as ix on a.Ledger=ix.Ledger and a.Co_id=ix.Co_id and ix.Month=@Month");
            strSql.Append(" where a.Ledger=@Ledger and a.Attrib=@Attrib");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            strSql.Append(" order by ix.Idx,a.Co_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = (int)CoAttribV2_1.Cell, Month = new DateTime(startTime.Year, startTime.Month, 1), AreaPowerStr = AreaPowerStr });
        }

        public int UpdateIdx(int co_id, DateTime startTime, int idx)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into v1_cust_idx(Ledger,Co_id,Month,Idx)values(@Ledger,@Co_id,@Month,@Idx)");
            strSql.Append(" ON DUPLICATE KEY UPDATE Idx=@Idx");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id, Month = new DateTime(startTime.Year, startTime.Month, 1), Idx = idx });
        }

    }
}
