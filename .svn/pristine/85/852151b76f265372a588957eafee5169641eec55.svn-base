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
        /// 获取建筑信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetBuildList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Co_id,a.Number,a.CoName,a.Parent_id,a.Disabled,b.Area,a.CustAddr,a.CustName,a.Mobile,a.Email,a.Remark");
            strSql.Append(" from vp_coinfo as a left join v1_custinfobuild as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@ledger and a.Attrib =@Attrib");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = (int)CoAttrib.Build });
        }

        /// <summary>
        /// 设置建筑信息
        /// </summary>
        /// <param name="build"></param>
        /// <returns></returns>
        public int SetBuild(BuildVModel build)
        {
            StringBuilder strSql = new StringBuilder();
            int layer = 0;
            strSql.Clear();
            strSql.Append("select count(*) from v1_cust where Ledger=@Ledger and CoName=@CoName and Co_id!=@Co_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, CoName = build.BuildName, Co_id = build.Id });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("名称:" + build.BuildName + "已存在");

            if (build.Parent_id != 0)
            {
                strSql.Clear();
                strSql.Append("select Layer from v1_cust where Ledger=@Ledger and Co_id=@Co_id");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = build.Parent_id });
                layer = CommFunc.ConvertDBNullToInt32(obj) + 1;
            }
            if (build.Id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Co_id)as Co_id from v1_cust where Ledger=@Ledger");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger });
                build.Id = CommFunc.ConvertDBNullToInt32(obj) + 1;
            }
            object params01 = new
            {
                Ledger = this.Ledger,
                Co_id = build.Id,
                CoNo = build.Id,
                CoName = build.BuildName,
                Parent_id = build.Parent_id,
                CustAddr = build.BuildAddr,
                Mobile = build.TelNo,
                Email = build.Email,
                Remark = build.Remark,
                Disabled = 0,
                Attrib = CoAttrib.Build,
                Layer = layer,
                SysUid = this.SysUid
            };
            ///////////////////////////////////////////////////
            strSql.Clear();
            strSql.Append("insert into v1_cust(Ledger,Co_id,CoNo,CoName,Disabled,Parent_id,Attrib,Layer,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,@Co_id,@CoNo,@CoName,@Disabled,@Parent_id,@Attrib,@Layer,@SysUid,now(),@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE CoNo=@CoNo,CoName=@CoName,Disabled=@Disabled,Update_by=@SysUid,Update_dt=now();");
            strSql.Append("insert into v1_custinfo(Ledger,Co_id,CustAddr,Mobile,Email,Remark,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,@Co_id,@CustAddr,@Mobile,@Email,@Remark,@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE CustAddr=@CustAddr,Mobile=@Mobile,Email=@Email,Remark=@Remark,Update_by=@SysUid,Update_dt=now();");
            strSql.Append("update v1_custinfo set StrucName=GetCoOnStrucName(Ledger,Co_id) where Ledger=@Ledger and FIND_IN_SET(Co_id,GetCoChildList(Ledger,@Co_id))"); // 更新全名
            int cnt = SQLHelper.Execute(strSql.ToString(), params01);
            return cnt;
        }

        /// <summary>
        /// 删除建筑信息
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public int DelBuild(int co_id)
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
                throw new Exception("请先删除建筑下的子项");
            strSql.Clear();
            strSql.Append("select count(*)as cnt from vp_mdinfo where Ledger=@Ledger and Co_id=@Co_id");
            obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("请先删除建筑下的设备");
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
