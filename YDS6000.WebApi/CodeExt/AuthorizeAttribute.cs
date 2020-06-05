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
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //CacheUser user = HttpContext.Current.Session["CacheUser"] as CacheUser;
            #region Session验证
            //if (user == null)
            //{
            //    APIResult api = new APIResult();
            //    api.Code = (int)System.Net.HttpStatusCode.Forbidden;
            //    api.Msg = "登录过期";
            //    actionContext.Response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.Forbidden, Content = new StringContent(JsonHelper.Serialize(api), Encoding.GetEncoding("UTF-8"), "application/json") };
            //    base.OnActionExecuting(actionContext);
            //}
            #endregion

            #region 类型验证
            //var modelState = actionContext.ModelState;
            //if (!modelState.IsValid)
            //{
            //    string error = string.Empty;
            //    foreach (var key in modelState.Keys)
            //    {
            //        var state = modelState[key];
            //        if (state.Errors.Any())
            //        {
            //            error = state.Errors.First().ErrorMessage;
            //            break;
            //        }
            //    }
            //    APIResult api = new APIResult();
            //    api.Code = -2;//数据类型不正确
            //    api.Msg = error;
            //    actionContext.Response = new HttpResponseMessage { Content = new StringContent(JsonHelper.Serialize(api), Encoding.GetEncoding("UTF-8"), "application/json") };
            //    if (actionContext.Request.Headers.Contains("Origin"))
            //    {
            //        actionContext.Response.Headers.Add("Access-Control-Allow-Origin", HttpUtility.UrlDecode(actionContext.Request.Headers.GetValues("Origin").FirstOrDefault()));
            //    }
            //    else
            //    {
            //        actionContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            //    }
            //}
            #endregion
            base.OnActionExecuting(actionContext);
        }
        // 跨域处理
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {

            if (actionExecutedContext.Request.Headers.Contains("Origin"))
            {
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", HttpUtility.UrlDecode(actionExecutedContext.Request.Headers.GetValues("Origin").FirstOrDefault()));
            }
            else
            {
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            }

            if (!actionExecutedContext.Request.Headers.Contains("CacheUser"))
            {
                actionExecutedContext.Response.Headers.Add("CacheUser", "LiucjTest");
            }
            base.OnActionExecuted(actionExecutedContext);
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