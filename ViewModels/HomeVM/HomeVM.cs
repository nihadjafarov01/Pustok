using WebApplication1.Models;
using WebApplication1.ViewModels.ProductVM;

namespace WebApplication1.ViewModels.HomeVM
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductListItemVM> PaginatedProducts { get; set; }
    }
}
