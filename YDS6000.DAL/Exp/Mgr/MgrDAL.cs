using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.Exp.Mgr
{
    public partial class MgrDAL
    {
        private int Ledger = 0;
        private int SysUid = 0;
        public MgrDAL(int ledger, int uid)
        {
            this.Ledger = ledger;
            this.SysUid = uid;
        }

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="attrib"></param>
        /// <returns></returns>
        public DataTable GetVp_coinfo(CoAttribV2_1 attrib)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Co_id,a.CoNo,a.CoName,a.CustAddr,a.Mobile,a.Parent_id,a.Remark,a.CoStrcName,a.Disabled,b.CoName as ParentName,a.Path");
            strSql.Append(" from vp_coinfo as a");
            strSql.Append(" left join v1_cust as b on a.Ledger=b.Ledger and a.Parent_id=b.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Attrib=@Attrib");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = (int)attrib});
        }

        public int UpdateCoInfo(int co_id, string coNo, string coName, int parent_id, CoAttribV2_1 attrib ,string custAddr, string mobile, string remark, int disabled)
        {
            StringBuilder strSql = new StringBuilder();
            int layer = 0;
            strSql.Clear();
            strSql.Append("select count(*) from v1_cust where Ledger=@Ledger and CoName=@CoName and Co_id!=@Co_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, CoName = coName, Co_id = co_id });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("名称:" + coName + "已存在");

            if (parent_id != 0)
            {
                strSql.Clear();
                strSql.Append("select Layer from v1_cust where Ledger=@Ledger and Co_id=@Co_id");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = parent_id });
                layer = CommFunc.ConvertDBNullToInt32(obj) + 1;
            }
            if (co_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Co_id)as Co_id from v1_cust where Ledger=@Ledger");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger });
                co_id = CommFunc.ConvertDBNullToInt32(obj) + 1;
            }
            object params01 = new
            {
                Ledger = this.Ledger,
                Co_id = co_id,
                CoNo = coNo,
                CoName = coName,
                Parent_id = parent_id,
                CustAddr = custAddr,
                Mobile = mobile,
                Email = "",
                Remark = remark,
                Disabled = disabled,
                Attrib = (int)attrib,
                Layer = layer,
                SysUid = this.SysUid
            };
            ///////////////////////////////////////////////////
            strSql.Clear();
            strSql.Append("insert into v1_cust(Ledger,Co_id,CoNo,CoName,Disabled,Parent_id,Attrib,Layer,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,@Co_id,@CoNo,@CoName,@Disabled,@Parent_id,@Attrib,@Layer,@SysUid,now(),@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE CoNo=@CoNo,CoName=@CoName,Parent_id=@Parent_id,Disabled=@Disabled,Update_by=@SysUid,Update_dt=now();");
            strSql.Append("insert into v1_custinfo(Ledger,Co_id,CustAddr,Mobile,Email,Remark,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,@Co_id,@CustAddr,@Mobile,@Email,@Remark,@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE CustAddr=@CustAddr,Mobile=@Mobile,Email=@Email,Remark=@Remark,Update_by=@SysUid,Update_dt=now();");
            strSql.Append("update v1_custinfo set StrucName=GetCoOnStrucName(Ledger,Co_id) where Ledger=@Ledger and FIND_IN_SET(Co_id,GetCoChildList(Ledger,@Co_id))"); // 更新全名
            int cnt = SQLHelper.Execute(strSql.ToString(), params01);
            return cnt;
        }

        /// <summary>
        /// 删除区域、单位、项目、用电单元
        /// </summary>
        /// <param name="co_id">ID号</param>
        /// <returns></returns>
        public int DelCoInfo(int co_id)
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
                throw new Exception("请先删除已经使用的信息");
            strSql.Clear();
            strSql.Append("select count(*)as cnt from vp_mdinfo where Ledger=@Ledger and Co_id=@Co_id");
            obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("请先删除已经使用过的设备信息");
            //
            strSql.Clear();
            strSql.Append("delete d,c,b,a from v1_cust as a left join v1_custinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v1_custinfobuild as c on a.Ledger=c.Ledger and a.Co_id=c.Co_id");
            strSql.Append(" left join v1_custinfobuild_fee as d on a.Ledger=d.Ledger and a.Co_id=d.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Co_id=@Co_id;");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }

        public DataTable GetMdList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select c.Meter_id,c.MeterNo,c.MeterName,c.MeterAddr,c.Co_id,c.Mm_id,c.Disabled,c1.CoName,m1.ModuleType,m1.ModuleName,m1.Number,c1.Path,c.Inst_loc");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger = b.Ledger and a.Gw_id = b.Gw_id");
            strSql.Append(" inner join v1_gateway_esp_meter as c on b.Ledger = c.Ledger and b.Esp_id = c.Esp_id");
            strSql.Append(" left join vp_coinfo as c1 on c.Ledger = c1.Ledger and c.Co_id = c1.Co_id");
            strSql.Append(" left join v0_module as m1 on c.Ledger = m1.Ledger and c.Mm_id = m1.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger});
        }
        public DataTable GetMapList(int meter_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Module_id,c.Fun_id,d.TagName,d.DataValue,a.ModuleName,c.FunType,c.FunName,c.Action");
            strSql.Append(" from vp_mdinfo as a inner join v0_module as b on a.Ledger = b.Ledger and a.Mm_id = b.Mm_id");
            strSql.Append(" inner join v0_fun as c on b.Ledger=c.Ledger and b.Mm_id=c.Mm_id");
            strSql.Append(" left join v1_map as d on a.Ledger = d.Ledger and a.Module_id=d.Module_id and c.Fun_id = d.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Meter_id=@Meter_id");
            //strSql.Append("select a.Module_id,a.Fun_id,a.TagName,a.DataValue,b.ModuleName,c.FunType,c.FunName,c.Action");
            //strSql.Append(" from v1_map as a inner join vp_mdinfo as b on a.Ledger = b.Ledger and a.Module_id = b.Module_id");
            //strSql.Append(" inner join v0_fun as c on a.Ledger = c.Ledger and a.Fun_id = c.Fun_id");
            //strSql.Append(" where a.Ledger=@Ledger and b.Meter_id=@Meter_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = meter_id });
        }
        /// <summary>
        /// 保存设备信息
        /// </summary>
        /// <param name="id">设备ID号</param>
        /// <param name="name">设备名称</param>
        /// <param name="cellId">用电单元id号</param>
        /// <param name="moduleId">设备型号id号</param>
        /// <param name="disabled">设备状态=0正常=1弃用</param>
        /// <returns></returns>
        public int SaveMdInfo(int id, string name, int cellId, int moduleId, int disabled)
        {
            int mid = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("insert into v1_gateway(Ledger,Gw_id,GwName,GwAddr,GwIp,GwPort,Timeout,Disabled,Create_by,Create_dt,Update_by,Update_dt)values");
            strSql.Append("(@Ledger,1,'1','1','0.0.0.0','80',1000,0,@SysUid,now(),@SysUid,now()) ON DUPLICATE KEY UPDATE Update_by=@SysUid,Update_dt=now();");
            strSql.Append("insert into v1_gateway_esp(Ledger,Esp_id,Gw_id,EspName,EspAddr,TransferType,EspIp,EspPort,Timeout,EspType,ComPort,Baud,DataBit,StopBit,Parity,Disabled,Create_by,Create_dt,Update_by,Update_dt)values");
            strSql.Append("(@Ledger,1,1,'1','1',4,'0.0.0.0','80',1000,'','COM1',9600,8,2,1,0,@SysUid,now(),@SysUid,now()) ON DUPLICATE KEY UPDATE Update_by=@SysUid,Update_dt=now();");
            SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, SysUid = this.SysUid });
            strSql.Clear();
            strSql.Append("select case when a.Mm_id=@Mm_id then 0 else 1 end as cnt");
            strSql.Append(" from vp_funinfo as a inner join v1_map as b on a.Ledger=b.Ledger and a.Module_id=b.Module_id and a.Fun_id=b.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Meter_id=@Meter_id limit 1");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = id, Mm_id = moduleId });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("不能更改设备型号,请先删除采集项数据");
            bool isAdd = false;
            if (id == 0)
            {
                isAdd = true;
                strSql.Clear();
                strSql.Append("select max(Meter_id)as cnt from v1_gateway_esp_meter where Ledger=@Ledger");
                id = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger })) + 1;
                //
                strSql.Clear();
                strSql.Append("select max(Module_id)as id from v1_gateway_esp_module where Ledger=@Ledger");
                mid = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger }));
            }
            else
            {

            }
            object params01 = new
            {
                Ledger = this.Ledger,
                Meter_id = id,
                Esp_id = 1,
                MeterName = name,
                MeterAddr = id,
                MeterNo = "",
                Inst_loc = "",
                //Supplier = meter.Supplier,
                //Switch = meter.Switch,
                //ChrgType = meter.ChrgType,
                Disabled = disabled,
                Remark = "",
                Multiply = 1,
                EnergyItemCode = "",
                Mm_id = moduleId,
                Co_id = cellId,
                Attrib = 0,
                SysUid = this.SysUid
            };
            //
            strSql.Clear();
            strSql.Append("select Number from v0_module where Ledger=@Ledger and Mm_id=@Mm_id");
            obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = moduleId });
            int number = CommFunc.ConvertDBNullToInt32(obj);
            number = number == 0 ? 1 : number;
            //
            strSql.Clear();
            strSql.Append("START transaction;");
            strSql.Append("insert into v1_gateway_esp_meter(");
            strSql.Append("Ledger,Meter_id,Esp_id,MeterName,MeterAddr,MeterNo,Inst_loc,Disabled,Remark,Multiply,EnergyItemCode,Mm_id,Co_id,Create_by,Create_dt,Update_by,Update_dt,Attrib,ActiveTime)");
            strSql.Append(" values (@Ledger,@Meter_id,@Esp_id,@MeterName,@MeterAddr,@MeterNo,@Inst_loc,@Disabled,@Remark,@Multiply,@EnergyItemCode,@Mm_id,@Co_id,@SysUid,now(),@SysUid,now(),@Attrib,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE ");
            strSql.Append("Esp_id=@Esp_id,");
            strSql.Append("MeterName=@MeterName,");
            strSql.Append("MeterNo=@MeterNo,");
            //strSql.Append("MeterAddr=@MeterAddr,");
            strSql.Append("Inst_loc=@Inst_loc,");
            strSql.Append("Disabled=@Disabled,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("Multiply=@Multiply,");
            strSql.Append("Mm_id=@Mm_id,");
            strSql.Append("Co_id=@Co_id,");
            strSql.Append("Attrib=@Attrib,");
            strSql.Append("Update_by=@SysUid,");
            strSql.Append("Update_dt=now();");
            if (isAdd == true)
            {
                if (number != 0)
                {
                    while ((number--) != 0)
                    {
                        ++mid;
                        strSql.Append("insert into v1_gateway_esp_module(Ledger,Module_id,Meter_id,ModuleName,ModuleAddr,ModuleNo,Disabled,EnergyItemCode,Parent_id,Layer,Co_id,Create_by,Create_dt,Update_by,Update_dt)values");
                        strSql.Append("(@Ledger," + mid + ", @Meter_id,@MeterName,'" + mid + "','" + mid + "',@Disabled,@EnergyItemCode,0,0,@Co_id,@SysUid,now(),@SysUid,now());"); // ON DUPLICATE KEY UPDATE Co_id=@Co_id,Update_by=@SysUid,Update_dt=now();
                        strSql.Append("insert into v1_gateway_esp_module_info(Ledger,Module_id,Update_by,Update_dt)values");
                        strSql.Append("(@Ledger," + mid + ",@SysUid,now()) ON DUPLICATE KEY UPDATE Update_by=@SysUid,Update_dt=now();");
                    }
                }
            }
            else
            {
                strSql.Append("update v1_gateway_esp_module set ModuleName=@MeterName,Co_id=@Co_id,Update_by=@SysUid,Update_dt=now() where Ledger=@Ledger and Meter_id=@Meter_id;");
            }
            /*修改了型号，就得修改变量名*/
            strSql.Append("commit;");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }

        /// <summary>
        /// 更新设备位置信息
        /// </summary>
        /// <param name="meter_id">设备ID号</param>
        /// <param name="inst_loc">设备安装地址</param>
        /// <returns></returns>
        public int UpdateInst_loc(int meter_id, string inst_loc)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v1_gateway_esp_meter set Inst_loc=@Inst_loc where Ledger=@Ledger and Meter_id=@Meter_id;");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = meter_id, Inst_loc = inst_loc });
        }
        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="meter_id"></param>
        /// <returns></returns>
        public int DelMdInfo(int meter_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select count(a.Ledger)as cnt from vp_mdinfo as a inner join v2_info as b");
            strSql.Append(" on a.Ledger=b.Ledger and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr");
            strSql.Append(" where a.Ledger=@Ledger and a.Meter_id=@Meter_id  limit 1");
            Object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = meter_id });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("此设备已经有采集数据不能删除");
            strSql.Clear();
            strSql.Append("delete d,c,b,a from v1_gateway_esp_meter as a left join v1_gateway_esp_module as b on a.Ledger=b.Ledger and a.Meter_id=b.Meter_id");
            strSql.Append(" left join v1_gateway_esp_module_info as c on b.Ledger=c.Ledger and b.Module_id=c.Module_id");
            strSql.Append(" left join v1_map as d on b.Ledger=d.Ledger and b.Module_id=d.Module_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Meter_id=@Meter_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = meter_id });
        }
        public int SaveMapInfo(int module_id, int fun_id, string tagName, string dataValue)
        {
            StringBuilder strSql = new StringBuilder();
            if (string.IsNullOrEmpty(tagName))
            {
                strSql.Clear();
                strSql.Append("delete from v1_map where Ledger=@Ledger and Module_id=@Module_id and Fun_id=@Fun_id");
            }
            else
            {
                strSql.Clear();
                strSql.Append("insert into v1_map(Ledger,Module_id,Fun_id,TagName,DataValue,Update_by,Update_dt)values");
                strSql.Append("(@Ledger,@Module_id,@Fun_id,@TagName,@DataValue,@SysUid,now())");
                strSql.Append(" ON DUPLICATE KEY UPDATE TagName=@TagName,DataValue=@DataValue,Update_by=@SysUid,Update_dt=now()");
            }
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, Fun_id = fun_id, TagName = tagName, DataValue = dataValue, SysUid = this.SysUid });
        }

        /// <summary>
        /// 清零
        /// </summary>
        /// <returns></returns>
        public int SetClear(int module_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("START TRANSACTION;");
            strSql.Append("insert into v2_info(`Ledger`, `Co_id`, `Module_id`, `ModuleAddr`, `Fun_id`, `Hist_id`, `YT_id`,");
            strSql.Append("`BeginVal`, `FirstVal`, `LastVal`, `MinVal`, `MaxVal`, `FirstTime`, `LastTime`, `MinTime`, `MaxTime`, `DataCfg`,");
            strSql.Append("`FirstVal1st`,`FirstVal2nd`, `FirstVal3rd`, `FirstVal4th`, `LastVal1st`, `LastVal2nd`, `LastVal3rd`, `LastVal4th`, `ChargVal`, `RdVal`, `DebtTime`,");
            strSql.Append("`Create_by`, `Create_dt`, `Update_by`, `Update_dt`, `BaseVal`, `RaVal`, `InVal`, `Remark`)");
            strSql.Append(" select a.Ledger,a.Co_id,a.Module_id,a.ModuleAddr,a.Fun_id,ifnull(max.Hist_id,0)+1,a.YT_id,");
            strSql.Append(" a.BeginVal,a.FirstVal,a.LastVal,a.MinVal,a.MaxVal,a.FirstTime,a.LastTime,a.MinTime,a.MaxTime,a.DataCfg,");
            strSql.Append(" a.FirstVal1st,a.FirstVal2nd,a.FirstVal3rd,a.FirstVal4th,a.LastVal1st,a.LastVal2nd,a.LastVal3rd,a.LastVal4th,a.ChargVal,a.RdVal,DebtTime,");
            strSql.Append(" @SysUid, now(), @SysUid, now(), a.BaseVal,a.RaVal,a.InVal,'底度清零'");
            strSql.Append(" from v2_info as a inner join syscont as b on a.Ledger=b.Ledger");
            strSql.Append(" inner join vp_funinfo as fun on a.Ledger=fun.Ledger and a.Co_id = fun.Co_id and a.Module_id = fun.Module_id and a.ModuleAddr = fun.ModuleAddr and a.Fun_id = fun.Fun_id");
            strSql.Append(" left join (select Ledger,Co_id,Module_id,ModuleAddr,Fun_id,max(Hist_id)as Hist_id from v2_info group by Ledger,Co_id,Module_id,ModuleAddr,Fun_id) as max");
            strSql.Append(" on a.Ledger=max.Ledger and a.Co_id = max.Co_id and a.Module_id = max.Module_id and a.ModuleAddr = max.ModuleAddr and a.Fun_id = max.Fun_id");
            strSql.Append(" where a.Ledger=@Ledger and a.Hist_id=0 and fun.FunType='E'");
            if (module_id != 0)
                strSql.Append(" and a.Module_id=@Module_id");
            strSql.Append(";");
            //
            strSql.Append("update vp_v2info as a inner join syscont as b on a.Ledger=b.Ledger");
            strSql.Append(" inner join vp_funinfo as fun on a.Ledger=fun.Ledger and a.Co_id = fun.Co_id and a.Module_id = fun.Module_id and a.ModuleAddr = fun.ModuleAddr and a.Fun_id = fun.Fun_id");
            strSql.Append(" set a.FirstVal = 0");
            strSql.Append(" where a.Ledger=@Ledger and a.Hist_id=0 and fun.FunType='E'");
            if (module_id != 0)
                strSql.Append(" and a.Module_id=@Module_id");
            strSql.Append(";");
            //
            strSql.Append("COMMIT;");

            //strSql.Append("update vp_v2info as a inner join vp_mdinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            //strSql.Append(" and a.Module_id=b.Module_id and a.ModuleAddr=b.ModuleAddr");
            //strSql.Append(" set a.FirstVal=0");
            //strSql.Append(" where a.Ledger=@Ledger and (a.FunType='E' or a.IsCharg=1)");
            //if (module_id != 0)
            //    strSql.Append(" and a.Module_id=@Module_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id , SysUid  = this.SysUid });
        }
        public string GetImg(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Path from v1_custinfo where Ledger=@Ledger and Co_id=@Co_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id, SysUid = this.SysUid });
            return CommFunc.ConvertDBNullToString(obj);
        }
        public int UpdateImg(int co_id, string path)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update v1_custinfo set Path=@Path where Ledger=@Ledger and Co_id=@Co_id");
            return SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id, Path=path ,SysUid = this.SysUid });
        }

        public int ImportTagsFromIOServer(int module_id, string busname, string devname)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Busname,Devname,Tagname from ydrdb.tag_info where Devname=@Devname and Busname=@Busname");
            DataTable dtSource = SQLHelper.Query(strSql.ToString(), new { Devname = devname, Busname = busname });
            int cc = 0;
            foreach (DataRow dr in dtSource.Rows)
            {
                string tagname = CommFunc.ConvertDBNullToString(dr["Tagname"]);
                string funType = CommFunc.ConvertDBNullToString(tagname.Replace(busname + "." + devname + ".", ""));
                strSql.Clear();
                strSql.Append("insert into v1_map(Ledger,Module_id,Fun_id,TagName,DataValue,Update_by,Update_dt)");
                strSql.Append(" select a.Ledger,a.Module_id,a.Fun_id,@TagName,'',@SysUid,now() from vp_funinfo as a where a.Ledger=@Ledger and a.Module_id=@Module_id and a.ExSp=@FunType");
                strSql.Append(" ON DUPLICATE KEY UPDATE TagName=@TagName,Update_by=@SysUid,Update_dt=now()");
                cc = cc + SQLHelper.Execute(strSql.ToString(), new { Ledger = this.Ledger, Module_id = module_id, FunType = funType, TagName = tagname, SysUid = this.SysUid });
            }
            return cc;
        }

    }
}
