using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OrderAnydayProject.Models;

namespace OrderAnydayProject.Controllers
{
    [Authorize]
    public class VendorsController : Controller
    {
        private OrderAnyDayContext db = new OrderAnyDayContext();

        // GET: Vendors
        public ActionResult Index()
        {
            OrderAnyDayContext db = new OrderAnyDayContext();
            var vendors = db.Vendors.Where(v => v.Inactive == false).ToList();
            return View(vendors);
        }

        // GET: Vendors
        public ActionResult Inactive()
        {
            OrderAnyDayContext db = new OrderAnyDayContext();
            var vendors = db.Vendors.Where(v => v.Inactive == true).ToList();
            return View(vendors);
        }

        [HttpPost]//searches database for text entered search
        public ActionResult Index(string searchString)
        {
            OrderAnyDayContext db = new OrderAnyDayContext();
            List<Vendor> vendors;
            List<Product> products;

            if (searchString.Contains(" - Vendor"))
            {
                //Do Vendor name Search
                string[] test = searchString.Split('-');
                string test2 = test[0];
                test2 = test2.Trim();
                vendors = db.Vendors.Where(s => s.Vendor_Name == test2).ToList();
                //if...
            }
            else if (searchString.Contains(" - Account Manager"))
            {
                //Do Accountmanager Search
                string[] test = searchString.Split('-');
                string test2 = test[0];
                test2 = test2.Trim();
                vendors = db.Vendors.Where(s => s.Account_Manager == test2).ToList();
                //if...
            }
            else if (searchString.Contains(" - Product Name"))
            {
           
                //Do Accountmanager Shit
                string[] test = searchString.Split('-');
                string test2 = test[0];
                test2 = test2.Trim();
                products = db.Products.Where(s => s.ItemName == test2).ToList();
                return View("~/Views/Products/Index.cshtml", products);
                //if...
            }
            else if (string.IsNullOrEmpty(searchString))
            {
                vendors = db.Vendors.ToList();
            }
            else
            {
                vendors = db.Vendors.Where(s => s.Vendor_Name == searchString).ToList();
                products = db.Products.Where(t => t.ItemName == searchString).ToList();

                if (vendors.Count > 0)
                    return View(vendors);
                if (products.Count > 0) 
                    return View("~/Views/Products/Index.cshtml", products);
                else 
                    vendors = db.Vendors.ToList();
            }

            return View(vendors);
        }

        public JsonResult GetVendors(string term)
        {//searches database while typing and gives a drop down of all items in db based on leters typed
           
            OrderAnyDayContext db = new OrderAnyDayContext();
            List<string> vendors;
            List<string> accountmanagers;
            List<string> productname;
            vendors = db.Vendors.Where(s => s.Vendor_Name.StartsWith(term))
                .Select(t => t.Vendor_Name).Distinct().ToList();

            for (int x = 0; x < vendors.Count; x++)
            {
                vendors[x] = vendors[x] + " - Vendor";
            }

            accountmanagers = db.Vendors.Where(b => b.Account_Manager.StartsWith(term))
                .Select(u=> u.Account_Manager).Distinct().ToList();
           

            for (int x = 0; x < accountmanagers.Count; x++)
            {
                accountmanagers[x] = accountmanagers[x] + " - Account Manager";
            }
            vendors.AddRange(accountmanagers);

            productname = db.Products.Where(s => s.ItemName.StartsWith(term))
               .Select(t => t.ItemName).Distinct().ToList();

            for (int x = 0; x < productname.Count; x++)
            {
                productname[x] = productname[x] + " - Product Name";
            }
            vendors.AddRange(productname);

            return Json(vendors, JsonRequestBehavior.AllowGet);
        }

        // GET: Vendors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // GET: Vendors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VendorID,Vendor_Name,Address,Phone,Website,Account_Manager,Notes,Inactive")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                db.Vendors.Add(vendor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendor);
        }

        // GET: Vendors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VendorID,Vendor_Name,Address,Phone,Website,Account_Manager,Notes,Inactive")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendor);
        }

        // GET: Vendors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendor vendor = db.Vendors.Find(id);
            db.Vendors.Remove(vendor);
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
