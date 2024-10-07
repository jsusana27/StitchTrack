using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetBusinessAPI.Models
{
    public class FinishedProductMaterial
    {
        [Key]
        [Required]
        public int FinishedProductMaterialsID { get; set; }  //PK
        [Required]
        public int FinishedProductsID { get; set; }  //FK
        public string MaterialType { get; set; } = string.Empty;
        [Required]
        public int MaterialID { get; set; }  //Could represent Yarn, SafetyEyes, Stuffing, etc.
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? QuantityUsed { get; set; }

        public FinishedProduct FinishedProduct { get; set; }  = null!;  //Navigation Property
    }
}