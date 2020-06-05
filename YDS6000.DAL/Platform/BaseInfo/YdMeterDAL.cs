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
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetMeterList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select c.Meter_id,c.MeterNo,c.MeterName,c.MeterAddr,c.Mm_id,c.Disabled,d.ModuleName,c.Attrib");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" inner join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
            strSql.Append(" left join v0_module as d on c.Ledger=d.Ledger and c.Mm_id=d.Mm_id");
            strSql.Append(" where a.Ledger=@Ledger");
            strSql.Append(" order by c.MeterNo,c.Meter_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 设置设备信息
        /// </summary>
        /// <param name="md"></param>
        /// <returns></returns>
        public int SetMeter(MdVModel md)
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
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = md.Meter_id, Mm_id = md.MeterTypeId });

            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("已经采集项的映射变量,不能更改设备型号,请删除映射变量再更改型号");

            if (md.Meter_id == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Meter_id)as cnt from v1_gateway_esp_meter where Ledger=@Ledger");
                md.Meter_id = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger })) + 1;
                //
                strSql.Clear();
                strSql.Append("select max(Module_id)as id from v1_gateway_esp_module where Ledger=@Ledger");
                mid = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger }));
            }
            object params01 = new
            {
                Ledger = this.Ledger,
                Meter_id = md.Meter_id,
                Esp_id = 1,
                MeterName = md.MeterName,
                MeterAddr = md.MeterName,
                MeterNo = md.MeterNo,
                Inst_loc = "",
                //Supplier = meter.Supplier,
                //Switch = meter.Switch,
                //ChrgType = meter.ChrgType,
                Disabled = md.Disabled,
                Remark = "",
                Multiply = 1,
                EnergyItemCode = "",
                Mm_id = md.MeterTypeId,
                Co_id = 0,
                Attrib = md.Attrib,
                SysUid = this.SysUid
            };
            //
            strSql.Clear();
            strSql.Append("select Number from v0_module where Ledger=@Ledger and Mm_id=@Mm_id");
            obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = md.MeterTypeId });
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
            //strSql.Append("Co_id=@Co_id,");
            strSql.Append("Attrib=@Attrib,");
            strSql.Append("Update_by=@SysUid,");
            strSql.Append("Update_dt=now();");
            //if (mid != 0)
            if (number != 0)
            {
                while ((number--) != 0)
                {
                    ++mid;
                    strSql.Append("insert into v1_gateway_esp_module(Ledger,Module_id,Meter_id,ModuleName,ModuleAddr,ModuleNo,Disabled,EnergyItemCode,Parent_id,Layer,Create_by,Create_dt,Update_by,Update_dt)values");
                    strSql.Append("(@Ledger," + mid + ", @Meter_id,'" + mid + "','" + mid + "','" + mid + "',0,@EnergyItemCode,0,0,@SysUid,now(),@SysUid,now()) ON DUPLICATE KEY UPDATE Update_by=@SysUid,Update_dt=now();");
                    strSql.Append("insert into v1_gateway_esp_module_info(Ledger,Module_id,Update_by,Update_dt)values");
                    strSql.Append("(@Ledger," + mid + ",@SysUid,now()) ON DUPLICATE KEY UPDATE Update_by=@SysUid,Update_dt=now();");
                }
            }
            /*修改了型号，就得修改变量名*/
            strSql.Append("commit;");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }
        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="meter_id"></param>
        /// <returns></returns>
        public int DelMeter(int meter_id)
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

        /// <summary>
        /// 获取设备列表(PDU)
        /// </summary>
        /// <returns></returns>
        public DataTable GetMeterList_PDU()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select c.Meter_id,c.MeterNo,c.MeterName,c.MeterAddr,c.Mm_id,c.Disabled,d.ModuleName,f.Co_id,e.CoName,f.Parent_id,h.MeterName as Parent_MeterName");
            strSql.Append(" from v1_gateway as a inner join v1_gateway_esp as b on a.Ledger=b.Ledger and a.Gw_id=b.Gw_id");
            strSql.Append(" inner join v1_gateway_esp_meter as c on b.Ledger=c.Ledger and b.Esp_id=c.Esp_id");
            strSql.Append(" left join v0_module as d on c.Ledger=d.Ledger and c.Mm_id=d.Mm_id");
            strSql.Append(" left join v1_gateway_esp_module as f on c.Ledger=f.Ledger and c.Meter_id=f.Meter_id");
            strSql.Append(" left join v1_gateway_esp_module as g on f.Ledger=g.Ledger and f.Parent_id=g.Module_id");
            strSql.Append(" left join v1_cust as e on f.Ledger=e.Ledger and f.Co_id=e.Co_id");
            strSql.Append(" left join v1_gateway_esp_meter as h on g.Ledger=h.Ledger and g.Meter_id=h.Meter_id");
            strSql.Append(" where a.Ledger=@Ledger and not exists(select 1 from v1_gateway_esp_module where Ledger=c.Ledger and Meter_id=c.Meter_id and Module_id<f.Module_id)");
            strSql.Append(" order by c.MeterNo,c.Meter_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

        /// <summary>
        /// 设置设备信息(PDU)
        /// </summary>
        /// <param name="md"></param>
        /// <returns></returns>
        public int SetMeter_PDU(MdVModel_PDU md)
        {
            int mid = -1;
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
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Meter_id = md.Meter_id, Mm_id = md.MeterTypeId });

            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("已经采集项的映射变量,不能更改设备型号,请删除映射变量再更改型号");

            if (md.Meter_id == 0)
            {
                strSql.Clear();
                strSql.Append("select count(1) from v1_gateway_esp_meter where Ledger=@Ledger and MeterName=@MeterName");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, MeterName = md.MeterName });
                if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                    throw new Exception("已经存在相同名称的设备");

                strSql.Clear();
                strSql.Append("select count(1) from v1_gateway_esp_meter where Ledger=@Ledger and MeterNo=@MeterNo");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, MeterNo = md.MeterNo });
                if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                    throw new Exception("已经存在相同编码的设备");

                strSql.Clear();
                strSql.Append("select max(Meter_id)as cnt from v1_gateway_esp_meter where Ledger=@Ledger");
                md.Meter_id = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger })) + 1;
                //
                strSql.Clear();
                strSql.Append("select max(Module_id)as id from v1_gateway_esp_module where Ledger=@Ledger");
                mid = CommFunc.ConvertDBNullToInt32(SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger }));
            }
            object params01 = new
            {
                Ledger = this.Ledger,
                Meter_id = md.Meter_id,
                Esp_id = 1,
                MeterName = md.MeterName,
                MeterAddr = md.MeterName,
                MeterNo = md.MeterNo,
                Inst_loc = "",
                //Supplier = meter.Supplier,
                //Switch = meter.Switch,
                //ChrgType = meter.ChrgType,
                Disabled = md.Disabled,
                Remark = "",
                Multiply = 1,
                EnergyItemCode = "",
                Mm_id = md.MeterTypeId,
                Co_id = md.Co_id,
                Parent_id = md.Parent_id,
                SysUid = this.SysUid
            };
            //
            strSql.Clear();
            strSql.Append("select Number from v0_module where Ledger=@Ledger and Mm_id=@Mm_id");
            obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Mm_id = md.MeterTypeId });
            int number = CommFunc.ConvertDBNullToInt32(obj);
            number = number == 0 ? 1 : number;
            //
            strSql.Clear();
            strSql.Append("START transaction;");
            strSql.Append("insert into v1_gateway_esp_meter(");
            strSql.Append("Ledger,Meter_id,Esp_id,MeterName,MeterAddr,MeterNo,Inst_loc,Disabled,Remark,Multiply,EnergyItemCode,Mm_id,Co_id,Create_by,Create_dt,Update_by,Update_dt,Attrib,ActiveTime)");
            strSql.Append(" values (@Ledger,@Meter_id,@Esp_id,@MeterName,@MeterAddr,@MeterNo,@Inst_loc,@Disabled,@Remark,@Multiply,@EnergyItemCode,@Mm_id,@Co_id,@SysUid,now(),@SysUid,now(),0,now())");
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
            strSql.Append("Update_by=@SysUid,");
            strSql.Append("Update_dt=now();");
            if (mid != -1)//add
            {
                if (number != 0)
                {
                    while ((number--) != 0)
                    {
                        ++mid;
                        strSql.Append("insert into v1_gateway_esp_module(Ledger,Module_id,Meter_id,ModuleName,ModuleAddr,ModuleNo,Disabled,EnergyItemCode,Parent_id,Layer,Co_id,Create_by,Create_dt,Update_by,Update_dt)values");
                        strSql.Append("(@Ledger," + mid + ", @Meter_id,'" + mid + "','" + mid + "','" + mid + "',0,@EnergyItemCode,@Parent_id,0,@Co_id,@SysUid,now(),@SysUid,now()) ON DUPLICATE KEY UPDATE Update_by=@SysUid,Update_dt=now();");
                        strSql.Append("insert into v1_gateway_esp_module_info(Ledger,Module_id,Update_by,Update_dt)values");
                        strSql.Append("(@Ledger," + mid + ",@SysUid,now()) ON DUPLICATE KEY UPDATE Update_by=@SysUid,Update_dt=now();");
                    }
                }
            }
            else//modify
            {
                strSql.Append("update v1_gateway_esp_module");
                strSql.Append(" set Co_id=@Co_id,Parent_id=case when Module_id=@Parent_id then Parent_id else @Parent_id end ,Update_by=@SysUid,Update_dt=now()");
                strSql.Append(" where Ledger=@Ledger and Meter_id=@Meter_id;");
                strSql.Append("update v1_gateway_esp_module_info");
                strSql.Append(" set Update_by=@SysUid,Update_dt=now()");
                strSql.Append(" where Ledger=@Ledger and Module_id in(select Module_id from v1_gateway_esp_module where Ledger=@Ledger and Meter_id=@Meter_id);");
            }
            /*修改了型号，就得修改变量名*/
            strSql.Append("commit;");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }

        /// <summary>
        /// 获取父设备列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetParentMeterCombox()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select Module_id,MeterName from vp_mdinfo");
            strSql.Append(" where Ledger=@Ledger and IsDefine=0");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger });
        }

    }
}
