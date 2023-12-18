using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Contexts;
using WebApplication1.ViewModels.ProductVM;
using WebApplication1.ViewModels.ProductDetailsVM;
using WebApplication1.ViewModels.CategoryVM;
using WebApplication1.ViewModels.BasketVM;

namespace WebApplication1.Controllers
{
    public class ProductDetailsController : Controller
    {
        public PustokDbContext _context { get; set; }

        public ProductDetailsController(PustokDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }
            var data = await _context.Products.Include(p => p.ProductTags).ThenInclude(p => p.Tag).SingleOrDefaultAsync(p => p.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            ProductDetailsVM vm = new ProductDetailsVM
            {
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
                ProductDetail = new ProductDetailVM
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
                    ImageUrls = data.Images,
                    Tags = data.ProductTags.Select(p => p.Tag)
                }
            };
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();
            List<BasketProductItemVM> bp = await _context.Products.Select(p => new BasketProductItemVM
            {
                Count = p.Quantity,
                Id = p.Id,
                Discount = p.Discount,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Price = p.SellPrice
            }).ToListAsync();
            ViewBag.BasketProducts = bp;
            return View(vm);
        }
    }
}
