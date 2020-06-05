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
    public partial class ExpYdSysLogDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public ExpYdSysLogDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }


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
    }
}
