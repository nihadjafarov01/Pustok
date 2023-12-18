using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using WebApplication1.Areas.Admin.ViewModels;
using WebApplication1.Contexts;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.ViewModels.CategoryVM;
using WebApplication1.ViewModels.CommonVM;
using WebApplication1.ViewModels.HomeVM;
using WebApplication1.ViewModels.ProductVM;
using WebApplication1.ViewModels.SliderVM;
using WebApplication1.Areas.Admin.ViewModels.CommonVM;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        PustokDbContext _db { get; }
        IWebHostEnvironment _env { get; }

        public ProductController(PustokDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var datas = _db.Products.Take(2).Select(p => new AdminProductListItemVM
            {
                Id = p.Id,
                Name = p.Name,
                SellPrice = p.SellPrice,
                Quantity = p.Quantity,
                Images = p.Images,
                Category = p.Category,
                CostPrice = p.CostPrice,
                Discount = p.Discount,
                HoverImageUrl = p.HoverImageUrl,
                ImageUrl = p.ImageUrl,
                IsDeleted = p.IsDeleted,
                Tags = p.ProductTags.Select(p => p.Tag)
            });
            int count = await _db.Products.CountAsync();
            PaginationVM<IEnumerable<AdminProductListItemVM>> pag = new(count, 1, (int)Math.Ceiling((decimal)count / 2), datas);
            return View(pag);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories;
            ViewBag.Tags = new SelectList(_db.Tags, "Id", "Title");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (vm.CostPrice > vm.SellPrice)
            {
                ViewBag.Categories = _db.Categories;
                ViewBag.Tags = new SelectList(_db.Tags, "Id", "Title");
                ModelState.AddModelError("CostPrice", "Sell price must be bigger than cost price");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _db.Categories;
                ViewBag.Tags = new SelectList(_db.Tags, "Id", "Title");
                return View(vm);
            }
            if (vm.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "Category must be selected");
                ViewBag.Categories = _db.Categories;
                ViewBag.Tags = new SelectList(_db.Tags, "Id", "Title");
                return View(vm);
            }
            if (!await _db.Categories.AnyAsync(x => x.Id == vm.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category doesn't exist");
                ViewBag.Categories = _db.Categories;
                ViewBag.Tags = new SelectList(_db.Tags, "Id", "Title");
                return View(vm);
            }
            string fileName = Path.Combine("image", "products", vm.ImageFile.FileName);
            using (FileStream fs = System.IO.File.Create(Path.Combine(_env.WebRootPath, fileName)))
            {
                await vm.ImageFile.CopyToAsync(fs);
            }
            string? hoverFileName = null;
            if (vm.HoverImageFile != null)
            {
                hoverFileName = Path.Combine("image", "products", vm.HoverImageFile.FileName);
                using (FileStream fs = System.IO.File.Create(Path.Combine(_env.WebRootPath, hoverFileName)))
                {
                    await vm.HoverImageFile.CopyToAsync(fs);
                }
            }
            Product prod = new Product
            {
                Name = vm.Name,
                About = vm.About,
                Quantity = vm.Quantity,
                Description = vm.Description,
                Discount = vm.Discount,
                ImageUrl = fileName,
                HoverImageUrl = hoverFileName,
                CostPrice = vm.CostPrice,
                SellPrice = vm.SellPrice,
                CategoryId = vm.CategoryId,
                Images = vm.Images.Select(i => new ProductImages
                {
                    ImagePath = i.SaveAsync(PathConstants.Product).Result
                }).ToList(),
                ProductTags = vm.TagIds.Select(id => new Models.ProductTags
                {
                    TagId = id,
                }).ToList()
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
            data.IsDeleted = true;
            _db.Products.Update(data);
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
            var data = await _db.Products.Include(b => b.ProductTags).SingleOrDefaultAsync(b => b.Id == id);
            //var data = await _db.Products.FindAsync(id);
            if (data == null)
            {
                return NotFound();  
            }
            ViewBag.Categories = _db.Categories;
            ViewBag.ProductImages = _db.ProductImages.Where(pi => pi.ProductId == id).ToList();
            ViewBag.Tags = new SelectList(_db.Tags, "Id", "Title");
            ProductUpdateVM returnData = new ProductUpdateVM
            {
                Name = data.Name,
                About = data.About,
                Description = data.Description,
                ImagePath = data.ImageUrl,
                HoverImagePath = data.HoverImageUrl,
                SellPrice = data.SellPrice,
                CostPrice = data.CostPrice,
                Discount = data.Discount,
                Quantity = data.Quantity,
                CategoryId = data.CategoryId,
                IsDeleted = data.IsDeleted,
                ImageUrls = _db.ProductImages.Where(pi => pi.ProductId == id).ToList(),
                TagIds = data.ProductTags.Select(p => p.TagId),
            };
            return View(returnData);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
        {
            TempData["ProductUpdateResponse"] = false;
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _db.Categories;
                ViewBag.ProductImages = _db.ProductImages.Where(pi => pi.ProductId == id).ToList();
                return View(vm);
            }
            var data = await _db.Products.Include(b => b.ProductTags).SingleOrDefaultAsync(b => b.Id == id);
            if (data == null)
            {
                ViewBag.Categories = _db.Categories;
                ViewBag.ProductImages = _db.ProductImages.Where(pi => pi.ProductId == id).ToList();
                return NotFound();
            }
            data.Name = vm.Name;
            data.About = vm.About;
            data.Description = vm.Description;
            data.SellPrice = vm.SellPrice;
            data.CostPrice = vm.CostPrice;
            data.Discount = vm.Discount;
            data.Quantity = vm.Quantity;
            data.CategoryId = vm.CategoryId;
            data.IsDeleted = vm.IsDeleted;
            if (vm.ImageFile != null)
            {
                string fileName = Path.Combine("image", "products", vm.ImageFile.FileName);
                using (FileStream fs = System.IO.File.Create(Path.Combine(_env.WebRootPath, fileName)))
                {
                    await vm.ImageFile.CopyToAsync(fs);
                }
                data.ImageUrl = fileName;
            }
            if (vm.HoverImageFile != null)
            {
                string hoverFileName = Path.Combine("image", "products", vm.HoverImageFile.FileName);
                using (FileStream fs = System.IO.File.Create(Path.Combine(_env.WebRootPath, hoverFileName)))
                {
                    await vm.HoverImageFile.CopyToAsync(fs);
                }
                data.HoverImageUrl = hoverFileName;
            }
            if (!Enumerable.SequenceEqual(data.ProductTags?.Select(p => p.TagId),vm.TagIds))
            {
                data.ProductTags = vm.TagIds.Select(t => new ProductTags { TagId = t }).ToList();
            }
            await _db.SaveChangesAsync();
            TempData["ProductUpdateResponse"] = true;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ProductPagination(int page = 1, int count = 8)
        {
            var datas = _db.Products.Skip((page - 1) * count).Take(count).Select(p => new AdminProductListItemVM
            {
                Id = p.Id,
                Name = p.Name,
                CostPrice = p.CostPrice,
                Images = p.Images,
                Category = p.Category,
                IsDeleted = p.IsDeleted,
                Quantity = p.Quantity,
                Discount = p.Discount,
                ImageUrl = p.ImageUrl,
                HoverImageUrl = p.HoverImageUrl,
                SellPrice = p.SellPrice,
            });
            int totalCount = await _db.Products.CountAsync();
            PaginationVM<IEnumerable<AdminProductListItemVM>> pag = new(totalCount, page, (int)Math.Ceiling((decimal)totalCount / count), datas);
            return PartialView("_ProductPaginationPartial" ,pag);
        }
    }
}
