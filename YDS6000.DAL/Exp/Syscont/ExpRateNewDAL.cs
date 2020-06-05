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
    public partial class ExpRateNewDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public ExpRateNewDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取物业收费信息
        /// </summary>
        /// <param name="Descr">筛选条件：描述</param>
        /// <param name="rate_id">费率ID号</param>
        /// <returns></returns>
        public DataTable GetYdRateNewList(int Rate_id, string Descr)
        {
            if (string.IsNullOrEmpty(Descr) || Descr == "{Descr}" || Descr == "null")
                Descr = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select Rate_id,Descr,Rule,Unit,UnitBase");
            strSql.Append(" from v1_rate where Ledger=@Ledger and Descr like @Descr and Attrib=1");
            if (Rate_id != 0)
                strSql.Append(" and Rate_id=@Rate_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Rate_id = Rate_id, Descr = "%" + Descr + "%" });
        }

        /// <summary>
        /// 设置保存物业收费
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="Descr">描述</param>
        /// <param name="Rule">计算规则: 0=正常,1=时间范围,2=数量范围</param>
        /// <param name="Unit">单位: Area=平方米,Bank=户数</param>
        /// <param name="UnitBase">单位基数</param>
        /// <param name="Disabled">是否弃用:0=否,1=是</param>
        /// <returns></returns>
        public int SaveYdRateNew(v1_rateVModel rv)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = null;
            if (rv.Rate_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Rate_id) as id from v1_rate where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                rv.Rate_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["id"]) + 1 : 1;
                //rv.Rate_id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["id"]) + 1;
            }
            strSql.Clear();
            strSql.Append("insert into v1_rate(Ledger,Rate_id,Descr,Attrib,Rule,Unit,UnitBase,Disabled,Create_by,Create_dt,Update_by,Update_dt)values");
            strSql.Append("(@Ledger,@Rate_id,@Descr,@Attrib,@Rule,@Unit,@UnitBase,@Disabled,@SysUid,now(),@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE Descr=@Descr,Rule=@Rule,Unit=@Unit,UnitBase=@UnitBase,Disabled=@Disabled,Update_by=@SysUid,Update_dt=now()");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Rate_id = rv.Rate_id, Descr = rv.Descr, Attrib = rv.Attrib, Rule = rv.Rule, Unit = rv.Unit, UnitBase = rv.UnitBase, Disabled = rv.Disabled, SysUid = this.SysUid });
        }

        /// <summary>
        /// 删除费率信息
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <returns></returns>
        public int GetDelYdRateNew(int Rate_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("delete b,a from v1_rate as a left join v1_rate_cs as b on a.Ledger=b.Ledger and a.Rate_id=b.Rate_id where a.Ledger=@Ledger and a.Rate_id=@Rate_id;");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Rate_id = Rate_id });
        }

        /// <summary>
        /// 获取物业收费-单价详情
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <returns></returns>
        public DataTable GetYdRateNewCs(int Rate_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Rate_id,b.CsId,b.Price,b.PStart,b.PEnd from v1_rate as a inner join v1_rate_cs as b on a.Ledger=b.Ledger and a.Rate_id=b.Rate_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Rate_id=@Rate_id;");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Rate_id = Rate_id });
        }

        /// <summary>
        /// 设置物业收费-单价详情
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="CsId">单价ID号</param>
        /// <param name="Price">单价</param>
        /// <param name="pStart">开始区间</param>
        /// <param name="pEnd">结束区间</param>
        /// <returns></returns>
        public int SetSaveYdRateNewCs(int Rate_id, int CsId, decimal Price, string pStart, string pEnd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = null;
            if (CsId == 0)
            {
                strSql.Clear();
                strSql.Append("select max(CsId) as Id from v1_rate_cs where Ledger=@Ledger and Rate_id=@Rate_id");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Rate_id= Rate_id });
                CsId = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Id"]) + 1 : 1;
                //CsId = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Id"]) + 1;
            }
            strSql.Clear();
            strSql.Append("insert into v1_rate_cs(Ledger,Rate_id,CsId,Price,PStart,PEnd,Update_by,Update_dt)values(@Ledger,@Rate_id,@CsId,@Price,@PStart,@PEnd,@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE Price=@Price,PStart=@PStart,PEnd=@PEnd,Update_by=@SysUid,Update_dt=now()");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Rate_id = Rate_id, CsId = CsId, Price = Price, PStart = pStart, PEnd = pEnd, SysUid = this.SysUid });
        }

        /// <summary>
        /// 删除物业收费-单价详情
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="csId">单价ID号</param>
        /// <returns></returns>
        public int GetDelYdRateCs(int Rate_id, int csId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("delete from v1_rate_cs where Ledger=@Ledger and Rate_id=@Rate_id and CsId=@CsId");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Rate_id = Rate_id, CsId = csId });
        }


    }
}
