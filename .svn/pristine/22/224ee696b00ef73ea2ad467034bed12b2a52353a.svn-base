using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YDS6000.Models;


namespace YDS6000.BLL.Exp.Alarm
{
    partial class AlarmBLL
    {
        public DataTable GetYdAlarmOfCmdList(string CoStrcName, string CoName, DateTime Start, DateTime End)
        {
            DataTable dtSource = dal.GetYdAlarmOfCmdList(CoStrcName, CoName, Start, End);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            dtSource.Columns.Add("FunTypeS", typeof(System.String));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["RowId"] = ++RowId;
                int errCode = CommFunc.ConvertDBNullToInt32(dr["ErrCode"]);
                if (errCode == 0)
                    dr["ErrTxt"] = "远程控制中,若延时,请重新远程控制";
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
            return dtSource;
        }
    }
}
