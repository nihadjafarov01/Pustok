using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.Admin.ViewModels;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.ViewModels.CategoryVM;
using WebApplication1.ViewModels.ProductVM;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        PustokDbContext _db { get; }

        public ProductController(PustokDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Products.Select(p => new AdminProductListItemVM
            {
                Id = p.Id,
                Name = p.Name,
                CostPrice = p.CostPrice,
                Discount = p.Discount,
                Category = p.Category,
                //ImageUrl = p.ImageUrl,
                IsDeleted = p.IsDeleted,
                Quantity = p.Quantity,
                SellPrice = p.SellPrice
            }));
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (vm.CostPrice > vm.SellPrice)
            {
                ModelState.AddModelError("CostPrice", "Sell price must be bigger than cost price");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _db.Categories;
                return View(vm);
            }
            if (await _db.Categories.AnyAsync(x => x.Id != vm.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category doesn't exist");
                ViewBag.Categories = _db.Categories;
                return View(vm);
            }
            Product prod = new Product
            {
                Name = vm.Name,
                About = vm.About,
                Quantity = vm.Quantity,
                Description = vm.Description,
                Discount = vm.Discount,
                //ImageUrl = vm.ImageUrl,
                CostPrice = vm.CostPrice,
                SellPrice = vm.SellPrice,
                CategoryId = vm.CategoryId
            };
            await _db.Products.AddAsync(prod);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["ProductDeleteResponse"] = false;
            if (id == null) return BadRequest();
            var data = await _db.Products.FindAsync(id);
            if (data == null) return NotFound();
            _db.Products.Remove(data);
            await _db.SaveChangesAsync();
            TempData["ProductDeleteResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }
            var data = await _db.Products.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            ViewBag.Categories = _db.Categories;
            return View(new ProductUpdateVM
            {
                Name = data.Name,
                About = data.About,
                Description = data.Description,
                SellPrice = data.SellPrice,
                CostPrice = data.CostPrice,
                Discount = data.Discount,
                Quantity = data.Quantity,
                //ImageUrl = data.ImageUrl,
                CategoryId = data.CategoryId
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
        {
            TempData["ProductUpdateResponse"] = false;
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _db.Categories;
                return View(vm);
            }
            var data = await _db.Products.FindAsync(id);
            if (data == null)
            {
                ViewBag.Categories = _db.Categories;
                return NotFound();
            }
            data.Name = vm.Name;
            data.About = vm.About;
            data.Description = vm.Description;
            data.SellPrice = vm.SellPrice;
            data.CostPrice = vm.CostPrice;
            data.Discount = vm.Discount;
            data.Quantity = vm.Quantity;
            //data.ImageUrl = vm.ImageUrl;
            data.CategoryId = vm.CategoryId;    
            await _db.SaveChangesAsync();
            TempData["ProductUpdateResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}
