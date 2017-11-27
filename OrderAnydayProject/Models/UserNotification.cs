namespace OrderAnydayProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserNotification
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Order Number")]
        public int OrderNumber { get; set; }

        [Display(Name = "Notification ID")]
        public int NotificationID { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        [Key]
        [Column(Order = 1)]
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual Notification Notification { get; set; }

        public virtual Order Order { get; set; }
    }
}
