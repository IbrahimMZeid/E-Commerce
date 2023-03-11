using E_commerceProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerceProject.Controllers
{
    public class UserController : Controller
    {
        ECommerceContext eCommerceContext;
        public UserController(ECommerceContext eCommerceContext)
        {
            this.eCommerceContext = eCommerceContext;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet]
        public IActionResult register()
        {
            int? id = HttpContext.Session.GetInt32("AdminId") ?? HttpContext.Session.GetInt32("UserId");
            if (id == null)
            {
                User user = new User();
                return View(user);
            }
            return RedirectToAction("index", "home");
        }
        [HttpPost]
        public IActionResult register(User user, IFormFile ProfileImage)
        {
            if (user == null)
            {
                return RedirectToAction("register");
            }
            Admin admin = eCommerceContext.Admins.SingleOrDefault(a => a.Email == user.Email);
            if (admin == null)
            {
                User foundUser = eCommerceContext.Users.SingleOrDefault(u => u.Email == user.Email);
                if (foundUser == null)
                {
                    if (ProfileImage == null)
                    {
                        user.ProfileImage = $"/img/Profile/profile.png";
                    }
                    else
                    {
                        string path = $"wwwroot/img/Profile/{ProfileImage.FileName}";
                        FileInfo fileInfo = new FileInfo(path);
                        if (!fileInfo.Exists)
                        {
                            FileStream fs = new FileStream(path, FileMode.Create);
                            ProfileImage.CopyTo(fs);
                            fs.Close();
                        }
                        user.ProfileImage = $"/img/Profile/{ProfileImage.FileName}";
                    }
                    eCommerceContext.Users.Add(user);
                    eCommerceContext.SaveChanges();
                    return RedirectToAction("login");
                }
            }
            user.ErrorMsg = "This Email is already exists";
            return View("register", user);
        }
        [HttpGet]
        public IActionResult login()
        {
            int? id = HttpContext.Session.GetInt32("AdminId") ?? HttpContext.Session.GetInt32("UserId");
            if (id == null)
            {
                User user = new User();
                return View(user);
            }
            return RedirectToAction("index", "home");
        }
        [HttpPost]
        public IActionResult login(User user)
        {
            if (user == null)
                return RedirectToAction("login");
            Admin admin = eCommerceContext.Admins.SingleOrDefault(a => a.Email == user.Email && a.Password == user.Password);
            if (admin == null)
            {
                User foundUser = eCommerceContext.Users.SingleOrDefault(u => u.Email == user.Email && u.Password == user.Password);
                if (foundUser == null)
                {
                    user.ErrorMsg = "Email or password wrong";
                    return View("login", user);
                }
                HttpContext.Session.SetInt32("UserId", foundUser.Id);
            }
            else
            {
                HttpContext.Session.SetInt32("AdminId", admin.Id);
            }
            return RedirectToAction("index", "home");
        }
        public IActionResult profile()
        {
            int? id = HttpContext.Session.GetInt32("UserId");
            if (id == null)
                return RedirectToAction("login");
            User user = eCommerceContext.Users.SingleOrDefault(u => u.Id == id);
            return View(user);
        }
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index", "home");
        }
        [HttpGet]
        public IActionResult edit()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("login", "user");
            }
            User user = eCommerceContext.Users.Find(userId);
            if (user == null)
            {
                return RedirectToAction("index", "home");
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult edit(User user)
        {
            User oldUser = eCommerceContext.Users.Find(user.Id);
            oldUser.Name = user.Name;
            oldUser.Address = user.Address;
            oldUser.Phone = user.Phone;
            oldUser.Age = user.Age;
            eCommerceContext.Users.Update(oldUser);
            eCommerceContext.SaveChanges();
            return RedirectToAction("profile");
        }
        [HttpGet]
        public IActionResult changePicture()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("login", "user");
            }
            User user = eCommerceContext.Users.Find(userId);
            if (user == null)
            {
                return RedirectToAction("index", "home");
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult changePicture(User user, IFormFile ProfileImage)
        {

            string path = $"wwwroot/img/Profile/{ProfileImage.FileName}";
            FileInfo fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                ProfileImage.CopyTo(fs);
                fs.Close();
            }
            User oldUser = eCommerceContext.Users.Find(user.Id);
            List<User> findImage = eCommerceContext.Users.Where(p => p.ProfileImage.Equals(oldUser.ProfileImage)).ToList();
            if (findImage.Count == 1 && !oldUser.ProfileImage.Equals($"/img/Profile/profile.png"))
            {
                path = $"wwwroot{oldUser.ProfileImage}";
                fileInfo = new FileInfo(path);
                fileInfo.Delete();
            }
            oldUser.ProfileImage = $"/img/Profile/{ProfileImage.FileName}";
            eCommerceContext.Users.Update(oldUser);
            eCommerceContext.SaveChanges();
            return RedirectToAction("profile");

        }
        [HttpGet]
        public IActionResult changePass()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("login", "user");
            }
            User user = eCommerceContext.Users.Find(userId);
            if (user == null)
            {
                return RedirectToAction("index", "home");
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult changePass(User user)
        {
            User oldUser = eCommerceContext.Users.Find(user.Id);
            if(oldUser.Password == user.OldPass)
            {
                oldUser.Password = user.Password;
                eCommerceContext.Users.Update(oldUser);
                eCommerceContext.SaveChanges();
            }
            return RedirectToAction("profile");
        }
        [HttpGet]
        public IActionResult delete(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            int? adminId = HttpContext.Session.GetInt32("AdminId");
            if (userId == null && adminId == null)
            {
                return RedirectToAction("login", "user");
            }
            else if (userId != null && id == 0)
            {
                User user = eCommerceContext.Users.Find(userId);
                eCommerceContext.Users.Remove(user);
                eCommerceContext.SaveChanges();
            }else if(adminId != null && id != 0)
            {
                User user = eCommerceContext.Users.Find(id);
                eCommerceContext.Users.Remove(user);
                eCommerceContext.SaveChanges();
                return RedirectToAction("userList");
            }
            else
            {
                return RedirectToAction("login", "user");
            }
            return RedirectToAction("logout");
        }
        [HttpGet]
        public IActionResult userList()
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
            List<User> users = eCommerceContext.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public IActionResult userList(string name)
        {
            if (name == null)
            {
                return RedirectToAction("userList");
            }
            List<User> users = eCommerceContext.Users.Where(u => u.Email.ToLower().Contains(name.ToLower()) || u.Name.ToLower().Contains(name.ToLower())).ToList();
            return View("userList",users);
        }
    }
}
