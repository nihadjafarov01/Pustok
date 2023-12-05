using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels.CategoryVM
{
    public class CategoryCreateVM
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public int? ParentCategory { get; set; }
    }
}
