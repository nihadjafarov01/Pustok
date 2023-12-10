using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public bool IsDeleted { get; set; } = false;
        public IEnumerable<BlogTags?> BlogTags { get; set; }
    }
}
