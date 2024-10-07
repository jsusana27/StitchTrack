using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetBusinessAPI.Models
{
    public class SafetyEye
    {
        [Key]
        public int? SafetyEyesID { get; set; }  //PK

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
        public int? SizeInMM { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Shape { get; set; } = string.Empty;
        public int? NumberOfEyesOwned { get; set; }
    }
}