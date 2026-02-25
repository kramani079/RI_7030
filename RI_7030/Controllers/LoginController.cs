using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RI_7030.Controllers
{
    public class LoginController : Controller
    {
        // GET: /Login  → Show login form
        public ActionResult Index()
        {
            // If already logged in, go straight to Home
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("IsLoggedIn")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: /Login  → Handle login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Please enter both Email ID and Password.";
                return View();
            }

            // ── Static credentials (replace with DB lookup later) ────────────
            // Admin login
            if (email == "admin@ri.com" && password == "admin123")
            {
                HttpContext.Session.SetString("IsLoggedIn", "true");
                HttpContext.Session.SetString("Role",       "Admin");
                HttpContext.Session.SetString("Name",       "Admin User");
                HttpContext.Session.SetString("Email",      email);
                return RedirectToAction("Index", "Home");
            }

            // Employee login (any employee — static example)
            if (email == "rajesh.kumar@ri.com" && password == "emp123")
            {
                HttpContext.Session.SetString("IsLoggedIn", "true");
                HttpContext.Session.SetString("Role",       "Employee");
                HttpContext.Session.SetString("Name",       "Rajesh Kumar");
                HttpContext.Session.SetString("Email",      email);
                HttpContext.Session.SetString("EmpType",    "Gold Plating");
                HttpContext.Session.SetString("EmpId",      "EMP-003");
                HttpContext.Session.SetString("Phone",      "+91 98765 43210");
                HttpContext.Session.SetString("Salary",     "₹25,000 / month");
                HttpContext.Session.SetString("Joining",    "12 Jan 2024");
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Invalid email or password. Please try again.";
            return View();
        }

        // GET: /Login/Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        // GET: /Login/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Login/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(string fullName, string email, string password,
                                      string confirmPassword, string accountType, string employeeType)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ViewBag.ErrorMessage = "Please fill in all required fields.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match. Please try again.";
                return View();
            }

            if (string.IsNullOrWhiteSpace(accountType))
            {
                ViewBag.ErrorMessage = "Please select an account type (Admin or Employee).";
                return View();
            }

            // TODO: Save user to database in Stage 3.
            // For now, redirect to Login with a success message.
            TempData["RegisterSuccess"] = $"Registration successful! Welcome, {fullName}. Please log in.";
            return RedirectToAction("Index", "Login");
        }
    }
}
