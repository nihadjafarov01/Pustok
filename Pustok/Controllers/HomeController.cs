using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication1.Contexts;
using WebApplication1.Models;
//using WebApplication1.ViewModels.CommonVM;
using WebApplication1.ViewModels.HomeVM;
using WebApplication1.ViewModels.ProductVM;
using WebApplication1.ViewModels.CommonVM;
using WebApplication1.ViewModels.SliderVM;
using WebApplication1.ViewModels.BasketVM;
using WebApplication1.ViewModels.CategoryVM;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public PustokDbContext _context { get; set; }

        public HomeController(PustokDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var datas = _context.Products.Where(p => !p.IsDeleted).Take(2).Select(p => new ProductListItemVM
            {
                Id = p.Id,
                Name = p.Name,
                SellPrice = p.SellPrice,
            Quantity = p.Quantity,
                ProductImages = p.Images,
                About = p.About,
                CategoryId = p.CategoryId,
                CostPrice = p.CostPrice,
                Description = p.Description,
                Discount = p.Discount,
                HoverImageUrl = p.HoverImageUrl,
                ImageUrl = p.ImageUrl,
                IsDeleted = p.IsDeleted,
            });
            int count = await _context.Products.CountAsync(x => !x.IsDeleted);
            PaginationVM<IEnumerable<ProductListItemVM>> pag = new(count, 1, (int)Math.Ceiling((decimal)count / 2), datas);
            HomeVM vm = new HomeVM
            {
                Sliders = await _context.Sliders.Select(s => new SliderListItemVM
                {
                    Id = s.Id,
                    ImageUrl = s.ImageUrl,
                    IsLeft = s.IsLeft,
                    Text = s.Text,
                    Title = s.Title,
                }).ToListAsync(),
                Products = await _context.Products.Select(p => new ProductListItemVM
                {
                    Id = p.Id,
                    About = p.About,
                    CategoryId = p.CategoryId,
                    CostPrice = p.CostPrice,
                    Description = p.Description,
                    Discount = p.Discount,
                    HoverImageUrl = p.HoverImageUrl,
                    ImageUrl = p.ImageUrl,
                    IsDeleted = p.IsDeleted,
                    Name = p.Name,
                    ProductImages = p.Images,
                    Quantity = p.Quantity,
                    SellPrice = p.SellPrice
                }).ToListAsync(),
                PaginatedProducts = pag
                };
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();
            //var items = JsonConvert.DeserializeObject<List<BasketProductAndCountVM>>(HttpContext.Request.Cookies["basket"] ?? "[]");
            //var products = _context.Products.Where(p => items.Select(i => i.Id).Contains(p.Id));
            //List<BasketProductItemVM> basketItems = new();
            //foreach (var item in products)
            //{
            //    basketItems.Add(new BasketProductItemVM
            //    {
            //        Id = item.Id,
            //        Discount = item.Discount,
            //        ImageUrl = item.ImageUrl,
            //        Name = item.Name,
            //        Price = item.SellPrice,
            //        Count = items.FirstOrDefault(x => x.Id == item.Id).Count
            //    });
            //}
            //ViewBag.BasketProducts = basketItems;
            return View(vm);
        }

        public async Task<IActionResult> ProductPagination(int page = 1, int count = 8)
        {
            var datas = _context.Products.Where(p => !p.IsDeleted).Skip((page-1)*count).Take(count).Select(p => new ProductListItemVM
            {
                Id=p.Id,
                Name=p.Name,
                CategoryId= p.CategoryId,
                Discount= p.Discount,
                ImageUrl = p.ImageUrl,
                HoverImageUrl = p.HoverImageUrl,
                SellPrice = p.SellPrice,
            });
            int totalCount = await _context.Products.CountAsync(x => !x.IsDeleted);
            PaginationVM<IEnumerable<ProductListItemVM>> pag = new(totalCount, page, (int)Math.Ceiling((decimal)totalCount / count), datas);
            return PartialView("_ProductPaginationPartial", pag);
        }
        public string GetCookie(string key)
        {
            return HttpContext.Request.Cookies[key] ?? "";
        }
        public async Task<IActionResult> GetBasket()
        {
            return ViewComponent("Basket");
        }

        public async Task<IActionResult> Cart()
        {
            HomeVM vm = new HomeVM
            {
                Sliders = await _context.Sliders.Select(s => new SliderListItemVM
                {
                    Id = s.Id,
                    ImageUrl = s.ImageUrl,
                    IsLeft = s.IsLeft,
                    Text = s.Text,
                    Title = s.Title,
                }).ToListAsync(),
                Products = await _context.Products.Select(p => new ProductListItemVM
                {
                    Id = p.Id,
                    About = p.About,
                    CategoryId = p.CategoryId,
                    CostPrice = p.CostPrice,
                    Description = p.Description,
                    Discount = p.Discount,
                    HoverImageUrl = p.HoverImageUrl,
                    ImageUrl = p.ImageUrl,
                    IsDeleted = p.IsDeleted,
                    Name = p.Name,
                    ProductImages = p.Images,
                    Quantity = p.Quantity,
                    SellPrice = p.SellPrice
                }).ToListAsync(),
            };
            return View(vm);
        }
        public IActionResult GetCart()
        {
            return ViewComponent("Cart");
        }
    }
}
