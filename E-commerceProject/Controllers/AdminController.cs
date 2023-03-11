using E_commerceProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceProject.Controllers
{
    public class AdminController : Controller
    {
        ECommerceContext eCommerceContext;
        public AdminController(ECommerceContext eCommerceContext)
        {
            this.eCommerceContext = eCommerceContext;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet]
        public IActionResult addAdmin()
        {
            int? id = HttpContext.Session.GetInt32("AdminId");
            if (id == null)
                return RedirectToAction("login", "user");
            Admin admin = new Admin();
            return View(admin);
        }
        [HttpPost]
        public IActionResult addAdmin(Admin admin)
        {
            if (admin == null)
            {
                return RedirectToAction("addAdmin");
            }
            Admin foundAdmin = eCommerceContext.Admins.SingleOrDefault(u => u.Email == admin.Email);
            if (foundAdmin == null)
            {
                eCommerceContext.Admins.Add(admin);
                eCommerceContext.SaveChanges();
                return RedirectToAction("index", "home");
            }
            foundAdmin.ErrorMsg = "this email is already exists";
            return View("addAdmin", foundAdmin);
        }
        public IActionResult profile()
        {
            int? id = HttpContext.Session.GetInt32("AdminId");
            if (id == null)
                return RedirectToAction("login", "user");
            Admin admin = eCommerceContext.Admins.SingleOrDefault(a => a.Id == id);
            return View(admin);
        }

        /////////////////////////////////////
        ///
        [HttpGet]
        public IActionResult edit()
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("login", "user");
            }
            Admin admin = eCommerceContext.Admins.Find(adminId);
            if (admin == null)
            {
                return RedirectToAction("index", "home");
            }
            return View(admin);
        }
        [HttpPost]
        public IActionResult edit(Admin admin)
        {
            Admin oldAdmin = eCommerceContext.Admins.Find(admin.Id);
            oldAdmin.Name = admin.Name;
            oldAdmin.Address = admin.Address;
            oldAdmin.Phone = admin.Phone;
            oldAdmin.Age = admin.Age;
            eCommerceContext.Admins.Update(oldAdmin);
            eCommerceContext.SaveChanges();
            return RedirectToAction("profile");
        }
        [HttpGet]
        public IActionResult changePicture()
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("login", "user");
            }
            Admin admin = eCommerceContext.Admins.Find(adminId);
            if (admin == null)
            {
                return RedirectToAction("index", "home");
            }
            return View(admin);
        }
        [HttpPost]
        public IActionResult changePicture(Admin admin, IFormFile ProfileImage)
        {

            string path = $"wwwroot/img/Profile/{ProfileImage.FileName}";
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                ProfileImage.CopyTo(fs);
                fs.Close();
            }
            Admin oldAdmin = eCommerceContext.Admins.Find(admin.Id);
            List<Admin> findImage = eCommerceContext.Admins.Where(p => p.ProfileImage.Equals(oldAdmin.ProfileImage)).ToList();
            if (findImage.Count == 1 && !oldAdmin.ProfileImage.Equals($"/img/Profile/profile.png"))
            {
                path = $"wwwroot{oldAdmin.ProfileImage}";
                fileInfo = new FileInfo(path);
                fileInfo.Delete();
            }
            oldAdmin.ProfileImage = $"/img/Profile/{ProfileImage.FileName}";
            eCommerceContext.Admins.Update(oldAdmin);
            eCommerceContext.SaveChanges();
            return RedirectToAction("profile");

        }
        [HttpGet]
        public IActionResult changePass()
        {
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("login", "user");
            }
            Admin admin = eCommerceContext.Admins.Find(adminId);
            if (admin == null)
            {
                return RedirectToAction("index", "home");
            }
            return View(admin);
        }
        [HttpPost]
        public IActionResult changePass(Admin admin)
        {
            Admin oldAdmin = eCommerceContext.Admins.Find(admin.Id);
            if (oldAdmin.Password == admin.OldPass)
            {
                oldAdmin.Password = admin.Password;
                eCommerceContext.Admins.Update(oldAdmin);
                eCommerceContext.SaveChanges();
            }
            return RedirectToAction("profile");
        }
    }
}
