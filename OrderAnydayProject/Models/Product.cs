namespace OrderAnydayProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        [Key]
        [Display(Name = "Item Number")]
        public int ItemNumber { get; set; }

        [Display(Name = "Vendor ID")]
        public int VendorID { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Required]
        [StringLength(255)]
        public string Brand { get; set; }
        
        [StringLength(255)]
        [Display(Name = "Keyword")]
        public string Type { get; set; }

        [StringLength(255)]
        public string Category { get; set; }

        [Required]
        [StringLength(255)]
        public string Price { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Added")]
        public DateTime Date_Added { get; set; }

        public bool Discontinued { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual Vendor Vendor { get; set; }
    }
}
