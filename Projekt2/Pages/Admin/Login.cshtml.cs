using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

namespace Projekt2.Pages.Admin
{
    public class LoginModel : PageModel
    {
		private readonly MyDbContext _context;

		[BindProperty]
        [Required(ErrorMessage = "Nazwa u篡tkownika jest wymagana.")]
        public string Username { get; set; }

		[BindProperty]
        [Required(ErrorMessage = "Has這 jest wymagane.")]
        public string Password { get; set; }

		public LoginModel(MyDbContext context)
		{
			_context = context;
		}

        public List<Product> PromotionalProducts { get; set; }

        public void OnGet()
		{
            PromotionalProducts = _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            // Sprawdzenie, czy u篡tkownik o podanej nazwie istnieje
            var user = _context.Users.FirstOrDefault(u => u.Username == Username);
            if (user == null)
            {
                ModelState.AddModelError("Username", "Nieprawid這wa nazwa u篡tkownika.");
                return Page();
            }

            // Sprawdzenie, czy has這 jest poprawne
            if (user.Password != Password)
            {
                ModelState.AddModelError("Password", "Nieprawid這we has這.");
                return Page();
            }

            // Uwierzytelnienie u篡tkownika
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

            return RedirectToPage("/Index");
        }


        //
    }
}
