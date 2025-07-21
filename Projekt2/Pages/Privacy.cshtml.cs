using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly MyDbContext _context;

        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<Product> PromotionalProducts { get; set; }

        public void OnGet()
        {
            PromotionalProducts = _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToList();
        }
    }
}