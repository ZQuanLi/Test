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
    public partial class ExpBuildingNewDAL
    {
        private int Ledger = 0;
        private int SysUid = 1;
        public ExpBuildingNewDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取全部客户信息
        /// </summary>
        /// <param name="Co_id"></param>
        /// <returns></returns>
        public DataTable GetYdCustOnCoInfoList(int Co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Co_id,a.CoNo,a.CoName,a.Disabled,a.Parent_id,a.Attrib,");
            strSql.Append("b.CustAddr,b.Office_tel,b.Mobile,b.Email,b.IsDefine,m1.CoName as PCoName");
            strSql.Append(" from v1_cust as a left join v1_custinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v1_cust as m1 on a.Ledger=m1.Ledger and a.Parent_id=m1.Co_id");
            strSql.Append(" where a.Ledger=@Ledger");
            if (Co_id != 0)
                strSql.Append(" and a.Co_id=@Co_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id });
        }

        /// <summary>
        /// 获取用户的用能列表
        /// </summary>
        /// <param name="Co_id"></param>
        /// <returns></returns>
        public DataTable GetYdCustOnMdList(int Co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select GetMdChildListByCo(@Ledger,@Co_id) as SplitVal;");
            DataTable obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id });
            string SplitVal = CommFunc.ConvertDBNullToString(obj.Rows[0]["SplitVal"]);

            strSql.Clear();
            strSql.Append("select a.Gw_id,a.Esp_id,a.Module_id,a.GwName,a.EspName,a.ModuleName,a.GwAddr,a.EspAddr,a.ModuleAddr,a.MdInst_loc as Inst_loc,a.Multiply,");
            strSql.Append("ifnull(a.Parent_id,0)as Parent_id,a.Mm_id,a.ModuleType,a.FullStruc,a.Co_id,c1.CoName,a.EnergyItemCode,d.EnergyItemName,b.Price,b.MinPay,b.Rate_id,b.IsAlarm,b.AlarmVal");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" left join v1_gateway_esp_module_info as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id ");
            //strSql.Append(" left join v1_gateway_esp_module as c on a.Ledger=c.Ledger and a.Module_id=c.Module_id ");
            strSql.Append(" left join v0_energyitemdict as d on a.EnergyItemCode=d.EnergyItemCode ");
            strSql.Append(" left join v1_cust as c1 on a.Ledger=c1.Ledger and a.Co_id=c1.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and FIND_IN_SET(a.Module_id,@SplitVal);");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id, SplitVal = SplitVal });
        }

        /// <summary>
        ///判断公司名称是否存在
        /// </summary>
        /// <param name="FindName"></param>
        /// <returns>存在，返回存在ID，否则返回0</returns>
        public int IsExistSameYdCustName(string FindName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select Co_id from v1_cust where Ledger=@Ledger and CoName=@FindName limit 1");
            DataTable obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, FindName = FindName });
            int Id = 0;
            if (obj.Rows.Count > 0)
                Id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Co_id"].ToString());
            return Id;
        }

        /// <summary>
        /// 新增修改客户信息
        /// </summary>
        /// <param name="cust"></param>
        /// <returns></returns>
        public int SetCustInfo(v1_custInfoVModel cust)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            DataTable obj = new DataTable();
            if (cust.Co_id == 0)
            {
                strSql.Clear();
                strSql.Append("select Co_id from v1_cust where Ledger=@Ledger and Parent_id=@Parent_id and CoName=@CoName");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Parent_id = cust.Parent_id, CoName = cust.CoName });
                if (obj.Rows.Count > 0)
                {
                    throw new Exception("名称:" + cust.CoName + "已经在本区域中存在，请检查后使用。");
                }
                obj = null;
                strSql.Clear();
                strSql.Append("select Layer from v1_cust where Ledger=@Ledger and Co_id=@Parent_id");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Parent_id = cust.Parent_id });
                cust.Layer = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Layer"]) + 1 : 0;
                obj = null;
                strSql.Clear();
                strSql.Append("select max(Co_id) as Co_id from v1_cust where Ledger=@Ledger");
                obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
                cust.Co_id = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Co_id"]) + 1 : 1;
                //cust.Co_id = CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Co_id"]) + 1;
                strSql.Clear();
                strSql.Append("insert into v1_cust(");
                strSql.Append("Ledger,Co_id,CoNo,CoName,Disabled,Parent_id,Attrib,Layer,Create_by,Create_dt,Update_by,Update_dt)");
                strSql.Append(" values (");
                strSql.Append("@Ledger,@Co_id,@Co_id,@CoName,@Disabled,@Parent_id,@Attrib,@Layer,@SysUid,now(),@SysUid,now());");
            }
            else
            {
                strSql.Append("update v1_cust set ");
                //strSql.Append("CoNo=@CoNo,");
                strSql.Append("CoName=@CoName,");
                strSql.Append("Disabled=@Disabled,");
                strSql.Append("Parent_id=@Parent_id,");
                strSql.Append("Attrib=@Attrib,");
                strSql.Append("Update_by=@SysUid,");
                strSql.Append("Update_dt=now()");
                strSql.Append(" where Ledger=@Ledger and Co_id=@Co_id;");
            }
            strSql.Append("insert into v1_custinfo(");
            strSql.Append("Ledger,Co_id,CustAddr,Office_tel,Mobile,Email,IsDefine,Update_by,Update_dt)");
            strSql.Append(" values (");
            strSql.Append("@Ledger,@Co_id,@CustAddr,@Office_tel,@Mobile,@Email,@IsDefine,@SysUid,now())");
            strSql.Append(" ON DUPLICATE KEY UPDATE ");
            strSql.Append("CustAddr=@CustAddr,");
            strSql.Append("Office_tel=@Office_tel,");
            strSql.Append("Mobile=@Mobile,");
            strSql.Append("Email=@Email,");
            strSql.Append("IsDefine=@IsDefine,");
            strSql.Append("Update_by=@SysUid,");
            strSql.Append("Update_dt=now(); ");
            // 更新全名
            strSql.Append("update v1_cust as a,v1_custinfo as b set b.StrucName=GetCoOnStrucName(a.Ledger,a.Co_id)");
            strSql.Append(" where a.Ledger=b.Ledger and a.Co_id=b.Co_id and a.Ledger=@Ledger and FIND_IN_SET(a.Co_id,GetCoChildList(a.Ledger,@Co_id));");
            //return 1;
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Co_id = cust.Co_id, CoName = cust.CoName, Disabled = cust.Disabled, Parent_id = cust.Parent_id, Attrib = cust.Attrib, Layer = cust.Layer, SysUid = this.SysUid, CustAddr = cust.CustAddr, Office_tel = cust.Office_tel, Mobile = cust.Mobile, Email = cust.Email, IsDefine = cust.IsDefine });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Co_id">主键ID号</param>
        /// <returns></returns>
        public int GeDelCo(int Co_id)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable obj = new DataTable();
            string SplitCoChild = "";
            strSql.Clear();
            strSql.Append("select GetCoChildList(@Ledger,@Co_id) as SplitCoChild");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id });
            SplitCoChild = CommFunc.ConvertDBNullToString(obj.Rows[0]["SplitCoChild"]);

            obj = null;
            strSql.Clear();
            strSql.Append("select count(a.Co_id) as cnt");
            strSql.Append(" from v1_cust as a inner join v2_info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and FIND_IN_SET(a.Co_id,@SplitCoChild);");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, SplitCoChild = SplitCoChild });
            if (CommFunc.ConvertDBNullToInt32(obj.Rows[0]["cnt"]) > 0)
            {
                throw new Exception("有用户已进行消费，不能删除");
            }

            strSql.Clear();
            strSql.Append("update v1_cust as a inner join v1_gateway_esp_meter as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" inner join v1_gateway_esp_module as c on b.Ledger=c.Ledger and b.Meter_id=c.Meter_id");
            strSql.Append(" set b.Co_id=0,c.Co_id=0 where a.Ledger=@Ledger and FIND_IN_SET(a.Co_id,@SplitCoChild);");
            strSql.Append("DELETE d,c,b,a from v1_cust as a LEFT JOIN v1_custinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v1_custinfobuild as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" left join v1_custinfobuild_fee as d on a.Ledger=d.Ledger and a.Co_id=d.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and FIND_IN_SET(a.Co_id,@SplitCoChild);");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, SplitCoChild = SplitCoChild });
        }

        /// <summary>
        /// 查找用能列表
        /// </summary>
        /// <param name="Co_id">主键</param>
        /// <param name="FindStr">电能表地址</param>
        /// <returns></returns>
        public DataTable GetYdCustOnMdInFind(int Co_id, string FindStr)
        {
            if (string.IsNullOrEmpty(FindStr) || FindStr == "{FindStr}" || FindStr == "null")
                FindStr = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select GetMdChildListByCo(@Ledger,@Co_id) as splitChild");
            DataTable obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id });
            string SplitCoChild = CommFunc.ConvertDBNullToString(obj.Rows[0]["splitChild"]);

            //获取 非其他组织，不在此组织的设备
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Struc,a.FullStruc");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and not FIND_IN_SET(a.Module_id,@SplitChild)");
            if (!string.IsNullOrEmpty(FindStr))
            {
                strSql.Append(" and (");
                strSql.Append("(a.ModuleAddr like @FindStr)");
                strSql.Append(")");
            }
            strSql.Append(" limit 50");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, SplitChild = SplitCoChild, FindStr = "%" + FindStr + "%" });
        }

        /// <summary>
        /// 获取计量点的总加组设置
        /// </summary>
        /// <param name="Module_id"></param>
        /// <returns></returns>
        public DataTable GetYdCustInMdFormula(int Module_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Struc,a.FullStruc");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and a.Parent_id=@Module_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id });
        }

        /// <summary>
        /// 获取计量点有序用电可进行总加组信息
        /// </summary>
        /// <param name="Co_id"></param>
        /// <param name="Module_id"></param>
        /// <param name="FindStr"></param>
        /// <returns></returns>
        public DataTable GetYdCustInMdFormulaFind(int Co_id, int Module_id, string FindStr)
        {
            if (string.IsNullOrEmpty(FindStr) || FindStr == "{FindStr}" || FindStr == "null")
                FindStr = string.Empty;
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            //获取 非其他设备子项，非此设备下的子项，不在此组织的设备
            strSql.Append("select a.Module_id,a.ModuleAddr,a.ModuleName,a.Struc,a.FullStruc");
            strSql.Append(" from vp_mdinfo as a");
            strSql.Append(" where a.Ledger=@Ledger and ifnull(a.Parent_id,0)=0 and not FIND_IN_SET(a.Module_id,GetMdChildList(@Ledger,@Module_id)) and a.Co_id!=@Co_id");
            if (!string.IsNullOrEmpty(FindStr))
            {
                strSql.Append(" and (");
                strSql.Append("(a.ModuleAddr like @FindStr)");
                strSql.Append("or (a.ModuleName like @FindStr)");
                //strSql.Append("or (a.GwAddr like @FindStr)");
                strSql.Append(")");
            }
            strSql.Append(" limit 50");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id, Co_id = Co_id, FindStr = "%" + FindStr + "%" });
        }

        /// <summary>
        /// 单独设置电能表
        /// </summary>
        /// <param name="Module_id"></param>
        /// <param name="EnergyItemCode"></param>
        /// <param name="Rate_id"></param>
        /// <param name="Price"></param>
        /// <param name="AlarmVal"></param>
        /// <param name="IsAlarm"></param>
        /// <returns></returns>
        public int Setv1_gateway_esp_module_info(int Module_id, string EnergyItemCode, int Rate_id, decimal Price, decimal AlarmVal, int IsAlarm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append(" update v1_gateway_esp_meter as a inner join v1_gateway_esp_module as b on a.Ledger=b.Ledger and a.Meter_id=b.Meter_id");
            strSql.Append(" set a.EnergyItemCode=@EnergyItemCode,b.EnergyItemCode=@EnergyItemCode,a.Update_by=@SysUid,a.Update_dt=now()");
            strSql.Append(" where a.Ledger=@Ledger and b.Module_id=@Module_id;");
            strSql.Append("insert into v1_gateway_esp_module_info (Ledger,Module_id,Rate_id,Si_id,Price,MinPay,IsAlarm,Update_by,Update_dt,AlarmVal)");
            strSql.Append("values(@Ledger,@Module_id,@Rate_id,0,@Price,@MinPay,@IsAlarm,@SysUid,now(),@AlarmVal)");
            strSql.Append(" on duplicate key update Price=@Price,Rate_id=@Rate_id,IsAlarm=@IsAlarm,Update_by=@SysUid,Update_dt=now(),AlarmVal=@AlarmVal");
            //return 1;
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id, EnergyItemCode = EnergyItemCode, Rate_id = Rate_id, Price = Price, AlarmVal = AlarmVal, MinPay = 0, IsAlarm = IsAlarm, SysUid = this.SysUid });
        }

        /// <summary>
        /// 设置电表所属公司
        /// </summary>
        /// <param name="Co_id">公司ID</param>
        /// <param name="IdStr">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        public int YdCustOnMdInSave(int Co_id, string IdStr)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable obj = new DataTable();
            strSql.Clear();
            strSql.Append("select b.Module_id,3 as nAct from v1_gateway_esp_meter as a inner join v1_gateway_esp_module as b on a.Ledger=b.Ledger and a.Meter_id=b.Meter_id where a.Ledger=@Ledger and b.Co_id=@Co_id");// and ifnull(IsOrg,0)=1");/*查找资料*/
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id });
            int cnt = 0;
            int Module_id;
            string SplitChild = "";
            foreach (string id in IdStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int cid = CommFunc.ConvertDBNullToInt32(id);
                Module_id = cid;
                int nAct = 1; /*新增*/
                foreach (DataRow drInfo in obj.Select("Module_id=" + cid))
                {
                    nAct = 2;/*修改*/
                    drInfo["nAct"] = nAct;
                }
                if (nAct == 1)
                {
                    /*新增,更新组织为1 ，其他设备子项全部更新为此组织*/
                    strSql.Clear();
                    strSql.Append("select GetMdChildList(@Ledger,@Module_id) as MdChild");
                    DataTable obj2 = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id });
                    SplitChild = CommFunc.ConvertDBNullToString(obj2.Rows[0]["MdChild"]);
                    //SplitChild = CommFunc.ConvertDBNullToString(SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id }));
                    strSql.Clear();
                    strSql.Append("update v1_gateway_esp_meter as a inner join v1_gateway_esp_module as b on a.Ledger=b.Ledger and a.Meter_id=b.Meter_id");
                    strSql.Append(" set a.Co_id=@Co_id,b.Co_id=@Co_id");
                    strSql.Append(" where a.Ledger=b.Ledger and a.Meter_id=b.Meter_id and a.Ledger=@Ledger and FIND_IN_SET(b.Module_id,@SplitChild)");
                    cnt = cnt + SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id, SplitChild = SplitChild });
                }
            }
            foreach (DataRow dr in obj.Select("nAct=3"))
            {/*删除,清除加总组和公司关系*/
                int cid = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);
                Module_id = cid;
                strSql.Clear();
                strSql.Append("select GetMdChildList(@Ledger,@Module_id) as MdChild");
                DataTable obj3 = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id });
                SplitChild = CommFunc.ConvertDBNullToString(obj3.Rows[0]["MdChild"]);
                //string sc = CommFunc.ConvertDBNullToString(SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id }));
                //SplitChild = sc;
                //清楚全部关系
                strSql.Clear();
                strSql.Append("update v1_gateway_esp_meter as a inner join v1_gateway_esp_module as b on a.Ledger=b.Ledger and a.Meter_id=b.Meter_id");
                strSql.Append(" set a.Co_id=case when ifnull(a.Co_id,0)=@Co_id then 0 else a.Co_id end,");
                strSql.Append("b.Co_id=case when ifnull(b.Co_id,0)=@Co_id then 0 else b.Co_id end,");
                strSql.Append("b.Parent_id=case when b.Module_id=@Module_id then b.Parent_id else 0 end,");
                strSql.Append("b.Layer=case when b.Module_id=@Module_id then b.Layer else 0 end,");
                strSql.Append("b.Update_by=@SysUid,b.Update_dt=now()");
                strSql.Append(" where a.Ledger=@Ledger and FIND_IN_SET(b.Module_id,@SplitChild);");
                cnt = cnt + SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id, Module_id = Module_id, SplitChild = SplitChild, SysUid = this.SysUid });
            }
            return cnt;
        }


        public int SetYdCustInMdFormula(int Co_id, int Module_id, string IdStr)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable obj = new DataTable();
            strSql.Clear();

            int cnt = 0;
            int SubItme, Layer;
            string SplitVal = "";

            strSql.Append("select Layer from v1_gateway_esp_module where Module_id=@Module_id");
            obj = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id });
            Layer = obj.Rows.Count > 0 ? CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Layer"]) + 1 : 0;/*获取层级数*/
            //Layer = obj == null ? 0 : CommFunc.ConvertDBNullToInt32(obj.Rows[0]["Layer"]) + 1;

            strSql.Clear();
            strSql.Append("select GetMdChildList(@Ledger,@Module_id) as MdChild");
            DataTable obj2 = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id });
            SplitVal = CommFunc.ConvertDBNullToString(obj2.Rows[0]["MdChild"]);

            ///////////////////先获取所有的组织
            strSql.Clear();
            strSql.Append("select b.Module_id,b.Parent_id,b.Layer,a.Co_id,3 as nAct from v1_gateway_esp_meter as a inner join v1_gateway_esp_module as b on a.Ledger=b.Ledger and a.Meter_id=b.Meter_id");
            strSql.Append(" where a.Ledger=@Ledger and b.Module_id!=@Module_id and FIND_IN_SET(b.Module_id,@SplitVal)");
            DataTable dtSource = SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Module_id = Module_id, SplitVal = SplitVal });


            foreach (string id in IdStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int cid = CommFunc.ConvertDBNullToInt32(id);
                SubItme = cid;
                int nAct = 1; /*新增*/
                foreach (DataRow drInfo in dtSource.Select("Module_id=" + cid))
                {
                    nAct = 2;/*修改*/
                    drInfo["nAct"] = nAct;
                }
                if (nAct == 1)
                {
                    /*新增,更新组织为此组织*/
                    strSql.Clear();
                    strSql.Append("update v1_gateway_esp_module as b set  b.Co_id=case when ifnull(b.Co_id,0)=0 then @Co_id else b.Co_id end,b.Parent_id=@Module_id,b.Layer=@Layer,b.Update_by=@SysUid,b.Update_dt=now()");
                    strSql.Append(" where b.Ledger=@Ledger and b.Module_id=@SubItme;");
                    cnt = cnt + SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id, Module_id = Module_id, Layer = Layer, SysUid = this.SysUid, SubItme = SubItme });
                }
            }
            foreach (DataRow dr in dtSource.Select("nAct=3"))
            {/*删除,清除加总组关系*/
                int cid = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);
                SubItme = cid;
                strSql.Clear();
                strSql.Append("update v1_gateway_esp_module as b set b.Parent_id=0,b.Layer=0,");
                strSql.Append("b.Co_id=case when ifnull(b.Co_id,0)=@Co_id then 0 else b.Co_id end,");
                strSql.Append("b.Update_by=@SysUid,b.Update_dt=now()");
                strSql.Append(" where b.Ledger=@Ledger and b.Module_id=@SubItme;");
                cnt = cnt + SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id, SysUid = this.SysUid, SubItme = SubItme });
            }
            return cnt;
        }

        /// <summary>
        /// 分类分项编码下拉列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetEnergyItemCode()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append(" select * FROM v0_energyitemdict WHERE ParentItemCode='01000'; ");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        #region 建筑明细信息

        public DataTable GetYdCustInfoBuildData(int Co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select b.Co_id,b.Area,a.CustName,b.CorpName,b.BuildType,b.Bank");
            strSql.Append(" from vp_coinfo as a inner join v1_custinfobuild as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id });
        }
        public int SaveYdCustInfoBuildData(int Co_id, decimal Area, string BuildType, string CustName, string CorpName, int Bank)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable obj = new DataTable();
            strSql.Clear();
            strSql.Append("update v1_custinfo set CustName=@CustName where Ledger=@Ledger and Co_id=@Co_id;");
            strSql.Append("insert into v1_custinfobuild(Ledger,Co_id,Area,BuildType,Update_by,Update_dt,Bank,CorpName)values(@Ledger,@Co_id,@Area,@BuildType,@SysUid,now(),@Bank,@CorpName)");
            strSql.Append(" ON DUPLICATE KEY UPDATE Area=@Area,BuildType=@BuildType,Update_by=@SysUid,Update_dt=now(),Bank=@Bank,CorpName=@CorpName");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id, SysUid = this.SysUid, Area = Area, BuildType = BuildType, CustName = CustName, CorpName = CorpName, Bank = Bank }); ;
        }

        #endregion


        #region 物业信息

        public DataTable GetYdCustInfoBuild_fee(int Co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select a.Cic_id,a.CicName,b.Rate_id,c.Descr as RateName,b.Remark,c.Rule,c.Unit,c.UnitBase");
            strSql.Append(" from v0_cic as a left join v1_custinfobuild_fee as b on a.Ledger=b.Ledger and a.Cic_id=b.Cic_id and b.Co_id=@Co_id");
            strSql.Append(" left join v1_rate as c on b.Ledger=c.Ledger and b.Rate_id=c.Rate_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Attrib=@Attrib and a.IsCtl=0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id, Attrib = (int)CicAttrib.ChrgType });
        }
        public int SaveYdCustInfoBuildFee(int Co_id, int Cic_id, int Rate_id)
        {
            StringBuilder strSql = new StringBuilder();
            DataTable obj = new DataTable();
            strSql.Clear();
            if (Rate_id == 0)
            {
                strSql.Clear();
                strSql.Append("delete from v1_custinfobuild_fee where Ledger=@Ledger and Co_id=@Co_id and Cic_id=@Cic_id");
            }
            else
            {
                strSql.Clear();
                strSql.Append("insert into v1_custinfobuild_fee(Ledger,Co_id,Cic_id,Rate_id,Disabled,Remark,Update_by,Update_dt)values(@Ledger,@Co_id,@Cic_id,@Rate_id,0,'',@SysUid,now())");
                strSql.Append(" ON DUPLICATE KEY UPDATE Rate_id=@Rate_id,Update_by=@SysUid,Update_dt=now()");
            }
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Co_id = Co_id, SysUid = this.SysUid, Cic_id = Cic_id, Rate_id = Rate_id }); ;
        }

        #endregion


        public DataTable GetHomeBuilding(int Attrib)
        {
            if (string.IsNullOrEmpty(Attrib.ToString()))
                Attrib = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("SELECT * FROM v1_cust");
            strSql.Append(" where Ledger=@Ledger and Attrib=@Attrib"); // 0 = 空，100=建筑 ， 9000 = 房间
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = Attrib });
        }


    }
}
