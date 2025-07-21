using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages.Klient
{
    public class RegisterModel : PageModel
    {
		private readonly MyDbContext _context;

		[BindProperty]
		public Client NewClient { get; set; }
		public RegisterModel(MyDbContext dbContext)
		{
			_context = dbContext;
		}

        public List<Product> PromotionalProducts { get; set; }

        public void OnGet()
        {
            PromotionalProducts = _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToList();
        }

		public IActionResult OnPost()
		{
			if (ModelState.IsValid)
			{
				bool emailExists = _context.Clients.Any(u => u.Email == NewClient.Email);

				if (emailExists)
				{
					ModelState.AddModelError("NewClient.Email", "E-mail ju¿ istnieje.");
					return Page();
				}

				if (!IsValidEmail(NewClient.Email))
				{
					ModelState.AddModelError("NewClient.Email", "Nieprawid³owy adres e-mail.");
					return Page();
				}

				_context.Clients.Add(NewClient);
				_context.SaveChanges();

				TempData["SuccessMessage"] = "Konto zosta³o utworzone. Mo¿esz siê teraz zalogowaæ.";

				//return RedirectToPage("Login");
			}

			return Page();
		}

		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}

		//
	}
}
