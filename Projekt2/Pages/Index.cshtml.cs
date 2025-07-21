using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt2.Models;
using System.Text;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;


namespace Projekt2.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public List<Product> Products { get; set; }
        public List<Product> TopRatedProducts { get; set; }
        public List<Product> PromotionalProducts { get; set; }

        public IndexModel(MyDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _context = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        //private readonly ILogger<IndexModel> _logger;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}

        public async Task OnGetAsync()
        {
            Products = await _context.Products.ToListAsync();

            PromotionalProducts = await _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToListAsync();

            var productRatings = _context.Opinie.GroupBy(o => o.ProductId).Select(g => new
            {
                ProductId = g.Key,
                AverageRating = g.Average(o => o.Rating)
            }).OrderByDescending(p => p.AverageRating).Take(3).ToList();

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

