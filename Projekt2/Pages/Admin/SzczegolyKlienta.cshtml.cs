using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages.Admin
{
    public class SzczegolyKlientaModel : PageModel
    {
        private readonly MyDbContext _context;

        public SzczegolyKlientaModel(MyDbContext context)
        {
            _context = context;
        }

        public Client Client { get; set; }
        public int OrderCount { get; set; }

        public void OnGet(int clientId)
        {

            Client = _context.Clients.FirstOrDefault(c => c.Id == clientId);

            // Pobierz liczbê zamówieñ dla tego klienta
            OrderCount = _context.Orders.Count(o => o.Client.Id == clientId);
        }
    }
}
