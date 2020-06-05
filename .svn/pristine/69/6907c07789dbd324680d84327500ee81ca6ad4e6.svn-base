using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using YDS6000.Models;
using System.Web.Mvc;
using System.IO;

namespace YDS6000.WebApi.Areas.Exp.Opertion.Syscont
{
    public partial class ExpParameterHelper
    {
        private CacheUser user = null;
        private YDS6000.BLL.Exp.Syscont.ParameterBLL bll = null;
        public ExpParameterHelper()
        {
            user = WebConfig.GetSession();
            bll = new YDS6000.BLL.Exp.Syscont.ParameterBLL(user.Ledger, user.Uid);
            WebConfig.GetSysConfig();
        }

        /// <summary>
        /// 增加操作页面LOG
        /// </summary>
        /// <returns></returns>
        public APIRst AddLog(string content)
        {
            APIRst rst = new APIRst();
            try
            {
                
                //HttpRequestBase req = ((System.Web.HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request;
                //string nameSpace = actionContext.ActionDescriptor.ControllerDescriptor.ControllerType.Namespace;
                //string controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                //string actionName = actionContext.ActionDescriptor.ActionName;
                //string userHostAddress = "", absolutePath = "";
                //if (req != null)
                //{
                //    userHostAddress = req.UserHostAddress;
                //    absolutePath = req.Url.AbsolutePath;
                //}
                //else
                //{
                //    FileLog.WriteLog(string.Format("访问命名空间{0}控制器{1}操作对象{2}的HttpRequestBase对象不存在！", nameSpace, controllerName, actionName));
                //}
                YDS6000.BLL.WholeBLL.AddLog(user.Ledger, user.Uid, "", "", "", "", content);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("增加访问记录错误:", ex.Message);
            }
            return rst;
        }

