using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Projekt2.Pages.Klient
{
    public class LogoutModel : PageModel
    {
		public async Task<IActionResult> OnGetAsync()
		{
			// Wylogowanie u�ytkownika
			await HttpContext.SignOutAsync("MyCookieAuth");

			// Przekierowanie na stron� g��wn�
			return RedirectToPage("/Index");
		}
	}
}
