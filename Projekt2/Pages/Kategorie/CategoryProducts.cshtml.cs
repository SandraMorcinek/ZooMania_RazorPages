using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages.Kategorie
{
    public class CategoryProductsModel : PageModel
    {
        private readonly MyDbContext _context;

        public Category Category { get; set; }
        public IList<Product> Products { get; set; }
        public List<Product> TopRatedProducts { get; set; }
        public List<Product> PromotionalProducts { get; set; }

        public CategoryProductsModel(MyDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(int categoryId)
        {
            PromotionalProducts = await _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToListAsync();
            Category = await _context.Categories.FindAsync(categoryId);

            if (Category != null)
            {
                Products = await _context.Products.Where(p => p.CategoryId == Category.Id).ToListAsync();
            }
            else
            {
                RedirectToPage("/Index"); // je¿eli nie mo¿na znaleŸæ kategorii
            }

            var productRatings = _context.Opinie.GroupBy(o => o.ProductId).Select(g => new
            {
                ProductId = g.Key,
                AverageRating = g.Average(o => o.Rating)
            })
            .OrderByDescending(p => p.AverageRating)
            .Take(3)
            .ToList();

            TopRatedProducts = new List<Product>();

            foreach (var productRating in productRatings)
            {
                var product = await _context.Products.FindAsync(productRating.ProductId);
                if (product != null)
                {
                    product.AverageRating = productRating.AverageRating;
                    TopRatedProducts.Add(product);
                }
            }
        }
    }
}
