using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RI_7030.Data;
using RI_7030.Models;

namespace RI_7030.Controllers
{
    public class HomeController : BaseController
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            if (IsAdmin)
                return await AdminDashboard();
            else
                return await EmployeeDashboard();
        }

        // ── Admin Dashboard ─────────────────────────────────────────────────
        private async Task<IActionResult> AdminDashboard()
        {
            // Summary cards
            var toReceive = await _db.Transactions
                .Where(t => t.Type == "Sell" && t.Status == "Pending")
                .SumAsync(t => t.Amount);

            var toPay = await _db.Transactions
                .Where(t => t.Type == "Buy" && t.Status == "Pending")
                .SumAsync(t => t.Amount);

            var pendingOrdersCount = await _db.Orders
                .CountAsync(o => o.Status != "Delivered");

            decimal netBalance = toReceive - toPay;

            ViewBag.ToReceive          = toReceive;
            ViewBag.ToPay              = toPay;
            ViewBag.PendingOrdersCount = pendingOrdersCount;
            ViewBag.NetBalance         = netBalance;

            // Pending transactions to receive (Sell, Pending) — top 10 by date
            var paymentsToReceive = await _db.Transactions
                .Where(t => t.Type == "Sell" && t.Status == "Pending")
                .OrderBy(t => t.TransactionDate)
                .Take(10)
                .ToListAsync();

            // Payments to make (Buy, Pending) — top 10
            var paymentsToPay = await _db.Transactions
                .Where(t => t.Type == "Buy" && t.Status == "Pending")
                .OrderBy(t => t.TransactionDate)
                .Take(10)
                .ToListAsync();

            // Pending orders
            var pendingOrders = await _db.Orders
                .Where(o => o.Status != "Delivered")
                .OrderBy(o => o.DueDate)
                .Take(10)
                .ToListAsync();

            ViewBag.PaymentsToReceive = paymentsToReceive;
            ViewBag.PaymentsToPay     = paymentsToPay;
            ViewBag.PendingOrders     = pendingOrders;

            return View("Index");
        }

        // ── Employee Dashboard ───────────────────────────────────────────────
        private async Task<IActionResult> EmployeeDashboard()
        {
            var today = DateTime.Today;

            var todaySales = await _db.Transactions
                .Where(t => t.Type == "Sell" && t.TransactionDate.Date == today)
                .SumAsync(t => t.Amount);

            var lowStockCount = await _db.Products
                .CountAsync(p => p.Stock < 15);

            var recentTransactions = await _db.Transactions
                .OrderByDescending(t => t.TransactionDate)
                .Take(5)
                .ToListAsync();

            ViewBag.TodaySales         = todaySales;
            ViewBag.LowStockCount      = lowStockCount;
            ViewBag.RecentTransactions = recentTransactions;

            return View("EmployeeDashboard");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
