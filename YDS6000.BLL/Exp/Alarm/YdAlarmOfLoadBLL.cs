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
        public DataTable GetYdAlarmOfLoadList(string CoStrcName, string CoName, DateTime Start, DateTime End)
        {
            DataTable dtSource = dal.GetYdAlarmOfLoadList(CoStrcName, CoName, Start, End);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["RowId"] = ++RowId;
            }
            return dtSource;
        }



    }
}
