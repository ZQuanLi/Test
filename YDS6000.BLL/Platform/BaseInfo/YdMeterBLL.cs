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
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetMeterList()
        {
            return dal.GetMeterList();
        }
        /// <summary>
        /// 设置设备信息
        /// </summary>
        /// <param name="md"></param>
        /// <returns></returns>
        public int SetMeter(MdVModel md)
        {
            return dal.SetMeter(md);
        }

        public int DelMeter(int meter_id)
        {
            return dal.DelMeter(meter_id);
        }
        /// <summary>
        /// 获取设备列表(PDU)
        /// </summary>
        /// <returns></returns>
        public DataTable GetMeterList_PDU()
        {
            return dal.GetMeterList_PDU();
        }
        /// <summary>
        /// 设置设备信息(PDU)
        /// </summary>
        /// <param name="md"></param>
        /// <returns></returns>
        public int SetMeter_PDU(MdVModel_PDU md)
        {
            return dal.SetMeter_PDU(md);
        }
        /// <summary>
        /// 获取父设备列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetParentMeterCombox()
        {
            return dal.GetParentMeterCombox();
        }
    }
}
