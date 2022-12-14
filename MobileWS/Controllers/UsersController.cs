using MobileWS.Helpers;
using MobileWS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Saloon.Controllers
{
    [AuthFilter]
    public class UsersController : Controller
    {
        private Entities db = new Entities();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {

            //set Viwebag item
            //set the Roles List
            List<SelectListItem> roleList = new List<SelectListItem>();
            roleList.Add(new SelectListItem { Text = "Admin", Value = "admin" });
            roleList.Add(new SelectListItem { Text = "Employee", Value = "worker" }); 
            ViewBag.RolesList = roleList;
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Username,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                if(String.IsNullOrEmpty(user.Username))
                {
                    ModelState.AddModelError("Username", "Username cannot be empty!");
                }
                else if (String.IsNullOrEmpty(user.Password))
                {
                    ModelState.AddModelError("Username", "Password cannot be empty!");
                }
                else if (String.IsNullOrEmpty(user.Role))
                {
                    ModelState.AddModelError("Username", "Role cannot be empty!");
                }
                else
                {
                    String password = user.Password;
                    user.Password = SecurityHelper.SHA256(password);

                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            //set Viwebag item
            //set the Roles List
            List<SelectListItem> roleList = new List<SelectListItem>(); 
            roleList.Add(new SelectListItem { Text = "Admin", Value = "admin", Selected = user.Role.StartsWith("admin") });
            roleList.Add(new SelectListItem { Text = "Employee", Value = "worker", Selected = user.Role.StartsWith("worker") });
          
            List<SelectListItem> companyList = new List<SelectListItem>();
            foreach (Company c in db.Companies)
            {
                companyList.Add(new SelectListItem { Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(c.Name), Value = c.Name , Selected=user.UserCompany==c.Name});
            }             

            ViewBag.RolesList = roleList;
            ViewBag.companyList = companyList;

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Username,Password,Role, Mobile,UserCompany")] User user)
        {
            User dbUser = db.Users.FirstOrDefault(u => u.ID == user.ID);

            if (ModelState.IsValid)
            {
                if (dbUser == null)
                {
                    ModelState.AddModelError("Username", "Invalid User");
                }
                else
                {                    
                    if (!String.IsNullOrEmpty(user.Username))
                    {
                        dbUser.Username = user.Username;
                    }
                    
                    if (!String.IsNullOrEmpty(user.Password))
                    {
                        String password = user.Password;
                        if (password.Length != 64)
                            dbUser.Password = SecurityHelper.SHA256(password);
                    }

                    if (!String.IsNullOrEmpty(user.Role))
                    {
                        dbUser.Role = user.Role;
                    }

                    if (!String.IsNullOrEmpty(user.UserCompany))
                    {
                        dbUser.UserCompany = user.UserCompany;
                    }
                    dbUser.Mobile = user.Mobile; 
                    db.Entry(dbUser).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}