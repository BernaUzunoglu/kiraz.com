using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using kiraz.com.Models;

namespace kiraz.com.Pages
{
    public class MagazaModel : PageModel
    {
        public List<ProductVM> Products { get; set; } = new();
        public string? CurrentCollectionSlug { get; set; }
        public string? CurrentCollectionTitle { get; set; }

        public void OnGet([FromServices] IWebHostEnvironment env,
                          string? koleksiyon = null,
                          string? q = null,
                          string? tag = null)
        {
            // JSON oku (UTF-8 fallback �rne�i)
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var path = Path.Combine(env.WebRootPath, "data", "products.json");
            var json = System.IO.File.ReadAllText(path, Encoding.UTF8);
            if (json.Contains('?'))
            {
                json = System.IO.File.ReadAllText(path, Encoding.GetEncoding(1254));
            }

            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var root = JsonSerializer.Deserialize<ProductsRoot>(json, opts);
            var list = (root?.Products ?? new()).Where(p => p.IsActive).ToList();

            // Filtre: koleksiyon
            if (!string.IsNullOrWhiteSpace(koleksiyon))
            {
                CurrentCollectionSlug = koleksiyon;
                // Ba�l�k �ret (slug -> ba�l�k)
                CurrentCollectionTitle = ToTitle(koleksiyon);

                list = list.Where(p =>
                        string.Equals(p.Collection, koleksiyon, StringComparison.OrdinalIgnoreCase)
                        || (p.Tags?.Any(t => Slugify(t) == koleksiyon) ?? false))
                    .ToList();
            }

            // Filtre: tag
            if (!string.IsNullOrWhiteSpace(tag))
            {
                list = list.Where(p => p.Tags?.Any(t => Slugify(t) == Slugify(tag)) ?? false).ToList();
            }

            // Filtre: arama
            if (!string.IsNullOrWhiteSpace(q))
            {
                var nq = q.Trim().ToLowerInvariant();
                list = list.Where(p =>
                    (p.Name?.ToLowerInvariant().Contains(nq) ?? false) ||
                    (p.Subtitle?.ToLowerInvariant().Contains(nq) ?? false) ||
                    (p.Description?.ToLowerInvariant().Contains(nq) ?? false)).ToList();
            }

            Products = list;
        }

        private static string Slugify(string s)
        {
            s = s.ToLowerInvariant()
                 .Replace('�', 'i').Replace('�', 'i')
                 .Replace('�', 's').Replace('�', 's')
                 .Replace('�', 'g').Replace('�', 'g')
                 .Replace('�', 'c').Replace('�', 'c')
                 .Replace('�', 'o').Replace('�', 'o')
                 .Replace('�', 'u').Replace('�', 'u');
            return string.Join("-", s.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        private static string ToTitle(string slug)
        {
            var parts = slug.Split('-', StringSplitOptions.RemoveEmptyEntries);
            return string.Join(' ', parts.Select(p => char.ToUpperInvariant(p[0]) + p[1..]));
        }

        // JSON root
        public class ProductsRoot
        {
            public int Version { get; set; }
            public DateTime UpdatedAt { get; set; }
            public List<ProductVM> Products { get; set; } = new();
        }
    }
}
