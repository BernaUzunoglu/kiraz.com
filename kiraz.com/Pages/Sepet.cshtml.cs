using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using kiraz.com.Models;
using kiraz.com.Helpers;

namespace kiraz.com.Pages
{
    public class SepetModel : PageModel
    {
        private const string CART_KEY = "CART";

        public List<CartItem> Items { get; set; } = new();
        public decimal Total => Items.Sum(x => x.LineTotal);

        public void OnGet()
        {
            Items = HttpContext.Session.GetObj<List<CartItem>>(CART_KEY) ?? new();
        }

        public IActionResult OnPostAdd(string id, string name, decimal price, string? imageUrl)
        {
            var cart = HttpContext.Session.GetObj<List<CartItem>>(CART_KEY) ?? new();
            var existing = cart.FirstOrDefault(x => x.Id == id);
            if (existing != null) existing.Qty += 1;
            else cart.Add(new CartItem { Id = id, Name = name, Price = price, Qty = 1, ImageUrl = imageUrl });

            HttpContext.Session.SetObj(CART_KEY, cart);

            TempData["Toast"] = $"{name} sepete eklendi.";
            // Eski sayfaya dön (Maðaza)
            var referer = Request.Headers["Referer"].ToString();
            return !string.IsNullOrEmpty(referer) ? Redirect(referer) : RedirectToPage("/Sepet");
        }

        public IActionResult OnPostRemove(string id)
        {
            var cart = HttpContext.Session.GetObj<List<CartItem>>(CART_KEY) ?? new();
            cart.RemoveAll(x => x.Id == id);
            HttpContext.Session.SetObj(CART_KEY, cart);
            return RedirectToPage("/Sepet");
        }

        public IActionResult OnPostQty(string id, int qty)
        {
            var cart = HttpContext.Session.GetObj<List<CartItem>>(CART_KEY) ?? new();
            var item = cart.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                item.Qty = Math.Max(1, qty);
                HttpContext.Session.SetObj(CART_KEY, cart);
            }
            return RedirectToPage("/Sepet");
        }

        // Header sayacý için: /Sepet?handler=Count
        public JsonResult OnGetCount()
        {
            var cart = HttpContext.Session.GetObj<List<CartItem>>(CART_KEY) ?? new();
            return new JsonResult(cart.Sum(x => x.Qty));
        }
    }
}
