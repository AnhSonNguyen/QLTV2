using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QLTV2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var admin = context.HttpContext.Session.GetString("AdminUsername");
            if (string.IsNullOrEmpty(admin))
            {
                context.Result = new RedirectToActionResult("Login", "Accounts", new { area = "Admin" });
            }
            base.OnActionExecuting(context);
        }
    }
}
