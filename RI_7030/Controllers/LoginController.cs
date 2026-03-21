using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RI_7030.Data;
using RI_7030.Models;

namespace RI_7030.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _db;

        public LoginController(AppDbContext db)
        {
            _db = db;
        }

        // ── GET /Login ──────────────────────────────────────────────────────
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("IsLoggedIn")))
                return RedirectToAction("Index", "Home");

            ViewBag.RegisterSuccess = TempData["RegisterSuccess"]?.ToString();
            ViewBag.LogoutMessage   = TempData["LogoutMessage"]?.ToString();
            return View();
        }

        // ── POST /Login ─────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Please enter both Email and Password.";
                return View();
            }

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email.Trim().ToLower());

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ViewBag.ErrorMessage = "Invalid email or password. Please try again.";
                return View();
            }

            // Find linked employee record
            var emp = await _db.Employees
                .FirstOrDefaultAsync(e => e.UserId == user.UserId);

            // Store session
            HttpContext.Session.SetString("IsLoggedIn", "true");
            HttpContext.Session.SetString("Role",       user.Role);
            HttpContext.Session.SetString("Name",       user.FullName);
            HttpContext.Session.SetString("Email",      user.Email);
            HttpContext.Session.SetString("UserId",     user.UserId.ToString());

            if (emp != null)
            {
                HttpContext.Session.SetString("EmpId",      emp.EmployeeId);
                HttpContext.Session.SetString("EmpType",    emp.Department ?? "");
                HttpContext.Session.SetString("Salary",     emp.Salary.ToString("N0"));
                HttpContext.Session.SetString("Joining",    emp.CreatedAt.ToString("dd MMM yyyy"));
            }

            if (user.Phone != null)
                HttpContext.Session.SetString("Phone", user.Phone);

            return RedirectToAction("Index", "Home");
        }

        // ── GET /Login/Logout ───────────────────────────────────────────────
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["LogoutMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Login");
        }

        // ── GET /Login/Register ─────────────────────────────────────────────
        public IActionResult Register()
        {
            return View();
        }

        // ── POST /Login/Register ────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string fullName, string email, string password,
                                                  string confirmPassword, string accountType, string employeeType)
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ViewBag.ErrorMessage = "Please fill in all required fields.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match.";
                return View();
            }

            if (password.Length < 6)
            {
                ViewBag.ErrorMessage = "Password must be at least 6 characters.";
                return View();
            }

            // Check duplicate email
            var normalizedEmail = email.Trim().ToLower();
            if (await _db.Users.AnyAsync(u => u.Email == normalizedEmail))
            {
                ViewBag.ErrorMessage = "An account with this email already exists.";
                return View();
            }

            var role = (accountType == "Admin") ? "Admin" : "Employee";

            var user = new User
            {
                FullName     = fullName.Trim(),
                Email        = normalizedEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role         = role,
                EmployeeType = string.IsNullOrEmpty(employeeType) ? null : employeeType,
                CreatedAt    = DateTime.Now
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();   // saves user, gets UserId

            // If Employee, auto-create Employee record
            if (role == "Employee")
            {
                var empId = await GenerateEmployeeId();
                var emp = new Employee
                {
                    EmployeeId = empId,
                    FullName   = user.FullName,
                    Email      = user.Email,
                    Department = user.EmployeeType ?? "General",
                    Salary     = 0,
                    UserId     = user.UserId,
                    CreatedAt  = DateTime.Now
                };
                _db.Employees.Add(emp);
                await _db.SaveChangesAsync();
            }

            TempData["RegisterSuccess"] = $"Registration successful! Welcome, {fullName}. Please log in.";
            return RedirectToAction("Index", "Login");
        }

        // ── GET /Login/ForgotPassword ───────────────────────────────────────
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // ── POST /Login/ForgotPassword ──────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.ErrorMessage = "Please enter your email address.";
                return View();
            }

            var normalizedEmail = email.Trim().ToLower();
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);

            // Always show success to prevent email enumeration
            ViewBag.SuccessMessage = "If that email exists in our system, a reset link has been sent.";

            if (user == null) return View();

            // Invalidate old tokens
            var oldTokens = _db.PasswordResetTokens
                .Where(t => t.UserId == user.UserId && !t.IsUsed);
            foreach (var t in oldTokens) t.IsUsed = true;

            var token = new PasswordResetToken
            {
                UserId    = user.UserId,
                Token     = Guid.NewGuid().ToString("N"),
                ExpiresAt = DateTime.Now.AddHours(1),
                IsUsed    = false
            };
            _db.PasswordResetTokens.Add(token);
            await _db.SaveChangesAsync();

            // TODO: Send email with MailKit using token.Token
            // For now — show token in dev mode
            ViewBag.DevToken = token.Token;

            return View();
        }

        // ── GET /Login/ResetPassword?token=xxx ─────────────────────────────
        public async Task<IActionResult> ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Index");

            var resetToken = await _db.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == token && !t.IsUsed && t.ExpiresAt > DateTime.Now);

            if (resetToken == null)
            {
                ViewBag.ErrorMessage = "This reset link is invalid or has expired.";
                return View();
            }

            ViewBag.Token = token;
            return View();
        }

        // ── POST /Login/ResetPassword ───────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string token, string newPassword, string confirmPassword)
        {
            var resetToken = await _db.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == token && !t.IsUsed && t.ExpiresAt > DateTime.Now);

            if (resetToken == null)
            {
                ViewBag.ErrorMessage = "This reset link is invalid or has expired.";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.ErrorMessage = "Passwords do not match.";
                ViewBag.Token = token;
                return View();
            }

            if (newPassword.Length < 6)
            {
                ViewBag.ErrorMessage = "Password must be at least 6 characters.";
                ViewBag.Token = token;
                return View();
            }

            var user = await _db.Users.FindAsync(resetToken.UserId);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found.";
                return View();
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            resetToken.IsUsed = true;
            await _db.SaveChangesAsync();

            TempData["RegisterSuccess"] = "Password reset successfully. Please log in.";
            return RedirectToAction("Index");
        }

        // ── Helper: Auto-generate Employee ID ──────────────────────────────
        private async Task<string> GenerateEmployeeId()
        {
            var ids = await _db.Employees
                .Where(e => e.EmployeeId.StartsWith("RI_4"))
                .Select(e => e.EmployeeId)
                .ToListAsync();

            int maxNum = ids
                .Select(id => int.TryParse(id.Replace("RI_4", ""), out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            return $"RI_4{(maxNum + 1):000}";
        }
    }
}
