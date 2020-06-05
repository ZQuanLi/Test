using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Report
{
    public partial class ReportDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        public ReportDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取全部区域列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetAreaList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select Co_id,CoName,Parent_id,CoStrcName,Attrib");
            strSql.Append(" from vp_coinfo where Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }
        /// <summary>
        /// 获取全部设备列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetMeterList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Meter_id,a.MeterName,a.Co_id ");
            strSql.Append(" from vp_mdinfo as a where a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }
    }
}
