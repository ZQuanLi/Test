using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YDS6000.Models.Tables;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 运行报表-采集历史报表
    /// </summary>
    [RoutePrefix("api/Exp/RunReport")]
    public class ExpYdCollectController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.RunReport.ExpYdCollectHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.RunReport.ExpYdCollectHelper();

        /// <summary>
        /// 获取采集历史报表
        /// </summary>
        /// <param name="StrcName">建筑名称</param>
        /// <param name="CoName">房间名称</param>
        /// <param name="FunType">电参量:E=电量,Ia=A相电流,Ib=B相电流,Ic=C相电流,Ua=A相电压,Ub=B相电压,Uc=C相电压,Psum=总有功功率,Pa=A相有功功率,Pb=B相有功功率,Pc=C相有功功率,Qsum=总无功功率,Qa=A相无功功率,Qb=B相无功功率,Qc=C相无功功率,Pfav=总功率因数,PFa=A相功率因数,PFb=B相功率因数,PFc=C相功率因数</param>
        /// <param name="IsDefine">属性:0=全部,10=商户,20=商铺,30=宿舍,40=厂房</param>
        /// <param name="StartTime">日期开始</param>
        /// <param name="EndTime">日期结束</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdCollectData")]
        public APIRst GetYdCollectData(string StrcName, string CoName, string FunType, int IsDefine, DateTime StartTime, DateTime EndTime)
        {
            return infoHelper.GetYdCollectData(StrcName, CoName, FunType, IsDefine, StartTime, EndTime);
        }

        /// <summary>
        /// 汇出Excel
        /// </summary>
        /// <param name="Data">对象：传输Json格式的数据,转格式var data = JSON.stringify(this.list);</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("GetYdCollectOnExport")]
        public APIRst GetYdCollectOnExport(DataModels Data)
        {
            return infoHelper.GetYdCollectOnExport(Data);
        }
    }
}