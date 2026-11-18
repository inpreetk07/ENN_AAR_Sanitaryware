using Microsoft.AspNetCore.Mvc;

namespace ENN_AAR_Sanitaryware.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
