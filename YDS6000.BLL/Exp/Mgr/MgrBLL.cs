using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Mgr
{
    public partial class MgrBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private YDS6000.DAL.Exp.Mgr.MgrDAL dal = null;
        public MgrBLL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
            dal = new YDS6000.DAL.Exp.Mgr.MgrDAL(ledger, uid);
        }
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="attrib"></param>
        /// <returns></returns>
        public DataTable GetVp_coinfo(CoAttribV2_1 attrib)
        {
            return dal.GetVp_coinfo(attrib);
        }

        public int UpdateCoInfo(int co_id, string coNo, string coName, int parent_id, CoAttribV2_1 attrib, string custAddr, string mobile, string remark, int disabled)
        {
            return dal.UpdateCoInfo(co_id, coNo, coName, parent_id, attrib, custAddr, mobile, remark, disabled);
        }

        /// <summary>
        /// 删除区域、单位、项目、用电单元
        /// </summary>
        /// <param name="co_id">ID号</param>
        /// <returns></returns>
        public int DelCoInfo(int co_id)
        {
            return dal.DelCoInfo(co_id);
        }

        public DataTable GetMdList()
        {
            return dal.GetMdList();
        }
        public DataTable GetMapList(int meter_id)
        {
            return dal.GetMapList(meter_id);
        }
        /// <summary>
        /// 保存设备信息
        /// </summary>
        /// <param name="id">设备ID号</param>
        /// <param name="name">设备名称</param>
        /// <param name="cellId">用电单元id号</param>
        /// <param name="moduleId">设备型号id号</param>
        /// <param name="disabled">设备状态=0正常=1弃用</param>
        /// <returns></returns>
        public int SaveMdInfo(int id, string name, int cellId, int moduleId, int disabled)
        {
            return dal.SaveMdInfo(id, name, cellId, moduleId, disabled);
        }

        /// <summary>
        /// 更新设备位置信息
        /// </summary>
        /// <param name="meter_id">设备ID号</param>
        /// <param name="inst_loc">设备安装地址</param>
        /// <returns></returns>
        public int UpdateInst_loc(int meter_id, string inst_loc)
        {
            return dal.UpdateInst_loc(meter_id, inst_loc);
        }
        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="meter_id"></param>
        /// <returns></returns>
        public int DelMdInfo(int meter_id)
        {
            return dal.DelMdInfo(meter_id);
        }

        public int SaveMapInfo(int module_id, int fun_id, string tagName, string dataValue)
        {
            return dal.SaveMapInfo(module_id, fun_id, tagName, dataValue);
        }

        /// <summary>
        /// 清零
        /// </summary>
        /// <returns></returns>
        public int SetClear(int module_id)
        {
            return dal.SetClear(module_id);
        }

        public string GetImg(int co_id)
        {
            return dal.GetImg(co_id);
        }

        public int UpdateImg(int co_id, string path)
        {
            return dal.UpdateImg(co_id, path);
        }

        public int ImportTagsFromIOServer(int module_id, string busname, string devname)
        {
            return dal.ImportTagsFromIOServer(module_id, busname, devname);
        }
    }
}
