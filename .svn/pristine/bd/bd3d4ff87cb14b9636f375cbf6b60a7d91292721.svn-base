using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Controllers
{
    /// <summary>
    /// 工程配置-建筑管理
    /// </summary>
    [RoutePrefix("api/Exp/Syscont")]
    public class ExpBuildingNewController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpBuildingNewHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpBuildingNewHelper();

        /// <summary>
        /// 获取组织树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdCustOnCoInfoList")]
        public APIRst GetYdCustOnCoInfoList()
        {
            return infoHelper.GetYdCustOnCoInfoList();
        }

        /// <summary>
        /// 获取组织信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdCustOnCoInfo")]
        public APIRst GetYdCustOnCoInfo(int Co_id)
        {
            return infoHelper.GetYdCustOnCoInfo(Co_id);
        }

        /// <summary>
        /// 获取用户的用能列表
        /// </summary>
        /// <param name="Co_id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdCustOnMdList")]
        public APIRst GetYdCustOnMdList(int Co_id)
        {
            return infoHelper.GetYdCustOnMdList(Co_id);
        }

        /// <summary>
        /// 保存公司信息
        /// </summary>
        /// <param name="Type">空=A,新增建筑=B,新增房间=H</param>
        /// <param name="PCoChk">父ID号</param>
        /// <param name="v1_CustInfo">Co_id主键ID:0新增,CoName:客户名称,Parent_id:父ID号,Disabled:是否弃用0否1是,CustAddr:客户地址,Office_tel:办公电话,Mobile:移动电话,Email,IsDefine:定义属性默认0</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetSaveCo")]
        public APIRst SetSaveCo(string Type, int PCoChk, v1_custInfoVModel v1_CustInfo)
        {
            return infoHelper.SetSaveCo(Type, PCoChk, v1_CustInfo);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Co_id">主键ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GeDelCo")]
        public APIRst GeDelCo(int Co_id)
        {
            return infoHelper.GeDelCo(Co_id);
        }

        /// <summary>
        /// 查找用能列表
        /// </summary>
        /// <param name="Co_id">主键</param>
        /// <param name="FindStr">电能表地址</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdCustOnMdInFind")]
        public APIRst GetYdCustOnMdInFind(int Co_id, string FindStr)
        {
            return infoHelper.GetYdCustOnMdInFind(Co_id, FindStr);
        }

        /// <summary>
        /// 获取计量点有序用电总加组信息
        /// </summary>
        /// <param name="Module_id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdCustInMdFormula")]
        public APIRst GetYdCustInMdFormula(int Module_id)
        {
            return infoHelper.GetYdCustInMdFormula(Module_id);
        }

        /// <summary>
        /// 获取计量点有序用电可进行总加组信息
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Module_id"></param>
        /// <param name="FindStr"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdCustInMdFormulaFind")]
        public APIRst GetYdCustInMdFormulaFind(int Co_id, int Module_id, string FindStr)
        {
            return infoHelper.GetYdCustInMdFormulaFind(Co_id, Module_id, FindStr);
        }

        /// <summary>
        /// 修改电能表
        /// </summary>
        /// <param name="Module_id"></param>
        /// <param name="energyType"></param>
        /// <param name="Rate_id"></param>
        /// <param name="Price"></param>
        /// <param name="AlarmVal"></param>
        /// <param name="IsAlarm"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SaveMdSet")]
        public APIRst SaveMdSet(int Module_id, string energyType, int Rate_id, decimal Price, decimal AlarmVal, int IsAlarm)
        {
            return infoHelper.SaveMdSet(Module_id, energyType, Rate_id, Price, AlarmVal, IsAlarm);
        }

        /// <summary>
        /// 设置用能列表
        /// </summary>
        /// <param name="Co_id">公司ID</param>
        /// <param name="IdStr">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("YdCustOnMdInSave")]
        public APIRst YdCustOnMdInSave(int Co_id, string IdStr)
        {
            return infoHelper.YdCustOnMdInSave(Co_id, IdStr);
        }

        /// <summary>
        /// 设置有序用电总加组
        /// </summary>
        /// <param name="Co_id">公司ID</param>
        /// <param name="Module_id">计量点ID</param>
        /// <param name="IdStr">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetYdCustInMdFormula")]
        public APIRst SetYdCustInMdFormula(int Co_id, int Module_id, string IdStr)
        {
            return infoHelper.SetYdCustInMdFormula(Co_id, Module_id, IdStr);
        }

        /// <summary>
        /// 分类分项编码下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetEnergyItemCode")]
        public APIRst GetEnergyItemCode()
        {
            return infoHelper.GetEnergyItemCode();
        }

        /// <summary>
        /// 建筑详细信息
        /// </summary>
        /// <param name="Co_id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdCustInfoBuildData")]
        public APIRst GetYdCustInfoBuildData(int Co_id)
        {
            return infoHelper.GetYdCustInfoBuildData(Co_id);
        }

        /// <summary>
        /// 保存建筑详细信息
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Area">面积</param>
        /// <param name="BuildType">房屋性质</param>
        /// <param name="CustName">用户名称</param>
        /// <param name="CorpName">企业名称</param>
        /// <param name="Bank">户数</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetSaveYdCustInfoBuildData")]
        public APIRst SetSaveYdCustInfoBuildData(int Co_id, decimal Area, string BuildType, string CustName, string CorpName, int Bank)
        {
            return infoHelper.SaveYdCustInfoBuildData(Co_id, Area, BuildType, CustName, CorpName, Bank);
        }


        /// <summary>
        /// 获取物业费用标准
        /// </summary>
        /// <param name="Co_id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdCustInfoBuild_fee")]
        public APIRst GetYdCustInfoBuild_fee(int Co_id)
        {
            return infoHelper.GetYdCustInfoBuild_fee(Co_id);
        }

        /// <summary>
        /// 保存取物业费用标准
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Cic_id"></param>
        /// <param name="Rate_id"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetSaveYdCustInfoBuildFee")]
        public APIRst SetSaveYdCustInfoBuildFee(int Co_id, int Cic_id, int Rate_id)
        {
            return infoHelper.SaveYdCustInfoBuildFee(Co_id, Cic_id, Rate_id);
        }

        /// <summary>
        /// 获取首页建筑数和房间(总用户数量取数)
        /// </summary>
        /// <param name="Attrib">// 0=空,100=建筑,9000=房间</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetHomeBuilding")]
        public APIRst GetHomeBuilding(int Attrib)
        {
            return infoHelper.GetHomeBuilding(Attrib);
        }


    }
}