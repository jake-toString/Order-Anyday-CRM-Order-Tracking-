using Microsoft.AspNet.Identity;
using OrderAnydayProject.Models;
using OrderAnydayProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderAnydayProject.Controllers
{
    [Authorize]
    public class OrderDetailsController : Controller
    {
        private OrderAnyDayContext db = new OrderAnyDayContext();
        // GET: OrderDetails

        [HttpGet]
        public ActionResult OrderInvoice(int? id)
        {
            if (id != null)
            {
                OrderDetailsViewModel orderDvM = new OrderDetailsViewModel();
                orderDvM.orders = GetOrder(id);
                orderDvM.user = GetCurrentUser(orderDvM.orders.UserId);
                GetOrderItems(orderDvM);
                //GetVendors(orderDvM);
                //GetCurrentProducts(orderDvM);
                orderDvM.FullName = orderDvM.user.FirstName + orderDvM.user.LastName;
                return View(orderDvM);
            }
            else
            {
                return RedirectToAction("Profile", "Home");
            }
        }

        public Order GetOrder(int? id)
        {
            
            var order = db.Orders.Where(o => o.OrderNumber == id).FirstOrDefault();
        
            return order;
        }

        public AspNetUser GetCurrentUser(string userId)
        {
            //string uid = User.Identity.GetUserId();
            var user = (from s in db.AspNetUsers where s.Id == userId select s).FirstOrDefault();
            return (AspNetUser)user;
        }

        public void GetOrderItems(OrderDetailsViewModel orderDvM)
        {
            orderDvM.orderitem = (from s in db.OrderItems where s.OrderNumber == orderDvM.orders.OrderNumber select s).ToList();
            decimal total = 0;
            foreach (OrderItem o in orderDvM.orderitem)
            {
                total += Convert.ToDecimal(o.Product.Price) * o.Quantity;
            }
            orderDvM.totalPrice = total;
        }

        public void GetVendors(OrderDetailsViewModel orderDvM)
        {
            foreach (OrderItem o in orderDvM.orderitem)
            {
                orderDvM.vendor = (from s in db.Vendors where s.VendorID == o.VendorID select s).ToList();
            }
        }

        public void GetCurrentProducts(OrderDetailsViewModel orderDvM)
        {
            foreach (OrderItem o in orderDvM.orderitem)
            {
                orderDvM.product = (from s in db.Products where s.ItemNumber == o.ItemNumber select s).ToList();
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reorder(int? id)
        {

            Order order = db.Orders.Where(o => o.OrderNumber == id).FirstOrDefault();
            Order reorder = new Order();
            reorder.AspNetUser = order.AspNetUser;
            reorder.IsActive = "p";
            reorder.Last_Active = DateTime.Now;
            reorder.Placed = DateTime.Now;
            reorder.UserId = order.UserId;

            //db.Orders.Add(reorder);
            //db.SaveChanges();
            foreach (OrderItem item in order.OrderItems)
            {
                var oi = new OrderItem
                {
                    ItemNumber = item.ItemNumber,
                    Quantity = item.Quantity,
                    VendorID = item.VendorID,
                    OrderNumber = reorder.OrderNumber
                };
               
                reorder.OrderItems.Add(oi);
            }
            db.Orders.Add(reorder);
            // db.Orders.Add(reorder);
            db.SaveChanges();

            return RedirectToAction("CompletedOrders", "OrderApproval");
            //return order;
        }
    }
}