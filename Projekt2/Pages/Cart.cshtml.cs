using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages
{
    public class CartModel : PageModel
    {
        private readonly MyDbContext _dbContext;

        public CartModel(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<CartItem> CartItems { get; set; }

        public void OnGet()
        {
            CartItems = _dbContext.CartItems.Include(ci => ci.Product).ThenInclude(p => p.Promotion).ToList();
        }

        public IActionResult OnPost(int itemId, string action)
        {
            var cartItem = _dbContext.CartItems.Include(ci => ci.Product).ThenInclude(p => p.Promotion).Single(ci => ci.Id == itemId);

            switch (action)
            {
                case "increase":
                    cartItem.Quantity++;
                    break;
                case "decrease":
                    if (cartItem.Quantity > 1)
                    {
                        cartItem.Quantity--;
                    }
                    break;
                case "remove":
                    _dbContext.CartItems.Remove(cartItem);
                    break;
            }

            _dbContext.SaveChanges();

            return RedirectToPage();
        }
    }

}
