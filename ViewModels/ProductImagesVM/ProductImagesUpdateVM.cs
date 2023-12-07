using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels.ProductImagesVM
{
    public class ProductImagesUpdateVM
    {
        public IFormFile? ImageFile { get; set; }
        public string? ImagePath { get; set; }
        [Required]
        public int ProductId { get; set; }
        public bool IsActive { get; set; }
    }
}
