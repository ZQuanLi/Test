using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.IFSMgr.Monitor
{
    public partial class MonitorDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public MonitorDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取采集点信息
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public DataTable GetTagInfo(string tags)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Ledger,a.Gw_id,a.GwAddr,a.Esp_id,a.EspAddr,a.EspIp,a.EspPort,a.ComPort,a.Baud,a.DataBit,a.Parity,a.StopBit,");
            strSql.Append("a.GwIp as Ip,a.EspPort as TcpPort,a.EspTimeout as TimeOut,a.HandledBY,a.`MeterType` as ModuleType, a.`MeterPwd` as ModulePwd,a.MeterUid as  ModuleUid,a.TransferType,");
            strSql.Append("a.Module_id,a.ModuleAddr,a.ModuleName,a.Co_id,b.Fun_id,b.FunName,b.FunType,b.Action,b.LpszDbVarName,");
            strSql.Append("c.CoFullName as StrucName,a.Protocol,v2.LastVal,v2.LastTime,s1.FrMd,a.MeterAddr,b.TagName,");
            strSql.Append("mp.DataValue,mp.Status,mp.Update_dt");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" inner join vp_funinfo as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and b.Disabled=0"); //and b.Action!=1
            strSql.Append(" inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" left join vp_coinfo as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" left join v2_info as v2 on a.Ledger=v2.Ledger and a.Co_id=v2.Co_id and a.Module_id=v2.Module_id and a.ModuleAddr=v2.ModuleAddr and b.Fun_id=v2.Fun_id");
            strSql.Append(" left join v1_map as mp on b.Ledger=mp.Ledger and b.Module_id=mp.Module_id and b.Fun_id=mp.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and FIND_IN_SET(b.LpszDbVarName,@Strlist) ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = Ledger , Strlist = tags });
        }

        /// <summary>
        /// 更新数据值
        /// </summary>
        /// <param name="module_id"></param>
        /// <param name="fun_id"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public int UpdateMapDataVal(int module_id, int fun_id, string dataValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into v1_map(Ledger,Module_id,Fun_id,TagName,DataValue,Status,Disabled,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,@Module_id,@Fun_id,'',@DataValue,0,0,@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE DataValue=@DataValue,Status=0,Update_by=@SysUid,Update_dt=now()");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = Ledger, Module_id = module_id, Fun_id = fun_id, DataValue = dataValue, SysUid = this.SysUid });
        }

        public DataTable GetYdPayListResult(string trade_no)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Ledger,Log_id,Co_id,Module_id,Create_by from v4_pay_log where Trade_no=@Trade_no and ifnull(Trade_no,'')<>'' and ErrCode<>1");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Trade_no  = trade_no} );
        }
    }
}
