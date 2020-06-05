using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Opertion.PayCharg
{
    public class PayChargHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.PayCharg.PayChargBLL bll = null;
        public PayChargHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.PayCharg.PayChargBLL(WebConfig.SysProject, user.Ledger, user.Uid);
        }

        /// <summary>
        /// 获取物业列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetChargList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetChargList();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               Log_id = CommFunc.ConvertDBNullToInt32(s1["Log_id"]),
                               Co_id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               FirstVal = CommFunc.ConvertDBNullToDecimal(s1["FirstVal"]).ToString("f2"),
                               LastVal = CommFunc.ConvertDBNullToDecimal(s1["LastVal"]).ToString("f2"),
                               Price = CommFunc.ConvertDBNullToDecimal(s1["Price"]).ToString("f2"),
                               LastAmt = CommFunc.ConvertDBNullToDecimal(s1["LastAmt"]).ToString("f2"),
                               ChargVal = CommFunc.ConvertDBNullToDecimal(s1["ChargVal"]).ToString("f2"),
                               FirstTime = CommFunc.ConvertDBNullToDateTime(s1["FirstTime"]).ToString("yyyy-MM-dd"),
                               LastTime = CommFunc.ConvertDBNullToDateTime(s1["LastTime"]).ToString("yyyy-MM-dd"),
                           };
                rst.data = res1.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取物业列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 新增物业列表
        /// </summary>
        /// <returns></returns>
        public APIRst AddCharg(int co_id, decimal firstVal, decimal lastVal, DateTime firstTime, DateTime lastTime, decimal price, decimal chargAmt)
        {
            APIRst rst = new APIRst();
            try
            {
                int cc = bll.AddCharg(co_id, firstVal, lastVal, firstTime, lastTime, price, chargAmt);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("新增物业列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIRst UpdateCharg(long log_id, decimal firstVal, decimal lastVal, DateTime firstTime, DateTime lastTime, decimal price, decimal chargAmt)
        {
            APIRst rst = new APIRst();
            try
            {
                int cc = bll.UpdateCharg(log_id, firstVal, lastVal, firstTime, lastTime, price, chargAmt);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("新增物业列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="log_id">ID号</param>
        /// <returns></returns>
        public APIRst DeleteCharg(long log_id)
        {
            APIRst rst = new APIRst();
            try
            {
                int cc = bll.DeleteCharg(log_id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除物业列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        public APIRst ToLeadCharg()
        {
            APIRst rst = new APIRst();
            var fileToUpload = HttpContext.Current.Request.Files;
            //var serverPath = Common.GetXlsPath();
            try
            {
                var rootPath = HttpContext.Current.Server.MapPath("~/files/");
                if (!System.IO.Directory.Exists(rootPath))
                {
                    System.IO.Directory.CreateDirectory(rootPath);
                }
                string filePath = "";
                for (int i = 0; i < fileToUpload.Count; i++)
                {
                    string[] fileName = System.IO.Path.GetFileName(fileToUpload[i].FileName).Split('.');
                    string tF_name = fileName[0] + DateTime.Now.ToString("yyyyMMddhhmmss") + "." + fileName[1];
                    if (fileToUpload[i] != null)
                    {
                        if (System.IO.Path.GetExtension(fileToUpload[i].FileName) != ".xls")
                        {
                            rst.rst = false;
                            rst.err = new APIErr() { code = -1, msg = "请上传xls格式的excel文件" };
                            return rst;
                        }                      
                        filePath = rootPath + tF_name;
                        fileToUpload[i].SaveAs(filePath);
                    }
                    DataTable dtSource = this.GetExcelData(filePath);
                    if (dtSource == null)
                    {
                        rst.rst = false;
                        rst.err = new APIErr() { code = -1, msg = "导入模板中无数据.." };
                        return rst;
                    }
                    int cc = bll.ToLeadCharg(ref dtSource);
                    string fn = WebConfig.XlsPath();
                    string msg = this.ExportInfo(dtSource,ref fn);
                    rst.data = new { msg = msg, fn = fn };
                }
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("新增物业列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public DataTable GetExcelData(string filename)
        {
            string filename1 = filename;
            //
            #region 1.0获取XLS数据
            Excel.XlsOparete excel = new Excel.XlsOparete();
            DataTable dtSource = excel.XlsToDataTable(filename1);
            DataRow dr = null;
            StringBuilder strDel = new StringBuilder();
            if (dtSource.Rows.Count > 0)
            {
                dr = dtSource.Rows[0];
            }
            else
            {
                foreach (DataColumn dc in dtSource.Columns)
                {
                    if (!string.IsNullOrEmpty(strDel.ToString()))
                        strDel.Append(",");
                    strDel.Append(dc.ColumnName);
                }
            }
            #endregion

            #region 2.0修改列
            if (dr != null)
            {
                foreach (DataColumn dc in dtSource.Columns)
                {
                    #region 2.1集中器
                    if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Equals("房间号"))
                    {
                        dc.ColumnName = "CoName";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Contains("上期计数"))
                    {
                        dc.ColumnName = "FirstVal";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Contains("本期计数"))
                    {
                        dc.ColumnName = "LastVal";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Contains("上期时间"))
                    {
                        dc.ColumnName = "FirstTime";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Contains("本期时间"))
                    {
                        dc.ColumnName = "LastTime";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Contains("水费单价"))
                    {
                        dc.ColumnName = "Price";
                    }
                    else if (CommFunc.ConvertDBNullToString(dr[dc.ColumnName]).Trim().Contains("物业费"))
                    {
                        dc.ColumnName = "ChargVal";
                    }
                    #endregion
                    else
                    {
                        if (!string.IsNullOrEmpty(strDel.ToString()))
                            strDel.Append(",");
                        strDel.Append(dc.ColumnName);
                    }
                }
                dtSource.Rows.RemoveAt(0);
            }
            #endregion

            #region 3.0删除列
            if (!string.IsNullOrEmpty(strDel.ToString()))
            {
                foreach (string s in strDel.ToString().Split(','))
                    dtSource.Columns.Remove(s);
            }
            #endregion

            #region 4.0增加列
            string strCs = "ErrTxt";
            string intCs = "Co_id,ErrCode";
            foreach (string s in strCs.Split(','))
            {
                if (dtSource.Columns.Contains(s) == true) continue;
                dtSource.Columns.Add(s, typeof(System.String));
            }
            foreach (string s in intCs.Split(','))
            {
                if (dtSource.Columns.Contains(s) == true) continue;
                dtSource.Columns.Add(s, typeof(System.Int32));
            }
            #endregion

            #region 5.0删除空白行 (必须倒叙删除，因为每删除一行，索引就会发生改变)
            for (int i = dtSource.Rows.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(dtSource.Rows[i]["CoName"])) && string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(dtSource.Rows[i]["FirstVal"])) && string.IsNullOrEmpty(CommFunc.ConvertDBNullToString(dtSource.Rows[i]["LastVal"])))
                {
                    dtSource.Rows[i].Delete();
                }
            }
            #endregion

            dtSource.AcceptChanges();
            return dtSource;
        }

        private string ExportInfo(DataTable dtSource,ref string rootPath)
        {
            string msg = "";
            #region 汇出上载错误信息
            try
            {
                var fileName = HttpContext.Current.Server.MapPath(rootPath);
                if (!System.IO.Directory.Exists(fileName))
                    System.IO.Directory.CreateDirectory(fileName);

                //var fileName = HttpContext.Current.Server.MapPath("~/files/");
                int Total = dtSource.Rows.Count;
                //string sPath = fileName;
                //if (!System.IO.Directory.Exists(sPath))
                //    System.IO.Directory.CreateDirectory(sPath);
                //
                DataRow[] errArr = dtSource.Select("isnull(ErrCode,0)<0");
                int eNum = errArr.Count();
                msg = "总记录:" + Total + ",成功上载:" + (Total - eNum);
                if (eNum > 0)
                {
                    Excel.ExcelCellStyle columnCellStyle0 = new Excel.ExcelCellStyle();
                    columnCellStyle0 = new Excel.ExcelCellStyle()
                    {
                        DataFormart = "0.00",
                        HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment.RIGHT
                    };
                    string fn = "执行错误信息" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";
                    string filePath = fileName + fn;
                    rootPath = rootPath + fn;
                    //fileName = fn;
                    Excel.ExcelColumnCollection columns = new Excel.ExcelColumnCollection();
                    columns.Add(new Excel.ExcelColumn("房间号", "CoName", 20) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("上期计数", "FirstVal", 20) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("本期计数", "LastVal", 20) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("上期时间", "FirstTime", 20) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("本期时间", "LastTime", 40) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("水费单价", "Price", 10) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("物业费", "ChargVal", 10) { IsSetWith = true });
                    columns.Add(new Excel.ExcelColumn("错误提示", "ErrTxt", 30) { IsSetWith = true });
                    Excel.ExcelOparete excel = new Excel.ExcelOparete("执行结果");
                    excel.SetColumnName(columns, 0, 0);
                    excel.SetColumnValue(columns, errArr, 1, 0);
                    excel.SaveExcelByFullFileName(filePath);
                    msg = msg + ",失败上载:" + eNum;
                }
                else
                {
                    rootPath = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            #endregion
            return msg;
        }
    }
}