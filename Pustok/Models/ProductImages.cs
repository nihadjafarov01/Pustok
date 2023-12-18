namespace WebApplication1.Models
{
    public class ProductImages
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
