using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages
{
    public class ProductsViewModel : PageModel
    {
		private readonly MyDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

        public List<Product> Products { get; set; }

        [BindProperty]
        public Product ProductsView { get; set; }

        public List<Category> Categories { get; set; }
        public List<Product> TopRatedProducts { get; set; }
        public List<Product> PromotionalProducts { get; set; }

        public string NoProductsMessage { get; set; }

        public ProductsViewModel(MyDbContext dbContext, IWebHostEnvironment webHostEnvironment)
		{
			_context = dbContext;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task OnGetAsync()
        {
            Categories = _context.Categories.ToList();
            Products = _context.Products.ToList();
            PromotionalProducts = await _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToListAsync();

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

        public void OnPost()
        {
            PromotionalProducts = _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToList();

            if (ProductsView.CategoryId == 0) // Jeœli wybrano kategoriê "Wszystkie"
            {
                Products = _context.Products.ToList();
            }
            else
            {
                Products = _context.Products.Where(p => p.CategoryId == ProductsView.CategoryId).ToList();
            }

            // Obliczanie œredniej oceny dla wybranych produktów
            var productRatings = _context.Opinie
                .Where(o => Products.Select(p => p.Id).Contains(o.ProductId)) // Filter reviews for the selected products
                .GroupBy(o => o.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    AverageRating = g.Average(o => o.Rating)
                })
                .ToList();

            // Uaktualnij œrednie oceny w liœcie produktów
            foreach (var product in Products)
            {
                var productRating = productRatings.FirstOrDefault(pr => pr.ProductId == product.Id);
                if (productRating != null)
                {
                    product.AverageRating = productRating.AverageRating;
                }
            }

            Categories = _context.Categories.ToList();

            if (Products.Count == 0)
            {
                NoProductsMessage = "Brak produktów";
            }
            else
            {
                NoProductsMessage = "";
            }
        }


    }
}
