using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.PDU.Opertion.Home
{
    public partial class HomeHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.PDU.Home.HomeBLL bll = null;
        public HomeHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.PDU.Home.HomeBLL(user.Ledger, user.Uid);
        }

        /// <summary>
        /// 获取PDU列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetPduList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetPduList();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               Text = CommFunc.ConvertDBNullToString(s1["CoName"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取PDU列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        #region 获取PDU运行状态信息
        /// <summary>
        /// 获取PDU运行状态信息
        /// </summary>
        /// <param name="id">Pdu ID号</param>
        /// <returns></returns>
        public APIRst GetPduStatus(int id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetPduStatus(id);
                object status = this.GetTagName(dtSource.Select("IsDefine=0 and FunType='Ssr'"), 1, "状态");/*状态*/
                object wd = this.GetTagName(dtSource.Select("IsDefine=10"), 3, "温度传感器");/*温度*/
                object sd = this.GetTagName(dtSource.Select("IsDefine=20"), 2, "湿度传感器");/*湿度*/
                object yg = this.GetTagName(dtSource.Select("IsDefine=30"), 2, "传感器接口");/*烟感*/
                object mk = this.GetTagName(dtSource.Select("IsDefine=40"), 4, "开关输入");/*门控*/
                rst.data = new { status = status, wd = wd, sd = sd, yg = yg, mk = mk };
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取PDU运行状态信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private object GetTagName(DataRow[] arr, int count, string descr)
        {
            List<object> list = new List<object>();
            object obj = new { name = descr, tag = "" };
            string unit = "";
            foreach (DataRow dr in arr)
            {
                string name = CommFunc.ConvertDBNullToString(dr["ModuleName"]);
                unit = CommFunc.ConvertDBNullToString("Unit");
                obj = new { name = CommFunc.ConvertDBNullToString(dr["ModuleName"]), tag = CommFunc.ConvertDBNullToString(dr["LpszDbVarName"]) , Unit = unit };
                list.Add(obj);
                if (list.Count == count) break;
            }
            int cc = list.Count;
            while (cc < count)
                list.Add(new { name = descr + (++cc).ToString().PadLeft(2, '0'), tag = "", Unit = unit });
            //if (count == 1)
            //    return obj;
            return list;
        }
        #endregion

        /// <summary>
        /// 获取PDU采集点曲线信息
        /// </summary>
        /// <param name="id">Pdu ID号</param>
        /// <param name="funTypes">采集点</param>
        /// <returns></returns>
        public APIRst GetPduFunTypes(int id, string funTypes)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetPduFunTypes(id, funTypes);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取PDU采集点曲线信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取PDU电能信息
        /// </summary>
        /// <param name="id">Pdu ID号</param>
        /// <returns></returns>
        public APIRst GetPduEnergy(int id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetPduEnergy(id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取PDU电能信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 获取全部告警数据
        /// </summary>
        /// <returns></returns>
        public APIRst GetPduAlarm(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetPduAlarm(co_id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取全部告警数据:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}