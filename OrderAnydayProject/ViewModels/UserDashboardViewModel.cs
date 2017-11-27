using OrderAnydayProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderAnydayProject.ViewModels
{
    public class UserDashboardViewModel
    {
        public string productCount { get; set; }

        public string discontinuedProductCount { get; set; }

        public List<string> productCats { get; set; }
        public List<int> productCatsCount { get; set; }

        public string monthtotal { get; set; }

        public string userCount { get; set; }

        public string vendorCount { get; set; }

        public string inactiveVendorCount { get; set; }

        public string pendingOrdersCount { get; set; }

        public string approvedOrdersCount { get; set; }

        public string completedOrdersCount { get; set; }

        public string declinedOrdersCount { get; set; }

        public string ordersCount { get; set; }

        public string notificationCount { get; set; }

        public string cartCount { get; set; }

        public AspNetUser user { get; set; }

        public List<Order> orders { get; set; }
        public List<Order> latestOrders { get; set; }
        public List<Product> latestProducts { get; set; }
        public List<UserNotification> notification { get; set; }

        public List<Order> janTotals { get; set; }

        public decimal janTotal { get; set; }
        public decimal febTotal { get; set; }
        public decimal marTotal { get; set; }
        public decimal aprTotal { get; set; }
        public decimal mayTotal { get; set; }

        public int Year { get; set; }
    }
}