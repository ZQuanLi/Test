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
    public partial class ExpYdCustomerDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public ExpYdCustomerDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        private static string EncryptPwd = "DiLidili";

        public DataTable GetCustomerList(int Crmid, string CrmName = "", string CrmNo = "", string RoomName = "")
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            //strSql.Append("select a.Ledger,a.Crm_id,a.CrmNo,a.CrmName,a.MPhone,a.Email,a.IsAdmin,a.Phone,a.Remark,GetDECODE(a.Passwd,@EncryptPwd)as Passwd,b.Co_id,b.IsHold,c.CoFullName,a.Contract,m.ModuleName");
            //strSql.Append(" from v3_user a left join v3_userinfo b on a.Ledger=b.Ledger and a.Crm_id=b.Crm_id");
            //strSql.Append(" left join vp_coinfo c on c.Ledger=b.Ledger and c.Co_id=b.Co_id");
            //strSql.Append(" left join vp_mdinfo m on m.Ledger=c.Ledger and m.Co_id=c.Co_id");
            strSql.Append("select a.Ledger,a.Uid as Crm_id,a.UserNo as CrmNo,a.UName as CrmName,a.TelNo as MPhone,a.Email,");
            strSql.Append("b.IsAdmin,a.Phone,a.Remark,GetDECODE(a.UPasswd,@EncryptPwd)as Passwd,b.Co_id,b.IsAdmin as IsHold,c.CoFullName,a.Contract,m.ModuleName");
            strSql.Append(" from vp_userinfo a left join v3_user b on a.Ledger=b.Ledger and a.Uid=b.Crm_id"); //
            strSql.Append(" left join vp_coinfo c on c.Ledger=b.Ledger and c.Co_id=b.Co_id");
            strSql.Append(" left join vp_mdinfo m on m.Ledger=c.Ledger and m.Co_id=c.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.UserType=100");
            if (Crmid > 0)
                strSql.Append(" and a.Uid = @Crm_id");
            if (!string.IsNullOrEmpty(CrmName) && CrmName != "null" && CrmName != "{CName}")
                strSql.Append(" and a.UName like @CrmName");
            if (!string.IsNullOrEmpty(CrmNo) && CrmNo != "null" && CrmNo != "{CNum}")
                strSql.Append(" and a.UserNo like @CrmNo");
            if (!string.IsNullOrEmpty(RoomName) && RoomName != "null" && RoomName != "{RoomName}")
                strSql.Append(" and ifnull(c.CoFullName,'') like @CoFullName");
            strSql.Append(" group by a.Ledger,a.Uid");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = Crmid, CrmName = "%" + CrmName + "%", CrmNo = "%" + CrmNo + "%", CoFullName = "%" + RoomName + "%", EncryptPwd = EncryptPwd });
        }

        /// <summary>
        /// 获取首页用户数（月数and总数）
        /// </summary>
        /// <returns></returns>
        public DataTable GetHomeCustomer(int or)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Clear();
            //strSql.Append("select a.Ledger,a.Crm_id,a.CrmNo,a.CrmName,a.MPhone,a.Email,a.IsAdmin,a.Phone,a.Remark,GetDECODE(a.Passwd,@EncryptPwd) as Passwd, b.Co_id,b.IsHold,c.CoFullName,a.Contract,m.ModuleName,a.Create_dt,a.Update_dt ");
            //strSql.Append(" from v3_user a left join v3_userinfo b on a.Ledger=b.Ledger and a.Crm_id=b.Crm_id ");
            //strSql.Append(" left join vp_coinfo c on c.Ledger=b.Ledger and c.Co_id=b.Co_id ");
            //strSql.Append(" left join vp_mdinfo m on m.Ledger=c.Ledger and m.Co_id=c.Co_id ");
            strSql.Clear();
            strSql.Append("select a.Ledger,a.Uid as Crm_id,a.UserNo as CrmNo,a.UName as CrmName,a.TelNo as MPhone,a.Email,b.IsAdmin,a.Phone,a.Remark,");
            strSql.Append("GetDECODE(a.UPasswd,@EncryptPwd) as Passwd, b.Co_id,b.IsAdmin as IsHold,c.CoFullName,'' as Contract,m.ModuleName,a.Create_dt,a.Update_dt");
            strSql.Append(" from vp_userinfo a left join v3_user b on a.Ledger=b.Ledger and a.Uid=b.Uid ");
            strSql.Append(" left join vp_coinfo c on c.Ledger=b.Ledger and c.Co_id=b.Co_id ");
            strSql.Append(" left join vp_mdinfo m on m.Ledger=c.Ledger and m.Co_id=c.Co_id ");
            strSql.Append(" where a.Ledger=@Ledger and a.UserType=100");
            var StartTime = "";
            var EndTime = "";
            if (or == 1)
            {
                DateTime dt = DateTime.Now; //当前时间
                StartTime = dt.AddDays(1 - dt.Day).ToString("yyyy-MM-dd"); //本月月初
                EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                strSql.Append(" AND a.Create_dt >= @Start AND a.Create_dt <= @End ");
            }
            strSql.Append(" group by a.Ledger,a.Uid");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Start = StartTime, End = EndTime, EncryptPwd = EncryptPwd });
        }

        /// <summary>
        /// 判断学号是否存在
        /// </summary>
        /// <param name="crmNo"></param>
        /// <returns></returns>
        public bool IsExistSameCrmNo(string crmNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(1) as Id from vp_userinfo where Ledger=@Ledger and UserNo=@CrmNo");
            DataTable obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, CrmNo = crmNo });
            int Id = 0;
            if (obj.Rows.Count > 0)
                Id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Id"].ToString());
            return Id > 0 ? true : false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Crm_id">主键ID号</param>
        /// <returns></returns>
        public int DelCustomer(int Crm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            //1.删除v3_user v3_userinfo
            strSql.Append("delete t4,t3,t2,t1 from sys_user as t1 left join sys_userinfo as t2 on t1.Ledger=t2.Ledger and t1.Uid=t2.Uid");
            strSql.Append(" left join v3_user as t3 on t1.Ledger=t3.Ledger and t1.Uid=t3.Uid");
            strSql.Append(" left join v3_openid as t4 on t1.Ledger=t4.Ledger and t1.Uid=t4.Uid");
            strSql.Append(" where t1.Ledger=@Ledger and t1.Uid=@Crm_id;");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = Crm_id });
        }

        /// <summary>
        /// 编辑顾客信息
        /// </summary>
        /// <param name="ui_Customer"></param>
        public int EditCustomer(v3_userVModel ui_Customer)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable obj = new DataTable();
            strSql.Clear();
            strSql.Append("select count(*) as cnt from vp_userinfo where Ledger=@Ledger and Uid!=@Crm_id and UserNo=@CrmNo");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = ui_Customer.Crm_id, CrmNo = ui_Customer.CrmNo });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) > 0)
                throw new Exception("证件号有重复");
            ui_Customer.Passwd = "123456";
            if (ui_Customer.Crm_id == 0)
            {               
                obj = null;
                strSql.Clear();
                strSql.Append("select max(Uid) as id from sys_user where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                ui_Customer.Crm_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["id"]) + 1 : 1;
            }
            strSql.Clear();
            strSql.Append("insert into sys_user(Ledger,Uid,USign,UPasswd,UName,Role_id,Disabled,UserType,Create_by,Create_dt,Update_by,Update_dt)values");
            strSql.Append("(@Ledger,@Crm_id,@MPhone,GetENCODE(@Passwd,@EncryptPwd),@CrmName,0,0,100,@SysUid,now(),@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE ");
            strSql.Append("UName=@CrmName,");
            strSql.Append("Update_by=@SysUid,");
            strSql.Append("Update_dt=now();");
            strSql.Append("insert into sys_userinfo(Ledger,Uid,UserNo,Sex,TelNo,Phone,Email,Status,Period,Create_by,Create_dt,Update_by,Update_dt,Remark,Contract)values");
            strSql.Append("(@Ledger,@Crm_id,@CrmNo,0,@MPhone,@Phone,@Email,0,'',@SysUid,now(),@SysUid,now(),@Remark,@Contract)");
            strSql.Append(" ON DUPLICATE KEY UPDATE ");
            strSql.Append("UserNo=@CrmNo,");
            strSql.Append("TelNo=@MPhone,");
            strSql.Append("Phone=@Phone,");
            strSql.Append("Email=@Email,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("Contract=@Contract,");
            strSql.Append("Update_by=@SysUid,");
            strSql.Append("Update_dt=now();");
            //strSql.Clear();
            //strSql.Append(" INSERT INTO v3_user(Ledger ,Crm_id ,CrmNo ,CrmName ,MPhone ,IsAdmin,Email ,Phone ,Remark ,Passwd ,Create_by ,Create_dt ,Update_by , Update_dt , Contract )");
            //strSql.Append(" VALUES (@Ledger,@Crm_id,@CrmNo,@CrmName,@MPhone,@IsAdmin,@Email,@Phone,@Remark,GetENCODE(@Passwd,@EncryptPwd),@SysUid,now(),@SysUid,now(),@Contract)");
            //strSql.Append(" ON DUPLICATE KEY UPDATE ");
            //strSql.Append("CrmNo=@CrmNo,");
            //strSql.Append("CrmName=@CrmName,");
            //strSql.Append("Phone=@Phone,");
            //strSql.Append("Mphone=@Mphone,");
            //strSql.Append("Email=@Email,");
            //strSql.Append("IsAdmin=@IsAdmin,");
            //strSql.Append("Remark=@Remark,");
            //strSql.Append("Update_by=@SysUid,");
            //strSql.Append("Update_dt=now(),");
            //strSql.Append("Contract=@Contract");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = ui_Customer.Crm_id, CrmNo = ui_Customer.CrmNo, CrmName = ui_Customer.CrmName, MPhone = ui_Customer.MPhone, IsAdmin = ui_Customer.IsAdmin, Email = ui_Customer.Email, Phone = ui_Customer.Phone, Remark = ui_Customer.Remark, Passwd = ui_Customer.Passwd, EncryptPwd = EncryptPwd, SysUid = this.SysUid, Contract = ui_Customer.Contract });
        }

        /// <summary>
        /// 获取组织
        /// </summary>
        /// <param name="coid">组织ID</param>
        /// <param name="isRoom">是查房间，还是查询建筑</param>
        /// <param name="bname">建筑</param>
        /// <param name="rname">房间名</param>
        /// /// <param name="allOrg">是否把建筑与房间都查询出来</param>
        /// <param name="strcName">树节点选中筛选</param>
        /// <returns></returns>
        public DataTable GetOrgs(int coid, bool isRoom = false, string bname = "", string rname = "", string strcName = "", int allOrg = 0)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append(" select Co_id,CoStrcName,CoName,CoFullName,Parent_id,IsDefine,CoNo,CdrPrice  from vp_coinfo  where Ledger=@Ledger  and Disabled=0 ");
            if (allOrg < 1) strSql.Append("  and Attrib=@Attrib  ");
            if (coid > 0)
                strSql.Append(" and Co_id=@Co_id ");
            if (!string.IsNullOrEmpty(bname))
                strSql.Append(" and CoStrcName like @bname ");
            if (!string.IsNullOrEmpty(rname))
                strSql.Append(" and CoName like @rname ");
            if (!string.IsNullOrEmpty(strcName))
                strSql.Append(" and CoFullName like @strcName ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = coid, Attrib = isRoom == true ? 9000 : 0, bname = "%" + bname + "%", rname = "%" + rname + "%", strcName = "%" + strcName + "%" });
        }

        public DataTable GetYdCustomerRoomInfo(int Crm_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Ledger,a.Uid as Crm_id,b.Co_id,c.CoFullName,a.Create_dt");
            strSql.Append(" from vp_userinfo a inner join v3_user b on a.Ledger=b.Ledger and a.Uid=b.Crm_id ");
            strSql.Append(" left join vp_coinfo c on c.Ledger=b.Ledger and c.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Uid = @Crm_id ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = Crm_id });
        }

        public DataTable GetYdCustomerRoomFind(string CoName)
        {
            if (string.IsNullOrEmpty(CoName) || CoName == "{CoName}" || CoName == "null")
                CoName = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Ledger,a.Co_id,a.CoFullName");
            strSql.Append(" from vp_coinfo a inner join vp_mdinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.CoFullName like @CoName");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, CoName = "%" + CoName + "%" });
        }

        public int SaveYdCustomerOpenRoom(int Crm_id, string CoIdStr)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable obj = new DataTable();
            strSql.Clear();
            strSql.Append("delete from v3_user where Ledger=@Ledger and Uid=@Crm_id and not FIND_IN_SET(Co_id,@StrList) ");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = Crm_id, StrList = CoIdStr });
            int cnt = 0;
            foreach (string id in CoIdStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int co_id = CommFunc.ConvertDBNullToInt32(id);
                if (co_id == 0)
                    continue;
                strSql.Clear();
                strSql.Append("insert into v3_user(Ledger,Uid,Co_id,IsAdmin,Update_by,Update_dt)values(@Ledger,@Crm_id,@Co_id,0,@SysUid,now())");
                strSql.Append(" ON DUPLICATE KEY UPDATE IsAdmin=IsAdmin");
                cnt = cnt + SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = Crm_id, Co_id = co_id, SysUid = this.SysUid });

            }
            return cnt;
        }

        public int SaveYdCustomerOpenWd(int Crm_id, string Passwd)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable obj = new DataTable();
            strSql.Clear();
            strSql.Append("update sys_user set UPasswd=GetENCODE(@Passwd,@EncryptPwd) where Ledger=@Ledger and Uid=@Crm_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Crm_id = Crm_id, Passwd = Passwd, EncryptPwd = EncryptPwd });
        }
    }
}
