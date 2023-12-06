using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels.CategoryVM
{
    public class CategoryUpdateVM
    {
        [MaxLength(16)]
        public string Name { get; set; }
        public int? ParentCategory { get; set; }
        public bool IsDeleted { get; set; }
    }
}
