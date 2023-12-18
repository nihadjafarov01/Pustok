using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels.TagVM
{
    public class TagCreateVM
    {
        [Required, MaxLength()]
        public string Title { get; set; }
    }
}
