using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages
{
    public class PotwierdzenieZamowieniaModel : PageModel
    {
        private readonly MyDbContext _context;
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<Product> PromotionalProducts { get; set; }

        public PotwierdzenieZamowieniaModel(MyDbContext context)
        {
            _context = context;
        }

        public void OnGet(int orderId)
        {
            PromotionalProducts = _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToList();

            Order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (Order != null)
            {
                OrderItems = _context.OrderItems.Where(oi => oi.OrderId == orderId).Include(oi => oi.Product).ToList();
            }
        }

    }
}
