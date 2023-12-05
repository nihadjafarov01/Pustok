using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Slider
{
    public int Id { get; set; }
    [Required]
    public string ImageUrl { get; set; }
    [Required,MinLength(3),MaxLength(64),DataType("nvarchar")]
    public string Title { get; set; }
    [Required, MinLength(3), MaxLength(128), DataType("nvarchar")]
    public string Text { get; set; }
    public bool? IsLeft { get; set; }
}
