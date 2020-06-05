using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;
using YDS6000.DAL.Report;

namespace YDS6000.BLL.Report
{
    /// <summary>
    /// 报表定制类
    /// </summary>
    public partial class ReportBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly ReportDAL dal = null;
        public ReportBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new ReportDAL(_ledger, _uid);
        }
        /// <summary>
        /// 获取对象选择树形列表
        /// </summary>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<Treeview> GetSelectObject(out int total)
        {
            DataTable dtSource = dal.GetAreaList();
            DataTable dtMeter = dal.GetMeterList();
            total = dtSource.Rows.Count;
            List<Treeview> rst = new List<Treeview>();
            DataRow[] pArr = dtSource.Select("Parent_id=0", "Co_id");
            foreach (DataRow dr in pArr)
            {
                int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
                int attrib = CommFunc.ConvertDBNullToInt32(dr["Attrib"]);
                string type = attrib == (int)CoAttrib.Area ? "A" : attrib == (int)CoAttrib.Station ? "S" : "R";
                Treeview pTr = new Treeview();
                pTr.id = type + "-" + co_id.ToString();
                pTr.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                pTr.attributes = type;
                //
                total = total + this.GetSelectObject(co_id, ref pTr, ref dtSource, ref dtMeter);
                rst.Add(pTr);
            }
            return rst;
        }

        private int GetSelectObject(int parent_id, ref Treeview pTr, ref DataTable dtSource, ref DataTable dtMeter)
        {
            DataRow[] pArr = dtSource.Select("Parent_id=" + parent_id, "Co_id");
            int pRows = pArr.Count();
            pTr.nodes = new List<Treeview>();
            int meterCnt = 0;
            foreach (DataRow dr in pArr)
            {
                int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
                int attrib = CommFunc.ConvertDBNullToInt32(dr["Attrib"]);
                string type = attrib == (int)CoAttrib.Area ? "A" : attrib == (int)CoAttrib.Station ? "S" : "R";
                Treeview cTr = new Treeview();
                cTr.id = type + "-" + co_id.ToString();
                cTr.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                cTr.attributes = type;
                pTr.nodes.Add(cTr);
                this.GetSelectObject(co_id, ref cTr, ref dtSource, ref dtMeter);
                if (attrib == (int)CoAttrib.Room)
                {
                    #region 增加站点
                    DataRow[] meterArr = dtMeter.Select("Co_id=" + co_id, "Meter_id");
                    meterCnt = meterArr.Count();
                    cTr.nodes = new List<Treeview>();
                    foreach (DataRow drMeter in meterArr)
                    {
                        int mid = CommFunc.ConvertDBNullToInt32(drMeter["Meter_id"]);
                        Treeview mTr = new Treeview();
                        mTr.id = "M-" + mid;
                        mTr.text = CommFunc.ConvertDBNullToString(drMeter["MeterName"]);
                        mTr.attributes = "M";
                    }
                    #endregion
                }
            }
            return meterCnt;
        }
    }
}
