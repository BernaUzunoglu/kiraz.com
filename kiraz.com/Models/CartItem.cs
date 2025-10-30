namespace kiraz.com.Models
{
    public class CartItem
    {
        public string Id { get; set; } = default!;     // Ürün Id
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int Qty { get; set; } = 1;
        public string? ImageUrl { get; set; }

        public decimal LineTotal => Price * Qty;
    }
}
