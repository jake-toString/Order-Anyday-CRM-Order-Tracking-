using OrderAnydayProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OrderAnydayProject.ViewModels
{
    public class OrderViewModel
    {
        private OrderAnyDayContext db = new OrderAnyDayContext();

        [Display(Name = "Order Total")]
        public string orderTotal { get; set; }

        public AspNetUser user { get; set; }

        public string fullname { get; set; }

        public Order order { get; set; }

        public void GetOrder(int? id)
        {
            order = new Order();
            order = db.Orders.Where(o => o.OrderNumber == id).FirstOrDefault();
            decimal total = 0;
            foreach(OrderItem item in order.OrderItems)
            {
                total += Convert.ToDecimal(item.Product.Price) * item.Quantity;
            }
            orderTotal = total.ToString();
            GetUser(order);
        }

        public void GetUser(Order order)
        {
            user = new AspNetUser();
            user = db.AspNetUsers.Where(u => u.Id == order.UserId).FirstOrDefault();
            fullname = user.FirstName + " " + user.LastName;            
        }
        
    }
}