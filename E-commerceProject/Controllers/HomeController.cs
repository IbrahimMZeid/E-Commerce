using E_commerceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace E_commerceProject.Controllers
{
    public class HomeController : Controller
    {
        ECommerceContext eCommerceContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ECommerceContext eCommerceContex)
        {
            _logger = logger;
            this.eCommerceContext = eCommerceContex;
        }

        public IActionResult Index()
        {
            List<Category> categories = eCommerceContext.Categories.Include(c => c.Products).ToList();
            return View(categories);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}