using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Contexts;
using WebApplication1.ViewModels.CategoryVM;
using WebApplication1.ViewModels.ProductImagesVM;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductImagesController : Controller
    {
        PustokDbContext _db { get; }

        public ProductImagesController(PustokDbContext db)
        {
            _db = db;
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
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await _db.ProductImages.AddAsync(new Models.ProductImages { ImagePath = vm.ImagePath });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
