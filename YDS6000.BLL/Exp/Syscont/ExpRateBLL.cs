using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Syscont
{
    public partial class ExpRateBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Exp.Syscont.ExpRateDAL dal = null;
        public ExpRateBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Exp.Syscont.ExpRateDAL(_ledger, _uid);
        }
        /// <summary>
        /// 获取费率信息
        /// </summary>
        /// <param name="Descr">筛选条件：费率描述</param>
        /// <param name="rate_id">费率ID号</param>
        /// <returns></returns>
        public DataTable GetYdRateList(int rate_id, string descr)
        {
            DataTable dtSource = dal.GetYdRateList(rate_id, descr);
            dtSource.Columns.Add("T1st", typeof(System.String));
            dtSource.Columns.Add("T2nd", typeof(System.String));
            dtSource.Columns.Add("T3rd", typeof(System.String));
            dtSource.Columns.Add("T4th", typeof(System.String));

            foreach (DataRow dr in dtSource.Rows)
            {
                string dataCfg = CommFunc.ConvertDBNullToString(dr["DataCfg"]);
                if (!string.IsNullOrEmpty(dataCfg))
                {
                    v1_rateCfg cfg = JsonHelper.Deserialize<v1_rateCfg>(dataCfg);
                    dr["T1st"] = cfg.T1st;
                    dr["T2nd"] = cfg.T2nd;
                    dr["T3rd"] = cfg.T3rd;
                    dr["T4th"] = cfg.T4th;
                }
            }
            return dtSource;
        }

        /// <summary>
        /// 设置保存费率信息
        /// </summary>
        /// <param name="T1st">尖单价-对应的开始时间</param>
        /// <param name="T2nd">峰单价-对应的开始时间</param>
        /// <param name="T3rd">平单价-对应的开始时间</param>
        /// <param name="T4th">谷单价-对应的开始时间</param>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="Descr">费率描述</param>
        /// <param name="Pri1st">尖单价</param>
        /// <param name="Pri2nd">峰单价</param>
        /// <param name="Pri3rd">平单价</param>
        /// <param name="Pri4th">谷单价</param>
        /// <returns></returns>
        public int SaveYdRate(v1_rateVModel rv)
        {
            return dal.SaveYdRate(rv);
        }

        /// <summary>
        /// 删除费率信息
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <returns></returns>
        public int GetDelYdRate(int Rate_id)
        {
            return dal.GetDelYdRate(Rate_id);
        }



    }
}
