using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;
using System.Security.Claims;

namespace Projekt2.Pages.Klient
{
    public class MyAccountModel : PageModel
    {
        private readonly MyDbContext _context;
        public Client Client { get; set; }
        public List<Order> Orders { get; set; }
        public List<Product> PromotionalProducts { get; set; }

        public MyAccountModel(MyDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            PromotionalProducts = _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToList();

            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Klient/Login");
                return;
            }

            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            Client = _context.Clients.FirstOrDefault(c => c.Name == name);

            if (Client != null)
            {
                Orders = _context.Orders.Where(o => o.Client.Id == Client.Id).ToList();
            }
        }
    }
}
