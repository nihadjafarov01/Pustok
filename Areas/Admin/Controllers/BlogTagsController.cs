using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.ViewModels.BlogTagsVM;
using WebApplication1.ViewModels.BlogVM;
using WebApplication1.ViewModels.TagVM;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogTagsController : Controller
    {
        PustokDbContext _db { get; }

        public BlogTagsController(PustokDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var items = await _db.BlogTags.Select(s => new BlogTagsListItemVM
            {
                Id = s.Id,
                BlogId = s.BlogId,
                TagId = s.TagId,
            }).ToListAsync();
            return View(items);
        }
        public IActionResult Create()
        {
            ViewBag.Tags = _db.Tags;
            ViewBag.Blogs = _db.Blogs;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BlogTagsCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            BlogTags blogtag = new BlogTags()
            {
                BlogId = vm.BlogId,
                TagId = vm.TagId,
            };
            await _db.BlogTags.AddAsync(blogtag);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.BlogTags.FindAsync(id);
            if (data == null) return NotFound();
            ViewBag.Blogs = _db.Blogs;
            ViewBag.Tags = _db.Tags;
            return View(new BlogTagsUpdateVM
            {
                BlogId = data.BlogId,
                TagId = data.TagId,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, BlogTagsUpdateVM vm)
        {
            TempData["UpdateResponse"] = false;
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Blogs = _db.Blogs;
                ViewBag.Tags = _db.Tags;
                return View(vm);
            }
            var data = await _db.BlogTags.FindAsync(id);
            if (data == null) return NotFound();
            data.TagId = vm.TagId;
            data.BlogId = vm.BlogId;
            await _db.SaveChangesAsync();
            TempData["UpdateResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["BlogTagsDeleteResponse"] = false;
            if (id == null) return BadRequest();
            var data = await _db.BlogTags.FindAsync(id);
            if (data == null) return NotFound();
            _db.BlogTags.Remove(data);
            await _db.SaveChangesAsync();
            TempData["BlogTagsDeleteResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
    }
}
