using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels.TagVM
{
    public class TagUpdateVM
    {
        [Required, MaxLength()]
        public string Title { get; set; }
    }
}
