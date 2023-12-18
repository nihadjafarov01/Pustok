using WebApplication1.Models;

namespace WebApplication1.ViewModels.BlogTagsVM
{
    public class BlogTagsListItemVM
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
