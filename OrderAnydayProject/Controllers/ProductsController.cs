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
    public class ProductsController : Controller
    {
        private OrderAnyDayContext db = new OrderAnyDayContext();
        public List<string> typeList { get; set; }
        //public List<string> catList { get; set; }

        public ProductsController()
        {
            //catList = new List<string>();
            //catList.Add("Cleaning");
            //catList.Add("Office");
            //catList.Add("Misc");
            //catList.Add("Paper");
            //catList.Add("Ink");
            //catList.Sort();
        }

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Vendor).Where(p => p.Discontinued == false).Take(100).OrderByDescending(p => p.ItemNumber).ToList();
            return View(products.ToList());
        }

        // GET: Products
        public ActionResult Discontinued()
        {
            var products = db.Products.Where(p => p.Discontinued == true);
            return View(products.ToList());
        }

        [HttpPost]
        public ActionResult Index(string searchString)
        {
            OrderAnyDayContext db = new OrderAnyDayContext();
            List<Product> products;

            if (string.IsNullOrEmpty(searchString))
            {
                products = db.Products.ToList();
            }
            else
            {
                products = db.Products.Where(s => s.ItemName == searchString).ToList();
            }

            return View(products);
        }

        public JsonResult Get_Products(string term)
        {
            //var vendors = from m in db.Vendors
            //            select m;
            OrderAnyDayContext db = new OrderAnyDayContext();
            List<string> products;

            products = db.Products.Where(s => s.ItemName.StartsWith(term))
                .Select(t => t.ItemName).ToList();


            return Json(products, JsonRequestBehavior.AllowGet);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Cat = new SelectList(db.Categories.OrderBy(x => x.Name).ToList(), "Name", "Name"); //SelectList(catList);
            ViewBag.VendorID = new SelectList(db.Vendors, "VendorID", "Vendor_Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemNumber,VendorID,ItemName,Brand,Price,Type,Category,Discontinued,Date_Added")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VendorID = new SelectList(db.Vendors, "VendorID", "Vendor_Name", product.VendorID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.Cat = new SelectList(db.Categories.OrderBy(x => x.Name).ToList(), "Name", "Name"); //SelectList(catList);
            ViewBag.VendorID = new SelectList(db.Vendors, "VendorID", "Vendor_Name", product.VendorID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemNumber,VendorID,ItemName,Brand,Price,Type,Category,Date_Added,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cat = new SelectList(db.Categories.OrderBy(x => x.Name).ToList(), "Name", "Name"); //SelectList(catList);
            ViewBag.VendorID = new SelectList(db.Vendors, "VendorID", "Vendor_Name", product.VendorID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
