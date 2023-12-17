using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.Admin.ViewModels.CommonVM;
using WebApplication1.Areas.Admin.ViewModels;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.ViewModels.CategoryVM;
using WebApplication1.ViewModels.CommonVM;
using WebApplication1.ViewModels.ProductVM;
using WebApplication1.ViewModels.SliderVM;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            //ViewBag.Categories = _db.Categories;
            var datas = _db.Categories.Take(2).Select(p => new CategoryListItemVM
            {
                Id = p.Id,
                IsDeleted = p.IsDeleted,
                Name = p.Name,  
                ParentCategoryId = p.ParentCategoryId,
                ParentCategory = p.ParentCategory,
                
            });
            int count = await _db.Categories.CountAsync();
            PaginationVM<IEnumerable<CategoryListItemVM>> pag = new(count, 1, (int)Math.Ceiling((decimal)count / 2), datas);
            return View(pag);
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
                ParentCategoryId = vm.ParentCategoryId
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
                IsDeleted = data.IsDeleted,
                ParentCategory = data.ParentCategory,
                ParentCategoryId = data.ParentCategoryId,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, CategoryUpdateVM vm)
        {
            TempData["UpdateResponse"] = false;
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                //ViewBag.Categories = _db.Categories.Where(o => id != o.Id && id != o.ParentCategory);
                return View(vm);
            }
            var data = await _db.Categories.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            data.ParentCategoryId = (int?)vm.ParentCategoryId;
            data.IsDeleted = vm.IsDeleted;
            await _db.SaveChangesAsync();
            TempData["UpdateResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> CategoryPagination(int page = 1, int count = 8)
        {
            ViewBag.Categories = _db.Categories;
            var datas = _db.Categories.Skip((page - 1) * count).Take(count).Select(p => new CategoryListItemVM
            {
                Id = p.Id,
                IsDeleted = p.IsDeleted,
                Name = p.Name,
                ParentCategoryId = p.ParentCategoryId,
                ParentCategory = p.ParentCategory,
            });
            int totalCount = await _db.Categories.CountAsync();
            PaginationVM<IEnumerable<CategoryListItemVM>> pag = new(totalCount, page, (int)Math.Ceiling((decimal)totalCount / count), datas);
            return PartialView("_CategoryPaginationPartial", pag);
        }
    }
}
