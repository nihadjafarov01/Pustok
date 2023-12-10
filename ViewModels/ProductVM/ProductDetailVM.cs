using WebApplication1.Models;

namespace WebApplication1.ViewModels.ProductVM
{
    public class ProductDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? About { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
        public string HoverImageUrl { get; set; }
        public decimal SellPrice { get; set; }
        public decimal CostPrice { get; set; }
        public float Discount { get; set; }
        public ushort Quantity { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<ProductImages>? ProductImages { get; set; }
    }
}
