using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Opertion.Syscont
{
    public partial class ExpModelsHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Syscont.ExpModelsBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpModelsHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Syscont.ExpModelsBLL(user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 获取设备型号
        /// </summary>
        /// <param name="ModuleType">筛选条件：设备类型</param>
        /// <returns></returns>
        public APIRst GetYdModelsOnList(string ModuleType)
        {
            APIRst rst = new APIRst();
            try
            {
                var dt = bll.GetYdModelsOnList(ModuleType);
                int total = dt.Rows.Count;
                var dtsource = from s1 in dt.AsEnumerable()
                               select new
                               {
                                   RowId = dt.Rows.IndexOf(s1) + 1,
                                   Mm_id = CommFunc.ConvertDBNullToInt32(s1["Mm_id"]),
                                   ModuleType = CommFunc.ConvertDBNullToString(s1["ModuleType"]),
                                   ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                                   Spec = CommFunc.ConvertDBNullToString(s1["Spec"]),
                                   Fty_prod = CommFunc.ConvertDBNullToString(s1["Fty_prod"]),
                                   IsCharg = CommFunc.ConvertDBNullToInt32(s1["IsCharg"]),
                                   ModulePwd = CommFunc.ConvertDBNullToString(s1["ModulePwd"]),
                                   ModuleUid = CommFunc.ConvertDBNullToString(s1["ModuleUid"]),
                                   Level = CommFunc.ConvertDBNullToString(s1["Level"]),
                                   Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               };
                object obj = new { total = total, rows = dtsource.ToList() };
                rst.data = obj;
                //rst.data = dtsource.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取充值参数信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 修改设备型号
        /// </summary>
        /// <returns></returns>
        public APIRst GetYdModelsOfEditOnModelList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dt = XMLCofig.GetDrive();
                var dtsource = from s1 in dt.AsEnumerable()
                               select new
                               {
                                   id = CommFunc.ConvertDBNullToString(s1["type"]),
                                   text = CommFunc.ConvertDBNullToString(s1["type"]),
                               };
                rst.data = dtsource.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取充值参数信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取采集信息
        /// </summary>
        /// <param name="mm_id">类型ID号</param>
        /// <param name="ModuleType">设备类型</param>
        /// <returns></returns>
        public APIRst GetYdModelsOfEditOnList(int mm_id, string ModuleType)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = XMLCofig.GetDrive(ModuleType);
                dtSource.Clear();
                dtSource.Columns.Add("RowId", typeof(System.Int32));
                dtSource.Columns.Add("Fun_id", typeof(System.Int32));
                dtSource.Columns.Add("Mm_id", typeof(System.Int32));
                dtSource.Columns.Add("DataValue", typeof(System.String));
                dtSource.Columns.Add("Disabled", typeof(System.Int32));
                DataTable dtDb = bll.GetYdModelsOfEditOnList(mm_id);
                int rowId = 0;
                foreach (DataRow dr in dtSource.Rows)
                {
                    dr["RowId"] = ++rowId;
                    dr["Fun_id"] = 0;
                    dr["Mm_id"] = mm_id;
                    foreach (DataRow drDb in dtDb.Select("FunType='" + CommFunc.ConvertDBNullToString(dr["FunType"]) + "'"))
                    {
                        drDb["nAct"] = 2;
                        dr["Fun_id"] = drDb["Fun_id"];
                        dr["FunName"] = drDb["FunName"];
                        dr["DataValue"] = drDb["DataValue"];
                        dr["Disabled"] = drDb["Disabled"];
                    }
                }
                foreach (DataRow dr in dtDb.Select("nAct=0"))
                {
                    DataRow addDr = dtSource.NewRow();
                    addDr["RowId"] = ++rowId;
                    addDr["Type"] = ModuleType;
                    foreach (DataColumn dc in dtDb.Columns)
                    {
                        if (dtSource.Columns.Contains(dc.ColumnName))
                            addDr[dc.ColumnName] = dr[dc.ColumnName];
                    }
                    dtSource.Rows.Add(addDr);
                }
                int total = dtSource.Rows.Count;
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = CommFunc.ConvertDBNullToInt32(s1["RowId"]),
                               Fun_id = CommFunc.ConvertDBNullToInt32(s1["Fun_id"]),
                               Mm_id = CommFunc.ConvertDBNullToInt32(s1["Mm_id"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               FunType = CommFunc.ConvertDBNullToString(s1["FunType"]),
                               DataValue = CommFunc.ConvertDBNullToString(s1["DataValue"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               Action = CommFunc.ConvertDBNullToInt32(s1["Action"]),
                               DataType = CommFunc.ConvertDBNullToInt32(s1["DataType"]),
                               Scale = CommFunc.ConvertDBNullToInt32(s1["Scale"]),
                               OrdNo = CommFunc.ConvertDBNullToInt32(s1["OrdNo"]),
                               @checked = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               Protocol = CommFunc.ConvertDBNullToString(s1["Protocol"]),
                               OrdGrp = CommFunc.ConvertDBNullToInt32(s1["OrdGrp"]),
                               Unit = CommFunc.ConvertDBNullToString(s1["Unit"]),
                               PointType = CommFunc.ConvertDBNullToInt32(s1["PointType"]),
                               AlarmModel = CommFunc.ConvertDBNullToInt32(s1["AlarmModel"]),
                               SubTab = CommFunc.ConvertDBNullToString(s1["SubTab"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取充值参数信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 新增采集信息
        /// </summary>
        /// <param name="Mm_id">类型ID号</param>
        /// <param name="Fun_id">功能ID号</param>
        /// <param name="FunType">采集类型</param>
        /// <param name="FunName">采集项</param>
        /// <param name="Action">读写：0=读，1=写，3=读写</param>
        /// <returns></returns>
        public APIRst SetYdModelsOnSaveFunc(int Mm_id, int Fun_id, string FunType, string FunName, int Action)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Mm_id == 0)
                    throw new Exception("设备型号不能为空");
                if (string.IsNullOrEmpty(FunType) && Fun_id != 0)
                    throw new Exception("设备采集码不能为空");
                if (string.IsNullOrEmpty(FunName) && Fun_id != 0)
                    throw new Exception("请输入角色名称");

                int cc = bll.SetYdModelsOnSaveFunc(Mm_id, Fun_id, FunType, FunName, Action);

                rst.data = cc;

            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置角色信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除采集信息
        /// </summary>
        /// <param name="Fun_id">设备采集码ID</param>
        /// <returns></returns>
        public APIRst SetYdModelsOnDelFunc(int Fun_id)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Fun_id == 0)
                    throw new Exception("设备采集码ID不能为空");
                int cc = bll.SetYdModelsOnDelFunc(Fun_id);
                rst.data = cc;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取充值参数信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 采集-保存并发送指令
        /// </summary>
        /// <param name="Cmd"></param>
        /// <param name="Mm_id"></param>
        /// <param name="ModuleName">型号名称</param>
        /// <param name="ModuleType">型号</param>
        /// <param name="Disabled">是否弃用</param>
        /// <param name="ModulePwd">编程密码</param>
        /// <param name="ModuleUid">编程操作人</param>
        /// <param name="Level">密码级别</param>
        /// <param name="Spec">接线方式</param>
        /// <param name="Fty_prod">生产厂家</param>
        /// <param name="IsCharg">计费方式</param>
        /// <param name="Protocol">协议类型</param>
        /// <param name="Pmap"></param>
        /// <returns></returns>
        public APIRst SetYdModelsOnSave(int Cmd, int Mm_id, string ModuleName, string ModuleType, int Disabled, string ModulePwd, string ModuleUid, string Level, string Spec, string Fty_prod, int IsCharg, string Protocol, string Pmap)
        {
            APIRst rst = new APIRst();
            try
            {


                v0_moduleVModel mm = new v0_moduleVModel();
                mm.Mm_id = Mm_id;
                mm.ModuleName = ModuleName;
                mm.ModuleType = ModuleType;
                mm.Disabled = Disabled;
                mm.ModulePwd = ModulePwd == "undefined" || ModulePwd == "null" || ModulePwd == "{ModulePwd}" ? null : ModulePwd;
                mm.ModuleUid = ModuleUid == "undefined" || ModuleUid == "null" || ModuleUid == "{ModuleUid}" ? null : ModuleUid;
                mm.Level = Level == "undefined" || Level == "null" || Level == "{Level}" ? null : Level;
                mm.Spec = Spec == "undefined" || Spec == "null" || Spec == "{Spec}" ? null : Spec;
                mm.Fty_prod = Fty_prod == "undefined" || Fty_prod == "null" || Fty_prod == "{Fty_prod}" ? null : Fty_prod;
                mm.IsCharg = IsCharg;
                mm.Protocol = Protocol;

                if (string.IsNullOrEmpty(mm.ModuleType))
                    throw new Exception("型号不能为空");
                if (string.IsNullOrEmpty(mm.ModuleName))
                    throw new Exception("型号名称不能为空");

                int cmdNum = 0;
                DataTable dtSource = JsonHelper.ToDataTable(Pmap);
                bll.SetYdModelsOnSave(mm, dtSource);
                if (Cmd == 1)
                {
                    DataTable dtCmd = bll.GetYdModelsSendCmd(mm.Mm_id);
                    cmdNum = dtCmd.Rows.Count;
                    this.GetYdModelsSendCmd(dtCmd);
                }
                DataTable dtNew = bll.GetYdModelsOnList(mm.Mm_id);
                var res1 = from s1 in dtNew.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Mm_id = CommFunc.ConvertDBNullToInt32(s1["Mm_id"]),
                               ModuleType = CommFunc.ConvertDBNullToString(s1["ModuleType"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               Spec = CommFunc.ConvertDBNullToString(s1["Spec"]),
                               Fty_prod = CommFunc.ConvertDBNullToString(s1["Fty_prod"]),
                               IsCharg = CommFunc.ConvertDBNullToInt32(s1["IsCharg"]),
                               ModulePwd = CommFunc.ConvertDBNullToString(s1["ModulePwd"]),
                               ModuleUid = CommFunc.ConvertDBNullToString(s1["ModuleUid"]),
                               Level = CommFunc.ConvertDBNullToString(s1["Level"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                           };
                rst.data = res1.ToList();

            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("采集-保存并发送指令错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private void GetYdModelsSendCmd(DataTable dtSource)
        {
            foreach (DataRow dr in dtSource.Rows)
            {
                CommandVModel cmd = ModelHandler<CommandVModel>.FillModel(dr);
                ListenVModel vm = new ListenVModel();
                vm.cfun = ListenCFun.cmd.ToString();
                vm.content = JsonHelper.Serialize(cmd);
                CacheMgr.BeginSend(vm);
            }
        }

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="Mm_id">类型ID号</param>
        /// <returns></returns>
        public APIRst SetYdModelsOnDel(int Mm_id)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Mm_id == 0)
                    throw new Exception("设备采集码ID不能为空");
                int cc = bll.SetYdModelsOnDel(Mm_id);
                rst.data = cc == 0 ? "此类型可能已删除" : cc.ToString();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取充值参数信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

    }
}