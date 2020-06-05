using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations;
using YDS6000.Models;
using YDS6000.DAL.BaseInfo;

namespace YDS6000.BLL.BaseInfo
{
    partial class BaseInfoBLL
    {
        #region 区域信息
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <param name="co_id">区域ID号</param>
        /// <returns></returns>
        public DataTable GetAreaList(int co_id,string coName)
        {
            return dal.GetAreaList(co_id, coName);
        }

        /// <summary>
        /// 获取区域下的机房个数
        /// </summary>
        /// <param name="co_id">区域ID号</param>
        /// <returns></returns>
        public int GetStationCount(int co_id)
        {
            return dal.GetStationCount(co_id);
        }
        /// <summary>
        /// 获取区域下的站点个数
        /// </summary>
        /// <param name="co_id">区域ID号</param>
        /// <returns></returns>
        public int GetRoomCount(int co_id)
        {
            return dal.GetRoomCount(co_id);
        }
        /// <summary>
        /// 设置区域信息
        /// </summary>
        /// <param name="area">区域信息</param>
        /// <returns></returns>
        public int SetArea(AreaVModel area)
        {
            return dal.SetArea(area);
        }
        /// <summary>
        /// 删除区域信息
        /// </summary>
        /// <param name="co_id">区域ID号</param>
        /// <returns></returns>
        public int DelCoInfo(int co_id)
        {
            return dal.DelCoInfo(co_id);
        }
        #endregion
        #region 机房信息
        /// <summary>
        /// 获取机房列表信息
        /// </summary>
        /// <param name="name">机房名称</param>
        /// <returns></returns>
        public DataTable GetStationList(int stationId,string name)
        {
            return dal.GetStationList(stationId,name);
        }

        /// <summary>
        /// 新增机房信息
        /// </summary>
        /// <param name="station">机房信息</param>
        /// <returns></returns>
        public int SetStation(StationVModel station)
        {
            return dal.SetStation(station);
        }

        #endregion
        #region 站点信息
        /// <summary>
        /// 获取站点列表信息
        /// </summary>
        /// <param name="areaId">站点ID号</param>
        /// <returns></returns>
        public DataTable GetRoomList(int roomId, string roomName)
        {
            return dal.GetRoomList(roomId, roomName);
        }

        /// <summary>
        /// 站点供电方式列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetSelectSwitch(string type)
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("Id", typeof(System.String));
            dtSource.Columns.Add("Text", typeof(System.String));

            System.Reflection.FieldInfo[] fields = typeof(Switch).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (System.Reflection.FieldInfo field in fields)
            {
                if (!field.Name.ToLower().StartsWith(type.ToLower())) continue;
                //if (!field.Name.ToLower().Contains(type.ToLower())) continue;

                Switch aa = (Switch)Enum.Parse(typeof(Switch), field.Name);
                var obj = field.GetCustomAttributes(typeof(DisplayAttribute), false);
                if (obj != null && obj.Count() != 0)
                {
                    DisplayAttribute md = obj[0] as DisplayAttribute;
                    dtSource.Rows.Add(new object[] { aa.ToString(), md.Name });
                }
            }
            return dtSource;
        }

        /// <summary>
        /// 站点类型列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetSelectRoomType()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add("Id", typeof(System.String));
            dtSource.Columns.Add("Text", typeof(System.String));

            System.Reflection.FieldInfo[] fields = typeof(CoType).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (System.Reflection.FieldInfo field in fields)
            {
                if (!field.Name.Contains("Rm")) continue;
                CoType aa = (CoType)Enum.Parse(typeof(CoType), field.Name);
                var obj = field.GetCustomAttributes(typeof(DisplayAttribute), false);
                if (obj != null && obj.Count() != 0)
                {
                    DisplayAttribute md = obj[0] as DisplayAttribute;
                    dtSource.Rows.Add(new object[] { aa.ToString(), md.Name });
                }
            }
            return dtSource;
        }

        /// <summary>
        /// 设置站点信息
        /// </summary>
        /// <param name="room">站点信息</param>
        /// <returns></returns>
        public int SetRoom(RoomVModel room)
        {
            return dal.SetRoom(room);
        }
        #endregion

        #region  场景、公司、机房类型 列表

        /// <summary>
        /// 获取场景列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCicList(CicAttrib cicAttrib)
        {
            return dal.GetCicList(cicAttrib);
        }
        #endregion
    }
}
