using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.ViewModels.CategoryVM;
using WebApplication1.ViewModels.SliderVM;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        PustokDbContext _db { get; }

        public CategoryController(PustokDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var items = await _db.Categories.Select(s => new CategoryListItemVM
            {
                Id = s.Id,
                Name = s.Name,
                ParentCategory = s.ParentCategory,
            }).ToListAsync();
            ViewBag.Categories = _db.Categories;
            return View(items);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (await _db.Categories.AnyAsync(x => x.Name == vm.Name))
            {
                ModelState.AddModelError("Name", vm.Name + " already exist");
                return View(vm);
            }
            await _db.Categories.AddAsync(new Models.Category 
            { 
                Name = vm.Name, 
                ParentCategory = vm.ParentCategory  
            });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["DeleteResponse"] = false;
            if (id == null) return BadRequest();
            var data = await _db.Categories.FindAsync(id);
            if (data == null) return NotFound();
            _db.Categories.Remove(data);
            await _db.SaveChangesAsync();
            TempData["DeleteResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Categories.FindAsync(id);
            if (data == null) return NotFound();
            ViewBag.Categories = _db.Categories;
            return View(new CategoryUpdateVM
            {
                Name = data.Name,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, CategoryUpdateVM vm)
        {
            TempData["UpdateResponse"] = false;
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Categories.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            data.ParentCategory = (int?)vm.ParentCategory;
            await _db.SaveChangesAsync();
            TempData["UpdateResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}
