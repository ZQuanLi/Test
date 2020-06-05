using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;
namespace YDS6000.BLL.Exp.Syscont
{
    public partial class ExpModelsBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Exp.Syscont.ExpModelsDAL dal = null;
        public ExpModelsBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Exp.Syscont.ExpModelsDAL(_ledger, _uid);
        }

        /// <summary>
        /// 获取设备型号
        /// </summary>
        /// <param name="ModuleType">筛选条件：设备类型</param>
        /// <returns></returns>
        public DataTable GetYdModelsOnList(string ModuleType)
        {
            return dal.GetYdModelsOnList(0, ModuleType);
        }

        /// <summary>
        /// 获取设备型号
        /// </summary>
        /// <param name="ModuleType">筛选条件：设备类型</param>
        /// <returns></returns>
        public DataTable GetYdModelsOnList(int mm_id)
        {
            return dal.GetYdModelsOnList(mm_id, "");
        }

        /// <summary>
        /// 获取采集信息
        /// </summary>
        /// <param name="Mm_id">类型ID号</param>
        /// <returns></returns>
        public DataTable GetYdModelsOfEditOnList(int Mm_id)
        {
            //return dal.GetYdModelsOfEditOnList(Mm_id);
            DataTable dtSource = dal.GetYdModelsOfEditOnList(Mm_id);
            foreach (DataRow dr in dtSource.Rows)
            {
                if (CommFunc.ConvertDBNullToString(dr["FunType"]).Equals(V0Fun.IsRelay.ToString()))
                    if (CommFunc.ConvertDBNullToInt32(dr["DataValue"]) >= 2)
                        dr["DataValue"] = "2";
            }
            return dtSource;
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
        public int SetYdModelsOnSaveFunc(int Mm_id, int Fun_id, string FunType, string FunName, int Action)
        {
            return dal.SetYdModelsOnSaveFunc(Mm_id, Fun_id, FunType, FunName, Action);
        }

        /// <summary>
        /// 删除采集信息
        /// </summary>
        /// <param name="Fun_id">设备采集码ID</param>
        /// <returns></returns>
        public int SetYdModelsOnDelFunc(int Fun_id)
        {
            return dal.SetYdModelsOnDelFunc(Fun_id);
        }

        public int SetYdModelsOnSave(v0_moduleVModel mm, DataTable v0_fun)
        {
            return dal.SetYdModelsOnSave(mm, v0_fun);
        }

        /// <summary>
        /// 获取发送命令信息
        /// </summary>
        /// <param name="mm_id"></param>
        /// <returns></returns>
        public DataTable GetYdModelsSendCmd(int mm_id)
        {
            return dal.GetYdModelsSendCmd(mm_id);
        }

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="Mm_id">类型ID号</param>
        /// <returns></returns>
        public int SetYdModelsOnDel(int Mm_id)
        {
            return dal.SetYdModelsOnDel(Mm_id);
        }
    }
}
