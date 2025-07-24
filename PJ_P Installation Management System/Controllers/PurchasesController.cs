using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PJ_P_Installation_Management_System.Data;
using PJ_P_Installation_Management_System.Models;

namespace PJ_P_Installation_Management_System.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly PJInstallationDbContext _context;

        public PurchasesController(PJInstallationDbContext context)
        {
            _context = context;
        }

        // GET: Purchases
        public async Task<IActionResult> Index()
        {
            var purchases = await _context.Purchases
                .Include(p => p.Product)
                .Include(p => p.Supplier)
                .ToListAsync();
            return View(purchases);
        }

        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Include(p => p.Product)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.PurchaseId == id);

            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: Purchases/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name");
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName");
            return View();
        }

        // POST: Purchases/Create
        [HttpPost]
        public async Task<IActionResult> Create(int productId, int supplierId, DateTime purchaseDate, string status)
        {
            try
            {
                var purchase = new Purchase
                {
                    ProductId = productId,
                    SupplierId = supplierId,
                    PurchaseDate = purchaseDate,
                    Status = status
                };

                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }

        // GET: Purchases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", purchase.ProductId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName", purchase.SupplierId);
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id,
            int productId,
            int supplierId,
            DateTime purchaseDate,
            string status)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $@"UPDATE Purchases 
               SET ProductId = {productId}, 
                   SupplierId = {supplierId}, 
                   PurchaseDate = {purchaseDate}, 
                   Status = {status}
               WHERE PurchaseId = {id}");

                TempData["SuccessMessage"] = "Updated!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }

        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchases
                .Include(p => p.Product)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.PurchaseId == id);

            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase != null)
            {
                _context.Purchases.Remove(purchase);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchases.Any(e => e.PurchaseId == id);
        }

    }
}