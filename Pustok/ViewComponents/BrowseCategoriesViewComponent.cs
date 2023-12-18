using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Contexts;
using WebApplication1.ViewModels.BasketVM;
using WebApplication1.ViewModels.CategoryVM;

namespace WebApplication1.ViewComponents
{
	public class BrowseCategoriesViewComponent : ViewComponent
    {
        PustokDbContext _context { get; }

        public BrowseCategoriesViewComponent(PustokDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var vm = await _context.Categories.Select(c => new CategoryListItemVM
            {
                Id = c.Id,
                Name = c.Name,
                IsDeleted = c.IsDeleted,
                ParentCategory = c.ParentCategory,
                ParentCategoryId = c.ParentCategoryId,
            }).ToListAsync();
            return View(vm);
        }
    }
}

