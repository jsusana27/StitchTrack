using System.ComponentModel.DataAnnotations;

namespace CrochetBusinessAPI.Models
{
    public class Customer
    {
        [Key]
        public int? CustomerID { get; set; }  //PK
        public string Name { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }

        public ICollection<CustomerPurchase> CustomerPurchases { get; set; } = new List<CustomerPurchase>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}