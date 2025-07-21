using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;
using System.Security.Claims;

namespace Projekt2.Pages.Klient
{
    public class EditAccountModel : PageModel
    {
        private readonly MyDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [BindProperty]
        public Client Client { get; set; }

        public EditAccountModel(MyDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Product> PromotionalProducts { get; set; }

        public void OnGet()
        {
            PromotionalProducts = _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToList();

            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            Client = _context.Clients.FirstOrDefault(c => c.Name == userName);
        }

        public IActionResult OnPost()
        {
            var existingClient = _context.Clients.FirstOrDefault(c => c.Id == Client.Id);

            if (existingClient != null)
            {
                existingClient.Name = Client.Name;
                existingClient.LastName = Client.LastName;
                existingClient.Email = Client.Email;
                if (!string.IsNullOrEmpty(Client.Phone))
                {
                    existingClient.Phone = Client.Phone;
                }

                _context.Clients.Update(existingClient);
                _context.SaveChanges();

                return RedirectToPage("/Klient/MyAccount");
            }
            else
            {
                ModelState.AddModelError("", "Nie znaleziono klienta.");
                return Page();
            }
        }


    }
}
