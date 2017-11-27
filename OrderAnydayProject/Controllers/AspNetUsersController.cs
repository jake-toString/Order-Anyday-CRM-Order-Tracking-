using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using OrderAnydayProject.Models;
using OrderAnydayProject.CustomFilters;
using Microsoft.AspNet.Identity.EntityFramework;
using OrderAnydayProject.ViewModels;

namespace OrderAnydayProject.Controllers
{
    [Authorize]
    public class AspNetUsersController : Controller
    {
        private OrderAnyDayContext db;
        private ApplicationDbContext context;
        //public List<string> deptList { get; set; }
        //public List<IdentityRole> roleList { get; set; }

        public AspNetUsersController()
        {
            db = new OrderAnyDayContext();
            context = new ApplicationDbContext();
            //roleList = (from r in context.Roles select r).ToList();
        }

        public Boolean isAdmin()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var role = UserManager.GetRoles(user.GetUserId());
                if (role[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public ActionResult GetUserOrders()
        {
            UserProfileViewModel UserProfileVM = new UserProfileViewModel();
            string uid = User.Identity.GetUserId();
            UserProfileVM.user = (from s in db.AspNetUsers where s.Id == uid select s).FirstOrDefault();
            UserProfileVM.orders = (from o in db.Orders where o.UserId == uid select o).ToList();
            return View(UserProfileVM);
        }

        // GET: AspNetUsers        
        [AuthorizeLog(Roles = "Admin")]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {


                if (isAdmin())
                {
                    var aspnetuser = db.AspNetUsers.ToList();
                    return View(aspnetuser);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Index", "Manage");            
        }

        [HttpPost]
        public ActionResult Index(string searchString)
        {
            List<AspNetUser> aspnetuser;
            List<AspNetUser> department;

            if (string.IsNullOrEmpty(searchString))
            {
                aspnetuser = db.AspNetUsers.ToList();
            }
            else if (searchString.Contains(" - Department"))
            {
                //Do Vendor name Search
                string[] test = searchString.Split('-');
                string test2 = test[0];
                test2 = test2.Trim();
                aspnetuser = db.AspNetUsers.Where(s => s.Department == test2).ToList();
                //if...
            }
            else
            {
                string[] name = searchString.Split(' ');
                if (name.Count() > 1)
                {
                    string firstname = name[0];
                    string lastname = name[1];
                    aspnetuser = db.AspNetUsers.Where(s => s.FirstName == firstname && 
                        s.LastName == lastname).ToList();
                    return View(aspnetuser);
                }

                
                department = db.AspNetUsers.Where(t => t.Department == searchString).ToList();

                if (department.Count() > 0)
                {
                    return View(department);
                }

                else
                {
                    List<AspNetUser> allLike;
                    allLike = db.AspNetUsers.Where(s => s.FirstName.StartsWith(searchString) ||
                    s.LastName.StartsWith(searchString) || s.Department.StartsWith(searchString))
                    .Distinct().ToList();
                    return View(allLike);
                }
               
            }

            return View(aspnetuser);
        }

        public JsonResult Get_Users(string term)
        {
            List<string> aspnetuser;
            List<string> lastname;
            List<string> department;

            aspnetuser = db.AspNetUsers.Where(s => s.FirstName.StartsWith(term))
                .Select(t => t.FirstName).ToList();
            lastname = db.AspNetUsers.Where(q => q.FirstName.StartsWith(term))
                .Select(u => u.LastName).ToList();
            department = db.AspNetUsers.Where(r => r.Department.StartsWith(term))
                .Select(v => v.Department).Distinct().ToList();

            for (int x = 0; x < aspnetuser.Count; x++)
            {
                aspnetuser[x] = aspnetuser[x] + " " + lastname[x];
            }

            for (int x = 0; x < department.Count; x++)
            {
                department[x] = department[x] + " - Department";
            }

            aspnetuser.AddRange(department);


            return Json(aspnetuser, JsonRequestBehavior.AllowGet);
        }

        // GET: AspNetUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Create
        [AuthorizeLog(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,FirstName,LastName,Birthdate,Department,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetUser);
        }

        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }

            //ViewBag.Name = new SelectList(context.Roles.ToList(), "Name", "Name");
            ViewBag.Role = new SelectList(context.Roles.ToList(), "Name", "Name"); //SelectList(roleList);
            ViewBag.Dept = new SelectList(db.Departments.ToList(), "Name", "Name"); //SelectList(deptList);
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,FirstName,LastName,Birthdate,Department,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.Name = new SelectList(context.Roles.ToList(), "Name", "Name");
            ViewBag.Role = new SelectList(context.Roles.ToList(), "Name", "Name"); //SelectList(roleList);
            ViewBag.Dept = new SelectList(db.Departments.ToList(), "Name", "Name"); //SelectList(deptList);
            return View(aspNetUser);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditUser(UserRoleViewModel model)//[Bind(Include = "Id,UserName,FirstName,LastName,Birthdate,Department,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount")] AspNetUser aspNetUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Department = model.Department, Birthdate = model.Birthdate, FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.PhoneNumber };
        //        AspNetUser aspUser = (from s in db.AspNetUsers where s.Id == model.Id select s).FirstOrDefault();
        //        aspUser.Birthdate = model.Birthdate;
        //        aspUser.Department = model.Department;
        //        aspUser.Email = model.Email;
        //        aspUser.FirstName = model.FirstName;
        //        aspUser.LastName = model.LastName;
        //        aspUser.PhoneNumber = model.PhoneNumber;
        //        //aspUser.UserName = model.UserName;
        //        //aspUser.U
        //        db.Entry(aspUser).State = EntityState.Modified;
        //        db.SaveChanges();
        //        //this.UserManager.AddToRoleAsync(user.Id, model.UserRoles);
        //        return RedirectToAction("Index");
        //    }

        //    //ViewBag.Name = new SelectList(context.Roles.ToList(), "Name", "Name");
        //    ViewBag.Role = new SelectList(roleList);
        //    ViewBag.Dept = new SelectList(deptList);
        //    return View(model);
        //}

        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUser);
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
