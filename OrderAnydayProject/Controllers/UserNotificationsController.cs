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
    public class UserNotificationsController : Controller
    {
        private OrderAnyDayContext db = new OrderAnyDayContext();

        // GET: UserNotifications
        public ActionResult Index()
        {
            var userNotifications = db.UserNotifications.Include(u => u.AspNetUser).Include(u => u.Notification).Include(u => u.Order);
            return View(userNotifications.ToList());
        }

        // GET: UserNotifications/Details/5
        public ActionResult Details(int? order, string uid)
        {
            if (order == null || uid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserNotification userNotification = db.UserNotifications.Find(order, uid);
            if (userNotification == null)
            {
                return HttpNotFound();
            }
            return View(userNotification);
        }

        // GET: UserNotifications/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FirstName");
            ViewBag.NotificationID = new SelectList(db.Notifications, "NotificationID", "Description");
            ViewBag.OrderNumber = new SelectList(db.Orders, "OrderNumber", "UserId");
            return View();
        }

        // POST: UserNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderNumber,UserId,NotificationID,Notes")] UserNotification userNotification)
        {
            if (ModelState.IsValid)
            {
                db.UserNotifications.Add(userNotification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FirstName", userNotification.UserId);
            ViewBag.NotificationID = new SelectList(db.Notifications, "NotificationID", "Description", userNotification.NotificationID);
            ViewBag.OrderNumber = new SelectList(db.Orders, "OrderNumber", "UserId", userNotification.OrderNumber);
            return View(userNotification);
        }

        // GET: UserNotifications/Edit/5
        public ActionResult Edit(int? order, string uid)
        {
            if (order == null || uid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserNotification userNotification = db.UserNotifications.Find(order, uid);
            if (userNotification == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FirstName", userNotification.UserId);
            ViewBag.NotificationID = new SelectList(db.Notifications, "NotificationID", "Description", userNotification.NotificationID);
            ViewBag.OrderNumber = new SelectList(db.Orders, "OrderNumber", "UserId", userNotification.OrderNumber);
            return View(userNotification);
        }

        // POST: UserNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderNumber,UserId,NotificationID,Notes")] UserNotification userNotification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userNotification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "FirstName", userNotification.UserId);
            ViewBag.NotificationID = new SelectList(db.Notifications, "NotificationID", "Description", userNotification.NotificationID);
            ViewBag.OrderNumber = new SelectList(db.Orders, "OrderNumber", "UserId", userNotification.OrderNumber);
            return View(userNotification);
        }

        // GET: UserNotifications/Delete/5
        public ActionResult Delete(int? order, string uid)
        {
            if (order == null || uid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserNotification userNotification = db.UserNotifications.Find(order, uid);
            if (userNotification == null)
            {
                return HttpNotFound();
            }
            return View(userNotification);
        }

        // POST: UserNotifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? order, string uid)
        {
            UserNotification userNotification = db.UserNotifications.Find(order, uid);
            db.UserNotifications.Remove(userNotification);
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
