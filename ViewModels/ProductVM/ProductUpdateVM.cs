using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels.ProductVM
{
    public class ProductUpdateVM
    {
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string? ImagePath { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? HoverImagePath { get; set; }
        public IFormFile? HoverImageFile { get; set; }
        public List<ProductImages>? ImageUrls { get; set; }
        public List<IFormFile>? Images { get; set; }
        public string? About { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal SellPrice { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal CostPrice { get; set; }
        [Range(0, 100)]
        public float Discount { get; set; }
        public ushort Quantity { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<int> TagIds { get; set; }
    }
}
