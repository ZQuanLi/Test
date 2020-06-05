using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Platform.BaseInfo
{
    partial class BaseInfoBLL
    {
        public DataTable GetModuleOfBuildList()
        {
            return dal.GetModuleOfBuildList();
        }
        /// <summary>
        /// 获取能耗列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetEnergyCombox()
        {
            return dal.GetEnergyCombox();
        }
        /// <summary>
        /// 获取回路信息
        /// </summary>
        /// <param name="meter_id"></param>
        /// <returns></returns>
        public DataTable GetModuleList(int meter_id)
        {
            return dal.GetModuleList(meter_id);
        }

        /// <summary>
        /// 获取进线列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetIncomingCombox()
        {
            return dal.GetIncomingCombox();
        }

        /// <summary>
        /// 设置回路信息列表
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="moduleName">回路名称</param>
        /// <param name="buildId">建筑ID号</param>
        /// <returns></returns>
        public int SetModuleList(int module_id, string moduleName, int buildId,string energyItemCode,int parent_id)
        {
            return dal.SetModuleList(module_id, moduleName, buildId, energyItemCode, parent_id);
        }
        /// <summary>
        /// 设置回路信息列表(PDU)
        /// </summary>
        /// <param name="module_id">回路ID号</param>
        /// <param name="moduleName">回路名称</param>
        /// <returns></returns>
        public int SetModuleList_PDU(int module_id, string moduleName)
        {
            return dal.SetModuleList_PDU(module_id, moduleName);
        }
        /// <summary>
        /// 获取设备采集点信息
        /// </summary>
        /// <param name="meter_id"></param>
        /// <returns></returns>
        public DataTable GetModuleOfMapList(int meter_id)
        {
            return dal.GetModuleOfMapList(meter_id);
        }

        /// <summary>
        /// 设置回路采集项列表
        /// </summary>
        /// <param name="module_id">设备信息ID号</param>
        /// <param name="fun_id">设备采集项ID号</param>
        /// <param name="tagName">设备采集项映射变量</param>
        /// <returns></returns>
        public int SetModuleOfMapList(int module_id, int fun_id, string tagName)
        {
            return dal.SetModuleOfMapList(module_id, fun_id, tagName);
        }
    }
}
