using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly MyDbContext _context;

        [BindProperty]
        public User NewUser { get; set; }
        public RegisterModel(MyDbContext dbContext)
		{
			_context = dbContext;
		}

		public void OnGet()
        {

        }

		public IActionResult OnPost()
        {
			if (ModelState.IsValid)
			{
				// SprawdŸ, czy nazwa u¿ytkownika ju¿ istnieje
				bool usernameExists = _context.Users.Any(u => u.Username == NewUser.Username);

				if (usernameExists)
				{
					ModelState.AddModelError("NewUser.Username", "Nazwa u¿ytkownika ju¿ istnieje.");
					return Page();
				}

				_context.Users.Add(NewUser);
				_context.SaveChanges();

                TempData["SuccessMessage"] = "U¿ytkownik zosta³ utworzony.";

                //return RedirectToPage();
			}

			return Page();
		}

	}
}
