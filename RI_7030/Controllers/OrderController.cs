using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RI_7030.Controllers
{
    public class OrderController : BaseController
    {
        // GET: /Order
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Order/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        // GET: /Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            // TODO: Save to DB in Stage 3
            TempData["OrderSuccess"] = "Order placed successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Order/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        // POST: /Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            TempData["OrderSuccess"] = "Order updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Order/Delete/5
        public ActionResult Delete(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        // POST: /Order/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            TempData["OrderSuccess"] = "Order deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
