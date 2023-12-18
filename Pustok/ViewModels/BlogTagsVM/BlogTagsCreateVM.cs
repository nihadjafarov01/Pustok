using WebApplication1.Models;

namespace WebApplication1.ViewModels.BlogTagsVM
{
    public class BlogTagsCreateVM
    {
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }
        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
