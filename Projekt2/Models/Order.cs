using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt2.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Client Client { get; set; }

        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal Transport { get; set; }

        public List<OrderItem> OrderItems { get; set; }

    }
}
