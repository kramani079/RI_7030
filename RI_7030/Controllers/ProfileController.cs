using Microsoft.AspNetCore.Mvc;

namespace RI_7030.Controllers
{
    public class ProfileController : BaseController
    {
        public IActionResult Index()
        {
            // Pass session values into ViewBag for the profile view
            ViewBag.Role       = HttpContext.Session.GetString("Role")    ?? "Employee";
            ViewBag.Name       = HttpContext.Session.GetString("Name")    ?? "";
            ViewBag.Email      = HttpContext.Session.GetString("Email")   ?? "";
            ViewBag.Phone      = HttpContext.Session.GetString("Phone")   ?? "—";
            ViewBag.EmpType    = HttpContext.Session.GetString("EmpType") ?? "—";
            ViewBag.EmpId      = HttpContext.Session.GetString("EmpId")   ?? "—";
            ViewBag.Salary     = HttpContext.Session.GetString("Salary")  ?? "—";
            ViewBag.Joining    = HttpContext.Session.GetString("Joining") ?? "—";
            ViewBag.Department = "Production";
            return View();
        }
    }
}
