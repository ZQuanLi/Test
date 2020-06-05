using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.PDU.Home
{
    partial class HomeDAL
    {
        public DataTable GetPduMap()
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Clear();
            //strSql.Append("select b.Co_id,b.CoName,b.CustAddr");
            //strSql.Append(" from vp_mdinfo as a inner join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            //strSql.Append(" where a.Ledger=@ledger");
            strSql.Clear();
            strSql.Append("select b.Co_id,b.CoName,b.CustAddr,b.Remark");
            strSql.Append(" from vp_coinfo as b");
            strSql.Append(" where b.Ledger=@Ledger and b.Layer=0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }


        /// <summary>
        /// 获取PDU树状
        /// </summary>
        /// <returns></returns>
        public DataTable GetPduTree(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            string strList = "";
            if (co_id != 0)
            {
                strSql.Clear();
                strSql.Append("select GetCoChildList(@Ledger,@Co_id)");
                object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
                strList = CommFunc.ConvertDBNullToString(obj);
            }
            strSql.Clear();            
            strSql.Append("select a.Co_id,a.CoNo,a.CoName,a.Disabled,a.Parent_id,a.Attrib,a.Layer");
            strSql.Append(" from v1_cust as a");
            strSql.Append(" where a.Ledger=@Ledger");
            if (!string.IsNullOrEmpty(strList))
                strSql.Append(" and FIND_IN_SET(a.Co_id,@Strlist)");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Strlist= strList });
        }

        /// <summary>
        /// 分组
        /// </summary>
        /// <returns></returns>
        public int GetPduGroupCount(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            string strList = "";
            if (co_id != 0)
            {
                strSql.Clear();
                strSql.Append("select GetCoChildList(@Ledger,@Co_id)");
                object obj1 = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
                strList = CommFunc.ConvertDBNullToString(obj1);
            }

            strSql.Clear();
            strSql.Append("select count(distinct c.Co_id) as cnt");
            strSql.Append(" from vp_mdinfo as a inner join v1_cust as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" inner join v1_cust as c on b.Ledger=c.Ledger and b.Parent_id=c.Co_id");
            strSql.Append(" where a.Ledger=@ledger");
            if (co_id != 0)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@Strlist)");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Strlist = strList });
            return CommFunc.ConvertDBNullToInt32(obj);
        }
        /// <summary>
        /// PDU信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetPduInfo(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            string strList = "";
            if (co_id != 0)
            {
                strSql.Clear();
                strSql.Append("select GetCoChildList(@Ledger,@Co_id)");
                object obj1 = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
                strList = CommFunc.ConvertDBNullToString(obj1);
            }

            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Parent_id,a.Co_id,cast(ifnull(b.LastTime,'1900-01-01') as char(30))as LastTime,s1.FrMd");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" left join vp_v2info as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and a.Co_id=b.Co_id and a.Fun_id=b.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.FunType='E'");
            if (co_id != 0)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@Strlist)");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Strlist = strList });
        }

        public DataTable GetPduEnergyPie(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            string strList = "";
            if (co_id != 0)
            {
                strSql.Clear();
                strSql.Append("select GetCoChildList(@Ledger,@Co_id)");
                object obj1 = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
                strList = CommFunc.ConvertDBNullToString(obj1);
            }
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Fun_id,a.Scale,a.Multiply,a.Co_id,b.Parent_id,c.CoName");
            strSql.Append(" from vp_funinfo as a inner join v1_cust as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" inner join v1_cust as c on b.Ledger=c.Ledger and b.Parent_id=c.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Parent_id=0 and a.FunType='E'");
            if (co_id != 0)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@Strlist)");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Strlist = strList });
        }
    }
}
