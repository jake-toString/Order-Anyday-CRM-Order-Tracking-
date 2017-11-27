using OrderAnydayProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderAnydayProject.ViewModels
{
    public class UserProfileViewModel
    {
        public AspNetUser user { get; set; }

        public List<Order> orders { get; set; }


    }
}