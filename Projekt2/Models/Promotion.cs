namespace Projekt2.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        public int DiscountPercent { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
