using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;
using YDS6000.DAL;


namespace YDS6000.DAL.Exp.RunReport
{
    public partial class ExpYdMotionDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        private string Project = "";
        public ExpYdMotionDAL(string Project, int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            this.Project = Project;
        }

        public DataTable GetYdMontionOnList(string CoStrcName, string CoName)
        {
            if (string.IsNullOrEmpty(CoStrcName) || CoStrcName == "{StrcName}" || CoStrcName == "null")
                CoStrcName = string.Empty;
            if (string.IsNullOrEmpty(CoName) || CoName == "{CoName}" || CoName == "null")
                CoName = string.Empty;

            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);
            StringBuilder strSql = new StringBuilder();
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
            strSql.Append(" order by a.Module_id,vf.OrdNo,vf.Fun_id");

            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, AreaPowerStr = AreaPowerStr, CoStrcName = "%" + CoStrcName + "%", CoName = "%" + CoName + "%", SplitException = WHoleDAL.splitException, MdItems = WHoleDAL.MdItems });
        }

        public DataTable GetYdMontionOnList(int module_id,string funType)
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
        /// 获取项目信息
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
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = (int)attrib, AreaPowerStr = AreaPowerStr });
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
    }
}
