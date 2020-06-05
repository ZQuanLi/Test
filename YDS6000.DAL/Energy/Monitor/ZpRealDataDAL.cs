using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;


namespace YDS6000.DAL.Energy.Monitor
{
    public partial class MonitorDAL
    {
        /// <summary>
        /// 获取实时数据列表
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public DataTable GetRealList(int co_id,string moduleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select GetCoChildList(@Ledger,@Co_id)as ids");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
            string strList = CommFunc.ConvertDBNullToString(obj);
            strSql.Clear();
            strSql.Append("select a.Module_id,a.Fun_id,a.ModuleAddr,a.ModuleName,a.FunType,a.FunName,a.Scale,a.Multiply,");
            //strSql.Append("case when ifnull(mp.TagName,'')='' then a.LpszDbVarName else mp.TagName end as LpszDbVarName,b.CoName,c.LastVal,c.LastTime");
            strSql.Append("a.LpszDbVarName,b.CoName,c.LastVal,c.LastTime");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" left join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v2_info as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id and a.Module_id=c.Module_id and a.ModuleAddr=c.ModuleAddr and a.Fun_id=c.Fun_id");
            strSql.Append(" left join v1_map as mp on a.Ledger=mp.Ledger and a.Module_id=mp.Module_id and a.Fun_id=mp.Fun_id");
            strSql.Append(" where a.Ledger=@ledger and a.IsChrg=1 and FIND_IN_SET(a.Co_id,@StrList)");
            if (!string.IsNullOrEmpty(moduleName))
            {
                strSql.Append(" and a.ModuleName like @ModuleName");
            }
            strSql.Append(" order by a.MeterNo,a.Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, StrList = strList, ModuleName = moduleName + "%" });
        }

        /// <summary>
        /// 获取实时数据图表
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public DataTable GetRealChart(int co_id, string moduleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select GetCoChildList(@Ledger,@Co_id)as ids");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
            string strList = CommFunc.ConvertDBNullToString(obj);
            strSql.Clear();
            strSql.Append("select a.Module_id,a.Fun_id,a.ModuleAddr,a.ModuleName,a.FunType,a.FunName,a.Scale,a.Multiply,");
            //strSql.Append("case when ifnull(mp.TagName,'')='' then a.LpszDbVarName else mp.TagName end as LpszDbVarName,b.CoName,c.LastVal,c.LastTime");
            strSql.Append("a.LpszDbVarName,b.CoName,c.LastVal,c.LastTime");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" left join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v2_info as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id and a.Module_id=c.Module_id and a.ModuleAddr=c.ModuleAddr and a.Fun_id=c.Fun_id");
            strSql.Append(" left join v1_map as mp on a.Ledger=mp.Ledger and a.Module_id=mp.Module_id and a.Fun_id=mp.Fun_id");
            strSql.Append(" where a.Ledger=@ledger and a.Parent_id=0 and a.IsChrg=1 and FIND_IN_SET(a.Co_id,@StrList)");
            strSql.Append(" and (a.IsDefine=0 or a.IsDefine=330)");/*特殊要求： 用电跟压缩空气*/
            if (!string.IsNullOrEmpty(moduleName))
            {
                strSql.Append(" and a.ModuleName like @ModuleName");
            }
            strSql.Append(" order by a.MeterNo,a.Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, StrList = strList, ModuleName = moduleName + "%" });
        }
    }
}
