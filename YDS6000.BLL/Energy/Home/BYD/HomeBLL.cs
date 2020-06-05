using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Configuration;
using YDS6000.Models;

namespace YDS6000.BLL.Energy.Home
{
    public partial class HomeBLL
    {
        private static DataTable DT_SOURCE = null;
        private DataTable DT_USE_HOUR=null;
        private DataTable DT_USE_DAY=null;
        private DataTable DT_USE_TODAY=null;
        private DataTable DT_MONITOR = null;
        private static StringBuilder splitMdQuery = new StringBuilder();
        private static StringBuilder splitTyQuery = new StringBuilder();
        private static string lpFileName = AppDomain.CurrentDomain.BaseDirectory + "BYD.ini";
        static string KT1 = IniHepler.GetConfig(lpFileName, "DATA", "KT1");//一楼空调
        static string JX2 = IniHepler.GetConfig(lpFileName, "DATA", "JX2");//二楼进线
        static string KT2 = IniHepler.GetConfig(lpFileName, "DATA", "KT2");//二楼空调
        static string JX3 = IniHepler.GetConfig(lpFileName, "DATA", "JX3");//三楼进线
        static string JX3_1 = IniHepler.GetConfig(lpFileName, "DATA", "JX3_1");//三楼进线1(东总进线)
        static string JX3_2 = IniHepler.GetConfig(lpFileName, "DATA", "JX3_2");//三楼进线2(西总进线)
        static string CJ3 = IniHepler.GetConfig(lpFileName, "DATA", "CJ3");//三楼车间
        static string CJ3_1 = IniHepler.GetConfig(lpFileName, "DATA", "CJ3_1");//三楼车间1(东办公室)
        static string CJ3_2 = IniHepler.GetConfig(lpFileName, "DATA", "CJ3_2");//三楼车间2-10
        static string LH3_1 = IniHepler.GetConfig(lpFileName, "DATA", "LH3_1");//三楼老化一期
        static string LH3_2 = IniHepler.GetConfig(lpFileName, "DATA", "LH3_2");//三楼老化二期
        static string KT3_1 = IniHepler.GetConfig(lpFileName, "DATA", "KT3_1");//三楼空调1(西空调房)
        static string CJ5 = IniHepler.GetConfig(lpFileName, "DATA", "CJ5");//# 五楼车间
        static string KT5 = IniHepler.GetConfig(lpFileName, "DATA", "KT5");//五楼空调
        static string JX5 = IniHepler.GetConfig(lpFileName, "DATA", "JX5");//五楼进线
        static string JX5_1 = IniHepler.GetConfig(lpFileName, "DATA", "JX5_1");//五楼进线1(东5301总进线)
        static string JX5_2 = IniHepler.GetConfig(lpFileName, "DATA", "JX5_2");//五楼进线2(东5101总进线)
        static string CJ5_1 = IniHepler.GetConfig(lpFileName, "DATA", "CJ5_1");//五楼车间8-9
        static string CJ5_2 = IniHepler.GetConfig(lpFileName, "DATA", "CJ5_2");//五楼车间10-14
        static string CJ5_3 = IniHepler.GetConfig(lpFileName, "DATA", "CJ5_3");//五楼车间1-7、15-23
        static string CJ6 = IniHepler.GetConfig(lpFileName, "DATA", "CJ6");//C1栋2楼车间
        static string KT6 = IniHepler.GetConfig(lpFileName, "DATA", "KT6");//C1栋2楼空调
        //private int Ledger = 0;
        //private int SysUid = 0;
        //private readonly YDS6000.DAL.Energy.Home.HomeDAL dal = null;
        //public HomeBLL(int _ledger, int _uid)
        //{
        //    this.Ledger = _ledger;
        //    this.SysUid = _uid;
        //    dal = new YDS6000.DAL.Energy.Home.HomeDAL(_ledger, _uid);
        //}

        ///// <summary>
        ///// 获取楼房当日用能曲线数据
        ///// 返回参数
        ///// 第一个是当天的小时用能
        ///// 第二个是昨日的小时用能
        ///// 第三个当天的用能总和
        ///// 最后是同期比
        ///// </summary>
        ///// <param name="co_id">楼房ID号</param>
        ///// <returns></returns>
        //public object GetDayEnergyUseValForBuild(int co_id)
        //{
        //    DataTable DT_SOURCE = dal.GetEnergyUseVal(co_id, 1);
        //    DT_SOURCE.PrimaryKey = new DataColumn[] { DT_SOURCE.Columns["Module_id"], DT_SOURCE.Columns["Fun_id"] };
        //    StringBuilder splitMdQuery = new StringBuilder();
        //    StringBuilder splitTyQuery = new StringBuilder();
        //    foreach (DataRow dr in DT_SOURCE.Rows)
        //    {
        //        if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
        //            splitMdQuery.Append(",");
        //        splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
        //        if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitTyQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["FunType"]), ",")))
        //        {
        //            if (!string.IsNullOrEmpty(splitTyQuery.ToString()))
        //                splitTyQuery.Append(",");
        //            splitTyQuery.Append(CommFunc.ConvertDBNullToString(dr["FunType"]));
        //        }
        //    }
        //    DateTime today2 = DateTime.Now.AddHours(-1); DateTime today1 = new DateTime(today2.Year, today2.Month, today2.Day);
        //    DateTime yesterday1 = today1.AddDays(-1); DateTime yesterday2 = new DateTime(yesterday1.Year, yesterday1.Month, yesterday1.Day, 23, 59, 0);
        //    DataTable dtUse1 = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), today1, today2, "hour", splitTyQuery.ToString());
        //    DataTable dtUse2 = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), yesterday1, yesterday2, "hour", splitTyQuery.ToString());
        //    List<decimal> todayList = new List<decimal>();
        //    List<decimal> ysdayList = new List<decimal>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //    int nn = today2.Hour;
        //    while (nn-- >= 0)
        //    {
        //        todayList.Add(0);
        //    }
        //    decimal todayUse = 0, yesterdayUse = 0;
        //    foreach (DataRow dr in dtUse1.Rows)
        //    {
        //        DataRow curDr = DT_SOURCE.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
        //        if (curDr == null) continue;
        //        int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
        //        decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
        //        scale = scale == 0 ? 2 : scale;

