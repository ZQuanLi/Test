using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Exp.Alarm
{
    partial class AlarmDAL
    {
        /// <summary>
        /// 获取告警类型
        /// </summary>
        /// <returns></returns>
        public int GetYdAlarmOfChrgType()
        {
            object params01 = new {
                Ledger=this.Ledger
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ChrgType from syscont where Ledger=@Ledger");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            int chrgType = CommFunc.ConvertDBNullToInt32(obj);
            return (chrgType <= 1 ? 0 : 1);
        }

        /// <summary>
        /// 获取告警值
        /// </summary>
        /// <returns></returns>
        public decimal GetYdAlarmOfVal()
        {
            object params01 = new {
                Ledger=this.Ledger
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CfValue from sys_config where Ledger=@Ledger and CfKey='AlarmVal1'");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            return CommFunc.ConvertDBNullToDecimal(obj);
        }

        public DataTable GetYdAlarmOfValList(string CoStrcName, string CoName,decimal AlarmVal)
        {
            string AreaPowerStr = "";
            bool IsCheckAreaPower = WHoleDAL.GetAreaPower(this.Ledger, this.SysUid, out AreaPowerStr);

            object params01 = new {
                Ledger=this.Ledger,
                AreaPowerStr=AreaPowerStr,
                CoStrcName="%"+CoStrcName+"%",
                CoName="%"+CoName+"%",
                AlarmVal=AlarmVal,
                MdItems= WHoleDAL.MdItems,
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Co_id,a.PayVal,a.UseVal,a.ShareVal,a.DebtTime,s1.ChrgType,m1.Module_id,m1.ModuleAddr,m1.ModuleName,b.CoStrcName,b.CoName,");
            strSql.Append("case when s1.ChrgType<=1 then a.SyVal else ifnull(c.RdVal,0)*m1.Multiply end as SyVal");
            strSql.Append(" from vp_cobill as a inner join syscont as s1 on a.Ledger=s1.Ledger");
            strSql.Append(" inner  join vp_coinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" inner join v0_fun as vf on a.Ledger = vf.Ledger and a.Fun_id = vf.Fun_id and vf.FunType='E'");
            strSql.Append(" left join vp_mdinfo as m1 on a.Ledger=m1.Ledger and a.Co_id=m1.Co_id and vf.Mm_id=m1.Mm_id and m1.Module_id=case when s1.ChrgType<=1 then -9991 else m1.Module_id END");
            strSql.Append(" left join vp_v2info as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id and a.Fun_id=c.Fun_id and m1.Module_id=c.Module_id and m1.ModuleAddr=c.ModuleAddr and m1.Mm_id=c.Mm_id");
            strSql.Append(" left join v1_gateway_esp_module_info as d on a.Ledger=d.Ledger and m1.Module_id=d.Module_id");
            strSql.Append(" where a.Ledger=@Ledger");
            if (IsCheckAreaPower == true)
                strSql.Append(" and FIND_IN_SET(a.Co_id,@AreaPowerStr)");
            strSql.Append(" and ifnull(b.CoStrcName,'') like @CoStrcName");
            strSql.Append(" and ifnull(b.CoName,'') like @CoName");
            //strSql.Append(" and case when s1.ChrgType<=1 then a.SyVal else ifnull(c.RdVal,0) end<= @AlarmVal");
            strSql.Append(" and case when s1.ChrgType<=1 then a.SyVal else ifnull(c.RdVal,0)*m1.Multiply end <= case when s1.ChrgType<=1 or ifnull(d.AlarmVal,0)=0 then @AlarmVal else d.AlarmVal end");
            strSql.Append(" and m1.ModuleAddr=case when s1.Zb=0 then m1.MeterAddr else m1.ModuleAddr end");
            if (!string.IsNullOrEmpty(WHoleDAL.MdItems))
            {
                strSql.Append(" and exists(select * from vp_mdinfo as md where md.Ledger=a.Ledger and md.Co_id=a.Co_id and FIND_IN_SET(m1.IsDefine,@MdItems))");
            }
            return SQLHelper.Query(strSql.ToString(), params01);
        }

        /// <summary>
        /// 获取首页欠费户数
        /// </summary>
        /// <returns></returns>
        public int GetHomeUserAlarm()
        {
            object params01 = new
            {
                Ledger = this.Ledger
            };
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(b.Crm_id) as Number from vp_v2info as a left join v3_userinfo as b on a.Ledger = b.Ledger and a.Co_id = b.Co_id where a.Ledger = @Ledger and a.RdVal < 0  and FunType='E' ");
            DataTable obj = SQLHelper.Query(strSql.ToString(), params01);
            int chrgType = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Number"]);
            return chrgType;
        }
    }
}
