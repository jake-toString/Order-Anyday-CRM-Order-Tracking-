namespace OrderAnydayProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vendor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vendor()
        {
            OrderItems = new HashSet<OrderItem>();
            Products = new HashSet<Product>();
        }

        [Display(Name = "Vendor ID")]
        public int VendorID { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Vendor Name")]
        public string Vendor_Name { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(13)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Website { get; set; }

        [StringLength(255)]
        [Display(Name = "Account Manager")]
        public string Account_Manager { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        public bool Inactive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
