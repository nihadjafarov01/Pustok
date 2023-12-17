using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels.TagVM
{
    public class TagListItemVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<BlogTags>? BlogTags { get; set; }
    }
}
