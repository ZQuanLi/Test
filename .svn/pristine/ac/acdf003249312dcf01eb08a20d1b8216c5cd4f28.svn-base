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
    public class SyscontDAL
    {
        private string Ledgers = "";
        public SyscontDAL(string _ledgers)
        {
            this.Ledgers = _ledgers;
        }

        /// <summary>
        /// 获取基本配置信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSyscont()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Ledger,CoName,FrMd,MaxVal,Project,Auto from syscont ");
            if (!string.IsNullOrEmpty(Ledgers))
            strSql.Append(" where FIND_IN_SET(Ledger,@Strlist)");
            return SQLHelper.Query(strSql.ToString(), new { Strlist = Ledgers });
        }

        /// <summary>
        /// 获取回路采集点信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetMapInfo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Ledger,a.Module_id,a.ModuleAddr,a.Fun_id,a.FunType,a.FunName,a.Scale,a.Action,a.HandledBY,a.LpszDbVarName as CachedKey,a.TagName as NKey,b.TagName");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" inner join v1_map as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and a.Fun_id=b.Fun_id");
            strSql.Append(" where ifnull(b.TagName,'')!=''");
            if (!string.IsNullOrEmpty(Ledgers))
                strSql.Append(" and FIND_IN_SET(a.Ledger,@Strlist)");
            return SQLHelper.Query(strSql.ToString(), new { Strlist = Ledgers });
        }
    }
}
