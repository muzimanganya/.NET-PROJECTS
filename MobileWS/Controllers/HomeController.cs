using MobileWS.Helpers;
using MobileWS.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MobileWS.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();

        // GET: Home
        [AuthFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(user.Username) || String.IsNullOrEmpty(user.Password))
                {
                    if (String.IsNullOrEmpty(user.Username))
                    {
                        ModelState.AddModelError("Username", "Empty Username!");
                    }
                    else if (String.IsNullOrEmpty(user.Password))
                        ModelState.AddModelError("Password", "Empty Password!");
                }
                else
                {
                    String password = user.Password;
                    password = SecurityHelper.SHA256(password);

                    User usr = db.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == password);
                    if (usr == null)
                    {
                        ModelState.AddModelError("Password", "Invalid Username or Password!");
                    }
                    else
                    {
                        //set the values 
                        HttpContext.Session["UserName"] = usr.Username;
                        HttpContext.Session["Name"] = usr.Name;
                        HttpContext.Session["ID"] = usr.ID;
                        HttpContext.Session["Role"] = usr.Role; //Role

                        return RedirectToAction("Index");
                    }
                }
            }

            return View(user);
        }


        public ActionResult Forbidden()
        {
            return View();
        }
    }
}