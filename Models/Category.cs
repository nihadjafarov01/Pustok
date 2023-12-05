using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public int? ParentCategory { get; set; } = null;
        public bool IsDeleted { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}
