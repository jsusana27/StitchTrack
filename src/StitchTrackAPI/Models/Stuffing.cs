using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetBusinessAPI.Models
{
    public class Stuffing
    {
        [Key]
        [Required]
        public int StuffingID { get; set; }  //PK
        public string Brand { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PricePerFivelbs { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}