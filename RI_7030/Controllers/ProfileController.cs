using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RI_7030.Data;

namespace RI_7030.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly AppDbContext _db;

        public ProfileController(AppDbContext db) { _db = db; }

        // ── GET /Profile ─────────────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var user = await _db.Users.FindAsync(CurrentUserId);
            if (user == null) return RedirectToAction("Logout", "Login");

            // Find linked employee record
            var emp = await _db.Employees.FirstOrDefaultAsync(e => e.UserId == user.UserId);

            ViewBag.User = user;
            ViewBag.Emp  = emp;
            return View(user);
        }

        // ── POST /Profile/Update ─────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string fullName, string? phone, string? bio,
                                                string? country, string? cityState,
                                                string? postalCode, string? taxId, string? address)
        {
            var user = await _db.Users.FindAsync(CurrentUserId);
            if (user == null) return RedirectToAction("Logout", "Login");

            user.FullName   = fullName.Trim();
            user.Phone      = phone;
            user.Bio        = bio;
            user.Country    = country;
            user.CityState  = cityState;
            user.PostalCode = postalCode;
            user.TaxId      = taxId;
            user.Address    = address;

            // Update session name
            HttpContext.Session.SetString("Name", user.FullName);
            if (phone != null) HttpContext.Session.SetString("Phone", phone);

            await _db.SaveChangesAsync();
            TempData["Success"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ── POST /Profile/ChangePassword ─────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword,
                                                        string confirmPassword)
        {
            var user = await _db.Users.FindAsync(CurrentUserId);
            if (user == null) return RedirectToAction("Logout", "Login");

            // Validate current password
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            {
                TempData["PasswordError"] = "Current password is incorrect.";
                return RedirectToAction(nameof(Index));
            }

            if (newPassword.Length < 6)
            {
                TempData["PasswordError"] = "New password must be at least 6 characters.";
                return RedirectToAction(nameof(Index));
            }

            if (newPassword == currentPassword)
            {
                TempData["PasswordError"] = "New password must be different from current password.";
                return RedirectToAction(nameof(Index));
            }

            if (newPassword != confirmPassword)
            {
                TempData["PasswordError"] = "Passwords do not match.";
                return RedirectToAction(nameof(Index));
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Password changed successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ── POST /Profile/SetLanguage ────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetLanguage(string lang)
        {
            HttpContext.Session.SetString("Lang", lang == "gu" ? "gu" : "en");
            // Redirect back to the referring page
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
                return Redirect(referer);
            return RedirectToAction("Index", "Home");
        }
    }
}
