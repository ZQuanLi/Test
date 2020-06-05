using System.Web.Mvc;

namespace YDS6000.WebApi.Areas.PDU
{
    public class PDUAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PDU";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PDU_default",
                "PDU/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}