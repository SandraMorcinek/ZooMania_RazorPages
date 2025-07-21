using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projekt2.Models;

namespace Projekt2.Pages
{
    public class EditCategoryModel : PageModel
    {
        private readonly MyDbContext _context;

        [BindProperty]
        public Category EditCategory { get; set; }

        public EditCategoryModel(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            EditCategory = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (EditCategory == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                // SprawdŸ, czy istnieje kategoria o podanej nazwie
                var existingCategory = _context.Categories.FirstOrDefault(c => c.Name == EditCategory.Name && c.Id != EditCategory.Id);

                if (existingCategory != null)
                {
                    ModelState.AddModelError("EditCategory.Name", "Nazwa kategorii ju¿ istnieje.");
                    return Page();
                }

                var categoryToUpdate = _context.Categories.FirstOrDefault(c => c.Id == EditCategory.Id);

                if (categoryToUpdate == null)
                {
                    return NotFound();
                }

                categoryToUpdate.Name = EditCategory.Name;

                _context.SaveChanges();

                return RedirectToPage("Categories");
            }

            return Page();
        }

    }
}