        /// <summary>
        /// 获取充值参数信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetFrPay()
        {
            APIRst rst = new APIRst();
            try
            {
                var dt = bll.GetFrPay();
                var dtsource = from s1 in dt.AsEnumerable()
                               select new
                               {
                                   frpay = CommFunc.ConvertDBNullToFloat(s1["Price"]),
                                   fCoName = CommFunc.ConvertDBNullToString(s1["CoName"]),
                                   vRdChrgType = CommFunc.ConvertDBNullToInt32(s1["ChrgType"])
                               };
                rst.data = dtsource.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取充值参数信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 充值参数
        /// </summary>
        /// <param name="frPay">电量单价</param>
        /// <param name="fReort_head">售电票据单位名称</param>
        /// <param name="vRdChrgType">付费类型</param>
        /// <returns></returns>
        public APIRst FSetFrPay(string frPay, string fReort_head, int vRdChrgType)
        {
            APIRst rst = new APIRst();
            try
            {
                float frPay1 = Convert.ToSingle(frPay);
                rst.data = bll.FSetFrPay(frPay1, fReort_head, vRdChrgType);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取充值参数信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取采集频率
        /// </summary>
        /// <returns></returns>
        public APIRst GetTheCmTask()
        {
            APIRst rst = new APIRst();
            try
            {
                var dt = bll.GetTheCmTask();
                var dtsource = from s1 in dt.AsEnumerable()
                               select new
                               {
                                   frmd = CommFunc.ConvertDBNullToInt32(s1["FrMd"])
                               };
                rst.data = dtsource.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取采集频率信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置采集频率
        /// </summary>
        /// <param name="frmd">周期</param>
        /// <returns></returns>
        public APIRst SetCmTaskFrMd(string frmd)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetCmTaskFrMd(frmd);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取采集频率信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取登录页文字
        /// </summary>
        /// <returns></returns>
        public APIRst GetTheWords()
        {
            APIRst rst = new APIRst();
            try
            {
                var dt = bll.GetTheWords();
                var dtsource = from s1 in dt.AsEnumerable()
                               select new
                               {
                                   frName = CommFunc.ConvertDBNullToString(s1["Fname"]),
                                   frCopyright = CommFunc.ConvertDBNullToString(s1["FCopyright"]),
                                   BgImg = CommFunc.ConvertDBNullToString(s1["BgImg"]),
                               };
                rst.data = dtsource.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取登录页文字信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置登录页文字
        /// </summary>
        /// <param name="frName">首页标题展示文字</param>
        /// <param name="FCopyright">首页版权展示文字</param>
        /// <returns></returns>
        public APIRst SetWords(string frName, string FCopyright)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetWords(frName, FCopyright);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取登录页文字信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取首页的LOGO,背景图和标题
        /// </summary>
        /// <returns></returns>
        public APIRst GetPicANDWord()
        {
            APIRst rst = new APIRst();
            try
            {
                DataTable dtSource = bll.GetPicANDWord();
                var res1 = from s1 in dtSource.AsEnumerable()
                           select new
                           {
                               attached = CommFunc.ConvertDBNullToString(s1["Attached"]),
                               fname = CommFunc.ConvertDBNullToString(s1["Fname"]),
                               BgImg = CommFunc.ConvertDBNullToString(s1["BgImg"]),
                           };
                object obj = new { total = dtSource.Rows.Count, rows = res1.ToList() };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取首页的LOGO,背景图和标题(GetPicANDWord)信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置首页logo
        /// </summary>
        /// <returns></returns>
        public APIRst SetPic()
        {
            APIRst rst = new APIRst();
            try
            {
                int num = 0;
                var fileCollectionBase = HttpContext.Current.Request.Files;
                string Attached = "";
                if (fileCollectionBase.Count > 0)
                {
                    var fileUploadPic = fileCollectionBase[0];

                    string ename = System.IO.Path.GetExtension(fileUploadPic.FileName).ToLower();
                    if (ename != ".jpg" && ename != ".jpeg" && ename != ".gif" && ename != ".png")
                        throw new Exception("不允许上传的文件类型(允许的类型：.jpg/.jpeg/.gif/.png)");
                    //判断附件大小是否符合不大于20MB
                    double fileLength = fileUploadPic.ContentLength / (1024.0 * 1024.0);
                    if (fileLength > 1.0)
                        throw new Exception("图片最大不能超过1MB");
                    //获取文件名（或者重命名）
                    var uploadResult = ToUpload(fileUploadPic, "files/img/logo/", "image");
                    if (uploadResult.error > 0)
                    {
                        FileLog.WriteLog("上载图片错误:", uploadResult.message);
                        throw new Exception("上载图片错误:" + uploadResult.message);
                        //return Json(new { rst = false, msg = uploadResult.message, data = "" });
                    }
                    else
                    {
                        Attached = uploadResult.url;//取出服务器虚拟路径,存储上传文件 
                    }
                }
                Attached = string.IsNullOrEmpty(Attached) ? CommFunc.ConvertDBNullToString(HttpContext.Current.Request["pAttached"]) : Attached;

                num = bll.SetPic(Attached);

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
        /// 设置首页背景图
        /// </summary>
        /// <returns></returns>
        public APIRst SetBgImg()
        {
            APIRst rst = new APIRst();
            try
            {
                int num = 0;
                var fileCollectionBase = HttpContext.Current.Request.Files;
                string BgImg = "";
                if (fileCollectionBase.Count > 0)
                {
                    var fileUploadPic = fileCollectionBase[0];

                    string ename = System.IO.Path.GetExtension(fileUploadPic.FileName).ToLower();
                    if (ename != ".jpg" && ename != ".jpeg" && ename != ".gif" && ename != ".png")
                        throw new Exception("不允许上传的文件类型(允许的类型：.jpg/.jpeg/.gif/.png)");
                    //判断附件大小是否符合不大于20MB
                    double fileLength = fileUploadPic.ContentLength / (1024.0 * 1024.0);
                    if (fileLength > 1.0)
                        throw new Exception("图片最大不能超过1MB");
                    //获取文件名（或者重命名）
                    var uploadResult = ToUpload(fileUploadPic, "files/img/logo/", "image");
                    if (uploadResult.error > 0)
                    {
                        FileLog.WriteLog("上载图片错误:", uploadResult.message);
                        throw new Exception("上载图片错误:" + uploadResult.message);
                        //return Json(new { rst = false, msg = uploadResult.message, data = "" });
                    }
                    else
                    {
                        BgImg = uploadResult.url;//取出服务器虚拟路径,存储上传文件 
                    }
                }
                BgImg = string.IsNullOrEmpty(BgImg) ? CommFunc.ConvertDBNullToString(HttpContext.Current.Request["pBgImg"]) : BgImg;

                num = bll.SetBgImg(BgImg);

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
        public static dynamic ToUpload(HttpPostedFile fb, string directoryUrl, string dirType)
        {

            Stream sm = null;
            FileStream fsm = null;
            int bufferLen = 1024;
            byte[] buffer = new byte[bufferLen];
            int contentLen = 0;
            string relativeUrl = "";//相对路径
            try
            {
                string fileName = Path.GetFileName(fb.FileName);
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
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public APIRst GetMenuList()
        {
            APIRst rst = new APIRst();
            try
            {
                int total = 0;
                List<Treeview> tr = bll.GetMenuList(out total);
                object obj = new { total = total, rows = tr };
                rst.data = obj;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取区域树形列表错误(GetMenuList):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置菜单
        /// </summary>
        /// <param name="ids">选中的ID号（多个用逗号拼接）</param>
        /// <returns></returns>
        public APIRst SetMenuList(string ids)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetMenuList(ids);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取区域树形列表错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取白名单设置
        /// </summary>
        /// <returns></returns>
        public APIRst GetWhitelist()
        {
            APIRst rst = new APIRst();
            try
            {
                var dt = bll.GetWhitelist();
                rst.data = CommFunc.ConvertDBNullToString(dt.Rows[0]["ExCfg"]);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取白名单信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置白名单
        /// </summary>
        /// <param name="exCfg"></param>
        /// <returns></returns>
        public APIRst SetWhitelist(string exCfg)
        {
            APIRst rst = new APIRst();
            try
            {
                // exCfg = {m01:0,m02:1,m03:0,m04:0,m05:1,m06:0,m07:0,m08:0,m09:0,m10:0,m11:0,m12:0}
                rst.data = bll.SetWhitelist(exCfg);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取白名单信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取赠电参数
        /// </summary>
        /// <returns></returns>
        public APIRst GetZdCfg()
        {
            APIRst rst = new APIRst();
            try
            {
                var dt = bll.GetZdCfg();
                var dtsource = from s1 in dt.AsEnumerable()
                               select new
                               {
                                   mthBaseRoom = CommFunc.ConvertDBNullToDecimal(s1["mthBaseRoom"]),
                                   mthBaseCrm = CommFunc.ConvertDBNullToDecimal(s1["mthBaseCrm"]),
                                   mthBaseIsAcp = CommFunc.ConvertDBNullToInt32(s1["mthBaseIsAcp"]),
                                   BaseType = CommFunc.ConvertDBNullToInt32(s1["BaseType"])
                               };
                rst.data = dtsource.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取充赠电参数信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置赠电参数
        /// </summary>
        /// <param name="MthBaseRoom">每月赠送</param>
        /// <param name="MthBaseCrm">每月按用户个数赠送</param>
        /// <param name="MthBaseIsAcp">未使用完的赠电量是否累计到下月</param>
        /// <param name="BaseType">赠电类型</param>
        /// <returns></returns>
        public APIRst SetZdCfg(decimal MthBaseRoom, decimal MthBaseCrm, int MthBaseIsAcp, int BaseType)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetZdCfg(MthBaseRoom, MthBaseCrm, MthBaseIsAcp, BaseType);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取赠电参数信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取告警参数--告警设置
        /// </summary>
        /// <returns></returns>
        public APIRst GetAlarmVal()
        {
            APIRst rst = new APIRst();
            try
            {
                var dt = bll.GetAlarmVal();
                var dtsource = from s1 in dt.AsEnumerable()
                               select new
                               {
                                   AlarmVal1 = CommFunc.ConvertDBNullToDecimal(s1["AlarmVal1"]),
                                   Rule1 = CommFunc.ConvertDBNullToDecimal(s1["Rule1"]),
                                   AlarmVal2 = CommFunc.ConvertDBNullToDecimal(s1["AlarmVal2"]),
                                   Rule2 = CommFunc.ConvertDBNullToDecimal(s1["Rule2"]),
                                   AlarmVal3 = CommFunc.ConvertDBNullToDecimal(s1["AlarmVal3"]),
                                   Rule3 = CommFunc.ConvertDBNullToDecimal(s1["Rule3"])
                               };
                rst.data = dtsource.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取告警参数--告警设置信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置告警参数--告警设置
        /// </summary>
        /// <param name="AlarmVal1">方案①： 低于告警电量</param>
        /// <param name="Rule1">天/次的频率告警</param>
        /// <param name="AlarmVal2">方案②： 低于告警电量</param>
        /// <param name="Rule2">天/次的频率告警</param>
        /// <param name="AlarmVal3">方案③： 低于告警电量</param>
        /// <param name="Rule3">天/次的频率告警</param>
        /// <returns></returns>
        public APIRst SetAlarmVal(decimal AlarmVal1, decimal Rule1, decimal AlarmVal2, decimal Rule2, decimal AlarmVal3, decimal Rule3)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetAlarmVal(AlarmVal1, Rule1, AlarmVal2, Rule2, AlarmVal3, Rule3);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("系统配置-参数管理告警设置参数:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取告警参数--透支电量
        /// </summary>
        /// <returns></returns>
        public APIRst GetAlarmCfg()
        {
            APIRst rst = new APIRst();
            try
            {
                var dt = bll.GetAlarmCfg();
                var dtsource = from s1 in dt.AsEnumerable()
                               select new
                               {
                                   odValue = CommFunc.ConvertDBNullToDecimal(s1["CfValue"]),
                                   isClosed = CommFunc.ConvertDBNullToInt32(s1["Rule"])
                               };
                rst.data = dtsource.ToList();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("系统配置-参数管理透支电量参数信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置告警参数--透支电量
        /// </summary>
        /// <param name="OdValue">最低拉闸电量</param>
        /// <param name="IsClosed">最低电量是否拉闸</param>
        /// <returns></returns>
        public APIRst SetAlarmCfg(decimal OdValue, int IsClosed)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.SetAlarmCfg(OdValue, IsClosed);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("系统配置-参数管理透支电量参数(SetAlarmCfg):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取微信公众号信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetWxCofig()
        {
            APIRst rst = new APIRst();
            try
            {

                rst.data = WebConfig.WxConfig;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("系统配置-微信公众号信息错误:" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置微信公众号信息
        /// </summary>
        /// <param name="appid">APPID</param>
        /// <param name="mchid">商户号</param>
        /// <param name="key">商户支付密钥</param>
        /// <param name="appsecret">公众号Secert(仅JSAPI支付时需配置)</param>
        /// <param name="redirect_uri">公众号回调页面(仅JSAPI支付时需配置)</param>
        /// <returns></returns>
        public APIRst SetWxCofig(string appid, string mchid, string key, string appsecret, string redirect_uri)
        {
            APIRst rst = new APIRst();
            try
            {
                if (string.IsNullOrEmpty(appid))
                    throw new Exception(" APPID不能为空");
                if (string.IsNullOrEmpty(mchid))
                    throw new Exception(" 商户号不能为空");
                if (string.IsNullOrEmpty(key))
                    throw new Exception(" 商户支付密钥不能为空");
                rst.data = bll.SetWxCofig(appid, mchid, key, appsecret, redirect_uri);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("系统配置-微信公众号信息错误(SetAlarmCfg):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取短信接口信息-模块一
        /// </summary>
        /// <returns></returns>
        public APIRst GetAliConfig()
        {
            APIRst rst = new APIRst();
            try
            {

                rst.data = SmsFun.AliSmsConfig;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("系统配置-短信接口信息-模块一错误(GetAliConfig):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取短信接口信息-模块二
        /// </summary>
        /// <returns></returns>
        public APIRst GetComConfig()
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = WebConfig.ComSmsConfig; //  SmsFun.ComSmsConfig;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("系统配置-短信接口信息-模块二错误(GetComConfig):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置短信接口信息
        /// </summary>
        /// <param name="AliUrl">APPID</param>
        /// <param name="AliAppKey">商户号</param>
        /// <param name="AliAppSecret">商户支付密钥</param>
        /// <param name="AliSignName">公众号Secert(仅JSAPI支付时需配置)</param>
        /// <param name="AliTemplateCode">公众号回调页面(仅JSAPI支付时需配置)</param>
        /// <param name="ComSms">商户支付密钥</param>
        /// <returns></returns>
        public APIRst SetAliConfig(string AliUrl, string AliAppKey, string AliAppSecret, string AliSignName, string AliTemplateCode, string ComSms)
        {
            APIRst rst = new APIRst();
            try
            {
                if (string.IsNullOrEmpty(AliUrl))
                    throw new Exception(" 请求地址不能为空");
                if (string.IsNullOrEmpty(AliAppKey))
                    throw new Exception(" 应用AppKey不能为空");
                if (string.IsNullOrEmpty(AliAppSecret))
                    throw new Exception(" 密钥不能为空");
                if (string.IsNullOrEmpty(AliSignName))
                    throw new Exception(" 签名名称不能为空");
                if (string.IsNullOrEmpty(AliTemplateCode))
                    throw new Exception(" 模板ID不能为空");

                AliSmsConfig Alisms = new AliSmsConfig();
                Alisms.AliUrl = AliUrl;
                Alisms.AliAppKey = AliAppKey;
                Alisms.AliAppSecret = AliAppSecret;
                Alisms.AliSignName = AliSignName;
                Alisms.AliTemplateCode = AliTemplateCode;

                rst.data = bll.SetAliConfig(Alisms);

                ComSmsConfig comConfig = new ComSmsConfig();
                comConfig.ComSms = ComSms;
                bll.SetComConfig(comConfig);
                WebConfig.GetSysConfig();

            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("系统配置-短信接口信息错误(SetAlarmCfg):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 获取邮件接口信息
        /// </summary>
        /// <returns></returns>
        public APIRst GetMailConfig()
        {
            APIRst rst = new APIRst();
            try
            {

                rst.data = YDS6000.Models.EmailUtilities.EmConfig;
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("获取邮件接口信息错误(GetMailConfig):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

        /// <summary>
        /// 设置邮件接口信息
        /// </summary>
        /// <param name="MailFrom">邮件发送人</param>
        /// <param name="MailSmtpHost">邮件服务器地址</param>
        /// <param name="MailSmtpPassword">邮件登陆密码</param>
        /// <param name="MailSmtpUser">邮件登陆名</param>
        /// <returns></returns>
        public APIRst SetMailConfig(string MailFrom, string MailSmtpHost, string MailSmtpPassword, string MailSmtpUser)
        {
            APIRst rst = new APIRst();
            try
            {
                if (string.IsNullOrEmpty(MailFrom))
                    throw new Exception(" 邮件发送人不能为空");
                if (string.IsNullOrEmpty(MailSmtpHost))
                    throw new Exception(" 邮件服务器地址不能为空");
                if (string.IsNullOrEmpty(MailSmtpPassword))
                    throw new Exception(" 邮件登陆密码不能为空");
                if (string.IsNullOrEmpty(MailSmtpUser))
                    throw new Exception(" 邮件登陆名不能为空");

                rst.data = bll.SetMailConfig(MailFrom, MailSmtpHost, MailSmtpPassword, MailSmtpUser);

                WebConfig.GetSysConfig();
                ListenVModel vm = new ListenVModel();
                vm.cfun = ListenCFun.config.ToString();
                vm.content = "";
                CacheMgr.BeginSend(vm);

            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("系统配置-短信接口信息错误(SetAlarmCfg):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 获取短信接收人
        /// </summary>
        /// <returns></returns>
        public APIRst GetSmsBy()
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.GetSmsBy();
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置短信接收人错误(UpdateSmsBy):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }
        /// <summary>
        /// 设置短信接收人
        /// </summary>
        /// <param name="uid">接收人ID号</param>
        /// <returns></returns>
        public APIRst UpdateSmsBy(int uid)
        {
            APIRst rst = new APIRst();
            try
            {
                rst.data = bll.UpdateSmsBy(uid);
            }
            catch (Exception ex)
            {
                rst.rst = false;
                rst.err.code = (int)ResultCodeDefine.Error;
                rst.err.msg = ex.Message;
                FileLog.WriteLog("设置短信接收人错误(UpdateSmsBy):" + ex.Message + ex.StackTrace);
            }
            return rst;
        }

    }
}