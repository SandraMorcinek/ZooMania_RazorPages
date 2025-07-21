using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages
{
	public class EditProductModel : PageModel
	{
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public Product EditProduct { get; set; }

        [BindProperty]
        public int SelectedCategory { get; set; }

        public List<SelectListItem> Categories { get; set; }

		[BindProperty]
		public IFormFile NewImageFile { get; set; }

		public EditProductModel(MyDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult OnGet(int id)
        {
            EditProduct = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);

            if (EditProduct == null)
            {
                return NotFound();
            }

            Categories = _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == EditProduct.CategoryId
            }).ToList();

            SelectedCategory = EditProduct.CategoryId; // Przypisanie wartoœci SelectedCategory na podstawie kategorii produktu

            return Page();
        }

        public IActionResult OnPost()
        {
            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == EditProduct.Id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = EditProduct.Name;
            product.Description = EditProduct.Description;
            product.Price = EditProduct.Price;

            if (product.CategoryId != SelectedCategory)
            {
                var category = _context.Categories.FirstOrDefault(c => c.Id == SelectedCategory);
                if (category != null)
                {
                    product.CategoryId = SelectedCategory;
                    product.Category = category;
                }
            }

            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                if (file != null && file.Length > 0)
                {
                    var imagePath = Guid.NewGuid().ToString() + "_" + file.FileName;
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", imagePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    product.ImagePath = imagePath;
                }
            }

            // Walidacja modelu
            if (product.CategoryId == 0)
            {
                ModelState.AddModelError("SelectedCategory", "Wybierz kategoriê");
                Categories = _context.Categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == product.CategoryId
                }).ToList();
                return Page();
            }

            _context.SaveChanges();

            return RedirectToPage("Products");
        }


        //
    }
}
