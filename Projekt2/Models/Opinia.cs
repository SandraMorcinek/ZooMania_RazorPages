using System.ComponentModel.DataAnnotations;

namespace Projekt2.Models
{
    public class Opinia
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Wypełnij pole!")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Podaj ocenę!")]
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
