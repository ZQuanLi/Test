using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Syscont
{
    public partial class ExpRoleBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Exp.Syscont.ExpRoleDAL dal = null;
        public ExpRoleBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Exp.Syscont.ExpRoleDAL(_ledger, _uid);
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetRole(int role_id)
        {
            DataTable dtSource = dal.GetRole(role_id);
            dtSource.Columns.Add("RowId", typeof(System.Int32));
            int RowId = 0;
            foreach (DataRow dr in dtSource.Rows)
                dr["RowId"] = ++RowId;
            return dtSource;
        }

        /// <summary>
        /// 新增修改角色信息
        /// </summary>
        /// <param name="sys_role"></param>
        public void EditRole(sys_role sys_role, int nAct)
        {
            if (nAct == 1)
                dal.AddRole(sys_role);
            else if (nAct == 2)
                dal.EditRole(sys_role);
            else
                throw new Exception("操作类型错误");
        }
        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="Role_id"></param>
        public void DelRole(int Role_id)
        {
            dal.DelRole(Role_id);
        }

        /// <summary>
        /// 获取基本权限
        /// </summary>
        /// 
        /// <returns></returns>
        public List<Treeview> GetPowerById(int id, out int total)
        {
            DataTable dtSource = dal.GetPowerById(id);
            total = dtSource.Rows.Count;
            return this.GetMenuList(dtSource);
        }

        /// <summary>
        /// 设置基本权限
        /// </summary>
        /// <param name="pRole_id">权限ID</param>
        /// <param name="ids">选中的ID号（多个用逗号拼接）</param>
        public int SetPower(int role_id, string ids)
        {
            return dal.SetPower(role_id, ids);
        }
        

        private List<Treeview> GetMenuList(DataTable dtSource)
        {
            List<Treeview> rst = new List<Treeview>();
            DataRow[] pArr = dtSource.Select("(parent_no='0' or parent_no='')", "menu_no");
            foreach (DataRow dr in pArr)
            {
                Treeview pTr = new Treeview();
                pTr.id = CommFunc.ConvertDBNullToString(dr["prog_id"]);
                pTr.text = CommFunc.ConvertDBNullToString(dr["descr"]);
                pTr.attributes = CommFunc.ConvertDBNullToInt32(dr["power"]);
                pTr.nodes = new List<Treeview>();
                this.GetMenuList(ref dtSource, ref pTr, CommFunc.ConvertDBNullToString(dr["menu_no"]));
                rst.Add(pTr);
            }
            return rst;
        }

        private void GetMenuList(ref DataTable dtSource, ref Treeview pTr, string menu_no)
        {
            DataRow[] pArr = dtSource.Select("parent_no='" + menu_no + "'", "menu_no");
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
                this.GetMenuList(ref dtSource, ref cTr, CommFunc.ConvertDBNullToString(dr["menu_no"]));
            }
        }


        /// <summary>
        /// 获取基本权限
        /// </summary>
        /// 
        /// <returns></returns>
        public List<Treeview> GetAreaById(int id, string project, out int total)
        {
            DataTable dtSource = dal.GetAreaById2();            
            DataTable dtSource2 = dal.GetAreaById(id, project);
            total = dtSource.Rows.Count;
            string power = dtSource2.Rows.Count == 0 ? "" : CommFunc.ConvertDBNullToString(dtSource2.Rows[0]["_area"]);
            return this.GetMenuList2(dtSource, power); ;
        }
        public DataTable GetAreaById2()
        {
            DataTable dtSource = dal.GetAreaById2();
            return dtSource;
        }

        private List<Treeview> GetMenuList2(DataTable dtSource, string power)
        {
            List<Treeview> rst = new List<Treeview>();
            DataRow[] pArr = dtSource.Select("Parent_id=0", "CoNo");
            string[] powers = power.Split(',');
            foreach (DataRow dr in pArr)
            {
                Treeview pTr = new Treeview();
                pTr.id = CommFunc.ConvertDBNullToString(dr["Co_id"]);
                pTr.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                pTr.attributes = powers.ToList().Exists(p => p.Trim().Equals(pTr.id));
                pTr.nodes = new List<Treeview>();
                this.GetMenuList2(ref dtSource, ref pTr, CommFunc.ConvertDBNullToString(dr["Co_id"]), ref powers);
                rst.Add(pTr);
            }
            return rst;
        }

        private void GetMenuList2(ref DataTable dtSource, ref Treeview pTr, string Bd_id, ref string[] powers)
        {
            DataRow[] pArr = dtSource.Select("Parent_id=" + Bd_id, "CoNo");
            int pRows = pArr.Count();
            if (pRows > 0)
                pTr.nodes = new List<Treeview>();
            foreach (DataRow dr in pArr)
            {
                Treeview cTr = new Treeview();
                cTr.id = CommFunc.ConvertDBNullToString(dr["Co_id"]);
                cTr.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                cTr.attributes = powers.ToList().Exists(p => p.Trim().Equals(cTr.id));
                pTr.nodes.Add(cTr);
                this.GetMenuList2(ref dtSource, ref cTr, CommFunc.ConvertDBNullToString(dr["Co_id"]), ref powers);
            }
        }

        /// <summary>
        /// 设置区域权限
        /// </summary>
        /// <param name="pRole_id">权限ID</param>
        /// <param name="ids">选中的ID号（多个用逗号拼接）</param>
        public int SetPowerArea(int role_id, string ids, string Project)
        {
            return dal.SetPowerArea(role_id, ids, Project);
        }


    }
}
