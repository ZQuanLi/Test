using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBUtility;
using YDS6000.Models;

namespace YDS6000.DAL.BaseInfo
{
    partial class BaseInfoDAL
    {
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <param name="co_id">区域ID号</param>
        /// <returns></returns>
        public DataTable GetAreaList(int co_id,string coName)
        {
            StringBuilder strSql = new StringBuilder();
            //string strlist = "";
            //strSql.Clear();
            //strSql.Append("select GetCoChildList(@Ledger,@Co_id)");
            //object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
            //strlist = CommFunc.ConvertDBNullToString(obj);
            int topId = 0;
            if (!string.IsNullOrEmpty(coName))
            {
                strSql.Clear();
                strSql.Append("select Co_id from vp_coinfo where Ledger=@Ledger and Attrib=@Attrib and CoName like @CoName limit 1");
                object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, CoName = coName + "%", Attrib = CoAttrib.Area });
                topId = CommFunc.ConvertDBNullToInt32(obj);
            }
            strSql.Clear();
            strSql.Append("select Co_id,CoName,Parent_id,CoStrcName");
            strSql.Append(" from vp_coinfo where Ledger=@Ledger and Attrib=@Attrib");
            //strSql.Append(" and CoName like @CoName");
            if (!string.IsNullOrEmpty(coName))            
                strSql.Append(" and FIND_IN_SET(Co_id,GetCoChildList(@Ledger,@TopId))");            
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id, CoName = coName + "%", Attrib = CoAttrib.Area, TopId = topId });
        }

        /// <summary>
        /// 获取区域下的机房个数
        /// </summary>
        /// <param name="co_id">区域ID号</param>
        /// <returns></returns>
        public int GetStationCount(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) from vp_coinfo where Ledger=@Ledger and Parent_id=@Parent_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Parent_id = co_id });
            return CommFunc.ConvertDBNullToInt32(obj);
        }
        /// <summary>
        /// 获取区域下的站点个数
        /// </summary>
        /// <param name="co_id">区域ID号</param>
        /// <returns></returns>
        public int GetRoomCount(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select GetCoChildList(@Ledger,@Parent_id)as str");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Parent_id = co_id });
            string childList = CommFunc.ConvertDBNullToString(obj);
            strSql.Clear();
            strSql.Append("select count(*) from vp_coinfo where Ledger=@Ledger and FIND_IN_SET(Co_id,@ChildList) and Co_id!=@Co_id");
            obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id, ChildList = childList });
            return CommFunc.ConvertDBNullToInt32(obj);
        }

        /// <summary>
        /// 设置区域信息
        /// </summary>
        /// <param name="area">区域信息</param>
        /// <returns></returns>
        public int SetArea(AreaVModel area)
        {
            StringBuilder strSql = new StringBuilder();
            int layer = 0;
            strSql.Clear();
            strSql.Append("select count(*) from v1_cust where Ledger=@Ledger and CoName=@CoName and Co_id!=@Co_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, CoName = area.AreaName, Co_id = area.AreaId });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("机房名称:" + area.AreaName + "已存在");

            if (area.Parent_id != 0)
            {
                strSql.Clear();
                strSql.Append("select Layer from v1_cust where Ledger=@Ledger and Co_id=@Co_id");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = area.Parent_id });
                layer = CommFunc.ConvertDBNullToInt32(obj) + 1;
            }
            if (area.AreaId == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Co_id)as Co_id from v1_cust where Ledger=@Ledger");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger });
                area.AreaId = CommFunc.ConvertDBNullToInt32(obj) + 1;
            }
            object params01 = new
            {
                Ledger = this.Ledger,
                Co_id = area.AreaId,
                CoNo = area.AreaId,
                CoName = area.AreaName,
                Parent_id = area.Parent_id,
                CustAddr = area.AreaName,
                Mobile = "",
                Remark = "",
                Disabled = 0,
                Attrib = CoAttrib.Area,
                Layer = layer,
                SysUid = this.SysUid
            };
            ///////////////////////////////////////////////////
            strSql.Clear();
            strSql.Append("insert into v1_cust(Ledger,Co_id,CoNo,CoName,Disabled,Parent_id,Attrib,Layer,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,@Co_id,@CoNo,@CoName,@Disabled,@Parent_id,@Attrib,@Layer,@SysUid,now(),@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE CoNo=@CoNo,CoName=@CoName,Disabled=@Disabled,Update_by=@SysUid,Update_dt=now();");
            strSql.Append("insert into v1_custinfo(Ledger,Co_id,CustAddr,Mobile,Remark,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,@Co_id,@CustAddr,@Mobile,@Remark,@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE CustAddr=@CustAddr,Mobile=@Mobile,Remark=@Remark,Update_by=@SysUid,Update_dt=now();");
            strSql.Append("update v1_custinfo set StrucName=GetCoOnStrucName(Ledger,Co_id) where Ledger=@Ledger and FIND_IN_SET(Co_id,GetCoChildList(Ledger,@Co_id))"); // 更新全名
            int cnt = SQLHelper.Execute(strSql.ToString(), params01);
            return cnt;
        }
        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="co_id"></param>
        /// <returns></returns>
        public int DelCoInfo(int co_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Clear();
            strSql.Append("select GetCoChildList(@Ledger,@Co_id)");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = co_id });
            string splitChildList = CommFunc.ConvertDBNullToString(obj);
            object params01 = new { Ledger = this.Ledger, Co_id = co_id, SplitChildList = splitChildList };
            //
            strSql.Clear();
            strSql.Append("select count(a.Co_id)as cnt");
            strSql.Append(" from v1_cust as a inner join v2_info as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and FIND_IN_SET(a.Co_id,@SplitChildList)");
            obj = SQLHelper.ExecuteScalar(strSql.ToString(), params01);
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("站点下已挂有电表已进行采集数据，不能删除");
            //
            strSql.Clear();
            strSql.Append("update v1_cust as a inner join v1_gateway_esp_meter as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" set b.Co_id=0 where FIND_IN_SET(a.Co_id,@SplitChildList);");
            strSql.Append("delete b,a from v1_cust as a left join v1_custinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" where a.Ledger=@Ledger and FIND_IN_SET(a.Co_id,@SplitChildList);");
            return SQLHelper.Execute(strSql.ToString(), params01);
        }

        /// <summary>
        /// 获取机房列表信息
        /// </summary>
        /// <param name="name">机房名称</param>
        /// <returns></returns>
        public DataTable GetStationList(int stationId, string name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.Co_id,a.CoName,a.CoNo,a.Parent_id,a.Disabled,b.CustAddr,b.Remark,b.Cic_id,c1.CoName as AreaName,c2.CicName");
            strSql.Append(" from vp_coinfo as a left join v1_custinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v1_cust as c1 on a.Ledger=c1.Ledger and a.Parent_id=c1.Co_id");
            strSql.Append(" left join v0_cic as c2 on b.Ledger=c2.Ledger and b.Cic_id=c2.Cic_id");
            strSql.Append(" where a.Ledger=@Ledger and a.CoName like @CoName and a.Attrib=@Attrib");
            if (stationId != 0)
                strSql.Append(" and a.Co_id=@Co_id");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Co_id = stationId, CoName = name + "%", Attrib = CoAttrib.Station });
        }

        /// <summary>
        /// 新增机房信息
        /// </summary>
        /// <param name="station">机房信息</param>
        /// <returns></returns>
        public int SetStation(StationVModel station)
        {
            StringBuilder strSql = new StringBuilder();
            int layer = 0;
            strSql.Clear();
            strSql.Append("select count(*) from v1_cust where Ledger=@Ledger and CoName=@CoName and Co_id!=@Co_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, CoName = station.StationName, Co_id = station.StationId });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("机房名称:" + station.StationName + "已存在");

            if (station.AreaId != 0)
            {
                strSql.Clear();
                strSql.Append("select Layer from v1_cust where Ledger=@Ledger and Co_id=@Co_id");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = station.AreaId });
                layer = CommFunc.ConvertDBNullToInt32(obj) + 1;
            }
            else
            {
                throw new Exception("机房名称:" + station.StationName + "所属区域不正确");
            }
            if (station.StationId == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Co_id)as Co_id from v1_cust where Ledger=@Ledger");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger });
                station.StationId = CommFunc.ConvertDBNullToInt32(obj) + 1;
            }
            object params01 = new
            {
                Ledger = this.Ledger,
                Co_id = station.StationId,
                CoNo = station.StationNo,
                CoName = station.StationName,
                Parent_id = station.AreaId,
                CustAddr = station.Address,
                Remark = station.Remark,
                Disabled = station.Disabled,
                Attrib = CoAttrib.Station,
                Layer = layer,
                Cic_id = station.StationType,
                SysUid = this.SysUid
            };
            ///////////////////////////////////////////////////
            strSql.Clear();
            strSql.Append("insert into v1_cust(Ledger,Co_id,CoNo,CoName,Disabled,Parent_id,Attrib,Layer,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,@Co_id,@CoNo,@CoName,@Disabled,@Parent_id,@Attrib,@Layer,@SysUid,now(),@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE CoNo=@CoNo,CoName=@CoName,Parent_id=@Parent_id,Disabled=@Disabled,Update_by=@SysUid,Update_dt=now();");
            strSql.Append("insert into v1_custinfo(Ledger,Co_id,CustAddr,Cic_id,Remark,Update_by,Update_dt)");
            strSql.Append("values (@Ledger,@Co_id,@CustAddr,@Cic_id,@Remark,@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE CustAddr=@CustAddr,Cic_id=@Cic_id,Remark=@Remark,Update_by=@SysUid,Update_dt=now();");
            strSql.Append("update v1_custinfo set StrucName=GetCoOnStrucName(Ledger,Co_id) where Ledger=@Ledger and FIND_IN_SET(Co_id,GetCoChildList(Ledger,@Co_id))"); // 更新全名
            int cnt = SQLHelper.Execute(strSql.ToString(), params01);
            return cnt;
        }

        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public DataTable GetRoomList(int roomId, string roomName)
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Clear();
            //strSql.Append("select GetCoChildList(@Ledger,@Co_id)");
            //object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = areaId });
            //string splitChildList = CommFunc.ConvertDBNullToString(obj);
            object params01 = new { Ledger = this.Ledger, Co_id = roomId, CoName = roomName + "%", Attrib = CoAttrib.Room };
            strSql.Clear();
            strSql.Append("select a.Co_id,a.CoName,a.CoNo,a.Parent_id,a.Disabled,b.CustAddr,b.Mobile,b.Remark,b.Cic_id,b.CorpId,b.Remark,");
            strSql.Append("m1.CoName as StationName,m2.CicName,m3.CicName as CorpName,b.Switch,b.CoType");
            strSql.Append(" from vp_coinfo as a left join v1_custinfo as b on a.Ledger=b.Ledger and a.Co_id=b.Co_id");
            strSql.Append(" left join v1_cust as m1 on a.Ledger=m1.Ledger and a.Parent_id=m1.Co_id");
            strSql.Append(" left join v0_cic as m2 on a.Ledger=m2.Ledger and b.Cic_id=m2.Cic_id");
            strSql.Append(" left join v0_cic as m3 on a.Ledger=m3.Ledger and b.CorpId=m3.Cic_id");
            strSql.Append(" where a.Ledger=@Ledger and a.CoName like @CoName and a.Attrib=@Attrib");
            if (roomId != 0)
                strSql.Append(" and a.Co_id=@Co_id");
            //strSql.Append(" where a.Ledger=@Ledger and FIND_IN_SET(a.Co_id,@SplitChildList) and a.Attrib=@Attrib");
            return SQLHelper.Query(strSql.ToString(), params01);
        }
        /// <summary>
        /// 设置站点信息
        /// </summary>
        /// <param name="station">站点信息</param>
        /// <returns></returns>
        public int SetRoom(RoomVModel room)
        {
            StringBuilder strSql = new StringBuilder();
            int layer = 0;
            strSql.Clear();
            strSql.Append("select count(*) from v1_cust where Ledger=@Ledger and CoName=@CoName and Co_id!=@Co_id");
            object obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, CoName = room.RoomName, Co_id = room.RoomId });
            if (CommFunc.ConvertDBNullToInt32(obj) > 0)
                throw new Exception("站点名称:" + room.RoomName + "已存在");

            if (room.StationId != 0)
            {
                strSql.Clear();
                strSql.Append("select Layer from v1_cust where Ledger=@Ledger and Co_id=@Co_id");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger, Co_id = room.StationId });
                layer = CommFunc.ConvertDBNullToInt32(obj) + 1;
            }
            else
            {
                throw new Exception("站点名称:" + room.RoomName +"所属机房不正确");
            }

            if (room.RoomId == 0)
            {
                strSql.Clear();
                strSql.Append("select max(Co_id)as Co_id from v1_cust where Ledger=@Ledger");
                obj = SQLHelper.ExecuteScalar(strSql.ToString(), new { Ledger = this.Ledger });
                room.RoomId = CommFunc.ConvertDBNullToInt32(obj) + 1;
            }        
            object params01 = new
            {
                Ledger = this.Ledger,
                Co_id = room.RoomId,
                CoNo = room.RoomNo,
                CoName = room.RoomName,
                Parent_id = room.StationId,
                CustAddr = room.Address,
                Mobile = "",
                Remark = room.Remark,
                Disabled = room.Disabled,
                Cic_id = room.RoomSight,
                Switch = room.Switch,
                CoType = room.RoomType,
                CorpId = 0,
                Attrib = CoAttrib.Room,
                Layer = layer,
                SysUid = this.SysUid
            };
            ///////////////////////////////////////////////////
            strSql.Clear();
            strSql.Append("insert into v1_cust(Ledger,Co_id,CoNo,CoName,Disabled,Parent_id,Attrib,Layer,Create_by,Create_dt,Update_by,Update_dt)");
            strSql.Append("values(@Ledger,@Co_id,@CoNo,@CoName,@Disabled,@Parent_id,@Attrib,@Layer,@SysUid,now(),@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE CoNo=@CoNo,CoName=@CoName,Parent_id=@Parent_id,Disabled=@Disabled,Update_by=@SysUid,Update_dt=now();");
            strSql.Append("insert into v1_custinfo(Ledger,Co_id,CustAddr,Mobile,Remark,Cic_id,CorpId,Switch,CoType,Update_by,Update_dt)");
            strSql.Append("values (@Ledger,@Co_id,@CustAddr,@Mobile,@Remark,@Cic_id,@CorpId,@Switch,@CoType,@SysUid,now())");
            strSql.Append("ON DUPLICATE KEY UPDATE CustAddr=@CustAddr,Mobile=@Mobile,Remark=@Remark,Cic_id=@Cic_id,CorpId=@CorpId,Switch=@Switch,CoType=@CoType,Update_by=@SysUid,Update_dt=now();");
            strSql.Append("update v1_custinfo set StrucName=GetCoOnStrucName(Ledger,Co_id) where Ledger=@Ledger and FIND_IN_SET(Co_id,GetCoChildList(Ledger,@Co_id))"); // 更新全名
            int cnt = SQLHelper.Execute(strSql.ToString(), params01);
            return cnt;
        }

        #region  场景、公司 列表
        /// <summary>
        /// 获取场景列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCicList(CicAttrib cicAttrib)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Cic_id,CicName from v0_cic where Ledger=@Ledger and Attrib=@Attrib");
            return SQLHelper.Query(strSql.ToString(), new { Ledger = this.Ledger, Attrib = (int)cicAttrib });
        }
        #endregion
    }
}
