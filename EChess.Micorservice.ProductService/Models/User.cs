namespace EChess.Micorservice.ProductService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
