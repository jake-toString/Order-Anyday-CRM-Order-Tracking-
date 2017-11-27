using OrderAnydayProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderAnydayProject.ViewModels
{
    public class OrderApprovalViewModel
    {
        public OrderApproval orderApproval { get; set; }

        public AspNetUser user { get; set; }

        public string fullname { get; set; }

        public Order order { get; set; }

        public OrderViewModel orderVM { get; set; }

        public List<OrderViewModel> ordersVM { get; set; }

        public Notification note { get; set; }
    }
}