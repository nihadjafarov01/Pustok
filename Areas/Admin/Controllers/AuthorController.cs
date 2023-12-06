using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.ViewModels.AuthorVM;
using WebApplication1.ViewModels.BlogVM;
using WebApplication1.ViewModels.CategoryVM;
using WebApplication1.ViewModels.ProductVM;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        PustokDbContext _db { get; }

        public AuthorController(PustokDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var items = await _db.Authors.Select(s => new AuthorListItemVM
            {
                Id = s.Id,
                Name = s.Name,
                Surname = s.Surname,
                Blogs = s.Blogs
            }).ToListAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuthorCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Author auth = new Author
            {
                Name = vm.Name,
                Surname = vm.Surname
            };
            await _db.Authors.AddAsync(auth);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Authors.FindAsync(id);
            if (data == null) return NotFound();
            return View(new AuthorUpdateVM
            {
                Name = data.Name,
                Surname = data.Surname,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, AuthorUpdateVM vm)
        {
            TempData["UpdateResponse"] = false;
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Authors.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            data.Surname = vm.Surname;
            await _db.SaveChangesAsync();
            TempData["UpdateResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["DeleteResponse"] = false;
            if (id == null) return BadRequest();
            var data = await _db.Authors.FindAsync(id);
            if (data == null) return NotFound();
            _db.Authors.Remove(data);
            await _db.SaveChangesAsync();
            TempData["DeleteResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}
