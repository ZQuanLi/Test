using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using YDS6000.Models;

namespace YDS6000.WebApi.Areas.Exp.Opertion.Mgr
{
    public class MgrHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Mgr.MgrBLL bll = null;
        public MgrHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Mgr.MgrBLL(user.Ledger, user.Uid);
        }
        /// <summary>
        /// 区域列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetAreaList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetVp_coinfo(CoAttribV2_1.Area);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1)+1,
                               Id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               No = CommFunc.ConvertDBNullToString(s1["CoNo"]),
                               Name = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("区域列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 单位列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetUnitList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetVp_coinfo(CoAttribV2_1.Unit);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               No = CommFunc.ConvertDBNullToString(s1["CoNo"]),
                               Name = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               AreaId = CommFunc.ConvertDBNullToInt32(s1["Parent_id"]),
                               AreaName = CommFunc.ConvertDBNullToString(s1["ParentName"])

                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("单位列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 项目列表
        /// </summary>
        /// <returns></returns>
        public APIRst GetProjectList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetVp_coinfo(CoAttribV2_1.Project);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               No = CommFunc.ConvertDBNullToString(s1["CoNo"]),
                               Name = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Addr = CommFunc.ConvertDBNullToString(s1["CustAddr"]),
                               Mobile = CommFunc.ConvertDBNullToString(s1["Mobile"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               UnitId = CommFunc.ConvertDBNullToInt32(s1["Parent_id"]),
                               UnitName = CommFunc.ConvertDBNullToString(s1["CoStrcName"])
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("项目列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 用电单元
        /// </summary>
        /// <returns></returns>
        public APIRst GetCellList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetVp_coinfo(CoAttribV2_1.Cell);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Id = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               No = CommFunc.ConvertDBNullToString(s1["CoNo"]),
                               Name = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               Addr = CommFunc.ConvertDBNullToString(s1["CustAddr"]),
                               Mobile = CommFunc.ConvertDBNullToString(s1["Mobile"]),
                               Remark = CommFunc.ConvertDBNullToString(s1["Remark"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               ProjectId = CommFunc.ConvertDBNullToInt32(s1["Parent_id"]),
                               ProjectName = CommFunc.ConvertDBNullToString(s1["ParentName"]),
                               Path = CommFunc.ConvertDBNullToString(s1["Path"])
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("用电单元:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 更新区域列表
        /// </summary>
        /// <param name="id">区域ID号</param>
        /// <param name="no">编码</param>
        /// <param name="name">名称</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public APIRst UpdateArea(int id, string no, string name, string remark)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.UpdateCoInfo(id, no, name, 0, CoAttribV2_1.Area, "", "", remark, 0);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("更新区域列表:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 更新单元 id=0 新增
        /// </summary>
        /// <param name="id">单元ID号</param>
        /// <param name="no">编码</param>
        /// <param name="name">名称</param>
        /// <param name="remark">备注</param>
        /// <param name="areaId">区域ID号</param>
        /// <returns></returns>
        public APIRst UpdateUnit(int id, string no, string name, string remark, int areaId)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.UpdateCoInfo(id, no, name, areaId, CoAttribV2_1.Unit, "", "", remark, 0);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("更新单元:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 更新项目 id=0 新增
        /// </summary>
        /// <param name="id">项目ID号</param>
        /// <param name="no">编码</param>
        /// <param name="name">项目名称</param>
        /// <param name="addr">地址</param>
        /// <param name="mobile">电话号码</param>
        /// <param name="remark">备注</param>
        /// <param name="disabled">是否弃用</param>
        /// <param name="unitId">单位ID号</param>
        /// <returns></returns>
        public APIRst UpdateProject(int id, string no, string name, string addr, string mobile, string remark, int disabled, int unitId)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.UpdateCoInfo(id, no, name, unitId, CoAttribV2_1.Project, addr, mobile, remark, disabled);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("更新项目:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 更新用电单元 id=0 新增
        /// </summary>
        /// <param name="id">用电单元ID号</param>
        /// <param name="no">编码</param>
        /// <param name="name">名称</param>
        /// <param name="addr">地址</param>
        /// <param name="mobile">电话号码</param>
        /// <param name="remark">备注</param>
        /// <param name="projectId">项目ID号</param>
        /// <returns></returns>
        public APIRst UpdateCell(int id, string no, string name, string addr, string mobile, string remark, int projectId)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.UpdateCoInfo(id, no, name, projectId, CoAttribV2_1.Cell, addr, mobile, remark, 0);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("更新用电单元:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除区域、单位、项目、用电单元
        /// </summary>
        /// <param name="co_id">ID号</param>
        /// <returns></returns>
        public APIRst DelCoInfo(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.DelCoInfo(co_id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除信息:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设备信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetMdList()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetMdList();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Id = CommFunc.ConvertDBNullToInt32(s1["Meter_id"]),
                               Name = CommFunc.ConvertDBNullToString(s1["MeterName"]),
                               CellId = CommFunc.ConvertDBNullToInt32(s1["Co_id"]),
                               CellName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                               ModuleId = CommFunc.ConvertDBNullToString(s1["Mm_id"]),
                               ModuleType = CommFunc.ConvertDBNullToString(s1["ModuleType"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               Disabled = CommFunc.ConvertDBNullToInt32(s1["Disabled"]),
                               Path = CommFunc.ConvertDBNullToString(s1["Path"]),
                               Inst_loc = CommFunc.ConvertDBNullToString(s1["Inst_loc"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设备信息:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设备采集项信息
        /// </summary>
        /// <param name="id">设备ID号</param>
        /// <returns></returns>
        public APIRst GetMapList(int id)
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetMapList(id);
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               RowId = dtSource.Rows.IndexOf(s1) + 1,
                               Module_id = CommFunc.ConvertDBNullToInt32(s1["Module_id"]),
                               Fun_id = CommFunc.ConvertDBNullToInt32(s1["Fun_id"]),
                               ModuleName = CommFunc.ConvertDBNullToString(s1["ModuleName"]),
                               FunType = CommFunc.ConvertDBNullToString(s1["FunType"]),
                               FunName = CommFunc.ConvertDBNullToString(s1["FunName"]),
                               TagName = CommFunc.ConvertDBNullToString(s1["TagName"]),
                               DataValue = CommFunc.ConvertDBNullToString(s1["DataValue"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设备采集项信息:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 保存设备信息
        /// </summary>
        /// <param name="id">设备ID号</param>
        /// <param name="name">设备名称</param>
        /// <param name="cellId">用电单元id号</param>
        /// <param name="moduleId">设备型号id号</param>
        /// <param name="disabled">设备状态=0正常=1弃用</param>
        /// <returns></returns>
        public APIRst SaveMdInfo(int id, string name, int cellId, int moduleId, int disabled)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SaveMdInfo(id, name, cellId, moduleId, disabled);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("保存设备信息:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 更新设备位置信息
        /// </summary>
        /// <param name="meter_id">设备ID号</param>
        /// <param name="inst_loc">设备安装地址</param>
        /// <returns></returns>
        public APIRst UpdateInst_loc(int meter_id, string inst_loc)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.UpdateInst_loc(meter_id, inst_loc);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("更新设备位置信息:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="meter_id"></param>
        /// <returns></returns>
        public APIRst DelMdInfo(int meter_id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.DelMdInfo(meter_id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("删除信息:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        public APIRst SaveMapInfo(int module_id, int fun_id, string tagName, string dataValue = "")
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SaveMapInfo(module_id, fun_id, tagName, dataValue);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("保存采集项目错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        public APIRst SetClear(int module_id)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetClear(module_id);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("一键清零:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="co_id">用电单元ID号</param>
        /// <returns></returns>
        public APIRst UpdateImg(int co_id)
        {
            APIRst rst = new APIRst();
            try
            {
                if (co_id <= 0)
                {
                    rst.rst = false;
                    rst.err.code = (int)ResultCodeDefine.Error;
                    rst.err.msg = "id不能为零";
                    return rst;
                }
                int num = 0;
                var fileCollectionBase = HttpContext.Current.Request.Files;
                string path = "";
                if (fileCollectionBase.Count > 0)
                {
                    var fileUploadPic = fileCollectionBase[0];

                    string ename = System.IO.Path.GetExtension(fileUploadPic.FileName).ToLower();
                    if (ename != ".jpg" && ename != ".jpeg" && ename != ".gif" && ename != ".png")
                        throw new Exception("不允许上传的文件类型(允许的类型：.jpg/.jpeg/.gif/.png)");
                    //判断附件大小是否符合不大于20MB
                    double fileLength = fileUploadPic.ContentLength / (1024.0 * 1024.0);
                    if (fileLength > 2.0)
                        throw new Exception("图片最大不能超过2MB");
                    //获取文件名（或者重命名）
                    var uploadResult = ToUpload(fileUploadPic, "files/img/view/", "image");
                    if (uploadResult.error > 0)
                    {
                        FileLog.WriteLog("上载图片错误:", uploadResult.message);
                        throw new Exception("上载图片错误:" + uploadResult.message);
                        //return Json(new { rst = false, msg = uploadResult.message, data = "" });
                    }
                    else
                    {
                        path = uploadResult.url;//取出服务器虚拟路径,存储上传文件 
                    }
                }
                //attached = string.IsNullOrEmpty(attached) ? CommFunc.ConvertDBNullToString(HttpContext.Current.Request["pAttached"]) : attached;
                string oldPath = bll.GetImg(co_id);
                if (string.IsNullOrEmpty(oldPath))
                {
                    try
                    {
                        File.Delete(HttpContext.Current.Server.MapPath(oldPath));
                    }
                    catch {
                    }
                }
                num = bll.UpdateImg(co_id, path);

                //DataTable dtSource = JsonHelper.ToDataTable(data.Data);
                //string path = GetYdCollectOnExport(data.Data);
                rst.data = num;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("系统配置-参数管理首页logo设置错误(SetPic)" + ex.Message + ex.StackTrace);
            }
            return rst;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fb">HttpPostedFileBase</param>
        /// <param name="directoryUrl">directoryUrl 目录URL 如“img/adr”</param>
        /// <param name="dirType">dirType文件类型</param>
        /// <returns></returns>
        private static dynamic ToUpload(HttpPostedFile fb, string directoryUrl, string dirType)
        {

            Stream sm = null;
            FileStream fsm = null;
            int bufferLen = 1024;
            byte[] buffer = new byte[bufferLen];
            int contentLen = 0;
            string relativeUrl = "";//相对路径
            try
            {
                //string fileName = Path.GetFileName(fb.FileName);
                string fileBaseName = Path.GetFileName(fb.FileName);
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999) + Path.GetExtension(fileBaseName);

                string baseUrl = HttpContext.Current.Server.MapPath("/");
                relativeUrl = directoryUrl + fileName;
                string uploadPath = baseUrl + "/" + directoryUrl;//目录路径

                if (!System.IO.Directory.Exists(uploadPath)) //判断目录路径是否存在
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string fullPath = uploadPath + fileName;
                //if (File.Exists(fullPath) == true)
                //{
                //    return new { error = 1, message = "服务器上已经有了你正在上传的文件" };
                //}                
                sm = fb.InputStream;
                fsm = new FileStream(fullPath, FileMode.Create, FileAccess.ReadWrite);
                while ((contentLen = sm.Read(buffer, 0, bufferLen)) != 0)
                {
                    fsm.Write(buffer, 0, bufferLen);
                    fsm.Flush();
                }
            }
            catch (Exception ex)
            {
                return new { error = 1, message = ex.Message };
            }

            finally
            {
                if (fsm != null)
                {
                    fsm.Dispose();
                }
                if (sm != null)
                {
                    sm.Close();
                }
            }

            return new { error = 0, message = "上传成功", url = "/" + relativeUrl, dir = dirType };
        }


        /// <summary>
        /// 从ioserver导入变量
        /// </summary>
        /// <param name="module_id">设备id</param>
        /// <param name="busname">总线名称</param>
        /// <param name="devname">设备名称</param>
        /// <returns></returns>
        public APIRst ImportTagsFromIOServer(int module_id, string busname, string devname)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.ImportTagsFromIOServer(module_id, busname, devname);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("从ioserver导入变量错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
    }
}