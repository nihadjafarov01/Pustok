namespace WebApplication1.Helpers
{
    public class PathConstants
    {
        IWebHostEnvironment _env;

        public PathConstants(IWebHostEnvironment env)
        {
            _env = env;
        }

        public static string Product => Path.Combine("image", "products");
        public static string RootPath {  get; set; }
    }
}
