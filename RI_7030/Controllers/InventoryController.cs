using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RI_7030.Data;
using RI_7030.Models;

namespace RI_7030.Controllers
{
    public class InventoryController : BaseController
    {
        private readonly AppDbContext _db;

        public InventoryController(AppDbContext db) { _db = db; }

        // ── GET /Inventory ───────────────────────────────────────────────────
        public async Task<IActionResult> Index(string? search)
        {
            var query = _db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p =>
                    p.ProductId.Contains(search) ||
                    p.Name.Contains(search));

            var products = await query.OrderBy(p => p.ProductId).ToListAsync();

            ViewBag.Search = search ?? "";
            ViewBag.NextId = await GenerateProductId();

            return View(products);
        }

        // ── POST /Inventory/Create ───────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, int stock, decimal unitCost)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            var product = new Product
            {
                ProductId = await GenerateProductId(),
                Name      = name.Trim(),
                Stock     = stock,
                UnitCost  = unitCost,
                LowStock  = stock < 15
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Product {product.ProductId} added.";
            return RedirectToAction(nameof(Index));
        }

        // ── POST /Inventory/Edit/{id} ────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string name, int stock, decimal unitCost,
                                              bool stageCasting, bool stageFinishing,
                                              bool stageGoldPlating, bool stagePackaging)
        {
            // Admin or Employee can edit
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Name            = name.Trim();
            product.Stock           = stock;
            product.UnitCost        = unitCost;
            product.LowStock        = stock < 15;
            product.Stage_Casting   = stageCasting;
            product.Stage_Finishing = stageFinishing;
            product.Stage_GoldPlating = stageGoldPlating;
            product.Stage_Packaging = stagePackaging;

            await _db.SaveChangesAsync();
            TempData["Success"] = $"Product {id} updated.";
            return RedirectToAction(nameof(Index));
        }

        // ── POST /Inventory/Delete/{id} ──────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (RequireAdmin() is { } redirect) return redirect;

            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Product {id} deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        // ── Helper ───────────────────────────────────────────────────────────
        private async Task<string> GenerateProductId()
        {
            var ids = await _db.Products
                .Where(p => p.ProductId.StartsWith("RI_1"))
                .Select(p => p.ProductId)
                .ToListAsync();

            int maxNum = ids
                .Select(id => int.TryParse(id.Replace("RI_1", ""), out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();

            return $"RI_1{(maxNum + 1):000}";
        }
    }
}
