using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Author
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Surname { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
