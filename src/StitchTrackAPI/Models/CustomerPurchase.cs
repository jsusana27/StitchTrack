using System.ComponentModel.DataAnnotations;

namespace CrochetBusinessAPI.Models
{
    public class CustomerPurchase
    {
        [Key]
        [Required]
        public int CustomerPurchaseID { get; set; }  // PK
        [Required]
        public int CustomerID { get; set; }  // FK
        [Required]
        public int FinishedProductsID { get; set; }  // FK

        // Navigation Properties
        public Customer Customer { get; set; } = null!;  // Navigation Property
        public FinishedProduct FinishedProduct { get; set; } = null!;  // Navigation Property
    }
}