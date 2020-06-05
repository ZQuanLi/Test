using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Energy.Home
{
    public partial class HomeDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;

        public HomeDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetProjectInfo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Co_id,a.Number,a.CoName,a.Disabled,b.Area,a.CustAddr,a.CustName,a.Mobile,a.Remark");
            strSql.Append(" from vp_coinfo as a left join v1_custinfobuild as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@ledger and a.Attrib =@Attrib");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = (int)CoAttrib.Project });
        }

        public DataTable GetAlarmInfo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Co_id,a.Module_id,a.ModuleAddr,a.Content,a.CollectTime,a.Total,b.ModuleName,c.CoName");
            strSql.Append(" from v2_alarm_log as a left join vp_mdinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id");
            strSql.Append(" left join vp_coinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" where a.CDate = @CDate and a.Ledger=@Ledger and a.AType=@AType");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, CDate = DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo), AType = AlarmType.Err_Tx.ToString() });
        }

        public DataTable GetMdList(int isDefine = - 999)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.Fun_id,a.FunType,a.Co_id,a.EnergyItemCode,a.IsDefine,a.Scale,a.Multiply,a.Unit,b.EnergyItemName,c.CoName");
            strSql.Append(" from vp_funinfo as a"); 
            strSql.Append(" left join v0_energyitemdict as b on a.EnergyItemCode=b.EnergyItemCode");
            strSql.Append(" left join vp_coinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");       
            strSql.Append(" where a.Ledger=@ledger and a.Parent_id=0 and a.IsChrg=1");
            if (isDefine >= 0)
                strSql.Append(" and a.IsDefine = @IsDefine");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, IsDefine = isDefine });
        }

        public DataTable GetDayForBuild(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.Fun_id,a.FunType,a.Co_id,a.EnergyItemCode,a.IsDefine,a.Scale,a.Multiply,a.Unit,b.EnergyItemName,c.CoName");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" left join v0_energyitemdict as b on a.EnergyItemCode=b.EnergyItemCode");
            strSql.Append(" left join vp_coinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" where a.Ledger=@ledger and a.Co_id=@Co_id and a.Parent_id=0 and a.IsChrg=1");
           strSql.Append(" and a.IsDefine = 0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
        }

        

        public DataTable GetDayForEnergy(int isDefine)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.Fun_id,a.FunType,a.Co_id,a.EnergyItemCode,a.IsDefine,a.Scale,a.Multiply,a.Unit,b.EnergyItemName,c.CoName,d.ModuleName as MmName");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" left join v0_energyitemdict as b on a.EnergyItemCode=b.EnergyItemCode");
            strSql.Append(" left join vp_coinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" left join v0_module as d on a.Ledger = d.Ledger and a.Mm_id = d.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger and a.IsChrg=1");
            if (isDefine >= 0)
                strSql.Append(" and a.IsDefine = @IsDefine");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, IsDefine = isDefine });
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetProList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Co_id,a.Number,a.CoName,a.Disabled,b.Area,a.CustAddr,a.CustName,a.Mobile,a.Remark");
            strSql.Append(" from vp_coinfo as a left join v1_custinfobuild as b on a.Ledger=b.Ledger");
            strSql.Append(" where a.Ledger=@ledger and a.Attrib =@Attrib");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = (int)CoAttrib.Project });
        }

        /// <summary>
        /// 获取建筑信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetBuildList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Co_id,a.Number,a.CoName,a.Parent_id,a.Disabled,b.Area,a.CustAddr,a.CustName,a.Mobile,a.Email,a.Remark,0 as IsAdd");
            strSql.Append(" from vp_coinfo as a left join v1_custinfobuild as b on a.Ledger=b.Ledger");
            strSql.Append(" where a.Ledger=@ledger and a.Attrib =@Attrib");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = (int)CoAttrib.Build });
        }

        public int GetBuildCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(*)as cnt");
            strSql.Append(" from vp_coinfo as a");
            strSql.Append(" where a.Ledger=@ledger and a.Attrib =@Attrib and a.Layer<=1");            
            return CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Attrib = (int)CoAttrib.Build }));
        }

        public DataTable GetMmDefineCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.IsDefine,count(*)as Cnt");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" where a.Ledger=@ledger group by a.IsDefine");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }
    }
}
