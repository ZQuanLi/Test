using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YDS6000.Models;
using YDS6000.Models.Tables;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 系统配置-客户管理
    /// </summary>
    [RoutePrefix("api/Exp/Syscont")]
    public class ExpYdCustomerController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpYdCustomerHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpYdCustomerHelper();

        /// <summary>
        /// 获取客户信息列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="CName">名称</param>
        /// <param name="CNum">证件号</param>
        /// <param name="RoomName">房间名</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetCustomerList")]
        public APIRst GetCustomerList(int pageIndex, int pageSize, string CName, string CNum, string RoomName)
        {
            return infoHelper.GetCustomerList(pageIndex, pageSize, CName, CNum, RoomName);
        }

        /// <summary>
        /// 获取首页用户数（月数and总数）
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetHomeCustomer")]
        public APIRst GetHomeCustomer()
        {
            return infoHelper.GetHomeCustomer();
        }

        /// <summary>
        /// 设置客户信息
        /// </summary>
        /// <param name="nAct">nAct：新增=1,修改=2,删除=3</param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetCustomer")]
        public APIRst SetCustomer(int nAct, v3_userVModel model)
        {
            return infoHelper.SetCustomer(nAct, model);
        }

        /// <summary>
        /// 获取建筑房间信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="BuildName"></param>
        /// <param name="RoomName">房间名</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetRoomInfo")]
        public APIRst GetRoomInfo(int pageIndex, int pageSize, string BuildName, string RoomName)
        {
            return infoHelper.GetRoomInfo(pageIndex, pageSize, BuildName, RoomName);
        }

        /// <summary>
        /// 汇出Excel
        /// </summary>
        /// <param name="Data">对象：传输Json格式的数据,转格式var data = JSON.stringify(this.list);</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("GetYdCustomerOnExport")]
        public APIRst GetYdCustomerOnExport(DataModels Data)
        {
            return infoHelper.GetYdCustomerOnExport(Data);
        }

        /// <summary>
        /// 获取入驻房号-已选项
        /// </summary>
        /// <param name="Crm_id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdCustomerRoomInfo")]
        public APIRst GetYdCustomerRoomInfo(int Crm_id)
        {
            return infoHelper.GetYdCustomerRoomInfo(Crm_id);
        }

        /// <summary>
        /// 获取入驻房号-可选项
        /// </summary>
        /// <param name="Crm_id"></param>
        /// <param name="CoName">建筑名称</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdCustomerRoomFind")]
        public APIRst GetYdCustomerRoomFind(int Crm_id, string CoName)
        {
            return infoHelper.GetYdCustomerRoomFind(Crm_id, CoName);
        }

        /// <summary>
        /// 保存入驻房号
        /// </summary>
        /// <param name="Crm_id"></param>
        /// <param name="CoIdStr">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SaveYdCustomerOpenRoom")]
        public APIRst SaveYdCustomerOpenRoom(int Crm_id, string CoIdStr)
        {
            return infoHelper.SaveYdCustomerOpenRoom(Crm_id, CoIdStr);
        }

        /// <summary>
        /// 保存安全密码
        /// </summary>
        /// <param name="Crm_id"></param>
        /// <param name="Passwd">密码</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SaveYdCustomerOpenWd")]
        public APIRst SaveYdCustomerOpenWd(int Crm_id, string Passwd)
        {
            return infoHelper.SaveYdCustomerOpenWd(Crm_id, Passwd);
        }


    }
}