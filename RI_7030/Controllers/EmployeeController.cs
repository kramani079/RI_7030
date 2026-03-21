using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RI_7030.Data;
using RI_7030.Models;

namespace RI_7030.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly AppDbContext _db;

        public EmployeeController(AppDbContext db) { _db = db; }

        // ── GET /Employee (Admin only) ───────────────────────────────────────
        public async Task<IActionResult> Index(string? search)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            var query = _db.Employees.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                query = query.Where(e =>
                    e.FullName.Contains(search) ||
                    e.Email.Contains(search)    ||
                    (e.Department != null && e.Department.Contains(search)));

            var employees = await query.OrderBy(e => e.EmployeeId).ToListAsync();

            ViewBag.Search = search ?? "";
            ViewBag.NextId = await GenerateEmployeeId();

            return View(employees);
        }

        // ── POST /Employee/Create ────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string fullName, string email,
                                                string? department, decimal salary)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            // Check duplicate email
            if (await _db.Employees.AnyAsync(e => e.Email == email.Trim().ToLower()))
            {
                TempData["Error"] = "An employee with this email already exists.";
                return RedirectToAction(nameof(Index));
            }

            var emp = new Employee
            {
                EmployeeId = await GenerateEmployeeId(),
                FullName   = fullName.Trim(),
                Email      = email.Trim().ToLower(),
                Department = department,
                Salary     = salary,
                CreatedAt  = DateTime.Now
            };

            _db.Employees.Add(emp);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Employee {emp.EmployeeId} ({fullName}) added.";
            return RedirectToAction(nameof(Index));
        }

        // ── POST /Employee/Edit/{id} ─────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string fullName, string email,
                                              string? department, decimal salary)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            var emp = await _db.Employees.FindAsync(id);
            if (emp == null) return NotFound();

            emp.FullName   = fullName.Trim();
            emp.Email      = email.Trim().ToLower();
            emp.Department = department;
            emp.Salary     = salary;

            await _db.SaveChangesAsync();
            TempData["Success"] = $"Employee {id} updated.";
            return RedirectToAction(nameof(Index));
        }

        // ── POST /Employee/Delete/{id} ───────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            var emp = await _db.Employees.FindAsync(id);
            if (emp != null)
            {
                _db.Employees.Remove(emp);
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Employee {id} removed.";
            }
            return RedirectToAction(nameof(Index));
        }

        // ── POST /Employee/PaySalary/{id} ────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaySalary(string id, decimal amount, string? paymentMethod)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            var emp = await _db.Employees.FindAsync(id);
            if (emp == null) return NotFound();

            // Create Buy transaction for salary
            var txId = await GenerateTransactionId();
            var tx = new Transaction
            {
                TransactionId   = txId,
                Type            = "Buy",
                PartyName       = emp.FullName,
                Quantity        = 1,
                Amount          = amount,
                Status          = "Paid",
                Category        = "EMP_SALARY",
                PaymentMethod   = paymentMethod ?? "Bank Transfer",
                TransactionDate = DateTime.Now
            };

            _db.Transactions.Add(tx);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Salary of ₹{amount:N0} paid to {emp.FullName} (Tx: {txId}).";
            return RedirectToAction(nameof(Index));
        }

        // ── Helpers ──────────────────────────────────────────────────────────
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

        private async Task<string> GenerateTransactionId()
        {
            var ids = await _db.Transactions
                .Where(t => t.TransactionId.StartsWith("RI_3"))
                .Select(t => t.TransactionId)
                .ToListAsync();

            int maxNum = ids
                .Select(id => int.TryParse(id.Replace("RI_3", ""), out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            return $"RI_3{(maxNum + 1):000}";
        }
    }
}
