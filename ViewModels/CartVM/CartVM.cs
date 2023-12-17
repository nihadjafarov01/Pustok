using WebApplication1.ViewModels.BasketVM;
using WebApplication1.ViewModels.ProductVM;

namespace WebApplication1.ViewModels.CartVM
{
	public class CartVM
	{
		public IEnumerable<ProductListItemVM> Products { get; set; }
		public List<BasketProductItemVM> BasketProducts { get; set; }
	}
}
