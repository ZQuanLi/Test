using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Energy.Home
{
    public partial class HomeBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Energy.Home.HomeDAL dal = null;
        public HomeBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Energy.Home.HomeDAL(_ledger, _uid);
        }

        public object GetSysInfo()
        {
            int buildCnt = dal.GetBuildCount();
            DataTable dtSource = dal.GetMmDefineCount();
            var res1 = from s1 in dtSource.AsEnumerable()
                       select new
                       {
                           DeFineName = CommFunc.GetEnumDisplay(typeof(MmDefine), CommFunc.ConvertDBNullToInt32(s1["IsDefine"])),
                           Cnt = CommFunc.ConvertDBNullToInt32(s1["Cnt"])
                       };
            return new { BuildCnt = buildCnt, MdCount = res1.ToList() };
        }

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetProjectInfo()
        {
            return dal.GetProjectInfo();
        }

        public DataTable GetAlarmInfo()
        {
            return dal.GetAlarmInfo();
        }
        /// <summary>
        /// 获取能源列表（300到400的ID号）
        /// </summary>
        /// <returns></returns>
        public DataTable GetEnergyList()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("Id", typeof(System.Int32));
            dtSource.Columns.Add("Text", typeof(System.String));

            System.Reflection.FieldInfo[] fields = typeof(MmDefine).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (System.Reflection.FieldInfo field in fields)
            {
                MmDefine aa = (MmDefine)Enum.Parse(typeof(MmDefine), field.Name);
                int id = (int)aa;
                if (!(id == 0 || (id >= 300 && id < 400))) continue;

                var obj = field.GetCustomAttributes(typeof(DisplayAttribute), false);
                if (obj != null && obj.Count() != 0)
                {
                    DisplayAttribute md = obj[0] as DisplayAttribute;
                    dtSource.Rows.Add(new object[] { (int)aa, md.Name });
                }
            }
            return dtSource;
        }

        /// <summary>
        /// 获取用能数据
        /// 返回参数
        /// 第一个是当天的小时用能
        /// 第二个是昨日的小时用能
        /// 第三个当天的用能总和
        /// 最后是同期比
        /// </summary>
        /// <param name="isDefine"></param>
        /// <returns></returns>
        public object GetDayForChart(int isDefine)
        {
            DataTable dtSource = dal.GetMdList(isDefine);
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
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
            }
            DateTime today2 = DateTime.Now.AddHours(-1); DateTime today1 = new DateTime(today2.Year, today2.Month, today2.Day);
            DateTime yesterday1 = today1.AddDays(-1); DateTime yesterday2 = new DateTime(yesterday1.Year, yesterday1.Month, yesterday1.Day, 23, 59, 0);
            DataTable dtUse1 = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), today1, today2, "hour", splitTyQuery.ToString());
            DataTable dtUse2 = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), yesterday1, yesterday2, "hour", splitTyQuery.ToString());
            List<decimal> todayList = new List<decimal>();
            List<decimal> ysdayList = new List<decimal>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int nn = today2.Hour;
            while (nn-- >= 0) {
                todayList.Add(0);
            }
            decimal todayUse = 0, yesterdayUse = 0;
            foreach (DataRow dr in dtUse1.Rows)
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
                todayUse = todayUse + useVal;
            }
            foreach (DataRow dr in dtUse2.Rows)
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

                ysdayList[tagTime.Hour] = CommFunc.ConvertDBNullToDecimal(ysdayList[tagTime.Hour]) + useVal;
                if (tagTime.Hour<= today2.Hour)
                    yesterdayUse = yesterdayUse + useVal;
            }
            decimal tongBiZeng = Math.Round((yesterdayUse == 0 ? 0 : ((todayUse - yesterdayUse) / yesterdayUse)) * 100, 2, MidpointRounding.AwayFromZero);
            return new { todayList = todayList, ysdayList = ysdayList, todayUse = todayUse, tongBiZeng = tongBiZeng.ToString("f2") + "%" };
        }


        /// <summary>
        /// 获取用能数据
        /// 返回参数
        /// 第一个是当天的小时用能
        /// 第二个是昨日的小时用能
        /// 第三个当天的用能总和
        /// 最后是同期比
        /// </summary>
        /// <param name="co_id">配电房ID号</param>
        /// <returns></returns>
        public object GetDayForBuild(int co_id)
        {
            DataTable dtSource = dal.GetDayForBuild(co_id);
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
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
            }
            DateTime today2 = DateTime.Now.AddHours(-1); DateTime today1 = new DateTime(today2.Year, today2.Month, today2.Day);
            DateTime yesterday1 = today1.AddDays(-1); DateTime yesterday2 = new DateTime(yesterday1.Year, yesterday1.Month, yesterday1.Day, 23, 59, 0);
            DataTable dtUse1 = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), today1, today2, "hour", splitTyQuery.ToString());
            DataTable dtUse2 = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), yesterday1, yesterday2, "hour", splitTyQuery.ToString());
            List<decimal> todayList = new List<decimal>();
            List<decimal> ysdayList = new List<decimal>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int nn = today2.Hour;
            while (nn-- >= 0)
            {
                todayList.Add(0);
            }
            decimal todayUse = 0, yesterdayUse = 0;
            foreach (DataRow dr in dtUse1.Rows)
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
                todayUse = todayUse + useVal;
            }
            foreach (DataRow dr in dtUse2.Rows)
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

                ysdayList[tagTime.Hour] = CommFunc.ConvertDBNullToDecimal(ysdayList[tagTime.Hour]) + useVal;
                if (tagTime.Hour <= today2.Hour)
                    yesterdayUse = yesterdayUse + useVal;
            }
            decimal tongBiZeng = Math.Round((yesterdayUse == 0 ? 0 : ((todayUse - yesterdayUse) / yesterdayUse)) * 100, 2, MidpointRounding.AwayFromZero);
            return new { todayList = todayList, ysdayList = ysdayList, todayUse = todayUse, tongBiZeng = tongBiZeng.ToString("f2") + "%" };
        }

        /// <summary>
        /// 获取建筑当日用能前10名
        /// </summary>
        /// <param name="isDefine"></param>
        /// <returns></returns>
        public object GetDayForTopLine(int isDefine)
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("Co_id", typeof(System.Int32));
            dtRst.Columns.Add("UseVal", typeof(System.Decimal));
            dtRst.Columns.Add("Unit", typeof(System.String));
            dtRst.Columns.Add("CoName", typeof(System.String));
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["Co_id"] };
            //
            DataTable dtSource = dal.GetMdList(isDefine);
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
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
                int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
                //if (co_id != 0)
                //{
                    DataRow addDr = dtRst.Rows.Find(dr["Co_id"]);
                    if (addDr == null)
                        addDr = dtRst.Rows.Add(new object[] { dr["Co_id"], 0, dr["Unit"], dr["CoName"] });
                //}
            }

            DateTime today2 = DateTime.Now; DateTime today1 = new DateTime(today2.Year, today2.Month, today2.Day);
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), today1, today2, "day", splitTyQuery.ToString());
            decimal allUseVal = 0;
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
                allUseVal = allUseVal + useVal;
                DataRow addDr = dtRst.Rows.Find(dr["Co_id"]);
                if (addDr != null)
                {
                    addDr["UseVal"] = CommFunc.ConvertDBNullToDecimal(addDr["UseVal"]) + useVal;
                }
            }
            var res1 = from s1 in dtRst.AsEnumerable()
                       orderby CommFunc.ConvertDBNullToDecimal(s1["UseVal"]) descending
                       select new
                       {
                           BuildId = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                           BuildName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           UseVal = CommFunc.ConvertDBNullToDecimal(s1["UseVal"]),
                           Unit = CommFunc.ConvertDBNullToString(s1["Unit"]),
                           Percent = allUseVal == 0 ? 0 : CommFunc.ConvertDBNullToDecimal(s1["UseVal"]) / allUseVal,
                       };
            return res1.ToList().Take(10);
        }

        /// <summary>
        /// 获取分类用能数据
        /// </summary>     
        /// <returns></returns>
        public object GetClassification()
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("IsDefine", typeof(System.String));
            dtRst.Columns.Add("Day", typeof(System.Decimal));
            dtRst.Columns.Add("Month", typeof(System.Decimal));
            dtRst.Columns.Add("Year", typeof(System.Decimal));
            dtRst.Columns.Add("Unit", typeof(System.String));          
            //
            DataTable dtSource = dal.GetMdList();
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"] , dtSource.Columns["Fun_id"] };
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["IsDefine"] };
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

                DataRow addDr = dtRst.Rows.Find(dr["IsDefine"]);
                if (addDr == null)
                    addDr = dtRst.Rows.Add(new object[] { dr["IsDefine"], 0, 0, 0, dr["Unit"] });
            }
            DateTime today2 = DateTime.Now; DateTime today1 = new DateTime(today2.Year, today2.Month, today2.Day);
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), today1, today2, "day", splitTyQuery.ToString());
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                DataRow addDr = dtRst.Rows.Find(curDr["IsDefine"]);
                if (addDr == null) continue;
                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;
                addDr["Day"] = CommFunc.ConvertDBNullToDecimal(addDr["Day"]) + useVal;
            }

            dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), new DateTime(today1.Year, today1.Month, 1), new DateTime(today1.Year, today1.Month, 1), "month", splitTyQuery.ToString());
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                scale = scale == 0 ? 2 : scale;

                DataRow addDr = dtRst.Rows.Find(curDr["IsDefine"]);
                if (addDr == null) continue;
                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;
                useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

                addDr["Month"] = CommFunc.ConvertDBNullToDecimal(addDr["Month"]) + useVal;
            }
            dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), new DateTime(today1.Year, 1, 1), new DateTime(today1.Year, 12, 1), "month", splitTyQuery.ToString());
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                scale = scale == 0 ? 2 : scale;

                DataRow addDr = dtRst.Rows.Find(curDr["IsDefine"]);
                if (addDr == null) continue;
                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;
                useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

                addDr["Year"] = CommFunc.ConvertDBNullToDecimal(addDr["Year"]) + useVal;
            }
            var res1 = from s1 in dtRst.AsEnumerable()
                       select new
                       {
                           Id = CommFunc.ConvertDBNullToInt32(s1["IsDefine"]),
                           Day = CommFunc.ConvertDBNullToDecimal(s1["Day"]),
                           Month = CommFunc.ConvertDBNullToDecimal(s1["Month"]),
                           Year = CommFunc.ConvertDBNullToDecimal(s1["Year"]),
                           Unit = CommFunc.ConvertDBNullToString(s1["Unit"]),
                           ClassName = CommFunc.GetEnumDisplay(typeof(MmDefine), CommFunc.ConvertDBNullToInt32(s1["IsDefine"]))
                       };
            return res1.ToList();
        }

        /// <summary>
        /// 获取分项用能数据
        /// </summary>     
        /// <returns></returns>
        public object GetDayForEnergy(int isDefine)
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("EnergyItemCode", typeof(System.String));
            dtRst.Columns.Add("UseVal", typeof(System.Decimal));
            dtRst.Columns.Add("Unit", typeof(System.String));
            dtRst.Columns.Add("EnergyItemName", typeof(System.String));            
            //
            DataTable dtSource = dal.GetDayForEnergy(isDefine);
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["EnergyItemCode"] };
            StringBuilder splitMdQuery = new StringBuilder();
            StringBuilder splitTyQuery = new StringBuilder();
            string mmName = "";
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

                if (!string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(dr["EnergyItemCode"])))
                {
                    DataRow addDr = dtRst.Rows.Find(dr["EnergyItemCode"]);
                    if (addDr == null)
                        addDr = dtRst.Rows.Add(new object[] { dr["EnergyItemCode"], 0, dr["Unit"], dr["EnergyItemName"] });
                }
                mmName = CommFunc.ConvertDBNullToString(dr["MmName"]);
            }
            DateTime today2 = DateTime.Now; DateTime today1 = new DateTime(today2.Year, today2.Month, today2.Day);
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), today1, today2, "day", splitTyQuery.ToString());
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                scale = scale == 0 ? 2 : scale;

                DataRow addDr = dtRst.Rows.Find(curDr["EnergyItemCode"]);
                if (addDr == null) continue;
                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;
                useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

                addDr["UseVal"] = CommFunc.ConvertDBNullToDecimal(addDr["UseVal"]) + useVal;
            }
            if (dtRst.Rows.Count == 0)
            {
                dtRst.Rows.Add(new object[] { isDefine.ToString(), 0, "", mmName });
            }
            var res1 = from s1 in dtRst.AsEnumerable()
                       select new
                       {
                           EnergyItemCode = CommFunc.ConvertDBNullToString(s1["EnergyItemCode"]),
                           UseVal = CommFunc.ConvertDBNullToDecimal(s1["UseVal"]),
                           Unit = CommFunc.ConvertDBNullToString(s1["Unit"]),
                           EnergyItemName = CommFunc.ConvertDBNullToString(s1["EnergyItemName"]),
                       };
            return res1.ToList();
        }

        /// <summary>
        /// 获取支路信息
        /// </summary>
        /// <returns></returns>
        public object GetTree()
        {
            List<Treeview> rst = new List<Treeview>();
            DataTable dtPro = dal.GetProList();
            DataTable dtSource = dal.GetBuildList();
            foreach (DataRow dr in dtPro.Rows)
            {
                Treeview pro = new Treeview();
                int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
                pro.id = co_id.ToString();
                pro.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                pro.attributes = null;
                rst.Add(pro);
                this.GetTree(ref pro, ref dtSource, ref co_id);
            }
            //foreach (DataRow dr in dtSource.Select("IsAdd==0"))
            //{
            //    dr["IsAdd"] = 1;
            //    int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
            //    Treeview tb = new Treeview();
            //    tb.id = co_id.ToString();
            //    tb.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
            //    tb.attributes = new
            //    {
            //        Id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]),
            //        BuildName = CommFunc.ConvertDBNullToString(dr["CoName"]),
            //        BuildAddr = CommFunc.ConvertDBNullToString(dr["CustAddr"]),
            //        TelNo = CommFunc.ConvertDBNullToString(dr["Mobile"]),
            //        Email = CommFunc.ConvertDBNullToString(dr["Email"]),
            //        Parent_id = CommFunc.ConvertDBNullToInt32(dr["Parent_id"]),
            //    };
            //    rst.Add(tb);
            //    this.GetTree(ref tb, ref dtSource, ref co_id);
            //}
            return rst;
        }
        private void GetTree(ref Treeview list, ref DataTable dtSource, ref int parent_id)
        {
            if (list == null) list = new Treeview();
            DataRow[] arr = dtSource.Select("Parent_id=" + parent_id);
            if (arr.Count() > 0)
                list.nodes = new List<Treeview>();
            foreach (DataRow dr in arr)
            {
                dr["IsAdd"] = 1;
                int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
                Treeview tb = new Treeview();
                tb.id = co_id.ToString();
                tb.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                tb.attributes = new
                {
                    Id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]),
                    BuildName = CommFunc.ConvertDBNullToString(dr["CoName"]),
                    BuildAddr = CommFunc.ConvertDBNullToString(dr["CustAddr"]),
                    TelNo = CommFunc.ConvertDBNullToString(dr["Mobile"]),
                    Email = CommFunc.ConvertDBNullToString(dr["Email"]),
                    Parent_id = CommFunc.ConvertDBNullToInt32(dr["Parent_id"]),
                };
                list.nodes.Add(tb);
                this.GetTree(ref tb, ref dtSource, ref co_id);
            }
        }

    }
}
