using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RI_7030.Helpers;

namespace RI_7030.Controllers
{
    /// <summary>
    /// All controllers (except Login) inherit from this.
    /// Provides session auth guard, role helper properties, and i18n.
    /// </summary>
    public class BaseController : Controller
    {
        protected string CurrentRole    => HttpContext.Session.GetString("Role")  ?? "";
        protected string CurrentName    => HttpContext.Session.GetString("Name")  ?? "";
        protected string CurrentEmail   => HttpContext.Session.GetString("Email") ?? "";
        protected int    CurrentUserId  => int.TryParse(HttpContext.Session.GetString("UserId"), out var id) ? id : 0;
        protected string CurrentEmpId   => HttpContext.Session.GetString("EmpId") ?? "";
        protected bool   IsAdmin        => CurrentRole == "Admin";
        protected string CurrentLang    => HttpContext.Session.GetString("Lang") ?? "en";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isLoggedIn = context.HttpContext.Session.GetString("IsLoggedIn");
            if (string.IsNullOrEmpty(isLoggedIn))
            {
                context.Result = RedirectToAction("Index", "Login");
                return;
            }

            // Inject translations into every view
            var lang = context.HttpContext.Session.GetString("Lang") ?? "en";
            ViewBag.T    = Translations.For(lang);
            ViewBag.Lang = lang;

            base.OnActionExecuting(context);
        }

        /// <summary>Returns a 403 redirect if the current user is not Admin.</summary>
        protected IActionResult? RequireAdmin()
        {
            if (!IsAdmin)
                return RedirectToAction("Index", "Home");
            return null;
        }
    }
}
