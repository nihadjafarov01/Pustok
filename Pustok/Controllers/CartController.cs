using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Contexts;
using WebApplication1.ViewModels.BasketVM;
using WebApplication1.ViewModels.HomeVM;
using WebApplication1.ViewModels.ProductVM;
using WebApplication1.ViewModels.SliderVM;
using WebApplication1.ViewModels.CartVM;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        public PustokDbContext _context { get; set; }

        public CartController(PustokDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
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
			CartVM cartVM = new CartVM
			{
				Products = await _context.Products.Select(x => new ProductListItemVM
				{
					Id=x.Id,
					About = x.About,
					CategoryId = x.CategoryId,
					CostPrice = x.CostPrice,
					Description = x.Description,
					Discount = x.Discount,
					HoverImageUrl = x.HoverImageUrl,
					ImageUrl=x.ImageUrl,
					Name = x.Name,
					IsDeleted = x.IsDeleted,
					ProductImages = x.Images,
					Quantity = x.Quantity,
					SellPrice = x.SellPrice,
				}).ToListAsync(),
				BasketProducts = basketItems,
			};
			ViewBag.Categories = await _context.Categories.ToListAsync();
			ViewBag.Products = await _context.Products.ToListAsync();
			return View(cartVM);
		}
        public IActionResult GetCart()
        {
            return ViewComponent("Cart");
        }
    }
}
