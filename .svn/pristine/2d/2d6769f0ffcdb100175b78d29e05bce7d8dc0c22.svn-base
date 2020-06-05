using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Platform.Home
{
    public class HomeBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Platform.Home.HomeDAL dal = null;
        public HomeBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Platform.Home.HomeDAL(_ledger, _uid);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSys_user(string uSign)
        {
            return dal.GetSys_user(uSign);
        }

        public int UpdateLoginErr(string uSign)
        {
            return dal.UpdateLogin(uSign, 1);
        }

        public int UpdateLoginSue(string uSign)
        {
            return dal.UpdateLogin(uSign, 0);
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="role_id"></param>
        /// <param name="prog_id"></param>
        /// <returns></returns>
        public bool GetPower(int role_id, string prog_id)
        {
            return dal.GetPower(role_id, prog_id);
        }



        /// <summary>
        /// 获取子系统信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSubSystem()
        {
            return dal.GetSubSystem();
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="project">项目名称</param>
        /// <returns></returns>
        public List<Treeview> GetMenuList(string project, out int total)
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
                pTr.icon = CommFunc.ConvertDBNullToString(dr["icon"]);
                string child = this.GetPower(pTr.id, ref dtSource);
                int isShow = (CommFunc.ConvertDBNullToInt32(dr["pass"]) > 0 ? 1 : 0);
                pTr.nodes = new List<Treeview>();
                int cc = this.GetMenuTreeList(ref dtSource, ref pTr);
                isShow = cc == 1 ? 1 : isShow;
                pTr.attributes = new { path = CommFunc.ConvertDBNullToString(dr["path"]), power = CommFunc.ConvertDBNullToString(dr["Power"]), show = isShow, child = child };
                rst.Add(pTr);
            }
            return rst;
        }

        private int GetMenuTreeList(ref DataTable dtSource, ref Treeview pTr)
        {
            int rst = 0;
            DataRow[] pArr = dtSource.Select("parent_no='" + pTr.id + "' and attrib=0", "ordno,menu_no");
            int pRows = pArr.Count();
            if (pRows > 0)
                pTr.nodes = new List<Treeview>();
            foreach (DataRow dr in pArr)
            {
                Treeview cTr = new Treeview();
                cTr.id = CommFunc.ConvertDBNullToString(dr["menu_no"]);
                cTr.text = CommFunc.ConvertDBNullToString(dr["descr"]);
                cTr.icon = CommFunc.ConvertDBNullToString(dr["icon"]);
                string child = this.GetPower(cTr.id, ref dtSource);
                int isShow = (CommFunc.ConvertDBNullToInt32(dr["pass"]) > 0 ? 1 : 0);
                rst = isShow == 1 ? 1 : rst;              
                pTr.nodes.Add(cTr);
                int cc = this.GetMenuTreeList(ref dtSource, ref cTr);
                isShow = cc == 1 ? 1 : isShow;
                cTr.attributes = new { path = CommFunc.ConvertDBNullToString(dr["path"]), power = CommFunc.ConvertDBNullToString(dr["Power"]), show = isShow, child = child };
            }
            return rst;
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
