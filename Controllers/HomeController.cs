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
            //ViewBag.Sliders = _context.Sliders;
            //ViewBag.Products = _context.Products;
            //ViewBag.Categories = _context.Categories;
            
            return View(vm);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }
            var res1 = await _context.Products.Include(p => p.Images).ToListAsync();
            var data = await _context.Products.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            ViewBag.Sliders = _context.Sliders;
            ViewBag.Products = _context.Products;
            ViewBag.Categories = _context.Categories;
            return View(new ProductDetailVM
            {
                Id = data.Id,
                Name = data.Name,
                About = data.About,
                Description = data.Description,
                ImageUrl = data.ImageUrl,
                HoverImageUrl = data.HoverImageUrl,
                SellPrice = data.SellPrice,
                CostPrice = data.CostPrice,
                Discount = data.Discount,
                Quantity = data.Quantity,
                CategoryId = data.CategoryId,
                IsDeleted = data.IsDeleted,
                ImageUrls = data.Images
            });
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
        public IActionResult GetBasket()
        {
            return ViewComponent("Basket");
        }

        public async Task<IActionResult> Cart()
        {
            var res1 = await _context.Products.Include(p => p.Images).ToListAsync();
            var data = await _context.Products.FindAsync();
            if (data == null)
            {
                return NotFound();
            }
            ViewBag.Sliders = _context.Sliders;
            ViewBag.Products = _context.Products;
            ViewBag.Categories = _context.Categories;
            return View(new BasketProductItemVM
            {
                Id = data.Id,
                Name = data.Name,
            });
        }
    }
}
