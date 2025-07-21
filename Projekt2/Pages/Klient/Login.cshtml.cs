using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;
using System.Security.Claims;

namespace Projekt2.Pages.Klient
{
    public class LoginModel : PageModel
    {
        private readonly MyDbContext _context;

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public LoginModel(MyDbContext dbContext)
        {
            _context = dbContext;
        }

        public List<Product> PromotionalProducts { get; set; }

        public void OnGet()
        {
            PromotionalProducts = _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Weryfikacja logowania klienta
            var client = _context.Clients.FirstOrDefault(c => c.Email == Email && c.Password == Password);
            if (client != null)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, client.Name),
                new Claim(ClaimTypes.Role, "Klient")
            };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var claimsPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Nieprawid³owy e-mail lub has³o.");
            return Page();
        }

        //
    }
}
