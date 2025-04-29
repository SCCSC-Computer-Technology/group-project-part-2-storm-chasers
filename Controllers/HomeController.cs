using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StormChasersGroupProject2.Models;

namespace StormChasersGroupProject2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    //displays home page
    public IActionResult Index()
    {
        return View();
    }

    //displays error page
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
