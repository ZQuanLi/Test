using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Energy.Monitor
{
    partial class MonitorBLL
    {
        /// <summary>
        /// 获取实时数据列表
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public DataTable GetRealList(int co_id,string moduleName)
        {
            return dal.GetRealList(co_id, moduleName);
        }

        /// <summary>
        /// 获取回路的实时曲线数据
        /// </summary>
        /// <param name="co_id">回路ID号</param>
        /// <returns></returns>
        public object GetRealChart(int co_id)
        {
            DataTable dtSource = dal.GetRealChart(co_id, "");
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            string moduleName = "";
            StringBuilder splitMdQuery = new StringBuilder();
            StringBuilder splitTyQuery = new StringBuilder();
            foreach (DataRow dr in dtSource.Rows)
            {
                moduleName = CommFunc.ConvertDBNullToString(dr["ModuleName"]);
                if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
                    splitMdQuery.Append(",");
                splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitTyQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["FunType"]), ",")))
                {
                    if (!string.IsNullOrEmpty(splitTyQuery.ToString()))
                        splitTyQuery.Append(",");
                    splitTyQuery.Append(CommFunc.ConvertDBNullToString(dr["FunType"]));
                }
            }
            DateTime today2 = DateTime.Now.AddHours(-1); DateTime today1 = new DateTime(today2.Year, today2.Month, today2.Day);

            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), today1, today2, "hour", splitTyQuery.ToString());
            List<decimal> todayList = new List<decimal>();
            int nn = today2.Hour;
            while (nn-- >= 0)
                todayList.Add(0);
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                scale = scale == 0 ? 2 : scale;

                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;
                useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

                todayList[tagTime.Hour] = CommFunc.ConvertDBNullToDecimal(todayList[tagTime.Hour]) + useVal;
            }
            return new { moduleName = moduleName, list = todayList };
        }
    }
}
