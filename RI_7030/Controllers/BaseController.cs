using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RI_7030.Controllers
{
    /// <summary>
    /// All controllers (except Login) should inherit from this.
    /// Redirects unauthenticated users to the Login page.
    /// </summary>
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isLoggedIn = context.HttpContext.Session.GetString("IsLoggedIn");
            if (string.IsNullOrEmpty(isLoggedIn))
            {
                context.Result = RedirectToAction("Index", "Login");
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
