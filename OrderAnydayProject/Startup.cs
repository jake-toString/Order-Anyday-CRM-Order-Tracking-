using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using OrderAnydayProject.Models;
using Owin;
using System;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(OrderAnydayProject.Startup))]
namespace OrderAnydayProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            using (OrderAnyDayContext db = new OrderAnyDayContext())
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    //Register OWIN delegates
                    ConfigureAuth(app);
                    //Setup Role and user for first run         
                    createRolesandUsers();
                    //SeedData();
                }
                else
                {
                    return;
                }

            }

        }

        // Create default User roles and Admin user for first run of app   
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            ApplicationRoleManager roleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context));
            ApplicationUserManager UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
          
                // Create first Admin Role and Admin User    
                if (!roleManager.RoleExists("Admin"))
                {

                    // Create Admin role   
                    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                    role.Name = "Admin";
                    roleManager.Create(role);

                    // Create Admin user                  

                    var user = new ApplicationUser();
                    user.UserName = "Admin";
                    user.Email = "AdminAdmin@admin.com";
                    user.Birthdate = System.DateTime.Now;
                    user.FirstName = "Admin";
                    user.LastName = "Admin";
                    user.Department = "Admin";

                    string userPWD = "Admin_2017";

                    var chkUser = UserManager.Create(user, userPWD);

                    //Add default User to Role Admin   
                    if (chkUser.Succeeded)
                    {
                        var result1 = UserManager.AddToRole(user.Id, "Admin");

                    }
                }
            
            // Create Employee role    
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);

                SeedData();
            }
            
        }

        public void SeedData()
        {

            ApplicationDbContext context = new ApplicationDbContext();
            ApplicationRoleManager roleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context));
            ApplicationUserManager UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            OrderAnyDayContext db = new OrderAnyDayContext();

            System.Collections.Generic.List<string> deptList = (from d in db.Departments select d.Name).ToList();
            
            uint pwd = 1000;
            Random rnd = new Random();
            
            uint phone = 2164441000;
            for (int i = 0; i < 500; i++)
            {
                // Create Employee  
                var user = new ApplicationUser();
                user.UserName = "Employee" + i.ToString();
                user.Email = user.UserName + "@orderanyday.com";
                user.Birthdate = System.DateTime.Now;
                user.FirstName = "Employee";
                user.LastName = "Name" + i.ToString();
                user.PhoneNumber = (phone + i).ToString();
                user.Department = deptList[rnd.Next(deptList.Count)];
                string userPWD = user.UserName + "_" + (pwd + i).ToString();

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Employee Role   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Employee");
                }
                
            }
            phone = 2165551000;
            for (int i = 0; i < 5; i++)
            {
                var user = new ApplicationUser();
                user.UserName = "Admin" + i.ToString(); ;
                user.Email = user.UserName + "@orderanyday.com";
                user.Birthdate = System.DateTime.Now;
                user.FirstName = "Admin";
                user.LastName = "Name" + i.ToString();
                user.Department = "Admin";
                user.PhoneNumber = (phone + i).ToString();
                string userPWD = user.UserName + "_" + (pwd + i).ToString();

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            System.Collections.Generic.List<string> catList = (from d in db.Categories select d.Name).ToList();
            
            //catList = new List<string>();
            //catList.Add("Cleaning");
            //catList.Add("Office");
            //catList.Add("Misc");
            //catList.Add("Paper");
            //catList.Add("Ink");
            phone = 2166661000;
            int address = 100;
            for (int i = 0; i < 10; i++)
            {
                Vendor v = new Vendor();
                v.Inactive = false;
                v.Address = (address + i).ToString() + " " + "This Street" ;
                v.Phone = (phone + i).ToString();
                v.Account_Manager = "contact" + i.ToString();
                v.Vendor_Name = "Vendor" + i.ToString();
                v.Website = "www." + v.Vendor_Name + ".com";

                for (int j = 0; j < 100; j++)
                {
                    Product p = new Product();
                    p.Category = catList[rnd.Next(catList.Count)];
                    p.Brand = "Brand" + j.ToString();
                    p.Discontinued = false;
                    p.ItemName = "Product" + j.ToString();
                    p.ItemNumber = 1000 + j;
                    p.Price = rnd.Next(100).ToString() + "." + rnd.Next(99).ToString();
                    p.Type = p.Category;
                    v.Products.Add(p);

                }
                db.Vendors.Add(v);
                db.SaveChanges();
            }
            
        }
    }
}
