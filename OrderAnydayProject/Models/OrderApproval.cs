using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrderAnydayProject.Models
{
    public class OrderApproval
    {
        [Key]
        public int Id { get; set; }
        //public string fullname { get; set; }

        public virtual Order order { get; set; }
        public virtual AspNetUser user { get; set; }
    }
}