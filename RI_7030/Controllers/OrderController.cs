using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RI_7030.Data;
using RI_7030.Models;

namespace RI_7030.Controllers
{
    public class OrderController : BaseController
    {
        private readonly AppDbContext _db;

        public OrderController(AppDbContext db) { _db = db; }

        // ── GET /Order ───────────────────────────────────────────────────────
        public async Task<IActionResult> Index(string? filter, string? search)
        {
            var query = _db.Orders.AsQueryable();

            // Status filter
            if (!string.IsNullOrEmpty(filter) && filter != "All")
                query = query.Where(o => o.Status == filter);

            // Search
            if (!string.IsNullOrEmpty(search))
                query = query.Where(o =>
                    o.OrderId.Contains(search) ||
                    o.CustomerName.Contains(search) ||
                    (o.ProductName != null && o.ProductName.Contains(search)));

            var orders = await query.OrderByDescending(o => o.CreatedAt).ToListAsync();

            // Products for create modal
            ViewBag.Products = await _db.Products.OrderBy(p => p.Name).ToListAsync();
            ViewBag.Filter   = filter ?? "All";
            ViewBag.Search   = search ?? "";
            ViewBag.NextId   = await GenerateOrderId();

            return View(orders);
        }

        // ── POST /Order/Create ───────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string customerId, string email, string productId,
                                                decimal unitPrice, int quantity, DateTime dueDate)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            var product = await _db.Products.FindAsync(productId);

            var order = new Order
            {
                OrderId      = await GenerateOrderId(),
                CustomerName = customerId.Trim(),
                Email        = email,
                ProductId    = productId,
                ProductName  = product?.Name,
                UnitPrice    = unitPrice,
                Quantity     = quantity,
                TotalAmount  = unitPrice * quantity,
                DueDate      = dueDate,
                Status       = "Pending",
                CreatedAt    = DateTime.Now
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Order {order.OrderId} created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ── POST /Order/Edit/{id} ────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string customerName, string? email,
                                              string? productId, decimal unitPrice, int quantity,
                                              DateTime dueDate, string status,
                                              bool stageC, bool stageF, bool stageG, bool stageP)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();

            var product = productId != null ? await _db.Products.FindAsync(productId) : null;

            order.CustomerName = customerName.Trim();
            order.Email        = email;
            order.ProductId    = productId;
            order.ProductName  = product?.Name ?? order.ProductName;
            order.UnitPrice    = unitPrice;
            order.Quantity     = quantity;
            order.TotalAmount  = unitPrice * quantity;
            order.DueDate      = dueDate;
            order.Status       = status;
            order.Stage_C      = stageC;
            order.Stage_F      = stageF;
            order.Stage_G      = stageG;
            order.Stage_P      = stageP;

            await _db.SaveChangesAsync();
            TempData["Success"] = $"Order {id} updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ── POST /Order/Dispatch/{id} ────────────────────────────────────────
        /// <summary>Marks order Delivered, deducts stock, auto-creates Sell transaction.</summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dispatch(string id)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();

            if (order.Status == "Delivered")
            {
                TempData["Error"] = "Order already marked as Delivered.";
                return RedirectToAction(nameof(Index));
            }

            // Validate stock
            if (!string.IsNullOrEmpty(order.ProductId))
            {
                var product = await _db.Products.FindAsync(order.ProductId);
                if (product != null)
                {
                    if (product.Stock < order.Quantity)
                    {
                        TempData["Error"] = $"Insufficient stock. Available: {product.Stock}, Required: {order.Quantity}.";
                        return RedirectToAction(nameof(Index));
                    }

                    product.Stock  -= order.Quantity;
                    product.LowStock = product.Stock < 15;
                }
            }

            // Mark delivered
            order.Status = "Delivered";

            // Auto-create Sell transaction
            var txId = await GenerateTransactionId();
            var tx = new Transaction
            {
                TransactionId   = txId,
                Type            = "Sell",
                PartyName       = order.CustomerName,
                ProductId       = order.ProductId,
                ProductName     = order.ProductName,
                Quantity        = order.Quantity,
                Amount          = order.TotalAmount,
                Status          = "Received",
                Category        = "PRODUCT",
                TransactionDate = DateTime.Now
            };
            _db.Transactions.Add(tx);

            await _db.SaveChangesAsync();
            TempData["Success"] = $"Order {id} dispatched → Sell transaction {txId} created.";
            return RedirectToAction(nameof(Index));
        }

        // ── POST /Order/Delete/{id} ──────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            var order = await _db.Orders.FindAsync(id);
            if (order != null)
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Order {id} deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        // ── Helpers ──────────────────────────────────────────────────────────
        private async Task<string> GenerateOrderId()
        {
            var ids = await _db.Orders
                .Where(o => o.OrderId.StartsWith("RI_2"))
                .Select(o => o.OrderId)
                .ToListAsync();

            int maxNum = ids
                .Select(id => int.TryParse(id.Replace("RI_2", ""), out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            return $"RI_2{(maxNum + 1):000}";
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
