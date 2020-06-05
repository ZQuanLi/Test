using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Energy.Controllers
{
    public partial class HomeHelper
    {
        //private CacheUser user = null;
        //private YDS6000.BLL.Energy.Home.HomeBLL bll = null;
        //public HomeHelper()
        //{
        //    user = WebConfig.GetSession();
        //    bll = new YDS6000.BLL.Energy.Home.HomeBLL(user.Ledger, user.Uid);
        //}


        ///// <summary>
        ///// 获取楼房当日用能趋曲线数据
        ///// 第一个是当天的小时用能
        ///// 第二个是昨日的小时用能
        ///// 第三个当天的用能总和
        ///// 最后是同期比
        ///// </summary>
        ///// <param name="co_id">楼房ID号</param>
        ///// <returns></returns>
        //public APIRst GetDayEnergyUseValForBuild(int co_id)
        //{
        //    APIRst rst = new APIRst();
        //    try
        //    {
        //        rst.data = bll.GetDayEnergyUseValForBuild(co_id);
        //    }
        //    catch (Exception ex)
        //    {
        //        rst.rst = false;
        //        rst.err.code = (int)ResultCodeDefine.Error;
        //        rst.err.msg = ex.Message;
        //        FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
        //    }
        //    return rst;
        //}


        /// <summary>
        /// 获取当日用能曲线数据
        /// </summary>
        /// <returns></returns>
        public APIRst GetDayEnergyUseVal(DateTime date)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetDayEnergyUseVal(date);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        ///// <summary>
        ///// 获取C1栋2层当日用能曲线数据
        ///// </summary>
        ///// <param name="co_id">楼层的ID号</param>
        ///// <returns></returns>
        //public APIRst GetDayEnergyUseValForC1_2(int co_id)
        //{
        //    APIRst rst = new APIRst();
        //    try
        //    {
        //        rst.data = bll.GetDayEnergyUseValForC1_2(co_id);
        //    }
        //    catch (Exception ex)
        //    {
        //        rst.rst = false;
        //        rst.err.code = (int)ResultCodeDefine.Error;
        //        rst.err.msg = ex.Message;
        //        FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
        //    }
        //    return rst;
        //}
        /// <summary>
        /// 获取当月用能柱状图数据
        /// </summary>
        /// <returns></returns>
        public APIRst GetMonthEnergyUseVal()
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetMonthEnergyUseVal();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        ///// <summary>
        ///// 获取C6栋当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id">楼房ID号</param>
        ///// <returns></returns>
        //public APIRst GetMonthEnergyUseValForBuild(int co_id)
        //{
        //    APIRst rst = new APIRst();
        //    try
        //    {
        //        rst.data = bll.GetMonthEnergyUseValForBuild(co_id);
        //    }
        //    catch (Exception ex)
        //    {
        //        rst.rst = false;
        //        rst.err.code = (int)ResultCodeDefine.Error;
        //        rst.err.msg = ex.Message;
        //        FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
        //    }
        //    return rst;
        //}

        ///// <summary>
        ///// 获取监测数据信息
        ///// </summary>
        ///// <returns></returns>
        //public APIRst GetMonitorList(int co_id,int? Attrib=null)
        //{
        //    APIRst rst = new APIRst();
        //    try
        //    {
        //        //List<object> rb = new List<object>();
        //        DataTable dtSource = bll.GetMonitorList(co_id,Attrib);
        //        //foreach (DataRow s1 in dtSource.Rows)
        //        //{
        //        //    object list = new
        //        //    {
        //        //        E_Tag = new
        //        //        {
        //        //            name = "电能采集点",
        //        //            tag = CommFunc.ConvertDBNullToString(s1["E_Tag"]),
        //        //            unit = CommFunc.ConvertDBNullToString(s1["E_Unit"]),
        //        //        },
        //        //        Ua_Tag = new
        //        //        {
        //        //            name = "电压采集点",
        //        //            tag = CommFunc.ConvertDBNullToString(s1["U_Tag"]),
        //        //            unit = CommFunc.ConvertDBNullToString(s1["U_Unit"]),
        //        //        },
        //        //        Ia_Tag = new
        //        //        {
        //        //            name = "电流采集点",
        //        //            tag = CommFunc.ConvertDBNullToString(s1["I_Tag"]),
        //        //            unit = CommFunc.ConvertDBNullToString(s1["I_Unit"]),
        //        //        },
        //        //        Pa_Tag = new
        //        //        {
        //        //            name = "有功功率",
        //        //            tag = CommFunc.ConvertDBNullToString(s1["P_Tag"]),
        //        //            unit = CommFunc.ConvertDBNullToString(s1["P_Unit"]),
        //        //        },
        //        //    };
        //        //    object obj = new
        //        //    {
        //        //        RowId = dtSource.Rows.IndexOf(s1) + 1,
        //        //        Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
        //        //        ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
        //        //        Status = CommFunc.ConvertDBNullToString(s1["Status"]),
        //        //        FrMd = CommFunc.ConvertDBNullToInt32(s1["FrMd"]),
        //        //        List = list,
        //        //    };
        //        //    rb.Add(obj);
        //        //}
        //        var res1 = from s1 in dtSource.AsEnumerable()
        //                   select new
        //                   {
        //                       RowId = dtSource.Rows.IndexOf(s1) + 1,
        //                       Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
        //                       ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
        //                       E_Tag = CommFunc.ConvertDBNullToString(s1["E_Tag"]),
        //                       P_Tag = CommFunc.ConvertDBNullToString(s1["P_Tag"]),
        //                       E_Unit = CommFunc.ConvertDBNullToString(s1["E_Unit"]),
        //                       P_Unit = CommFunc.ConvertDBNullToString(s1["P_Unit"]),
        //                       FrMd = CommFunc.ConvertDBNullToInt32(s1["FrMd"]),
        //                   };
        //        rst.data = res1.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        rst.rst = false;
        //        rst.err.code = (int)ResultCodeDefine.Error;
        //        rst.err.msg = ex.Message;
        //        FileLog.WriteLog("获取监测数据信息错误:" + ex.Message + ex.StackTrace);
        //    }
        //    return rst;
        //}
        /// <summary>
        /// 获取监测数据信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetMonitorList()
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = GetMonitorData();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取监测数据信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        ///// <summary>
        ///// 获取C1栋2层监测数据信息
        ///// </summary>
        ///// <param name="co_id">二楼的ID号</param>
        ///// <returns></returns>
        //public APIRst GetMonitorListForC1_2(int co_id)
        //{
        //    APIRst rst = new APIRst();
        //    try
        //    {
        //        rst.data = GetMonitorDataForC1_2(co_id);
        //    }
        //    catch (Exception ex)
        //    {
        //        rst.rst = false;
        //        rst.err.code = (int)ResultCodeDefine.Error;
        //        rst.err.msg = ex.Message;
        //        FileLog.WriteLog("获取监测数据信息错误:" + ex.Message + ex.StackTrace);
        //    }
        //    return rst;
        //}

        //private object GetMonitorDataForC1_2(int co_id)
        //{
        //    DataTable dtCJ = bll.GetMonitorList(co_id, null, "01C00");//二楼车间
        //    DataTable dtKT = bll.GetMonitorList(co_id, null, "01B00");//二楼空调

        //    decimal totalCJ_P;//二楼车间功率
        //    decimal totalCJ_E;//二楼车间电能
        //    decimal totalKT_P;//二楼空调功率
        //    decimal totalKT_E;//二楼空调电能

        //    DateTime UTimeCJ;
        //    DateTime UTimeKT;

        //    GetRealVal(dtCJ, out totalCJ_P, out totalCJ_E, out UTimeCJ);
        //    GetRealVal(dtKT, out totalKT_P, out totalKT_E, out UTimeKT);

        //    string P_Unit = "";//功率单位
        //    string E_Unit = "";//电能单位
        //    if(dtCJ.Rows.Count>0)
        //    {
        //        P_Unit = dtCJ.Rows[0]["P_Unit"].ToString();
        //        E_Unit = dtCJ.Rows[0]["E_Unit"].ToString();
        //    }
        //    else if(dtKT.Rows.Count>0)
        //    {
        //        P_Unit = dtKT.Rows[0]["P_Unit"].ToString();
        //        E_Unit = dtKT.Rows[0]["E_Unit"].ToString();
        //    }

        //    return new
        //    {
        //        DataCJ = new {name="二楼车间",P_total=totalCJ_P,E_total=totalCJ_E,time=UTimeCJ},//二楼车间
        //        DataKT = new {name="二楼空调",P_total=totalKT_P,E_total=totalKT_E,time=UTimeKT},//二楼空调
        //        P_Unit=P_Unit,
        //        E_Unit=E_Unit
        //    };
        //}

        public object GetMonitorData()
        {
            DataTable dtKT1;//一楼空调

            DataTable dtJX2;//二楼进线
            DataTable dtKT2;//二楼空调

            DataTable dtJX3_1;//三楼进线1(东总进线)
            DataTable dtJX3_2;//三楼进线2(西总进线)
            DataTable dtCJ3;//三楼车间
            DataTable dtCJ3_1;//三楼车间1(东办公室)
            DataTable dtKT3_1;//三楼空调1(西空调房)
            DataTable dtLH3_1;//三楼老化一期
            DataTable dtLH3_2;//三楼老化二期

            DataTable dtCJ5;//五楼车间
            DataTable dtKT5;//五楼空调
            DataTable dtJX5_1;//五楼进线1(东5301总进线)
            DataTable dtJX5_2;//五楼进线2(东5101总进线)
            DataTable dtCJ5_1;//五楼车间8-9
            DataTable dtCJ5_2;//五楼车间10-14

            DataTable dtCJ6;//C1栋2楼车间
            DataTable dtKT6;//C1栋2楼空调

            bll.GetMonitorData(out dtKT1, out dtJX2, out dtKT2, out dtJX3_1, out dtJX3_2, out dtCJ3, out dtCJ3_1, out dtKT3_1, out dtLH3_1, out dtLH3_2, out dtCJ5, out dtKT5, out dtJX5_1, out

dtJX5_2, out dtCJ5_1, out dtCJ5_2, out dtCJ6, out dtKT6);

            decimal totalCJ_P;//全部车间功率
            decimal totalCJ_E;//全部车间电能
            decimal totalKT_P;//全部空调功率
            decimal totalKT_E;//全部空调电能

            decimal totalKT1_P;//一楼空调功率
            decimal totalKT1_E;//一楼空调电能

            decimal totalJX2_P;//二楼进线功率
            decimal totalJX2_E;//二楼进线电能
            decimal totalCJ2_P;//二楼车间功率
            decimal totalCJ2_E;//二楼车间电能
            decimal totalKT2_P;//二楼空调功率
            decimal totalKT2_E;//二楼空调电能

            decimal totalJX3_1_P;//三楼进线1功率
            decimal totalJX3_1_E;//三楼进线1电能
            decimal totalJX3_2_P;//三楼进线2功率
            decimal totalJX3_2_E;//三楼进线2电能
            decimal totalCJ3_P;//三楼车间功率
            decimal totalCJ3_E;//三楼车间电能
            decimal totalCJ3_1_P;//三楼车间1功率
            decimal totalCJ3_1_E;//三楼车间1电能
            decimal totalKT3_P;//三楼空调功率
            decimal totalKT3_E;//三楼空调电能
            decimal totalKT3_1_P;//三楼空调1功率
            decimal totalKT3_1_E;//三楼空调1电能
            decimal totalLH3_1_P;//三楼老化一期功率
            decimal totalLH3_1_E;//三楼老化一期电能
            decimal totalLH3_2_P;//三楼老化二期功率
            decimal totalLH3_2_E;//三楼老化二期电能

            decimal totalCJ5_P;//五楼车间功率
            decimal totalCJ5_E;//五楼车间电能
            decimal totalJX5_1_P;//五楼进线1功率
            decimal totalJX5_1_E;//五楼进线1电能
            decimal totalJX5_2_P;//五楼进线2功率
            decimal totalJX5_2_E;//五楼进线2电能
            decimal totalCJ5_1_P;//五楼车间8-9功率
            decimal totalCJ5_1_E;//五楼车间8-9电能
            decimal totalCJ5_2_P;//五楼车间10-14功率
            decimal totalCJ5_2_E;//五楼车间10-14电能
            decimal totalKT5_P;//五楼空调功率
            decimal totalKT5_E;//五楼空调电能

            decimal totalCJ6_P;//C1栋2楼车间功率
            decimal totalCJ6_E;//C1栋2楼车间电能
            decimal totalKT6_P;//C1栋2楼空调功率
            decimal totalKT6_E;//C1栋2楼空调电能

            DateTime UTimeKT1;//一楼空调更新时间
            DateTime UTimeJX2;
            DateTime UTimeKT2;
            DateTime UTimeJX3_1;
            DateTime UTimeJX3_2;
            DateTime UTimeCJ3;
            DateTime UTimeCJ3_1;
            DateTime UTimeKT3_1;
            DateTime UTimeLH3_1;
            DateTime UTimeLH3_2;
            DateTime UTimeCJ5;
            DateTime UTimeKT5;
            DateTime UTimeJX5_1;
            DateTime UTimeJX5_2;
            DateTime UTimeCJ5_1;
            DateTime UTimeCJ5_2;
            DateTime UTimeCJ6;
            DateTime UTimeKT6;

            GetRealVal(dtKT1, out totalKT1_P, out totalKT1_E, out UTimeKT1);
            GetRealVal(dtJX2, out totalJX2_P, out totalJX2_E, out UTimeJX2);
            GetRealVal(dtKT2, out totalKT2_P, out totalKT2_E, out UTimeKT2);
            GetRealVal(dtJX3_1, out totalJX3_1_P, out totalJX3_1_E, out UTimeJX3_1);
            GetRealVal(dtJX3_2, out totalJX3_2_P, out totalJX3_2_E, out UTimeJX3_2);
            GetRealVal(dtCJ3, out totalCJ3_P, out totalCJ3_E, out UTimeCJ3);
            GetRealVal(dtCJ3_1, out totalCJ3_1_P, out totalCJ3_1_E, out UTimeCJ3_1);
            GetRealVal(dtKT3_1, out totalKT3_1_P, out totalKT3_1_E, out UTimeKT3_1);
            GetRealVal(dtLH3_1, out totalLH3_1_P, out totalLH3_1_E, out UTimeLH3_1);
            GetRealVal(dtLH3_2, out totalLH3_2_P, out totalLH3_2_E, out UTimeLH3_2);
            GetRealVal(dtCJ5, out totalCJ5_P, out totalCJ5_E, out UTimeCJ5);
            GetRealVal(dtKT5, out totalKT5_P, out totalKT5_E, out UTimeKT5);
            GetRealVal(dtJX5_1, out totalJX5_1_P, out totalJX5_1_E, out UTimeJX5_1);
            GetRealVal(dtJX5_2, out totalJX5_2_P, out totalJX5_2_E, out UTimeJX5_2);
            GetRealVal(dtCJ5_1, out totalCJ5_1_P, out totalCJ5_1_E, out UTimeCJ5_1);
            GetRealVal(dtCJ5_2, out totalCJ5_2_P, out totalCJ5_2_E, out UTimeCJ5_2);
            GetRealVal(dtCJ6, out totalCJ6_P, out totalCJ6_E, out UTimeCJ6);
            GetRealVal(dtKT6, out totalKT6_P, out totalKT6_E, out UTimeKT6);

            totalCJ2_P = totalJX2_P - totalKT2_P;
            totalCJ2_E = totalJX2_E - totalKT2_E;
            totalCJ3_P = totalCJ3_P + totalJX3_2_P - totalKT3_1_P;
            totalCJ3_E = totalCJ3_E + totalJX3_2_E - totalKT3_1_E;
            totalKT3_P = totalJX3_1_P - totalCJ3_1_P + totalKT3_1_P;
            totalKT3_E = totalJX3_1_E - totalCJ3_1_E + totalKT3_1_E;
            totalKT5_P = totalKT5_P + totalJX5_1_P - totalCJ5_1_P + totalJX5_2_P - totalCJ5_2_P;
            totalKT5_E = totalKT5_E + totalJX5_1_E - totalCJ5_1_E + totalJX5_2_E - totalCJ5_2_E;
            totalCJ_P = totalCJ2_P + totalCJ3_P + totalCJ5_P + totalCJ6_P;
            totalCJ_E = totalCJ2_E + totalCJ3_E + totalCJ5_E + totalCJ6_E;
            totalKT_P = totalKT1_P + totalKT2_P + totalKT3_P + totalKT5_P + totalKT6_P;
            totalKT_E = totalKT1_E + totalKT2_E + totalKT3_E + totalKT5_E + totalKT6_E;

            string P_Unit = "";//功率单位
            string E_Unit = "";//电能单位
            if (dtKT1.Rows.Count > 0)
            {
                P_Unit = dtKT1.Rows[0]["P_Unit"].ToString();
                E_Unit = dtKT1.Rows[0]["E_Unit"].ToString();
            }
            else if (dtJX2.Rows.Count > 0)
            {
                P_Unit = dtJX2.Rows[0]["P_Unit"].ToString();
                E_Unit = dtJX2.Rows[0]["E_Unit"].ToString();
            }
            else if (dtKT2.Rows.Count > 0)
            {
                P_Unit = dtKT2.Rows[0]["P_Unit"].ToString();
                E_Unit = dtKT2.Rows[0]["E_Unit"].ToString();
            }
            else if (dtJX3_1.Rows.Count > 0)
            {
                P_Unit = dtJX3_1.Rows[0]["P_Unit"].ToString();
                E_Unit = dtJX3_1.Rows[0]["E_Unit"].ToString();
            }
            else if (dtJX3_2.Rows.Count > 0)
            {
                P_Unit = dtJX3_2.Rows[0]["P_Unit"].ToString();
                E_Unit = dtJX3_2.Rows[0]["E_Unit"].ToString();
            }
            else if (dtCJ3.Rows.Count > 0)
            {
                P_Unit = dtCJ3.Rows[0]["P_Unit"].ToString();
                E_Unit = dtCJ3.Rows[0]["E_Unit"].ToString();
            }
            else if (dtKT3_1.Rows.Count > 0)
            {
                P_Unit = dtKT3_1.Rows[0]["P_Unit"].ToString();
                E_Unit = dtKT3_1.Rows[0]["E_Unit"].ToString();
            }
            else if (dtCJ5.Rows.Count > 0)
            {
                P_Unit = dtCJ5.Rows[0]["P_Unit"].ToString();
                E_Unit = dtCJ5.Rows[0]["E_Unit"].ToString();
            }
            else if (dtKT5.Rows.Count > 0)
            {
                P_Unit = dtKT5.Rows[0]["P_Unit"].ToString();
                E_Unit = dtKT5.Rows[0]["E_Unit"].ToString();
            }
            else if (dtJX5_1.Rows.Count > 0)
            {
                P_Unit = dtJX5_1.Rows[0]["P_Unit"].ToString();
                E_Unit = dtJX5_1.Rows[0]["E_Unit"].ToString();
            }
            else if (dtJX5_2.Rows.Count > 0)
            {
                P_Unit = dtJX5_2.Rows[0]["P_Unit"].ToString();
                E_Unit = dtJX5_2.Rows[0]["E_Unit"].ToString();
            }
            else if (dtCJ6.Rows.Count > 0)
            {
                P_Unit = dtCJ6.Rows[0]["P_Unit"].ToString();
                E_Unit = dtCJ6.Rows[0]["E_Unit"].ToString();
            }
            else if (dtKT6.Rows.Count > 0)
            {
                P_Unit = dtKT6.Rows[0]["P_Unit"].ToString();
                E_Unit = dtKT6.Rows[0]["E_Unit"].ToString();
            }

            DateTime[] arrCJ = { UTimeJX2, UTimeCJ3, UTimeJX3_2, UTimeCJ5 };
            DateTime[] arrKT = { UTimeKT1, UTimeKT2, UTimeJX3_1, UTimeJX3_2, UTimeKT5, UTimeJX5_1, UTimeJX5_2 };
            DateTime[] arrKT5 = { UTimeJX5_1, UTimeJX5_2, UTimeKT5 };
            Array.Sort(arrCJ);
            Array.Sort(arrKT);
            Array.Sort(arrKT5);
            return new
            {
                DataCJ = new { name = "全部车间", P_total = totalCJ_P, E_total = totalCJ_E, time = arrCJ[arrCJ.Length - 1] },//整栋车间
                DataKT = new { name = "全部空调", P_total = totalKT_P, E_total = totalKT_E, time = arrKT[arrKT.Length - 1] },//整栋空调
                DataKT1 = new { name = "C6-1F空调", P_total = totalKT1_P, E_total = totalKT1_E, time = UTimeKT1 },//一楼空调
                DataCJ2 = new { name = "C6-2F车间", P_total = totalCJ2_P, E_total = totalCJ2_E, time = UTimeJX2 },//二楼车间
                DataKT2 = new { name = "C6-2F空调", P_total = totalKT2_P, E_total = totalKT2_E, time = UTimeKT2 },//二楼空调
                DataCJ3 = new { name = "C6-3F车间", P_total = totalCJ3_P, E_total = totalCJ3_E, time = UTimeCJ3 > UTimeJX3_2 ? UTimeCJ3 : UTimeJX3_2 },//三楼车间
                DataKT3 = new { name = "C6-3F空调", P_total = totalKT3_P, E_total = totalKT3_E, time = UTimeJX3_1 > UTimeJX3_2 ? UTimeJX3_1 : UTimeJX3_2 },//三楼空调
                DataLH3_1 = new { name = "C6-3F一期老化房", P_total = totalLH3_1_P, E_total = totalLH3_1_E, time = UTimeLH3_1 },//三楼老化一期
                DataLH3_2 = new { name = "C6-3F二期老化房", P_total = totalLH3_2_P, E_total = totalLH3_2_E, time = UTimeLH3_2 },//三楼老化二期
                DataCJ5 = new { name = "C6-5F车间", P_total = totalCJ5_P, E_total = totalCJ5_E, time = UTimeCJ5 },//五楼车间
                DataKT5 = new { name = "C6-5F空调", P_total = totalKT5_P, E_total = totalKT5_E, time = arrKT5[arrKT5.Length - 1] },//五楼空调
                DataCJ6 = new { name = "C1-2F车间", P_total = totalCJ6_P, E_total = totalCJ6_E, time = UTimeCJ6 },//C1栋2楼车间
                DataKT6 = new { name = "C1-2F空调", P_total = totalKT6_P, E_total = totalKT6_E, time = UTimeKT6 },//C1栋2楼空调
                P_Unit = P_Unit,
                E_Unit = E_Unit
            };
        }
        private void GetRealVal(DataTable dt, out decimal pTotal, out decimal eTotal, out DateTime UTime)
        {
            pTotal = 0;
            eTotal = 0;
            UTime = DateTime.MinValue;
            CacheUser user = WebConfig.GetSession();
            string key = (user == null || user.Uid == 0) ? "" : user.CacheKey;
            if (string.IsNullOrEmpty(key))
                key = WebConfig.MemcachKey;
            string ccKey = "";
            foreach (DataRow dr in dt.Rows)
            {
                ccKey = key + CommFunc.ConvertDBNullToString(dr["P_Tag"]);
                int i = 0;
                RstVar var = null;
                while (++i <= 2)
                {
                    var = MemcachedMgr.GetVal<RstVar>(ccKey);
                    if (var != null) break;
                    System.Threading.Thread.Sleep(50);
                }
                if (var != null)
                {
                    pTotal += CommFunc.ConvertDBNullToDecimal(var.lpszVal);
                    if (var.lpszdateTime > UTime) UTime = var.lpszdateTime;
                }
                //ccKey = key + CommFunc.ConvertDBNullToString(dr["E_Tag"]);
                //i = 0;
                //var = null;
                //while (++i <= 2)
                //{
                //    var = MemcachedMgr.GetVal<RstVar>(ccKey);
                //    if (var != null) break;
                //    System.Threading.Thread.Sleep(50);
                //}
                //if (var != null)
                //{
                //    eTotal += CommFunc.ConvertDBNullToDecimal(var.lpszVal);
                //    if (var.lpszdateTime > UTime) UTime = var.lpszdateTime;
                //}
            }
        }

        /// <summary>
        /// 获取当天用能实时数据
        /// </summary>
        /// <returns></returns>
        public APIRst GetDayEnergyUseValForReal()
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetDayEnergyUseValForReal();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取C6栋每个楼层当日总用能饼状图数据
        /// </summary>
        /// <returns></returns>
        public APIRst GetDayEnergyUseValForPie()
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetDayEnergyUseValForPie();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        ///// <summary>
        ///// 获取一楼当日用能曲线数据
        ///// 返回参数
        ///// 空调小时用能
        ///// </summary>
        ///// <param name="co_id">一楼的ID号</param>
        ///// <returns></returns>
        //public APIRst GetDayEnergyUseValForFloor1(int co_id)
        //{
        //    APIRst rst = new APIRst();
        //    try
        //    {
        //        rst.data = bll.GetDayEnergyUseValForFloor1(co_id);
        //    }
        //    catch (Exception ex)
        //    {
        //        rst.rst = false;
        //        rst.err.code = (int)ResultCodeDefine.Error;
        //        rst.err.msg = ex.Message;
        //        FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
        //    }
        //    return rst;
        //}
        ///// <summary>
        ///// 获取二楼当日用能曲线数据
        ///// 返回参数
        ///// 第一个是车间的小时用能
        ///// 第二个是空调的小时用能
        ///// </summary>
        ///// <param name="co_id">二楼的ID号</param>
        ///// <returns></returns>
        //public APIRst GetDayEnergyUseValForFloor2(int co_id)
        //{
        //    APIRst rst = new APIRst();
        //    try
        //    {
        //        rst.data = bll.GetDayEnergyUseValForFloor2(co_id);
        //    }
        //    catch (Exception ex)
        //    {
        //        rst.rst = false;
        //        rst.err.code = (int)ResultCodeDefine.Error;
        //        rst.err.msg = ex.Message;
        //        FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
        //    }
        //    return rst;
        //}
        ///// <summary>
        ///// 获取三楼当日用能曲线数据
        ///// 返回参数
        ///// 第一个是车间的小时用能
        ///// 第二个是空调的小时用能
        ///// </summary>
        ///// <param name="co_id">三楼的ID号</param>
        ///// <returns></returns>
        //public APIRst GetDayEnergyUseValForFloor3(int co_id)
        //{
        //    APIRst rst = new APIRst();
        //    try
        //    {
        //        rst.data = bll.GetDayEnergyUseValForFloor3(co_id);
        //    }
        //    catch (Exception ex)
        //    {
        //        rst.rst = false;
        //        rst.err.code = (int)ResultCodeDefine.Error;
        //        rst.err.msg = ex.Message;
        //        FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
        //    }
        //    return rst;
        //}
        ///// <summary>
        ///// 获取五楼当日用能曲线数据
        ///// 返回参数
        ///// 第一个是车间的小时用能
        ///// 第二个是空调的小时用能
        ///// </summary>
        ///// <param name="co_id">五楼的ID号</param>
        ///// <returns></returns>
        //public APIRst GetDayEnergyUseValForFloor5(int co_id)
        //{
        //    APIRst rst = new APIRst();
        //    try
        //    {
        //        rst.data = bll.GetDayEnergyUseValForFloor5(co_id);
        //    }
        //    catch (Exception ex)
        //    {
        //        rst.rst = false;
        //        rst.err.code = (int)ResultCodeDefine.Error;
        //        rst.err.msg = ex.Message;
        //        FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
        //    }
        //    return rst;
        //}


        ///// <summary>
        ///// 获取C6栋一楼当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id1">一楼的ID号</param>
        ///// <param name="co_id2">二楼的ID号</param>
        ///// <param name="co_id3">三楼的ID号</param>
        ///// <param name="co_id5">五楼的ID号</param>
        ///// <returns></returns>
        //public APIRst GetMonthEnergyUseValForFloor1(int co_id1,int co_id2,int co_id3,int co_id5)
        //{
        //    APIRst rst = new APIRst();
        //    try
        //    {
        //        rst.data = bll.GetMonthEnergyUseValForFloor1(co_id1,co_id2,co_id3,co_id5);
        //    }
        //    catch (Exception ex)
        //    {
        //        rst.rst = false;
        //        rst.err.code = (int)ResultCodeDefine.Error;
        //        rst.err.msg = ex.Message;
        //        FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
        //    }
        //    return rst;
        //}
        ///// <summary>
        ///// 获取C6栋二楼车间当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id">二楼的ID号</param>
        ///// <returns></returns>
        //public APIRst GetMonthEnergyUseValForFloor2(int co_id)
        //{
        //    APIRst rst = new APIRst();
        //    try
        //    {
        //        rst.data = bll.GetMonthEnergyUseValForFloor2(co_id);
        //    }
        //    catch (Exception ex)
        //    {
        //        rst.rst = false;
        //        rst.err.code = (int)ResultCodeDefine.Error;
        //        rst.err.msg = ex.Message;
        //        FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
        //    }
        //    return rst;
        //}
        ///// <summary>
        ///// 获取C6栋三楼（五楼）车间当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id">楼层的ID号</param>
        ///// <returns></returns>
        //public APIRst GetMonthEnergyUseValForFloor3(int co_id)
        //{
        //    APIRst rst = new APIRst();
        //    try
        //    {
        //        rst.data = bll.GetMonthEnergyUseValForFloor3(co_id);
        //    }
        //    catch (Exception ex)
        //    {
        //        rst.rst = false;
        //        rst.err.code = (int)ResultCodeDefine.Error;
        //        rst.err.msg = ex.Message;
        //        FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
        //    }
        //    return rst;
        //}
        ///// <summary>
        ///// 获取C1栋2层当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id">楼层的ID号</param>
        ///// <returns></returns>
        //public APIRst GetMonthEnergyUseValForC1_2(int co_id)
        //{
        //    APIRst rst = new APIRst();
        //    try
        //    {
        //        rst.data = bll.GetMonthEnergyUseValForC1_2(co_id);
        //    }
        //    catch (Exception ex)
        //    {
        //        rst.rst = false;
        //        rst.err.code = (int)ResultCodeDefine.Error;
        //        rst.err.msg = ex.Message;
        //        FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
        //    }
        //    return rst;
        //}

    }
}