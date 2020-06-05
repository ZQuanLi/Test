using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Platform.User
{
    partial class UserBLL
    {
        public DataTable GetRoleList()
        {
            return dal.GetRoleList();
        }

        /// <summary>
        /// 设置角色信息
        /// </summary>
        /// <param name="sys_role">角色类型</param>
        /// <returns></returns>
        public int SetRole(sys_role role)
        {
            return dal.SetRole(role);
        }

        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="role_id">角色ID号</param>
        /// <returns></returns>
        public int DelRole(int role_id)
        {
            return dal.DelRole(role_id);
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="role_id">角色ID号</param>
        /// <param name="project"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<Treeview> GetMenuOnOperateList(int role_id,string project, out int total)
        {
            DataTable dtSource = dal.GetMenuOnOperateList(role_id, project);
            total = dtSource.Rows.Count;
            return this.GetMenuOnOperateList(dtSource);
        }

        private List<Treeview> GetMenuOnOperateList(DataTable dtSource)
        {
            List<Treeview> rst = new List<Treeview>();
            DataRow[] pArr = dtSource.Select("(parent_no='0' or parent_no='')", "ordno,menu_no");
            foreach (DataRow dr in pArr)
            {
                Treeview pTr = new Treeview();
                pTr.id = CommFunc.ConvertDBNullToString(dr["prog_id"]);
                pTr.text = CommFunc.ConvertDBNullToString(dr["descr"]);
                pTr.attributes = CommFunc.ConvertDBNullToInt32(dr["power"]);
                pTr.nodes = new List<Treeview>();
                this.GetMenuOnOperateList(ref dtSource, ref pTr, CommFunc.ConvertDBNullToString(dr["menu_no"]));
                rst.Add(pTr);
            }
            return rst;
        }

        private void GetMenuOnOperateList(ref DataTable dtSource, ref Treeview pTr,string menu_no)
        {
            DataRow[] pArr = dtSource.Select("parent_no='" + menu_no + "'", "ordno,menu_no");
            int pRows = pArr.Count();
            if (pRows > 0)
                pTr.nodes = new List<Treeview>();
            foreach (DataRow dr in pArr)
            {
                Treeview cTr = new Treeview();
                cTr.id = CommFunc.ConvertDBNullToString(dr["prog_id"]);
                cTr.text = CommFunc.ConvertDBNullToString(dr["descr"]);
                cTr.attributes = CommFunc.ConvertDBNullToInt32(dr["power"]);
                pTr.nodes.Add(cTr);
                this.GetMenuOnOperateList(ref dtSource, ref cTr, CommFunc.ConvertDBNullToString(dr["menu_no"]));
            }
        }

        public int SaveMenuOnOperateList(int role_id, string ids)
        {
            return dal.SaveMenuOnOperateList(role_id, ids);
        }
    }
}
