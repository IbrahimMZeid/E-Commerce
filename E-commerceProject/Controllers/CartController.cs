using E_commerceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerceProject.Controllers
{
    public class CartController : Controller
    {
        ECommerceContext eCommerceContext;
        public CartController(ECommerceContext eCommerceContext)
        {
            this.eCommerceContext = eCommerceContext;
        }
        public IActionResult Index(int id)
        {
            List<Cart> progressCarts = new List<Cart>();
            List<Cart> doneCarts = new List<Cart>();
            Dictionary<string, List<Cart>> map = new Dictionary<string, List<Cart>>();
            int? AdminId = HttpContext.Session.GetInt32("AdminId");
            if (AdminId == null)
            {
                int? UserId = HttpContext.Session.GetInt32("UserId");
                if (UserId == null)
                    return RedirectToAction("login", "user");
                progressCarts = eCommerceContext.Carts.Include(u => u.User).Include(p => p.Product).ThenInclude(c => c.Category).Where(c => c.UserId == UserId && c.Stats.Equals("In Progress")).ToList();
                doneCarts = eCommerceContext.Carts.Include(u => u.User).Include(p => p.Product).ThenInclude(c => c.Category).Where(c => c.UserId == UserId && c.Stats.Equals("Done")).ToList();

            }
            else
            {
                progressCarts = eCommerceContext.Carts.Include(c => c.User).Include(c => c.Product).ThenInclude(c => c.Category).Where(c => c.Stats.Equals("In Progress")).ToList();
                doneCarts = eCommerceContext.Carts.Include(c => c.User).Include(c => c.Product).ThenInclude(c => c.Category).Where(c => c.Stats.Equals("Done")).ToList();
            }
            map["Progress"] = progressCarts;
            map["Done"] = doneCarts;
            return View(map);
        }
        public IActionResult addToCart(Cart cart)
        {
            int? userId = HttpContext.Session.GetInt32("AdminId") ?? HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                User user = new User();
                user.ErrorMsg = "Login first";
                return RedirectToAction("login", "user", user);
            }
            Product product = eCommerceContext.Products.Find(cart.ProductId);
            if (product != null)
            {
                //Cart cart = new Cart();
                //cart.ProductId = productid;
                cart.UserId = userId ?? 0;
                cart.Price = product.Price;
                cart.Stats = "In Progress";
                cart.Time = DateTime.Now;
                //cart.Number = number;
                eCommerceContext.Carts.Add(cart);
                eCommerceContext.SaveChanges();
            }
            return RedirectToAction("index", "home");
        }
        public IActionResult removeFromCart(int userId, int productId, DateTime time)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            Cart cart = new Cart();
            if (adminId == null)
            {
                int? userfound = HttpContext.Session.GetInt32("UserId");
                if (userfound == null)
                {
                    User user = new User();
                    user.ErrorMsg = "Login first";
                    return RedirectToAction("login", "user", user);
                }
                cart = eCommerceContext.Carts.SingleOrDefault(c => c.UserId == userfound && c.ProductId == productId && c.Time.Date == time.Date);
            }
            else
            {
                if (userId == null)
                {
                    User user = new User();
                    user.ErrorMsg = "Login first";
                    return RedirectToAction("login", "user", user);
                }
                cart = eCommerceContext.Carts.SingleOrDefault(c => c.UserId == userId && c.ProductId == productId && c.Time.Date == time.Date && c.Time.Hour == time.Hour && c.Time.Minute == time.Minute && c.Time.Second == time.Second);
            }
            if (cart != null)
            {
                eCommerceContext.Carts.Remove(cart);
                eCommerceContext.SaveChanges();
            }
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult changeState(int userId, int productId, DateTime time, string stats)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                User user = new User();
                user.ErrorMsg = "Login first";
                return RedirectToAction("login", "user", user);
            }
            Cart cart = eCommerceContext.Carts.SingleOrDefault(c => c.UserId == userId && c.ProductId == productId && c.Time.Date == time.Date && c.Time.Hour == time.Hour && c.Time.Minute == time.Minute && c.Time.Second == time.Second);
            if (cart != null)
            {
                cart.Stats = stats;
                eCommerceContext.Carts.Update(cart);
                eCommerceContext.SaveChanges();
            }
            return RedirectToAction("index");
        }
    }
}
