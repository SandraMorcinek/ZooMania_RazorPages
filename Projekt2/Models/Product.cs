using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt2.Models
{
	public class Product
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Nazwa jest wymagana.")]
		public string Name { get; set; }

		//[Required(ErrorMessage = "Dodaj zdjęcie.")]
		public string ImagePath { get; set; }

		[Required(ErrorMessage = "Opis jest wymagany.")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Cena jest wymagana.")]
		[Range(0, double.MaxValue, ErrorMessage = "Cena musi być liczbą dodatnią.")]
		public decimal Price { get; set; }


		[Required(ErrorMessage = "Wybierz kategorię.")]
		public int CategoryId { get; set; }

		public Category Category { get; set; }

        public Promotion Promotion { get; set; }


        [NotMapped] // Ignorowane przy tworzeniu schematu bazy danych
        public double? AverageRating { get; set; } // Nullable, bo niektóre produkty mogą nie mieć żadnej oceny

    }
}
