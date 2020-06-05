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
using System.Net.Http;

namespace YDS6000.WebApi.Areas.Exp.Opertion.Syscont
{
    public partial class ExpYdCustomerHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Syscont.ExpYdCustomerBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpYdCustomerHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Syscont.ExpYdCustomerBLL(user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 获取客户信息列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetCustomerList(int pageIndex, int pageSize, string CName, string CNum, string RoomName)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = null;
                dtSource = bll.GetCustomerList(0, CName, CNum, RoomName);
                object data = GetTableToList(dtSource, pageIndex, pageSize);
                object obj = new { total = dtSource.Rows.Count, rows = data };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取客户信息列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取首页用户数（月数and总数）
        /// </summary>
        /// <returns></returns>
        public APIRst GetHomeCustomer()
        {
            APIRst rst = new APIRst();
            try
            {
                //查询月数
                DataTable dtSource = bll.GetHomeCustomer(1);

                //查询总数
                DataTable dtSource2 = bll.GetHomeCustomer(2);

                object obj = new { Total = dtSource2.Rows.Count, Months = dtSource.Rows.Count };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页用户数错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private object GetTableToList(DataTable dtSource, int pageIndex, int pageSize)
        {
            if (pageIndex != 0 && pageSize != 0)
            {
                var res1 = from s1 in dtSource.AsEnumerable().Skip((pageIndex - 1) * pageSize).Take(pageSize)
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Crm_id = CommFunc.ConvertDBNullToInt32(s1["Crm_id"]),
                               CrmName = CommFunc.ConvertDBNullToString(s1["CrmName"]),//姓名
                               CrmNo = CommFunc.ConvertDBNullToString(s1["CrmNo"]),//客户编号
                               Phone = CommFunc.ConvertDBNullToString(s1["Phone"]),//固定电话
                               MPhone = CommFunc.ConvertDBNullToString(s1["MPhone"]),//移动电话
                               Email = CommFunc.ConvertDBNullToString(s1["Email"]),//邮件地址
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),//备注  
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),//房间ID
                               CoFullName = CommFunc.ConvertDBNullToString(s1["CoFullName"]), //入住地址   
                               Passwd = CommFunc.ConvertDBNullToString(s1["Passwd"]),//登录密码                              
                               IsAdmin = CommFunc.ConvertDBNullToInt32(s1["IsAdmin"]),//是否管理员 0不是,1是   
                               IsHold = CommFunc.ConvertDBNullToInt32(s1["IsHold"]),//是否房间主要负责人 0不是,1是
                               Contract = CommFunc.ConvertDBNullToString(s1["Contract"]),//合同号
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"])//电户号
                           };
                return res1.ToList();
            }
            else
            {
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Crm_id = CommFunc.ConvertDBNullToInt32(s1["Crm_id"]),
                               CrmName = CommFunc.ConvertDBNullToString(s1["CrmName"]),//姓名
                               CrmNo = CommFunc.ConvertDBNullToString(s1["CrmNo"]),//客户编号
                               Phone = CommFunc.ConvertDBNullToString(s1["Phone"]),//固定电话
                               MPhone = CommFunc.ConvertDBNullToString(s1["MPhone"]),//移动电话
                               Email = CommFunc.ConvertDBNullToString(s1["Email"]),//邮件地址
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),//备注  
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),//房间ID
                               CoFullName = CommFunc.ConvertDBNullToString(s1["CoFullName"]), //入住地址   
                               Passwd = CommFunc.ConvertDBNullToString(s1["Passwd"]),//登录密码                              
                               IsAdmin = CommFunc.ConvertDBNullToInt32(s1["IsAdmin"]),//是否管理员 0不是,1是   
                               IsHold = CommFunc.ConvertDBNullToInt32(s1["IsHold"]),//是否房间主要负责人 0不是,1是     
                               Contract = CommFunc.ConvertDBNullToString(s1["Contract"]),//合同号
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"])//电户号
                           };
                return res1.ToList();
            }
        }

        /// <summary>
        /// 设置客户信息
        /// </summary>
        /// <param name="nAct">nAct：新增=1,修改=2,删除=3</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public APIRst SetCustomer(int nAct, v3_userVModel model)
        {
            APIRst rst = new APIRst();
            try
            {
                if (nAct != 1 && nAct != 2 && nAct != 3)
                {
                    throw new Exception("执行操作类型错误");
                }
                if (nAct != 3 && Convert.IsDBNull(model.CrmNo))
                    throw new Exception("请输入证件号");
                if (nAct == 1 && bll.IsExistSameCrmNo(model.CrmNo))
                    throw new Exception("证件号" + model.CrmNo + "重复");
                if (nAct != 3 && string.IsNullOrEmpty(model.CrmName))
                    throw new Exception("请输入姓名");
                if (nAct != 1 && model.Crm_id == 0)
                    throw new Exception("获取数据错误");

                if (nAct == 3)
                    bll.DelCustomer(model.Crm_id);//删除               
                else
                    bll.EditCustomer(model, nAct);

                DataTable dtSource = bll.GetCustomerList(model.Crm_id);
                object data = this.GetTableToList(dtSource, 0, 0);
                object obj = new { total = dtSource.Rows.Count, data = data };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置客户信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 获取建筑房间信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="BuildName"></param>
        /// <param name="RoomName"></param>
        /// <returns></returns>
        public APIRst GetRoomInfo(int pageIndex, int pageSize, string BuildName, string RoomName)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = null;
                dtSource = bll.GetOrgs(0, true, BuildName, RoomName);
                object data = GetRoomToList(dtSource, pageIndex, pageSize);
                object obj = new { total = dtSource.Rows.Count, rows = data };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取建筑房间信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public object GetRoomToList(DataTable dtSource, int pageIndex, int pageSize)
        {
            if (pageIndex != 0 && pageSize != 0)
            {
                var res1 = from s1 in dtSource.AsEnumerable().Skip((pageIndex - 1) * pageSize).Take(pageSize)
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               Parent_id = CommFunc.ConvertDBNullToInt32(s1["Parent_id"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               StrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               IsDefineS = GetRoomType(s1["IsDefine"]),
                               IsDefine = CommFunc.ConvertDBNullToInt32(s1["IsDefine"]),
                               CoNo = CommFunc.ConvertDBNullToString(s1["CoNo"]),
                               Price = CommFunc.ConvertDBNullToString(s1["CdrPrice"]),
                               CoFullName = CommFunc.ConvertDBNullToString(s1["CoFullName"])
                           };
                return res1.ToList();
            }
            else
            {
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               Parent_id = CommFunc.ConvertDBNullToInt32(s1["Parent_id"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               StrcName = CommFunc.ConvertDBNullToString(s1["CoStrcName"]),
                               IsDefineS = GetRoomType(s1["IsDefine"]),
                               IsDefine = CommFunc.ConvertDBNullToInt32(s1["IsDefine"]),
                               CoNo = CommFunc.ConvertDBNullToString(s1["CoNo"]),
                               Price = CommFunc.ConvertDBNullToString(s1["CdrPrice"]),
                               CoFullName = CommFunc.ConvertDBNullToString(s1["CoFullName"])
                           };
                return res1.ToList();
            }
        }

        /// <summary>
        /// 汇出Excel
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public APIRst GetYdCustomerOnExport(DataModels data)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = JsonHelper.ToDataTable(data.Data);
                string path = GetYdCustomerOnExport(dtSource);
                //object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = path;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("导出采集器模板(导出Excel模板):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        private string GetYdCustomerOnExport(DataTable dtSource)
        {
            string fn = "/XTemp/客户列表.xls";
            string filePath = System.Web.HttpContext.Current.Server.MapPath(@"/XTemp");
            if (System.IO.Directory.Exists(filePath) == false)
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            string filename = System.Web.HttpContext.Current.Server.MapPath(fn);
            if (System.IO.File.Exists(filename))/*先删除已存在的文件，再汇出Excel*/
                System.IO.File.Delete(filename);
            if (dtSource == null || dtSource.Rows.Count == 0)
                throw new Exception("没有数据");
            Excel.ExcelCellStyle columnCellStyle0 = new Excel.ExcelCellStyle();
            columnCellStyle0 = new Excel.ExcelCellStyle()
            {
                DataFormart = "0.00",
                HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT
            };
            Excel.ExcelCellStyle columnCellStyle1 = new Excel.ExcelCellStyle();
            columnCellStyle1 = new Excel.ExcelCellStyle()
            {
                DataFormart = "yyyy-MM-dd HH:mm:ss",
            };
            Excel.ExcelColumnCollection columns = new Excel.ExcelColumnCollection();
            columns.Add(new Excel.ExcelColumn("序号", "RowId", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("合同号", "Contract", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("电户号", "ModuleName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("租客姓名", "CrmName", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("租客联系电话", "MPhone", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("租客身份证号码/护照", "CrmNo", 15) { IsSetWith = true });
            //columns.Add(new Excel.ExcelColumn("固定电话", "Phone", 15) { IsSetWith = true });
            //columns.Add(new Excel.ExcelColumn("邮件地址", "Email", 15) { IsSetWith = true });
            columns.Add(new Excel.ExcelColumn("入住地址", "CoFullName", 15) { IsSetWith = true });
            //columns.Add(new Excel.ExcelColumn("备注", "Remark", 15) { IsSetWith = true });
            Excel.ExcelOparete excel = new Excel.ExcelOparete("客户列表");
            excel.SetObjectValue("客户列表", 0, 0, 3);
            excel.SetColumnName(columns, 1, 0);
            excel.SetColumnValue(columns, dtSource.Select(), 2, 0);
            excel.SaveExcelByFullFileName(filename);
            return fn;
        }


        #region 入驻房号

        /// <summary>
        /// 获取入驻房号-已选项
        /// </summary>
        /// <param name="Crm_id"></param>
        /// <returns></returns>
        public APIRst GetYdCustomerRoomInfo(int Crm_id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdCustomerRoomInfo(Crm_id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Crm_id = CommFunc.ConvertDBNullToInt32(s1["Crm_id"]),
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),//房间ID
                               CoFullName = CommFunc.ConvertDBNullToString(s1["CoFullName"]), //入住地址
                               Create_dt = CommFunc.ConvertDBNullToDateTime(s1["Create_dt"]).ToString("yyyy-MM-dd"),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取入驻房号-已选项信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取入驻房号-可选项
        /// </summary>
        /// <param name="Crm_id"></param>
        /// <param name="CoName">建筑名称</param>
        /// <returns></returns>
        public APIRst GetYdCustomerRoomFind(int Crm_id, string CoName)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetYdCustomerRoomFind(CoName);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Crm_id = Crm_id,
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),//房间ID
                               CoFullName = CommFunc.ConvertDBNullToString(s1["CoFullName"]), //入住地址
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取入驻房号-可选项信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 保存入驻房号
        /// </summary>
        /// <param name="Crm_id"></param>
        /// <param name="CoIdStr">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        public APIRst SaveYdCustomerOpenRoom(int Crm_id, string CoIdStr)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Crm_id == 0)
                    throw new Exception("请选择业主");

                int cnt = bll.SaveYdCustomerOpenRoom(Crm_id, CoIdStr);

                object obj = new { total = cnt, rows = cnt };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("保存入驻房号信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 保存安全密码
        /// </summary>
        /// <param name="Crm_id"></param>
        /// <param name="Passwd">密码</param>
        /// <returns></returns>
        public APIRst SaveYdCustomerOpenWd(int Crm_id, string Passwd)
        {
            APIRst rst = new APIRst();
            try
            {
                if (Crm_id == 0)
                    throw new Exception("请选择业主");

                int cnt = bll.SaveYdCustomerOpenWd(Crm_id, Passwd);
                object obj = new { total = cnt, rows = cnt };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("保存安全密码信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }





        #endregion



        private string GetRoomType(object obj)
        {
            int type = CommFunc.ConvertDBNullToInt32(obj);
            switch (type)
            {
                case 0:
                    return "房间表";
                case 1:
                    return "公区表";
                case 2:
                    return "公共表";
                case 3:
                    return "公摊表";
                case 4:
                    return "洗衣房间";
                case 9999:
                    return "备用房间";
            }
            return "房间表";
        }


    }
}