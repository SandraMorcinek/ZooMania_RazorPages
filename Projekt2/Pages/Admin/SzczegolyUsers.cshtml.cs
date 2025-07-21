using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt2.Models;

namespace Projekt2.Pages.Admin
{
    public class SzczegolyUsersModel : PageModel
    {
        private readonly MyDbContext _context;

        public SzczegolyUsersModel(MyDbContext context)
        {
            _context = context;
        }

        public User Users { get; set; }

        public void OnGet(int userId)
        {
            Users = _context.Users.FirstOrDefault(u => u.Id == userId);
        }
    }
}
