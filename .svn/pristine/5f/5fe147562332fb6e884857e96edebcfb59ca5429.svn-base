using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Syscont
{
    public partial class ExpRateNewBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Exp.Syscont.ExpRateNewDAL dal = null;
        public ExpRateNewBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Exp.Syscont.ExpRateNewDAL(_ledger, _uid);
        }

        /// <summary>
        /// 获取物业收费信息
        /// </summary>
        /// <param name="Descr">筛选条件：描述</param>
        /// <param name="rate_id">费率ID号</param>
        /// <returns></returns>
        public DataTable GetYdRateNewList(int rate_id, string descr)
        {
            DataTable dtSource = dal.GetYdRateNewList(rate_id, descr);
            return dtSource;
        }

        /// <summary>
        /// 设置保存物业收费
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="Descr">描述</param>
        /// <param name="Rule">计算规则: 0=正常,1=时间范围,2=数量范围</param>
        /// <param name="Unit">单位: Area=平方米,Bank=户数</param>
        /// <param name="UnitBase">单位基数</param>
        /// <param name="Disabled">是否弃用:0=否,1=是</param>
        /// <returns></returns>
        public int SaveYdRateNew(v1_rateVModel rv)
        {
            return dal.SaveYdRateNew(rv);
        }

        /// <summary>
        /// 删除费率信息
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <returns></returns>
        public int GetDelYdRateNew(int Rate_id)
        {
            return dal.GetDelYdRateNew(Rate_id);
        }


        /// <summary>
        /// 获取物业收费-单价详情
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <returns></returns>
        public DataTable GetYdRateNewCs(int Rate_id)
        {
            DataTable dtSource = dal.GetYdRateNewCs(Rate_id);
            return dtSource;
        }

        /// <summary>
        /// 设置物业收费-单价详情
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="CsId">单价ID号</param>
        /// <param name="Price">单价</param>
        /// <param name="pStart">开始区间</param>
        /// <param name="pEnd">结束区间</param>
        /// <returns></returns>
        public int SetSaveYdRateNewCs(int Rate_id, int CsId, decimal Price, string pStart, string pEnd)
        {
            return dal.SetSaveYdRateNewCs(Rate_id, CsId, Price, pStart, pEnd);
        }

        /// <summary>
        /// 删除物业收费-单价详情
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="csId">单价ID号</param>
        /// <returns></returns>
        public int GetDelYdRateCs(int Rate_id, int csId)
        {
            return dal.GetDelYdRateCs(Rate_id,csId);
        }


    }
}
