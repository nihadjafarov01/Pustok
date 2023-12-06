using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels.AuthorVM
{
    public class AuthorUpdateVM
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Surname { get; set; }
        public List<Blog>? Blogs { get; set; }
    }
}
