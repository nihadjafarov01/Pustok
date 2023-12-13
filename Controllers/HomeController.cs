using AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.ViewModels.CommonVM;
using WebApplication1.ViewModels.HomeVM;
using WebApplication1.ViewModels.ProductVM;

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
            ViewBag.Sliders = _context.Sliders;
            ViewBag.Products = _context.Products;
            ViewBag.Categories = _context.Categories;
            
            return View(ViewBag);
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
            return PartialView(nameof(Views_Shared__ProductPaginationPartial),pag);
        }
    }
}
