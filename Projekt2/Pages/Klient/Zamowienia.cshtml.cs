using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages.Klient
{
    public class ZamowieniaModel : PageModel
    {
        private readonly MyDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public List<Order> Orders { get; set; }
        public List<Product> PromotionalProducts { get; set; }

        public ZamowieniaModel(MyDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

            Orders = new List<Order>();
        }

        public void OnGet()
        {
            PromotionalProducts = _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToList();

            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            var client = _context.Clients.FirstOrDefault(c => c.Name == userName);

            if (client != null)
            {
                Orders = _context.Orders.Where(o => o.Client.Id == client.Id).ToList();
            }
        }

        public IActionResult OnPostCancelOrder(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order != null)
            {
                order.Status = "Anulowane";
                _context.Orders.Update(order);
                _context.SaveChanges();

                var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
                var client = _context.Clients.FirstOrDefault(c => c.Name == userName);

                if (client != null)
                {
                    Orders = _context.Orders.Where(o => o.Client.Id == client.Id).ToList();
                }

                return RedirectToPage();
            }

            return NotFound();
        }

    }
}
