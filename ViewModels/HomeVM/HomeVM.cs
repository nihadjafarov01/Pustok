using WebApplication1.Models;
using WebApplication1.ViewModels.CategoryVM;
using WebApplication1.ViewModels.CommonVM;
using WebApplication1.ViewModels.ProductVM;
using WebApplication1.ViewModels.SliderVM;

namespace WebApplication1.ViewModels.HomeVM
{
    public class HomeVM
    {
        public IEnumerable<SliderListItemVM> Sliders { get; set; }
        public IEnumerable<CategoryListItemVM> Categories { get; set; }
        public IEnumerable<ProductListItemVM> Products { get; set; }
        public PaginationVM<IEnumerable<ProductListItemVM>> PaginatedProducts { get; set; }
    }
}
