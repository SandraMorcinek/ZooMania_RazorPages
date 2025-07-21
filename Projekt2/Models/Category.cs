using System.ComponentModel.DataAnnotations;

namespace Projekt2.Models
{
	public class Category
	{
		public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [RegularExpression(@"^[^0-9]*$", ErrorMessage = "Nazwa nie może zawierać liczb.")]
        public string Name { get; set; }

	}
}
