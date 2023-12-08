using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.ViewModels.CategoryVM;
using WebApplication1.ViewModels.SliderVM;
using WebApplication1.ViewModels.TagVM;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        PustokDbContext _db { get; }

        public TagController(PustokDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var items = await _db.Tags.Select(s => new TagListItemVM
            {
                Id = s.Id,
                Title = s.Title,
            }).ToListAsync();
            return View(items);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TagCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Tag tag = new Tag()
            {
                Title = vm.Title
            };
            await _db.Tags.AddAsync(tag);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Tags.FindAsync(id);
            if (data == null) return NotFound();
            return View(new TagUpdateVM
            {
                Title = data.Title,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, TagUpdateVM vm)
        {
            TempData["UpdateResponse"] = false;
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Tags.FindAsync(id);
            if (data == null) return NotFound();
            data.Title = vm.Title;
            await _db.SaveChangesAsync();
            TempData["UpdateResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["TagDeleteResponse"] = false;
            if (id == null) return BadRequest();
            var data = await _db.Tags.FindAsync(id);
            if (data == null) return NotFound();
            _db.Tags.Remove(data);
            await _db.SaveChangesAsync();
            TempData["TagDeleteResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}
