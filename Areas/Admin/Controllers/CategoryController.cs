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
                IsDeleted = s.IsDeleted,
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
                ViewBag.Categories = _db.Categories;
                return View(vm);
            }
            if (await _db.Categories.AnyAsync(x => x.Name == vm.Name))
            {
                ModelState.AddModelError("Name", vm.Name + " already exist");
                ViewBag.Categories = _db.Categories;
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
            data.IsDeleted = true;
            _db.Categories.Update(data);
            await _db.SaveChangesAsync();
            TempData["DeleteResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Categories.FindAsync(id);
            if (data == null) return NotFound();
            ViewBag.Categories = _db.Categories.Where(o => id != o.Id);
            return View(new CategoryUpdateVM
            {
                Name = data.Name,
                ParentCategory = data.ParentCategory,
                IsDeleted = data.IsDeleted,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, CategoryUpdateVM vm)
        {
            TempData["UpdateResponse"] = false;
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _db.Categories.Where(o => id != o.Id && id != o.ParentCategory);
                return View(vm);
            }
            var data = await _db.Categories.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            data.ParentCategory = (int?)vm.ParentCategory;
            data.IsDeleted = vm.IsDeleted;
            await _db.SaveChangesAsync();
            TempData["UpdateResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}
