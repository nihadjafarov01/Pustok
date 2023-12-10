using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels.BlogVM
{
    public class BlogListItemVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public bool IsDeleted { get; set; } = false;
        public IEnumerable<Tag?> Tags { get; set; }
    }
}
