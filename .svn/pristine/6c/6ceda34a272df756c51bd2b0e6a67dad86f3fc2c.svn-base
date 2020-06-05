using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace YDS6000.WebApi.Areas.Energy.Controllers
{
    /// <summary>
    /// 能源首页Api
    /// </summary>
    //[RoutePrefix("api/Energy/Home")]
    public partial class EngHomeController : ApiController
    {
        //private HomeHelper infoHelper = new HomeHelper();



        /// <summary>
        /// 获取当日用能曲线数据
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetDayEnergyUseVal")]
        public APIRst GetDayEnergyUseVal(DateTime? date=null)
        {
            if (!date.HasValue) date = DateTime.Today;
            return infoHelper.GetDayEnergyUseVal(date.Value);
        }
        /// <summary>
        /// 获取当月用能柱状图数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMonthEnergyUseVal")]
        public APIRst GetMonthEnergyUseVal()
        {
            return infoHelper.GetMonthEnergyUseVal();
        }
        ///// <summary>
        ///// 获取C6栋当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id">楼房ID号</param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetMonthEnergyUseValForBuild")]
        //public APIRst GetMonthEnergyUseValForBuild(int co_id)
        //{
        //    return infoHelper.GetMonthEnergyUseValForBuild(co_id);
        //}
        ///// <summary>
        ///// 获取楼房监测数据信息
        ///// </summary>
        ///// <param name="co_id">楼房ID号</param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetMonitorListForBuild")]
        //public APIRst GetMonitorListForBuild(int co_id)
        //{
        //    return infoHelper.GetMonitorList(co_id,1);
        //}
        ///// <summary>
        ///// 获取楼层监测数据信息
        ///// </summary>
        ///// <param name="co_id">楼层ID号</param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetMonitorListForFloor")]
        //public APIRst GetMonitorListForFloor(int co_id)
        //{
        //    return infoHelper.GetMonitorList(co_id);
        //}
        /// <summary>
        /// 获取监测数据信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetMonitorList")]
        public APIRst GetMonitorList()
        {
            return infoHelper.GetMonitorList();
        }
        /// <summary>
        /// 获取当天用能实时数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetDayEnergyUseValForReal")]
        public APIRst GetDayEnergyUseValForReal()
        {
            return infoHelper.GetDayEnergyUseValForReal();
        }
        /// <summary>
        /// 获取当日总用能饼状图数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetDayEnergyUseValForPie")]
        public APIRst GetDayEnergyUseValForPie()
        {
            return infoHelper.GetDayEnergyUseValForPie();
        }
        ///// <summary>
        ///// 获取一楼当日用能曲线数据
        ///// 返回参数
        ///// 空调小时用能
        ///// </summary>
        ///// <param name="co_id">一楼的ID号</param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetDayEnergyUseValForFloor1")]
        //public APIRst GetDayEnergyUseValForFloor1(int co_id)
        //{
        //    return infoHelper.GetDayEnergyUseValForFloor1(co_id);
        //}
        ///// <summary>
        ///// 获取二楼当日用能曲线数据
        ///// 返回参数
        ///// 第一个是车间的小时用能
        ///// 第二个是空调的小时用能
        ///// </summary>
        ///// <param name="co_id">二楼的ID号</param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetDayEnergyUseValForFloor2")]
        //public APIRst GetDayEnergyUseValForFloor2(int co_id)
        //{
        //    return infoHelper.GetDayEnergyUseValForFloor2(co_id);
        //}
        ///// <summary>
        ///// 获取三楼当日用能曲线数据
        ///// 返回参数
        ///// 第一个是车间的小时用能
        ///// 第二个是空调的小时用能
        ///// </summary>
        ///// <param name="co_id">三楼的ID号</param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetDayEnergyUseValForFloor3")]
        //public APIRst GetDayEnergyUseValForFloor3(int co_id)
        //{
        //    return infoHelper.GetDayEnergyUseValForFloor3(co_id);
        //}
        ///// <summary>
        ///// 获取五楼当日用能曲线数据
        ///// 返回参数
        ///// 第一个是车间的小时用能
        ///// 第二个是空调的小时用能
        ///// </summary>
        ///// <param name="co_id">五楼的ID号</param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetDayEnergyUseValForFloor5")]
        //public APIRst GetDayEnergyUseValForFloor5(int co_id)
        //{
        //    return infoHelper.GetDayEnergyUseValForFloor5(co_id);
        //}



        ///// <summary>
        ///// 获取C6栋一楼当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id1">一楼的ID号</param>
        ///// <param name="co_id2">二楼的ID号</param>
        ///// <param name="co_id3">三楼的ID号</param>
        ///// <param name="co_id5">五楼的ID号</param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetMonthEnergyUseValForFloor1")]
        //public APIRst GetMonthEnergyUseValForFloor1(int co_id1,int co_id2,int co_id3,int co_id5)
        //{
        //    return infoHelper.GetMonthEnergyUseValForFloor1(co_id1,co_id2,co_id3,co_id5);
        //}
        ///// <summary>
        ///// 获取C6栋二楼车间当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id">二楼的ID号</param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetMonthEnergyUseValForFloor2")]
        //public APIRst GetMonthEnergyUseValForFloor2(int co_id)
        //{
        //    return infoHelper.GetMonthEnergyUseValForFloor2(co_id);
        //}
        ///// <summary>
        ///// 获取C6栋三楼（五楼）车间当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id">楼层的ID号</param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetMonthEnergyUseValForFloor3")]
        //public APIRst GetMonthEnergyUseValForFloor3(int co_id)
        //{
        //    return infoHelper.GetMonthEnergyUseValForFloor3(co_id);
        //}
        ///// <summary>
        ///// 获取C1栋2层当日用能曲线数据
        ///// </summary>
        ///// <param name="co_id">楼层的ID号</param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetDayEnergyUseValForC1_2")]
        //public APIRst GetDayEnergyUseValForC1_2(int co_id)
        //{
        //    return infoHelper.GetDayEnergyUseValForC1_2(co_id);
        //}
        ///// <summary>
        ///// 获取C1栋2层当月用能柱状图数据
        ///// </summary>
        ///// <param name="co_id">楼层的ID号</param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetMonthEnergyUseValForC1_2")]
        //public APIRst GetMonthEnergyUseValForC1_2(int co_id)
        //{
        //    return infoHelper.GetMonthEnergyUseValForC1_2(co_id);
        //}
        ///// <summary>
        ///// 获取C1栋2层监测数据信息
        ///// </summary>
        ///// <param name="co_id">楼层的ID号</param>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("GetMonitorListForC1_2")]
        //public APIRst GetMonitorListForC1_2(int co_id)
        //{
        //    return infoHelper.GetMonitorListForC1_2(co_id);
        //}
        //[HttpGet, HttpOptions]
        //[Route("GetTestList")]
        //public APIRst GetTestList()
        //{
        //    APIRst rst = new APIRst();
        //    List<decimal> list = new List<decimal>();
        //    list.Add(12);
        //    list.Add(34);
        //    rst.data = list;
        //    return rst;
        //}
        
    }
}
