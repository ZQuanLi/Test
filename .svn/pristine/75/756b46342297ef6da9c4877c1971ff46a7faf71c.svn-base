using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.RunReport
{
    public partial class ExpYdSsrBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private string Project = "";
        private readonly YDS6000.DAL.Exp.RunReport.ExpYdSsrDAL dal = null;
        public ExpYdSsrBLL(string _project, int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            this.Project = _project;
            dal = new YDS6000.DAL.Exp.RunReport.ExpYdSsrDAL(_project, _ledger, _uid);
        }
        
        public DataTable GetYdSsrOfList(string CoStrcName, string CoName, DateTime Start, DateTime End)
        {
            DataTable dtSource = dal.GetYdSsrOfCmd(CoStrcName, CoName, Start, End);
            //DataTable dtAlarm = dal.GetYdSsrOfAlarm(CoStrcName, CoName, Start, End);
            //dtSource.Merge(dtAlarm, true, MissingSchemaAction.Ignore);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            dtSource.Columns.Add("FunTypeS", typeof(System.String));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["RowId"] = ++RowId;
                if (CommFunc.ConvertDBNullToString(dr["FunType"]).Equals(V0Fun.Ssr.ToString()))
                {
                    if (CommFunc.ConvertDBNullToInt32(dr["DataValue"]) == 0)
                    {
                        dr["FunTypeS"] = "合闸";
                    }
                    else if (CommFunc.ConvertDBNullToInt32(dr["DataValue"]) == 1)
                    {
                        dr["FunTypeS"] = "拉闸";
                    }
                }
                else
                {
                    System.Reflection.FieldInfo info = typeof(V0Fun).GetField(CommFunc.ConvertDBNullToString(dr["FunType"]));
                    if (info != null)
                    {
                        var obj = info.GetCustomAttributes(typeof(Describe), false);
                        if (obj != null)
                        {
                            foreach (Describe md in obj)
                                dr["FunTypeS"] = md.describe;
                        }
                    }
                }
            }
            return dtSource;
        }
    }
}
