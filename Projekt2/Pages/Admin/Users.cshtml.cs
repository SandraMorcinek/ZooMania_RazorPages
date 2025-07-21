using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt2.Models;

namespace Projekt2.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly MyDbContext _context;

        [TempData]
        public string Message { get; set; }

        public UsersModel(MyDbContext context)
        {
            _context = context;
        }

        public List<User> Users { get; set; }

        public void OnGet()
        {
            Users = _context.Users.ToList();
        }

        public IActionResult OnPostDeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null && user.Username != "admin")
            {
                _context.Users.Remove(user);
                _context.SaveChanges();

                Message = "U¿ytkownik usuniêty pomyœlnie.";
            }
            else
            {
                Message = "U¿ytkownik nie zosta³ odnaleziony lub jest administratorem.";
            }

            return RedirectToPage();
        }

    }
}
