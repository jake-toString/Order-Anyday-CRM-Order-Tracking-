using OrderAnydayProject.ViewModels;
using System;

using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using OrderAnydayProject.Models;

namespace OrderAnydayProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private OrderAnyDayContext db = new OrderAnyDayContext();
        private ApplicationDbContext context = new ApplicationDbContext();
        public new ActionResult Profile()
        {
            //TODO: Update these for admin
            if (User.IsInRole("Admin"))
            {
                var cart = ShoppingCart.GetCart(HttpContext);
                UserDashboardViewModel UserProfileVM = new UserDashboardViewModel();
                string uid = User.Identity.GetUserId();
                UserProfileVM.user = (from s in db.AspNetUsers where s.Id == uid select s).FirstOrDefault();
                UserProfileVM.orders = (from o in db.Orders where o.UserId == uid select o).ToList();
                UserProfileVM.latestOrders = (from o in db.Orders select o).Take(10).OrderByDescending(c => c.Last_Active).ToList();
                UserProfileVM.latestProducts = (from p in db.Products select p).Take(10).OrderByDescending(c => c.Date_Added).ToList();
                UserProfileVM.notification = (from n in db.UserNotifications where n.UserId == uid select n).ToList();
                UserProfileVM.approvedOrdersCount = (from o in db.Orders where o.IsActive == "a" select o).Count().ToString();
                UserProfileVM.completedOrdersCount = (from o in db.Orders where o.IsActive == "co" select o).Count().ToString();
                UserProfileVM.pendingOrdersCount = (from o in db.Orders where o.IsActive == "p" select o).Count().ToString();
                UserProfileVM.ordersCount = (from o in db.Orders select o).Count().ToString();
                UserProfileVM.declinedOrdersCount = (from o in db.Orders select o).Count().ToString();
                UserProfileVM.notificationCount = (from n in db.UserNotifications where n.UserId == uid select n).Count().ToString();
                UserProfileVM.userCount = (from u in db.AspNetUsers select u).Count().ToString();
                UserProfileVM.discontinuedProductCount = (from dp in db.Products where dp.Discontinued == true select dp).Count().ToString();
                UserProfileVM.inactiveVendorCount = (from iv in db.Vendors where iv.Inactive == true select iv).Count().ToString();

                UserProfileVM.productCats = (from p in db.Categories select p.Name).Distinct().ToList();
                UserProfileVM.productCatsCount = new List<int>();
                for (int i = 0; i < UserProfileVM.productCats.Count; i++)
                {
                    UserProfileVM.productCatsCount.Add(0);
                }

                UserProfileVM.Year = DateTime.Now.Year;
                DateTime start = new DateTime(UserProfileVM.Year, 01, 01);
                DateTime end = new DateTime(UserProfileVM.Year, 02, 01);
                var janTotals = (from o in db.Orders where (o.IsActive == "a" || o.IsActive == "co") && o.Placed >= start && o.Placed < end select o).ToList();
                UserProfileVM.marTotal = 0;
                foreach (Order total in janTotals)
                {
                    OrderDetailsViewModel odvm = new OrderDetailsViewModel();
                    odvm.orders = total;
                    odvm = GetOrderItems(odvm);
                    foreach (OrderItem item in odvm.orderitem)
                    {
                        for (int i = 0; i < UserProfileVM.productCats.Count; i++)
                        {
                            if (item.Product.Category == UserProfileVM.productCats[i])
                                UserProfileVM.productCatsCount[i] += item.Quantity;
                        }
                        
                    }
                        
                    UserProfileVM.janTotal += odvm.totalPrice;
                }
                start = new DateTime(UserProfileVM.Year, 02, 01);
                end = new DateTime(UserProfileVM.Year, 03, 01);
                var febTotals = (from o in db.Orders where (o.IsActive == "a" || o.IsActive == "co") && o.Placed >= start && o.Placed < end select o).ToList();
                UserProfileVM.febTotal = 0;
                foreach (Order total in febTotals)
                {
                    OrderDetailsViewModel odvm = new OrderDetailsViewModel();
                    odvm.orders = total;
                    odvm = GetOrderItems(odvm);
                    foreach (OrderItem item in odvm.orderitem)
                    {
                        for (int i = 0; i < UserProfileVM.productCats.Count; i++)
                        {
                            if (item.Product.Category == UserProfileVM.productCats[i])
                                UserProfileVM.productCatsCount[i] += item.Quantity;
                        }

                    }
                    UserProfileVM.febTotal += odvm.totalPrice;
                }

                start = new DateTime(UserProfileVM.Year, 03,01);
                end = new DateTime(UserProfileVM.Year, 04, 01);
                var marchTotals = (from o in db.Orders where (o.IsActive == "a" || o.IsActive == "co") && o.Placed >= start && o.Placed < end select o).ToList();
                UserProfileVM.marTotal = 0;                
                foreach (Order total in marchTotals)
                {
                    OrderDetailsViewModel odvm = new OrderDetailsViewModel();
                    odvm.orders = total;
                    odvm = GetOrderItems(odvm);
                    foreach (OrderItem item in odvm.orderitem)
                    {
                        for (int i = 0; i < UserProfileVM.productCats.Count; i++)
                        {
                            if (item.Product.Category == UserProfileVM.productCats[i])
                                UserProfileVM.productCatsCount[i] += item.Quantity;
                        }

                    }
                    UserProfileVM.marTotal += odvm.totalPrice;
                }

                start = new DateTime(UserProfileVM.Year, 04, 01);
                end = new DateTime(UserProfileVM.Year, 05, 01);
                var aprilTotals = (from o in db.Orders where (o.IsActive == "a" || o.IsActive == "co") && o.Placed >= start && o.Placed < end select o).ToList();
                UserProfileVM.aprTotal = 0;
                foreach (Order total in aprilTotals)
                {
                    OrderDetailsViewModel odvm = new OrderDetailsViewModel();
                    odvm.orders = total;
                    odvm = GetOrderItems(odvm);
                    foreach (OrderItem item in odvm.orderitem)
                    {
                        for (int i = 0; i < UserProfileVM.productCats.Count; i++)
                        {
                            if (item.Product.Category == UserProfileVM.productCats[i])
                                UserProfileVM.productCatsCount[i] += item.Quantity;
                        }

                    }
                    UserProfileVM.aprTotal += odvm.totalPrice;
                }

                start = new DateTime(UserProfileVM.Year, 05, 01);
                end = new DateTime(UserProfileVM.Year, 06, 01);
                var mayTotals = (from o in db.Orders where (o.IsActive == "a" || o.IsActive == "co") && o.Placed >= start && o.Placed < end select o).ToList();
                UserProfileVM.mayTotal = 0;
                foreach (Order total in mayTotals)
                {
                    OrderDetailsViewModel odvm = new OrderDetailsViewModel();
                    odvm.orders = total;
                    odvm = GetOrderItems(odvm);
                    foreach (OrderItem item in odvm.orderitem)
                    {
                        for (int i = 0; i < UserProfileVM.productCats.Count; i++)
                        {
                            if (item.Product.Category == UserProfileVM.productCats[i])
                                UserProfileVM.productCatsCount[i] += item.Quantity;
                        }

                    }
                    UserProfileVM.mayTotal += odvm.totalPrice;
                }

                UserProfileVM.cartCount = cart.GetCount().ToString();
                if (UserProfileVM.cartCount == "0") UserProfileVM.cartCount = "Empty";
                return View(UserProfileVM);
            }
            else
            {
                var cart = ShoppingCart.GetCart(HttpContext);
                UserDashboardViewModel UserProfileVM = new UserDashboardViewModel();
                string uid = User.Identity.GetUserId();
                UserProfileVM.user = (from s in db.AspNetUsers where s.Id == uid select s).FirstOrDefault();
                UserProfileVM.orders = (from o in db.Orders where o.UserId == uid select o).ToList();
                UserProfileVM.notification = (from n in db.UserNotifications where n.UserId == uid select n).ToList();
                UserProfileVM.approvedOrdersCount = (from o in db.Orders where o.IsActive == "a" && o.AspNetUser.Id == uid select o).Count().ToString();
                UserProfileVM.completedOrdersCount = (from o in db.Orders where o.IsActive == "co" && o.AspNetUser.Id == uid select o).Count().ToString();
                UserProfileVM.pendingOrdersCount = (from o in db.Orders where o.IsActive == "p" && o.AspNetUser.Id == uid select o).Count().ToString();
                UserProfileVM.ordersCount = (from o in db.Orders where o.UserId == uid select o).Count().ToString();
                UserProfileVM.declinedOrdersCount = (from o in db.Orders where o.IsActive == "d" && o.UserId == uid select o).Count().ToString();
                UserProfileVM.notificationCount = (from n in db.UserNotifications where n.UserId == uid select n).Count().ToString();
                UserProfileVM.userCount = (from u in db.AspNetUsers select u).Count().ToString();
                UserProfileVM.cartCount = cart.GetCount().ToString();
                if (UserProfileVM.cartCount == "0") UserProfileVM.cartCount = "Empty";
                return View(UserProfileVM);
            }
                
        }

        public OrderDetailsViewModel GetOrderItems(OrderDetailsViewModel orderDvM)
        {
            orderDvM.orderitem = (from s in db.OrderItems where s.OrderNumber == orderDvM.orders.OrderNumber select s).ToList();
            decimal total = 0;
            foreach (OrderItem o in orderDvM.orderitem)
            {
                total += Convert.ToDecimal(o.Product.Price) * o.Quantity;
            }
            orderDvM.totalPrice = total;
            return orderDvM;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cancel(int id)
        {
            OrderAnyDayContext db = new OrderAnyDayContext();
            var query = from Ord in db.Orders where Ord.OrderNumber == id select Ord;

            foreach (Order Ord in query)
            {
                Ord.IsActive = "c";
                Ord.Last_Active = DateTime.Now;
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Profile");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}