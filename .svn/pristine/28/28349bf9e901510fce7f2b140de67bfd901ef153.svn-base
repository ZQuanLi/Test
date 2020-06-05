using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Energy.Controllers
{
    public partial class HomeHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Energy.Home.HomeBLL bll = null;
        public HomeHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Energy.Home.HomeBLL(user.Ledger, user.Uid);
        }

        /// <summary>
        /// 获取系统的信息
        /// </summary>     
        /// <returns></returns>
        public APIRst GetSysInfo()
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetSysInfo();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取系统的信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        ///获取项目信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetProjectInfo()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetProjectInfo();
                ProVModel pm = new ProVModel();
                if (dtSource.Rows.Count > 0)
                {
                    pm = new ProVModel() {
                        Id = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Co_id"]),
                        ProName = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["CoName"]),
                        Disabled = CommFunc.ConvertDBNullToInt32(dtSource.Rows[0]["Disabled"]),
                        Person = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["CustName"]),
                        ProAddr = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["CustAddr"]),
                        TelNo = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["Mobile"]),
                        Area = CommFunc.ConvertDBNullToDecimal(dtSource.Rows[0]["Area"]),
                        Remark = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["Remark"]),
                    };
                }
                rst.data = pm;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取获取能源分类列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        private static Dictionary<int, dynamic> alarm = new Dictionary<int, dynamic>();
        private static DateTime ss = new DateTime(1900, 1, 1);

        /// <summary>
        /// 获取弹窗告警功能
        /// </summary>
        /// <returns></returns>
        public APIRst GetAlarmInfo()
        {
            APIRst rst = new APIRst();
            try
            {
                List<object> list = new List<object>();
                if (DateTime.Now > ss.AddMinutes(2))
                {
                    DataTable dtSource = bll.GetAlarmInfo();
                    foreach (DataRow dr in dtSource.Rows)
                    {
                        int mid = CommFunc.ConvertDBNullToInt32(dr["Module_id"]);
                        dynamic obj = null;
                        if (alarm.TryGetValue(mid, out obj) == false)
                        {
                            obj = new
                            {
                                BuildName = CommFunc.ConvertDBNullToString(dr["CoName"]),
                                ModuleName = CommFunc.ConvertDBNullToString(dr["ModuleName"]),
                                Content = CommFunc.ConvertDBNullToString(dr["Content"]),
                                CollectTime = new DateTime(1900, 1, 1),
                            };
                            alarm.Add(mid, obj);
                        }
                        DateTime collectTime = CommFunc.ConvertDBNullToDateTime(obj.CollectTime);
                        if (collectTime != CommFunc.ConvertDBNullToDateTime(dr["CollectTime"]))
                        {
                            obj = new
                            {
                                BuildName = CommFunc.ConvertDBNullToString(dr["CoName"]),
                                ModuleName = CommFunc.ConvertDBNullToString(dr["ModuleName"]),
                                Content = CommFunc.ConvertDBNullToString(dr["Content"]),
                                CollectTime = CommFunc.ConvertDBNullToString(dr["CollectTime"]),
                            };
                            alarm[mid] = obj;
                            list.Add(obj);
                        }
                    }
                    ss = DateTime.Now;
                }              
                rst.data = list;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取获取能源分类列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取能源分类列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetEnergyList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetEnergyList();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToInt32(s1["Id"]),
                               Text = CommFunc.ConvertDBNullToString(s1["Text"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取获取能源分类列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取当日用能趋势
        /// </summary>
        /// <param name="isDefine">分类ID号</param>
        /// <returns></returns>
        public APIRst GetDayForChart(int isDefine)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetDayForChart(isDefine);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取获取能源分类列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取当日用能趋势(配电房)
        /// 第一个是当天的小时用能
        /// 第二个是昨日的小时用能
        /// 第三个当天的用能总和
        /// 最后是同期比
        /// </summary>
        /// <param name="co_id">配电房ID号</param>
        /// <returns></returns>
        public APIRst GetDayForBuild(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetDayForBuild(co_id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取配电房用电的列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取建筑当日用能前10名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public APIRst GetDayForTopLine(int id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetDayForTopLine(id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取建筑当日用能前10名错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取分类用能数据
        /// </summary>     
        /// <returns></returns>
        public APIRst GetClassification()
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetClassification();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取分类用能数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取分项用能数据
        /// </summary>     
        /// <returns></returns>
        public APIRst GetDayForEnergy(int isDefine)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetDayForEnergy(isDefine);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取分项用能数据错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取支路树形数据
        /// </summary>     
        /// <returns></returns>
        public APIRst GetTree()
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetTree();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取支路树形数据:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}