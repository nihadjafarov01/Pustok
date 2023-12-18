using Microsoft.AspNetCore.Mvc;
using WebApplication1.Contexts;

namespace WebApplication1.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        PustokDbContext _context {  get; }

        public HeaderViewComponent(PustokDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
