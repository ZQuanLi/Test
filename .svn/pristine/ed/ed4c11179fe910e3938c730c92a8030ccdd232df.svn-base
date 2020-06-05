using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.ExpAdminApp.Monitor
{
    public partial class ExpAdminMonitorDAL
    {

        private int Ledger = 0;
        private int SysUid = 1;
        private string Project = "";
        public ExpAdminMonitorDAL(string Project, int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            this.Project = Project;
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
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Project, this.Ledger, this.SysUid, out AreaPowerStr);
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
            strSql.Append("b.LastTime,c1.CoName,c1.Attrib,c1.CoStrcName,''as Ssr,s1.FrMd,b.MemcachKey,a.HandledBY,");
            strSql.Append("case when TIMESTAMPDIFF(MINUTE,b.LastTime,NOW()) < s1.FrMd then 1 else 0 end as `OnLine`,mp.IsAlarm,");
            strSql.Append("case when ifnull(mp.Price,0)=0 then s1.Price else mp.Price end as Price,fp.LpszDbVarName as CachComm");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" left join vp_v2info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr and a.Mm_id=b.Mm_id and b.FunType='E'");
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
        /// 获取查询参数
        /// </summary>
        /// <param name="Attrib"></param>
        /// <param name="Co_id"></param>
        /// <returns></returns>
        public DataTable GetCity(int Attrib, int Co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Co_id,a.CoNo,a.CoName,a.Disabled,a.Parent_id,a.Attrib,");
            strSql.Append("b.CustAddr,b.Office_tel,b.Mobile,b.Email,b.IsDefine,m1.CoName as PCoName");
            strSql.Append(" from v1_cust as a left join v1_custinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v1_cust as m1 on a.Ledger=m1.Ledger and a.Parent_id=m1.Co_id");
            strSql.Append(" where a.Ledger=@Ledger  ");
            if (Attrib != 0)
            {
                strSql.Append(" AND a.Attrib=@Attrib");
            }
            if (Co_id != 0)
            {
                strSql.Append(" and a.Parent_id=@Parent_id");
            }
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = Attrib, Parent_id = Co_id });
        }

        /// <summary>
        /// 获取房间查询参数
        /// </summary>
        /// <param name="Attrib"></param>
        /// <param name="Co_id"></param>
        /// <returns></returns>
        public DataTable GetRoom(int Attrib, int Co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Co_id,a.CoNo,a.CoName,a.Disabled,a.Parent_id,a.Attrib,");
            strSql.Append("b.CustAddr,b.Office_tel,b.Mobile,b.Email,b.IsDefine,m1.CoName as PCoName");
            strSql.Append(" from v1_cust as a left join v1_custinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v1_cust as m1 on a.Ledger=m1.Ledger and a.Parent_id=m1.Co_id");
            strSql.Append(" where a.Ledger=@Ledger  ");
            if (Co_id != 0)
            {
                strSql.Append(" and a.Parent_id=@Parent_id");
            }
            DataTable db = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = Attrib, Parent_id = Co_id });
            string ids = "";
            if (db.Rows.Count > 0)
            {
                foreach (DataRow dr in db.Rows)
                {
                    ids += dr["Co_id"] + ",";
                }
                ids = ids.TrimEnd(',');
            }

            strSql.Clear();
            strSql.Append("select a.Co_id,a.CoNo,a.CoName,a.Disabled,a.Parent_id,a.Attrib,");
            strSql.Append("b.CustAddr,b.Office_tel,b.Mobile,b.Email,b.IsDefine,m1.CoName as PCoName");
            strSql.Append(" from v1_cust as a left join v1_custinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v1_cust as m1 on a.Ledger=m1.Ledger and a.Parent_id=m1.Co_id");
            strSql.Append(" where a.Ledger=@Ledger  ");
            if (Attrib != 0)
            {
                strSql.Append(" AND a.Attrib=@Attrib");
            }
            if (!string.IsNullOrEmpty(ids))
            {
                strSql.Append(" and a.Parent_id in (@ids)");
            }
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = Attrib, ids = ids });
        }


    }
}
