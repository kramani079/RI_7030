using Microsoft.AspNetCore.Mvc;

namespace RI_7030.Controllers
{
    public class ProfileController : Controller
    {
        // GET: /Profile
        public IActionResult Index()
        {
            return View();
        }
    }
}

