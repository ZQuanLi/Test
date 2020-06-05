using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YDS6000.Models;
using DBUtility;

namespace YDS6000.DAL.CmdService
{
    public class ProcessDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public ProcessDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取设备测试ID号
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="funType"></param>
        /// <returns></returns>
        public int GetYdToFun_id(int module_id, V0Fun funType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select b.Fun_id");
            strSql.Append(" from vp_mdinfo as a inner join v0_fun as b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and b.FunType=@FunType");
            object id = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, FunType = funType.ToString() });
            return CommFunc.ConvertDBNullToInt32(id);
        }

        /// <summary>
        /// 下发命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public long YdToGwOfAddCmd(int module_id, int fun_id, string funType, string dataValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into v2_command(");
            strSql.Append("CDate,Ledger,Co_id,Module_id,ModuleAddr,Fun_id,FunType,DataValue,Create_by,Create_dt,ErrCode)");
            strSql.Append("select now(),a.Ledger,a.Co_id,a.Module_id,a.ModuleAddr,@Fun_id,@FunType,@DataValue,@SysUid,now(),0");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id;");
            strSql.Append("select last_insert_id();");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, Fun_id = fun_id, FunType = funType, DataValue = dataValue, SysUid = this.SysUid });
            return CommFunc.ConvertDBNullToLong(obj);
        }

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        public CommandVModel GetYdToGwOfSendCmd(long log_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from vp_cmdinfo where Log_id=@Log_id");
            DataTable dt = SQLHelper.Query(strSql.ToString(), new { Log_id = log_id} );
            CommandVModel cmd = null;
            if (dt.Rows.Count > 0)
                cmd = ModelHandler<CommandVModel>.FillModel(dt.Rows[0]);
            return cmd;
        }
    }
}
