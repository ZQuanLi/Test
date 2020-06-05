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
        public DataTable GetYdAlarmOfGwList(string CoStrcName, string CoName, string AType, DateTime Start, DateTime End)
        {
            DataTable dtSource = dal.GetYdAlarmOfGwList(CoStrcName, CoName, AType, Start, End);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            //dtSource.Columns.Add("ContentS", typeof(System.String));
            dtSource.Columns.Add("ATypeS", typeof(System.String));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["RowId"] = ++RowId;
                //System.Reflection.FieldInfo info = typeof(V0Fun).GetField(CommFunc.ConvertDBNullToString(dr["Content"]));
                //if (info != null)
                //{
                //    var obj = info.GetCustomAttributes(typeof(Describe), false);
                //    if (obj != null)
                //    {
                //        foreach (Describe md in obj)
                //            dr["ContentS"] = md.describe;
                //    }
                //}
                /////////////////////////////////
                System.Reflection.FieldInfo info = typeof(AlarmType).GetField(CommFunc.ConvertDBNullToString(dr["AType"]));
                if (info != null)
                {
                    var obj = info.GetCustomAttributes(typeof(Describe), false);
                    if (obj != null)
                    {
                        foreach (Describe md in obj)
                            dr["ATypeS"] = md.describe;
                    }
                }
            }
            return dtSource;
        }
       
    }
}
