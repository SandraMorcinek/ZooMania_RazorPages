using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages.Admin
{
    public class ZamowieniaAdminModel : PageModel
    {
        private readonly MyDbContext _context;

        public List<Order> Orders { get; set; }

        public ZamowieniaAdminModel(MyDbContext context)
        {
            _context = context;

            Orders = new List<Order>();
        }

        public void OnGet()
        {
            Orders = _context.Orders.Include(o => o.Client).Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ToList();
        }

        public IActionResult OnPostChangeStatus(int orderId, string newStatus)
        {
            var order = _context.Orders.Include(o => o.Client).Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.Status = newStatus;
                _context.Orders.Update(order);
                _context.SaveChanges();

                OnGet();  // Wywo³aj metodê OnGet ponownie
                return RedirectToPage();
            }
            return NotFound();
        }

    }
}
