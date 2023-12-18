using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels.BlogVM
{
    public class BlogCreateVM
    {
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public IEnumerable<int> TagIds { get; set; }

    }
}
