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

        public DataTable GetMmDefineList()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("Id", typeof(System.Int32));
            dtSource.Columns.Add("Text", typeof(System.String));

            //DataTable dtRst = dal.GetModuleTypeList();
            //foreach (DataRow dr in dtRst.Rows)
            //{
            //    dtSource.Rows.Add(new object[] { CommFunc.ConvertDBNullToInt32(dr["IsDefine"]), CommFunc.ConvertDBNullToString(dr["ModuleName"]) });
            //}
            System.Reflection.FieldInfo[] fields = typeof(MmDefine).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (System.Reflection.FieldInfo field in fields)
            {
                MmDefine aa = (MmDefine)Enum.Parse(typeof(MmDefine), field.Name);
                var obj = field.GetCustomAttributes(typeof(DisplayAttribute), false);
                if (obj != null && obj.Count() != 0)
                {
                    DisplayAttribute md = obj[0] as DisplayAttribute;
                    dtSource.Rows.Add(new object[] { (int)aa, md.Name });
                }
            }
            return dtSource;
        }

        /// <summary>
        /// 获取设备型号列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetModuleTypeList()
        {
            return dal.GetModuleTypeList();
        }

        public int SetModuleType(ModuleTypeVModel mtype)
        {
            return dal.SetModuleType(mtype);
        }
        public int DelModuleType(int mm_id)
        {
            return dal.DelModuleType(mm_id);
        }
        /// <summary>
        /// 获取设备型号采集点数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetModuleFunList(int id)
        {
            return dal.GetModuleFunList(id);
        }
        /// <summary>
        /// 设置采集点信息
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        public int SetModuleFun(ModuleFunVModel fun)
        {
            return dal.SetModuleFun(fun);
        }

        /// <summary>
        /// 设置采集点信息
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        public int DelModuleFun(int fun_id)
        {
            return dal.DelModuleFun(fun_id);
        }
    }
}
