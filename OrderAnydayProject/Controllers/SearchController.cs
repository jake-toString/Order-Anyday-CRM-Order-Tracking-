using OrderAnydayProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace OrderAnydayProject.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
           
            return Redirect("/Home/Profile");
        }

        [HttpPost]//searches database for text entered search
        public ActionResult Index(string searchString)
        {
            OrderAnyDayContext db = new OrderAnyDayContext();
            List<Vendor> vendors;
            List<Product> products;
            List<Order> orders;
            //bool isnum = Regex.IsMatch(searchString, @"^\d+$");

            if (Regex.IsMatch(searchString, @"[0-9 ]+$"))
            {
                searchString.Trim();
                int number = 0;
                Int32.TryParse(searchString, out number);
                orders = db.Orders.Where(o => o.OrderNumber == number).ToList();

                if (orders.Count > 0)
                {
                    return View("~/Views/Orders/index.cshtml", orders);
                }
                else
                {
                    return Redirect("/Home/Profile");
                }
            }
            if (searchString.Contains(" - Vendor"))
            {
                //Do Vendor name Search
                string[] test = searchString.Split('-');
                string test2 = test[0];
                test2 = test2.Trim();
                vendors = db.Vendors.Where(s => s.Vendor_Name == test2).ToList();
                return View("~/Views/Vendors/Index.cshtml", vendors);
                //if...
            }
            else if (searchString.Contains(" - Account Manager"))
            {
                //Do Accountmanager Search
                string[] test = searchString.Split('-');
                string test2 = test[0];
                test2 = test2.Trim();
                vendors = db.Vendors.Where(s => s.Account_Manager == test2).ToList();
                return View("~/Views/Vendors/Index.cshtml", vendors);
                //if...
            }
            else if (searchString.Contains(" - Order Number"))
            {
                //Do Order Number Search
                string[] test = searchString.Split('-');
                string test2 = test[0];
                test2 = test2.Trim();

                int number = 0;
                Int32.TryParse(test2, out number);
                orders = db.Orders.Where(o => o.OrderNumber == number).ToList();
                return View("~/Views/Orders/index.cshtml", orders);
            }
            else if (searchString.Contains(" - Product Brand"))
            {
                //Do Accountmanager Search
                string[] test = searchString.Split('-');
                string test2 = test[0];
                test2 = test2.Trim();
                products = db.Products.Where(s => s.Brand == test2).ToList();
                return View("~/Views/Products/Index.cshtml", products);
                //if...
            }
            else if (searchString.Contains(" - Product Name"))
            {

                //Do Accountmanager
                string[] test = searchString.Split('-');
                string test2 = test[0];
                test2 = test2.Trim();
                products = db.Products.Where(s => s.ItemName == test2).ToList();
                return View("~/Views/Products/Index.cshtml", products);
                //if...
            }
            else if (searchString.Contains(" - Product Category"))
            {

                //Do Accountmanager
                string[] test = searchString.Split('-');
                string test2 = test[0];
                test2 = test2.Trim();
                products = db.Products.Where(s => s.Category == test2).ToList();
                return View("~/Views/Products/Index.cshtml", products);
                //if...
            }
            else if (string.IsNullOrEmpty(searchString))
            {
                return Redirect("/Home/Profile");
            }
            else
            {
                vendors = db.Vendors.Where(s => s.Vendor_Name == searchString).ToList();
                products = db.Products.Where(t => t.ItemName == searchString).ToList();


                if (vendors.Count > 0)
                {
                    return View("~/Views/Vendors/Index.cshtml", vendors);
                }
                if (products.Count > 0)
                {
                    return View("~/Views/Products/Index.cshtml", products);
                }
               
            }

            return Redirect("/Home/Profile");
        }

        public JsonResult Search(string term)
        {//searches database while typing and gives a drop down of all items in db based on leters typed

            OrderAnyDayContext db = new OrderAnyDayContext();
            List<string> vendors;
            List<string> accountmanagers;
            List<string> brand;
            List<string> productname;
            List<string> category;
            List<string> order;


            vendors = db.Vendors.Where(s => s.Vendor_Name.StartsWith(term))
                .Select(t => t.Vendor_Name).Distinct().ToList();

            for (int x = 0; x < vendors.Count; x++)
            {
                vendors[x] = vendors[x] + " - Vendor";
            }

            accountmanagers = db.Vendors.Where(b => b.Account_Manager.StartsWith(term))
                .Select(u => u.Account_Manager).Distinct().ToList();


            for (int x = 0; x < accountmanagers.Count; x++)
            {
                accountmanagers[x] = accountmanagers[x] + " - Account Manager";
            }
            vendors.AddRange(accountmanagers);

            order = db.Orders.Where(o => o.OrderNumber.ToString().StartsWith(term))
                    .Select(p => p.OrderNumber.ToString()).Distinct().ToList();
            for (int x = 0; x < order.Count; x++)
            {
                order[x] = order[x] + " - Order Number";
            }
            vendors.AddRange(order);

            category = db.Products.Where(c => c.Category.StartsWith(term))
                .Select(a => a.Category).Distinct().ToList();
            for (int x = 0; x < category.Count; x++)
            {
                category[x] = category[x] + " - Product Category";
            }
            vendors.AddRange(category);

            brand = db.Products.Where(br => br.Brand.StartsWith(term))
                .Select(b => b.Brand).Distinct().ToList();
            for (int x = 0; x < brand.Count; x++)
            {
                brand[x] = brand[x] + " - Product Brand";
            }
            vendors.AddRange(brand);

            productname = db.Products.Where(s => s.ItemName.StartsWith(term))
               .Select(t => t.ItemName).Distinct().ToList();

            for (int x = 0; x < productname.Count; x++)
            {
                productname[x] = productname[x] + " - Product Name";
            }
            vendors.AddRange(productname);

            return Json(vendors, JsonRequestBehavior.AllowGet);
        }
    }
}