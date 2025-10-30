using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using kiraz.com.Models;

namespace kiraz.com.Pages.Urun
{
    public class DetayModel : PageModel
    {
        public ProductVM? Product { get; set; }

        public IActionResult OnGet(
            string? id,          // ?id=1
            string? slug,        // /Urun/Detay/slug
            [FromServices] IWebHostEnvironment env)
        {
            var path = Path.Combine(env.WebRootPath, "data", "products.json");
            if (!System.IO.File.Exists(path)) return RedirectToPage("/Magaza");

            var json = System.IO.File.ReadAllText(path);
            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var root = JsonSerializer.Deserialize<ProductsJson>(json, opts);

            var list = root?.Products ?? new List<ProductVM>();

            if (!string.IsNullOrWhiteSpace(id))
                Product = list.FirstOrDefault(x => x.Id == id);
            else if (!string.IsNullOrWhiteSpace(slug))
                Product = list.FirstOrDefault(x => x.Slug == slug);
            else
                return RedirectToPage("/Magaza");

            return Product == null ? RedirectToPage("/Magaza") : Page();
        }
    }
}
