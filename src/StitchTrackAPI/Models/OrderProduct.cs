using System.ComponentModel.DataAnnotations;

namespace CrochetBusinessAPI.Models
{
    public class OrderProduct
    {
        [Key]
        [Required]
        public int OrderProductID { get; set; }  //PK
        [Required]
        public int OrderID { get; set; }  //FK
        public Order Order { get; set; } = null!;
        [Required]
        public int FinishedProductsID { get; set; }  //FK
        public FinishedProduct FinishedProduct { get; set; } = null!;
        public int? Quantity { get; set; }
    }
}