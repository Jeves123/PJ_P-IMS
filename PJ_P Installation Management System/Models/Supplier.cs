using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJ_P_Installation_Management_System.Models
{
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [StringLength(50, ErrorMessage = "Contact person name cannot exceed 50 characters")]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Display(Name = "Active Supplier")]
        public bool IsActive { get; set; } = true;

        // Navigation property
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}