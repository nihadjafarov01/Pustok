using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Contexts;
using WebApplication1.ViewModels.BasketVM;

namespace WebApplication1.ViewComponents
{
    public class CartViewComponent:ViewComponent
    {
        PustokDbContext _context { get; }

        public CartViewComponent(PustokDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = JsonConvert.DeserializeObject<List<BasketProductAndCountVM>>(HttpContext.Request.Cookies["basket"] ?? "[]");
            var products = _context.Products.Where(p => items.Select(i => i.Id).Contains(p.Id));
            List<BasketProductItemVM> basketItems = new();
            foreach (var item in products)
            {
                basketItems.Add(new BasketProductItemVM
                {
                    Id = item.Id,
                    Discount = item.Discount,
                    ImageUrl = item.ImageUrl,
                    Name = item.Name,
                    Price = item.SellPrice,
                    Count = items.FirstOrDefault(x => x.Id == item.Id).Count
                });
            }
            return View(basketItems);
        }
    }
}
