using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages
{
    public class ProductCardModel : PageModel
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Product ProductCard { get; set; }
        public Opinia NewOpinia { get; set; }

        public List<Product> Products { get; set; }
        public List<Opinia> OpinieProduktu { get; set; }

        public ProductCardModel(MyDbContext dbContext, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;

            // Inicjalizacja NewOpinia
            NewOpinia = new Opinia();
        }

        public void OnGet()
        {
            ProductCard = _context.Products.Include(p => p.Promotion).SingleOrDefault(p => p.Id == Id);

            var categoryId = ProductCard.CategoryId;

            Products = _context.Products.Include(p => p.Promotion).Where(p => p.CategoryId == categoryId && p.Id != Id).Take(4).ToList();

            // Pobierz nazwê kategorii
            string categoryName = string.Empty;
            if (categoryId != 0)
            {
                var category = _context.Categories.Find(categoryId);
                categoryName = category?.Name;
            }

            // Przeka¿ zmienne do widoku jako czêœæ modelu
            ViewData["CategoryName"] = categoryName;


            // Pobierz opinie dla wybranego produktu
            OpinieProduktu = _context.Opinie
                .Include(o => o.Client) // £aduj klientów dla opinii
                .Where(o => o.ProductId == Id)
                .ToList();

            // Ustaw klientów dla opinii
            foreach (var opinia in OpinieProduktu)
            {
                opinia.Client = _context.Clients.FirstOrDefault(c => c.Id == opinia.ClientId);
            }
        }

        public IActionResult OnPost(Opinia newOpinia)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // U¿ytkownik nie jest zalogowany, obs³u¿ b³¹d lub przekieruj do strony logowania
                return RedirectToPage("/Klient/Login");
            }

            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            var client = _context.Clients.FirstOrDefault(c => c.Name == userName);

            if (client == null)
            {
                // Nie mo¿na znaleŸæ klienta w bazie danych, obs³u¿ b³¹d
                string info = "Nie posiadasz konta";
                return BadRequest(info);
            }

            // Ustaw w³aœciwoœci opinii
            newOpinia.CreatedAt = DateTime.Now;
            newOpinia.ClientId = client.Id;
            newOpinia.ProductId = Id;

            // Zapisz opiniê do bazy danych
            _context.Opinie.Add(newOpinia);
            _context.SaveChanges();

            // Przekieruj do innej strony po zapisaniu opinii
            return RedirectToPage();
        }

        public IActionResult OnPostAddToCart(int productId, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // U¿ytkownik nie jest zalogowany, obs³u¿ b³¹d lub przekieruj do strony logowania
                return RedirectToPage("/Klient/Login");
            }

            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            var client = _context.Clients.FirstOrDefault(c => c.Name == userName);

            if (client == null)
            {
                // Nie mo¿na znaleŸæ klienta w bazie danych, obs³u¿ b³¹d
                string info = "Nie posiadasz konta";
                return BadRequest(info);
            }

            var product = _context.Products.Include(p => p.Promotion).FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                // Nie mo¿na znaleŸæ produktu w bazie danych, obs³u¿ b³¹d
                return NotFound();
            }

            decimal price = product.Promotion != null
                ? product.Price * (1 - product.Promotion.DiscountPercent / 100m)
                : product.Price;

            // Wyszukaj istniej¹ce elementy koszyka dla tego produktu
            var existingCartItems = _context.CartItems.Where(ci => ci.Product.Id == productId).ToList();

            foreach (var existingCartItem in existingCartItems)
            {
                // Aktualizacja iloœci i ceny dla ka¿dego elementu
                existingCartItem.Quantity += quantity;
                existingCartItem.Price = price;
            }

            if (!existingCartItems.Any())
            {
                // Jeœli nie istnieje, dodaj nowy element do koszyka
                var cartItem = new CartItem
                {
                    Product = product,
                    Quantity = quantity,
                    Price = price // Dodanie ceny z uwzglêdnieniem promocji do elementu koszyka
                };

                _context.CartItems.Add(cartItem);
                TempData["Message"] = "Dodano produkt do koszyka!";
            }
            else
            {
                TempData["Message"] = "Zaktualizowano iloœæ i cenê produktu w koszyku!";
            }

            _context.SaveChanges();

            Id = productId;
            OnGet();
            return Page();
        }

        //
    }
}
