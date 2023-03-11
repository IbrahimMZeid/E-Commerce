using E_commerceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E_commerceProject.Controllers
{
    public class ProductController : Controller
    {
        ECommerceContext eCommerceContext;
        public ProductController(ECommerceContext eCommerceContext)
        {
            this.eCommerceContext = eCommerceContext;
        }
        public IActionResult Index(int id)
        {
            if (id == null)
                return RedirectToAction("index", "home");
            Product product = eCommerceContext.Products.Include(c => c.Carts).Include(c => c.Category).SingleOrDefault(p => p.Id == id);
            if (product == null)
                return RedirectToAction("index", "home");
            Cart cart = new Cart();
            ViewBag.Cart = cart;
            return View(product);
        }
        [HttpGet]
        public IActionResult addProduct()
        {
            int? id = HttpContext.Session.GetInt32("AdminId");
            if (id == null)
            {
                return RedirectToAction("login", "user");
            }
            List<Category> category = eCommerceContext.Categories.ToList();
            SelectList select = new SelectList(category, "Id", "Name");
            ViewBag.category = select;
            return View();
        }
        [HttpPost]
        public IActionResult addProduct(Product product, IFormFile Image)
        {
            string path = $"wwwroot/img/Product/{Image.FileName}";
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                Image.CopyTo(fs);
                fs.Close();
            }
            product.Image = $"/img/Product/{Image.FileName}";
            eCommerceContext.Products.Add(product);
            eCommerceContext.SaveChanges();
            return RedirectToAction("addproduct");
        }
        [HttpGet]
        public IActionResult edit(int id)
        {
            if (id == null)
            {
                return RedirectToAction("index", "home");
            }
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("login", "user");
            }
            Product product = eCommerceContext.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("index", "home");
            }
            List<Category> category = eCommerceContext.Categories.ToList();
            SelectList select = new SelectList(category, "Id", "Name");
            ViewBag.category = select;
            return View(product);
        }
        [HttpPost]
        public IActionResult edit(Product product)
        {
            Product oldProduct = eCommerceContext.Products.Find(product.Id);
            oldProduct.Title = product.Title;
            oldProduct.Proccessor = product.Proccessor;
            oldProduct.OS = product.OS;
            oldProduct.Ram = product.Ram;
            oldProduct.Storage = product.Storage;
            oldProduct.Describtion = product.Describtion;
            oldProduct.Price = product.Price;
            oldProduct.Discount = product.Discount;
            eCommerceContext.Products.Update(oldProduct);
            eCommerceContext.SaveChanges();
            return RedirectToAction("index", "product", new { id = product.Id });
        }
        [HttpGet]
        public IActionResult changePicture(int id)
        {
            if (id == null)
            {
                return RedirectToAction("index", "home");
            }
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("login", "user");
            }
            Product product = eCommerceContext.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("index", "home");
            }
            return View(product);
        }
        [HttpPost]
        public IActionResult changePicture(Product product, IFormFile Image)
        {

            string path = $"wwwroot/img/Product/{Image.FileName}";
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                Image.CopyTo(fs);
                fs.Close();
            }
            Product oldProduct = eCommerceContext.Products.Find(product.Id);
            List<Product> findImage = eCommerceContext.Products.Where(p => p.Image.Equals(oldProduct.Image)).ToList();
            if (findImage.Count == 1)
            {
                path = $"wwwroot{oldProduct.Image}";
                fileInfo = new FileInfo(path);
                fileInfo.Delete();
            }
            oldProduct.Image = $"/img/Product/{Image.FileName}";
            eCommerceContext.Products.Update(oldProduct);
            eCommerceContext.SaveChanges();
            return RedirectToAction("index", "product", new { id = product.Id });

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
                return RedirectToAction("index", "home");
            }
            Product product = eCommerceContext.Products.Find(id);
            if (product != null)
            {
                eCommerceContext.Products.Remove(product);
                eCommerceContext.SaveChanges();

                string path = $"wwwroot{product.Image}";
                FileInfo fileInfo = new FileInfo(path);
                List<Product> findImage = eCommerceContext.Products.Where(p => p.Image.Equals(product.Image)).ToList();
                if (findImage.Count == 0)
                {
                    path = $"wwwroot{product.Image}";
                    fileInfo = new FileInfo(path);
                    fileInfo.Delete();
                }

                return RedirectToAction("index", "Category", new { product.CategoryId });
            }
            return RedirectToAction("index", "home");
        }
    }
}
