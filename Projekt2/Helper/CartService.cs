using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Helper
{
    public class CartService
    {
        private readonly MyDbContext _context;

        public CartService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetNumberOfItemsAsync()
        {
            return await _context.CartItems.SumAsync(item => item.Quantity);
        }

        public async Task<decimal> GetTotalCostAsync()
        {
            // Pobieramy wszystkie elementy do pamięci
            var cartItems = await _context.CartItems.Include(item => item.Product).ToListAsync();

            // Wykonujemy operację sumowania po stronie klienta
            var totalCost = cartItems.Sum(item => item.Product.Promotion != null
            ? item.Quantity * item.Product.Price * (1 - item.Product.Promotion.DiscountPercent / 100m) // cena promocyjna
            : item.Quantity * item.Product.Price); // cena orginalna

            return Math.Round(totalCost, 2);
        }

    }
}
