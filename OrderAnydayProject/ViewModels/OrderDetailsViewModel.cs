using OrderAnydayProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderAnydayProject.ViewModels
{
    public class OrderDetailsViewModel
    {
        private OrderAnyDayContext db = new OrderAnyDayContext();

        public string FullName { get; set; }

        public decimal totalPrice { get; set; }

        public AspNetUser user { get; set; }

        public Order orders { get; set; }

        public List<Vendor> vendor { get; set; }

        public List<OrderItem> orderitem { get; set; }
        
        public List<Product> product { get; set; }
    }
}