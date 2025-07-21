using System.ComponentModel.DataAnnotations;

namespace Projekt2.Models
{
	public class Client
	{
		public int Id { get; set; }

		[Required(ErrorMessage ="Podaj imię")]
		[MaxLength(20, ErrorMessage = "Imię nie może przekraczać 20 znaków.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Podaj nazwisko")]
		[MaxLength(50, ErrorMessage = "Nazwisko nie może przekraczać 50 znaków.")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Podaj e-mail")]
		[MaxLength(30, ErrorMessage = "E-mail nie może przekraczać 30 znaków.")]
		public string Email { get; set; }

		[RegularExpression("^[0-9]*$", ErrorMessage = "Telefon może zawierać tylko cyfry.")]
		[MaxLength(20, ErrorMessage = "Numer telefonu nie może przekraczać 20 znaków.")]
		public string? Phone { get; set; }

		[Required(ErrorMessage = "Podaj Hasło")]
		[MinLength(3, ErrorMessage = "Hasło musi mieć co najmniej 3 znaki.")]
		[MaxLength(20, ErrorMessage = "Hasło nie może przekraczać 20 znaków.")]
		public string Password { get; set; }
	}
}
