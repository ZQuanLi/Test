using System.Web.Mvc;

namespace YDS6000.WebApi.Areas.IFSMgr
{
    public class IFSMgrAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "IFSMgr";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "IFSMgr_default",
                "IFSMgr/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}