using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
    public class OrderApprovalController : Controller
    {
        private OrderAnyDayContext db = new OrderAnyDayContext();
        private ApplicationDbContext appDB = new ApplicationDbContext();

        //Edit
        public ActionResult ApproveOrder(int id)
        {
            OrderApprovalViewModel ApproveOrderVM = new OrderApprovalViewModel();
            ApproveOrderVM.orderVM.GetOrder(id);
            int result = UpdateStatus(id, "a");
            //db.SaveChanges();
            //ApproveOrderVM.orders = GetPendingOrders();
            ApproveOrderVM.user = GetCurrentUser();
            ApproveOrderVM.fullname = ApproveOrderVM.user.FirstName + " " + ApproveOrderVM.user.LastName;
            return View(ApproveOrderVM);
        }

        //Edit
        public ActionResult CancelOrder(int id)
        {
            OrderApprovalViewModel ApproveOrderVM = new OrderApprovalViewModel();
            ApproveOrderVM.orderVM.GetOrder(id);
            int result = UpdateStatus(id, "c");
            //db.SaveChanges();
            //ApproveOrderVM.orders = GetPendingOrders();
            ApproveOrderVM.user = GetCurrentUser();
            ApproveOrderVM.fullname = ApproveOrderVM.user.FirstName + " " + ApproveOrderVM.user.LastName;
            return View(ApproveOrderVM);
        }


        //Edit
        public ActionResult DeclineOrder(int id)
        {
            OrderApprovalViewModel ApproveOrderVM = new OrderApprovalViewModel();
            ApproveOrderVM.orderVM.GetOrder(id);
            int result = UpdateStatus(id, "d");
            //db.SaveChanges();
            //ApproveOrderVM.orders = GetPendingOrders();
            ApproveOrderVM.user = GetCurrentUser();
            ApproveOrderVM.fullname = ApproveOrderVM.user.FirstName + " " + ApproveOrderVM.user.LastName;
            return View(ApproveOrderVM);
        }

        public Order GetOrder(int? id)
        {
            Order order = new Order();
            order = db.Orders.Where(o => o.OrderNumber == id).FirstOrDefault();
            //decimal total = 0;
            //foreach (OrderItem item in order.OrderItems)
            //{
            //    total += Convert.ToDecimal(item.Product.Price) * item.Quantity;
            //}
            //orderTotal = total.ToString();
            //GetUser(order);
            return order;
        }

        //[ActionName("PendingOrders")]
        //public int UpdateStaus(int? id, string status)
        //{
        //    OrderApprovalViewModel ApproveOrderVM = new OrderApprovalViewModel();
        //    int result = UpdateStatus(id, status);
        //    return result;
        //}

        public int SendNotification(int? id, string status)
        {
            //db = new OrderAnyDayContext();
            OrderApprovalViewModel ApproveOrderVM = new OrderApprovalViewModel();
            ApproveOrderVM.order = new Order();
            ApproveOrderVM.order = GetOrder(id);
            ApproveOrderVM.user = (from s in db.AspNetUsers where s.Id == ApproveOrderVM.order.UserId select s).FirstOrDefault();

            //Check for and delete any notification related to this order. 
            //  Unfortunately, our schema will not allow more than one notification per order.
            int notecount = (from s in db.UserNotifications where s.UserId == ApproveOrderVM.order.UserId & s.OrderNumber == ApproveOrderVM.order.OrderNumber select s).Count();
            if(notecount > 0)
            {
                UserNotification n = (from un in db.UserNotifications where un.OrderNumber == ApproveOrderVM.order.OrderNumber & un.UserId == ApproveOrderVM.order.UserId select un).FirstOrDefault();
                db.UserNotifications.Remove(n);
                db.SaveChanges();               
            }
            UserNotification note = new UserNotification
            {
                AspNetUser = ApproveOrderVM.user,
                Order = ApproveOrderVM.order,                
                Notes = "You're order status has been updated. "
            };

            switch (status)
            {
                case "a":
                    note.Notes += "The new status is APPROVED";
                    note.NotificationID = 2;
                    break;
                case "c":
                    note.Notes += "The new status is CANCELLED";
                    note.NotificationID = 3;
                    break;
                case "d":
                    note.Notes += "The new status is DECLINED";
                    note.NotificationID = 1;
                    break;
                case "co":
                    note.Notes += "The New status is Completed";
                    note.NotificationID = 4;
                    break;
                default:
                    return 0;
            }
            
            db.UserNotifications.Add(note);

            return db.SaveChanges();
        }

        public ActionResult PendingOrders(int? id, string status)
        {
            OrderApprovalViewModel ApproveOrderVM = new OrderApprovalViewModel();
            //ApproveOrderVM.orderVM = new OrderViewModel();
            if (!(id == null))
            {
                ApproveOrderVM.order = GetOrder(id);
                int saved = UpdateStatus(id, status);
                if (saved > 0) SendNotification(id, status);
            }
            
            GetPendingOrders(ApproveOrderVM);
            //ApproveOrderVM.user = GetCurrentUser();
            if (status == "co")
            {
                return RedirectToAction("ApprovedOrders", ApproveOrderVM);
            }
            return View(ApproveOrderVM);
        }

        public ActionResult ApprovedOrders(int? id, string status)
        {
            OrderApprovalViewModel ApproveOrderVM = new OrderApprovalViewModel();
            
         /*   if (!(id == null))
            {
                ApproveOrderVM.order = GetOrder(id);
                int saved = UpdateStatus(id, status);
                if (saved > 0) SendNotification(id, status);
            }
        edit this in if needed for archiving maybe?*/
            GetApprovedOrders(ApproveOrderVM);
            
            return View(ApproveOrderVM);
        }

        public ActionResult DeclinedOrders(int? id, string status)
        {
            OrderApprovalViewModel ApproveOrderVM = new OrderApprovalViewModel();

            /*   if (!(id == null))
               {
                   ApproveOrderVM.order = GetOrder(id);
                   int saved = UpdateStatus(id, status);
                   if (saved > 0) SendNotification(id, status);
               }
           edit this in if needed for archiving maybe?*/
            GetDeclinedOrders(ApproveOrderVM);

            return View(ApproveOrderVM);
        }

        public void GetApprovedOrders(OrderApprovalViewModel ApproveOrderVM)
        {
            List<Order> orderList = new List<Order>();
            ApproveOrderVM.ordersVM = new List<OrderViewModel>();
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("Admin"))
            {
                orderList = (from o in db.Orders where o.IsActive == "a" select o).ToList();
            }
            else
            {
                orderList = (from o in db.Orders where o.IsActive == "a" && o.UserId == uid select o).ToList();
            }
            foreach (Order o in orderList)
            {
                OrderViewModel ovm = new OrderViewModel();
                ovm.GetOrder(o.OrderNumber);
                ApproveOrderVM.ordersVM.Add(ovm);
            }
            //return orders;
        }

        public ActionResult CompletedOrders(int? id, string status)
        {
            OrderApprovalViewModel ApproveOrderVM = new OrderApprovalViewModel();

               if (!(id == null) && status == "p") 
               {
                   ApproveOrderVM.order = GetOrder(id);
                   int saved = UpdateStatus(id, status);
                   if (saved > 0) SendNotification(id, status);
               }
          
            GetCompletedOrders(ApproveOrderVM);

            return View(ApproveOrderVM);
        }

        public void GetCompletedOrders(OrderApprovalViewModel ApproveOrderVM)
        {
            List<Order> orderList = new List<Order>();
            ApproveOrderVM.ordersVM = new List<OrderViewModel>();
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("Admin"))
            {
                orderList = (from o in db.Orders where o.IsActive == "co" select o).ToList();
            }
            else
            {
                orderList = (from o in db.Orders where o.IsActive == "co" && o.UserId == uid select o).ToList();
            }
            foreach (Order o in orderList)
            {
                OrderViewModel ovm = new OrderViewModel();
                ovm.GetOrder(o.OrderNumber);
                ApproveOrderVM.ordersVM.Add(ovm);
            }
            //return orders;
        }

        public void GetDeclinedOrders(OrderApprovalViewModel ApproveOrderVM)
        {
            List<Order> orderList = new List<Order>();
            ApproveOrderVM.ordersVM = new List<OrderViewModel>();
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("Admin"))
            {
                orderList = (from o in db.Orders where o.IsActive == "d" select o).ToList();
            }
            else
            {
                orderList = (from o in db.Orders where o.IsActive == "d" && o.UserId == uid select o).ToList();
            }
            foreach (Order o in orderList)
            {
                OrderViewModel ovm = new OrderViewModel();
                ovm.GetOrder(o.OrderNumber);
                ApproveOrderVM.ordersVM.Add(ovm);
            }
            //return orders;
        }

        //[ActionName("PendingOrders")]
        public int UpdateStatus(int? id, string status)
        {
            OrderApprovalViewModel ApproveOrderVM = new OrderApprovalViewModel();
            ApproveOrderVM.order = (from o in db.Orders where o.OrderNumber == id select o).FirstOrDefault();
            ApproveOrderVM.order.IsActive = status;
            return db.SaveChanges();
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            orders = db.Orders.ToList();            
            return orders;
        }

        public void GetPendingOrders(OrderApprovalViewModel ApproveOrderVM)
        {
            List<Order> orderList = new List<Order>();
            ApproveOrderVM.ordersVM = new List<OrderViewModel>();
            string uid = User.Identity.GetUserId();
            if (User.IsInRole("Admin"))
            {
                orderList = (from o in db.Orders where o.IsActive == "p" select o).ToList();
            }
            else
            {
                orderList = (from o in db.Orders where o.IsActive == "p" && o.UserId == uid select o).ToList();
            }
            foreach (Order o in orderList)
            {
                OrderViewModel ovm = new OrderViewModel();
                ovm.GetOrder(o.OrderNumber);
                ApproveOrderVM.ordersVM.Add(ovm);
            }          
            //return orders;
        }

        public List<Order> Details()
        {
            List<Order> orders = new List<Order>();
            orders = db.Orders.ToList();

            return orders;
        }

        public AspNetUser GetOrderUser(Order order)
        {
            var user = (from u in db.AspNetUsers where u.Id == order.UserId select u).FirstOrDefault();
            return (AspNetUser)user;
        }

        public AspNetUser GetCurrentUser()
        {
            string uid = User.Identity.GetUserId();
            var user = (from s in db.AspNetUsers where s.Id == uid select s).FirstOrDefault();
            return (AspNetUser)user;
        } 


        
    public Boolean isAdmin()
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var role = UserManager.GetRoles(user.GetUserId());
            if (role[0].ToString() == "Admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    // GET: OrderApproval
    public ActionResult Index()
        {
            return View();
        }
    }
}