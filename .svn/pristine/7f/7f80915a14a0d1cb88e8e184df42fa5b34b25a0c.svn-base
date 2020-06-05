using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Opertion.Syscont
{
    public partial class ExpRateNewHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Syscont.ExpRateNewBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpRateNewHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Syscont.ExpRateNewBLL(user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 获取物业收费信息
        /// </summary>
        /// <param name="Descr">筛选条件：描述</param>
        /// <returns></returns>
        public APIRst GetYdRateNewList(string Descr)
        {
            APIRst rst = new APIRst();
            try
            {

                var dt = this.GetYdRateNewList(0, Descr);
                rst.data = dt;
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

        private object GetYdRateNewList(int rate_id, string descr)
        {
            DataTable dtSource = bll.GetYdRateNewList(rate_id, descr);
            var res1 = from s1 in dtSource.AsEnumerable()
                       select new
                       {
                           Rate_id = CommFunc.ConvertDBNullToInt32(s1["Rate_id"]),
                           Descr = CommFunc.ConvertDBNullToString(s1["Descr"]),
                           Unit = CommFunc.ConvertDBNullToString(s1["Unit"]),
                           UnitBase = CommFunc.ConvertDBNullToDecimal(s1["UnitBase"]).ToString("f2"),
                           Rule = CommFunc.ConvertDBNullToInt32(s1["Rule"]),
                           UnitName = CommFunc.GetEnumDisplay(typeof(RateUnit), CommFunc.ConvertDBNullToString(s1["Unit"])),
                           RuleName = CommFunc.GetEnumDisplay(typeof(RateRule), CommFunc.ConvertDBNullToInt32(s1["Rule"])),
                       };
            object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
            return obj;
        }


        /// <summary>
        /// 设置保存物业收费
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="Descr">描述</param>
        /// <param name="Rule">计算规则: 0=正常,1=时间范围,2=数量范围</param>
        /// <param name="Unit">单位: Area=平方米,Bank=户数</param>
        /// <param name="UnitBase">单位基数</param>
        /// <param name="Disabled">是否弃用:0=否,1=是</param>
        /// <returns></returns>
        public APIRst SetSaveYdRateNew(int Rate_id, string Descr, int Rule, string Unit, decimal UnitBase, int Disabled)
        {
            APIRst rst = new APIRst();
            try
            {
                v1_rateVModel rv = new v1_rateVModel();
                rv.Rate_id = Rate_id;
                rv.Descr = Descr;
                rv.Attrib = 1;
                rv.Rule = Rule;
                rv.Unit = Unit;
                rv.UnitBase = UnitBase;
                rv.Disabled = Disabled;

                int cnt = bll.SaveYdRateNew(rv);
                object obj = GetYdRateNewList(rv.Rate_id, "");
                rst.data = obj;
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
        /// 删除费率信息
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <returns></returns>
        public APIRst GetDelYdRateNew(int Rate_id)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Rate_id == 0)
                    throw new Exception("费率ID号不能为空");
                int cc = bll.GetDelYdRateNew(Rate_id);
                rst.data = cc;
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
        /// 获取物业收费-单价详情
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <returns></returns>
        public APIRst GetYdRateNewCs(int Rate_id)
        {
            APIRst rst = new APIRst();
            try
            {

                DataTable dtSource = bll.GetYdRateNewCs(Rate_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               CsId = CommFunc.ConvertDBNullToInt32(s1["CsId"]),
                               Price = Math.Round(CommFunc.ConvertDBNullToDecimal(s1["Price"]), 2, MidpointRounding.AwayFromZero),
                               PStart = CommFunc.ConvertDBNullToString(s1["PStart"]),
                               PEnd = CommFunc.ConvertDBNullToString(s1["PEnd"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取物业收费-单价详情错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 设置保存物业收费
        /// </summary>
        /// <param name="Rule">计算规则: 0=正常,1=时间范围,2=数量范围</param>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="CsId">单价ID号</param>
        /// <param name="Price">单价</param>
        /// <param name="dtPStart">开始区间1</param>
        /// <param name="dtPEnd">结束区间1</param>
        /// <param name="nPStart">开始区间2</param>
        /// <param name="nPEnd">结束区间2</param>
        /// <returns></returns>
        public APIRst SetSaveYdRateNewCs(int Rule, int Rate_id, int CsId, decimal Price, DateTime dtPStart, DateTime dtPEnd, decimal nPStart, decimal nPEnd)
        {
            APIRst rst = new APIRst();
            try
            {
                string pPStart = "", pPEnd = "";
                if (Rate_id == 0)
                    throw new Exception("费率ID号不能为空");
                if (Price == 0)
                    throw new Exception("单价为零");
                if (Rule == 1)
                {
                    if (dtPStart.Year <= 2000)
                        throw new Exception("开始日期错误");
                    if (dtPEnd.Year <= 2000)
                        throw new Exception("结束日期错误");
                    if (dtPStart > dtPEnd)
                        throw new Exception("开始日期大于结束日期");
                    pPStart = dtPStart.ToString("yyyy-MM-dd");
                    pPEnd = dtPEnd.ToString("yyyy-MM-dd");
                }
                else if (Rule == 2)
                {
                    if (nPStart > nPEnd)
                        throw new Exception("开始数量大于结束数量");
                    pPStart = nPStart.ToString("f2");
                    pPEnd = nPEnd.ToString("f2");
                }

                int cnt = bll.SetSaveYdRateNewCs(Rate_id, CsId, Price, pPStart, pPEnd);
                //object obj = GetYdRateNewList(rv.Rate_id, "");
                rst.data = cnt;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("保存明细列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除物业收费-单价详情
        /// </summary>
        /// <param name="Rate_id">费率ID号</param>
        /// <param name="csId">单价ID号</param>
        /// <returns></returns>
        public APIRst GetDelYdRateCs(int Rate_id, int csId)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Rate_id == 0)
                    throw new Exception("费率ID号不能为空");
                if (csId == 0)
                    throw new Exception("单价ID号不能为空");
                int cc = bll.GetDelYdRateCs(Rate_id, csId);
                rst.data = cc;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除物业收费-单价详情:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


    }
}