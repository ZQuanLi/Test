using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using System.Web.Mvc;
using System.Text;
using YDS6000.Models.Tables;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Collections;

namespace YDS6000.WebApi.Areas.Exp.Opertion.Syscont
{
    public partial class ExpTimingHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Syscont.ExpTimingBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpTimingHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Syscont.ExpTimingBLL(user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 获取定时断送电策略
        /// </summary>
        /// <param name="psi_id"></param>
        /// <param name="Descr">筛选条件：费率描述</param>
        /// <returns></returns>
        public APIRst GetYdTiming(int psi_id, string Descr)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdTiming(psi_id, Descr);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               si_id = CommFunc.ConvertDBNullToInt32(s1["si_id"]),//策略id号
                               descr = CommFunc.ConvertDBNullToString(s1["descr"]),//策略描述
                               siSSR = CommFunc.ConvertDBNullToString(s1["siSSR"]),//断送电日期时间设置
                               md = CommFunc.ConvertDBNullToString(s1["md"]),//日期范围断送电设置
                               wk = CommFunc.ConvertDBNullToString(s1["wk"]),//星期断送电设置
                               ts = CommFunc.ConvertDBNullToString(s1["ts"]),//特殊日期断送电设置
                               disabled = CommFunc.ConvertDBNullToString(s1["disabled"]),//是否启用
                               disabledS = CommFunc.ConvertDBNullToString(s1["disabledS"]),//是否启用
                               update_by = CommFunc.ConvertDBNullToString(s1["update_by"]),//更新人
                               update_dt = CommFunc.ConvertDBNullToString(s1["update_dt"])
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取定时断送电策略错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取新增页面内的时段设置的表格
        /// </summary>
        /// <param name="nAct">1=增加,2=修改,3=删除</param>
        /// <param name="Psi_id"></param>
        /// <returns></returns>
        public APIRst GetsiSSR(int nAct, int Psi_id)
        {
            APIRst rst = new APIRst();
            string json = "";
            object obj = null;
            try
            {
                if (nAct == 1)
                {
                    DataTable dtSource = bll.GetTable(json);
                    int total = dtSource.Rows.Count;
                    var res1 = from s1 in dtSource.AsEnumerable()
                               select new
                               {
                                   t00 = CommFunc.ConvertDBNullToString(s1["t00"]),
                                   d01 = CommFunc.ConvertDBNullToString(s1["d01"]),
                                   d02 = CommFunc.ConvertDBNullToString(s1["d02"]),
                                   d03 = CommFunc.ConvertDBNullToString(s1["d03"]),
                                   d04 = CommFunc.ConvertDBNullToString(s1["d04"]),
                                   d05 = CommFunc.ConvertDBNullToString(s1["d05"]),
                                   d06 = CommFunc.ConvertDBNullToString(s1["d06"]),
                                   d07 = CommFunc.ConvertDBNullToString(s1["d07"]),
                                   d08 = CommFunc.ConvertDBNullToString(s1["d08"])
                               };
                    obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                }
                else
                {
                    if (Psi_id == 0)
                        throw new Exception("Psi_id不能为空");
                    DataTable dtSource = bll.GetYdm_si_ssr(Psi_id, "");
                    if (dtSource.Rows.Count > 0)
                        json = CommFunc.ConvertDBNullToString(dtSource.Rows[0]["siSSR"]);
                    DataTable dt = bll.GetTable(json);
                    int total = dt.Rows.Count;
                    var res1 = from s1 in dt.AsEnumerable()
                               select new
                               {
                                   t00 = CommFunc.ConvertDBNullToString(s1["t00"]),
                                   d01 = CommFunc.ConvertDBNullToString(s1["d01"]),
                                   d02 = CommFunc.ConvertDBNullToString(s1["d02"]),
                                   d03 = CommFunc.ConvertDBNullToString(s1["d03"]),
                                   d04 = CommFunc.ConvertDBNullToString(s1["d04"]),
                                   d05 = CommFunc.ConvertDBNullToString(s1["d05"]),
                                   d06 = CommFunc.ConvertDBNullToString(s1["d06"]),
                                   d07 = CommFunc.ConvertDBNullToString(s1["d07"]),
                                   d08 = CommFunc.ConvertDBNullToString(s1["d08"])
                               };
                    obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                }

                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取新增页面内的时段设置的表格错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 获取电表号列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetListM_Num()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = new DataTable();
                dtSource.Columns.Add("id");
                dtSource.Columns.Add("value");
                for (int i = 0; i < 9; i++)
                {
                    DataRow dr = dtSource.NewRow();
                    dr["id"] = i.ToString().PadLeft(2, '0');
                    dr["value"] = i.ToString().PadLeft(2, '0');
                    dtSource.Rows.Add(dr);
                }
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToInt32(s1["id"]),
                               Descr = CommFunc.ConvertDBNullToString(s1["value"]),
                           };
                string a = JsonHelper.Serialize(res1.ToList());
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取电表号列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取时间列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetListHourNum()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = new DataTable();
                dtSource.Columns.Add("id");
                dtSource.Columns.Add("value");
                for (int i = 0; i < 24; i++)
                {
                    DataRow dr = dtSource.NewRow();
                    dr["id"] = i.ToString().PadLeft(2, '0');
                    dr["value"] = i.ToString().PadLeft(2, '0');
                    dtSource.Rows.Add(dr);
                }
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToInt32(s1["id"]),
                               Descr = CommFunc.ConvertDBNullToString(s1["value"]),
                           };
                string a = JsonHelper.Serialize(res1.ToList());
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取时间列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取分钟列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetListSecondNum()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = new DataTable();
                dtSource.Columns.Add("id");
                dtSource.Columns.Add("value");
                for (int i = 0; i < 60; i++)
                {
                    DataRow dr = dtSource.NewRow();
                    dr["id"] = i.ToString().PadLeft(2, '0');
                    dr["value"] = i.ToString().PadLeft(2, '0');
                    dtSource.Rows.Add(dr);
                }
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Id = CommFunc.ConvertDBNullToInt32(s1["id"]),
                               Descr = CommFunc.ConvertDBNullToString(s1["value"]),
                           };
                string a = JsonHelper.Serialize(res1.ToList());
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取费率列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 设置定时电表策略
        /// </summary>
        /// <param name="nAct">1=增加,2=修改,3=删除</param>
        /// <param name="si_id">策略Id,修改和删除需要,新增时传0即可</param>
        /// <param name="pRemark">描述</param>
        /// <param name="pIsClosed">是否启用:1=启用,0=不启用</param>
        /// <param name="pcT">日时区策略</param>
        /// <param name="pcM">日时区策略-定时段表号</param>
        /// <param name="pcW">星期策略</param>
        /// <param name="pcD">特殊日期策略</param>
        /// <param name="pcB">特殊日期策略-定时段表号</param>
        /// <param name="Timing"></param>
        /// <returns></returns>
        public APIRst SetTiming(int nAct, int si_id, string pRemark, int pIsClosed, string pcT, string pcM, string pcW, string pcD, string pcB, DataModels Timing)
        {
            APIRst rst = new APIRst();
            string pcT1, pcT2, pcT3, pcT4, pcT5, pcT6, pcT7, pcM1, pcM2, pcM3, pcM4, pcM5, pcM6, pcM7, pcW1, pcW2, pcW3, pcW4, pcW5, pcW6, pcW7, pcD1, pcD2, pcD3, pcD4, pcD5, pcD6, pcD7, pcB1, pcB2, pcB3, pcB4, pcB5, pcB6, pcB7 = null;
            try
            {
                String[] pcTss = pcT.Split(','); //日时区策略
                pcT1 = pcTss[0];
                pcT2 = pcTss[1];
                pcT3 = pcTss[2];
                pcT4 = pcTss[3];
                pcT5 = pcTss[4];
                pcT6 = pcTss[5];
                pcT7 = pcTss[6];

                String[] pcMss = pcM.Split(',');  //日时区策略-定时段表号
                pcM1 = pcMss[0];
                pcM2 = pcMss[1];
                pcM3 = pcMss[2];
                pcM4 = pcMss[3];
                pcM5 = pcMss[4];
                pcM6 = pcMss[5];
                pcM7 = pcMss[6];

                String[] pcWss = pcW.Split(','); //星期策略
                pcW1 = pcWss[0];
                pcW2 = pcWss[1];
                pcW3 = pcWss[2];
                pcW4 = pcWss[3];
                pcW5 = pcWss[4];
                pcW6 = pcWss[5];
                pcW7 = pcWss[6];

                String[] pcDWss = pcD.Split(',');  //特殊日期策略
                pcD1 = pcDWss[0];
                pcD2 = pcDWss[1];
                pcD3 = pcDWss[2];
                pcD4 = pcDWss[3];
                pcD5 = pcDWss[4];
                pcD6 = pcDWss[5];
                pcD7 = pcDWss[6];

                String[] pcBWss = pcB.Split(',');  //特殊日期策略-定时段表号
                pcB1 = pcBWss[0];
                pcB2 = pcBWss[1];
                pcB3 = pcBWss[2];
                pcB4 = pcBWss[3];
                pcB5 = pcBWss[4];
                pcB6 = pcBWss[5];
                pcB7 = pcBWss[6];


                v1_si_ssrVModel si_ssr = new v1_si_ssrVModel();
                if (nAct != 1 && nAct != 2 && nAct != 3)
                    throw new Exception("执行操作类型错误");
                JsonSiModel si = ModelHandler<JsonSiModel>.jsonTextToModel(CommFunc.ConvertDBNullToString(si_ssr.siSSR));
                JsonSiMdModel md = ModelHandler<JsonSiMdModel>.jsonTextToModel(CommFunc.ConvertDBNullToString(si_ssr.md));
                JsonSiWkModel wk = ModelHandler<JsonSiWkModel>.jsonTextToModel(CommFunc.ConvertDBNullToString(si_ssr.wk));
                JsonSiTsModel ts = ModelHandler<JsonSiTsModel>.jsonTextToModel(CommFunc.ConvertDBNullToString(si_ssr.ts));

                si_ssr.descr = pRemark;//描述
                si_ssr.disabled = pIsClosed;//是否启用
                ////----8个时段的JSON值怎样获取----////
                //string data1 = pData1;
                DataTable djson = ToDataTable(Timing.Data);

                //var djson = CreateTable<ExpTimingModels>();
                //djson = FillData(djson, Timing);

                StringBuilder strD = new StringBuilder();
                StringBuilder strT = new StringBuilder();
                JsonSiModel.Value value;
                int rIndex = 0;
                string[] arrValue;
                object dObj;
                foreach (DataRow dr in djson.Rows)
                {
                    rIndex = rIndex + 1;
                    foreach (DataColumn dc in djson.Columns)
                    {
                        int cIndex = CommFunc.ConvertDBNullToInt32(dc.ColumnName.Trim().Substring(1, 2));
                        if (cIndex == 0) continue;
                        //arrValue = CommFunc.ConvertDBNullToString(cell.Value).Trim().Split(new char[2] { ':', '-' }, StringSplitOptions.RemoveEmptyEntries);
                        arrValue = CommFunc.ConvertDBNullToString(dr[dc]).Trim().Split('-');
                        strD.Clear();
                        strT.Clear();
                        strD.Append("d" + cIndex.ToString().Trim().PadLeft(2, '0'));
                        strT.Append("t" + rIndex.ToString().Trim().PadLeft(2, '0'));
                        System.Reflection.PropertyInfo dInfo = si.GetType().GetProperty(strD.ToString());
                        dObj = dInfo.GetValue(si, null);
                        System.Reflection.PropertyInfo tInfo = dObj.GetType().GetProperty(strT.ToString());
                        value = (JsonSiModel.Value)tInfo.GetValue(dObj, null);
                        value.hm = arrValue[0];
                        value.sr = arrValue[1];
                        //value.sr = arrValue[2];
                    }
                }

                /////
                #region
                md.md01.md = "0000";
                md.md02.md = "0000";
                md.md03.md = "0000";
                md.md04.md = "0000";
                md.md05.md = "0000";
                md.md06.md = "0000";
                md.md07.md = "0000";
                string ss;
                string a;
                a = pcT1;
                if (a.Length > 4 || a.Length < 4)
                {
                    ss = (CommFunc.ConvertDBNullToDateTime(pcT1)).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (ss != "19000101" && ss != "00010101")
                        md.md01.md = ss.Substring(4, 4);
                }
                else
                    md.md01.md = a;
                a = pcT2;
                if (a.Length > 4 || a.Length < 4)
                {
                    ss = (CommFunc.ConvertDBNullToDateTime(pcT2)).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (ss != "19000101" && ss != "00010101")
                        md.md02.md = ss.Substring(4, 4);
                }
                else
                    md.md02.md = a;
                a = pcT3;
                if (a.Length > 4 || a.Length < 4)
                {
                    ss = (CommFunc.ConvertDBNullToDateTime(pcT3)).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (ss != "19000101" && ss != "00010101")
                        md.md03.md = ss.Substring(4, 4);
                }
                else
                    md.md03.md = a;
                a = pcT4;
                if (a.Length > 4 || a.Length < 4)
                {
                    ss = (CommFunc.ConvertDBNullToDateTime(pcT4)).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (ss != "19000101" && ss != "00010101")
                        md.md04.md = ss.Substring(4, 4);
                }
                else
                    md.md04.md = a;
                a = pcT5;
                if (a.Length > 4 || a.Length < 4)
                {
                    ss = (CommFunc.ConvertDBNullToDateTime(pcT5)).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (ss != "19000101" && ss != "00010101")
                        md.md05.md = ss.Substring(4, 4);
                }
                else
                    md.md05.md = a;
                a = pcT6;
                if (a.Length > 4 || a.Length < 4)
                {
                    ss = (CommFunc.ConvertDBNullToDateTime(pcT6)).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (ss != "19000101" && ss != "00010101")
                        md.md06.md = ss.Substring(4, 4);
                }
                else
                    md.md06.md = a;
                a = pcT7;
                if (a.Length > 4 || a.Length < 4)
                {
                    ss = (CommFunc.ConvertDBNullToDateTime(pcT7)).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (ss != "19000101" && ss != "00010101")
                        md.md07.md = ss.Substring(4, 4);
                }
                else
                    md.md07.md = a;
                ////
                md.md01.si = pcM1;
                md.md02.si = pcM2;
                md.md03.si = pcM3;
                md.md04.si = pcM4;
                md.md05.si = pcM5;
                md.md06.si = pcM6;
                md.md07.si = pcM7;
                /////

                wk.si01 = pcW1;
                wk.si02 = pcW2;
                wk.si03 = pcW3;
                wk.si04 = pcW4;
                wk.si05 = pcW5;
                wk.si06 = pcW6;
                wk.si07 = pcW7;
                /////
                ts.ts01.dt = "00000000";
                ts.ts02.dt = "00000000";
                ts.ts03.dt = "00000000";
                ts.ts04.dt = "00000000";
                ts.ts05.dt = "00000000";
                ts.ts06.dt = "00000000";
                ts.ts07.dt = "00000000";
                string sd = "";
                string st = "";
                st = pcD1;
                if (st.Contains("-"))
                {
                    sd = (CommFunc.ConvertDBNullToDateTime(pcD1)).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (sd != "19000101" && sd != "00010101")
                        ts.ts01.dt = sd;
                }
                else if (st != "")
                    ts.ts01.dt = st;
                st = pcD2;
                if (st.Contains("-"))
                {
                    sd = (CommFunc.ConvertDBNullToDateTime(pcD2)).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (sd != "19000101" && sd != "00010101")
                        ts.ts02.dt = sd;
                }
                else if (st != "")
                    ts.ts02.dt = st;
                st = pcD3;
                if (st.Contains("-"))
                {
                    sd = (CommFunc.ConvertDBNullToDateTime(pcD3)).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (sd != "19000101" && sd != "00010101")
                        ts.ts03.dt = sd;
                }
                else if (st != "")
                    ts.ts03.dt = st;
                st = pcD4;
                if (st.Contains("-"))
                {
                    sd = (CommFunc.ConvertDBNullToDateTime(pcD4)).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (sd != "19000101" && sd != "00010101")
                        ts.ts04.dt = sd;
                }
                else if (st != "")
                    ts.ts04.dt = st;
                st = pcD5;
                if (st.Contains("-"))
                {
                    sd = (CommFunc.ConvertDBNullToDateTime(pcD5)).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (sd != "19000101" && sd != "00010101")
                        ts.ts05.dt = sd;
                }
                else if (st != "")
                    ts.ts05.dt = st;
                st = pcD6;
                if (st.Contains("-"))
                {
                    sd = (CommFunc.ConvertDBNullToDateTime(pcD6)).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (sd != "19000101" && sd != "00010101")
                        ts.ts06.dt = sd;
                }
                else if (st != "")
                    ts.ts06.dt = st;
                st = pcD7;
                if (st.Contains("-"))
                {
                    sd = (CommFunc.ConvertDBNullToDateTime(pcD7)).ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    if (sd != "19000101" && sd != "00010101")
                        ts.ts07.dt = sd;
                }
                else if (st != "")
                    ts.ts07.dt = st;
                ts.ts01.si = pcB1;
                ts.ts02.si = pcB2;
                ts.ts03.si = pcB3;
                ts.ts04.si = pcB4;
                ts.ts05.si = pcB5;
                ts.ts06.si = pcB6;
                ts.ts07.si = pcB7;
                #endregion
                /////////////////////////////////////////////////////////////////////////////////////
                si_ssr.siSSR = ModelHandler<JsonSiModel>.modelToJsonText(si);
                si_ssr.md = ModelHandler<JsonSiMdModel>.modelToJsonText(md);
                si_ssr.wk = ModelHandler<JsonSiWkModel>.modelToJsonText(wk);
                si_ssr.ts = ModelHandler<JsonSiTsModel>.modelToJsonText(ts);
                int cnt = 0;
                if (nAct != 1 && Convert.IsDBNull(si_ssr.si_id))
                    throw new Exception("请选中一行");
                if (nAct == 1)//新增
                {
                    cnt = bll.EditRow(si_ssr);
                }
                if (nAct == 2 || nAct == 3)
                {
                    si_ssr.si_id = si_id;//策略ID
                    if (nAct == 3)
                        bll.DelRow(si_ssr);//删除
                    else
                        bll.EditRow(si_ssr);
                }

                rst.data = cnt;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取费率列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        ///// <summary>
        ///// 将实体集合转换为DataTable
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="entities">实体集合</param>
        //public static DataTable ToDataTable<T>(List<T> entities)
        //{
        //    var result = CreateTable<T>();
        //    FillData(result, entities);
        //    return result;
        //}

        /// <summary>
        /// 创建表
        /// </summary>
        private static DataTable CreateTable<T>()
        {
            var result = new DataTable();
            var type = typeof(T);
            foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                var propertyType = property.PropertyType;
                if ((propertyType.IsGenericType) && (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    propertyType = propertyType.GetGenericArguments()[0];
                result.Columns.Add(property.Name, propertyType);
            }
            return result;
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        private DataTable FillData<T>(DataTable dt, IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                dt.Rows.Add(CreateRow(dt, entity));
            }
            return dt;
        }

        /// <summary>
        /// 创建行
        /// </summary>
        private DataRow CreateRow<T>(DataTable dt, T entity)
        {
            DataRow row = dt.NewRow();
            var type = typeof(T);
            foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                row[property.Name] = property.GetValue(entity) ?? DBNull.Value;
            }
            return row;
        }


        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            if (dictionary[current].ToString().ToLower().Equals("null"))
                                dataRow[current] = "0";
                            else
                                dataRow[current] = dictionary[current];
                        }
                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }


    }
}