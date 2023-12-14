using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Contexts;
using WebApplication1.ViewModels.SliderVM;

namespace WebApplication1.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        PustokDbContext _context {  get;}

        public SliderViewComponent(PustokDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View(await _context.Sliders.Select(s => new SliderListItemVM
            {
                Id = s.Id,
                ImageUrl = s.ImageUrl,
                IsLeft = s.IsLeft,
                Text = s.Text,
                Title = s.Title
            }).ToListAsync());
        }
    }
}
