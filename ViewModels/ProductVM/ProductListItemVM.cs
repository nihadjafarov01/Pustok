using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels.ProductVM
{
    public class ProductListItemVM
    {
        public string Name { get; set; }
        public decimal SellPrice { get; set; }
        public decimal CostPrice { get; set; }
        public float Discount { get; set; }
        public ushort Quantity { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; } 
        public List<ProductImages>? ProductImages { get; set; }
    }
}
