using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WebApplication1.Areas.Admin.ViewModels.CommonVM;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.ViewModels.AuthorVM;
using WebApplication1.ViewModels.BlogTagsVM;
using WebApplication1.ViewModels.BlogVM;
using WebApplication1.ViewModels.CategoryVM;
using WebApplication1.ViewModels.CommonVM;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        PustokDbContext _db { get; }

        public BlogController(PustokDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var datas = _db.Blogs.Take(2).Select(p => new BlogListItemVM
            {
                Id = p.Id,
                IsDeleted = p.IsDeleted,
                Author = p.Author,
                AuthorId = p.AuthorId,
                CreatedAt = p.CreatedAt,
                Description = p.Description,
                Tags = p.BlogTags.Select(b => b.Tag),
                Title = p.Title,
                UpdatedAt = p.UpdatedAt
            });
            int count = await _db.Blogs.CountAsync();
            PaginationVM<IEnumerable<BlogListItemVM>> pag = new(count, 1, (int)Math.Ceiling((decimal)count / 2), datas);
            return View(pag);
        }

        public IActionResult Create()
        {
            ViewBag.Authors = _db.Authors;
            ViewBag.Tags = new SelectList(_db.Tags,"Id","Title");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BlogCreateVM vm)
        {
            if (vm.AuthorId == 0)
            {
                ModelState.AddModelError("AuthorId", "Authors must be selected");
                ViewBag.Authors = _db.Authors;
                ViewBag.Tags = new SelectList(_db.Tags, "Id", "Title");
                return View(vm);
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = _db.Authors;
                ViewBag.Tags = new SelectList(_db.Tags, "Id", "Title");
                return View(vm);
            }
            await _db.Blogs.AddAsync(new Models.Blog
            {
                Title = vm.Title,
                Description = vm.Description,
                AuthorId = vm.AuthorId,
                BlogTags = vm.TagIds.Select(id => new Models.BlogTags
                {
                    TagId = id,
                }).ToList()
            });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Blogs.Include(b => b.BlogTags).SingleOrDefaultAsync(b => b.Id == id);
            if (data == null) return NotFound();
            ViewBag.Authors = _db.Authors;
            ViewBag.Tags = new SelectList(_db.Tags, "Id", "Title");
            //ViewBag.BlogTags = _db.BlogTags.Select(t => t.BlogId == id);
            var returnData = new BlogUpdateVM
            {
                Title = data.Title,
                Description = data.Description,
                AuthorId = data.AuthorId,
                IsDeleted = data.IsDeleted,
                TagIds = data.BlogTags.Select(t => t.Id)

            };
            return View(returnData);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, BlogUpdateVM vm)
        {
            TempData["UpdateResponse"] = false;
            if (id == null || id <= 0) return BadRequest();
            if (vm.AuthorId == 0)
            {
                ModelState.AddModelError("AuthorId","Author must be selected");
                ViewBag.Authors = _db.Authors;
                return View(vm);
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = _db.Authors;
                return View(vm);
            }
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            data.Title = vm.Title;
            data.Description = vm.Description;
            data.AuthorId = vm.AuthorId;
            data.IsDeleted = vm.IsDeleted;
            data.UpdatedAt = DateTime.Now;
            if (!Enumerable.SequenceEqual(data.BlogTags?.Select(p => p.TagId), vm.TagIds))
            {
                data.BlogTags = vm.TagIds.Select(t => new BlogTags { TagId = t }).ToList();
            }
            await _db.SaveChangesAsync();
            TempData["UpdateResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["DeleteResponse"] = false;
            if (id == null) return BadRequest();
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            data.IsDeleted = true;
            _db.Blogs.Update(data);
            await _db.SaveChangesAsync();
            TempData["DeleteResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> BlogPagination(int page = 1, int count = 8)
        {
            var datas = _db.Blogs.Skip((page - 1) * count).Take(count).Select(p => new BlogListItemVM
            {
                Id = p.Id,
                Tags = p.BlogTags.Select(b => b.Tag),
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Title = p.Title,
                Author = p.Author,
                AuthorId = p.AuthorId,
                IsDeleted = p.IsDeleted,
                Description = p.Description,
            });
            int totalCount = await _db.Blogs.CountAsync();
            PaginationVM<IEnumerable<BlogListItemVM>> pag = new(totalCount, page, (int)Math.Ceiling((decimal)totalCount / count), datas);
            return PartialView("_BlogPaginationPartial", pag);
        }
    }
}
