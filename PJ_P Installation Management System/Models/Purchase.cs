using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PJ_P_Installation_Management_System.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public string Status { get; set; }

        public Product Product { get; set; }
        public Supplier Supplier { get; set; }
    }
}