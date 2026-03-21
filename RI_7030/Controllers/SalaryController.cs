using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RI_7030.Data;
using RI_7030.Models;

namespace RI_7030.Controllers
{
    public class SalaryController : BaseController
    {
        private readonly AppDbContext _db;

        public SalaryController(AppDbContext db) { _db = db; }

        // ── GET /Salary ───────────────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            if (IsAdmin)
            {
                // Admin: pending advance requests
                var pendingAdvances = await _db.Transactions
                    .Where(t => t.Category == "EMP_ADVANCE" && t.Status == "Pending")
                    .OrderByDescending(t => t.TransactionDate)
                    .ToListAsync();

                ViewBag.PendingAdvances = pendingAdvances;
            }
            else
            {
                // Employee: own salary history
                var empName = CurrentName;
                var salaryHistory = await _db.Transactions
                    .Where(t => (t.Category == "EMP_SALARY" || t.Category == "EMP_ADVANCE")
                             && t.PartyName == empName)
                    .OrderByDescending(t => t.TransactionDate)
                    .ToListAsync();

                ViewBag.SalaryHistory = salaryHistory;
                ViewBag.NextTxId      = await GenerateTransactionId();
            }

            return View();
        }

        // ── POST /Salary/RequestAdvance (Employee) ────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestAdvance(decimal amount, string? reason)
        {
            if (IsAdmin)
            {
                TempData["Error"] = "Admins cannot request advances.";
                return RedirectToAction(nameof(Index));
            }

            var tx = new Transaction
            {
                TransactionId   = await GenerateTransactionId(),
                Type            = "Buy",
                PartyName       = CurrentName,
                Quantity        = 1,
                Amount          = amount,
                Status          = "Pending",
                Category        = "EMP_ADVANCE",
                PaymentMethod   = "Pending",
                TransactionDate = DateTime.Now
            };

            _db.Transactions.Add(tx);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Advance request of ₹{amount:N0} submitted.";
            return RedirectToAction(nameof(Index));
        }

        // ── POST /Salary/PayAdvance/{id} (Admin) ─────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayAdvance(string id)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            var tx = await _db.Transactions.FindAsync(id);
            if (tx == null) return NotFound();

            tx.Status        = "Paid";
            tx.PaymentMethod = "Bank Transfer";
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Advance {id} marked as Paid.";
            return RedirectToAction(nameof(Index));
        }

        // ── Helper ────────────────────────────────────────────────────────────
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
