using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.ViewModels.BlogTagsVM;
using WebApplication1.ViewModels.CommonVM;
using WebApplication1.ViewModels.ProductTagsVM;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTagsController : Controller
    {
        PustokDbContext _db { get; }

        public ProductTagsController(PustokDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var datas = _db.ProductTags.Take(2).Select(p => new ProductTagsListItemVM
            {
                Id = p.Id,
                Product = p.Product,
                Tag = p.Tag,
                TagId = p.TagId,
                ProductId = p.ProductId
            });
            int count = await _db.ProductTags.CountAsync();
            PaginationVM<IEnumerable<ProductTagsListItemVM>> pag = new(count, 1, (int)Math.Ceiling((decimal)count / 2), datas);
            return View(pag);
        }

        public IActionResult Create()
        {
            ViewBag.Tags = _db.Tags;
            ViewBag.Products = _db.Products;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductTagsCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            ProductTags producttag = new ProductTags()
            {
                TagId = vm.TagId,
                ProductId = vm.ProductId
            };
            await _db.ProductTags.AddAsync(producttag);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ProductTagsPagination(int page = 1, int count = 8)
        {
            var datas = _db.ProductTags.Skip((page - 1) * count).Take(count).Select(p => new ProductTagsListItemVM
            {
                Id = p.Id,
                TagId = p.TagId,
                Tag = p.Tag,
                ProductId= p.ProductId,
                Product = p.Product
            });
            int totalCount = await _db.ProductTags.CountAsync();
            PaginationVM<IEnumerable<ProductTagsListItemVM>> pag = new(totalCount, page, (int)Math.Ceiling((decimal)totalCount / count), datas);
            return PartialView("_ProductTagsPaginationPartial", pag);
        }
    }
}
