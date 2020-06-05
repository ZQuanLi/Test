using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.PDU.Report
{
    public partial class ReportBLL
    {
        /// <summary>
        /// 获取统计
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="dataType">类型 日=day月=month年year</param>
        /// <returns></returns>
        public object GetEnergyUseVal(DateTime time, string dataType, string moduleName)
        {
            DataTable dtRst = this.GetEnergyUseValTabSchema();
            int cnt = 24;
            DateTime fm = time, to = time;
            if (dataType.ToLower().Equals("day"))
            {
                fm = new DateTime(time.Year, time.Month, time.Day);
                to = new DateTime(time.Year, time.Month, time.Day, 23, 59, 0);
                cnt = 24;
                dataType = "hour";
            }
            else if (dataType.ToLower().Equals("month"))
            {
                fm = new DateTime(time.Year, time.Month, 1);
                to = fm.AddMonths(1).AddDays(-1);
                cnt = to.Day;
                dataType = "day";
            }
            else if (dataType.ToLower().Equals("year"))
            {
                fm = new DateTime(time.Year, 1, 1);
                to = new DateTime(time.Year, 12, 1);
                cnt = 12;
                dataType = "month";
            }
            else
            {
                throw new Exception("查询类型错误");
            }
            DataTable dtSource = dal.GetEnergyUseVal(moduleName);
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["Module_id"], dtRst.Columns["Fun_id"] };
            //
            StringBuilder splitMdQuery = new StringBuilder();
            StringBuilder splitTyQuery = new StringBuilder();
            foreach (DataRow dr in dtSource.Rows)
            {
                if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
                    splitMdQuery.Append(",");
                splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));

                if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitTyQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["FunType"]), ",")))
                {
                    if (!string.IsNullOrEmpty(splitTyQuery.ToString()))
                        splitTyQuery.Append(",");
                    splitTyQuery.Append(CommFunc.ConvertDBNullToString(dr["FunType"]));
                }

                DataRow addDr = dtRst.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (addDr == null)
                {
                    addDr = dtRst.NewRow();
                    addDr["Module_id"] = dr["Module_id"];
                    addDr["Fun_id"] = dr["Fun_id"];
                    addDr["Cnt"] = cnt;
                    addDr["ModuleName"] = dr["ModuleName"];
                    addDr["FunName"] = dr["FunName"];
                    addDr["IsDefine"] = dr["IsDefine"];                    
                    dtRst.Rows.Add(addDr);
                }
            }
            #region 获取存储数据
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), fm, to, dataType, splitTyQuery.ToString());
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;

                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                scale = scale == 0 ? 2 : scale;

                DataRow addDr = dtRst.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (addDr == null) continue;
                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;

                useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

                if (dataType.Equals("hour"))
                    addDr["V" + (tagTime.Hour + 1).ToString().PadLeft(2, '0')] = CommFunc.ConvertDBNullToDecimal(addDr["V" + (tagTime.Hour + 1).ToString().PadLeft(2, '0')]) + useVal;
                else if (dataType.Equals("day"))
                    addDr["V" + tagTime.Day.ToString().PadLeft(2, '0')] = CommFunc.ConvertDBNullToDecimal(addDr["V" + tagTime.Day.ToString().PadLeft(2, '0')]) + useVal;
                else if (dataType.Equals("month"))
                    addDr["V" + tagTime.Month.ToString().PadLeft(2, '0')] = CommFunc.ConvertDBNullToDecimal(addDr["V" + tagTime.Month.ToString().PadLeft(2, '0')]) + useVal;
                addDr["UseVal"] = CommFunc.ConvertDBNullToDecimal(addDr["UseVal"]) + useVal;
            }
            #endregion
            #region 获取结果集数据
            List<object> res1 = new List<object>();
            foreach (DataRow s1 in dtRst.Rows)
            {
                List<decimal> vd = new List<decimal>();
                int count = CommFunc.ConvertDBNullToInt32(s1["Cnt"]);
                int c1 = 0;
                while (++c1 <= count)
                    vd.Add(CommFunc.ConvertDBNullToDecimal(s1["V" + c1.ToString().PadLeft(2, '0')]));
                object fd = new
                {
                    ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]) + (CommFunc.ConvertDBNullToInt32(s1["IsDefine"]) == 0 ? " " + CommFunc.ConvertDBNullToString(s1["FunName"]) : ""),
                    FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                    Cnt = count,
                    Data = vd,
                };
                res1.Add(fd);
            }
            return res1;
            //var res1 = from s1 in dtRst.AsEnumerable()
            //           select new
            //           {
            //               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
            //               Cnt = CommFunc.ConvertDBNullToInt32(s1["Cnt"]),
            //               V01 = CommFunc.ConvertDBNullToDecimal(s1["V01"]),
            //               V02 = CommFunc.ConvertDBNullToDecimal(s1["V02"]),
            //               V03 = CommFunc.ConvertDBNullToDecimal(s1["V03"]),
            //               V04 = CommFunc.ConvertDBNullToDecimal(s1["V04"]),
            //               V05 = CommFunc.ConvertDBNullToDecimal(s1["V05"]),
            //               V06 = CommFunc.ConvertDBNullToDecimal(s1["V06"]),
            //               V07 = CommFunc.ConvertDBNullToDecimal(s1["V07"]),
            //               V08 = CommFunc.ConvertDBNullToDecimal(s1["V08"]),
            //               V09 = CommFunc.ConvertDBNullToDecimal(s1["V09"]),
            //               V10 = CommFunc.ConvertDBNullToDecimal(s1["V10"]),
            //               V11 = CommFunc.ConvertDBNullToDecimal(s1["V11"]),
            //               V12 = CommFunc.ConvertDBNullToDecimal(s1["V12"]),
            //               V13 = CommFunc.ConvertDBNullToDecimal(s1["V13"]),
            //               V14 = CommFunc.ConvertDBNullToDecimal(s1["V14"]),
            //               V15 = CommFunc.ConvertDBNullToDecimal(s1["V15"]),
            //               V16 = CommFunc.ConvertDBNullToDecimal(s1["V16"]),
            //               V17 = CommFunc.ConvertDBNullToDecimal(s1["V17"]),
            //               V18 = CommFunc.ConvertDBNullToDecimal(s1["V18"]),
            //               V19 = CommFunc.ConvertDBNullToDecimal(s1["V19"]),
            //               V20 = CommFunc.ConvertDBNullToDecimal(s1["V20"]),
            //               V21 = CommFunc.ConvertDBNullToDecimal(s1["V21"]),
            //               V22 = CommFunc.ConvertDBNullToDecimal(s1["V22"]),
            //               V23 = CommFunc.ConvertDBNullToDecimal(s1["V23"]),
            //               V24 = CommFunc.ConvertDBNullToDecimal(s1["V24"]),
            //               V25 = CommFunc.ConvertDBNullToDecimal(s1["V25"]),
            //               V26 = CommFunc.ConvertDBNullToDecimal(s1["V26"]),
            //               V27 = CommFunc.ConvertDBNullToDecimal(s1["V27"]),
            //               V28 = CommFunc.ConvertDBNullToDecimal(s1["V28"]),
            //               V29 = CommFunc.ConvertDBNullToDecimal(s1["V29"]),
            //               V30 = CommFunc.ConvertDBNullToDecimal(s1["V30"]),
            //               V31 = CommFunc.ConvertDBNullToDecimal(s1["V31"]),
            //           };
            //return res1.ToList();
            #endregion
        }

        private DataTable GetEnergyUseValTabSchema()
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("Module_id", typeof(System.Int32));// 分项ID号
            dtRst.Columns.Add("Fun_id", typeof(System.Int32));// 分项ID号
            dtRst.Columns.Add("Cnt", typeof(System.Int32));//当前值列数
            dtRst.Columns.Add("V01", typeof(System.Decimal));//值01
            dtRst.Columns.Add("V02", typeof(System.Decimal));//值02
            dtRst.Columns.Add("V03", typeof(System.Decimal));//值03
            dtRst.Columns.Add("V04", typeof(System.Decimal));//值04
            dtRst.Columns.Add("V05", typeof(System.Decimal));//值05
            dtRst.Columns.Add("V06", typeof(System.Decimal));//值06
            dtRst.Columns.Add("V07", typeof(System.Decimal));//值07
            dtRst.Columns.Add("V08", typeof(System.Decimal));//值08
            dtRst.Columns.Add("V09", typeof(System.Decimal));//值09
            dtRst.Columns.Add("V10", typeof(System.Decimal));//值10
            dtRst.Columns.Add("V11", typeof(System.Decimal));//值11
            dtRst.Columns.Add("V12", typeof(System.Decimal));//值12
            dtRst.Columns.Add("V13", typeof(System.Decimal));//值13
            dtRst.Columns.Add("V14", typeof(System.Decimal));//值14
            dtRst.Columns.Add("V15", typeof(System.Decimal));//值15
            dtRst.Columns.Add("V16", typeof(System.Decimal));//值16
            dtRst.Columns.Add("V17", typeof(System.Decimal));//值17
            dtRst.Columns.Add("V18", typeof(System.Decimal));//值18
            dtRst.Columns.Add("V19", typeof(System.Decimal));//值19
            dtRst.Columns.Add("V20", typeof(System.Decimal));//值20
            dtRst.Columns.Add("V21", typeof(System.Decimal));//值21
            dtRst.Columns.Add("V22", typeof(System.Decimal));//值22
            dtRst.Columns.Add("V23", typeof(System.Decimal));//值23
            dtRst.Columns.Add("V24", typeof(System.Decimal));//值24
            dtRst.Columns.Add("V25", typeof(System.Decimal));//值25
            dtRst.Columns.Add("V26", typeof(System.Decimal));//值26
            dtRst.Columns.Add("V27", typeof(System.Decimal));//值27
            dtRst.Columns.Add("V28", typeof(System.Decimal));//值28
            dtRst.Columns.Add("V29", typeof(System.Decimal));//值29
            dtRst.Columns.Add("V30", typeof(System.Decimal));//值30
            dtRst.Columns.Add("V31", typeof(System.Decimal));//值31
            dtRst.Columns.Add("UseVal", typeof(System.Decimal));//汇总值
            dtRst.Columns.Add("ModuleName", typeof(System.String));// 设备名称
            dtRst.Columns.Add("FunName", typeof(System.String));// 设备名称
            dtRst.Columns.Add("IsDefine", typeof(System.Int32));//当前值列数
            
            return dtRst;
        }
    }
}
