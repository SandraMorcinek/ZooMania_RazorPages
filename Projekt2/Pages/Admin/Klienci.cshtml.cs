using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages.Admin
{
    public class KlienciModel : PageModel
    {
        private readonly MyDbContext _context;

        public List<Product> PromotionalProducts { get; set; }

        public KlienciModel(MyDbContext context)
        {
            _context = context;
        }

        public List<Client> Clients { get; set; }

        public void OnGet()
        {
            Clients = _context.Clients.ToList();
            PromotionalProducts = _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToList();
        }
    }
}
