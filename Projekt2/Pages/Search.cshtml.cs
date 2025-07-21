using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages
{
    public class SearchModel : PageModel
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SearchModel(MyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<Product> Products { get; set; }
        public List<Product> PromotionalProducts { get; set; }

        public IActionResult OnGet(string searchQuery)
        {
            PromotionalProducts = _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToList();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                // Wyszukaj produkty na podstawie wpisanego tekstu (ignoruj¹c wielkoœæ liter)
                Products = _context.Products.Where(p =>
                    p.Name.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Description.ToLower().Contains(searchQuery.ToLower())).ToList();
            }
            else
            {
                Products = new List<Product>();
            }

            if (Products.Any())
            {
                return Page();
            }
            else
            {
                return Page();
            }
        }
    }
}
