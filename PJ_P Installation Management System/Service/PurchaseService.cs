// Services/PurchaseService.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PJ_P_Installation_Management_System.Data;
using PJ_P_Installation_Management_System.Models;
using PJ_P_Installation_Management_System.Service;

namespace PJ_P_Installation_Management_System.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly PJInstallationDbContext _context;
        private readonly ILogger<PurchaseService> _logger;

        public PurchaseService(PJInstallationDbContext context, ILogger<PurchaseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> CreatePurchaseAsync(Purchase purchase)
        {
            try
            {
                // Verify referenced entities exist
                var productExists = await _context.Products.AnyAsync(p => p.ProductId == purchase.ProductId);
                var supplierExists = await _context.Suppliers.AnyAsync(s => s.SupplierId == purchase.SupplierId);

                if (!productExists || !supplierExists)
                {
                    _logger.LogWarning($"Invalid references - Product: {productExists}, Supplier: {supplierExists}");
                    return false;
                }

                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving purchase");
                return false;
            }
        }
    }
}