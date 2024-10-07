using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetBusinessAPI.Models
{
    public class FinishedProduct
    {
        [Key]
        public int? FinishedProductsID { get; set; }  //PK
        public string Name { get; set; } = string.Empty;
        public TimeSpan? TimeToMake { get; set; }  //INTERVAL maps to TimeSpan in C#

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalCostToMake { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SalePrice { get; set; }
        public int? NumberInStock { get; set; }

        public ICollection<FinishedProductMaterial> FinishedProductMaterials { get; set; } = new List<FinishedProductMaterial>();
        public ICollection<CustomerPurchase> CustomerPurchases { get; set; } = new List<CustomerPurchase>();
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}