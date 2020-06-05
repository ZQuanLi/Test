using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;

namespace YDS6000.BLL.Exp.Syscont
{
    public partial class ExpBuildingNewBLL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        private readonly YDS6000.DAL.Exp.Syscont.ExpBuildingNewDAL dal = null;
        public ExpBuildingNewBLL(int _ledger, int _uid)
        {
            this.Ledger = _ledger;
            this.SysUid = _uid;
            dal = new YDS6000.DAL.Exp.Syscont.ExpBuildingNewDAL(_ledger, _uid);
        }
        /// <summary>
        /// 获取费率信息
        /// </summary>
        /// <param name="Co_id"></param>
        /// <returns></returns>
        public List<Treeview> GetYdCustOnCoInfoList(int Co_id, out int total)
        {
            DataTable dtSource = dal.GetYdCustOnCoInfoList(Co_id);
            total = dtSource.Rows.Count;
            return this.GetMenuList(dtSource);
        }

        private List<Treeview> GetMenuList(DataTable dtSource)
        {
            List<Treeview> rst = new List<Treeview>();
            DataRow[] pArr = dtSource.Select("Parent_id=0", "Co_id");
            foreach (DataRow dr in pArr)
            {
                Treeview pTr = new Treeview();
                pTr.id = CommFunc.ConvertDBNullToString(dr["Co_id"]);
                pTr.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                //pTr.attributes = CommFunc.ConvertDBNullToInt32(dr["power"]);
                pTr.nodes = new List<Treeview>();
                this.GetMenuList(ref dtSource, ref pTr, CommFunc.ConvertDBNullToString(dr["Co_id"]));
                rst.Add(pTr);
            }
            return rst;
        }

        private void GetMenuList(ref DataTable dtSource, ref Treeview pTr, string Co_id)
        {
            DataRow[] pArr = dtSource.Select("Parent_id=" + Co_id, "Co_id");
            int pRows = pArr.Count();
            if (pRows > 0)
                pTr.nodes = new List<Treeview>();
            foreach (DataRow dr in pArr)
            {
                Treeview cTr = new Treeview();
                cTr.id = CommFunc.ConvertDBNullToString(dr["Co_id"]);
                cTr.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                //cTr.attributes = CommFunc.ConvertDBNullToInt32(dr["power"]);
                pTr.nodes.Add(cTr);
                this.GetMenuList(ref dtSource, ref cTr, CommFunc.ConvertDBNullToString(dr["Co_id"]));
            }
        }

        /// <summary>
        /// 获取全部客户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdCustOnCoInfo(int Co_id)
        {
            return dal.GetYdCustOnCoInfoList(Co_id);
        }

        /// <summary>
        /// 获取用户的用能列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetYdCustOnMdList(int Co_id)
        {
            DataTable dtSource = dal.GetYdCustOnMdList(Co_id);
            dtSource.Columns.Add("pid", typeof(System.Int32));
            foreach (DataRow dr in dtSource.Select("Parent_id<>0"))
            {
                dr["pid"] = dr["Parent_id"];
                if (dtSource.Select("Module_id=" + CommFunc.ConvertDBNullToInt32(dr["Parent_id"])).Count() == 0)
                    dr["pid"] = 0;
            }
            return dtSource;
        }

        /// <summary>
        ///判断公司名称是否存在
        /// </summary>
        /// <param name="FindName"></param>
        /// <returns>存在，返回存在ID，否则返回0</returns>
        public int IsExistSameYdCustName(string FindName)
        {
            return dal.IsExistSameYdCustName(FindName);
        }

        /// <summary>
        /// 新增修改用户
        /// </summary>
        /// <param name="cust"></param>
        /// <returns></returns>
        public int SetCustInfo(v1_custInfoVModel cust)
        {
            return dal.SetCustInfo(cust);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Co_id">主键ID号</param>
        /// <returns></returns>
        public int GeDelCo(int Co_id)
        {
            return dal.GeDelCo(Co_id);
        }

        /// <summary>
        /// 查找用能列表
        /// </summary>
        /// <param name="Co_id">主键</param>
        /// <param name="FindStr">电能表地址</param>
        /// <returns></returns>
        public DataTable GetYdCustOnMdInFind(int Co_id, string FindStr)
        {
            return dal.GetYdCustOnMdInFind(Co_id, FindStr);
        }

        /// <summary>
        /// 获取计量点的总加组设置
        /// </summary>
        /// <param name="Module_id"></param>
        /// <returns></returns>
        public DataTable GetYdCustInMdFormula(int Module_id)
        {
            return dal.GetYdCustInMdFormula(Module_id);
        }

        /// <summary>
        /// 获取计量点有序用电可进行总加组信息
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Module_id"></param>
        /// <param name="FindStr"></param>
        /// <returns></returns>
        public DataTable GetYdCustInMdFormulaFind(int Co_id, int Module_id, string FindStr)
        {
            return dal.GetYdCustInMdFormulaFind(Co_id, Module_id, FindStr);
        }

        /// <summary>
        /// 单独设置电能表
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Module_id"></param>
        /// <param name="FindStr"></param>
        /// <returns></returns>
        public int Setv1_gateway_esp_module_info(int Module_id, string EnergyItemCode, int Rate_id, decimal Price, decimal AlarmVal, int IsAlarm)
        {
            return dal.Setv1_gateway_esp_module_info(Module_id, EnergyItemCode, Rate_id, Price, AlarmVal, IsAlarm);
        }
        /// <summary>
        /// 设置电表所属公司
        /// </summary>
        /// <param name="IdStr">选中的ID号（多个用逗号拼接）</param>
        /// <param name="Co_id">公司ID</param>
        /// <returns></returns>
        public int YdCustOnMdInSave(int Co_id, string IdStr)
        {
            return dal.YdCustOnMdInSave(Co_id, IdStr);
        }

        /// <summary>
        /// 保存有序用电总加总组
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Module_id"></param>
        /// <param name="IdStr"></param>
        /// <returns></returns>
        public int SetYdCustInMdFormula(int Co_id, int Module_id, string IdStr)
        {
            return dal.SetYdCustInMdFormula( Co_id, Module_id, IdStr);
        }

        // 分类分项编码下拉列表
        public DataTable GetEnergyItemCode()
        {
            return dal.GetEnergyItemCode();
        }


        #region 建筑明细信息
        public DataTable GetYdCustInfoBuildData(int co_id)
        {
            return dal.GetYdCustInfoBuildData(co_id);
        }
        public int SaveYdCustInfoBuildData(int Co_id, decimal Area, string BuildType, string CustName, string CorpName, int Bank)
        {
            return dal.SaveYdCustInfoBuildData(Co_id, Area, BuildType, CustName, CorpName, Bank);
        }
        #endregion

        #region 物业信息
        public DataTable GetYdCustInfoBuild_fee(int Co_id)
        {
            return dal.GetYdCustInfoBuild_fee(Co_id);
        }

        public int SaveYdCustInfoBuildFee(int Co_id, int Cic_id, int Rate_id)
        {
            return dal.SaveYdCustInfoBuildFee(Co_id, Cic_id, Rate_id);
        }
        #endregion

        public DataTable GetHomeBuilding(int Attrib)
        {
            return dal.GetHomeBuilding(Attrib);
        }
    }
}
