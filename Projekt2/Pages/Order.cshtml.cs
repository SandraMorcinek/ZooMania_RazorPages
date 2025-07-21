using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages
{
    public class OrderModel : PageModel
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Order Order { get; set; }
        public List<CartItem> CartItems { get; set; }
        public List<Product> PromotionalProducts { get; set; }

        public OrderModel(MyDbContext dbContext, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            CartItems = new List<CartItem>();
        }

        public void OnGet()
        {
            PromotionalProducts = _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToList();

            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            var client = _context.Clients.FirstOrDefault(c => c.Name == userName);
            if (client != null)
            {
                CartItems = _context.CartItems.Include(c => c.Product).ThenInclude(p => p.Promotion).ToList();
            }
        }

        public IActionResult OnPost(int transport)
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            var client = _context.Clients.FirstOrDefault(c => c.Name == userName);

            if (client != null)
            {
                CartItems = _context.CartItems.Include(c => c.Product).ThenInclude(p => p.Promotion).ToList();

                Order = new Order
                {
                    Client = client,
                    OrderDate = DateTime.Now,
                    Status = "Nowe",
                    Transport = transport,
                };

                _context.Orders.Add(Order);
                _context.SaveChanges(); // zapisz, aby uzyskaæ Id dla nowego zamówienia

                // Tworzenie OrderItems dla ka¿dego CartItem
                foreach (var cartItem in CartItems)
                {
                    var discountedPrice = cartItem.Product.Price;
                    if (cartItem.Product.Promotion != null)
                    {
                        discountedPrice *= (1 - cartItem.Product.Promotion.DiscountPercent / 100m);
                    }

                    var orderItem = new OrderItem
                    {
                        OrderId = Order.Id,
                        ProductId = cartItem.Product.Id,
                        Quantity = cartItem.Quantity,
                        PriceAtPurchase = discountedPrice
                    };

                    _context.OrderItems.Add(orderItem);
                }

                _context.CartItems.RemoveRange(CartItems);  // usuñ przedmioty z koszyka
                _context.SaveChanges();
                return RedirectToPage("/PotwierdzenieZamowienia", new { orderId = Order.Id });
            }
            return Page();
        }
        //
    }
}
