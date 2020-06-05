using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.PDU.Home
{
    partial class HomeBLL
    {

        public DataTable GetPduMap()
        {
            return dal.GetPduMap();
        }
        /// <summary>
        /// 获取PDU树状
        /// </summary>
        /// <returns></returns>
        public DataTable GetPduTree(int co_id=0)
        {
            return dal.GetPduTree(co_id);
        }
		/// <summary>
        /// 分组
        /// </summary>
        /// <returns></returns>
        public int GetPduGroupCount(int co_id)
        {
            return dal.GetPduGroupCount(co_id);
        }
		/// <summary>
        /// PDU信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetPduInfo(int co_id)
        {
            return dal.GetPduInfo(co_id);
        }

        public DataTable GetPduEnergyPie(int coid)
        {
            DataTable dtRst = new DataTable();
            dtRst.Columns.Add("Parent_id", typeof(System.Int32));
            dtRst.Columns.Add("UseVal", typeof(System.Decimal));
            dtRst.Columns.Add("CoName", typeof(System.String));
            dtRst.PrimaryKey = new DataColumn[] { dtRst.Columns["Parent_id"] };
            ///////////
            DataTable dtSource = dal.GetPduEnergyPie(coid);
            dtSource.PrimaryKey = new DataColumn[] { dtSource.Columns["Module_id"], dtSource.Columns["Fun_id"] };
            StringBuilder splitMdQuery = new StringBuilder();
            foreach (DataRow dr in dtSource.Rows)
            {
                if (!string.IsNullOrEmpty(splitMdQuery.ToString()))
                    splitMdQuery.Append(",");
                splitMdQuery.Append(CommFunc.ConvertDBNullToString(dr["Module_id"]));
                DataRow addDr = dtRst.Rows.Find(dr["Parent_id"]);
                if (addDr == null)
                {
                    addDr = dtRst.NewRow();
                    addDr["Parent_id"] = dr["Parent_id"];
                    addDr["CoName"] = dr["CoName"];
                    addDr["UseVal"] = 0;
                    dtRst.Rows.Add(addDr);
                }
            }
            DataTable dtUse = WholeBLL.GetCoreQueryData(this.Ledger, splitMdQuery.ToString(), DateTime.Now, DateTime.Now, "day", "E");
            foreach (DataRow dr in dtUse.Rows)
            {
                DataRow curDr = dtSource.Rows.Find(new object[] { dr["Module_id"], dr["Fun_id"] });
                if (curDr == null) continue;
                int scale = CommFunc.ConvertDBNullToInt32(curDr["Scale"]);
                int co_id = CommFunc.ConvertDBNullToInt32(curDr["Co_id"]);
                decimal multiply = CommFunc.ConvertDBNullToDecimal(curDr["Multiply"]);
                scale = scale == 0 ? 2 : scale;
                if (CommFunc.ConvertDBNullToInt32(dr["Co_id"]) != co_id) continue;
                DataRow addDr = dtRst.Rows.Find(curDr["Parent_id"]);
                if (addDr == null) continue;

                DateTime tagTime = CommFunc.ConvertDBNullToDateTime(dr["TagTime"]);
                decimal firstVal = CommFunc.ConvertDBNullToDecimal(dr["FirstVal"]);
                decimal lastVal = CommFunc.ConvertDBNullToDecimal(dr["LastVal"]);
                decimal useVal = lastVal - firstVal;
                useVal = Math.Round(useVal * multiply, scale, MidpointRounding.AwayFromZero);
                addDr["UseVal"] = CommFunc.ConvertDBNullToDecimal(addDr["UseVal"]) + useVal;
            }
            return dtRst;
        }
    }
}
