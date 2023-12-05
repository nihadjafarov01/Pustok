using WebApplication1.Models;

namespace WebApplication1.ViewModels.ProductImagesVM
{
    public class ProductImagesCreateVM
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int ProductId { get; set; }
    }
}
