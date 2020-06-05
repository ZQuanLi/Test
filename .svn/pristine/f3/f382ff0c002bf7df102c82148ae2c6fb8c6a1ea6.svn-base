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
    public partial class ExpYdBuildingBatchHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Syscont.ExpYdBuildingBatchBLL bll = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExpYdBuildingBatchHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Syscont.ExpYdBuildingBatchBLL(user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 导出采集器模板(导出Excel模板)
        /// </summary>
        /// <returns></returns>
        public APIRst ExportBuilding()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("GwName", typeof(System.String));
                dt.Columns.Add("GwIp", typeof(System.String));
                dt.Columns.Add("EspName", typeof(System.String));
                dt.Columns.Add("EspAddr", typeof(System.String));
                dt.Columns.Add("TransferType", typeof(System.Int32));
                dt.Columns.Add("EspPort", typeof(System.Int32));
                dt.Columns.Add("ComPort", typeof(System.String));
                dt.Columns.Add("Baud", typeof(System.Int32));
                dt.Columns.Add("DataBit", typeof(System.Int32));
                dt.Columns.Add("StopBit", typeof(System.Int32));
                dt.Columns.Add("Parity", typeof(System.Int32));
                //dt.Columns.Add("ModuleAddr", typeof(System.String));
                dt.Columns.Add("MeterAddr", typeof(System.String));
                dt.Columns.Add("Multiply", typeof(System.String));
                dt.Columns.Add("ModuleType", typeof(System.String));
                dt.Columns.Add("ModuleAddr", typeof(System.String));
                dt.Columns.Add("CoFullName", typeof(System.String));

                dt.Rows.Add(new object[] { "集中器名称1", "127.0.0.1", "采集器名称1", "127.0.0.1", 1, 1000, "COM1", 2400, 8, 1, 2, "165153101457", 1.5, "DDS3366D", "165153101457", "绿城中心->12楼西->1209" });

                string xlsPath = System.Web.HttpContext.Current.Server.MapPath(WebConfig.XlsPath());
                if (!System.IO.Directory.Exists(xlsPath))
                {
                    System.IO.Directory.CreateDirectory(xlsPath);
                }
                string fn = "批量导入Excel模板.xls";
                string fileName = xlsPath + fn;
                bll.ExportBuildingBatch(fileName, dt);

                //object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = WebConfig.XlsPath() + fn;
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

        /// <summary>
        /// 批量上载采集器Excel数据
        /// </summary>
        /// <returns></returns>
        public APIRst ToLeadBuilding()
        {
            APIRst rst = new APIRst();
            try
            {
                string filePath = "";
                string msg = string.Empty;
                System.Data.DataTable dtSource = null;
                string FileUrl = string.Empty;
                //var fileToUpload = Request.Files;
                var serverPath = WebConfig.XlsPath();

                string key = HttpContext.Current.Request["key"];
                string value = HttpContext.Current.Request["value"];
                HttpFileCollection files = HttpContext.Current.Request.Files;

                if(files.Count == 0)
                    throw new Exception("请上传xls格式的excel文件");
                for (int i = 0; i < files.Count; i++)
                {
                    string[] fileName = System.IO.Path.GetFileName(files[i].FileName).Split('.');
                    string tF_name = fileName[0] + DateTime.Now.ToString("yyyyMMddhhmmss") + "." + fileName[1];
                    if (files[i] != null)
                    {
                        if (System.IO.Path.GetExtension(files[i].FileName) != ".xls")
                        {
                            throw new Exception("请上传xls格式的excel文件");
                        }
                        var rootPath = System.Web.HttpContext.Current.Server.MapPath("~/files/");
                        if (!System.IO.Directory.Exists(rootPath))
                        {
                            System.IO.Directory.CreateDirectory(rootPath);
                        }
                        filePath = rootPath + tF_name;
                        files[i].SaveAs(filePath);
                    }
                    string fName = System.Web.HttpContext.Current.Server.MapPath(serverPath);
                    dtSource = bll.GetExcelDataOfBuilding(filePath);
                    if (dtSource == null)
                    {
                        throw new Exception("导入模板中无数据...");
                    }
                    bll.UpdateBuildingMode(dtSource, ref fName, out msg);
                    string fn = "";
                    if (!string.IsNullOrEmpty(fName))
                    {
                        fn = serverPath + fName;
                        rst.data = fn;
                        throw new Exception(msg);
                    }

                    rst.data = 1;
                    //return Json(new { rst = true, msg = msg, data = "" });
                }
                
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取用户的用能列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }






    }
}