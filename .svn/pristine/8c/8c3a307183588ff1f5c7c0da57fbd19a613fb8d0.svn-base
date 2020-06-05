using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using YDS6000.Models;
using DBUtility;

namespace YDS6000.DAL.Exp
{
    public class YdToGwDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        public YdToGwDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }
        
        public string GetYdToFun_idIOService(int module_id, string funType,out string dataValue)
        {
            dataValue = "";
            object params01 = new
            {
                Ledger = this.Ledger,
                Module_id = module_id,
                FunType = funType.ToString(),
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select b.Fun_id,c.TagName,case when ifnull(c.DataValue,'')='' then b.DataValue else c.DataValue end as DataValue");
            strSql.Append(" from vp_mdinfo as a inner join v0_fun as b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id");
            strSql.Append(" inner join v1_map as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id and b.Fun_id=c.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and b.FunType=@FunType and b.Disabled=0 and c.Disabled=0");
            DataTable dtSource = SQLHelper.Query(strSql.ToString(), params01);
            string tagName = "";
            foreach (DataRow dr in dtSource.Rows)
            {
                tagName = CommFunc.ConvertDBNullToString(dr["TagName"]);
                dataValue = CommFunc.ConvertDBNullToString(dr["DataValue"]);
            }
            return tagName;
        }

        public int GetYdToFun_id(int module_id, V0Fun funType)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Module_id = module_id,
                FunType = funType.ToString(),
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select b.Fun_id");
            strSql.Append(" from vp_mdinfo as a inner join v0_fun as b on a.Ledger=b.Ledger and a.Mm_id=b.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and b.FunType=@FunType");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            return CommFunc.ConvertDBNullToInt32(obj);
        }
        public DataTable GetYdToFun_id(int module_id, int fun_id)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Module_id = module_id,
                Fun_id = fun_id,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.Fun_id,a.FunType,b.TagName,b.DataValue");
            strSql.Append(" from vp_funinfo as a inner join v1_map as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and a.Fun_id=b.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and a.Fun_id=@Fun_id");
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        /// <summary>
        /// 下发命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public long YdToGwOfAddCmd(int module_id, int fun_id, string funType, string dataValue,int isRn)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Module_id = module_id,
                Fun_id = fun_id,
                FunType = funType,
                DataValue = dataValue,
                IsRn = isRn,
                SysUid = this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into v2_command(");
            strSql.Append("CDate,Ledger,Co_id,Module_id,ModuleAddr,Fun_id,FunType,DataValue,Create_by,Create_dt,ErrCode,IsRn)");
            strSql.Append("select now(),a.Ledger,a.Co_id,a.Module_id,a.ModuleAddr,@Fun_id,@FunType,@DataValue,@SysUid,now(),0,@IsRn");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id;");
            strSql.Append("select last_insert_id();");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            return CommFunc.ConvertDBNullToLong(obj);
        }

        /// <summary>
        /// 下发命令
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="funType"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public long YdToGwOfAddCmd(int module_id, int fun_id, string dataValue)
        {
            object params01 = new
            {
                Ledger = this.Ledger,
                Module_id = module_id,
                Fun_id = fun_id,
                DataValue = dataValue,
                SysUid = this.SysUid,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into v2_command(");
            strSql.Append("CDate,Ledger,Co_id,Module_id,ModuleAddr,Fun_id,FunType,DataValue,Create_by,Create_dt,ErrCode)");
            strSql.Append("select now(),a.Ledger,a.Co_id,a.Module_id,a.ModuleAddr,@Fun_id,a.FunType,@DataValue,@SysUid,now(),0");
            strSql.Append(" from vp_funinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Module_id=@Module_id and a.Fun_id=@Fun_id;");
            strSql.Append("select last_insert_id();");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            return CommFunc.ConvertDBNullToLong(obj);
        }

        /// <summary>
        /// 获取命令
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        public CommandVModel GetYdToGwOfSendCmd(long log_id)
        {
            object params01 = new
            {
                Log_id = log_id,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from vp_cmdinfo where Log_id=@Log_id");
            DataTable dt = SQLHelper.Query(strSql.ToString(), params01);
            CommandVModel cmd = null;
            if (dt.Rows.Count > 0)
                cmd = ModelHandler<CommandVModel>.FillModel(dt.Rows[0]);
            return cmd;
        }
        /// <summary>
        /// 获取远程命令
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        public v2_commandVModel GetYdToGwOfCmd(long log_id)
        {
            object params01 = new 
            {
               Log_id= log_id,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Log_id,CDate,Ledger,Co_id,Module_id,ModuleAddr,Fun_id,FunType,DataValue,Create_by,Create_dt,ErrCode");
            strSql.Append(" from v2_command where Log_id=@Log_id");
            DataTable dt = SQLHelper.Query(strSql.ToString(), params01);
            v2_commandVModel cmd = null;
            if (dt.Rows.Count > 0)
                cmd = ModelHandler<v2_commandVModel>.FillModel(dt.Rows[0]);
            return cmd;
        }

        /// <summary>
        /// 更新远程命令
        /// </summary>
        /// <param name="log_id"></param>
        /// <returns></returns>
        public int UpErrYdToGwOfCmd(long log_id,string ErrTxt)
        {
            object params01 = new
            {
                Log_id = log_id,
                ErrTxt = ErrTxt,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v2_command set ErrCode=-1,ErrTxt=@ErrTxt,EndTime=now() where Log_id=@Log_id");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }
    }
}
