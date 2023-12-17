using WebApplication1.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.ViewModels.SliderVM;
using WebApplication1.Areas.Admin.ViewModels.CommonVM;
using WebApplication1.Areas.Admin.ViewModels;
using WebApplication1.ViewModels.CommonVM;
using WebApplication1.ViewModels.ProductVM;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        PustokDbContext _db { get; }

        public SliderController(PustokDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var datas = _db.Sliders.Take(2).Select(p => new SliderListItemVM
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                IsLeft = p.IsLeft,
                Text = p.Text,
                Title = p.Title
            });
            int count = await _db.Sliders.CountAsync();
            PaginationVM<IEnumerable<SliderListItemVM>> pag = new(count, 1, (int)Math.Ceiling((decimal)count / 2), datas);
            return View(pag);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM vm)
        {
            if (vm.Position < -1 || vm.Position > 1)
            {
                ModelState.AddModelError("Position", "Wrong input");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Slider slider = new Slider()
            {
                Title = vm.Title,
                Text = vm.Text,
                ImageUrl = vm.ImageUrl,
                IsLeft = vm.Position switch
                {
                    0 => null,
                    -1 => true,
                    1 => false
                }
            };
            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            TempData["SliderDeleteResponse"] = false;
            if (id == null) return BadRequest();
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            _db.Sliders.Remove(data);
            await _db.SaveChangesAsync();
            TempData["SliderDeleteResponse"] = true;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            return View(new SliderUpdateVM
            {
                ImageUrl = data.ImageUrl,
                Position = data.IsLeft switch
                {
                    true => -1,
                    null => 0,
                    false => 1
                },
                Text = data.Text,
                Title = data.Title,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, SliderUpdateVM vm)
        {
            TempData["SliderUpdateResponse"] = false;
            if (id == null || id <= 0) return BadRequest();
            if (vm.Position < -1 || vm.Position > 1)
            {
                ModelState.AddModelError("Postion", "Wrong input");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            data.Text = vm.Text;
            data.Title = vm.Title;
            data.ImageUrl = vm.ImageUrl;
            data.IsLeft = vm.Position switch
            {
                0 => null,
                -1 => true,
                1 => false
            };
            await _db.SaveChangesAsync();
            TempData["SliderUpdateResponse"] = true;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SliderPagination(int page = 1, int count = 8)
        {
            var datas = _db.Sliders.Skip((page - 1) * count).Take(count).Select(p => new SliderListItemVM
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                IsLeft = p.IsLeft,
                Text = p.Text,
                Title = p.Title,
            });
            int totalCount = await _db.Sliders.CountAsync();
            PaginationVM<IEnumerable<SliderListItemVM>> pag = new(totalCount, page, (int)Math.Ceiling((decimal)totalCount / count), datas);
            return PartialView("_SliderPaginationPartial", pag);
        }
    }
}
