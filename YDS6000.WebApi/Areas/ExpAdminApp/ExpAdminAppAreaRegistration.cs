using System.Web.Mvc;

namespace YDS6000.WebApi.Areas.ExpAdminApp
{
    public class ExpAdminAppAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ExpAdminApp";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ExpAdminApp_default",
                "ExpAdminApp/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}