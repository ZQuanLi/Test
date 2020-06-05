using System.Web.Mvc;

namespace YDS6000.WebApi.Areas.Exp
{
    public class IFSMgrAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Exp";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Exp_default",
                "Exp/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}