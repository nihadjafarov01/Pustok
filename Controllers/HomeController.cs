using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication1.Contexts;
using WebApplication1.Models;
using WebApplication1.ViewModels.HomeVM;
using WebApplication1.ViewModels.ProductVM;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public PustokDbContext _context { get; set; }

        public HomeController(PustokDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Sliders = _context.Sliders;
            ViewBag.Products = _context.Products;
            ViewBag.Categories = _context.Categories;

            return View(ViewBag);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }
            var data = await _context.Products.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            ViewBag.Sliders = _context.Sliders;
            ViewBag.Products = _context.Products;
            ViewBag.Categories = _context.Categories;
            return View(new ProductDetailVM
            {
                Name = data.Name,
                About = data.About,
                Description = data.Description,
                ImageUrl = data.ImageUrl,
                HoverImageUrl = data.HoverImageUrl,
                SellPrice = data.SellPrice,
                CostPrice = data.CostPrice,
                Discount = data.Discount,
                Quantity = data.Quantity,
                CategoryId = data.CategoryId,
                IsDeleted = data.IsDeleted,
                ProductImages = data.Images
            });
        }
    }
}
