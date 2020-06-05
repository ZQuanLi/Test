using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Exp.Syscont
{
    public partial class ExpModelsDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public ExpModelsDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取设备型号
        /// </summary>
        /// <param name="Mm_id">类型ID号</param>
        /// <param name="ModuleType">筛选条件：设备类型</param>
        /// <returns></returns>
        public DataTable GetYdModelsOnList(int Mm_id, string ModuleType)
        {
            if (string.IsNullOrEmpty(ModuleType) || ModuleType == "{ModuleType}" || ModuleType == "null")
                ModuleType = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Mm_id,a.ModuleName,a.ModuleType,a.Disabled,a.ModulePwd,a.ModuleUid,a.Level,a.Spec,a.Fty_prod,a.IsCharg");
            strSql.Append(" from v0_module as a");
            strSql.Append(" where a.Ledger=@Ledger");
            if (Mm_id != 0)
                strSql.Append(" and a.Mm_id=@Mm_id");
            //if(!string.IsNullOrEmpty(ModuleType))
            strSql.Append(" and a.ModuleType like @ModuleType;");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = Mm_id, ModuleType = "%" + ModuleType + "%" });
        }

        /// <summary>
        /// 获取采集信息
        /// </summary>
        /// <param name="Mm_id">类型ID号</param>
        /// <returns></returns>
        public DataTable GetYdModelsOfEditOnList(int Mm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Fun_id,a.Mm_id,a.FunName,a.FunType,a.Action,a.DataValue,a.DataType,a.Scale,a.Disabled,a.OrdNo,");
            strSql.Append("a.Alias,a.OrdGrp,a.Unit,a.PointType,a.AlarmModel,a.SubTab,0 as nAct");
            strSql.Append(" from v0_fun as a ");
            strSql.Append(" where a.Ledger=@Ledger and a.Mm_id=@Mm_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = Mm_id });
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
        public int SetYdModelsOnSaveFunc(int Mm_id, int Fun_id, string FunType, string FunName, int Action)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = null;
            if (Fun_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Fun_id)as cnt from v0_fun where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                Fun_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1 : 1;
                //Fun_id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1;
            }
            strSql.Clear();
            strSql.Append("select count(*) from v0_fun where Ledger=@Ledger and Mm_id=@Mm_id and Fun_id!=@Fun_id and FunType=@FunType");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = Mm_id, Fun_id = Fun_id, FunType = FunType });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["count(*)"]) > 0)
                throw new Exception("采集码:" + FunType + "已存在");
            //
            strSql.Clear();
            strSql.Append("insert into v0_fun(");
            strSql.Append("Ledger,Fun_id,Mm_id,FunName,FunType,Action,DataValue,DataType,Scale,Disabled,OrdNo,OrdGrp,Unit,PointType,AlarmModel,SubTab)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Fun_id,@Mm_id,@FunName,@FunType,@Action,'',0,2,0,0,0,'',0,0,@FunType)");
            strSql.Append(" ON DUPLICATE KEY UPDATE FunName=@FunName,FunType=@FunType,Action=@Action,SubTab=@FunType;");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Fun_id = Fun_id, Mm_id = Mm_id, FunName = FunName, FunType = FunType, Action = Action });
        }

        /// <summary>
        /// 删除采集信息
        /// </summary>
        /// <param name="Fun_id">设备采集码ID</param>
        /// <returns></returns>
        public int SetYdModelsOnDelFunc(int Fun_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = null;
            strSql.Append("select count(*) from v1_map where Ledger=@Ledger and Fun_id=@Fun_id limit 1");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Fun_id = Fun_id });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["count(*)"]) > 0)
                throw new Exception("已经有数据在做映射");
            //
            strSql.Clear();
            strSql.Append("select count(*) from v2_info where Ledger=@Ledger and Fun_id=@Fun_id limit 1");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Fun_id = Fun_id });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["count(*)"]) > 0)
                throw new Exception("已经有数据在采集已存在");
            //
            strSql.Clear();
            strSql.Append("delete from v0_fun where Ledger=@Ledger and Fun_id=@Fun_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Fun_id = Fun_id });
        }


        public int SetYdModelsOnSave(v0_moduleVModel mm, DataTable v0_fun)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = null;
            strSql.Append("select count(*) from v0_module where Ledger=@Ledger and Mm_id!=@Mm_id and ModuleType=@ModuleType");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = mm.Mm_id, ModuleType = mm.ModuleType });
            int cnt = 0;
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["count(*)"]) > 0)
            {
                throw new Exception("已存在此型号:" + mm.ModuleType);
            }
            if (mm.Mm_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Mm_id) from v0_module where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                mm.Mm_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["max(Mm_id)"]) + 1 : 1;
                //mm.Mm_id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["max(Mm_id)"]) + 1;
                //
                strSql.Clear();
                strSql.Append("insert into v0_module(");
                strSql.Append("Ledger,Mm_id,ModuleName,ModuleType,Disabled,ModulePwd,ModuleUid,Level,Spec,Fty_prod,IsCharg,Protocol)");
                strSql.Append(" values (");
                strSql.Append("@Ledger,@Mm_id,@ModuleName,@ModuleType,@Disabled,@ModulePwd,@ModuleUid,@Level,@Spec,@Fty_prod,@IsCharg,@Protocol)");
                cnt = cnt + SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = mm.Mm_id, ModuleName = mm.ModuleName, ModuleType = mm.ModuleType, Disabled = mm.Disabled, ModulePwd = mm.ModulePwd, ModuleUid = mm.ModuleUid, Level = mm.Level, Spec = mm.Spec, Fty_prod = mm.Fty_prod, IsCharg = mm.IsCharg, Protocol = mm.Protocol });
            }
            else
            {
                strSql.Clear();
                strSql.Append("update v0_module set ");
                strSql.Append("ModuleName=@ModuleName,");
                strSql.Append("ModuleType=@ModuleType,");
                strSql.Append("Disabled=@Disabled,");
                strSql.Append("ModulePwd=@ModulePwd,");
                strSql.Append("ModuleUid=@ModuleUid,");
                strSql.Append("Level=@Level,");
                strSql.Append("Spec=@Spec,");
                strSql.Append("Fty_prod=@Fty_prod,");
                strSql.Append("IsCharg=@IsCharg,");
                strSql.Append("Protocol=@Protocol");
                strSql.Append(" where Ledger=@Ledger and Mm_id=@Mm_id;");
                cnt = cnt + SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = mm.Mm_id, ModuleName = mm.ModuleName, ModuleType = mm.ModuleType, Disabled = mm.Disabled, ModulePwd = mm.ModulePwd, ModuleUid = mm.ModuleUid, Level = mm.Level, Spec = mm.Spec, Fty_prod = mm.Fty_prod, IsCharg = mm.IsCharg, Protocol = mm.Protocol });
            }

            foreach (DataRow dr in v0_fun.Rows)
            {
                dr["Mm_id"] = mm.Mm_id;
                int action = CommFunc.ConvertDBNullToInt32(dr["Action"]);
                if (action != 1)
                {
                    dr["DataValue"] = "";
                }
                else
                {
                    if (CommFunc.ConvertDBNullToString(dr["FunType"]).Equals(V0Fun.IsRelay.ToString()))
                    {
                        if (CommFunc.ConvertDBNullToInt32(dr["DataValue"]) >= 2)
                            dr["DataValue"] = "129";
                        else
                            dr["DataValue"] = CommFunc.ConvertDBNullToInt32(dr["DataValue"]);
                    }
                }
                cnt = cnt + this.YdModelsOnSaveInFun(dr);
            }
            return cnt;
        }

        private int YdModelsOnSaveInFun(DataRow dr)
        {

            int Fun_id = CommFunc.ConvertDBNullToInt32(dr["Fun_id"]);
            int Mm_id = CommFunc.ConvertDBNullToInt32(dr["Mm_id"]);
            string FunName = CommFunc.ConvertDBNullToString(dr["FunName"]);
            string FunType = CommFunc.ConvertDBNullToString(dr["FunType"]);
            int Action = CommFunc.ConvertDBNullToInt32(dr["Action"]);
            string DataValue = CommFunc.ConvertDBNullToString(dr["DataValue"]);
            int DataType = CommFunc.ConvertDBNullToInt32(dr["DataType"]);
            int Scale = CommFunc.ConvertDBNullToInt32(dr["Scale"]);
            int Disabled = CommFunc.ConvertDBNullToInt32(dr["Disabled"]);
            int OrdNo = CommFunc.ConvertDBNullToInt32(dr["OrdNo"]);
            int OrdGrp = CommFunc.ConvertDBNullToInt32(dr["OrdGrp"]);
            string Unit = CommFunc.ConvertDBNullToString(dr["Unit"]);
            int PointType = CommFunc.ConvertDBNullToInt32(dr["PointType"]);
            int AlarmModel = CommFunc.ConvertDBNullToInt32(dr["AlarmModel"]);
            string SubTab = CommFunc.ConvertDBNullToString(dr["SubTab"]);

            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(*) from v0_fun where Ledger=@Ledger and Fun_id!=@Fun_id and Mm_id=@Mm_id and FunType=@FunType");
            DataTable obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Fun_id = Fun_id, Mm_id = Mm_id, FunType = FunType });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["count(*)"]) > 0)
            {
                throw new Exception("已存在此型号功能码:" + CommFunc.ConvertDBNullToString(dr["FunType"]));
            }
            //
            if (CommFunc.ConvertDBNullToInt32(dr["Fun_id"]) == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Fun_id)as cnt from v0_fun where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                int fun_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1 : 1;
                //int fun_id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) + 1;
                dr["Fun_id"] = fun_id;
                Fun_id = fun_id;
            }
            strSql.Clear();
            strSql.Append("insert into v0_fun(");
            strSql.Append("Ledger,Fun_id,Mm_id,FunName,FunType,Action,DataValue,DataType,Scale,Disabled,OrdNo,OrdGrp,Unit,PointType,AlarmModel,SubTab)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Fun_id,@Mm_id,@FunName,@FunType,@Action,@DataValue,@DataType,@Scale,@Disabled,@OrdNo,@OrdGrp,@Unit,@PointType,@AlarmModel,@SubTab)");
            strSql.Append(" ON DUPLICATE KEY UPDATE FunName=@FunName,DataValue=@DataValue,DataType=@DataType,Scale=@Scale,Disabled=@Disabled,OrdNo=@OrdNo,OrdGrp=@OrdGrp,Unit=@Unit,PointType=@PointType,AlarmModel=@AlarmModel");
            //strSql.Clear();
            //strSql.Append("insert into v0_fun(");
            //strSql.Append("Ledger,Fun_id,Mm_id,FunName,FunType,Action,DataValue,DataType,Scale,Disabled,OrdNo)");
            //strSql.Append(" values (");
            //strSql.Append("@Ledger,@Fun_id,@Mm_id,@FunName,@FunType,@Action,@DataValue,@DataType,@Scale,@Disabled,@OrdNo)");
            //strSql.Append(" ON DUPLICATE KEY UPDATE FunName=@FunName,DataValue=@DataValue,DataType=@DataType,Scale=@Scale,Disabled=@Disabled,OrdNo=@OrdNo");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Fun_id = Fun_id, Mm_id = Mm_id, FunName = FunName, FunType = FunType, Action = Action, DataValue = DataValue, DataType = DataType, Scale = Scale, Disabled = Disabled, OrdNo = OrdNo, OrdGrp = OrdGrp, Unit = Unit, PointType = PointType, AlarmModel = AlarmModel, SubTab = SubTab });
        }

        /// <summary>
        /// 获取发送命令信息
        /// </summary>
        /// <param name="Mm_id"></param>
        /// <returns></returns>
        public DataTable GetYdModelsSendCmd(int Mm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into v2_command(");
            strSql.Append("CDate,Ledger,Co_id,Module_id,ModuleAddr,Fun_id,FunType,DataValue,Create_by,Create_dt,ErrCode)");
            strSql.Append("select now(),a.Ledger,a.Co_id,a.Module_id,a.ModuleAddr,a.Fun_id,a.FunType,a.DataValue,@SysUid,now(),0");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" left join v1_map as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and a.Fun_id=b.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Mm_id=@Mm_id and a.Action=1;");
            strSql.Append(" update v1_map as a , vp_mdinfo as b  set a.`Status`=0,a.Update_by=@SysUid,a.Update_dt=NOW() where a.Ledger=b.Ledger and a.Module_id=b.Module_id and a.Ledger=@Ledger and b.Mm_id=@Mm_id;");
            //
            strSql.Append("select a.* ");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" left join v1_map as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and a.Fun_id=b.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Mm_id=@Mm_id and a.Action=1;");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = Mm_id, SysUid = this.SysUid });
        }

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="Mm_id">类型ID号</param>
        /// <returns></returns>
        public int SetYdModelsOnDel(int Mm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = null;
            strSql.Append("select count(*) from v1_gateway_esp_meter where Ledger=@Ledger and Mm_id=@Mm_id");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = Mm_id });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["count(*)"]) > 0)
                throw new Exception("此设备型号已经在使用，不能删除");
            strSql.Clear();
            strSql.Append("delete from v0_fun where Ledger=@Ledger and Mm_id=@Mm_id;");
            strSql.Append("delete from v0_module where Ledger=@Ledger and Mm_id=@Mm_id;");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = Mm_id });
        }


    }
}
