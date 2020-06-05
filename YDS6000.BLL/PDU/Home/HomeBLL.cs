using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.PDU.Home
{
    public partial class HomeBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.PDU.Home.HomeDAL dal = null;
        public HomeBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.PDU.Home.HomeDAL(_ledger, _uid);
        }
        /// <summary>
        /// 获取PDU列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetPduList()
        {
            return dal.GetPduList();
        }

        /// <summary>
        /// Pdu状态信息
        /// </summary>
        /// <param name="co_id">pdu ID号</param>
        /// <returns></returns>
        public DataTable GetPduStatus(int co_id)
        {
            return dal.GetPduStatus(co_id);
        }

        public object GetPduFunTypes(int co_id,string funTypes)
        {
            //string funType = string.Join(",", funTypes.ToArray());
            string funType = funTypes;//

            DateTime today2 = DateTime.Now.AddHours(-1); DateTime today1 = new DateTime(today2.Year, today2.Month, today2.Day);
            Dictionary<string, List<decimal>> rr = new Dictionary<string, List<decimal>>();
            foreach (string str in funTypes.Split(new char[] { ','},StringSplitOptions.RemoveEmptyEntries))
            {
                List<decimal> todayList = new List<decimal>();
                if (rr.TryGetValue(str, out todayList) == false)
                {
                    todayList = new List<decimal>();
                    int nn = today2.Hour;
                    while (nn-- >= 0)
                        todayList.Add(0);
                    rr.Add(str, todayList);
                }
            }
            //
            DataTable dtSource = dal.GetPduFunTypes(co_id, funType);

            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            StringBuilder splitMdQuery = new StringBuilder();
            StringBuilder splitTyQuery = new StringBuilder();
            foreach (DataRow dr in dtSource.Rows)
            {
                if (string.IsNullOrEmpty(splitMdQuery.ToString()))
                    splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                //if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitMdQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["Module_id"]), ",")))
                //{
                //    if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
                //        splitMdQuery.Append(",");
                //    splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                //}
                if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitTyQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["FunType"]), ",")))
                {
                    if (!string.IsNullOrEmpty(splitTyQuery.ToString()))
                        splitTyQuery.Append(",");
                    splitTyQuery.Append(CommFunc.ConvertDBNullToString(dr["FunType"]));
                }
            }
            DataTable dtUse1 = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), today1, today2, "hour", splitTyQuery.ToString());
            decimal todayUse = 0;
            foreach (DataRow dr in dtUse1.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                int isChrg = CommFunc.ConvertDBNullToInt32(curDr["IsChrg"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                scale = scale == 0 ? 2 : scale;
                //
                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = 0;
                if (isChrg == 1)
                {
                    useVal = lastVal - firstVal;
                    useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);
                    //
                    List<decimal> todayList = new List<decimal>();
                    if (rr.TryGetValue(CommFunc.ConvertDBNullToString(curDr["FunType"]), out todayList) == true)
                    {
                        todayList[tagTime.Hour] = CommFunc.ConvertDBNullToDecimal(todayList[tagTime.Hour]) + useVal;
                        todayUse = todayUse + useVal;
                    }
                }
                else
                {
                    useVal = lastVal;
                    useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);
                    //
                    List<decimal> todayList = new List<decimal>();
                    if (rr.TryGetValue(CommFunc.ConvertDBNullToString(curDr["FunType"]), out todayList) == true)
                    {
                        todayList[tagTime.Hour] = CommFunc.ConvertDBNullToDecimal(todayList[tagTime.Hour]) > useVal ? CommFunc.ConvertDBNullToDecimal(todayList[tagTime.Hour]) : useVal;
                        todayUse = todayUse > useVal ? todayUse : useVal;
                    }
                }
            }
            List<object> dd = new List<object>();
            foreach (var ss in rr)
                dd.Add(new { funType = ss.Key, list = ss.Value });
            return dd;
        }

        public object GetPduEnergy(int co_id)
        {
            DateTime today2 = DateTime.Now.AddHours(-1); DateTime today1 = new DateTime(today2.Year, today2.Month, today2.Day);            
            DataTable dtSource = dal.GetPduFunTypes(co_id, "E");
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            StringBuilder splitMdQuery = new StringBuilder();
            StringBuilder splitTyQuery = new StringBuilder();
            string unit = "";
            foreach (DataRow dr in dtSource.Rows)
            {
                unit = CommFunc.ConvertDBNullToString(dr["Unit"]);
                if (string.IsNullOrEmpty(splitMdQuery.ToString()))
                    splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitTyQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["FunType"]), ",")))
                {
                    if (!string.IsNullOrEmpty(splitTyQuery.ToString()))
                        splitTyQuery.Append(",");
                    splitTyQuery.Append(CommFunc.ConvertDBNullToString(dr["FunType"]));
                }
            }
            decimal todayUse = this.GetPduEnergy(dtSource, splitMdQuery.ToString(), today1, today2, "hour", splitTyQuery.ToString());
            decimal ysdayUse = this.GetPduEnergy(dtSource, splitMdQuery.ToString(), today1.AddDays(-1), today2.AddDays(-1), "hour", splitTyQuery.ToString());
            decimal monthUse = this.GetPduEnergy(dtSource, splitMdQuery.ToString(), today1, today2, "month", splitTyQuery.ToString());
            decimal yearUse = this.GetPduEnergy(dtSource, splitMdQuery.ToString(), new DateTime(today1.Year,1,1), new DateTime(today1.Year, 12, 1), "month", splitTyQuery.ToString());
            decimal ystb = Math.Round((ysdayUse == 0 ? 0 : ((todayUse - ysdayUse) / ysdayUse)) * 100, 2, MidpointRounding.AwayFromZero);

            return new { todayUse = todayUse , monthUse = monthUse , yearUse = yearUse , ystb = ystb.ToString("f2")+"%" ,unit = unit } ;
        }

        private decimal GetPduEnergy(DataTable dtSource,string module_id, DateTime start, DateTime end, string dataType,string funType)
        {
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, module_id, start, end, dataType , funType);
            decimal allUse = 0;
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                scale = scale == 0 ? 2 : scale;
                //
                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;
                useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);
                //
                allUse = allUse + useVal;
            }
            return allUse;
        }

        /// <summary>
        /// 获取告警数据
        /// </summary>
        /// <returns></returns>
        public object GetPduAlarm(int co_id)
        {
            DataTable dtSource = dal.GetPduAlarm(co_id);
            int mth = dtSource.Rows.Count;
            int day = dtSource.Select("CDate=#" + DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "#").Count();
            string code = "", type = "", time = "",name = "";
            if (mth > 0)
            {
                code = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["Module_id"]).PadLeft(5, '0') + CommFunc.ConvertDBNullToString(dtSource.Rows[0]["Fun_id"]).PadLeft(3, '0');
                type = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["FunName"]) + "越限";
                time = CommFunc.ConvertDBNullToDateTime(dtSource.Rows[0]["CollectTime"]).ToString("yyyy-MM-dd");
                name = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["ModuleName"]);
            }
            return new { mth = mth, day = day, code = code, type = type, time = time, name = name };
        }
    }
}
