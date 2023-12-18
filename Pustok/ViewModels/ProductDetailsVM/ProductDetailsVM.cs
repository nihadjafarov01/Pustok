using WebApplication1.ViewModels.CategoryVM;
using WebApplication1.ViewModels.CommonVM;
using WebApplication1.ViewModels.ProductVM;

namespace WebApplication1.ViewModels.ProductDetailsVM
{
    public class ProductDetailsVM
    {
        public IEnumerable<ProductListItemVM> Products { get; set; }
        public ProductDetailVM ProductDetail { get; set; }
    }
}
