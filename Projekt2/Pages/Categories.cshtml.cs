using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt2.Models;

namespace Projekt2.Pages
{
    public class CategoriesModel : PageModel
    {
        private readonly MyDbContext _context;

        public List<Category> Categories { get; set; } = new List<Category>();

        [BindProperty]
        public Category NewCategory { get; set; }
        public CategoriesModel(MyDbContext context)
		{
			_context = context;
		}

		public void OnGet()
        {
            Categories = _context.Categories.ToList();
        }

		public IActionResult OnPost()
		{
			if (ModelState.IsValid)
			{
				if (string.IsNullOrWhiteSpace(NewCategory.Name))
				{
					ModelState.AddModelError("NewCategory.Name", "Nazwa kategorii jest wymagana.");
				}
				// SprawdŸ, czy nazwa kategorii zawiera jakiekolwiek cyfry 
				else if (NewCategory.Name.Any(char.IsDigit))
				{
					ModelState.AddModelError("NewCategory.Name", "Nazwa kategorii nie mo¿e zawieraæ liczb.");
				}
				else
				{
					// SprawdŸ, czy nazwa kategorii ju¿ istnieje
					bool categoryNameExists = _context.Categories.Any(c => c.Name == NewCategory.Name);

					if (categoryNameExists)
					{
						ModelState.AddModelError		("NewCategory.Name", "Nazwa kategorii ju¿ istnieje.");
					}
					else
					{
						_context.Categories.Add(NewCategory);
						_context.SaveChanges();
                    }
				}
			}

			Categories = _context.Categories.ToList();

			return Page();
		}
	}
}
