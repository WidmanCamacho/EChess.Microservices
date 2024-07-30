using EChess.Microservice.UserService.Models;
using Microsoft.EntityFrameworkCore;

namespace EChess.Microservice.UserService.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await Users.FindAsync(id) ?? new User();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await Users.SingleOrDefaultAsync(u => u.Username == username) ?? new User();
        }

    }
}
