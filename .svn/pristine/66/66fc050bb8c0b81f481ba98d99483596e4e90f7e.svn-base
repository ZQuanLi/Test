using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Dqhz
{
    public partial class DqhzBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private YDS6000.DAL.Exp.Dqhz.DqhzDAL dal = null;
        public DqhzBLL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            dal = new YDS6000.DAL.Exp.Dqhz.DqhzDAL(ledger, uid);
        }

        /// <summary>
        /// 获取电气或者列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetDqhzList()
        {
            return dal.GetDqhzList();
        }

        /// <summary>
        /// 参数下拉框
        /// </summary>
        /// <returns></returns>
        public DataTable GetFunTypes()
        {
            return dal.GetFunTypes();
        }

        /// <summary>
        /// 获取参数详情
        /// </summary>
        /// <returns></returns>
        public DataTable GetFunData(int module_id, DateTime date, string dateType, string funType)
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("TagTime", typeof(System.DateTime));
            dtRst.Columns.Add("Time", typeof(System.String));
            dtRst.Columns.Add("UseVal", typeof(System.Decimal));
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["TagTime"] };
            StringBuilder strKey1 = new StringBuilder();
            DataTable dtDb = dal.GetFunTypes(module_id);
            dtDb.PrimaryKey = new DataColumn[] { dtDb.Columns["Module_id"], dtDb.Columns["Fun_id"] };
            DateTime start = date;
            DateTime end = start;
            if (dateType.Equals("day"))
            {
                start = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
                end = new DateTime(start.Year, start.Month, start.Day, 23, 0, 0);
            }
            if (dateType.Equals("month"))
            {
                start = new DateTime(start.Year, start.Month, start.Day);
                end = start.AddMonths(1).AddDays(-1);
            }
            if (dateType.Equals("year"))
            {
                start = new DateTime(start.Year, start.Month, 1);
                end = start.AddYears(1).AddDays(-1);
            }
            DateTime b1 = start, b2 = end;
            string ddType = dateType;
            #region 填充主数据
            while (start <= end)
            {
                strKey1.Clear();
                if (dateType.Equals("day"))
                    strKey1.Append(start.Hour.ToString().PadLeft(2, '0') + "时");
                if (dateType.Equals("month"))
                    strKey1.Append(start.Day.ToString().PadLeft(2, '0') + "日");
                if (dateType.Equals("year"))
                    strKey1.Append(start.Month.ToString().PadLeft(2, '0') + "月");
                DataRow addDr = dtRst.Rows.Find(new object[] { start });
                if (addDr == null)
                { //增加需显示的表
                    addDr = dtRst.NewRow();
                    addDr["TagTime"] = start;
                    addDr["Time"] = strKey1.ToString();
                    dtRst.Rows.Add(addDr);
                }
                if (dateType.Equals("day"))
                {
                    start = start.AddHours(1);
                    ddType = "hour";
                }
                else if (dateType.Equals("month"))
                {
                    start = start.AddDays(1);
                    ddType = "day";
                }
                else if (dateType.Equals("year"))
                {
                    start = start.AddMonths(1);
                    ddType = "month";
                }
                else
                {
                    break;
                }
            }
            #endregion
            DataTable dtSource = WholeBLL.GetCoreQueryData(this.Ledger, module_id.ToString(), b1, b2, ddType, funType);
            foreach (DataRow dr in dtSource.Rows)
            {
                DataRow tagDr = dtRst.Rows.Find(dr["TagTime"]);
                if (tagDr == null) continue;

                DataRow curDr = dtDb.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int fun_id = CommFunc.ConvertDBNullToInt32(curDr["Fun_id"]);
                int co_id = CommFunc.ConvertDBNullToInt32(curDr["Co_id"]);
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                //decimal multiply = CommFunc.ConvertDBNullToInt32(curDr["Multiply"]);
                if (CommFunc.ConvertDBNullToInt32(dr["Fun_id"]) != fun_id) continue;
                if (CommFunc.ConvertDBNullToInt32(dr["Co_id"]) != co_id) continue;
                if (!CommFunc.ConvertDBNullToString(dr["ModuleAddr"]).Equals(CommFunc.ConvertDBNullToString(curDr["ModuleAddr"]))) continue;                
                decimal useVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                if (scale != 0)
                    useVal = Math.Round(useVal, scale, MidpointRounding.AwayFromZero);
                tagDr["UseVal"] = useVal;
            }
            return dtRst;
        }
    }
}
