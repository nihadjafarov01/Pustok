using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels.SliderVM
{
    public class SliderUpdateVM
    {
        [Required]
        public string ImageUrl { get; set; }
        [Required, MinLength(3), MaxLength(64), DataType("nvarchar")]
        public string Title { get; set; }
        [Required, MinLength(3), MaxLength(128), DataType("nvarchar")]
        public string Text { get; set; }
        public sbyte Position { get; set; }
    }
}
