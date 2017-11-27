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
    public class OrderItemsController : Controller
    {
        private OrderAnyDayContext db = new OrderAnyDayContext();

        // GET: OrderItems
        public ActionResult Index()
        {
            var orderItems = db.OrderItems.Include(o => o.Order).Include(o => o.Product).Include(o => o.Vendor);
            return View(orderItems.ToList());
        }

        // GET: OrderItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // GET: OrderItems/Create
        public ActionResult Create()
        {
            ViewBag.OrderNumber = new SelectList(db.Orders, "OrderNumber", "UserId");
            ViewBag.ItemNumber = new SelectList(db.Products, "ItemNumber", "ItemName");
            ViewBag.VendorID = new SelectList(db.Vendors, "VendorID", "Vendor_Name");
            return View();
        }

        // POST: OrderItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderNumber,ItemNumber,VendorID,Quantity")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                db.OrderItems.Add(orderItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderNumber = new SelectList(db.Orders, "OrderNumber", "UserId", orderItem.OrderNumber);
            ViewBag.ItemNumber = new SelectList(db.Products, "ItemNumber", "ItemName", orderItem.ItemNumber);
            ViewBag.VendorID = new SelectList(db.Vendors, "VendorID", "Vendor_Name", orderItem.VendorID);
            return View(orderItem);
        }

        // GET: OrderItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderNumber = new SelectList(db.Orders, "OrderNumber", "UserId", orderItem.OrderNumber);
            ViewBag.ItemNumber = new SelectList(db.Products, "ItemNumber", "ItemName", orderItem.ItemNumber);
            ViewBag.VendorID = new SelectList(db.Vendors, "VendorID", "Vendor_Name", orderItem.VendorID);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderNumber,ItemNumber,VendorID,Quantity")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderNumber = new SelectList(db.Orders, "OrderNumber", "UserId", orderItem.OrderNumber);
            ViewBag.ItemNumber = new SelectList(db.Products, "ItemNumber", "ItemName", orderItem.ItemNumber);
            ViewBag.VendorID = new SelectList(db.Vendors, "VendorID", "Vendor_Name", orderItem.VendorID);
            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderItem orderItem = db.OrderItems.Find(id);
            db.OrderItems.Remove(orderItem);
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
