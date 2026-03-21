using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RI_7030.Data;
using RI_7030.Models;

namespace RI_7030.Controllers
{
    public class TransactionController : BaseController
    {
        private readonly AppDbContext _db;

        public TransactionController(AppDbContext db) { _db = db; }

        // ── GET /Transaction?tab=buy|sell|history|due ────────────────────────
        public async Task<IActionResult> Index(string tab = "buy", string? search = null)
        {
            ViewBag.Tab  = tab;
            ViewBag.Search = search ?? "";

            // Products for dropdowns
            ViewBag.Products = await _db.Products.OrderBy(p => p.Name).ToListAsync();
            ViewBag.NextId   = await GenerateTransactionId();

            // History tab — all transactions filtered by search
            var historyQuery = _db.Transactions.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                historyQuery = historyQuery.Where(t =>
                    t.TransactionId.Contains(search) ||
                    (t.PartyName   != null && t.PartyName.Contains(search))  ||
                    (t.ProductName != null && t.ProductName.Contains(search)));

            ViewBag.History = await historyQuery
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            // Due Payments (Admin only) — Pending transactions
            if (IsAdmin)
            {
                ViewBag.DuePayments = await _db.Transactions
                    .Where(t => t.Status == "Pending")
                    .OrderBy(t => t.TransactionDate)
                    .ToListAsync();
            }

            return View();
        }

        // ── POST /Transaction/Buy ────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(string partyName, string? productId, int quantity,
                                             decimal amount, string? paymentMethod, string? category)
        {
            var product = !string.IsNullOrEmpty(productId)
                ? await _db.Products.FindAsync(productId) : null;

            // Auto-update inventory for Buy
            if (product != null && quantity > 0)
            {
                product.Stock   += quantity;
                product.LowStock = product.Stock < 15;
            }

            var tx = new Transaction
            {
                TransactionId   = await GenerateTransactionId(),
                Type            = "Buy",
                PartyName       = partyName.Trim(),
                ProductId       = productId,
                ProductName     = product?.Name,
                Quantity        = quantity,
                Amount          = amount,
                Status          = string.IsNullOrEmpty(paymentMethod) ? "Pending" : "Paid",
                Category        = string.IsNullOrEmpty(category) ? "PRODUCT" : category,
                PaymentMethod   = paymentMethod,
                TransactionDate = DateTime.Now
            };

            _db.Transactions.Add(tx);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Buy transaction {tx.TransactionId} recorded.";
            return RedirectToAction("Index", new { tab = "history" });
        }

        // ── POST /Transaction/Sell ───────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sell(string partyName, string? productId, int quantity,
                                              decimal amount, string? paymentMethod)
        {
            var product = !string.IsNullOrEmpty(productId)
                ? await _db.Products.FindAsync(productId) : null;

            // Validate stock
            if (product != null && product.Stock < quantity)
            {
                TempData["Error"] = $"Insufficient stock for {product.Name}. Available: {product.Stock}.";
                return RedirectToAction("Index", new { tab = "sell" });
            }

            // Deduct inventory
            if (product != null)
            {
                product.Stock    -= quantity;
                product.LowStock  = product.Stock < 15;
            }

            var tx = new Transaction
            {
                TransactionId   = await GenerateTransactionId(),
                Type            = "Sell",
                PartyName       = partyName.Trim(),
                ProductId       = productId,
                ProductName     = product?.Name,
                Quantity        = quantity,
                Amount          = amount,
                Status          = string.IsNullOrEmpty(paymentMethod) ? "Pending" : "Received",
                Category        = "PRODUCT",
                PaymentMethod   = paymentMethod,
                TransactionDate = DateTime.Now
            };

            _db.Transactions.Add(tx);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Sell transaction {tx.TransactionId} recorded.";
            return RedirectToAction("Index", new { tab = "history" });
        }

        // ── POST /Transaction/Edit/{id} ──────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string partyName, decimal amount,
                                              string status, string? paymentMethod)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            var tx = await _db.Transactions.FindAsync(id);
            if (tx == null) return NotFound();

            tx.PartyName     = partyName.Trim();
            tx.Amount        = amount;
            tx.Status        = status;
            tx.PaymentMethod = paymentMethod;

            await _db.SaveChangesAsync();
            TempData["Success"] = $"Transaction {id} updated.";
            return RedirectToAction("Index", new { tab = "history" });
        }

        // ── POST /Transaction/PayNow/{id} ────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayNow(string id)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            var tx = await _db.Transactions.FindAsync(id);
            if (tx != null)
            {
                tx.Status = "Paid";
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Transaction {id} marked as Paid.";
            }
            return RedirectToAction("Index", new { tab = "due" });
        }

        // ── POST /Transaction/MarkReceived/{id} ──────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkReceived(string id)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            var tx = await _db.Transactions.FindAsync(id);
            if (tx != null)
            {
                tx.Status = "Received";
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Transaction {id} marked as Received.";
            }
            return RedirectToAction("Index", new { tab = "due" });
        }

        // ── Helper ───────────────────────────────────────────────────────────
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
