using WebApplication1.Models;

namespace WebApplication1.ViewModels.ProductTagsVM
{
    public class ProductTagsCreateVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
