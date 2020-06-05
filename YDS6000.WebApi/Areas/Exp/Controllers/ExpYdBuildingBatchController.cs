using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 工程配置-设备批量操作
    /// </summary>
    [RoutePrefix("api/Exp/Syscont")]
    public class ExpYdBuildingBatchController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpYdBuildingBatchHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpYdBuildingBatchHelper();

        /// <summary>
        /// 导出采集器模板(导出Excel模板)
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("ExportBuilding")]
        public APIRst ExportBuilding()
        {
            return infoHelper.ExportBuilding();
        }

        /// <summary>
        /// 批量上载采集器Excel数据
        /// </summary>
        /// <returns>web文件上传，webapi后台接收https://blog.csdn.net/qq503690160/article/details/54380114</returns>
        [HttpPost, HttpOptions]
        [Route("ToLeadBuilding")]
        public APIRst ToLeadBuilding()
        {
            return infoHelper.ToLeadBuilding();
            //return null;
        }
    }
}