using System.Globalization;
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

        public void OnGet(
            [FromServices] IWebHostEnvironment env,
            string? koleksiyon = null,
            string? q = null,
            string? tag = null,
            bool? clear = null)
        {
            // Men�den "Ma�aza" t�kland���nda filtreyi s�f�rla (clear=1)
            if (clear == true) koleksiyon = null;

            // JSON oku (UTF-8; sorun olursa 1254 fallback)
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var path = Path.Combine(env.WebRootPath, "data", "products.json");
            string json = System.IO.File.ReadAllText(path, Encoding.UTF8);
            if (json.Contains('?')) // yanl�� charset belirtisi
            {
                json = System.IO.File.ReadAllText(path, Encoding.GetEncoding(1254));
            }

            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var root = JsonSerializer.Deserialize<ProductsRoot>(json, opts);
            var list = (root?.Products ?? new()).Where(p => p.IsActive).ToList();

            // --- Koleksiyon filtresi ---
            if (!string.IsNullOrWhiteSpace(koleksiyon))
            {
                CurrentCollectionSlug = koleksiyon;
                CurrentCollectionTitle = ToTitle(koleksiyon);

                list = list.Where(p =>
                        string.Equals(p.Collection ?? "", koleksiyon, StringComparison.OrdinalIgnoreCase)
                        || (p.Tags?.Any(t => Slugify(t) == koleksiyon) ?? false))
                    .ToList();
            }

            // --- Etiket filtresi ---
            if (!string.IsNullOrWhiteSpace(tag))
            {
                var stag = Slugify(tag);
                list = list.Where(p => p.Tags?.Any(t => Slugify(t) == stag) ?? false).ToList();
            }

            // --- Arama ---
            if (!string.IsNullOrWhiteSpace(q))
            {
                var nq = q.Trim().ToLowerInvariant();
                list = list.Where(p =>
                    (p.Name?.ToLowerInvariant().Contains(nq) ?? false) ||
                    (p.Subtitle?.ToLowerInvariant().Contains(nq) ?? false) ||
                    (p.Description?.ToLowerInvariant().Contains(nq) ?? false))
                    .ToList();
            }

            Products = list;
        }

        private static string Slugify(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;

            s = s.Trim().ToLower(new CultureInfo("tr-TR"));
            // T�rk�e karakter d�n���mleri
            s = s.Replace('�', 'i').Replace('�', 'i')
                 .Replace('�', 's').Replace('�', 's')
                 .Replace('�', 'g').Replace('�', 'g')
                 .Replace('�', 'c').Replace('�', 'c')
                 .Replace('�', 'o').Replace('�', 'o')
                 .Replace('�', 'u').Replace('�', 'u');

            return string.Join("-", s.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        private static string ToTitle(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug)) return string.Empty;

            var ci = new CultureInfo("tr-TR");
            var parts = slug.Split('-', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                var p = parts[i];
                parts[i] = char.ToUpper(p[0], ci) + (p.Length > 1 ? p[1..] : "");
            }
            return string.Join(' ', parts);
        }

        public class ProductsRoot
        {
            public int Version { get; set; }
            public DateTime UpdatedAt { get; set; }
            public List<ProductVM> Products { get; set; } = new();
        }
    }
}
