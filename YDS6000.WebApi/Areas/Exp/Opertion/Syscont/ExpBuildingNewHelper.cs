using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Opertion.Syscont
{
    public partial class ExpBuildingNewHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Syscont.ExpBuildingNewBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpBuildingNewHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Syscont.ExpBuildingNewBLL(user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 获取组织树形结构
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdCustOnCoInfoList()
        {
            APIRst rst = new APIRst();
            try
            {
                int total = 0;
                List<Treeview> dt = bll.GetYdCustOnCoInfoList(0, out total);
                object obj = new { total = total, rows = dt };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取组织树形结构错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 获取组织信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdCustOnCoInfo(int Co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdCustOnCoInfo(Co_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Parent_id = CommFunc.ConvertDBNullToInt32(s1["Parent_id"]),
                               Mobile = CommFunc.ConvertDBNullToString(s1["Mobile"]),
                               Office_tel = CommFunc.ConvertDBNullToString(s1["Office_tel"]),
                               Email = CommFunc.ConvertDBNullToString(s1["Email"]),
                               CustAddr = CommFunc.ConvertDBNullToString(s1["CustAddr"]),
                               IsDefine = CommFunc.ConvertDBNullToInt32(s1["IsDefine"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               PCoName = CommFunc.ConvertDBNullToString(s1["PCoName"]),
                               Attrib = CommFunc.ConvertDBNullToString(s1["Attrib"]),

                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取组织信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取用户的用能列表
        /// </summary>
        /// <param name="Co_id"></param>
        /// <returns></returns>
        public APIRst GetYdCustOnMdList(int Co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                Co_id = Co_id == 0 ? -9999 : Co_id;
                DataTable dtSource = bll.GetYdCustOnMdList(Co_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               _parentId = CommFunc.ConvertDBNullToInt32(s1["pid"]),
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               ModuleType = CommFunc.ConvertDBNullToString(s1["ModuleType"]),
                               Parent_id = CommFunc.ConvertDBNullToString(s1["Parent_id"]),
                               Inst_loc = CommFunc.ConvertDBNullToString(s1["Inst_loc"]),
                               Multiply = CommFunc.ConvertDBNullToDecimal(s1["Multiply"]).ToString("f2"),
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Struc = CommFunc.ConvertDBNullToString(s1["GwName"]) + "->" + CommFunc.ConvertDBNullToString(s1["EspName"]),
                               //Waste = CommFunc.ConvertDBNullToDecimal(s1["Waste"]),
                               //Fees = CommFunc.ConvertDBNullToDecimal(s1["Fees"]),
                               MinPay = CommFunc.ConvertDBNullToDecimal(s1["MinPay"]),
                               Price = CommFunc.ConvertDBNullToDecimal(s1["Price"]),
                               //IsPSum = CommFunc.ConvertDBNullToInt32(s1["IsPSum"]),
                               //Format = CommFunc.ConvertDBNullToInt32(s1["Format"]),
                               //DataCfg = CommFunc.ConvertDBNullToString(s1["DataCfg"]),
                               Rate_id = CommFunc.ConvertDBNullToInt32(s1["Rate_id"]),
                               IsAlarm = CommFunc.ConvertDBNullToInt32(s1["IsAlarm"]),
                               AlarmVal = CommFunc.ConvertDBNullToDecimal(s1["AlarmVal"]),
                               EnergyItemCode = CommFunc.ConvertDBNullToString(s1["EnergyItemCode"]),
                               EnergyItemName = CommFunc.ConvertDBNullToString(s1["EnergyItemName"]),

                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取用户的用能列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 保存公司信息
        /// </summary>
        /// <param name="Type">空=A,新增建筑=B,新增房间=H</param>
        /// <param name="PCoChk">父ID号</param>
        /// <param name="v1_CustInfo">Co_id主键ID:0新增,CoName:客户名称,Parent_id:父ID号,Disabled:是否弃用0否1是,CustAddr:客户地址,Office_tel:办公电话,Mobile:移动电话,Email,IsDefine:定义属性默认0</param>
        /// <returns></returns>
        public APIRst SetSaveCo(string Type, int PCoChk, v1_custInfoVModel v1_CustInfo)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Type == "A") v1_CustInfo.Attrib = 0;
                if (Type == "B") v1_CustInfo.Attrib = 100;
                if (Type == "H") v1_CustInfo.Attrib = 9000;
                if (PCoChk == 0 && v1_CustInfo.Co_id == 0)
                    v1_CustInfo.Parent_id = 0;
                if (PCoChk == 1 && v1_CustInfo.Parent_id == 0)
                {
                    throw new Exception("父ID号错误");
                }
                var sameNameCoid = CommFunc.ConvertDBNullToInt32(bll.IsExistSameYdCustName(v1_CustInfo.CoName));
                if ((v1_CustInfo.Co_id == 0 && sameNameCoid > 0) || (v1_CustInfo.Co_id > 0 && sameNameCoid != 0 && sameNameCoid != v1_CustInfo.Co_id))
                {
                    throw new Exception("该用户名称已经存在");
                }
                bll.SetCustInfo(v1_CustInfo);
                VEasyUiTree tInfo = new VEasyUiTree();
                tInfo.id = v1_CustInfo.Co_id.ToString();
                tInfo.text = v1_CustInfo.CoName;
                //object obj = new { total = dtSource.Rows.Count, data = new { tt = tInfo, pid = cust.Parent_id } };
                rst.data = new { tt = tInfo, pid = v1_CustInfo.Parent_id };
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("修改组织信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Co_id">主键ID号</param>
        /// <returns></returns>
        public APIRst GeDelCo(int Co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Co_id == 0)
                    throw new Exception("ID不能为空");
                int cc = bll.GeDelCo(Co_id);
                rst.data = cc;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除组织信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 查找用能列表
        /// </summary>
        /// <param name="Co_id">主键</param>
        /// <param name="FindStr">电能表地址</param>
        /// <returns></returns>
        public APIRst GetYdCustOnMdInFind(int Co_id, string FindStr)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdCustOnMdInFind(Co_id, FindStr);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["Struc"]) + "->" + CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("查找电能表数据信息错误(GetCoInMdFind):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取计量点有序用电总加组信息
        /// </summary>
        /// <param name="Module_id"></param>
        /// <returns></returns>
        public APIRst GetYdCustInMdFormula(int Module_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdCustInMdFormula(Module_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取计量点有序用电总加组信息错误(GetYdCustInMdFormula):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取计量点有序用电可进行总加组信息
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Module_id"></param>
        /// <param name="FindStr"></param>
        /// <returns></returns>
        public APIRst GetYdCustInMdFormulaFind(int Co_id, int Module_id, string FindStr)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdCustInMdFormulaFind(Co_id, Module_id, FindStr);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               ModuleAddr = CommFunc.ConvertDBNullToString(s1["Struc"]) + "->" + CommFunc.ConvertDBNullToString(s1["ModuleAddr"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取计量点有序用电可进行总加组信息(GetYdCustInMdFormulaList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 修改电能表
        /// </summary>
        /// <param name="Module_id"></param>
        /// <param name="energyType"></param>
        /// <param name="Rate_id"></param>
        /// <param name="Price"></param>
        /// <param name="AlarmVal"></param>
        /// <param name="IsAlarm"></param>
        /// <returns></returns>
        public APIRst SaveMdSet(int Module_id, string energyType, int Rate_id, decimal Price, decimal AlarmVal, int IsAlarm)
        {
            APIRst rst = new APIRst();
            try
            {
                int dtSource = bll.Setv1_gateway_esp_module_info(Module_id, energyType, Rate_id, Price, AlarmVal, IsAlarm);
                rst.data = dtSource;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("保存电能表错误(SaveMdSet):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置用能列表
        /// </summary>
        /// <param name="Co_id">公司ID</param>
        /// <param name="IdStr">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        public APIRst YdCustOnMdInSave(int Co_id, string IdStr)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Co_id == 0)
                    throw new Exception("公司ID错误");
                int dtSource = bll.YdCustOnMdInSave(Co_id, IdStr);
                rst.data = dtSource;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置用能列表(YdCustOnMdInSave):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置有序用电总加组
        /// </summary>
        /// <param name="Co_id">公司ID</param>
        /// <param name="Module_id">计量点ID</param>
        /// <param name="IdStr">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        public APIRst SetYdCustInMdFormula(int Co_id, int Module_id, string IdStr)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Module_id == 0)
                    throw new Exception("计量点ID错误");
                int dtSource = bll.SetYdCustInMdFormula(Co_id, Module_id, IdStr);
                rst.data = dtSource;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("计量点设置所属公司信息错误(SetYdCustInMdFormula):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 分类分项编码下拉列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetEnergyItemCode()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetEnergyItemCode();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               EnergyItemCode = CommFunc.ConvertDBNullToString(s1["EnergyItemCode"]),
                               EnergyItemName = CommFunc.ConvertDBNullToString(s1["EnergyItemName"])
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取分类分项编码下拉列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        #region 建筑明细信息

        /// <summary>
        /// 建筑详细信息
        /// </summary>
        /// <param name="Co_id"></param>
        /// <returns></returns>
        public APIRst GetYdCustInfoBuildData(int Co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdCustInfoBuildData(Co_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CustName = CommFunc.ConvertDBNullToString(s1["CustName"]),
                               CorpName = CommFunc.ConvertDBNullToString(s1["CorpName"]),
                               Area = CommFunc.ConvertDBNullToDecimal(s1["Area"]),
                               BuildType = CommFunc.ConvertDBNullToString(s1["BuildType"]),
                               Bank = CommFunc.ConvertDBNullToInt32(s1["Bank"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取分类分项编码下拉列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 保存建筑详细信息
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Area"></param>
        /// <param name="BuildType"></param>
        /// <param name="CustName"></param>
        /// <param name="CorpName"></param>
        /// <param name="Bank"></param>
        /// <returns></returns>
        public APIRst SaveYdCustInfoBuildData(int Co_id, decimal Area, string BuildType, string CustName, string CorpName, int Bank)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Co_id == 0)
                    throw new Exception("公司ID错误");
                int dtSource = bll.SaveYdCustInfoBuildData(Co_id, Area, BuildType, CustName, CorpName, Bank);
                rst.data = dtSource;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("保存建筑详细信息(SaveYdCustInfoBuildData):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        #endregion


        #region 物业费用标准

        /// <summary>
        /// 获取物业费用标准
        /// </summary>
        /// <param name="Co_id"></param>
        /// <returns></returns>
        public APIRst GetYdCustInfoBuild_fee(int Co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdCustInfoBuild_fee(Co_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Cic_id = CommFunc.ConvertDBNullToInt32(s1["Cic_id"]),
                               Rate_id = CommFunc.ConvertDBNullToInt32(s1["Rate_id"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                               Rule = CommFunc.ConvertDBNullToInt32(s1["Rule"]),
                               Unit = CommFunc.ConvertDBNullToString(s1["Unit"]),
                               UnitBase = CommFunc.ConvertDBNullToDecimal(s1["UnitBase"]),
                               CicName = CommFunc.ConvertDBNullToString(s1["CicName"]),
                               RateName = CommFunc.ConvertDBNullToString(s1["RateName"]),
                               RuleName = CommFunc.GetEnumDisplay(typeof(RateRule), CommFunc.ConvertDBNullToInt32(s1["Rule"])),
                               UnitName = CommFunc.GetEnumDisplay(typeof(RateUnit), CommFunc.ConvertDBNullToString(s1["Unit"])),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取物业明细信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 保存获取物业费用标准
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Cic_id"></param>
        /// <param name="Rate_id"></param>
        /// <returns></returns>
        public APIRst SaveYdCustInfoBuildFee(int Co_id, int Cic_id, int Rate_id)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Co_id == 0)
                    throw new Exception("ID号为空");
                if (Cic_id == 0)
                    throw new Exception("收费项ID号为空");
                int dtSource = bll.SaveYdCustInfoBuildFee(Co_id, Cic_id, Rate_id);
                rst.data = dtSource;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("保存物业明细信息错误(SaveYdCustInfoBuildFee):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        #endregion

        /// <summary>
        /// 获取首页建筑数
        /// </summary>
        /// <returns></returns>
        public APIRst GetHomeBuilding(int Attrib)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetHomeBuilding(Attrib);
                //var res1 = from s1 in dtSource.AsEnumerable()
                //           select new
                //           {
                //               Cic_id = CommFunc.ConvertDBNullToInt32(s1["Cic_id"]),
                //               Rate_id = CommFunc.ConvertDBNullToInt32(s1["Rate_id"]),
                //               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                //               Rule = CommFunc.ConvertDBNullToInt32(s1["Rule"]),
                //               Unit = CommFunc.ConvertDBNullToString(s1["Unit"]),
                //               UnitBase = CommFunc.ConvertDBNullToDecimal(s1["UnitBase"]),
                //               CicName = CommFunc.ConvertDBNullToString(s1["CicName"]),
                //               RateName = CommFunc.ConvertDBNullToString(s1["RateName"]),
                //               RuleName = CommFunc.GetEnumDisplay(typeof(RateRule), CommFunc.ConvertDBNullToInt32(s1["Rule"])),
                //               UnitName = CommFunc.GetEnumDisplay(typeof(RateUnit), CommFunc.ConvertDBNullToString(s1["Unit"])),
                //           };
                object obj = new { Total = dtSource.Rows.Count };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取物业明细信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


    }
}