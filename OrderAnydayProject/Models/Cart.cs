using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrderAnydayProject.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string CartId { get; set; }
        public int ItemNumber { get; set; }
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }

        public virtual Product Product { get; set; }
    }
}