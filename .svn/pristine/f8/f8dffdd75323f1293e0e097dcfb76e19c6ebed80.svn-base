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
        /// <summary>
        /// 获取集中器列表
        /// </summary>
        /// <param name="esp_id">id号</param>
        /// <returns></returns>
        public DataTable GetEspList(int esp_id, string espName)
        {
            return dal.GetEspList(esp_id, espName);
        }
        /// <summary>
        /// 设置集中器信息
        /// </summary>
        /// <param name="esp"></param>
        /// <returns></returns>
        public int SetEsp(EspVModel esp)
        {
            return dal.SetEsp(esp);
        }
        /// <summary>
        /// 删除集中器
        /// </summary>
        /// <param name="esp_id"></param>
        /// <returns></returns>
        public int DelEsp(int esp_id)
        {
            return dal.DelEsp(esp_id);
        }

        #region 设备型号信息
        /// <summary>
        /// 设备类型列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetMmList(int mm_id, string moduleTypeName)
        {
            return dal.GetMmList(mm_id, moduleTypeName);
        }
        /// <summary>
        /// 根据ID获取型号
        /// </summary>
        /// <param name="mm_id"></param>
        /// <returns></returns>
        public string GetMmType(int mm_id)
        {
            return dal.GetMmType(mm_id);
        }
        /// <summary>
        /// 获取设备类型采集码列表
        /// </summary>
        /// <param name="mm_id"></param>
        /// <returns></returns>
        public DataTable GetMmFunTypeList(int mm_id)
        {
            return dal.GetMmFunTypeList(mm_id);
        }
        /// <summary>
        /// 设置设备类型
        /// </summary>
        /// <param name="mmType">设备信息</param>
        /// <param name="v0_fun"></param>
        /// <returns></returns>
        public int SetMm(MeterTypeVModel mmType, DataTable v0_fun)
        {
            return dal.SetMm(mmType, v0_fun);
        }
        /// <summary>
        /// 删除类型设备
        /// </summary>
        /// <param name="mm_id">类型ID号</param>
        /// <returns></returns>
        public int DelMm(int mm_id)
        {
            return dal.DelMm(mm_id);
        }
        #endregion

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="meter_id">设备id号</param>
        /// <returns></returns>
        public DataTable GetMeterList(int meter_id,string meterName)
        {
            return dal.GetMeterList(meter_id, meterName);
        }
        /// <summary>
        /// 设置设备信息
        /// </summary>
        /// <param name="meter">设备信息</param>
        /// <returns></returns>
        public int SetMeter(MeterVModel meter)
        {
            return dal.SetMeter(meter);
        }
        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="meter_id">设备ID号</param>
        /// <returns></returns>
        public int DelMeter(int meter_id)
        {
            return dal.DelMeter(meter_id);
        }
        /// <summary>
        /// 付款方式列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetChrgTypeList()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("Id", typeof(System.Int32));
            dtSource.Columns.Add("Text", typeof(System.String));

            System.Reflection.FieldInfo[] fields = typeof(ChargAttrib).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (System.Reflection.FieldInfo field in fields)
            {
                if (field.Name.Contains("Rm")) continue;
                ChargAttrib aa = (ChargAttrib)Enum.Parse(typeof(ChargAttrib), field.Name);
                var obj = field.GetCustomAttributes(typeof(DisplayAttribute), false);
                if (obj != null && obj.Count() !=0)
                {
                    DisplayAttribute md = obj[0] as DisplayAttribute;
                    dtSource.Rows.Add(new object[] { (int)aa, md.Name });
                }
            }
            return dtSource;
        }
        
        
        /// <summary>
        /// 通讯方式列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTransferTypeList()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("Id", typeof(System.Int32));
            dtSource.Columns.Add("Text", typeof(System.String));

            System.Reflection.FieldInfo[] fields = typeof(TfAttrib).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (System.Reflection.FieldInfo field in fields)
            {                
                TfAttrib aa = (TfAttrib)Enum.Parse(typeof(TfAttrib), field.Name);
                var obj = field.GetCustomAttributes(typeof(DisplayAttribute), false);
                if (obj != null && obj.Count() != 0)
                {
                    DisplayAttribute md = obj[0] as DisplayAttribute;
                    dtSource.Rows.Add(new object[] { (int)aa, md.Name });
                }
            }
            return dtSource;
        }




    }
}
