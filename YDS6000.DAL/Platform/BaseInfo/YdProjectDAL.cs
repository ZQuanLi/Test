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
    partial class BaseInfoDAL
    {
        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetProList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Co_id,a.Number,a.CoName,a.Parent_id,a.Disabled,b.Area,a.CustAddr,a.CustName,a.Mobile,a.Email,a.Remark");
            strSql.Append(" from vp_coinfo as a left join v1_custinfobuild as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@ledger and a.Attrib =@Attrib");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = (int)CoAttrib.Project });
        }

        /// <summary>
        /// 设置项目信息
        /// </summary>
        /// <param name="pro"></param>
        /// <returns></returns>
        public int SetPro(ProVModel pro)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(*) from v1_cust where Ledger=@Ledger and CoName=@CoName and Co_id!=@Co_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, CoName = pro.ProName, Co_id = pro.Id });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("项目信息名称:" + pro.ProName + "已存在");
            if (pro.Id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Co_id)as Co_id from v1_cust where Ledger=@Ledger");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger });
                pro.Id = CommFunc.ConvertDBNullToInt32(obj) + 1;
            }
            object params01 = new
            {
                Ledger = this.Ledger,
                Co_id = pro.Id,
                CoNo = pro.Id,
                CoName = pro.ProName,
                Parent_id = 0,
                CustAddr = pro.ProAddr,
                CustName = pro.Person,
                Mobile = pro.TelNo,
                Remark = pro.Remark,
                Disabled = pro.Disabled,
                Attrib = CoAttrib.Project,
                Layer = 0,
                Area = pro.Area,
                SysUid = this.SysUid
            };
            ///////////////////////////////////////////////////
            strSql.Clear();
            strSql.Append("insert into v1_cust(Ledger,Co_id,CoNo,CoName,Disabled,Parent_id,Attrib,Layer,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,@Co_id,@CoNo,@CoName,@Disabled,@Parent_id,@Attrib,@Layer,@SysUid,now(),@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE CoNo=@CoNo,CoName=@CoName,Disabled=@Disabled,Update_by=@SysUid,Update_dt=now();");
            strSql.Append("insert into v1_custinfo(Ledger,Co_id,CustName,CustAddr,Mobile,Remark,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,@Co_id,@CustName,@CustAddr,@Mobile,@Remark,@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE CustAddr=@CustAddr,CustName=@CustName,Mobile=@Mobile,Remark=@Remark,Update_by=@SysUid,Update_dt=now();");
            strSql.Append("update v1_custinfo set StrucName=GetCoOnStrucName(Ledger,Co_id) where Ledger=@Ledger and FIND_IN_SET(Co_id,GetCoChildList(Ledger,@Co_id));"); // 更新全名
            strSql.Append("insert into v1_custinfobuild (Ledger,Co_id,Area,Update_by,Update_dt)values(@Ledger,@Co_id,@Area,@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE Area=@Area,Update_by=@SysUid,Update_dt=now();");
            int cnt = SQLHelper.Execute(strSql.ToString(), params01);
            return cnt;
        }

        /// <summary>
        /// 删除项目信息
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public int DelPro(int co_id)
        {
            object params01 = new { Ledger = this.Ledger, Co_id = co_id };
            StringBuilder strSql = new StringBuilder();
            //
            strSql.Clear();
            strSql.Append("select count(a.Co_id)as cnt");
            strSql.Append(" from v1_cust as a ");
            strSql.Append(" where a.Ledger=@Ledger and a.Parent_id=@Co_id");
            Object obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("请先删除项目下的子项");
            //
            strSql.Clear();
            strSql.Append("delete d,c,b,a from v1_cust as a left join v1_custinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v1_custinfobuild as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" left join v1_custinfobuild_fee as d on a.Ledger=d.Ledger and a.Co_id=d.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id;");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }
    }
}
