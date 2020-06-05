using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;
using YDS6000.DAL;


namespace YDS6000.DAL.ExpAdminApp.Home
{
    public partial class ExpAdminHomeDAL
    {
        private static string EncryptPwd = "DiLidili";
        private int Ledger = 0;
        private int SysUid = 1;
        private string Project = "";
        public ExpAdminHomeDAL(string Project, int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            this.Project = Project;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSys_user(string uSign)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT a.Ledger,a.Uid,a.USign,GetDECODE(a.UPasswd,@EncryptPwd) UPasswd ,a.UName,a.Role_id,a.Disabled,a.LoginDate,a.LoginInt,m.Project");
            strSql.Append(" FROM sys_user as a inner join syscont as m on a.Ledger=m.Ledger LEFT JOIN sys_role as b on a.Ledger=b.Ledger and a.Role_id=b.Role_id");
            strSql.Append(" WHERE a.Ledger=@Ledger and a.USign=@USign ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, USign = uSign, EncryptPwd = WHoleDAL.EncryptPwd });
        }

        public int UpdateLogin(string uSign, int num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update sys_user set LoginDate=NOW(),LoginInt=case when @Num>0 then LoginInt+1 else 0 end  WHERE Ledger=@Ledger and USign=@USign");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, USign = uSign, Num = num });
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetUser()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Role_id,a.Create_by,a.Create_dt,a.Ledger,a.Uid,a.UName,cast( GetDECODE(a.UPasswd,'" + EncryptPwd + "') as char) UPasswd,a.USign,IFNULL(a.USign,'未知') Create_byName ,b.Descr from sys_user a left JOIN sys_role  b on a.Ledger=b.Ledger and a.Role_id=b.Role_id");
            strSql.Append(" where a.Ledger=@Ledger  and a.Disabled=0 ");
            strSql.Append("and a.Uid=@Uid; ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Uid = this.SysUid });
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="Uid"></param>
        public int EditUser(sys_user sys_user)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("update sys_user set UPasswd=GetENCODE(@UPasswd,'" + EncryptPwd + "'),UName=@UName  ,Disabled=0,Create_by=@Create_by,");
            strSql.Append("Create_dt=NOW(),Update_by=@Update_by,Update_dt=NOW() where Ledger=@Ledger and Uid=@Uid;");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, UPasswd = sys_user.UPasswd, UName = sys_user.UName, Create_by = sys_user.Uid, Update_by = this.SysUid, Uid = this.SysUid });
        }

        /// <summary>
        /// 运行历史报表
        /// </summary>
        /// <param name="CoStrcName"></param>
        /// <param name="CoName"></param>
        /// <returns></returns>
        public DataTable GetYdMontionOnList(string CoStrcName, string CoName, int Co_id)
        {
            if (string.IsNullOrEmpty(CoStrcName) || CoStrcName == "{StrcName}" || CoStrcName == "null")
                CoStrcName = string.Empty;
            if (string.IsNullOrEmpty(CoName) || CoName == "{CoName}" || CoName == "null")
                CoName = string.Empty;

            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Project, this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            string ids = "";
            if (Co_id != 0)
            {
                strSql.Clear();
                strSql.Append("select GetCoChildList(@Ledger,@Co_id)as ids");
                object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id });
                ids = CommFunc.ConvertDBNullToString(obj);
            }



            strSql.Clear();
            /*先找基站*/
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Parent_id,a.Co_id,a.Multiply,a.FullStruc,vf.Fun_id,c1.CoName,c1.CoStrcName,vf.Fun_id,vf.FunName,vf.FunType,vf.Scale,vf.OrdNo");
            strSql.Append(" from vp_mdinfo as a inner join vp_coinfo as c1 on a.Ledger=c1.Ledger and a.Co_id=c1.Co_id");
            strSql.Append(" inner join v0_fun as vf on a.Ledger=vf.Ledger and a.Mm_id=vf.Mm_id and vf.FunType='E'");
            strSql.Append(" where a.Ledger=@Ledger and not FIND_IN_SET(a.ModuleType,@SplitException)");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            strSql.Append(" and ifnull(vf.Disabled,0)=0 and ifnull(c1.CoStrcName,'') like @CoStrcName ");
            strSql.Append(" and ifnull(c1.CoName,'') like @CoName ");
            if (!string.IsNullOrEmpty(WHoleDAL.MdItems))
                strSql.Append(" and FIND_IN_SET(a.IsDefine,@MdItems)");
            if (!string.IsNullOrEmpty(ids))
                strSql.Append(" and FIND_IN_SET(a.Co_id,@ids)");
            strSql.Append(" order by a.Module_id,vf.OrdNo,vf.Fun_id");

            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, AreaPowerStr = AreaPowerStr, CoStrcName = "%" + CoStrcName + "%", CoName = "%" + CoName + "%", ids = ids, SplitException = WHoleDAL.splitException, MdItems = WHoleDAL.MdItems });
        }

        /// <summary>
        /// 获取首页缴费
        /// </summary>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public DataTable GetHomePay(string StartTime, string EndTime)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Project, this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Log_id,a.CDate,a.Co_id,a.Module_id,a.ModuleAddr,c.ModuleName,b.CoStrcName,b.CoName,b.CoFullName,");
            strSql.Append("a.PayAmt,a.PayVal,a.PayType,a.IsWrong,a.IsPay,a.Trade_no,case when a.IsPay=1 then s1.CrmName else s2.UName end as Create_by,");
            strSql.Append("a.Create_dt,a.ErrCode,a.EndTime,a.ErrTxt");
            strSql.Append(" from v4_pay_log as a");
            if (string.IsNullOrEmpty(WHoleDAL.MdItems))
            {
                strSql.Append(" left join vp_mdinfo as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id and a.ModuleAddr=c.ModuleAddr");
            }
            else
            {
                strSql.Append(" inner join vp_mdinfo as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id and a.ModuleAddr=c.ModuleAddr and FIND_IN_SET(c.IsDefine,@MdItems)");
            }
            strSql.Append(" left join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v3_user as s1 on a.Ledger=s1.Ledger and a.Create_by=s1.Crm_id");
            strSql.Append(" left join sys_user as s2 on a.Ledger=s2.Ledger and a.Create_by=s2.Uid");
            strSql.Append(" where a.Ledger=@Ledger and a.CDate>=@Start and a.CDate<=@End and a.ErrCode=1");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            strSql.Append(" and a.PayType=1");/*充值*/
            strSql.Append(" order by a.Log_id desc");

            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, AreaPowerStr = AreaPowerStr, Start = StartTime, End = EndTime, MdItems = WHoleDAL.MdItems });
        }

        /// <summary>
        /// 获取首页建筑数和房间(总用户数量取数)
        /// </summary>
        /// <param name="Attrib">// 0=空,100=建筑,9000=房间</param>
        /// <returns></returns>
        public DataTable GetHomeBuilding(int Attrib)
        {
            if (string.IsNullOrEmpty(Attrib.ToString()))
                Attrib = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT * FROM v1_cust");
            strSql.Append(" where Ledger=@Ledger and Attrib=@Attrib"); // 0 = 空，5000=城市，6000=城区，7000=园区, 100=建筑，8000=楼层，9000 = 房间
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = Attrib });
        }

        /// <summary>
        /// 获取首页设备数(仪表数)
        /// </summary>
        /// <returns></returns>
        public DataTable GetHomeModule()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select * from v1_gateway_esp_meter where Ledger=@Ledger ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }



    }
}
