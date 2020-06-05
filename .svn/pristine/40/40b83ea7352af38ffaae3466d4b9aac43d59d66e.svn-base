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
    /// 系统配置-设备型号
    /// </summary>
    [RoutePrefix("api/Exp/Syscont")]
    public class ExpModelsController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpModelsHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpModelsHelper();

        /// <summary>
        /// 获取设备型号
        /// </summary>
        /// <param name="ModuleType">筛选条件：设备类型</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdModelsOnList")]
        public APIRst GetYdModelsOnList(string ModuleType)
        {
            return infoHelper.GetYdModelsOnList(ModuleType);
        }

        /// <summary>
        /// 获取修改设备型号信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdModelsOfEditOnModelList")]
        public APIRst GetYdModelsOfEditOnModelList()
        {
            return infoHelper.GetYdModelsOfEditOnModelList();
        }

        /// <summary>
        /// 获取采集信息
        /// </summary>
        /// <param name="mm_id">类型ID号</param>
        /// <param name="ModuleType">设备类型</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdModelsOfEditOnList")]
        public APIRst GetYdModelsOfEditOnList(int mm_id, string ModuleType)
        {
            return infoHelper.GetYdModelsOfEditOnList(mm_id, ModuleType);
        }

        /// <summary>
        /// 新增采集信息
        /// </summary>
        /// <param name="Mm_id">类型ID号</param>
        /// <param name="Fun_id">功能ID号</param>
        /// <param name="FunType">采集类型</param>
        /// <param name="FunName">采集项</param>
        /// <param name="Action">读写：0=读，1=写，3=读写</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetYdModelsOnSaveFunc")]
        public APIRst SetYdModelsOnSaveFunc(int Mm_id, int Fun_id, string FunType, string FunName, int Action)
        {
            return infoHelper.SetYdModelsOnSaveFunc(Mm_id, Fun_id, FunType, FunName, Action);
        }

        /// <summary>
        /// 删除采集信息
        /// </summary>
        /// <param name="Fun_id">设备采集码ID</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SetYdModelsOnDelFunc")]
        public APIRst SetYdModelsOnDelFunc(int Fun_id)
        {
            return infoHelper.SetYdModelsOnDelFunc(Fun_id);
        }

        /// <summary>
        /// 采集-保存并发送指令
        /// </summary>
        /// <param name="Cmd">指令：0=保存，1=保存并发送指令</param>
        /// <param name="Mm_id">类型ID号</param>
        /// <param name="ModuleName">型号名称</param>
        /// <param name="ModuleType">型号</param>
        /// <param name="Disabled">是否弃用</param>
        /// <param name="ModulePwd">编程密码</param>
        /// <param name="ModuleUid">编程操作人</param>
        /// <param name="Level">密码级别</param>
        /// <param name="Spec">接线方式</param>
        /// <param name="Fty_prod">生产厂家</param>
        /// <param name="IsCharg">计费方式</param>
        /// <param name="Protocol">协议类型</param>
        /// <param name="Pmap"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetYdModelsOnSave")]
        public APIRst SetYdModelsOnSave(int Cmd, int Mm_id, string ModuleName, string ModuleType, int Disabled, string ModulePwd, string ModuleUid, string Level, string Spec, string Fty_prod, int IsCharg, string Protocol, DataModels Pmap)
        {
            return infoHelper.SetYdModelsOnSave(Cmd, Mm_id, ModuleName, ModuleType, Disabled, ModulePwd, ModuleUid, Level, Spec, Fty_prod, IsCharg, Protocol, Pmap.Data);
        }

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="Mm_id">类型ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("SetYdModelsOnDel")]
        public APIRst SetYdModelsOnDel(int Mm_id)
        {
            return infoHelper.SetYdModelsOnDel(Mm_id);
        }

    }
}