using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Contexts;
using WebApplication1.ViewModels.CategoryVM;
using WebApplication1.ViewModels.ProductImagesVM;
using WebApplication1.ViewModels.SliderVM;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductImagesController : Controller
    {
        PustokDbContext _db { get; }
        IWebHostEnvironment _env { get; }
        public ProductImagesController(PustokDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var items = await _db.ProductImages.Select(s => new ProductImagesListItemVM
            {
                Id = s.Id,
                ImagePath = s.ImagePath,
                ProductId = s.ProductId,
                IsActive = s.IsActive
            }).ToListAsync();
            return View(items);
        }
        public IActionResult Create()
        {
            ViewBag.Products = _db.Products;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductImagesCreateVM vm)
        {
            if (vm.ProductId == 0)
            {
                ModelState.AddModelError("ProductId", "Product must be selected!");
                ViewBag.Products = _db.Products;
                return View(vm);
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Products = _db.Products;
                return View(vm);
            }

            string fileName = Path.Combine("image", "products", vm.ImageFile.FileName);
            using (FileStream fs = System.IO.File.Create(Path.Combine(_env.WebRootPath, fileName)))
            {
                await vm.ImageFile.CopyToAsync(fs);
            }

            await _db.ProductImages.AddAsync(new Models.ProductImages 
            { 
                ImagePath = fileName,
                ProductId = (int)vm.ProductId
                
            });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["DeleteResponse"] = false;
            if (id == null) return BadRequest();
            var data = await _db.ProductImages.FindAsync(id);
            if (data == null) return NotFound();
            data.IsActive = false;
            _db.ProductImages.Update(data);
            await _db.SaveChangesAsync();
            TempData["DeleteResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }
            var data = await _db.ProductImages.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            ViewBag.Products = _db.Products;
            return View(new ProductImagesUpdateVM
            {
                ProductId= data.ProductId,
                IsActive = data.IsActive,
                ImagePath = data.ImagePath,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, ProductImagesUpdateVM vm)
        {
            TempData["UpdateResponse"] = false;
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.ProductImages.FindAsync(id);
            if (data == null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (vm.ImageFile != null)
            {
            string fileName = Path.Combine("image", "products", vm.ImageFile.FileName);
            using (FileStream fs = System.IO.File.Create(Path.Combine(_env.WebRootPath, fileName)))
            {
                await vm.ImageFile.CopyToAsync(fs);
            }
            data.ImagePath = fileName;
            data.ProductId = vm.ProductId;
            data.IsActive = vm.IsActive;
            }
            else
            {
                data.ProductId = vm.ProductId;
                data.IsActive = vm.IsActive;
            
            }
            await _db.SaveChangesAsync();
            TempData["UpdateResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}