        //        DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
        //        decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
        //        decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
        //        decimal useVal = lastVal - firstVal;
        //        useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

        //        todayList[tagTime.Hour] = CommFunc.ConvertDBNullToDecimal(todayList[tagTime.Hour]) + useVal;
        //        todayUse = todayUse + useVal;
        //    }
        //    foreach (DataRow dr in dtUse2.Rows)
        //    {
        //        DataRow curDr = DT_SOURCE.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
        //        if (curDr == null) continue;
        //        int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
        //        decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
        //        scale = scale == 0 ? 2 : scale;

        //        DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
        //        decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
        //        decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
        //        decimal useVal = lastVal - firstVal;
        //        useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

        //        ysdayList[tagTime.Hour] = CommFunc.ConvertDBNullToDecimal(ysdayList[tagTime.Hour]) + useVal;
        //        if (tagTime.Hour <= today2.Hour)
        //            yesterdayUse = yesterdayUse + useVal;
        //    }
        //    decimal tongBiZeng = Math.Round((yesterdayUse == 0 ? 0 : ((todayUse - yesterdayUse) / yesterdayUse)) * 100, 2, MidpointRounding.AwayFromZero);
        //    return new { todayList = todayList, ysdayList = ysdayList, todayUse = todayUse, tongBiZeng = tongBiZeng.ToString("f2") + "%" };
        //}
        /// <summary>
        /// 获取全部回路
        /// </summary>
        private void GetModuleData()
        {
            DT_SOURCE = dal.GetEnergyUseVal(1);
            DT_SOURCE.PrimaryKey = new DataColumn[] { DT_SOURCE.Columns["Module_id"], DT_SOURCE.Columns["Fun_id"] };

            foreach (DataRow dr in DT_SOURCE.Rows)
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
        }
        /// <summary>
        /// 获取全部回路小时数据
        /// </summary>
        private void GetHourData(DateTime date)
        {
            if(DT_SOURCE==null)  GetModuleData();
            DateTime today1, today2;
            today1 = date;
            today2 = today1.AddHours(71);
            DT_USE_HOUR=WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), today1, today2, "hour", splitTyQuery.ToString());
        }
        /// <summary>
        /// 获取全部回路当月日数据
        /// </summary>
        private void GetDayData()
        {
            if(DT_SOURCE==null) GetModuleData();
            DateTime start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime end = DateTime.Today;
            DT_USE_DAY = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), start, end, "day", splitTyQuery.ToString());
        }

        /// <summary>
        /// 获取全部回路当天日数据
        /// </summary>
        private void GetTodayData()
        {
            if(DT_SOURCE==null) GetModuleData();
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Today;
            DT_USE_TODAY = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), start, end, "day", splitTyQuery.ToString());            
        }
        /// <summary>
        /// 获取用能小时数据
        /// </summary>
        /// <param name="Module_ids">回路</param>
        /// <returns></returns>
        private Dictionary<DateTime, decimal> GetDayEnergyUseValList(string Module_ids,DateTime date)
        {
            DataRow[] rows = DT_USE_HOUR.Select(string.Format("Module_id in({0})",Module_ids));
            Dictionary<DateTime, decimal> dic = new Dictionary<DateTime, decimal>();
            for(int i=0;i<72;i++)
            {
                dic.Add(date.AddHours(i), 0);
            }
            //List<decimal> todayList = new List<decimal>();
            //int nn = DateTime.Now.Hour;
            //while (nn-- >= 0)
            //{
            //    todayList.Add(0);
            //}
            foreach (DataRow dr in rows)
            {
                DataRow curDr = DT_SOURCE.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                scale = scale == 0 ? 2 : scale;

                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;
                useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

                if (dic.ContainsKey(tagTime)) dic[tagTime] = CommFunc.ConvertDBNullToDecimal(dic[tagTime]) + useVal;

                //todayList[tagTime.Hour] = CommFunc.ConvertDBNullToDecimal(todayList[tagTime.Hour]) + useVal;
            }
            return dic;
        }

        ///// <summary>
        ///// 获取C6栋当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id">楼房ID号</param>
        ///// <returns></returns>
        //public object GetMonthEnergyUseValForBuild(int co_id)
        //{
        //    DataTable DT_SOURCE = dal.GetEnergyUseVal(co_id, 1);
        //    DT_SOURCE.PrimaryKey = new DataColumn[] { DT_SOURCE.Columns["Module_id"], DT_SOURCE.Columns["Fun_id"] };
        //    StringBuilder splitMdQuery = new StringBuilder();
        //    StringBuilder splitTyQuery = new StringBuilder();
        //    foreach (DataRow dr in DT_SOURCE.Rows)
        //    {
        //        if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
        //            splitMdQuery.Append(",");
        //        splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
        //        if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitTyQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["FunType"]), ",")))
        //        {
        //            if (!string.IsNullOrEmpty(splitTyQuery.ToString()))
        //                splitTyQuery.Append(",");
        //            splitTyQuery.Append(CommFunc.ConvertDBNullToString(dr["FunType"]));
        //        }
        //    }
        //    DateTime start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        //    DateTime end = DateTime.Today;
        //    DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), start, end, "day", splitTyQuery.ToString());
        //    List<decimal> dataList = new List<decimal>();
        //    int nn = end.Day;
        //    while (nn-- >= 0)
        //    {
        //        dataList.Add(0);
        //    }
        //    foreach (DataRow dr in dtUse.Rows)
        //    {
        //        DataRow curDr = DT_SOURCE.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
        //        if (curDr == null) continue;
        //        int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
        //        decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
        //        scale = scale == 0 ? 2 : scale;

        //        DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
        //        decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
        //        decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
        //        decimal useVal = lastVal - firstVal;
        //        useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

        //        dataList[tagTime.Day-1] = CommFunc.ConvertDBNullToDecimal(dataList[tagTime.Day-1]) + useVal;
        //    }

        //    return new { dataList = dataList };
        //}


        ///// <summary>
        ///// 获取C6栋每个楼层当日总用能饼状图数据
        ///// </summary>
        ///// <param name="co_id">楼房ID号</param>
        ///// <returns></returns>
        //public object GetDayEnergyUseValForFloor(int co_id)
        //{
        //    DataTable dtCo = dal.GetChildCos(co_id);
        //    List<object> dataList = new List<object>();
        //    foreach (DataRow drCo in dtCo.Rows)
        //    {
        //        DataTable DT_SOURCE = dal.GetEnergyUseVal(CommFunc.ConvertDBNullToInt32(drCo["Co_id"]));
        //        DT_SOURCE.PrimaryKey = new DataColumn[] { DT_SOURCE.Columns["Module_id"], DT_SOURCE.Columns["Fun_id"] };
        //        StringBuilder splitMdQuery = new StringBuilder();
        //        StringBuilder splitTyQuery = new StringBuilder();
        //        foreach (DataRow dr in DT_SOURCE.Rows)
        //        {
        //            if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
        //                splitMdQuery.Append(",");
        //            splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
        //            if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitTyQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["FunType"]), ",")))
        //            {
        //                if (!string.IsNullOrEmpty(splitTyQuery.ToString()))
        //                    splitTyQuery.Append(",");
        //                splitTyQuery.Append(CommFunc.ConvertDBNullToString(dr["FunType"]));
        //            }
        //        }
        //        DateTime start = DateTime.Today;
        //        DateTime end = DateTime.Today;
        //        DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), start, end, "day", splitTyQuery.ToString());
        //        decimal totalVal = 0;
        //        foreach (DataRow dr in dtUse.Rows)
        //        {
        //            DataRow curDr = DT_SOURCE.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
        //            if (curDr == null) continue;
        //            int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
        //            decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
        //            scale = scale == 0 ? 2 : scale;

        //            DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
        //            decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
        //            decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
        //            decimal useVal = lastVal - firstVal;
        //            useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

        //            totalVal = totalVal + useVal;
        //        }
        //        dataList.Add(new { Co_id = CommFunc.ConvertDBNullToInt32(drCo["Co_id"]), CoName = CommFunc.ConvertDBNullToString(drCo["CoName"]), totalVal = totalVal });
        //    }

        //    return new { dataList = dataList };
        //}


        ///// <summary>
        ///// 获取当日用能曲线数据
        ///// 返回参数
        ///// 第一个是当天的小时用能
        ///// 第二个是昨日的小时用能
        ///// 第三个当天的用能总和
        ///// 第四个昨天的用能总和
        ///// </summary>
        ///// <param name="co_id">co_id</param>
        ///// <returns></returns>
        //public void GetDayEnergyUseVal(int co_id,out List<decimal> todayList,out List<decimal> ysdayList,out decimal todayUse,out decimal yesterdayUse,int? Attrib=null,string EnergyItemCode=null)
        //{
        //    DataTable DT_SOURCE = dal.GetEnergyUseVal(co_id, Attrib, EnergyItemCode);
        //    DT_SOURCE.PrimaryKey = new DataColumn[] { DT_SOURCE.Columns["Module_id"], DT_SOURCE.Columns["Fun_id"] };
        //    StringBuilder splitMdQuery = new StringBuilder();
        //    StringBuilder splitTyQuery = new StringBuilder();
        //    foreach (DataRow dr in DT_SOURCE.Rows)
        //    {
        //        if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
        //            splitMdQuery.Append(",");
        //        splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
        //        if (!System.Text.RegularExpressions.Regex.IsMatch(string.Format("{0}{1}{2}", ",", splitTyQuery.ToString(), ","), string.Format("{0}{1}{2}", ",", CommFunc.ConvertDBNullToString(dr["FunType"]), ",")))
        //        {
        //            if (!string.IsNullOrEmpty(splitTyQuery.ToString()))
        //                splitTyQuery.Append(",");
        //            splitTyQuery.Append(CommFunc.ConvertDBNullToString(dr["FunType"]));
        //        }
        //    }
        //    DateTime today2 = DateTime.Now.AddHours(-1); DateTime today1 = new DateTime(today2.Year, today2.Month, today2.Day);
        //    DateTime yesterday1 = today1.AddDays(-1); DateTime yesterday2 = new DateTime(yesterday1.Year, yesterday1.Month, yesterday1.Day, 23, 59, 0);
        //    DataTable dtUse1 = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), today1, today2, "hour", splitTyQuery.ToString());
        //    DataTable dtUse2 = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), yesterday1, yesterday2, "hour", splitTyQuery.ToString());
        //    todayList = new List<decimal>();
        //    ysdayList = new List<decimal>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //    int nn = today2.Hour;
        //    while (nn-- >= 0)
        //    {
        //        todayList.Add(0);
        //    }
        //    todayUse = 0;
        //    yesterdayUse = 0;
        //    foreach (DataRow dr in dtUse1.Rows)
        //    {
        //        DataRow curDr = DT_SOURCE.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
        //        if (curDr == null) continue;
        //        int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
        //        decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
        //        scale = scale == 0 ? 2 : scale;

        //        DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
        //        decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
        //        decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
        //        decimal useVal = lastVal - firstVal;
        //        useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

        //        todayList[tagTime.Hour] = CommFunc.ConvertDBNullToDecimal(todayList[tagTime.Hour]) + useVal;
        //        todayUse = todayUse + useVal;
        //    }
        //    foreach (DataRow dr in dtUse2.Rows)
        //    {
        //        DataRow curDr = DT_SOURCE.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
        //        if (curDr == null) continue;
        //        int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
        //        decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
        //        scale = scale == 0 ? 2 : scale;

        //        DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
        //        decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
        //        decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
        //        decimal useVal = lastVal - firstVal;
        //        useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

        //        ysdayList[tagTime.Hour] = CommFunc.ConvertDBNullToDecimal(ysdayList[tagTime.Hour]) + useVal;
        //        if (tagTime.Hour <= today2.Hour)
        //            yesterdayUse = yesterdayUse + useVal;
        //    }
        //}


        /// <summary>
        /// 获取当日用能曲线数据
        /// </summary>
        /// <returns></returns>
        public object GetDayEnergyUseVal(DateTime date)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);

            GetHourData(date);

            Dictionary<DateTime,decimal> listKT1 = GetDayEnergyUseValList(KT1,date);//一楼空调

            Dictionary<DateTime,decimal> listJX2 = GetDayEnergyUseValList(JX2,date);//二楼进线
            Dictionary<DateTime,decimal> listKT2 = GetDayEnergyUseValList(KT2,date);//二楼空调
            Dictionary<DateTime,decimal> listCJ2 = new Dictionary<DateTime,decimal>();//二楼车间
            foreach(var kv in listJX2)
            {
                listCJ2.Add(kv.Key, listJX2[kv.Key] - listKT2[kv.Key]);
            }
            //for (int i = 0; i < listJX2.Count; i++)
            //{
            //    listCJ2.Add(listJX2[i] - listKT2[i]);
            //}

            Dictionary<DateTime,decimal> listJX3_1 = GetDayEnergyUseValList(JX3_1,date);//三楼进线1(东总进线)
            Dictionary<DateTime,decimal> listJX3_2 = GetDayEnergyUseValList(JX3_2,date);//三楼进线2(西总进线)
            Dictionary<DateTime,decimal> listCJ3 = GetDayEnergyUseValList(CJ3,date);//三楼车间
            Dictionary<DateTime,decimal> listCJ3_1 = GetDayEnergyUseValList(CJ3_1,date);//三楼车间1(东办公室)
            Dictionary<DateTime,decimal> listKT3 = new Dictionary<DateTime,decimal>();//三楼空调
            Dictionary<DateTime,decimal> listKT3_1 = GetDayEnergyUseValList(KT3_1,date);//三楼空调1(西空调房)
            Dictionary<DateTime,decimal> listLH3_1 = GetDayEnergyUseValList(LH3_1,date);//三楼老化一期
            Dictionary<DateTime,decimal> listLH3_2 = GetDayEnergyUseValList(LH3_2,date);//三楼老化二期
            foreach (var kv in listJX3_2)
            {
                listCJ3[kv.Key] = listCJ3[kv.Key] + listJX3_2[kv.Key] - listKT3_1[kv.Key];
            }
            foreach (var kv in listJX3_1)
            {
                listKT3.Add(kv.Key, listJX3_1[kv.Key] - listCJ3_1[kv.Key] + listKT3_1[kv.Key]);
            }
            //for (int i = 0; i < listCJ3.Count; i++)
            //{
            //    listCJ3[i] = listCJ3[i] + listJX3_2[i] - listKT3_1[i];
            //}
            //for (int i = 0; i < listJX3_1.Count; i++)
            //{
            //    listKT3.Add(listJX3_1[i] - listCJ3_1[i] + listKT3_1[i]);
            //}

            Dictionary<DateTime,decimal> listCJ5 = GetDayEnergyUseValList(CJ5,date);//五楼车间
            Dictionary<DateTime,decimal> listKT5 = GetDayEnergyUseValList(KT5,date);//五楼空调
            Dictionary<DateTime,decimal> listJX5_1 = GetDayEnergyUseValList(JX5_1,date);//五楼进线1(东5301总进线)
            Dictionary<DateTime,decimal> listJX5_2 = GetDayEnergyUseValList(JX5_2,date);//五楼进线2(东5101总进线)
            Dictionary<DateTime,decimal> listCJ5_1 = GetDayEnergyUseValList(CJ5_1,date);//五楼车间8-9
            Dictionary<DateTime,decimal> listCJ5_2 = GetDayEnergyUseValList(CJ5_2,date);//五楼车间10-14
            foreach (var kv in listJX5_1)
            {
                listKT5[kv.Key] = listKT5[kv.Key] + listJX5_1[kv.Key] + listJX5_2[kv.Key] - listCJ5_1[kv.Key] - listCJ5_2[kv.Key];
            }
            //for (int i = 0; i < listKT5.Count; i++)
            //{
            //    listKT5[i] = listKT5[i] + listJX5_1[i] + listJX5_2[i] - listCJ5_1[i] - listCJ5_2[i];
            //}

            Dictionary<DateTime,decimal> listCJ6 = GetDayEnergyUseValList(CJ6,date);//C1栋2楼车间
            Dictionary<DateTime,decimal> listKT6 = GetDayEnergyUseValList(KT6,date);//C1栋2楼空调

            Dictionary<DateTime,decimal> listCJ = new Dictionary<DateTime,decimal>();//全部车间
            Dictionary<DateTime,decimal> listKT = new Dictionary<DateTime,decimal>();//全部空调

            foreach(var kv in listCJ5)
            {
                listCJ.Add(kv.Key, listCJ2[kv.Key] + listCJ3[kv.Key] + listCJ5[kv.Key] + listCJ6[kv.Key]);
            }
            foreach(var kv in listKT5)
            {
                listKT.Add(kv.Key, listKT1[kv.Key] + listKT2[kv.Key] + listKT3[kv.Key] + listKT5[kv.Key] + listKT6[kv.Key]);
            }
            //for(int i=0;i<listCJ5.Count;i++)
            //{
            //    listCJ.Add(listCJ2[i] + listCJ3[i] + listCJ5[i] + listCJ6[i]);
            //}
            //for(int i=0;i<listKT5.Count;i++)
            //{
            //    listKT.Add(listKT1[i] + listKT2[i] + listKT3[i] + listKT5[i] + listKT6[i]);
            //}




            //当日总用能
            //decimal totalCJ=0;
            //decimal totalKT=0;
            //decimal totalKT1=0;
            //decimal totalCJ2=0;
            //decimal totalKT2=0;
            //decimal totalCJ3=0;
            //decimal totalLH3_1=0;
            //decimal totalLH3_2=0;
            //decimal totalKT3=0;
            //decimal totalCJ5=0;
            //decimal totalKT5=0;
            //decimal totalCJ6=0;
            //decimal totalKT6=0;
            //for (int i = 0; i < listCJ.Count; i++)
            //{
            //    totalCJ += listCJ[i];
            //    totalKT += listKT[i];
            //    totalKT1 += listKT1[i];
            //    totalCJ2 += listCJ2[i];
            //    totalKT2 += listKT2[i];
            //    totalCJ3 += listCJ3[i];
            //    totalKT3 += listKT3[i];
            //    totalLH3_1 += listLH3_1[i];
            //    totalLH3_2 += listLH3_2[i];
            //    totalCJ5 += listCJ5[i];
            //    totalKT5 += listKT5[i];
            //    totalCJ6 += listCJ6[i];
            //    totalKT6 += listKT6[i];
            //}

            //listCJ.RemoveAt(listCJ.Count - 1);
            //listKT.RemoveAt(listKT.Count - 1);
            //listKT1.RemoveAt(listKT1.Count - 1);
            //listCJ2.RemoveAt(listCJ2.Count - 1);
            //listKT2.RemoveAt(listKT2.Count - 1);
            //listCJ3.RemoveAt(listCJ3.Count - 1);
            //listKT3.RemoveAt(listKT3.Count - 1);
            //listCJ5.RemoveAt(listCJ5.Count - 1);
            //listKT5.RemoveAt(listKT5.Count - 1);
            //listCJ6.RemoveAt(listCJ6.Count - 1);
            //listKT6.RemoveAt(listKT6.Count - 1);

            return new
            {
                listCJ = listCJ.ToList(),//全部车间
                listKT = listKT.ToList(),//全部空调
                listKT1 = listKT1.ToList(),//一楼空调
                listCJ2 = listCJ2.ToList(),//二楼车间
                listKT2 = listKT2.ToList(),//二楼空调
                listCJ3 = listCJ3.ToList(),//三楼车间
                listKT3 = listKT3.ToList(),//三楼空调
                listCJ5 = listCJ5.ToList(),//五楼车间
                listKT5 = listKT5.ToList(),//五楼空调
                listCJ6 = listCJ6.ToList(),//C1栋2楼车间
                listKT6 = listKT6.ToList(),//C1栋2楼空调
                //totalCJ = totalCJ,
                //totalKT = totalKT,
                //totalKT1 = totalKT1,
                //totalCJ2 = totalCJ2,
                //totalKT2 = totalKT2,
                //totalCJ3 = totalCJ3,
                //totalKT3 = totalKT3,
                //totalLH3_1=totalLH3_1,
                //totalLH3_2=totalLH3_2,
                //totalCJ5 = totalCJ5,
                //totalKT5 = totalKT5,
                //totalCJ6 = totalCJ6,
                //totalKT6 = totalKT6
            };
        }
        ///// <summary>
        ///// 获取C1栋2层当日用能曲线数据
        ///// </summary>
        ///// <param name="co_id">楼层的ID号</param>
        ///// <returns></returns>
        //public object GetDayEnergyUseValForC1_2(int co_id)
        //{
        //    List<decimal> listCJ = GetDayEnergyUseValList(co_id, null, "01C00");//二楼车间
        //    List<decimal> listKT = GetDayEnergyUseValList(co_id, null, "01B00");//二楼空调
        //    return new
        //    {
        //        listCJ = listCJ,//二楼车间
        //        listKT = listKT//二楼空调
        //    };
        //}

        ///// <summary>
        ///// 获取一楼当日用能曲线数据
        ///// 返回参数
        ///// 空调小时用能
        ///// </summary>
        ///// <param name="co_id">一楼的ID号</param>
        ///// <returns></returns>
        //public object GetDayEnergyUseValForFloor1(int co_id)
        //{
        //    //List<decimal> listCJ = GetDayEnergyUseVal(co_id, null, "01C00");//一楼车间 一楼没车间
        //    List<decimal> listKT = GetDayEnergyUseVal(co_id, null, "01B00");//一楼空调
        //    return new { listKT = listKT };
        //}
        ///// <summary>
        ///// 获取二楼当日用能曲线数据
        ///// 返回参数
        ///// 第一个是车间的小时用能
        ///// 第二个是空调的小时用能
        ///// </summary>
        ///// <param name="co_id">二楼的ID号</param>
        ///// <returns></returns>
        //public object GetDayEnergyUseValForFloor2(int co_id)
        //{
        //    List<decimal> listJX = GetDayEnergyUseVal(co_id, 1);//二楼进线
        //    List<decimal> listKT = GetDayEnergyUseVal(co_id, null, "01B00");//二楼空调
        //    List<decimal> listCJ = new List<decimal>();//二楼车间
        //    for (int i = 0; i < listJX.Count; i++)
        //    {
        //        listCJ.Add(listJX[i] - listKT[i]);
        //    }
        //    return new { listCJ = listCJ, listKT = listKT };
        //}
        ///// <summary>
        ///// 获取三楼当日用能曲线数据
        ///// 返回参数
        ///// 第一个是车间的小时用能
        ///// 第二个是空调的小时用能
        ///// </summary>
        ///// <param name="co_id">三楼的ID号</param>
        ///// <returns></returns>
        //public object GetDayEnergyUseValForFloor3(int co_id)
        //{
        //    List<decimal> listJX = GetDayEnergyUseVal(co_id, 1);//三楼进线
        //    List<decimal> listCJ = GetDayEnergyUseVal(co_id, null, "01C00");//三楼车间
        //    List<decimal> listKT = new List<decimal>();//三楼空调
        //    for (int i = 0; i < listJX.Count; i++)
        //    {
        //        listKT.Add(listJX[i] - listCJ[i]);
        //    }
        //    return new { listCJ = listCJ, listKT = listKT };
        //}
        ///// <summary>
        ///// 获取五楼当日用能曲线数据
        ///// 返回参数
        ///// 第一个是车间的小时用能
        ///// 第二个是空调的小时用能
        ///// </summary>
        ///// <param name="co_id">五楼的ID号</param>
        ///// <returns></returns>
        //public object GetDayEnergyUseValForFloor5(int co_id)
        //{
        //    List<decimal> listCJ = GetDayEnergyUseVal(co_id, null, "01C00");//五楼车间
        //    List<decimal> listKT = GetDayEnergyUseVal(co_id, null, "01B00");//五楼空调
        //    return new { listCJ = listCJ, listKT = listKT };
        //}


        /// <summary>
        /// 获取当月用能柱状图数据
        /// </summary>
        /// <param name="Module_ids">回路</param>
        /// <returns></returns>
        private List<decimal> GetMonthEnergyUseVal(string Module_ids)
        {
            DataRow[] rows = DT_USE_DAY.Select(string.Format("Module_id in({0})",Module_ids));            
            List<decimal> dataList = new List<decimal>();
            int nn = DateTime.Today.Day;
            while (nn-- >= 0)
            {
                dataList.Add(0);
            }
            foreach (DataRow dr in rows)
            {
                DataRow curDr = DT_SOURCE.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                scale = scale == 0 ? 2 : scale;

                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;
                useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

                dataList[tagTime.Day-1] = CommFunc.ConvertDBNullToDecimal(dataList[tagTime.Day-1]) + useVal;
            }

            return dataList;
        }
        /// <summary>
        /// 获取当月用能柱状图数据
        /// </summary>
        /// <returns></returns>
        public object GetMonthEnergyUseVal()
        {
            GetDayData();
            List<decimal> dataListKT1 = GetMonthEnergyUseVal(KT1); //一楼空调

            List<decimal> dataList2 = GetMonthEnergyUseVal(JX2); //二楼进线

            List<decimal> dataListJX3 = GetMonthEnergyUseVal(JX3); //三楼进线
            List<decimal> dataListCJ3 = GetMonthEnergyUseVal(CJ3_2); //三楼车间2-10
            List<decimal> dataList3 = new List<decimal>();
            for(int i=0;i<dataListJX3.Count;i++)
            {
                dataList3.Add(dataListJX3[i] + dataListCJ3[i]);
            }

            List<decimal> dataListJX5 = GetMonthEnergyUseVal(JX5); //五楼进线
            List<decimal> dataListCJ5 = GetMonthEnergyUseVal(CJ5_3); //五楼车间1-7、15-23
            List<decimal> dataListKT5 = GetMonthEnergyUseVal(KT5); //五楼空调
            List<decimal> dataList5 = new List<decimal>();
            for(int i=0;i<dataListJX5.Count;i++)
            {
                dataList5.Add(dataListJX5[i] + dataListCJ5[i] + dataListKT5[i]);
            }

            List<decimal> dataList6 = GetMonthEnergyUseVal(string.Format("{0},{1}",CJ6,KT6)); //C1栋2楼
             

            List<decimal> dataList = new List<decimal>();//总用能
            for(int i=0;i<dataListKT1.Count;i++)
            {
                dataList.Add(dataListKT1[i] + dataList2[i] + dataList3[i] + dataList5[i] + dataList6[i]);
            }

            return new
            {
                dataList = dataList,
                dataList1 = dataListKT1,
                dataList2 = dataList2,
                dataList3 = dataList3,
                dataList5 = dataList5,
                dataList6 = dataList6
            };
        }
        /// <summary>
        /// 获取当日总用能饼状图数据
        /// </summary>
        /// <param name="Module_ids">回路</param>
        /// <returns></returns>
        public decimal GetDayEnergyUseValTotal(string Module_ids)
        {
            DataRow[] rows = DT_USE_TODAY.Select(string.Format("Module_id in({0})",Module_ids));
            decimal totalVal = 0;
            foreach (DataRow dr in rows)
            {
                DataRow curDr = DT_SOURCE.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                scale = scale == 0 ? 2 : scale;

                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;
                useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);

                totalVal = totalVal + useVal;
            }

            return totalVal;
        }
        /// <summary>
        /// 获取当日总用能饼状图数据
        /// </summary>
        /// <returns></returns>
        public object GetDayEnergyUseValForPie()
        {
            GetTodayData();

            List<object> dataList = new List<object>();
            decimal useVal1 = GetDayEnergyUseValTotal(KT1); //一楼空调

            decimal useVal2 = GetDayEnergyUseValTotal(JX2); //二楼进线

            decimal useValJX3 = GetDayEnergyUseValTotal(JX3); //三楼进线
            decimal useValCJ3 = GetDayEnergyUseValTotal(CJ3_2); //三楼车间2-10
            decimal useVal3 = useValJX3 + useValCJ3;

            decimal useValJX5 = GetDayEnergyUseValTotal(JX5); //五楼进线
            decimal useValCJ5 = GetDayEnergyUseValTotal(CJ5_3); //五楼车间1-7、15-23
            decimal useValKT5 = GetDayEnergyUseValTotal(KT5); //五楼空调
            decimal useVal5 = useValJX5 + useValCJ5 + useValKT5;

            decimal useVal6 = GetDayEnergyUseValTotal(string.Format("{0},{1}",CJ6,KT6)); //C1栋2楼

            decimal useVal = useVal1 + useVal2 + useVal3 + useVal5 + useVal6;//五厂总计

            dataList.Add(new { Co_id = 0, CoName = "五厂总计", totalVal = useVal });
            dataList.Add(new { Co_id = 2, CoName = "C6-1F", totalVal = useVal1 });
            dataList.Add(new { Co_id = 3, CoName = "C6-2F", totalVal = useVal2 });
            dataList.Add(new { Co_id = 4, CoName = "C6-3F", totalVal = useVal3 });
            dataList.Add(new { Co_id = 5, CoName = "C6-5F", totalVal = useVal5 });
            dataList.Add(new { Co_id = 6, CoName = "C1-2F", totalVal = useVal6 });

            return new
            {
                dataList = dataList
            };            
        }
        /// <summary>
        /// 获取当天用能实时数据
        /// </summary>
        /// <returns></returns>
        public object GetDayEnergyUseValForReal()
        {
            GetTodayData();

            decimal totalKT1 = GetDayEnergyUseValTotal(KT1);//一楼空调

            decimal totalJX2 = GetDayEnergyUseValTotal(JX2);//二楼进线
            decimal totalKT2 = GetDayEnergyUseValTotal(KT2);//二楼空调
            decimal totalCJ2 = totalJX2 - totalKT2;//二楼车间

            decimal totalJX3_1 = GetDayEnergyUseValTotal(JX3_1);//三楼进线1(东总进线)
            decimal totalJX3_2 = GetDayEnergyUseValTotal(JX3_2);//三楼进线2(西总进线)
            decimal totalCJ3 = GetDayEnergyUseValTotal(CJ3);//三楼车间
            decimal totalCJ3_1 = GetDayEnergyUseValTotal(CJ3_1);//三楼车间1(东办公室)
            decimal totalKT3_1 = GetDayEnergyUseValTotal(KT3_1);//三楼空调1(西空调房)
            decimal totalLH3_1 = GetDayEnergyUseValTotal(LH3_1);//三楼老化一期
            decimal totalLH3_2 = GetDayEnergyUseValTotal(LH3_2);//三楼老化二期
            totalCJ3 = totalCJ3 + totalJX3_2 - totalKT3_1;
            decimal totalKT3 = totalJX3_1 - totalCJ3_1 + totalKT3_1;//三楼空调

            decimal totalCJ5 = GetDayEnergyUseValTotal(CJ5);//五楼车间
            decimal totalKT5 = GetDayEnergyUseValTotal(KT5);//五楼空调
            decimal totalJX5_1 = GetDayEnergyUseValTotal(JX5_1);//五楼进线1(东5301总进线)
            decimal totalJX5_2 = GetDayEnergyUseValTotal(JX5_2);//五楼进线2(东5101总进线)
            decimal totalCJ5_1 = GetDayEnergyUseValTotal(CJ5_1);//五楼车间8-9
            decimal totalCJ5_2 = GetDayEnergyUseValTotal(CJ5_2);//五楼车间10-14
            totalKT5 = totalKT5 + totalJX5_1 + totalJX5_2 - totalCJ5_1 - totalCJ5_2;

            decimal totalCJ6 = GetDayEnergyUseValTotal(CJ6);//C1栋2楼车间
            decimal totalKT6 = GetDayEnergyUseValTotal(KT6);//C1栋2楼空调

            decimal totalCJ = totalCJ2 + totalCJ3 + totalCJ5 + totalCJ6;//全部车间
            decimal totalKT = totalKT1 + totalKT2 + totalKT3 + totalKT5 + totalKT6;//全部空调
            
            return new
            {
                totalCJ = totalCJ,
                totalKT = totalKT,
                totalKT1 = totalKT1,
                totalCJ2 = totalCJ2,
                totalKT2 = totalKT2,
                totalCJ3 = totalCJ3,
                totalKT3 = totalKT3,
                totalLH3_1 = totalLH3_1,
                totalLH3_2 = totalLH3_2,
                totalCJ5 = totalCJ5,
                totalKT5 = totalKT5,
                totalCJ6 = totalCJ6,
                totalKT6 = totalKT6
            };
        
        }
        /// <summary>
        /// 获取全部回路监测信息
        /// </summary>
        private void GetMonitor()
        {
            DT_MONITOR = dal.GetMonitorList(1);
        }

        /// <summar y>
        /// 获取监测数据信息
        /// </summary>
        /// <param name="co_id">co_id</param>
        /// <returns></returns>
        public DataTable GetMonitorList(string Module_ids)
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("Module_id", typeof(System.Int32));
            dtRst.Columns.Add("ModuleName", typeof(System.String));
            dtRst.Columns.Add("E_Tag", typeof(System.String));
            dtRst.Columns.Add("P_Tag", typeof(System.String));
            dtRst.Columns.Add("E_Unit", typeof(System.String));
            dtRst.Columns.Add("P_Unit", typeof(System.String));
            dtRst.Columns.Add("FrMd", typeof(System.Int32));
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["Module_id"] };
            DataRow[] rows = DT_MONITOR.Select(string.Format("Module_id in({0})", Module_ids));
            foreach (DataRow dr in rows)
            {
                int frMd = CommFunc.ConvertDBNullToInt32(dr["FrMd"]);
                frMd = frMd == 0 ? 15 : frMd;
                DataRow addDr = dtRst.Rows.Find(dr["Module_id"]);
                if (addDr == null)
                {
                    addDr = dtRst.NewRow();
                    addDr["Module_id"] = dr["Module_id"];
                    addDr["ModuleName"] = dr["ModuleName"];
                    addDr["FrMd"] = dr["FrMd"];
                    dtRst.Rows.Add(addDr);
                }
                addDr[CommFunc.ConvertDBNullToString(dr["FunType"]) + "_Tag"] = CommFunc.ConvertDBNullToString(dr["LpszDbVarName"]);
                addDr[CommFunc.ConvertDBNullToString(dr["FunType"]) + "_Unit"] = CommFunc.ConvertDBNullToString(dr["Unit"]);

            }
            return dtRst;
        }
        public void GetMonitorData(out DataTable dtKT1,out DataTable dtJX2,out DataTable dtKT2,out DataTable dtJX3_1,out DataTable dtJX3_2,out DataTable dtCJ3,out DataTable dtCJ3_1,out DataTable dtKT3_1,out DataTable dtLH3_1,out DataTable dtLH3_2,out DataTable dtCJ5,out DataTable dtKT5,out DataTable dtJX5_1,out DataTable dtJX5_2,out DataTable dtCJ5_1,out DataTable dtCJ5_2,out DataTable dtCJ6,out DataTable dtKT6)
        {
            GetMonitor();

            dtKT1 = GetMonitorList(KT1);//一楼空调

            dtJX2 = GetMonitorList(JX2);//二楼进线
            dtKT2 = GetMonitorList(KT2);//二楼空调

            dtJX3_1 = GetMonitorList(JX3_1);//三楼进线1(东总进线)
            dtJX3_2 = GetMonitorList(JX3_2);//三楼进线2(西总进线)
            dtCJ3 = GetMonitorList(CJ3);//三楼车间
            dtCJ3_1 = GetMonitorList(CJ3_1);//三楼车间1(东办公室)
            dtKT3_1 = GetMonitorList(KT3_1);//三楼空调1(西空调房)
            dtLH3_1 = GetMonitorList(LH3_1);//三楼老化一期
            dtLH3_2 = GetMonitorList(LH3_2);//三楼老化二期

            dtCJ5 = GetMonitorList(CJ5);//五楼车间
            dtKT5 = GetMonitorList(KT5);//五楼空调
            dtJX5_1 = GetMonitorList(JX5_1);//五楼进线1(东5301总进线)
            dtJX5_2 = GetMonitorList(JX5_2);//五楼进线2(东5101总进线)
            dtCJ5_1 = GetMonitorList(CJ5_1);//五楼车间8-9
            dtCJ5_2 = GetMonitorList(CJ5_2);//五楼车间10-14

            dtCJ6 = GetMonitorList(CJ6);//C1栋2楼车间
            dtKT6 = GetMonitorList(KT6);//C1栋2楼空调
        }

        ///// <summary>
        ///// 获取C6栋二楼当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id">二楼的ID号</param>
        ///// <returns></returns>
        //public object GetMonthEnergyUseValForFloor2(int co_id)
        //{
        //    List<decimal> dataList = GetMonthEnergyUseVal(co_id, 1); //二楼进线
            
        //    return new { dataList = dataList };
        //}
        ///// <summary>
        ///// 获取C6栋三楼当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id">楼层的ID号</param>
        ///// <returns></returns>
        //public object GetMonthEnergyUseValForFloor3(int co_id)
        //{
        //    List<decimal> dataListJX = GetMonthEnergyUseVal(co_id, 1); //三楼进线
        //    List<decimal> dataListCJ = GetMonthEnergyUseVal(co_id, null, "01C00","25,26,27,28,29,30"); //三楼车间2-7
        //    List<decimal> dataList = new List<decimal>();
        //    for(int i=0;i<dataListJX.Count;i++)
        //    {
        //        dataList.Add(dataListJX[i] + dataListCJ[i]);
        //    }
        //    return new { dataList = dataList };
        //}
        ///// <summary>
        ///// 获取C6栋五楼当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id">楼层的ID号</param>
        ///// <returns></returns>
        //public object GetMonthEnergyUseValForFloor5(int co_id)
        //{
        //    List<decimal> dataListJX = GetMonthEnergyUseVal(co_id, 1); //五楼进线
        //    List<decimal> dataListCJ = GetMonthEnergyUseVal(co_id, null, "01C00","34,35,36,37,38,39,40,50,51,52,53,54,55,56,57,58"); //五楼车间1-7、15-23
        //    List<decimal> dataList = new List<decimal>();
        //    for(int i=0;i<dataListJX.Count;i++)
        //    {
        //        dataList.Add(dataListJX[i] + dataListCJ[i]);
        //    }
        //    return new { dataList = dataList };
        //}
        ///// <summary>
        ///// 获取C1栋2层当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id">楼层的ID号</param>
        ///// <returns></returns>
        //public object GetMonthEnergyUseValForC1_2(int co_id)
        //{
        //    List<decimal> dataListCJ = GetMonthEnergyUseVal(co_id, null, "01C00"); //二楼车间
        //    List<decimal> dataListKT = GetMonthEnergyUseVal(co_id, null, "01B00"); //二楼空调        
        //    List<decimal> dataList = new List<decimal>();
        //    for(int i=0;i<dataListCJ.Count;i++)
        //    {
        //        dataList.Add(dataListCJ[i] + dataListKT[i]);
        //    }
        //    return new { dataList = dataList };
        //}

    }
}
