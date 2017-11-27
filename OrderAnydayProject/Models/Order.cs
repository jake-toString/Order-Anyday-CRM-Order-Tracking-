namespace OrderAnydayProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
            UserNotifications = new HashSet<UserNotification>();
        }

        [Key]
        [Display(Name = "Order Number")]
        public int OrderNumber { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date Placed")]
        public DateTime Placed { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime? Last_Active { get; set; }

        [StringLength(3)]
        [Display(Name = "Status")]
        public string IsActive { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserNotification> UserNotifications { get; set; }
    }
}
