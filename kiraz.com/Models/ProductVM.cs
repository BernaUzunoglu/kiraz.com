// Models/ProductVM.cs
namespace kiraz.com.Models
{
    public class ProductVM
    {
        public string Id { get; set; } = default!;
        public string? Slug { get; set; }
        public string Name { get; set; } = default!;
        public string? Subtitle { get; set; }
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public string? Currency { get; set; }
        public string MainImage { get; set; } = default!;
        public List<string> Gallery { get; set; } = new();
        public List<string>? Tags { get; set; }
        public Dictionary<string, string>? Attributes { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; } = true;

        public string? Collection { get; set; }
    }

    // JSON kök nesnesi
    public class ProductsJson
    {
        public int Version { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ProductVM> Products { get; set; } = new();
    }
}
