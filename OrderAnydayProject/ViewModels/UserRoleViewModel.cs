using OrderAnydayProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrderAnydayProject.ViewModels
{
    public class UserRoleViewModel
    {
        public UserRoleViewModel()
        {
            AspNetRoles = new HashSet<AspNetRole>();
            //AspNetUserClaims = new HashSet<AspNetUserClaim>();
            //AspNetUserLogins = new HashSet<AspNetUserLogin>();
        }

        //public virtual ICollection<AspNetRole> AspNetRoles { get; set; }

        public AspNetUser aspNetUser { get; set; }

        public string UserRole { get; set; }
        ////public class EditAspNetUserViewModel
        ////{
        //[Required]
        //    [Display(Name = "UserRoles")]
        public string UserRoles { get; set; }

        //    public string Id { get; set; }

        //    [Display(Name = "First Name")]
        //    public string FirstName { get; set; }

        //    [Display(Name = "Last Name")]
        //    public string LastName { get; set; }

        //    [DataType(DataType.Date)]
        //    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //    public DateTime Birthdate { get; set; }

        public string Department { get; set; }

        //    [StringLength(256)]
        //    public string Email { get; set; }

        //    public bool EmailConfirmed { get; set; }

        //    public string PasswordHash { get; set; }

        //    public string SecurityStamp { get; set; }

        //    [Display(Name = "Phone Number")]
        //    public string PhoneNumber { get; set; }

        //    public bool PhoneNumberConfirmed { get; set; }

        //    public bool TwoFactorEnabled { get; set; }

        //    public DateTime? LockoutEndDateUtc { get; set; }

        //    public bool LockoutEnabled { get; set; }

        //    public int AccessFailedCount { get; set; }

        //    [Required]
        //    [StringLength(256)]
        //    [Display(Name = "Username")]
        //    public string UserName { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Order> Orders { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<UserNotification> UserNotifications { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
        ////}

    }
}