using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Projekt2.Models;

namespace Projekt2.Pages
{
    public class ProductsModel : PageModel
    {
		private readonly MyDbContext _context;

		private readonly IWebHostEnvironment _webHostEnvironment;

		public List<Product> Products { get; set; } = new List<Product>();

		[BindProperty]
		public Product NewProduct { get; set; }

		public List<Category> Categories { get; set; }
        public List<Product> PromotionalProducts { get; set; }

        public ProductsModel(MyDbContext dbContext, IWebHostEnvironment webHostEnvironment)
		{
			_context = dbContext;
			_webHostEnvironment = webHostEnvironment;
		}

		public void OnGet()
		{
			Categories = _context.Categories.ToList();
			Products = _context.Products.ToList();
            PromotionalProducts = _context.Products.Include(p => p.Promotion).Where(p => p.Promotion != null).ToList();
        }

		public IActionResult OnPost(IFormFile imageFile)
		{
			// Usuñ ModelState dla ImagePath, Category, Promotion
			ModelState.Remove("NewProduct.Category");
			ModelState.Remove("NewProduct.Promotion");
			ModelState.Remove("NewProduct.ImagePath");

			if (NewProduct.CategoryId == 0)
			{
				ModelState.AddModelError("NewProduct.CategoryId", "Wybierz kategoriê.");
			}

			if (imageFile != null && imageFile.Length > 0)
			{
				// Generuj unikaln¹ nazwê pliku dla obrazka
				string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

				// Œcie¿ka, w której zostanie zapisany plik
				string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", uniqueFileName);

				using (var fileStream = new FileStream(imagePath, FileMode.Create))
				{
					imageFile.CopyTo(fileStream);
				}

				// Zapisz œcie¿kê obrazka w modelu produktu
				NewProduct.ImagePath = uniqueFileName;
			}
			else
			{
				ModelState.AddModelError("NewProduct.ImagePath", "Dodaj zdjêcie.");
			}

			if (!ModelState.IsValid)
			{
				Categories = _context.Categories.ToList();
				return Page();
			}

			// Dodaj produkt do bazy danych
			_context.Products.Add(NewProduct);
			_context.SaveChanges();

			// Resetuj formularz
			NewProduct = new Product();

			return RedirectToPage();
		}


	}
}
