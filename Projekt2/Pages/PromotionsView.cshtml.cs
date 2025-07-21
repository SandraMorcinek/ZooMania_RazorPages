using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages
{
    public class PromotionsViewModel : PageModel
    {
        private readonly MyDbContext _context;

        public PromotionsViewModel(MyDbContext context)
        {
            _context = context;
        }

        public List<Product> PromotionalProducts { get; set; }

        public async Task OnGetAsync()
        {
            PromotionalProducts = await _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToListAsync();
        }
    }
}
