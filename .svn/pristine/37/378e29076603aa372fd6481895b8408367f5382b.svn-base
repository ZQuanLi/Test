using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using YDS6000.Models;
using YDS6000.DAL.BaseInfo;

namespace YDS6000.BLL.BaseInfo
{
    partial class BaseInfoBLL
    {
        public DataTable GetModuleList(int module_id, string moduleName)
        {
            return dal.GetModuleList(module_id, moduleName);
        }

        /// <summary>
        /// 设置回路信息
        /// </summary>
        /// <param name="module">回路信息</param>
        /// <returns></returns>
        public int SetModule(ModuleVModel module)
        {
            return dal.SetModule(module);
        }

        public int DelModule(int module_id)
        {
            return dal.DelModule(module_id);
        }

        /// <summary>
        /// 获取设备功能码
        /// </summary>
        /// <param name="module_id">设备ID号</param>
        /// <returns></returns>
        public DataTable GetMapFunTypeList(int module_id, int fun_id)
        {
            return dal.GetMapFunTypeList(module_id, fun_id);
        }
        /// <summary>
        /// 映射采集码对应的TagName
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public int SetMapFunType(MapVModel tag)
        {
            return dal.SetMapFunType(tag);
        }
    }
}
