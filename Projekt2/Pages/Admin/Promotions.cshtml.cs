using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages.Admin
{
    public class PromotionsModel : PageModel
    {
        private readonly MyDbContext _context;
        public List<Product> Products { get; set; }

        [BindProperty]
        public int? SelectedProductId { get; set; }

        [BindProperty]
        public int DiscountPercent { get; set; }

        public PromotionsModel(MyDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Products = await _context.Products.Include(p => p.Promotion).ToListAsync();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            var product = await _context.Products.FindAsync(SelectedProductId);
            if (product != null && product.Promotion == null)
            {
                product.Promotion = new Promotion
                {
                    DiscountPercent = DiscountPercent
                };
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int promotionId)
        {
            var promotion = await _context.Promotions.FindAsync(promotionId);

            if (promotion != null)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Promotion == promotion);

                if (product != null)
                {
                    product.Promotion = null;
                }

                _context.Promotions.Remove(promotion);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

    }
}
