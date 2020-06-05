using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Platform.BaseInfo
{
    public partial class BaseInfoDAL
    {
        /// <summary>
        /// 获取设备型号列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetModuleTypeList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Mm_id,a.ModuleName,a.ModuleType,a.Disabled,a.Number,a.IsDefine");
            strSql.Append(" from v0_module as a");
            strSql.Append(" where a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        public int SetModuleType(ModuleTypeVModel mmType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) from v0_module where Ledger=@Ledger and Mm_id!=@Mm_id and ModuleType=@ModuleType");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = mmType.ModuleTypeId, ModuleType = mmType.ModuleName });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("已存在此型号:" + mmType.ModuleName);

            if (mmType.ModuleTypeId == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Mm_id)as cnt from v0_module where Ledger=@Ledger");
                mmType.ModuleTypeId = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger })) + 1;
            }
            object params01 = new { Ledger = this.Ledger, Mm_id = mmType.ModuleTypeId, ModuleName = mmType.ModuleName, ModuleType = mmType.ModuleName, Disabled = mmType.Disabled, Spec = "", Fty_prod = "", IsDefine = mmType.IsDefine };
            strSql.Clear();
            strSql.Append("insert into v0_module(");
            strSql.Append("Ledger,Mm_id,ModuleName,ModuleType,Disabled,ModulePwd,ModuleUid,Level,Spec,Fty_prod,IsCharg,Protocol,IsDefine)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Mm_id,@ModuleName,@ModuleType,@Disabled,'','','',@Spec,@Fty_prod,0,'',@IsDefine)");
            strSql.Append("ON DUPLICATE KEY UPDATE ModuleName=@ModuleName,ModuleType=@ModuleType,Disabled=@Disabled,Spec=@Spec,Fty_prod=@Fty_prod,IsDefine=@IsDefine;");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }

        /// <summary>
        /// 删除类型设备
        /// </summary>
        /// <param name="mm_id">类型ID号</param>
        /// <returns></returns>
        public int DelModuleType(int mm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(a.Ledger)as cnt from v0_fun as a inner join v2_info as b on a.Ledger=b.Ledger and a.Fun_id=b.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Mm_id=@Mm_id limit 1");
            Object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = mm_id });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("此设备型号已经有采集不能删除");

            strSql.Clear();
            strSql.Append("delete b,a");
            strSql.Append(" from v0_module as a left join v0_fun as b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Mm_id=@Mm_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = mm_id });
        }

        /// <summary>
        /// 获取设备型号采集点数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetModuleFunList(int mm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Mm_id,b.Fun_id,b.FunName,b.FunType,b.Action");
            strSql.Append(" from v0_module as a inner join v0_fun as b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Mm_id=@Mm_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = mm_id });
        }

        /// <summary>
        /// 设置采集点信息
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        public int SetModuleFun(ModuleFunVModel fun)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(*) from v0_fun where Ledger=@Ledger and Fun_id!=@Fun_id and FunType=@FunType");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Fun_id = fun.Fun_id, FunType = fun.FunType });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("已存在此采集点型号:" + fun.FunType);
            if (fun.ModuleTypeId == 0)
                throw new Exception("已存在此采集类型ID号为空");
            if (fun.Fun_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Fun_id)as cnt from v0_fun where Ledger=@Ledger");
                fun.Fun_id = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger })) + 1;
            }
            //
            strSql.Clear();
            strSql.Append("select count(a.Ledger)as cnt from v0_fun as a inner join v2_info as b on a.Ledger=b.Ledger and a.Fun_id=b.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Mm_id=@Mm_id and a.Fun_id=@Fun_id limit 1");
            obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = fun.ModuleTypeId, Fun_id = fun.Fun_id });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("此采集点型号:" + fun.FunType + "已经有采集数据不能修改");
            //
            object params01 = new { Ledger = this.Ledger, Mm_id = fun.ModuleTypeId, Fun_id = fun.Fun_id, FunType = fun.FunType, FunName = fun.FunName, Disabled = 0 };
            strSql.Clear();
            strSql.Append("insert into v0_fun(Ledger,Fun_id,Mm_id,FunName,FunType,Action,Scale,Disabled,SubTab)");
            strSql.Append(" values (@Ledger, @Fun_id, @Mm_id, @FunName, @FunType, 0, 2, 0, @FunType)");
            strSql.Append("ON DUPLICATE KEY UPDATE FunType=@FunType,FunName=@FunName,SubTab=@FunType;");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }

        /// <summary>
        /// 删除采集点信息
        /// </summary>
        /// <param name="fun_id"></param>
        /// <returns></returns>
        public int DelModuleFun(int fun_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(a.Ledger)as cnt from v0_fun as a inner join v2_info as b on a.Ledger=b.Ledger and a.Fun_id=b.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Fun_id=@Fun_id  limit 1");
            Object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Fun_id = fun_id });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("此采集点型号已经有采集数据不能删除");
            strSql.Clear();
            strSql.Append("delete from v0_fun where Ledger=@Ledger and Fun_id=@Fun_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Fun_id = fun_id });
        }
    }
}
