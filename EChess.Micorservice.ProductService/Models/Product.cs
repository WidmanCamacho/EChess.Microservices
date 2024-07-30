using System.ComponentModel.DataAnnotations.Schema;

namespace EChess.Micorservice.ProductService.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; } 
        public User User { get; set; }
    }
}
