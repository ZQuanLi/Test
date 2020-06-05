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
    /// 工程配置-电表管理
    /// </summary>
    [RoutePrefix("api/Exp/Syscont")]
    public class ExpYdModuleController : ApiController
    {
        private YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpYdModuleHelper infoHelper = new YDS6000.WebApi.Areas.Exp.Opertion.Syscont.ExpYdModuleHelper();

        /// <summary>
        /// 获取集中器数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdModuleOnGatewayList")]
        public APIRst GetYdModuleOnGatewayList()
        {
            return infoHelper.GetYdModuleOnGatewayList();
        }

        /// <summary>
        /// 获取采集列表
        /// </summary>
        /// <param name="Gw_id">主键ID号</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdModuleOnDetail")]
        public APIRst GetYdModuleOnDetail(int Gw_id)
        {
            return infoHelper.GetYdModuleOnDetail(Gw_id);
        }

        /// <summary>
        /// 保存集中器信息
        /// </summary>
        /// <param name="v1_gateway">对象：Gw_id:新增=0,GwName:集中器名称,GwAddr=集中器地址,GwIp=IP地址,GwPort=端口,Timeout=集中器延时设置(毫秒),GwType=类型，Inst_loc=安装位置,Remark=备注,Disabled=是否弃用(默认=0)</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetYdModuleOnSaveGw")]
        public APIRst SetYdModuleOnSaveGw(v1_gatewayVModel v1_gateway)
        {
            return infoHelper.YdModuleOnSaveGw(v1_gateway);
        }

        /// <summary>
        /// 删除集中器信息
        /// </summary>
        /// <param name="Gw_id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("YdModuleOnDelGw")]
        public APIRst YdModuleOnDelGw(int Gw_id)
        {
            return infoHelper.YdModuleOnDelGw(Gw_id);
        }

        /// <summary>
        /// 保存采集器数据
        /// </summary>
        /// <param name="ComPortNum"></param>
        /// <param name="ep">对象：Esp_id:新增=0,Gw_id=集中器序号,EspName=采集器名称,EspAddr=采集器地址,TransferType=通讯方式,Baud=波特率,DataBit=数据位,StopBit=停止位,Parity=校验方式,EspPort=端口,Timeout=超时秒,Inst_loc=安装位置,Remark=备注</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetYdModuleOnSaveEsp")]
        public APIRst SetYdModuleOnSaveEsp(int ComPortNum, v1_gateway_espVModel ep)
        {
            return infoHelper.YdModuleOnSaveEsp(ComPortNum, ep);
        }

        /// <summary>
        /// 删除采集器数据
        /// </summary>
        /// <param name="Esp_id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("YdModuleOnDelEsp")]
        public APIRst YdModuleOnDelEsp(int Esp_id)
        {
            return infoHelper.YdModuleOnDelEsp(Esp_id);
        }

        /// <summary>
        /// 获设备信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdModuleOnModelList")]
        public APIRst GetYdModuleOnModelList()
        {
            return infoHelper.GetYdModuleOnModelList();
        }

        /// <summary>
        /// 保存设备数据
        /// </summary>
        /// <param name="md">对象：Meter_id,Esp_id,MeterName=设备名称,MeterAddr=设备地址,MeterNo=设备编号,Mm_id=设备类型,Inst_loc=安装位置,Multiply=设备倍率,Remark=备注,Disabled=是否弃用</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetYdModuleOnSaveMm")]
        public APIRst SetYdModuleOnSaveMm(v1_gateway_esp_meterVModel md)
        {
            return infoHelper.YdModuleOnSaveMm(md);
        }

        /// <summary>
        /// 删除设备数据
        /// </summary>
        /// <param name="Meter_id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("YdModuleOnDelMm")]
        public APIRst YdModuleOnDelMm(int Meter_id)
        {
            return infoHelper.YdModuleOnDelMm(Meter_id);
        }

        /// <summary>
        /// 设置回路信息(电能表信息)
        /// </summary>
        /// <param name="md">对象：Module_id,Meter_id=设备ID号,ModuleName=回路名称,ModuleAddr=回路地址(电能表),Disabled=是否弃用(默认0),Remark=备注,MinPay=电表支付最小金额,Price=电费单价,Rate_id,EnergyItemCode=分类分项编码(01000),Create_by=建立人,Create_dt=建立时间,Update_by=更新人,Update_dt=更新人</param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetYdModuleOnSaveMd")]
        public APIRst SetYdModuleOnSaveMd(v1_gateway_esp_moduleVModel md)
        {
            return infoHelper.YdModuleOnSaveMd(md);
        }

        /// <summary>
        /// 删除回路信息(电能表信息)
        /// </summary>
        /// <param name="Module_id"></param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("YdModuleOnDelMd")]
        public APIRst YdModuleOnDelMd(int Module_id)
        {
            return infoHelper.YdModuleOnDelMd(Module_id);
        }

        /// <summary>
        /// 获取映射
        /// </summary>
        /// <param name="Module_id"></param>
        /// <param name="Action">0读(遥测,遥信)，1写(遥控),2事件,3读写</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdModuleOnMapList")]
        public APIRst GetYdModuleOnMapList(int Module_id, int Action)
        {
            return infoHelper.GetYdModuleOnMapList(Module_id, Action);
        }

        /// <summary>
        /// 保存映射
        /// </summary>
        /// <param name="Module_id"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("SetYdModuleOnMapInSave")]
        public APIRst SetYdModuleOnMapInSave(int Module_id, DataModels Data)
        {
            return infoHelper.YdModuleOnMapInSave(Module_id, Data);
        }

        /// <summary>
        /// 获取费率列表
        /// </summary>
        /// <param name="Attrib">费率=0,物业收费标准=1</param>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetYdModuleRateList")]
        public APIRst GetYdModuleRateList(int Attrib)
        {
            return infoHelper.GetYdModuleRateList(Attrib);
        }

        /// <summary>
        /// 获取首页仪表数
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetHomeModule")]
        public APIRst GetHomeModule()
        {
            return infoHelper.GetHomeModule();
        }

        /// <summary>
        /// 获取首页故障电表数
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("GetHomeModuleError")]
        public APIRst GetHomeModuleError()
        {
            return infoHelper.GetHomeModuleError();
        }



    }
}