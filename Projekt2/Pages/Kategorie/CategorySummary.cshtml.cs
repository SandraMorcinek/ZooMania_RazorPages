using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages.Kategorie
{
    public class CategorySummaryModel : PageModel
    {
        private readonly MyDbContext _context;

        public IList<CategoryProductCount> Categories { get; set; }
        public List<Product> PromotionalProducts { get; set; }

        public class CategoryProductCount
        {
            public Category Category { get; set; }
            public int ProductCount { get; set; }
        }

        public CategorySummaryModel(MyDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            PromotionalProducts = await _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToListAsync();

            Categories = await _context.Categories
                .Select(c => new CategoryProductCount
                {
                    Category = c,
                    ProductCount = _context.Products.Count(p => p.CategoryId == c.Id)
                })
                .ToListAsync();
        }
    }
}
