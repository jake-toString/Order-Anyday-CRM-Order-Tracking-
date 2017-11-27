using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using OrderAnydayProject.Models;
using OrderAnydayProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OrderAnydayProject.Controllers
{
    public class UserRoleController : Controller
    {
        private OrderAnyDayContext db;
        private ApplicationDbContext context;
        //public List<Department> deptList { get; set; }
        //public List<IdentityRole> roleList { get; set; }
        
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public UserRoleController()
        {
            db = new OrderAnyDayContext();
            context = new ApplicationDbContext();
            //deptList = (from d in db.Departments select d).ToList();
            //roleList = (from r in context.Roles select r).ToList();
        }

        public UserRoleController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            db = new OrderAnyDayContext();
            context = new ApplicationDbContext();
            //deptList = (from d in db.Departments select d).ToList();
            //roleList = (from r in context.Roles select r).ToList();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return this._roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set { this._roleManager = value; }
        }

        public ActionResult UpdateUser(int? id)
        {
            UserRoleViewModel user = new UserRoleViewModel();
            return View(user);
        }
        // GET: EditUser
        public ActionResult Index()
        {
            return View();
        }

        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRoleViewModel urVM = new UserRoleViewModel();
            urVM.aspNetUser = db.AspNetUsers.Find(id);
            if (urVM.aspNetUser == null)
            {
                return HttpNotFound();
            }
            urVM.Department = urVM.aspNetUser.Department;
            var user = context.Users.Find(id);
            var role = user.Roles.FirstOrDefault();
            urVM.UserRole = db.AspNetRoles.Find(role.RoleId).Name; //role.RoleId;
            //urVM.UserRole.
            //this.UserManager.Users.First()
            //urVM.UserRole = db.AspNetRoles.Find(id);
            ViewBag.Name = new SelectList(context.Roles.ToList(), "Name", "Name");
            //ViewBag.Role = new SelectList(roleList);
            ViewBag.Dept = new SelectList(db.Departments.ToList(), "Name", "Name"); //SelectList(deptList);
            return View(urVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserRoleViewModel model)//[Bind(Include = "Id,UserName,FirstName,LastName,Birthdate,Department,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount")] UserRoleViewModel model)//
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.aspNetUser.UserName, Email = model.aspNetUser.Email, Department = model.Department, Birthdate = model.aspNetUser.Birthdate, FirstName = model.aspNetUser.FirstName, LastName = model.aspNetUser.LastName, PhoneNumber = model.aspNetUser.PhoneNumber };
                AspNetUser aspUser = (from s in db.AspNetUsers where s.Id == model.aspNetUser.Id select s).FirstOrDefault();
                aspUser.Birthdate = model.aspNetUser.Birthdate;
                aspUser.Department = model.Department;
                aspUser.Email = model.aspNetUser.Email;
                aspUser.FirstName = model.aspNetUser.FirstName;
                aspUser.LastName = model.aspNetUser.LastName;
                aspUser.PhoneNumber = model.aspNetUser.PhoneNumber;
                //aspUser.UserName = model.UserName;
                user.Roles.Clear();
                //user.Roles.Add(model.UserRoles);
                
                db.SaveChanges();
                this.UserManager.AddToRoleAsync(user.Id, model.UserRoles);
                context.SaveChanges();
                //db.SaveChanges();
                return RedirectToAction("Index", "AspNetUsers");
            }

            ViewBag.Name = new SelectList(context.Roles.ToList(), "Name", "Name");
            //ViewBag.Role = new SelectList(roleList);
            ViewBag.Dept = new SelectList(db.Departments.ToList(), "Name", "Name"); //SelectList(deptList);
            return View(model);
        }

    }
}