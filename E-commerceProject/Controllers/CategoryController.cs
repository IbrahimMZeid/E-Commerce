using E_commerceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerceProject.Controllers
{
    public class CategoryController : Controller
    {
        ECommerceContext eCommerceContext;
        public CategoryController(ECommerceContext eCommerceContext)
        {
            this.eCommerceContext = eCommerceContext;
        }
        public IActionResult Index(int id)
        {
            if (id == null)
            {
                return RedirectToAction("index", "home");
            }
            Category category = eCommerceContext.Categories.Include(p => p.Products).SingleOrDefault(c => c.Id == id);
            if (category == null)
            {
                return RedirectToAction("index", "home");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult addCategory()
        {
            int? id = HttpContext.Session.GetInt32("AdminId");
            if (id == null)
            {
                return RedirectToAction("login", "user");
            }
            return View();
        }
        [HttpPost]
        public IActionResult addCategory(Category category)
        {
            eCommerceContext.Categories.Add(category);
            eCommerceContext.SaveChanges();
            return RedirectToAction("categoryList");
        }
        [HttpGet]
        public IActionResult categoryList()
        {
            List<Category> categories = eCommerceContext.Categories.ToList();
            return View(categories);
        }
        [HttpGet]
        public IActionResult edit(int id)
        {
            int? AdminId = HttpContext.Session.GetInt32("AdminId");
            if (AdminId == null)
            {
                return RedirectToAction("login", "user");
            }
            if (id == null)
            {
                return RedirectToAction("index", "home");
            }
            Category category = eCommerceContext.Categories.Find(id);
            if (category == null)
            {
                return RedirectToAction("index", "home");
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult edit(Category category)
        {
            if (category == null)
            {
                return RedirectToAction("index", "home");
            }
            Category oldCategory = eCommerceContext.Categories.Find(category.Id);
            oldCategory.Name = category.Name;
            oldCategory.Describtion = category.Describtion;
            eCommerceContext.Update(oldCategory);
            eCommerceContext.SaveChanges();
            return RedirectToAction("index", new { id = category.Id });
        }
        [HttpGet]
        public IActionResult delete(int id)
        {
            int? AdminId = HttpContext.Session.GetInt32("AdminId");
            if (AdminId == null)
            {
                return RedirectToAction("login", "user");
            }
            if (id == null)
            {
                return RedirectToAction("categoryList");
            }
            Category category = eCommerceContext.Categories.Find(id);
            if(category != null)
            {
                eCommerceContext.Categories.Remove(category);
                eCommerceContext.SaveChanges();
            }
            return RedirectToAction("categoryList");
        }
    }
}
