using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;
using YDS6000.DAL.Menu;

namespace YDS6000.BLL.Menu
{
    public partial class MenuBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly MenuDAL dal = null;
        public MenuBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new MenuDAL(_ledger, _uid);
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="project">项目名称</param>
        /// <returns></returns>
        public List<Treeview> GetMenuList(string project,out int total)
        {
            DataTable dtSource = dal.GetMenuList(project);
            total = dtSource.Rows.Count;
            return this.GetMenuTreeList(dtSource);
        }

        private List<Treeview> GetMenuTreeList(DataTable dtSource)
        {
            List<Treeview> rst = new List<Treeview>();
            DataRow[] pArr = dtSource.Select("(parent_no='0' or parent_no='') and attrib=0", "ordno,menu_no");
            foreach (DataRow dr in pArr)
            {
                
                Treeview pTr = new Treeview();
                pTr.id = CommFunc.ConvertDBNullToString(dr["menu_no"]);
                pTr.text = CommFunc.ConvertDBNullToString(dr["descr"]);
                string child = this.GetPower(pTr.id,ref dtSource);
                pTr.attributes = new { path = CommFunc.ConvertDBNullToString(dr["path"]), power = CommFunc.ConvertDBNullToString(dr["Power"]), child = child };
                pTr.nodes = new List<Treeview>();
                this.GetMenuTreeList(ref dtSource, ref pTr);
                rst.Add(pTr);
            }
            return rst;
        }

        private void GetMenuTreeList(ref DataTable dtSource, ref Treeview pTr)
        {
            DataRow[] pArr = dtSource.Select("parent_no='" + pTr.id + "' and attrib=0", "ordno,menu_no");
            int pRows = pArr.Count();
            if (pRows > 0)
                pTr.nodes = new List<Treeview>();
            foreach (DataRow dr in pArr)
            {
                Treeview cTr = new Treeview();
                cTr.id = CommFunc.ConvertDBNullToString(dr["menu_no"]);
                cTr.text = CommFunc.ConvertDBNullToString(dr["descr"]);
                string child = this.GetPower(cTr.id, ref dtSource);
                cTr.attributes = new { path = CommFunc.ConvertDBNullToString(dr["path"]), power = CommFunc.ConvertDBNullToString(dr["Power"]), child = child };
                pTr.nodes.Add(cTr);
                this.GetMenuTreeList(ref dtSource, ref cTr);
            }
        }

        private string GetPower(string parent_id, ref DataTable dtSource)
        {
            StringBuilder strJson = new StringBuilder();
            foreach (DataRow dr in dtSource.Select("parent_no='" + parent_id + "' and attrib=1"))
            {
                if (!string.IsNullOrEmpty(strJson.ToString()))
                    strJson.Append(",");
                strJson.Append(CommFunc.ConvertDBNullToString(dr["prog_id"]) + ":{" + CommFunc.ConvertDBNullToString(dr["Power"]) + "}");
            }
            return strJson.ToString();
        }
    }
}
