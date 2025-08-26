using Chat.App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Chat.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
