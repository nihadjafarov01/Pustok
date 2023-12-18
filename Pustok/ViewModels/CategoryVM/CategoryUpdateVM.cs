using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels.CategoryVM
{
    public class CategoryUpdateVM
    {
        [MaxLength(16)]
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public bool IsDeleted { get; set; }
    }
}
