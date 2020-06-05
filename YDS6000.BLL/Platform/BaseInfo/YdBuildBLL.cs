using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Platform.BaseInfo
{
    partial class BaseInfoBLL
    {
        /// <summary>
        /// 获取建筑信息
        /// </summary>
        /// <returns></returns>
        public List<Treeview> GetBuildList()
        {
            List<Treeview> rst = new List<Treeview>();
            DataTable dtPro = dal.GetProList();
            DataTable dtSource = dal.GetBuildList();
            dtSource.Columns.Add("IsAdd", typeof(System.Int32));
            foreach (DataRow dr in dtPro.Rows)
            {
                Treeview pro = new Treeview();
                int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
                pro.id = co_id.ToString();
                pro.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                pro.attributes = new
                {
                    Id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]),
                    BuildName = CommFunc.ConvertDBNullToString(dr["CoName"]),
                    BuildAddr = CommFunc.ConvertDBNullToString(dr["CustAddr"]),
                    TelNo = CommFunc.ConvertDBNullToString(dr["Mobile"]),
                    Email = CommFunc.ConvertDBNullToString(dr["Email"]),
                    Remark = CommFunc.ConvertDBNullToString(dr["Remark"]),
                    Parent_id = CommFunc.ConvertDBNullToInt32(dr["Parent_id"]),
                };
                rst.Add(pro);
                this.GetBuildList(ref pro, ref dtSource, ref co_id);
            }
            //
            foreach (DataRow dr in dtSource.Select("isnull(IsAdd,0)=0 and Parent_id =0"))
            {
                Treeview pro = new Treeview();
                int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
                pro.id = co_id.ToString();
                pro.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                pro.attributes = new
                {
                    Id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]),
                    BuildName = CommFunc.ConvertDBNullToString(dr["CoName"]),
                    BuildAddr = CommFunc.ConvertDBNullToString(dr["CustAddr"]),
                    TelNo = CommFunc.ConvertDBNullToString(dr["Mobile"]),
                    Email = CommFunc.ConvertDBNullToString(dr["Email"]),
                    Remark = CommFunc.ConvertDBNullToString(dr["Remark"]),
                    Parent_id = CommFunc.ConvertDBNullToInt32(dr["Parent_id"]),
                };
                rst.Add(pro);
                this.GetBuildList(ref pro, ref dtSource, ref co_id);
            }
            return rst;
        }
        private void GetBuildList(ref Treeview list,ref DataTable dtSource,ref int parent_id)
        {
            if (list == null) list = new Treeview();
            DataRow[] arr = dtSource.Select("Parent_id=" + parent_id);
            if (arr.Count() > 0)
                list.nodes = new List<Treeview>();
            foreach (DataRow dr in arr)
            {
                dr["IsAdd"] = 1;
                int co_id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]);
                Treeview tb = new Treeview();
                tb.id = co_id.ToString();
                tb.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                tb.attributes = new
                {
                    Id = CommFunc.ConvertDBNullToInt32(dr["Co_id"]),
                    BuildName = CommFunc.ConvertDBNullToString(dr["CoName"]),
                    BuildAddr = CommFunc.ConvertDBNullToString(dr["CustAddr"]),
                    TelNo = CommFunc.ConvertDBNullToString(dr["Mobile"]),
                    Email = CommFunc.ConvertDBNullToString(dr["Email"]),
                    Remark = CommFunc.ConvertDBNullToString(dr["Remark"]),
                    Parent_id = CommFunc.ConvertDBNullToInt32(dr["Parent_id"]),
                };
                list.nodes.Add(tb);
                this.GetBuildList(ref tb, ref dtSource, ref co_id);
            }
        }

        /// <summary>
        /// 设置建筑信息
        /// </summary>
        /// <param name="build"></param>
        /// <returns></returns>
        public int SetBuild(BuildVModel build)
        {
            return dal.SetBuild(build);
        }

        /// <summary>
        /// 删除建筑信息
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public int DelBuild(int co_id)
        {
            return dal.DelBuild(co_id);
        }
    }
    
}
