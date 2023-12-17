using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; } = null;
        public Category? ParentCategory { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}
