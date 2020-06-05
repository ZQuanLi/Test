using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using YDS6000.Models;

namespace YDS6000.WebApi
{
    /// <summary>
    /// 过滤验证
    /// </summary>
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 访问前
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //CacheUser user = HttpContext.Current.Session["CacheUser"] as CacheUser;
            string msg = "";
            CacheUser user = WebConfig.GetAuthorizeUser(out msg);
            HttpRequestBase req = ((System.Web.HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request;
            string nameSpace = actionContext.ActionDescriptor.ControllerDescriptor.ControllerType.Namespace;
            string controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = actionContext.ActionDescriptor.ActionName;
            string userHostAddress = "", absolutePath = "";
            if (req != null)
            {
                userHostAddress = req.UserHostAddress;
                absolutePath = req.Url.AbsolutePath;
            }
            else
            {
                FileLog.WriteLog(string.Format("访问命名空间{0}控制器{1}操作对象{2}的HttpRequestBase对象不存在！", nameSpace, controllerName, actionName));
            }
            string content = "";/*描述*/
            string prog_id = "";/*权限ID*/
            bool authorize = false;/*权限*/
            bool chkSession = true;/*检测seesion是否过期*/
                                   /////////////
            #region 对象信息
            var obj = actionContext.ControllerContext.Controller.GetType().GetMethod(actionName).GetCustomAttributes(typeof(SecurityCtrl), false);
            if (obj != null)
            {
                foreach (SecurityCtrl md in obj)
                {
                    content = md.describe;/*描述*/
                    prog_id = md.prog_id;/*权限ID号*/
                    authorize = md.authorize;/*是否检查权限*/
                    chkSession = md.chkSession;/*是否检查session*/
                }
            }
            #endregion
            #region 类型验证
            var modelState = actionContext.ModelState;
            if (!modelState.IsValid)
            {
                string error = string.Empty;
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        error = state.Errors.First().ErrorMessage;
                        break;
                    }
                }
                APIRst api = new APIRst() { rst = false };
                api.err.code = (int)ResultCodeDefine.Auth_ParamsInvalid;
                api.err.msg = "参数错误:" + error;
                actionContext.Response = new HttpResponseMessage { Content = new StringContent(JsonHelper.Serialize(api), Encoding.GetEncoding("UTF-8"), "application/json") };
                AddHeadersOrigin(actionContext.Request, actionContext.Response);
                base.OnActionExecuting(actionContext);
                return;
            }
            #endregion
            #region Session验证
            if (chkSession == true && user == null)
            {
                APIRst api = new APIRst() { rst = false };
                if (string.IsNullOrEmpty(msg))
                {
                    api.err.code = (int)ResultCodeDefine.Error_LoginInvalid;
                    api.err.msg = "登录过期";
                }
                else
                {
                    api.err.code = (int)ResultCodeDefine.Auth_TicketInvalid;
                    api.err.msg = msg;
                }
                actionContext.Response = new HttpResponseMessage { Content = new StringContent(JsonHelper.Serialize(api), Encoding.GetEncoding("UTF-8"), "application/json") };
                AddHeadersOrigin(actionContext.Request, actionContext.Response);
                base.OnActionExecuting(actionContext);
                return;
            }            

            #endregion
            #region 权限验证    
            if (authorize == true)
            {//检查权限
                if (user == null)
                {
                    APIRst api = new APIRst() { rst = false };
                    api.err.code = (int)ResultCodeDefine.Auth_UserNoPermission;
                    api.err.msg = "没有权限:原因登录过期";
                    actionContext.Response = new HttpResponseMessage { Content = new StringContent(JsonHelper.Serialize(api), Encoding.GetEncoding("UTF-8"), "application/json") };
                    AddHeadersOrigin(actionContext.Request, actionContext.Response);
                    base.OnActionExecuting(actionContext);
                    return;
                }
                YDS6000.BLL.Platform.Home.HomeBLL hBll = new BLL.Platform.Home.HomeBLL(user.Ledger, user.Uid);
                bool power = hBll.GetPower(user.Role_id, prog_id);
                if (power == false)
                {
                    APIRst api = new APIRst() { rst = false };
                    api.err.code = (int)ResultCodeDefine.Auth_UserNoPermission;
                    api.err.msg = "没有权限";
                    actionContext.Response = new HttpResponseMessage { Content = new StringContent(JsonHelper.Serialize(api), Encoding.GetEncoding("UTF-8"), "application/json") };
                    AddHeadersOrigin(actionContext.Request, actionContext.Response);
                }
            }

            bool isAddLog = string.IsNullOrEmpty(content) ? false : true;
            if (isAddLog == true)
            {
                try
                {
                    //YDS6000.BLL.WholeBLL.AddLog(user.Ledger, user.Uid, prog_id, userHostAddress, controllerName, actionName, content);
                }
                catch (Exception ex)
                {
                    FileLog.WriteLog("增加访问记录错误:", ex.Message);
                }
            }
            #endregion
            base.OnActionExecuting(actionContext);
        }
        /// <summary>
        /// 跨域处理
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            AddHeadersOrigin(actionExecutedContext.Request, actionExecutedContext.Response);
            base.OnActionExecuted(actionExecutedContext);
        }

        private void AddHeadersOrigin(HttpRequestMessage request, HttpResponseMessage response)
        {
            //if (request.Headers != null && request.Headers.Contains("Origin"))
            //    response.Headers.Add("Access-Control-Allow-Origin", HttpUtility.UrlDecode(request.Headers.GetValues("Origin").FirstOrDefault()));
            //else
            //    response.Headers.Add("Access-Control-Allow-Origin", "*");
            if (request.Headers != null && request.Headers.Contains("Ticket"))
                response.Headers.Add("Ticket", HttpUtility.UrlDecode(request.Headers.GetValues("Ticket").FirstOrDefault()));
        }
    }

    /// <summary>
    /// 自动启动服务
    /// </summary>
    internal class Automatic
    {
        private static object only = new object();
        private static string processName = "YDS6000.DataProcess";
        private static string processPath = "";
        private static string project = "";

        /// <summary>
        /// 检查是否有启动自动服务
        /// </summary>
        /// <returns></returns>
        private static int Check()
        {
            var prs = System.Diagnostics.Process.GetProcessesByName(processName);
            int pCc = prs.Count();
            return pCc;
        }
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns></returns>
        internal static bool Start()
        {
            if (Check() > 0) return true;
            int start = CommFunc.ConvertDBNullToInt32(YDS6000.Models.IniHepler.GetConfig(WebConfig.LpFileName, "Automatic", "Start"));
            if (start != 1) return false;/*配置不启动*/

            if (string.IsNullOrEmpty(processPath))
                processPath = AppDomain.CurrentDomain.BaseDirectory + @"Automatic\" + processName + ".exe"; //HttpContext.Current.Request.PhysicalApplicationPath
            if (string.IsNullOrEmpty(project))
                project = YDS6000.Models.IniHepler.GetConfig(WebConfig.LpFileName, "Automatic", "Project");

            if (System.IO.File.Exists(processName))
            {
                FileLog.WriteLog("自动启动的服务不存在");
                return false;
            }
            lock (only)
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName = processPath;
                psi.Arguments = project;
                System.Diagnostics.Process.Start(psi);
            }
            return true;
        }
    }
}