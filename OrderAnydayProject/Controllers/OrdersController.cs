using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OrderAnydayProject.Models;
using Microsoft.AspNet.Identity;

namespace OrderAnydayProject.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private OrderAnyDayContext db = new OrderAnyDayContext();

        public ActionResult GetUserOrders()
        {
            return View();
        }

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.ToList();

            //var orders = db.Orders.Include(o => o.AspNetUser);

            var user = User.Identity.GetUserId();
            if(User.Identity.GetUserName() != "Admin") // check to see if Admin is logged in
                orders = db.Orders.Where(s => s.UserId == user).ToList(); // if not show only orders placed by active user
            
            // extended to include check for active orders
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            Order model = new Order();
            model.UserId = User.Identity.GetUserId();
            model.Last_Active = DateTime.Now;
            model.Placed = DateTime.Now;
            model.IsActive = "p";

            return View(model);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderNumber,UserId,Placed,Last_Active,IsActive")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FirstName", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FirstName", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderNumber,UserId,Placed,Last_Active,IsActive")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FirstName", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            int notecount = (from s in db.UserNotifications where s.UserId == order.UserId & s.OrderNumber == order.OrderNumber select s).Count();
            if (notecount > 0)
            {
                UserNotification n = (from un in db.UserNotifications where un.OrderNumber == order.OrderNumber & un.UserId == order.UserId select un).FirstOrDefault();
                db.UserNotifications.Remove(n);
                db.SaveChanges();
            }
            db.Orders.Remove(order);
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
