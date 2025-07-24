using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PJ_P_Installation_Management_System.Models;

namespace PJ_P_Installation_Management_System.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public IActionResult Index()
    {
        return View(); // This should return Views/Home/Index.cshtml
    }
}
