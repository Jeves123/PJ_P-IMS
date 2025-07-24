using Microsoft.AspNetCore.Mvc;

namespace PJ_P_Installation_Management_System.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logout()
        {
            // Clear all session data
            HttpContext.Session.Clear();

            // Redirect to login page
            return RedirectToAction("Index", "Login");
        }
    }
}
