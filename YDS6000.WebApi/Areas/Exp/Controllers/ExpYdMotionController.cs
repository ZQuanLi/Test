using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YDS6000.Models.Tables;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 运行报表-运行历史报表
    /// </summary>
    [RoutePrefix("api/Exp/RunReport")]
    public class ExpYdMotionController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.RunReport.ExpYdMotionHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.RunReport.ExpYdMotionHelper();

        /// <summary>
        /// 获取运行历史报表
        /// </summary>
        /// <param name="StrcName">建筑名称</param>
        /// <param name="CoName">房间名称</param>
        /// <param name="DateType">报表类型</param>
        /// <param name="StartTime">日期开始</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdMontionOnList")]
        public APIRst GetYdMontionOnList(string StrcName, string CoName,string DateType, DateTime StartTime)
        {
            return infoHelper.GetYdMontionOnList(StrcName, CoName, DateType, StartTime);
        }

        /// <summary>
        /// 汇出Excel
        /// </summary>
        /// <param name="Data">对象：传输Json格式的数据,转格式var data = JSON.stringify(this.list);</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("GetYdMontionOnExport")]
        public APIRst GetYdMontionOnExport(DataModels Data)
        {
            return infoHelper.GetYdMontionOnExport(Data);
        }

        /// <summary>
        /// 区域下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetAearCombox")]
        public APIRst GetAearCombox()
        {
            return infoHelper.GetAearCombox();
        }
        /// <summary>
        /// 单位下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetUnitCombox")]
        public APIRst GetUnitCombox()
        {
            return infoHelper.GetUnitCombox();
        }
        /// <summary>
        /// 项目下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetProjectCombox")]
        public APIRst GetProjectCombox()
        {
            return infoHelper.GetProjectCombox();
        }

        /// <summary>
        /// 用电单元下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetCellCombox")]
        public APIRst GetCellCombox()
        {
            return infoHelper.GetCellCombox();
        }
        /// <summary>
        /// 回路下拉列表
        /// </summary>
        /// <param name="id">用电单元ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetModuleCombox")]
        public APIRst GetModuleCombox(int id)
        {
            return infoHelper.GetModuleCombox(id);
        }
        /// <summary>
        /// 采集项下拉列表
        /// </summary>
        /// <param name="id">回路ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetFunCombox")]
        public APIRst GetFunCombox(int id)
        {
            return infoHelper.GetFunCombox(id);
        }
        /// <summary>
        /// 查询类型下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetDataTypeCombox")]
        public APIRst GetDataTypeCombox()
        {
            return infoHelper.GetDataTypeCombox();
        }

        /// <summary>
        /// 历史数据
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="funType">采集项目</param>
        /// <param name="dataType">查询类型</param>
        /// <param name="startTime">开始日期</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetHisData")]
        public APIRst GetHisData(int module_id, string funType, string dataType, DateTime startTime)
        {
            return infoHelper.GetHisData(module_id, funType, dataType, startTime);
        }
        /// <summary>
        /// 数据对比
        /// </summary>
        /// <param name="areaId">区域ID号</param>
        /// <param name="unitId">单元ID号</param>
        /// <param name="projectId">项目ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetCompare")]
        public APIRst GetCompare(int areaId, int unitId, int projectId)
        {
            return infoHelper.GetCompare(areaId, unitId, projectId);
        }
    }
}