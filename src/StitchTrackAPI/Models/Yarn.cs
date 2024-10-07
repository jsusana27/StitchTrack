using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrochetBusinessAPI.Models
{
    public class Yarn
    {
        [Key]
        public int? YarnID { get; set; }  //PK
        public string Brand { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
        public string FiberType { get; set; } = string.Empty; 
        public int? FiberWeight { get; set; }
        public string Color { get; set; } = string.Empty; 

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? YardagePerSkein { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? GramsPerSkein { get; set; }
        public int? NumberOfSkeinsOwned { get; set; }
    }
}