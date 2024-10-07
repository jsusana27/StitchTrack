using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetBusinessAPI.Models
{
    public class Order
    {
        [Key]
        [Required]
        public int OrderID { get; set; }  //PK
        [Required]
        public int CustomerID { get; set; }  //FK
        public DateTime? OrderDate { get; set; } = DateTime.Now;  //Default to current timestamp
        public string FormOfPayment { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalPrice { get; set; }

        public Customer Customer { get; set; }  = null!;  //Navigation Property
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}