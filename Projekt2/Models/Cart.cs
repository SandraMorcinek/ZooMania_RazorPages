using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt2.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
