using System.ComponentModel.DataAnnotations;

namespace Projekt2.Models
{
	public class User
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Nazwa użytkownika jest wymagana.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Hasło jest wymagane.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Imię jest wymagane.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Nazwisko jest wymagane.")]
		public string Lastname { get; set; }

		[Required(ErrorMessage = "Adres e-mail jest wymagany.")]
		[EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail.")]
		public string Email { get; set; }

	}
}
