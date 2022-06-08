using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPIDemo.Models
{
    public class Stock
    {
        [Key]
        public string StockCode { get; set; }
        [Required]
        public string StockName { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        [Required]
        public decimal KDV { get; set; }
    }
}
