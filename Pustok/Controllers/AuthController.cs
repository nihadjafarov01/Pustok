using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
