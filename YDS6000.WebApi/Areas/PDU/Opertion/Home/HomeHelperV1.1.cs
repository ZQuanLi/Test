using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.PDU.Opertion.Home
{
    partial class HomeHelper
    {
        /// <summary>
        /// PDU地图信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetPduMap()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetPduMap();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Addr = CommFunc.ConvertDBNullToString(s1["CustAddr"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取PDU地图信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        #region PDU 列表
        public APIRst GetPduTree()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetPduTree();
                int total = dtSource.Rows.Count;
                List<Treeview> dt = this.GetPduTree(dtSource);
                object obj = new { total = total, rows = dt };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取组织树形结构错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private List<Treeview> GetPduTree(DataTable dtSource)
        {
            List<Treeview> rst = new List<Treeview>();
            DataRow[] pArr = dtSource.Select("Parent_id=0", "Co_id");
            foreach (DataRow dr in pArr)
            {
                Treeview pTr = new Treeview();
                pTr.id = CommFunc.ConvertDBNullToString(dr["Co_id"]);
                pTr.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                //pTr.attributes = CommFunc.ConvertDBNullToInt32(dr["power"]);
                pTr.nodes = new List<Treeview>();
                this.GetPduTree(ref dtSource, ref pTr, CommFunc.ConvertDBNullToString(dr["Co_id"]));
                rst.Add(pTr);
            }
            return rst;
        }
        private void GetPduTree(ref DataTable dtSource, ref Treeview pTr, string Co_id)
        {
            DataRow[] pArr = dtSource.Select("Parent_id=" + Co_id, "Co_id");
            int pRows = pArr.Count();
            pTr.attributes = new { lastLevel = 0 };
            if (pRows > 0)
                pTr.nodes = new List<Treeview>();
            else
                pTr.attributes = new { lastLevel = 1 };
            foreach (DataRow dr in pArr)
            {
                Treeview cTr = new Treeview();
                cTr.id = CommFunc.ConvertDBNullToString(dr["Co_id"]);
                cTr.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                //cTr.attributes = CommFunc.ConvertDBNullToInt32(dr["power"]);
                pTr.nodes.Add(cTr);
                this.GetPduTree(ref dtSource, ref cTr, CommFunc.ConvertDBNullToString(dr["Co_id"]));
            }
        }

        public APIRst GetPduTree200(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetPduTree(co_id);
                int total = dtSource.Rows.Count;
                List<Treeview> dt = this.GetPduTree200(dtSource);
                object obj = new { total = total, rows = dt };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取组织树形结构错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        private List<Treeview> GetPduTree200(DataTable dtSource)
        {
            List<Treeview> rst = new List<Treeview>();
            DataRow[] pArr = dtSource.Select("Parent_id=0", "Co_id");
            foreach (DataRow dr in pArr)
            {
                Treeview pTr = new Treeview();
                pTr.id = CommFunc.ConvertDBNullToString(dr["Co_id"]);
                pTr.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                //pTr.attributes = CommFunc.ConvertDBNullToInt32(dr["power"]);
                pTr.nodes = new List<Treeview>();
                this.GetPduTree200(ref dtSource, ref pTr, CommFunc.ConvertDBNullToString(dr["Co_id"]));
                rst.Add(pTr);
            }
            return rst;
        }
        private void GetPduTree200(ref DataTable dtSource, ref Treeview pTr, string Co_id)
        {
            DataRow[] pArr = dtSource.Select("Parent_id=" + Co_id + " and Layer<4", "Co_id");
            int pRows = pArr.Count();
            pTr.attributes = new { lastLevel = 0 };
            if (pRows > 0)
                pTr.nodes = new List<Treeview>();
            else
                pTr.attributes = new { lastLevel = 1 };
            foreach (DataRow dr in pArr)
            {
                Treeview cTr = new Treeview();
                cTr.id = CommFunc.ConvertDBNullToString(dr["Co_id"]);
                cTr.text = CommFunc.ConvertDBNullToString(dr["CoName"]);
                //cTr.attributes = CommFunc.ConvertDBNullToInt32(dr["power"]);
                pTr.nodes.Add(cTr);
                this.GetPduTree200(ref dtSource, ref cTr, CommFunc.ConvertDBNullToString(dr["Co_id"]));
            }
        }

        #endregion

        /// <summary>
        /// 获取概况
        /// </summary>
        /// <returns></returns>
        public APIRst GetPduGeneral(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                int groupCnt = bll.GetPduGroupCount(co_id);
                DataTable dtSource = bll.GetPduInfo(co_id);
                DataRow[] arr = dtSource.Select("Parent_id=0");
                int pduCnt = arr.Count();
                int moduleCnt = dtSource.Select("Parent_id>0").Count();
                int onLine = 0;
                foreach (DataRow dr in arr)
                {
                    DateTime lastTime = CommFunc.ConvertDBNullToDateTime(dr["LastTime"]);
                    int frMd = CommFunc.ConvertDBNullToInt32(dr["FrMd"]);
                    if (lastTime.AddMinutes(frMd) >= DateTime.Now)
                        onLine = onLine + 1;
                }
                object obj = new { GroupCnt = groupCnt, PduCnt = pduCnt, ModuleCnt = moduleCnt, OnLine = onLine, Leave = pduCnt - onLine };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取概况错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取PDU能耗分布
        /// </summary>     
        /// <returns></returns>
        public APIRst GetPduEnergyPie(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetPduEnergyPie(co_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new {
							   CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
							   UseVal = CommFunc.ConvertDBNullToDecimal(s1["UseVal"])
                           };               
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取PDU能耗分布错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}