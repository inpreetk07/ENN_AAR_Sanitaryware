using Microsoft.AspNetCore.Mvc;

namespace ENN_AAR_Sanitaryware.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Index()
        {
            // There is no Demo view; send visitors to the homepage instead of an error page.
            return RedirectToAction("Index", "Home");
        }
    }
}
