using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Syscont
{
    public partial class ExpTimingBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Exp.Syscont.ExpTimingDAL dal = null;
        public ExpTimingBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Exp.Syscont.ExpTimingDAL(_ledger, _uid);
        }
        /// <summary>
        /// 获取定时电表策略
        /// </summary>
        /// <param name="Descr">筛选条件：费率描述</param>
        /// <param name="rate_id">费率ID号</param>
        /// <returns></returns>
        public DataTable GetYdTiming(int Psi_id, string Descr)
        {
            DataTable dtSource = dal.GetYdTiming(Psi_id, Descr);
            dtSource.Columns.Add("disabledS", typeof(System.String));
            foreach (DataRow dr in dtSource.Rows)
            {
                int disabled = CommFunc.ConvertDBNullToInt32(dr["Disabled"]);
                if (disabled == 0)
                    dr["disabledS"] = "否";
                if (disabled == 1)
                    dr["disabledS"] = "是";
            }
            return dtSource;
        }

        /// <summary>
        /// 获取新增页面内的时段设置的表格1
        /// </summary>
        /// <param name="siSSR"></param>
        /// <returns></returns>
        public DataTable GetTable(string siSSR)
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("t00", typeof(System.String));
            dtSource.Columns.Add("d01", typeof(System.String));
            dtSource.Columns.Add("d02", typeof(System.String));
            dtSource.Columns.Add("d03", typeof(System.String));
            dtSource.Columns.Add("d04", typeof(System.String));
            dtSource.Columns.Add("d05", typeof(System.String));
            dtSource.Columns.Add("d06", typeof(System.String));
            dtSource.Columns.Add("d07", typeof(System.String));
            dtSource.Columns.Add("d08", typeof(System.String));
            dtSource.Rows.Add(new object[] { "第01时段定时断送电时间" });
            dtSource.Rows.Add(new object[] { "第02时段定时断送电时间" });
            dtSource.Rows.Add(new object[] { "第03时段定时断送电时间" });
            dtSource.Rows.Add(new object[] { "第04时段定时断送电时间" });
            dtSource.Rows.Add(new object[] { "第05时段定时断送电时间" });
            dtSource.Rows.Add(new object[] { "第06时段定时断送电时间" });
            dtSource.Rows.Add(new object[] { "第07时段定时断送电时间" });
            dtSource.Rows.Add(new object[] { "第08时段定时断送电时间" });
            JsonSiModel si = Models.ModelHandler<JsonSiModel>.jsonTextToModel(CommFunc.ConvertDBNullToString(siSSR));
            StringBuilder strD = new StringBuilder();
            StringBuilder strT = new StringBuilder();
            JsonSiModel.Value value;
            int rIndex = 0;

            foreach (DataRow dr in dtSource.Rows)
            {
                rIndex = rIndex + 1;
                foreach (DataColumn dc in dtSource.Columns)
                {
                    int cIndex = CommFunc.ConvertDBNullToInt32(dc.ColumnName.Trim().Substring(1, 2));
                    if (cIndex == 0) continue;
                    strD.Clear();
                    strT.Clear();
                    strD.Append("d" + cIndex.ToString().Trim().PadLeft(2, '0'));
                    strT.Append("t" + rIndex.ToString().Trim().PadLeft(2, '0'));
                    System.Reflection.PropertyInfo dInfo = si.GetType().GetProperty(strD.ToString());
                    object obj = dInfo.GetValue(si, null);
                    Type type = dInfo.PropertyType;
                    System.Reflection.PropertyInfo tInfo = Activator.CreateInstance(type).GetType().GetProperty(strT.ToString());
                    value = (JsonSiModel.Value)tInfo.GetValue(obj, null);
                    dr[dc.ColumnName] = value.hm + "-" + value.sr;
                }
            }
            return dtSource;
        }

        /// <summary>
        /// 获取新增页面内的时段设置的表格2
        /// </summary>
        /// <param name="nAct"></param>
        /// <param name="Psi_id"></param>
        /// <returns></returns>
        public DataTable GetYdm_si_ssr(int si_id, string descr)
        {
            DataTable dtSource = dal.GetYdm_si_ssr(si_id, descr);
            dtSource.Columns.Add("disabledS", typeof(System.String));
            foreach (DataRow dr in dtSource.Rows)
            {
                int disabled = CommFunc.ConvertDBNullToInt32(dr["Disabled"]);
                if (disabled == 0)
                    dr["disabledS"] = "否";
                if (disabled == 1)
                    dr["disabledS"] = "是";
            }
            return dtSource;
        }

        /// <summary>
        /// 设置定时电表策略
        /// </summary>
        /// <param name="si_ssr"></param>
        /// <returns></returns>
        public int EditRow(v1_si_ssrVModel si_ssr)
        {
            return dal.EditRow(si_ssr);
        }

        /// <summary>
        /// 删除定时电表策略
        /// </summary>
        /// <param name="si_ssr">费率ID号</param>
        /// <returns></returns>
        public int DelRow(v1_si_ssrVModel si_ssr)
        {
            return dal.DelRow(si_ssr);
        }


    }
}
