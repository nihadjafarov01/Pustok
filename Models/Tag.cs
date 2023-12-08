using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Tag
    {
        public int Id{ get; set; }
        [Required ,MaxLength()]
        public string Title{ get; set; }
    }
}
